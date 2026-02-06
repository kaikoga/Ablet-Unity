using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin.Utils;
using Ablet.Querying;
using UnityEditor;
using UnityEngine;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class PruneMissingScriptsLayer : IAbletLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.PruneMissingScripts;
        string IAbletDefinition.DisplayName => "Prune Missing Scripts";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            // NOTE: The actual position of this pass is undefined for now 
            config.AddDependency<AfterLayer<ImportingPhase>>();
        }

        AbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument) => new PruneMissingScriptsProcedure();
    }

    class PruneMissingScriptsProcedure : AbletBuildProcedure
    {
        public override void Process(IBuildContext context)
        {
            context.RootObject.GetComponentsInChildren<Transform>(true)
                .Observe(transform => GameObjectUtility.RemoveMonoBehavioursWithMissingScript(transform.gameObject));
        }
    }
}
