using System;
using Domain.Values;

namespace Domain.Events
{
    public class LandlordCreated : Event
    {
        public readonly Guid Id;
        public readonly Name Name;
        public readonly string EmailAddress;
        public readonly Address Address;

        public LandlordCreated(Guid id, Name name, string email, Address address)
        {
            Id = id;
            Name = name;
            EmailAddress = email;
            Address = address;
        }
    }
}