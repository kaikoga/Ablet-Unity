using System;
using Ablet.API.V1;
using Ablet.Building;
using Ablet.ErrorReporting.Serialized;
using Ablet.Hooks.NdmfPlugin;
using Ablet.Models.Serialized;
using Ablet.Planning;
using Ablet.Registries;
using Ablet.Repositories;
using Ablet.Utils;
using nadena.dev.ndmf;
using UnityEngine;
using BuildContext = nadena.dev.ndmf.BuildContext;

[assembly: ExportsPlugin(typeof(InitializeAbletPlugin))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfImportingPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfConvertingPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfPruningPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfPopulatingPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfGeneratingPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfTransformingPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfMaterializingPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfReducingPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfCollectingPhase>))]
[assembly: ExportsPlugin(typeof(AbletOnNdmfLayerPlugin<NdmfExportingPhase>))]

namespace Ablet.Hooks.NdmfPlugin
{
    [RunsOnAllPlatforms]
    class InitializeAbletPlugin : Plugin<InitializeAbletPlugin>
    {
        public override string QualifiedName => "net.kaikoga.ablet.initialize-ablet";
        public override string DisplayName => "Ablet: Initialize Ablet";

        protected override void Configure()
        {
            if (!EditorSettingsRepository.Instance.Value.IsAbletOnNdmf)
            {
                return;
            }
            InPhase(BuildPhase.FirstChance).Run(InitializeAbletPass.Instance);
        }
    }

    class AbletState
    {
        public readonly GameObject ActualEntrypoint;

        public AbletState(GameObject actualEntrypoint)
        {
            ActualEntrypoint = actualEntrypoint;
        }
    }

    class InitializeAbletPass : Pass<InitializeAbletPass>
    {
        protected override void Execute(BuildContext context)
        {
            var rootObject = context.AvatarRootObject;
            var actualEntrypoint = AbletRuntimeUtil.GuessActualEntrypointMaybeCloned(rootObject);
            var serializedEntrypointReference = SerializedEntrypointReference.FromContext(actualEntrypoint, rootObject);
            // NOTE: flush log here because we are in Ablet on NDMF and this is a partial build (BeforeLayer<PhaseContainer> won't run)
            BuildReportRepository.Instance.Clear<SerializedErrorReport>(serializedEntrypointReference);
            context.GetState(_ => new AbletState(actualEntrypoint));
        }
    }

    [RunsOnAllPlatforms]
    class AbletOnNdmfLayerPlugin<T> : Plugin<AbletOnNdmfLayerPlugin<T>>
    where T : AbletOnNdmfPhase, new()
    {
        readonly T _abletNdmfPhase = new T();

        public override string QualifiedName => $"net.kaikoga.{_abletNdmfPhase.LayerId.ToLowerInvariant()}";
        public override string DisplayName => $"Ablet: {_abletNdmfPhase.DisplayName}";

        protected override void Configure()
        {
            if (!EditorSettingsRepository.Instance.Value.IsAbletOnNdmf)
            {
                return;
            }
            var plan = AvatarBuildPlanner.PartialPlan(
                LayerRegistry.Instance.All(),
                _abletNdmfPhase.AbletLayer);
            var sequence = InPhase(_abletNdmfPhase.NdmfBuildPhase);
            foreach (var pass in plan)
            {
                _abletNdmfPhase.NdmfConfigure(sequence, new AbletOnNdmfPass(pass));
            }
        }
    }

    class AbletOnNdmfPass : Pass<AbletOnNdmfPass>
    {
        readonly AbletPass _abletPass;
        public override string QualifiedName => _abletPass.Layer.Id;
        public override string DisplayName => $"Ablet: {_abletPass.Layer.DisplayName}";

        public AbletOnNdmfPass() => throw new NotSupportedException();

        public AbletOnNdmfPass(AbletPass abletPass)
        {
            _abletPass = abletPass;
        }

        protected override void Execute(BuildContext context)
        {
            var actualEntrypoint = context.GetState<AbletState>(null).ActualEntrypoint;
            var platform = PlatformRegistry.Instance.RequirePlatform(context.AvatarRootObject); 
            var arguments = BuildArgument.FromPartialBuild(actualEntrypoint, platform, ObjectRetainMode.RetainObjectId, BuildInitiationSourceMode.NDMF);
            var process = BuildProcess.FromSinglePass(_abletPass, arguments);
            process.Build(context.AvatarRootObject);
        }
    }
}
