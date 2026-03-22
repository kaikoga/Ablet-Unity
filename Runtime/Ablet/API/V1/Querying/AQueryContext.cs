using System.Diagnostics.CodeAnalysis;

namespace Ablet.API.V1.Querying
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface AQueryContext
    {
        AQuery<T> Query<T>(AQueryResolver<T> resolver);
    }
}
