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
    class MarkAsTargetPlatformLayer : IAbletSpecialLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.MarkAsTargetPlatform;
        string IAbletDefinition.DisplayName => "Mark as Target Platform";

        // if NDMF on Ablet, this should after NDMF InternalPrePlatformInit (NDMF does stuff first)
        int IAbletSpecialLayer.LayerPriority => -900; 
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => int.MinValue;

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<ConvertingPhase>>();
        }

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            var targetPlatform = PlatformRegistry.Instance.ByPlatformHandle(argument.TargetPlatform);
            return targetPlatform.TryGetExtensionDef<IConvertiblePlatformExtension>(out var converter)
                ? new MarkAsTargetPlatformProcedure(converter)
                : null;
        }
    }
    
    class MarkAsTargetPlatformProcedure : AbletBuildProcedure
    {
        readonly IConvertiblePlatformExtension _converter;

        public MarkAsTargetPlatformProcedure(IConvertiblePlatformExtension converter)
        {
            _converter = converter;
        }

        public override void Process(IBuildContext context)
        {
            context.RootObject.Observe(rootObject =>
            {
                _converter.MarkAsAvatarRoot(rootObject);
            });
        }
    }
}
