using System;
using System.Data;

namespace Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction GetTransaction();
        void Commit();
    }
}