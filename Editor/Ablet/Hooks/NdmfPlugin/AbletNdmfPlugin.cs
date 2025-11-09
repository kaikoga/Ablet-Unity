using System;
using Ablet.Building;
using Ablet.Hooks.NdmfPlugin;
using Ablet.Planning;
using Ablet.Repositories;
using nadena.dev.ndmf;
using BuildContext = nadena.dev.ndmf.BuildContext;

[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfImportingPhase>))]
[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfPruningPhase>))]
[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfPopulatingPhase>))]
[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfGeneratingPhase>))]
[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfTransformingPhase>))]
[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfMaterializingPhase>))]
[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfReducingPhase>))]
[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfCollectingPhase>))]
[assembly: ExportsPlugin(typeof(AbletNdmfLayerPlugin<NdmfExportingPhase>))]

namespace Ablet.Hooks.NdmfPlugin
{
    class AbletNdmfLayerPlugin<T> : Plugin<AbletNdmfLayerPlugin<T>>
    where T : AbletNdmfPhase, new()
    {
        readonly T _abletNdmfPhase = new T();

        public override string QualifiedName => _abletNdmfPhase.QualifiedName;
        public override string DisplayName => $"Ablet: {_abletNdmfPhase.DisplayName}";

        protected override void Configure()
        {
            if (!EditorSettingsRepository.Instance.Value.IsAbletOnNdmf)
            {
                return;
            }
            var plan = BuildPlanner.PartialPlan(LayerRepository.Instance.All(), _abletNdmfPhase.AbletLayer);
            var sequence = InPhase(_abletNdmfPhase.NdmfBuildPhase);
            foreach (var pass in plan)
            {
                _abletNdmfPhase.NdmfConfigure(sequence, new AbletNdmfPass(pass));
            }
        }
    }

    class AbletNdmfPass : Pass<AbletNdmfPass>
    {
        readonly AbletPass _abletPass;
        public override string QualifiedName => _abletPass.Layer.Id;
        public override string DisplayName => $"Ablet: {_abletPass.Layer.DisplayName}";

        public AbletNdmfPass() => throw new NotSupportedException();

        public AbletNdmfPass(AbletPass abletPass)
        {
            _abletPass = abletPass;
        }

        protected override void Execute(BuildContext context)
        {
            var platform = PlatformRepository.Instance.GuessPlatform(context.AvatarRootObject);
            var arguments = BuildArgument.FromPartialBuild(platform, context.AvatarRootObject);
            var process = BuildProcess.FromSinglePass(_abletPass, arguments);
            process.Build();
        }
    }

}
