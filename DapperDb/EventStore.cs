using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Domain;
using Domain.Events;
using Newtonsoft.Json;

namespace DapperDb
{
    public class EventStore : IEventStore
    {
        private readonly IDbTransaction _transaction;

        public EventStore(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            var pastEvents = _transaction.Connection.Query<DbEvent>(
                @"SELECT id,uuid,version,eventdata
                FROM Events
                WHERE uuid=@Uuid
                ORDER BY id DESC",
                param: new { Uuid = aggregateId },
                transaction: _transaction)
                .ToList();

            if (pastEvents.Any() && pastEvents.Last().Version != expectedVersion && expectedVersion != -1)
            {
                throw new ConcurrencyException();
            }

            var i = expectedVersion;

            foreach (var @event in events)
            {
                i++;
                @event.Version = i;

                _transaction.Connection.Execute(
                    @"INSERT INTO Events
                        (uuid, version, eventdata)
                    VALUES
                        (@Uuid, @Version, @EventData)",
                    param: new {Uuid = aggregateId, Version = i, EventData = JsonConvert.SerializeObject(@event,
                                   new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects })
                               },
                    transaction: _transaction);

                //_publisher.Publish(@event);
            }
        }

        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            return _transaction.Connection.Query<DbEvent>(
                    @"SELECT id,uuid,version,eventdata
                    FROM Events
                    WHERE uuid=@Uuid",
                    param: new {Uuid = aggregateId},
                    transaction: _transaction)
                .Select(x => JsonConvert.DeserializeObject<Event>(x.EventData,
                            new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects}))
                .ToList();
        }
    }
}