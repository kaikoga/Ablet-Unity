using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin.Utils;
using Ablet.Hooks.Common;
using Ablet.Querying;
using UnityEngine;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class PruneSelectSubplatformLayer : IAbletLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.PruneSelectSubplatform;
        string IAbletDefinition.DisplayName => "Prune Select Subplatform";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<PruningPhase>>();
        }

        AbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument) => new PruneSelectSubplatformProcedure();
    }

    class PruneSelectSubplatformProcedure : AbletBuildProcedure
    {
        public override void Process(IBuildContext context)
        {
            context.RootObject
                .GetComponentsInChildren<AbletSelectSubplatform>(true)
                .Observe(component =>
                {
                    if (component)
                    {
                        Object.DestroyImmediate(component);
                    }
                });
        }
    }
}
