using System;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Extensions.Layer;
using Ablet.Building;
using Ablet.Models.Extensions;

namespace Ablet.Models
{
    public class AbletLayer : IAbletIdModelBase
    {
        readonly IAbletLayer _def;

        public Type DefType => _def.GetType();

        public string Id => _def.Id;
        public string DisplayName => _def.DisplayName;
        public void Configure(IDependencyConfigurator config) => _def.Configure(config);

        public AbletProcedure? ToProcedure(BuildArgument argument)
        {
            switch (argument.PreviewMode)
            {
                case PreviewMode.ActualBuild:
                    break;
                case PreviewMode.InPlacePreview:
                    if (this.TryGetExtensionDef<IInplacePreviewSupportExtension>(out var inplacePreview))
                    {
                        return inplacePreview.ToProcedure(argument);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return _def.ToProcedure(argument);
        }

        internal int LayerPriority => _def switch
        {
            IAbletSpecialLayer specialLayer => specialLayer.LayerPriority, 
            _ => 0
        };

        internal string IdForPriority => _def switch
        {
            IAbletSpecialLayer specialLayer => specialLayer.IdForPriority, 
            _ => _def.Id
        };

        // NOTE: for BeforeLayer<T> and AfterLayer<T>
        internal int InnerPriority => _def switch
        {
            IAbletSpecialLayer specialLayer => specialLayer.InnerPriority, 
            _ => 0
        };

        internal bool IsConcreteLayer => _def switch
        {
            IAbletSpecialLayer specialLayer => specialLayer.IsConcreteLayer, 
            _ => true
        };

        public AbletLayer(IAbletLayer def) => _def = def;
    }
}
