using System;
using System.Collections.Generic;
using Domain.Events;

namespace Domain
{
    public abstract class Aggregate
    {
        private Dictionary<Type, Action<IEvent>> _handlers = new Dictionary<Type, Action<IEvent>>();
        private readonly List<IEvent> _changes = new List<IEvent>();

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange<T>(T e) where T:IEvent
        {
            ApplyChange(e, true);
        }

        private void ApplyChange<T>(T e, bool isNew) where T:IEvent
        {
            if (_handlers.TryGetValue(typeof(T), out var handler))
            {
                handler(e);
            }

            if (isNew)
            {
                _changes.Add(e);
            }
        }

        protected void Register(Dictionary<Type, Action<IEvent>> handlers)
        {
            _handlers = handlers;
        }
    }
}
