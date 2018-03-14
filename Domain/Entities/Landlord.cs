using System;
using System.Collections.Generic;
using Domain.Events;

namespace Domain.Entities
{
    public class Landlord : Aggregate
    {
        private Guid _id;

        public Landlord(Guid id, string name)
        {
            var handlers = new Dictionary<Type, Action<IEvent>>
                           {
                               { typeof(LandlordCreated), x => Apply((LandlordCreated)x) },
                           };
            Register(handlers);

            ApplyChange(new LandlordCreated(id, name));
        }

        private void Apply(LandlordCreated e)
        {
            _id = e.Id;
        }

        public void ChangeName(string name)
        {
            ApplyChange(new LandlordChangedName(_id, name));
        }
    }
}