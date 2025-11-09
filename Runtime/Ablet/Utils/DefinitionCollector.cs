using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API;
using UnityEditor;

namespace Ablet.Utils
{
    public static class DefinitionCollector<TDefinition, TMarker>
    where TDefinition : IAbletDefinitionBase
    where TMarker : Attribute
    {
        public static Dictionary<Type, TDefinition> Collect()
        {
            return TypeCache.GetTypesWithAttribute<TMarker>()
                .Select(type => (type, def: MaybeConstruct(type)))
                .Where(kv => kv.def != null)
                .ToDictionary(kv => kv.type, kv => kv.def);
        }

        public static TDefinition MaybeConstruct(Type type) => (TDefinition)type.GetConstructor(Array.Empty<Type>())?.Invoke(null);
    }
}
