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
            var updatedVersionRecord = _transaction.Connection.QueryFirst<DbVersion>(
                @"IF NOT EXISTS (SELECT id FROM Versions WHERE uuid=@Uuid)
	                INSERT INTO Versions (uuid, version) VALUES (@Uuid, 0)
                UPDATE Versions SET version = version + 1 WHERE uuid=@Uuid
                SELECT id, uuid, version FROM Versions WHERE uuid=@Uuid",
                param: new {Uuid = aggregateId},
                transaction: _transaction);

            if (updatedVersionRecord.Version != expectedVersion + 1)
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