using Ablet.API;
using Ablet.Builtin;
using Ablet.Repositories;
using nadena.dev.ndmf;
using nadena.dev.ndmf.builtin;
using nadena.dev.ndmf.fluent;

namespace Ablet.Hooks.NdmfPlugin
{
    abstract class AbletNdmfPhase
    {
        public abstract IAbletLayer AbletLayer { get; }
        
        public string QualifiedName => AbletLayer.Id;
        public string DisplayName => AbletLayer.DisplayName;

        public abstract BuildPhase NdmfBuildPhase { get; }

        public void NdmfConfigure(Sequence sequence, AbletNdmfPass pass)
        {
            NdmfConfigureSequence(sequence);
            NdmfConfigurePass(sequence.Run(pass));
        }
        protected virtual void NdmfConfigureSequence(Sequence sequence) { }
        protected virtual void NdmfConfigurePass(DeclaringPass pass) { }
    }

    abstract class AbletNdmfPhase<T> : AbletNdmfPhase
    where T : IAbletLayer
    {
        public override IAbletLayer AbletLayer => LayerRepository.Instance.Get<T>();
    }

    class NdmfImportingPhase : AbletNdmfPhase<ImportingPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.PlatformInit;
    }

    class NdmfPruningPhase : AbletNdmfPhase<PruningPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.Resolving;
        protected override void NdmfConfigurePass(DeclaringPass pass) => pass.BeforePass(RemoveEditorOnlyPass.Instance);
    }

    class NdmfPopulatingPhase : AbletNdmfPhase<PopulatingPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.Resolving;

        // protected override void NdmfConfigureSequence(Sequence sequence) => sequence.AfterPass(RemoveEditorOnlyPass.Instance);
        protected override void NdmfConfigureSequence(Sequence sequence) => sequence.AfterPlugin("nadena.dev.ndmf.InternalPasses");

    }

    class NdmfGeneratingPhase : AbletNdmfPhase<GeneratingPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.Generating;
    }

    class NdmfTransformingPhase : AbletNdmfPhase<TransformingPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.Transforming;
    }

    class NdmfMaterializingPhase : AbletNdmfPhase<MaterializingPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.Optimizing;
    }

    class NdmfReducingPhase : AbletNdmfPhase<ReducingPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.Optimizing;

        protected override void NdmfConfigureSequence(Sequence sequence) => sequence.AfterPlugin(BuiltinLayers.Materializing);
    }

    class NdmfCollectingPhase : AbletNdmfPhase<CollectingPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.Optimizing;

        protected override void NdmfConfigureSequence(Sequence sequence) => sequence.AfterPlugin(BuiltinLayers.Reducing);
    }

    class NdmfExportingPhase : AbletNdmfPhase<ExportingPhase>
    {
        public override BuildPhase NdmfBuildPhase => BuildPhase.PlatformFinish;
    }
}
