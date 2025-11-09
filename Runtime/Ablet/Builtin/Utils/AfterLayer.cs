using Ablet.API;
using Ablet.API.Internal;
using Ablet.Building;
using Ablet.Repositories;
using JetBrains.Annotations;

namespace Ablet.Builtin.Utils
{
    [PublicAPI]
    public class AfterLayer<T> : IAfterLayer
    where T : IAbletLayer
    {
        IAbletLayer Target => LayerRepository.Instance.Get<T>(); 
        string IAbletDefinitionBase.Id => "ablet.after." + Target.Id;
        string IAbletDefinitionBase.DisplayName => "After::" + Target.DisplayName;
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 1L

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<T>();
        }
        public IAbletProcessor Processor(BuildArgument argument) => null;
    }
}
