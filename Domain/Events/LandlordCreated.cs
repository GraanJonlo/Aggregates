using System;

namespace Domain.Events
{
    public class LandlordCreated : Event
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