using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.Utils.Reflection;

namespace Ablet.Registries.Base
{
    public abstract class RegistryBase<TKeyType, TValue>
    where TValue : class
    {
        readonly Dictionary<Type, TValue> _models = new Dictionary<Type, TValue>();

        readonly List<IValueCollector<TValue>> _collectors = new List<IValueCollector<TValue>>();

        protected void Collect(IValueCollector<TValue> collector)
        {
            foreach (var kv in collector.Collect())
            {
                _models.Add(kv.Key, kv.Value);
            }
            _collectors.Add(collector);
        }

        public TValue Get<T>() where T : TKeyType => ByType(typeof(T));

        protected TValue ByType(Type type)
        {
            if (_models.TryGetValue(type, out var value))
            {
                return value;
            }
            var newValue = _collectors
                .Select(collector => collector.MaybeConstruct(type))
                .First(val => val != null)!;
            _models.Add(type, newValue);
            return newValue;
        }

        public abstract IEnumerable<TValue> All();

        protected IEnumerable<TValue> Unordered() => _models.Values;
    }
}
