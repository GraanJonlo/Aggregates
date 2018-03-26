using System;
using System.Collections.Generic;
using Domain.Events;
using Domain.Values;

namespace Domain.Entities
{
    public class Landlord : Aggregate
    {
        private Guid _id;

        public override Guid Id => _id;

        private Landlord()
        {}

        private Landlord(Guid id, Name name, string email, Address address)
        {
            ApplyChange(new LandlordCreated(id, name, email, address));
        }

        public static Landlord Create(Guid id, Name name, string email, Address address)
        {
            // TODO Validate
            return new Landlord(id, name, email, address);
        }

        public static Landlord Create(List<Event> e)
        {
            var instance = new Landlord();
            instance.LoadFromHistory(e);
            return instance;
        }

        protected override void Apply(Event e)
        {
            switch (e)
            {
                case LandlordCreated x:
                    Apply(x);
                    break;
            }
        }

        private void Apply(LandlordCreated e)
        {
            _id = e.Id;
        }

        public void ChangeName(Name name)
        {
            ApplyChange(new LandlordChangedName(_id, name));
        }
    }
}