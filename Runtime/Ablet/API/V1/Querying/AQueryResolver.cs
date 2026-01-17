using System.Collections.Generic;

namespace Ablet.API.V1.Querying
{
    public interface AQueryResolver<out T>
    {
        IEnumerable<T> ResolveNow();
    }
}
