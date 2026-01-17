using System;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin.Utils;
using Ablet.Hooks;
using Ablet.Utils;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class PersistGeneratedAssetsLayer : IAbletLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.PersistGeneratedAssets;
        string IAbletDefinition.DisplayName => "Persist Generated Assets";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<NextLayer<ExportingPhase>>();
        }

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            return argument.WillPersistGeneratedAssets switch
            {
                AssetGenerationMode.None => null,
                AssetGenerationMode.Temporary => new FinishManualApplyProcedure(true),
                AssetGenerationMode.UserInitiated => new FinishManualApplyProcedure(false),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    class FinishManualApplyProcedure : AbletBuildProcedure
    {
        readonly bool _isTemporary;
        
        public FinishManualApplyProcedure(bool isTemporary) => _isTemporary = isTemporary;

        public override void Process(IBuildContext context)
        {
            var rootObject = context.CurrentRootObject;
            AssetPersister.PersistAssets(rootObject, _isTemporary);
            rootObject.AddComponent<AbletManualAppliedTag>();
        }
    }
}
