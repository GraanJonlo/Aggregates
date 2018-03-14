using System;

namespace Domain.Events
{
    public readonly struct LandlordChangedName : IEvent
    {
        public readonly Guid Id;
        public readonly string Name;

        public LandlordChangedName(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}