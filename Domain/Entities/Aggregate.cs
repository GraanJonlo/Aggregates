using System;
using System.Collections.Generic;
using Domain.Events;

namespace Domain.Entities
{
    public abstract class Aggregate
    {
        private readonly List<Event> _changes = new List<Event>();
        public abstract Guid Id { get; }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
        }

        protected abstract void Apply(Event e);

        protected void ApplyChange<T>(T e) where T:Event
        {
            ApplyChange(e, true);
        }

        private void ApplyChange<T>(T e, bool isNew) where T:Event
        {
            Apply(e);

            if (isNew)
            {
                _changes.Add(e);
            }
        }
    }
}
