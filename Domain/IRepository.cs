using System;
using Domain.Entities;

namespace Domain
{
    public interface IRepository<T> where T : Aggregate
    {
        void Save(T aggregate, int expectedVersion);
        T Get(Guid id);
    }
}