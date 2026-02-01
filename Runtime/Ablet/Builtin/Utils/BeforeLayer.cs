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

        // This layer should dispatch immediately after resolve by dependency  
        int IAbletSpecialLayer.LayerPriority => int.MinValue;
        string IAbletSpecialLayer.IdForPriority => Target.Id;
        int IAbletSpecialLayer.InnerPriority => int.MinValue;

        bool IAbletSpecialLayer.IsConcreteLayer => false;

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<T>();
        }
        public AbletProcedure? ToProcedure(IBuildArgument argument) => null;
    }
}
