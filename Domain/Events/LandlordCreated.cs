using System;

namespace Domain.Events
{
    public readonly struct LandlordCreated : IEvent
    {
        public readonly Guid Id;
        public readonly string Name;

        public LandlordCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}