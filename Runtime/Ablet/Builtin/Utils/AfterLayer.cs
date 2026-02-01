using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Building;
using Ablet.Models;
using Ablet.Registries;

namespace Ablet.Builtin.Utils
{
    /// <summary>
    /// Specifies an After layer which runs after the specified phase or the specified layer and its dependents.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AfterLayer<T> : IAbletSpecialLayer
    where T : IAbletLayer
    {
        AbletLayer Target => LayerRegistry.Instance.Get<T>();
        string IAbletDefinition.Id => BuiltinLayerIds.AfterPrefix + Target.Id;
        string IAbletDefinition.DisplayName => "After::" + Target.DisplayName;

        // Delegates order of target layer to run after target layer and its dependents 
        int IAbletSpecialLayer.LayerPriority => Target.LayerPriority;
        string IAbletSpecialLayer.IdForPriority => Target.Id;
        int IAbletSpecialLayer.InnerPriority => 1;

        bool IAbletSpecialLayer.IsConcreteLayer => false;

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<T>();
        }
        public AbletProcedure? ToProcedure(IBuildArgument argument) => null;
    }
}
