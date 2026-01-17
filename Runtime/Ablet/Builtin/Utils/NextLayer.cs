using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Building;
using Ablet.Models;
using Ablet.Registries;

namespace Ablet.Builtin.Utils
{
    /// <summary>
    /// Specifies an Before layer which runs at the beginning of the specified phase or just after specified layer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NextLayer<T> : IAbletSpecialLayer
    where T : IAbletLayer
    {
        AbletLayer Target => LayerRegistry.Instance.Get<T>();
        string IAbletDefinition.Id => BuiltinLayerIds.NextPrefix + Target.Id;
        string IAbletDefinition.DisplayName => "Next::" + Target.DisplayName;

        // Delegates order of target layer to run just after target layer but before its dependents
        int IAbletSpecialLayer.LayerPriority => int.MinValue;
        string IAbletSpecialLayer.IdForPriority => Target.Id;
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<T>();
        }
        public AbletProcedure? ToProcedure(IBuildArgument argument) => null;
    }
}
