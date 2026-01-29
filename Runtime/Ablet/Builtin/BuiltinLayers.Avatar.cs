using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Planning;

namespace Ablet.Builtin
{
    public static partial class BuiltinLayerIds
    {
        public static class Avatar
        {
            public const string Root = AvatarBuildPlanner.DefaultRootLayerId;

            public const string Importing = "Ablet.Phase.Avatar.Importing";
            public const string Converting = "Ablet.Phase.Avatar.Converting";
            public const string Pruning = "Ablet.Phase.Avatar.Pruning";
            public const string Populating = "Ablet.Phase.Avatar.Populating";
            public const string Generating =  "Ablet.Phase.Avatar.Generating";
            public const string Transforming = "Ablet.Phase.Avatar.Transforming";
            public const string Materializing = "Ablet.Phase.Avatar.Materializing";
            public const string Reducing =  "Ablet.Phase.Avatar.Reducing";
            public const string Collecting =  "Ablet.Phase.Avatar.Collecting";
            public const string Exporting =  "Ablet.Phase.Avatar.Exporting";

            public const string InplacePreviewPosing =  "Ablet.Avatar.InplacePreviewPosing";
            public const string PreparePersistGeneratedAssets =  "Ablet.Avatar.PreparePersistGeneratedAssets";
            public const string FlushManualApply =  "Ablet.Avatar.FlushManualApply";
            public const string FlushErrorReports =  "Ablet.Avatar.FlushErrorReports";
            public const string CloneBeforeBuild =  "Ablet.Avatar.CloneBeforeBuild";

            public const string MarkAsTargetPlatform =  "Ablet.Avatar.MarkAsTargetPlatform";

            public const string PruneEditorOnlyTag =  "Ablet.Avatar.PruneEditorOnlyTag";

            public const string SetupAsTargetPlatform =  "Ablet.Avatar.SetupAsTargetPlatform";
            public const string UnmarkAsOtherPlatforms =  "Ablet.Avatar.UnmarkAsOtherPlatforms";

            public const string PersistGeneratedAssets =  "Ablet.Avatar.PersistGeneratedAssets";
        }
    }

    [AbletLayer]
    class AvatarBuildingRootLayer : IAbletSpecialLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Root;
        string IAbletDefinition.DisplayName => "Avatar Building Phases";
        int IAbletSpecialLayer.LayerPriority => 100;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) { }
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Importing phase is a pre-setup phase, where the context object imports platform specific formats.
     */
    [AbletLayer]
    public class ImportingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Importing;
        string IAbletDefinition.DisplayName => "Importing Phase";
        int IAbletSpecialLayer.LayerPriority => 1000;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Converting phase is a pre-setup phase, where the context object is set up for the target platform.
     */
    [AbletLayer]
    public class ConvertingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Converting;
        string IAbletDefinition.DisplayName => "Converting Phase";
        int IAbletSpecialLayer.LayerPriority => 1010;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Pruning phase is a setup phase, where redundant objects are removed before build.
     * It is not expected to add any contents to the context object.
     */
    [AbletLayer]
    public class PruningPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Pruning;
        string IAbletDefinition.DisplayName => "Pruning Phase";
        int IAbletSpecialLayer.LayerPriority => 1100;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Populating phase is a setup phase, where missing data structure components are supplemented before build.
     */
    [AbletLayer]
    public class PopulatingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Populating;
        string IAbletDefinition.DisplayName => "Populating Phase";
        int IAbletSpecialLayer.LayerPriority => 1110;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Generating phase is a build phase, where operator components are generated into the context object.
     */
    [AbletLayer]
    public class GeneratingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Generating;
        string IAbletDefinition.DisplayName => "Generating Phase";
        int IAbletSpecialLayer.LayerPriority => 1500;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Transforming phase is a build phase, where operator components would transform the structure of context object.
     */
    [AbletLayer]
    public class TransformingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Transforming;
        string IAbletDefinition.DisplayName => "Transforming Phase";
        int IAbletSpecialLayer.LayerPriority => 1510;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Materializing phase is a cleanup phase, where component setups are fixed to meet platform requirements.
     */
    [AbletLayer]
    public class MaterializingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Materializing;
        string IAbletDefinition.DisplayName => "Materializing Phase";
        int IAbletSpecialLayer.LayerPriority => 1800;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Reducing phase is a cleanup phase, where compressions and reductions take place.
     * It is not expected to add any major visible contents to the context object.
     */
    [AbletLayer]
    public class ReducingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Reducing;
        string IAbletDefinition.DisplayName => "Reducing Phase";
        int IAbletSpecialLayer.LayerPriority => 1810;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Collecting phase is a cleanup phase, where pure optimizations take place.
     * It is not expected to add any contents to the context object.
     */
    [AbletLayer]
    public class CollectingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Collecting;
        string IAbletDefinition.DisplayName => "Collecting Phase";
        int IAbletSpecialLayer.LayerPriority => 1820;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }

    /**
     * The Exporting phase is a post-cleanup phase, where the context object is exported to platform specific formats.
     * It is not expected to modify the context object.
     */
    [AbletLayer]
    public class ExportingPhase : IAbletSpecialLayer, IAbletPhase
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.Exporting;
        string IAbletDefinition.DisplayName => "Exporting Phase";
        int IAbletSpecialLayer.LayerPriority => 1900;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<AvatarBuildingRootLayer>();
        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument) => null;
    }
}
