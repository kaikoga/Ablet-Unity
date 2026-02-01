using System.Linq;
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
    class UnmarkAsOtherPlatformsLayer : IAbletSpecialLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.UnmarkAsOtherPlatforms;
        string IAbletDefinition.DisplayName => "Unmark as Other Platforms";

        int IAbletSpecialLayer.LayerPriority => int.MinValue;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => int.MinValue;

        bool IAbletSpecialLayer.IsConcreteLayer => true;

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<CollectingPhase>>();
        }

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            var otherPlatforms = PlatformRegistry.Instance.All()
                .Where(platform => platform.Id != argument.TargetPlatform.Id);
            var converters = otherPlatforms.Select(otherPlatform =>
                {
                    otherPlatform.TryGetExtensionDef<IConvertiblePlatformExtension>(out var converter);
                    return converter;
                })
                .OfType<IConvertiblePlatformExtension>()
                .ToArray();
            return new UnmarkAsOtherPlatformsProcedure(converters);
        }
    }

    class UnmarkAsOtherPlatformsProcedure : AbletBuildProcedure
    {
        readonly IConvertiblePlatformExtension[] _converters;

        public UnmarkAsOtherPlatformsProcedure(IConvertiblePlatformExtension[] converters)
        {
            _converters = converters;
        }

        public override void Process(IBuildContext context)
        {
            context.RootObject.Observe(rootObject =>
            {
                foreach (var converter in _converters)
                {
                    converter.UnmarkAsAvatarRoot(rootObject);
                }
            });
        }
    }
}
