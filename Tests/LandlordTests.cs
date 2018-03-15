using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Events;
using Xunit;

namespace Tests
{
    public class LandlordTests
    {
        [Fact]
        public void WhenChangingLandlordName()
        {
            var events = new List<Event>
                         {
                             new LandlordCreated(
                                 new Guid("00000000-0000-0000-0000-000000000000"), "Bob")
                         };
            var sut = new Landlord();
            sut.LoadFromHistory(events);

            sut.ChangeName("Harry");

            Assert.Equal(1, sut.GetUncommittedChanges().Count());

            var result = (LandlordChangedName)sut.GetUncommittedChanges().First();

            Assert.Equal(new Guid("00000000-0000-0000-0000-000000000000"), result.Id);
            Assert.Equal("Harry", result.Name);
        }
    }
}