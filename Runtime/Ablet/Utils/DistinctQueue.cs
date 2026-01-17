using System;
using System.Collections.Generic;

namespace Ablet.Utils
{
    public class DistinctQueue<T, TKey>
    {
        readonly Func<T, TKey> _func;
        readonly Queue<T> _queue = new Queue<T>();
        readonly HashSet<TKey> _added = new HashSet<TKey>();

        public DistinctQueue(Func<T, TKey> func)
        {
            _func = func;
        }

        public void EnqueueDistinct(T value)
        {
            var toAdd = _func(value);
            if (_added.Contains(toAdd))
            {
                return;
            }
            _queue.Enqueue(value);
            _added.Add(toAdd);
        }

        public bool TryDequeue(out T value)
        {
            return _queue.TryDequeue(out value);
        }
    }
    
    public class DistinctQueue<T> : DistinctQueue<T, T>
    {
        static T Identity(T value) => value;

        public DistinctQueue() : base(Identity)
        {
        }
    }
}
