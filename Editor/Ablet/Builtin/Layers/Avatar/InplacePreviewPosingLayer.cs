using System;
using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.API.V1.Extensions.Layer;
using Ablet.Builtin.Utils;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class InplacePreviewPosingLayer : IAbletSpecialLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.InplacePreviewPosing;
        string IAbletDefinition.DisplayName => "Inplace Preview Posing";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<AvatarBuildingRootLayer>>();
            config.AddDependency<CloneBeforeBuildLayer>();
        }

        int IAbletSpecialLayer.LayerPriority => int.MinValue;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;
        bool IAbletSpecialLayer.IsConcreteLayer => true;

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    [AbletExtension]
    class InplacePreviewPosingLayerExtension : IInplacePreviewSupportExtension
    {
        Type IAbletExtension.ForType => typeof(InplacePreviewPosingLayer);

        public AbletObservableProcedure ToProcedure(IBuildArgument argument) => new InplacePreviewPosingProcedure();
    }

    class InplacePreviewPosingInput
    {
        public readonly AbletObservableProcedure? Value;
        
        public InplacePreviewPosingInput(AbletObservableProcedure? value) => Value = value;
    }
    
    class InplacePreviewPosingProcedure : AbletObservableProcedure
    {
        public override void Observe(IObserveContext context)
        {
            if (context.Argument.TryGetInput<InplacePreviewPosingInput>(out var posing))
            {
                posing.Value?.Observe(context);
            }
        }
    }
}
