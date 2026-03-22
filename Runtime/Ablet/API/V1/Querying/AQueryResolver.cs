using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ablet.API.V1.Querying
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface AQueryResolver<out T>
    {
        IEnumerable<T> ResolveNow();
    }
}
