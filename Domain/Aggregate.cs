using System.Collections.Generic;
using Domain.Events;

namespace Domain
{
    public abstract class Aggregate
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
        }

        protected abstract void Apply(IEvent e);

        protected void ApplyChange<T2>(T2 e) where T2:IEvent
        {
            ApplyChange(e, true);
        }

        private void ApplyChange<T2>(T2 e, bool isNew) where T2:IEvent
        {
            Apply(e);

            if (isNew)
            {
                _changes.Add(e);
            }
        }
    }
}
