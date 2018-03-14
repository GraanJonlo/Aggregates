using System;
using System.Collections.Generic;
using Domain.Events;

namespace Domain.Entities
{
    public class Landlord : Aggregate
    {
        private Guid _id;

        public Landlord(IEnumerable<IEvent> history)
        {
            LoadFromHistory(history);
        }

        public Landlord(Guid id, string name)
        {
            ApplyChange(new LandlordCreated(id, name));
        }

        protected override void Apply(IEvent e)
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

        public void ChangeName(string name)
        {
            ApplyChange(new LandlordChangedName(_id, name));
        }
    }
}