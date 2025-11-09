using Ablet.API;
using Ablet.API.Attributes;
using Ablet.API.Internal;
using Ablet.Building;

namespace Ablet.Builtin
{
    public static class BuiltinLayers
    {
        internal const string Root = "ablet.root";

        public const string Importing = "ablet.phases.importing";
        public const string Pruning = "ablet.phases.pruning";
        public const string Populating = "ablet.phases.populating";
        public const string Generating = "ablet.phases.generating";
        public const string Transforming = "ablet.phases.transforming";
        public const string Materializing = "ablet.phases.materializing";
        public const string Reducing = "ablet.phases.reducing";
        public const string Collecting = "ablet.phases.collecting";
        public const string Exporting = "ablet.phases.exporting";
    }

    [AbletLayer]
    public class PhaseContainer : IAbletLayer
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Root;
        string IAbletDefinitionBase.DisplayName => "All Phases";
        int IAbletDefinitionBase.Priority => 0; // special handling for Before and After layers

        void IAbletLayer.Configure(IDependencyConfigurator config) { }
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class ImportingPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Importing;
        string IAbletDefinitionBase.DisplayName => "Importing Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<PhaseContainer>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class PruningPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Pruning;
        string IAbletDefinitionBase.DisplayName => "Pruning Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<ImportingPhase>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class PopulatingPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Populating;
        string IAbletDefinitionBase.DisplayName => "Populating Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<PruningPhase>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class GeneratingPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Generating;
        string IAbletDefinitionBase.DisplayName => "Generating Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<PopulatingPhase>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class TransformingPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Transforming;
        string IAbletDefinitionBase.DisplayName => "Transforming Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<GeneratingPhase>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class MaterializingPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Materializing;
        string IAbletDefinitionBase.DisplayName => "Materializing Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<TransformingPhase>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class ReducingPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Reducing;
        string IAbletDefinitionBase.DisplayName => "Reducing Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<MaterializingPhase>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class CollectingPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Collecting;
        string IAbletDefinitionBase.DisplayName => "Collecting Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<ReducingPhase>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }

    [AbletLayer]
    public class ExportingPhase : IAbletPhase
    {
        string IAbletDefinitionBase.Id => BuiltinLayers.Exporting;
        string IAbletDefinitionBase.DisplayName => "Exporting Phase";
        int IAbletDefinitionBase.Priority => 0; // int.MaxValue + 2L

        void IAbletLayer.Configure(IDependencyConfigurator config) => config.AddDependency<CollectingPhase>();
        IAbletProcessor IAbletLayer.Processor(BuildArgument argument) => null;
    }
}
