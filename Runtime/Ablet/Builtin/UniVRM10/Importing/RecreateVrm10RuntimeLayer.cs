using Ablet.API;
using Ablet.API.Attributes;
using Ablet.Building;
using UniVRM10;

namespace Ablet.Builtin.UniVRM10.Importing
{
    [AbletLayer]
    public class RecreateVrm10RuntimeLayer : IAbletLayer
    {
        string IAbletDefinitionBase.Id => "ablet.univrm.vrm1.dispose-vrm10-runtime";
        string IAbletDefinitionBase.DisplayName => "Recreate VRM10 Runtime";
        int IAbletDefinitionBase.Priority => 0;
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<ImportingPhase>();
        }

        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => new DiscardVrm10RuntimeProcessor();
    }

    public class DiscardVrm10RuntimeProcessor : IAbletProcessor
    {
        void IAbletProcessor.Process(BuildContext buildContext)
        {
            if (buildContext.CurrentRootObject.TryGetComponent<Vrm10Instance>(out var vrm10Instance))
            {
                // vrm10Instance.DisposeRuntime();
                // vrm10Instance.enabled = true;
            }
        }
    }

}
