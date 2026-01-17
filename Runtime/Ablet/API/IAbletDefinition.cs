namespace Ablet.API
{
    public interface IAbletDefinable
    {
    }

    public interface IAbletDefinition : IAbletDefinable
    {
        string Id { get; }
        string DisplayName { get; }
    }
}
