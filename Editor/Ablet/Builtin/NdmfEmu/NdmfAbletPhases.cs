using Ablet.API;
using Ablet.API.Attributes;
using Ablet.Building;
using Ablet.Builtin.Utils;
using Ablet.Repositories;
using nadena.dev.ndmf;
using nadena.dev.ndmf.platform;
using UnityEngine;
using AbletBuildContext = Ablet.Building.BuildContext;
using NdmfBuildContext = nadena.dev.ndmf.BuildContext;

namespace Ablet.Builtin.NdmfEmu
{
    abstract class NdmfAbletPhase<T> : IAbletLayer
    where T : IAbletLayer
    {
        public abstract string Id { get; }
        public abstract string DisplayName { get; }
        public virtual int Priority => 0x1000000; // very late

        protected abstract BuildPhase NdmfBuildPhase { get; }

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<T>();
        }

        IAbletProcessor IAbletLayer.Processor(BuildArgument argument)
        {
            return EditorSettingsRepository.Instance.Value.IsNdmfOnAblet ? new NdmfAbletProcessor(NdmfBuildPhase) : null;
        }
    }

    class NdmfAbletProcessor : IAbletProcessor
    {
        readonly BuildPhase _ndmfBuildPhase;
        public NdmfAbletProcessor(BuildPhase ndmfBuildPhase)
        {
            _ndmfBuildPhase = ndmfBuildPhase;
        }

        public void Process(AbletBuildContext buildContext)
        {
            if (!buildContext.CurrentRootObject.TryGetComponent<NdmfAbletContextHolder>(out var contextHolder))
            {
                contextHolder = buildContext.CurrentRootObject.AddComponent<NdmfAbletContextHolder>();
                var platform = PlatformRegistry.GetPrimaryPlatformForAvatar(buildContext.CurrentRootObject);
                contextHolder.NdmfBuildContext = new NdmfBuildContext(buildContext.CurrentRootObject, null, platform);
            }
            if (typeof(AvatarProcessor).GetMethod("ProcessAvatar",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic,
                    null,
                    new[] { typeof(NdmfBuildContext), typeof(BuildPhase), typeof(BuildPhase) },
                    null) is { } processAvatarMethod)
            {
                processAvatarMethod.Invoke(null, new[]
                {
                    contextHolder.NdmfBuildContext,
                    _ndmfBuildPhase,
                    _ndmfBuildPhase
                });
            }
            if (_ndmfBuildPhase == BuildPhase.PlatformFinish)
            {
                Object.DestroyImmediate(contextHolder);
            }
        }
    }

    // FIXME: conditional
    [AbletLayer]
    class NdmfAbletFirstChancePhase : NdmfAbletPhase<BeforeLayer<ImportingPhase>>
    {
        public override string Id => "ablet.ndmf.phase.first-chance";
        public override string DisplayName => "NDMF: First Chance";
        public override int Priority => -10000;
        protected override BuildPhase NdmfBuildPhase => BuildPhase.FirstChance;
    }

    [AbletLayer]
    class NdmfAbletInternalPrePlatformInitPhase : NdmfAbletPhase<BeforeLayer<ImportingPhase>>
    {
        public override string Id => "ablet.ndmf.phase.internal-pre-platform-init";
        public override string DisplayName => "NDMF: Internal Pre Platform Init";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.BuiltInPhases[1];
    }

    [AbletLayer]
    class NdmfAbletPlatformInitPhase : NdmfAbletPhase<ImportingPhase>
    {
        public override string Id => "ablet.ndmf.phase.platform-init";
        public override string DisplayName => "NDMF: Platform Init";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.PlatformInit;
    }

    [AbletLayer]
    class NdmfAbletResolvingPhase : NdmfAbletPhase<PopulatingPhase>
    {
        public override string Id => "ablet.ndmf.phase.resolving";
        public override string DisplayName => "NDMF: Resolving";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.Resolving;
    }

    [AbletLayer]
    class NdmfAbletGeneratingPhase : NdmfAbletPhase<GeneratingPhase>
    {
        public override string Id => "ablet.ndmf.phase.generating";
        public override string DisplayName => "NDMF: Generating";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.Generating;
    }

    [AbletLayer]
    class NdmfAbletTransformingPhase : NdmfAbletPhase<TransformingPhase>
    {
        public override string Id => "ablet.ndmf.phase.transforming";
        public override string DisplayName => "NDMF: Transforming";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.Transforming;
    }

    [AbletLayer]
    class NdmfAbletOptimizingPhase : NdmfAbletPhase<CollectingPhase>
    {
        public override string Id => "ablet.ndmf.phase.optimizing";
        public override string DisplayName => "NDMF: Optimizing";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.Optimizing;
    }

    [AbletLayer]
    class NdmfAbletPlatformFinishPhase : NdmfAbletPhase<ExportingPhase>
    {
        public override string Id => "ablet.ndmf.phase.platform-finish";
        public override string DisplayName => "NDMF: Platform Finish";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.PlatformFinish;
    }
}
