using System;
using Domain.Entities;

namespace Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Landlord> LandlordRepository { get; }

        void Commit();
    }
}