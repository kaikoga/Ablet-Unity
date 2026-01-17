using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin.Utils;
using Ablet.Querying;
using UnityEngine;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class PruneEditorOnlyTagLayer : IAbletLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.PruneEditorOnlyTag;
        string IAbletDefinition.DisplayName => "Prune Editor Only Tag";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<PruningPhase>>();
        }

        AbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument) => new PruneEditorOnlyTagProcedure();
    }

    class PruneEditorOnlyTagProcedure : AbletBuildProcedure
    {
        public override void Process(IBuildContext context)
        {
            context.RootObject
                .GetComponentsInChildren<Transform>(true)
                .Where(transform => transform && transform.gameObject.CompareTag("EditorOnly"))
                .Observe(transform =>
                {
                    if (transform)
                    {
                        Object.DestroyImmediate(transform.gameObject);
                    }
                });
        }
    }
}
