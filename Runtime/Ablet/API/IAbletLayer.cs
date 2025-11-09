using Ablet.Building;

namespace Ablet.API
{
    /// <summary>
    /// An Ablet Layer is the representation of a procedural operation unit against an Ablet asset.
    /// </summary>
    public interface IAbletLayer : IAbletDefinitionBase
    {
        void Configure(IDependencyConfigurator config);

        IAbletProcessor Processor(BuildArgument argument);
    }

    public interface IDependencyConfigurator
    {
        IDependencyConfigurator AddDependency(string id);
        IDependencyConfigurator AddDependency<T>() where T : IAbletLayer;
        IDependencyConfigurator AddReverseDependency(string id);
        IDependencyConfigurator AddReverseDependency<T>() where T : IAbletLayer;
    }
}
