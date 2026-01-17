using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ablet.Repositories
{
    class DatastoreRepository
    {
        readonly Dictionary<Type, object> _data = new Dictionary<Type, object>();

        public void Add<T>(T value) where T : class => Add(typeof(T), value);

        public bool TryGet<T>([MaybeNullWhen(false)] out T value)
        where T : class
        {
            var result = _data.TryGetValue(typeof(T), out var obj);
            value = (T?)obj;
            return result;
        }

        public T GetOrCreate<T>()
        where T : class, new()
        {
            if (TryGet<T>(out var value))
            {
                return value;
            }
            var newValue = new T();
            Add(typeof(T), newValue);
            return newValue;
        }

        void Add(Type type, object value) => _data.Add(type, value);
    }
}
