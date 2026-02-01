using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin.Utils;
using Ablet.ErrorReporting.Serialized;
using Ablet.Models.Serialized;
using Ablet.Repositories;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class FlushErrorReportsLayer : IAbletSpecialLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.FlushErrorReports;
        string IAbletDefinition.DisplayName => "Flush Error Reports";

        int IAbletSpecialLayer.LayerPriority => int.MinValue;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => int.MinValue;

        bool IAbletSpecialLayer.IsConcreteLayer => true;

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<AvatarBuildingRootLayer>>();
        }

        AbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            return new FlushErrorReportsProcedure();
        }
    }

    class FlushErrorReportsProcedure : AbletBuildProcedure
    {
        public override void Process(IBuildContext context)
        {
            BuildReportRepository.Instance.Clear<SerializedErrorReport>(SerializedEntrypointReference.FromContext(context));
        }
    }
}
