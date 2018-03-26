using System;
using System.Collections.Generic;
using Domain;
using Domain.Events;
using Domain.Values;
using InMemoryFakes;
using Xunit;

namespace Tests
{
    public class EventStoreTests
    {
        [Fact]
        public void Foo()
        {
            var address = new Address("Faux House", "Imaginary Street", "Scum on the Wold", "Widgetshire", "AB12 3CD");

            var events = new List<Event>
                         {
                             new LandlordCreated(new Guid("00000000-0000-0000-0000-000000000001"),new Name("Bob", "Rocket"),"bob.rocket@email.com", address),
                             new LandlordChangedName(new Guid("00000000-0000-0000-0000-000000000001"),new Name("Peter", "Crabkin"))
                         };
            IEventStore store = new EventStore();
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000001"),
                events, 0);

            var events2 = store.GetEventsForAggregate("test", new Guid("00000000-0000-0000-0000-000000000001"));

            Assert.Equal(events.Count, events2.Count);
        }
    }
}