using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Events;
using Newtonsoft.Json;

namespace InMemoryFakes
{
    public class EventStore : IEventStore
    {
        private readonly Dictionary<string, Dictionary<Guid, List<EventRecord>>> _fake =
            new Dictionary<string, Dictionary<Guid, List<EventRecord>>>();

        public void SaveEvents(string streamId, Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            if (!_fake.TryGetValue(streamId, out var aggregates))
            {
                aggregates = new Dictionary<Guid, List<EventRecord>>();
                _fake.Add(streamId, aggregates);
            }

            if (!aggregates.TryGetValue(aggregateId, out var eventRecords))
            {
                eventRecords = new List<EventRecord>();
                aggregates.Add(aggregateId, eventRecords);
            }
            else if (eventRecords[eventRecords.Count - 1].Version != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            var i = expectedVersion;

            foreach (var @event in events)
            {
                i++;
                @event.Version = i;

                eventRecords.Add(new EventRecord(aggregateId, i,
                    JsonConvert.SerializeObject(@event,
                        new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects})));

                //_publisher.Publish(@event);
            }
        }

        public List<Event> GetEventsForAggregate(string streamId, Guid aggregateId)
        {
            if (!_fake.TryGetValue(streamId, out var aggregates) ||
                !aggregates.TryGetValue(aggregateId, out var eventRecords))
            {
                throw new AggregateNotFoundException();
            }

            return eventRecords.Select(x => JsonConvert.DeserializeObject<Event>(x.EventData,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects}))
                .ToList();
        }
    }
}