using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Events;
using Domain.Values;
using Xunit;

namespace Tests
{
    public class LandlordTests
    {
        [Fact]
        public void WhenChangingLandlordName()
        {
            var address = new Address("Faux House", "Imaginary Street", "Scum on the Wold", "Widgetshire", "AB12 3CD");

            var events = new List<Event>
                         {
                             new LandlordCreated(new Guid("00000000-0000-0000-0000-000000000000"),new Name("Bob", "Rocket"), "bob.rocket@email.com", address)
                         };
            var sut = Landlord.Create(events);

            sut.ChangeName(new Name("Peter", "Crabkin"));

            Assert.Single(sut.GetUncommittedChanges());

            var result =
                (LandlordChangedName) sut.GetUncommittedChanges().First();

            Assert.Equal(new Guid("00000000-0000-0000-0000-000000000000"),
                result.Id);
            Assert.Equal(new Name("Peter", "Crabkin"), result.Name);
        }
    }
}