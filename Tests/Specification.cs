using System.Collections.Generic;
using Domain;
using Domain.Events;
using Xunit;

namespace Tests
{
    public abstract class Specification<T> where T : Aggregate
    {
        protected abstract T Given();
        protected abstract void When();
        protected readonly T Sut;
        protected readonly List<IEvent> Produced;

        protected Specification()
        {
            Sut = Given();
            Sut.MarkChangesAsCommitted();
            When();
            Produced = new List<IEvent>(Sut.GetUncommittedChanges());
        }
    }

    public class ThenAttribute : FactAttribute { }
}