namespace Ablet.API
{
    public interface IAbletDefinitionBase
    {
        string Id { get; }
        string DisplayName { get; }
        int Priority { get; }
    }
}
