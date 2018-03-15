using System;
using Domain;
using Domain.Entities;

namespace DapperDb
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Landlord> LandlordRepository { get; }

        void Commit();
    }
}