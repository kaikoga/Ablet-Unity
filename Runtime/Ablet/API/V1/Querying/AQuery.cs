using System;
using System.Collections.Generic;

namespace Ablet.API.V1.Querying
{
    public interface AQuery<out T>
    {
        AQueryContext Context { get; }
        IEnumerable<T> ResolveNow();
        void Observe(Action<T> filter);
    }
}
