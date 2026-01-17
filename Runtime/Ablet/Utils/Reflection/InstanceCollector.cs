using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API;

namespace Ablet.Utils.Reflection
{
    public class InstanceCollector<TDefinition, TMarker> : IValueCollector<TDefinition>
        where TDefinition : class, IAbletDefinable
        where TMarker : Attribute
    {
        public Dictionary<Type, TDefinition> Collect()
        {
            return TypeCollector.Collect<TMarker>()
                .Select(type => (type, def: MaybeActivator.MaybeConstruct<TDefinition>(type)))
                .Where(kv => kv.def != null)
                .ToDictionary(kv => kv.type, kv => kv.def!);
        }

        public TDefinition? MaybeConstruct(Type type)
        {
            return MaybeActivator.MaybeConstruct<TDefinition>(type);
        }
    }
}
