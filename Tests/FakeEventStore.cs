using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Events;
using Newtonsoft.Json;

namespace Tests
{
    public class FakeEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;

        private readonly struct EventRecord
        {
            public readonly string StreamId;
            public readonly Guid Uuid;
            public readonly int Version;
            public readonly string EventData;

            public EventRecord(string streamId, Guid uuid, int version, string eventData)
            {
                StreamId = streamId;
                Uuid = uuid;
                Version = version;
                EventData = eventData;
            }
        }

        public FakeEventStore(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        private readonly Dictionary<Guid, int> _versions = new Dictionary<Guid, int>();
        private readonly List<EventRecord> _events = new List<EventRecord>();

        private readonly Dictionary<string, Dictionary<Guid, List<EventRecord>>> _fake =
            new Dictionary<string, Dictionary<Guid, List<EventRecord>>>();

        public void SaveEvents(string streamId, Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            if (!_versions.TryGetValue(aggregateId, out int version))
            {
                version = 0;
                _versions.Add(aggregateId, version);
            }

            if (version != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            var i = expectedVersion;

            foreach (var @event in events)
            {
                i++;
                @event.Version = i;

                _events.Add(
                    new EventRecord(
                        streamId,
                        aggregateId,
                        i,
                        JsonConvert.SerializeObject(
                            @event,
                            new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects})));

                _publisher.Publish(@event);
            }
        }

        public IEnumerable<Event> GetEvents(string streamId, Guid aggregateId)
        {
            if (!_versions.ContainsKey(aggregateId))
            {
                throw new AggregateNotFoundException();
            }

            return _events
                .Where(x => x.StreamId.Equals(streamId, StringComparison.InvariantCultureIgnoreCase) &&
                            x.Uuid == aggregateId)
                .Select(x => JsonConvert.DeserializeObject<Event>(x.EventData,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects}));
        }

        public IEnumerable<Event> GetEvents(string streamId, int skip, int take)
        {
            return _events
                .Where(x => x.StreamId.Equals(streamId, StringComparison.InvariantCultureIgnoreCase))
                .Skip(skip)
                .Take(take)
                .Select(x => JsonConvert.DeserializeObject<Event>(x.EventData,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects}));
        }
    }
}