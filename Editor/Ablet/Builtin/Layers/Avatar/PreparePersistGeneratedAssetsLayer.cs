using System;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Building.Ephemeral;
using Ablet.Builtin.Utils;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class PreparePersistGeneratedAssetsLayer : IAbletLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.PreparePersistGeneratedAssets;
        string IAbletDefinition.DisplayName => "Prepare Persist Generated Assets";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<AvatarBuildingRootLayer>>();
        }

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            return argument.WillPersistGeneratedAssets switch
            {
                AssetGenerationMode.None => null,
                AssetGenerationMode.Temporary => new PreparePersistGeneratedAssetsProcedure(true),
                AssetGenerationMode.UserInitiated => new PreparePersistGeneratedAssetsProcedure(false),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    class PreparePersistGeneratedAssetsProcedure : AbletBuildProcedure
    {
        readonly bool _isTemporary;
        
        public PreparePersistGeneratedAssetsProcedure(bool isTemporary) => _isTemporary = isTemporary;

        public override void Process(IBuildContext context)
        {
            context.AddArtifact(new AssetPersisterState(context.Argument.EntrypointObject, _isTemporary));
        }
    }
}
