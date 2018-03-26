using System;
using System.Collections.Generic;
using Domain.Events;

namespace Domain
{
    public interface IEventStore
    {
        void SaveEvents(string streamId, Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        List<Event> GetEventsForAggregate(string streamId, Guid aggregateId);
    }

    public class AggregateNotFoundException : Exception
    {
    }

    public class ConcurrencyException : Exception
    {
    }
}