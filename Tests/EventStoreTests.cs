using System;
using System.Collections.Generic;
using System.Linq;
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
        public void RetrieveEventsForAggregate()
        {
            var address = new Address("Faux House", "Imaginary Street", "Scum on the Wold", "Widgetshire", "AB12 3CD");

            var e1 = new List<Event>
            {
                new LandlordCreated(new Guid("00000000-0000-0000-0000-000000000001"), new Name("Bob", "Rocket"),
                    "bob.rocket@email.com", address)
            };
            var e2 = new List<Event>
            {
                new LandlordCreated(new Guid("00000000-0000-0000-0000-000000000002"), new Name("Bob", "Rocket"),
                    "bob.rocket@email.com", address)
            };
            var e3 = new List<Event>
            {
                new LandlordChangedName(new Guid("00000000-0000-0000-0000-000000000001"), new Name("Peter", "Crabkin"))
            };
            var e4 = new List<Event>
            {
                new LandlordChangedName(new Guid("00000000-0000-0000-0000-000000000002"), new Name("Peter", "Crabkin"))
            };

            IEventStore store = new EventStore();
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000001"),e1, 0);
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000002"),e2, 0);
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000001"),e3, 0);
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000002"),e4, 0);

            var events = store.GetEvents("test", new Guid("00000000-0000-0000-0000-000000000001"));

            Assert.Equal(2, events.Count());
        }

        [Fact]
        public void RetrieveEventsForStream()
        {
            var address = new Address("Faux House", "Imaginary Street", "Scum on the Wold", "Widgetshire", "AB12 3CD");

            var e1 = new List<Event>
            {
                new LandlordCreated(new Guid("00000000-0000-0000-0000-000000000001"), new Name("Bob", "Rocket"),
                    "bob.rocket@email.com", address)
            };
            var e2 = new List<Event>
            {
                new LandlordCreated(new Guid("00000000-0000-0000-0000-000000000002"), new Name("Bob", "Rocket"),
                    "bob.rocket@email.com", address)
            };
            var e3 = new List<Event>
            {
                new LandlordChangedName(new Guid("00000000-0000-0000-0000-000000000001"), new Name("Peter", "Crabkin"))
            };
            var e4 = new List<Event>
            {
                new LandlordChangedName(new Guid("00000000-0000-0000-0000-000000000002"), new Name("Peter", "Crabkin"))
            };

            IEventStore store = new EventStore();
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000001"),e1, 0);
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000002"),e2, 0);
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000001"),e3, 0);
            store.SaveEvents("test", new Guid("00000000-0000-0000-0000-000000000002"),e4, 0);

            var events = store.GetEvents("test", 0, 10);

            Assert.Equal(4, events.Count());
        }
    }
}