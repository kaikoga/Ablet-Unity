namespace Ablet.API.V1.Querying
{
    public interface AQueryContext
    {
        AQuery<T> Query<T>(AQueryResolver<T> resolver);
    }
}
