using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.API.V1.Extensions.Platform;
using Ablet.Builtin.Utils;
using Ablet.Models.Extensions;
using Ablet.Registries;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class MaterializeAsTargetPlatformLayer : IAbletSpecialLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.SetupAsTargetPlatform;
        string IAbletDefinition.DisplayName => "Setup as Target Platform";

        int IAbletSpecialLayer.LayerPriority => int.MinValue;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => int.MinValue;

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<AfterLayer<MaterializingPhase>>();
        }

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            var targetPlatform = PlatformRegistry.Instance.ByPlatformHandle(argument.TargetPlatform);
            return targetPlatform.TryGetExtensionDef<IConvertiblePlatformExtension>(out var converter)
                ? new MaterializeAsTargetPlatformProcedure(converter)
                : null;
        }
    }

    class MaterializeAsTargetPlatformProcedure : AbletBuildProcedure
    {
        readonly IConvertiblePlatformExtension _converter;

        public MaterializeAsTargetPlatformProcedure(IConvertiblePlatformExtension converter)
        {
            _converter = converter;
        }

        public override void Process(IBuildContext context)
        {
            context.RootObject.Observe(rootObject =>
            {
                _converter.MaterializeAsAvatarRoot(rootObject);
            });
        }
    }
}
