using System;
using System.Collections.Generic;
using Domain;
using Domain.Events;
using InMemoryFakes;
using Xunit;

namespace Tests
{
    public class EventStoreTests
    {
        [Fact]
        public void Foo()
        {
            var events = new List<Event>
                         {
                             {
                                 new LandlordCreated(
                                     new Guid("00000000-0000-0000-0000-000000000001"), "Bob")
                             },
                             {
                                 new LandlordChangedName(
                                     new Guid("00000000-0000-0000-0000-000000000001"), "Harry")
                             }
                         };
            IEventStore store = new EventStore();
            store.SaveEvents(new Guid("00000000-0000-0000-0000-000000000001"), events, 0);

            var events2 = store.GetEventsForAggregate(new Guid("00000000-0000-0000-0000-000000000001"));

            Assert.Equal(2, events2.Count);
        }
    }
}