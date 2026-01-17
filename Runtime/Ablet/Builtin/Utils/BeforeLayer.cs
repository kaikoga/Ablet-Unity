using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Building;
using Ablet.Models;
using Ablet.Registries;

namespace Ablet.Builtin.Utils
{
    /// <summary>
    /// Specifies an Before layer which runs before the specified phase or the specified layer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BeforeLayer<T> : IAbletSpecialLayer
    where T : IAbletLayer
    {
        AbletLayer Target => LayerRegistry.Instance.Get<T>(); 
        string IAbletDefinition.Id => BuiltinLayerIds.BeforePrefix + Target.Id;
        string IAbletDefinition.DisplayName => "Before::" + Target.DisplayName;

        // Delegates order of target layer to run just before target layer 
        int IAbletSpecialLayer.LayerPriority => Target.LayerPriority;
        string IAbletSpecialLayer.IdForPriority => Target.Id;
        int IAbletSpecialLayer.InnerPriority => -1;

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddReverseDependency<T>();
        }
        public AbletProcedure? ToProcedure(IBuildArgument argument) => null;
    }
}
