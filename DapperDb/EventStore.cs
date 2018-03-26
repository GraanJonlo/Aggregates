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

        public void SaveEvents(string streamId, Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            var updatedVersionRecord = _transaction.Connection.QueryFirst<DbVersion>(
                @"IF NOT EXISTS (SELECT id FROM Versions WHERE streamid=@StreamId AND uuid=@Uuid)
	                INSERT INTO Versions (streamid, uuid, version) VALUES (@StreamId, @Uuid, 0)
                UPDATE Versions SET version = version + 1 WHERE uuid=@Uuid
                SELECT id, streamid, uuid, version FROM Versions WHERE streamid=@StreamId AND uuid=@Uuid",
                param: new {StreamId = streamId, Uuid = aggregateId},
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
                        (streamid, uuid, version, eventdata)
                    VALUES
                        (@StreamId, @Uuid, @Version, @EventData)",
                    param: new
                    {
                        StreamId = streamId,
                        Uuid = aggregateId,
                        Version = i,
                        EventData = JsonConvert.SerializeObject(@event,
                            new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects})
                    },
                    transaction: _transaction);

                //_publisher.Publish(@event);
            }
        }

        public IEnumerable<Event> GetEvents(string streamId, Guid aggregateId)
        {
            return _transaction.Connection.Query<DbEvent>(
                    @"SELECT id,uuid,version,eventdata
                    FROM Events
                    WHERE streamid=@StreamId AND uuid=@Uuid
                    ORDER BY id ASC",
                    param: new {StreamId = streamId, Uuid = aggregateId},
                    transaction: _transaction)
                .Select(x => JsonConvert.DeserializeObject<Event>(x.EventData,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects}));
        }

        public IEnumerable<Event> GetEvents(string streamId, int skip, int take)
        {
            return _transaction.Connection.Query<DbEvent>(
                    @"SELECT id,uuid,version,eventdata
                    FROM Events
                    WHERE streamid=@StreamId
                    ORDER BY id ASC
                    OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY",
                    param: new {StreamId = streamId, Skip = skip, Take = take},
                    transaction: _transaction)
                .Select(x => JsonConvert.DeserializeObject<Event>(x.EventData,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects}));
        }
    }
}