using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin.Utils;
using Ablet.Repositories;
using nadena.dev.ndmf;
using UnityEngine;
using NdmfBuildContext = nadena.dev.ndmf.BuildContext;
using PlatformRegistry = nadena.dev.ndmf.platform.PlatformRegistry;

namespace Ablet.Builtin.NdmfEmu
{
    abstract class NdmfOnAbletPhase<T> : IAbletSpecialLayer
    where T : IAbletSpecialLayer
    {
        public abstract string Id { get; }
        public abstract string DisplayName { get; }

        public virtual int LayerPriority => -1000;
        string IAbletSpecialLayer.IdForPriority => Id;
        int IAbletSpecialLayer.InnerPriority => 0;

        bool IAbletSpecialLayer.IsConcreteLayer => true;

        protected abstract BuildPhase NdmfBuildPhase { get; }

        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<T>();
        }

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            return EditorSettingsRepository.Instance.Value.IsNdmfOnAblet ? new NdmfOnAbletProcedure(NdmfBuildPhase) : null;
        }
    }

    class NdmfOnAbletProcedure : AbletBuildProcedure
    {
        readonly BuildPhase _ndmfBuildPhase;
        public NdmfOnAbletProcedure(BuildPhase ndmfBuildPhase)
        {
            _ndmfBuildPhase = ndmfBuildPhase;
        }

        public override void Process(IBuildContext context)
        {
            if (!context.CurrentRootObject.TryGetComponent<NdmfAbletContextHolder>(out var contextHolder))
            {
                contextHolder = context.CurrentRootObject.AddComponent<NdmfAbletContextHolder>();
                var platform = PlatformRegistry.GetPrimaryPlatformForAvatar(context.CurrentRootObject);
                contextHolder.NdmfBuildContext = new NdmfBuildContext(context.CurrentRootObject, null, platform);
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
    class NdmfOnAbletFirstChancePhase : NdmfOnAbletPhase<BeforeLayer<ImportingPhase>>
    {
        public override string Id => "Ablet.Ndmf.Phase.FirstChance";
        public override string DisplayName => "NDMF: First Chance";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.FirstChance;
    }

    [AbletLayer]
    class NdmfOnAbletInternalPrePlatformInitPhase : NdmfOnAbletPhase<BeforeLayer<ConvertingPhase>>
    {
        public override string Id => "Ablet.Ndmf.Phase.InternalPrePlatformInit";
        public override string DisplayName => "NDMF: Internal Pre Platform Init";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.BuiltInPhases[1];
    }

    [AbletLayer]
    class NdmfOnAbletPlatformInitPhase : NdmfOnAbletPhase<ConvertingPhase>
    {
        public override string Id => "Ablet.Ndmf.Phase.PlatformInit";
        public override string DisplayName => "NDMF: Platform Init";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.PlatformInit;
    }

    [AbletLayer]
    class NdmfOnAbletResolvingPhase : NdmfOnAbletPhase<PopulatingPhase>
    {
        public override string Id => "Ablet.Ndmf.Phase.Resolving";
        public override string DisplayName => "NDMF: Resolving";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.Resolving;
    }

    [AbletLayer]
    class NdmfOnAbletGeneratingPhase : NdmfOnAbletPhase<GeneratingPhase>
    {
        public override string Id => "Ablet.Ndmf.Phase.Generating";
        public override string DisplayName => "NDMF: Generating";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.Generating;
    }

    [AbletLayer]
    class NdmfOnAbletTransformingPhase : NdmfOnAbletPhase<TransformingPhase>
    {
        public override string Id => "Ablet.Ndmf.Phase.Transforming";
        public override string DisplayName => "NDMF: Transforming";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.Transforming;
    }

    [AbletLayer]
    class NdmfOnAbletOptimizingPhase : NdmfOnAbletPhase<CollectingPhase>
    {
        public override string Id => "Ablet.Ndmf.Phase.Optimizing";
        public override string DisplayName => "NDMF: Optimizing";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.Optimizing;
    }

    [AbletLayer]
    class NdmfOnAbletPlatformFinishPhase : NdmfOnAbletPhase<ExportingPhase>
    {
        public override string Id => "Ablet.Ndmf.Phase.PlatformFinish";
        public override string DisplayName => "NDMF: Platform Finish";
        protected override BuildPhase NdmfBuildPhase => BuildPhase.PlatformFinish;
    }
}
