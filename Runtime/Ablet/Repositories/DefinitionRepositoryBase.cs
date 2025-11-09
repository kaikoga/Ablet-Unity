using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API;
using Ablet.Utils;

namespace Ablet.Repositories
{
    public abstract class DefinitionRepositoryBase<TDefinition, TMarker>
    where TDefinition : IAbletDefinitionBase
    where TMarker : Attribute
    {
        Dictionary<Type, TDefinition> _definitions;
        Dictionary<Type, TDefinition> Definitions => _definitions ??= DefinitionCollector<TDefinition, TMarker>.Collect();

        public T Get<T>() where T : TDefinition
        {
            return (T)ByType(typeof(T));
        }

        public TDefinition ByType(Type type)
        {
            if (Definitions.TryGetValue(type, out var value))
            {
                return value;
            }
            var newValue = DefinitionCollector<TDefinition, TMarker>.MaybeConstruct(type);
            Definitions.Add(type, newValue);
            return newValue;
        }

        public bool TryGetById(string id, out TDefinition value)
        {
            foreach (var val in Definitions.Values.Where(def => def.Id == id))
            {
                value = val;
                return true;
            }
            value = default;
            return false;
        }

        public abstract IEnumerable<TDefinition> All();

        protected IEnumerable<TDefinition> Unordered() => Definitions.Values;

        protected IEnumerable<TDefinition> Ordered() => Definitions.Values.OrderBy(value => value.Priority);
    }
}
