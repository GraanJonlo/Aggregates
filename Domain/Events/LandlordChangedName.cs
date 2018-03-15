using System;

namespace Domain.Events
{
    public class LandlordChangedName : Event
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