using Ablet.API;
using Ablet.API.Internal;
using Ablet.Building;
using Ablet.Repositories;
using JetBrains.Annotations;

namespace Ablet.Builtin.Utils
{
    [PublicAPI]
    public class BeforeLayer<T> : IBeforeLayer
    where T : IAbletLayer
    {
        IAbletLayer Target => LayerRepository.Instance.Get<T>(); 
        string IAbletDefinitionBase.Id => "ablet.before." + Target.Id;
        string IAbletDefinitionBase.DisplayName => "Before::" + Target.DisplayName;
        int IAbletDefinitionBase.Priority => 0; // int.MinValue - 1L

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<T>();
        }
        public IAbletProcessor Processor(BuildArgument argument) => null;
    }
}
