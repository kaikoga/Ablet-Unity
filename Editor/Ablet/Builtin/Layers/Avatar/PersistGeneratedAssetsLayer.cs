using System.Linq;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Building;
using Ablet.Building.Ephemeral;
using Ablet.Builtin.Utils;
using Ablet.Hooks;
using UnityEditor;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class PersistGeneratedAssetsLayer : IAbletLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.PersistGeneratedAssets;
        string IAbletDefinition.DisplayName => "Persist Generated Assets";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            // FIXME: AfterLayer<AvatarBuildingRootLayer> is not working
            config.AddDependency<AfterLayer<AvatarBuildingRootLayer>>();
        }

        AbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument) => new PersistGeneratedAssetsProcedure();
    }

    class PersistGeneratedAssetsProcedure : AbletBuildProcedure
    {
        public override void Process(IBuildContext context)
        {
            if (context.TryGetArtifact<AssetPersisterState>(out var assetPersister))
            {
                var rootObject = context.CurrentRootObject;
                assetPersister.PersistAssets(AssetPersister.IterateHierarchyAssetReferences(rootObject)
                    .Where(obj => !EditorUtility.IsPersistent(obj)));
                assetPersister.SaveAsPrefab(rootObject);
                AbletHooksUtil.AddManualAppliedTag(rootObject, context.Argument.TargetPlatform.Id, context.Argument.TargetSubplatform.Id);
            }
        }
    }
}
