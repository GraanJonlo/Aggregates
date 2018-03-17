using System;
using Domain.Values;

namespace Domain.Events
{
    public class LandlordChangedName : Event
    {
        public readonly Guid Id;
        public readonly Name Name;

        public LandlordChangedName(Guid id, Name name)
        {
            Id = id;
            Name = name;
        }
    }
}