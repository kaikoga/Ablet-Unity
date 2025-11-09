using Ablet.API;
using Ablet.API.Attributes;
using Ablet.Building;
using Ablet.Builtin.Utils;
using UnityEditor;
using UnityEngine;

namespace Ablet.Builtin
{
    [AbletLayer]
    public class PrepareManualExportLayer : IAbletLayer
    {
        string IAbletDefinitionBase.Id => "ablet.layer.prepare-manual-export";
        string IAbletDefinitionBase.DisplayName => "Prepare Manual Export";
        int IAbletDefinitionBase.Priority => 0;
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<PhaseContainer>>();
        }

        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => argument.IsUserInitiatedAction ? new PrepareManualExportProcessor() : null;
    }

    public class PrepareManualExportProcessor : IAbletProcessor
    {
        void IAbletProcessor.Process(BuildContext buildContext)
        {
            var clone = Object.Instantiate(buildContext.CurrentRootObject);
            Undo.RegisterCreatedObjectUndo(clone, "Ablet: Manual Export");
            var position = clone.transform.position;
            position.z += 2;
            clone.transform.position = position;
            buildContext.SetCurrentRootObject(clone);
        }
    }
}
