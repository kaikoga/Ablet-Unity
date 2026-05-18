using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API;

namespace Ablet.Utils.Reflection
{
    public class ModelCollector<TDefinition, TMarker, TModel> : IValueCollector<TModel>
        where TDefinition : class, IAbletDefinable 
        where TMarker : Attribute
        where TModel : class
    {
        readonly Func<TDefinition, TModel?> _converter;

        public ModelCollector(Func<TDefinition, TModel?> converter) => _converter = converter;

        public Dictionary<Type, TModel> Collect()
        {
            return TypeCollector.Collect<TMarker>()
                .Select(type => (type, def: MaybeActivator.MaybeConstruct<TDefinition>(type)))
                .Where(kv => kv.def != null)
                .Select(kv => (kv.type, model: _converter(kv.def!)))
                .Where(kv => kv.model != null)
                .ToDictionary(kv => kv.type, kv => kv.model!);
        }

        public TModel? MaybeConstruct(Type type)
        {
            var def = MaybeActivator.MaybeConstruct<TDefinition>(type);
            return def != null ? _converter(def) : null;
        }
    }
}
