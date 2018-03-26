using System;

namespace Domain
{
    public interface IRepository<T> where T : Aggregate
    {
        void Save(T aggregate, int expectedVersion);
        T GetById(Guid id);
    }
}