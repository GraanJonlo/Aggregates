using System;
using Domain;
using Domain.Entities;

namespace DapperDb
{
    public class LandlordRepository : IRepository<Landlord>
    {
        private readonly IEventStore _storage;

        public LandlordRepository(IEventStore storage)
        {
            _storage = storage;
        }

        public void Save(Landlord aggregate, int expectedVersion)
        {
            _storage.SaveEvents("landlord", aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        }

        public Landlord Get(Guid id)
        {
            var e = _storage.GetEvents("landlord", id);
            return Landlord.Create(e);
        }
    }
}