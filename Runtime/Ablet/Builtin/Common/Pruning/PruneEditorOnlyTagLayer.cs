using Ablet.API;
using Ablet.API.Attributes;
using Ablet.Building;
using Ablet.Builtin.Utils;
using UnityEngine;

namespace Ablet.Builtin.Common.Pruning
{
    [AbletLayer]
    public class PruneEditorOnlyTagLayer : IAbletLayer
    {
        string IAbletDefinitionBase.Id => "ablet.layer.prune-editor-only";
        string IAbletDefinitionBase.DisplayName => "Prune Editor Only Tag";
        int IAbletDefinitionBase.Priority => 0;
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<PruningPhase>>();
        }

        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => new PruneEditorOnlyTagProcessor();

    }

    public class PruneEditorOnlyTagProcessor : IAbletProcessor
    {
        void IAbletProcessor.Process(BuildContext buildContext)
        {
            // Debug.LogError(context.CurrentRootObject);
        }
    }
}
