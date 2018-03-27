using System;
using System.Collections.Generic;
using System.Threading;
using Domain.Events;

namespace Domain
{
    public class DictionaryBackedEventPublisher : IEventPublisher
    {
        private readonly Dictionary<Type, List<Action<Event>>> _routes = new Dictionary<Type, List<Action<Event>>>();

        public void RegisterHandler<T>(Action<T> handler) where T : Event
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if(!_routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Action<Event>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T)x)));
        }

        public void Publish<T>(T @event) where T : Event
        {
            if (!_routes.TryGetValue(@event.GetType(), out var handlers)) return;

            foreach(var handler in handlers)
            {
                var h = handler;
                ThreadPool.QueueUserWorkItem(x => h(@event));
            }
        }
    }
}