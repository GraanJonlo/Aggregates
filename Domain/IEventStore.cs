using System;
using System.Collections.Generic;
using Domain.Events;

namespace Domain
{
    public interface IEventStore
    {
        void SaveEvents(string streamId, Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        IEnumerable<Event> GetEvents(string streamId, Guid aggregateId);
        IEnumerable<Event> GetEvents(string streamId, int skip, int take);
    }

    public class AggregateNotFoundException : Exception
    {
    }

    public class ConcurrencyException : Exception
    {
    }
}