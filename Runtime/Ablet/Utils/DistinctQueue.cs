using System.Collections.Generic;

namespace Ablet.Utils
{
    public class DistinctQueue<T>
    {
        readonly Queue<T> _queue = new Queue<T>();
        readonly HashSet<T> _added = new HashSet<T>();

        public void EnqueueDistinct(T value)
        {
            if (_added.Contains(value))
            {
                return;
            }
            _queue.Enqueue(value);
            _added.Add(value);
        }

        public bool TryDequeue(out T value)
        {
            return _queue.TryDequeue(out value);
        }
    }
}
