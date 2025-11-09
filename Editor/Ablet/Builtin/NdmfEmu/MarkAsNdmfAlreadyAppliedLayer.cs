using System.Reflection;
using Ablet.API;
using Ablet.API.Attributes;
using Ablet.Building;
using Ablet.Builtin.Utils;
using Ablet.Repositories;
using nadena.dev.ndmf.runtime;

namespace Ablet.Builtin.NdmfEmu
{
    [AbletLayer]
    public class MarkAsNdmfAlreadyAppliedLayer : IAbletLayer
    {
        string IAbletDefinitionBase.Id => "ablet.ndmf.layer.mark-as-ndmf-already-applied";
        string IAbletDefinitionBase.DisplayName => "Mark as NDMF Already Applied";
        int IAbletDefinitionBase.Priority => 0;
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<MaterializingPhase>>();
        }

        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => EditorSettingsRepository.Instance.Value.IsNdmfOnAblet ? new MarkAsAlreadyAppliedProcessor() : null;
    }

    public class MarkAsAlreadyAppliedProcessor : IAbletProcessor
    {
        void IAbletProcessor.Process(BuildContext buildContext)
        {
            // We have applied all NDMF passes partially from within Ablet, so prevent second time full run triggered by NDMF 
            var alreadyProcessedTag = typeof(RuntimeUtil).Assembly.GetType("nadena.dev.ndmf.runtime.AlreadyProcessedTag");
            if (alreadyProcessedTag == null) return;
            var processingCompleted = alreadyProcessedTag.GetField("processingCompleted", BindingFlags.Instance | BindingFlags.NonPublic);
            if (processingCompleted == null) return;
            var obj = buildContext.CurrentRootObject.AddComponent(alreadyProcessedTag);
            processingCompleted.SetValue(obj, true);
        }
    }
}
