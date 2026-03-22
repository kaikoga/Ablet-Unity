using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ablet.API.V1.Querying
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface AQuery<out T>
    {
        AQueryContext Context { get; }
        IEnumerable<T> ResolveNow();
        void Observe(Action<T> filter);
    }
}
