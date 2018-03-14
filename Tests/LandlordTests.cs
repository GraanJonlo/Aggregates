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
        public void CreateLandlord()
        {
            var l = new Landlord(Guid.NewGuid(), "Henry");
            l.ChangeName("Bob");
            l.ChangeName("Winston");
        }
    }

    public class changing_landlord_name : Specification<Landlord>
    {
        protected override Landlord Given()
        {
            return new Landlord(new Guid("00000000-0000-0000-0000-000000000000"), "Bob");
        }

        protected override void When()
        {
            Sut.ChangeName("Harry");
        }

        [Then]
        public void name_is_updated()
        {
            Assert.Equal(new List<IEvent>
                         {
                             new LandlordChangedName(new Guid("00000000-0000-0000-0000-000000000000"), "Harry")
                         }, Produced);
        }
    }
}