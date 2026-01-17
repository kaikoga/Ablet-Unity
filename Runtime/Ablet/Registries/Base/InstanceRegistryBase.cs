using Ablet.API;

namespace Ablet.Registries.Base
{
    public abstract class InstanceRegistryBase<TDefinition> : RegistryBase<TDefinition, TDefinition>
        where TDefinition : class, IAbletDefinable
    {
    }
}
