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
        private readonly Dictionary<Guid, List<EventRecord>> _fake = new Dictionary<Guid, List<EventRecord>>();

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            if (!_fake.TryGetValue(aggregateId, out var eventRecords))
            {
                eventRecords = new List<EventRecord>();
                _fake.Add(aggregateId, eventRecords);
            }
            else if (eventRecords[eventRecords.Count - 1].Version != expectedVersion && expectedVersion != -1)
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
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects })));

                //_publisher.Publish(@event);
            }
        }

        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            if (!_fake.TryGetValue(aggregateId, out var eventRecords))
            {
                throw new AggregateNotFoundException();
            }

            return eventRecords.Select(x => JsonConvert.DeserializeObject<Event>(x.EventData,
                                           new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }))
                .ToList();
        }
    }
}
