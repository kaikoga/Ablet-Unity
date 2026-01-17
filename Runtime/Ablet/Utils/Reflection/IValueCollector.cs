using System;
using System.Collections.Generic;

namespace Ablet.Utils.Reflection
{
    public interface IValueCollector<TModel>
    where TModel : class
    {
        Dictionary<Type, TModel> Collect();

        TModel? MaybeConstruct(Type type);
    }

}
