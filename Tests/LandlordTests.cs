using System;
using System.Collections.Generic;
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
            var sut = new Landlord(new List<IEvent>
                                   {
                                       new LandlordCreated(
                                           new Guid("00000000-0000-0000-0000-000000000000"), "Bob")
                                   });

            sut.ChangeName("Harry");

            Assert.Equal(new List<IEvent>
                         {
                             new LandlordChangedName(new Guid("00000000-0000-0000-0000-000000000000"), "Harry")
                         }, sut.GetUncommittedChanges());
        }
    }
}