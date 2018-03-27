using System;
using System.Data;
using Domain;
using Domain.Entities;

namespace DapperDb
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IEventPublisher _publisher;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IRepository<Landlord> _landlordRepository;
        private bool _disposed;

        public UnitOfWork(IEventPublisher publisher, IDbConnection connection)
        {
            _publisher = publisher;
            _connection = connection;
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IRepository<Landlord> LandlordRepository
        {
            get
            {
                return _landlordRepository ??
                       (_landlordRepository = new LandlordRepository(new SqlEventStore(_publisher, _transaction)));
            }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _landlordRepository = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }

                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}