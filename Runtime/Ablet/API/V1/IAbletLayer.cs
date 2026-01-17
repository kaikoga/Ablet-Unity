using Ablet.API.V1.Building;

namespace Ablet.API.V1
{
    /// <summary>
    /// An Ablet Layer is the representation of a procedural operation unit against an Ablet asset.
    /// </summary>
    public interface IAbletLayer : IAbletDefinition
    {
        void Configure(IDependencyConfigurator config);

        AbletProcedure? ToProcedure(IBuildArgument argument);
    }

    public interface IDependencyConfigurator
    {
        IDependencyConfigurator AddDependency(string id);
        IDependencyConfigurator AddDependency<T>() where T : IAbletLayer;
        IDependencyConfigurator AddReverseDependency(string id);
        IDependencyConfigurator AddReverseDependency<T>() where T : IAbletLayer;
    }
}
