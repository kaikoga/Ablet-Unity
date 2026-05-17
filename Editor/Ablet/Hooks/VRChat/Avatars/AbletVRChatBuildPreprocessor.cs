using System.Linq;
using Ablet.API.V1;
using Ablet.Building;
using Ablet.Builtin;
using Ablet.Builtin.VRChat.Avatars;
using Ablet.Models;
using Ablet.Planning;
using Ablet.Registries;
using Ablet.Repositories;
using Ablet.Utils;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;

namespace Ablet.Hooks.VRChat.Avatars
{
    class AbletVRChatBuildPreprocessor : IVRCSDKPreprocessAvatarCallback
    {
        // before NDMF, because we may want to initiate Ablet Build from NDMF Apply on Play
        // also see NdmfApplyOnPlay
        public int callbackOrder => -12000;

        public bool OnPreprocessAvatar(GameObject avatarGameObject)
        {
            if (!EditorSettingsRepository.Instance.Value.ApplyOnPlatformBuild)
            {
                return true;
            }
            if (IsAbletOnNdmf)
            {
                // do nothing now, Ablet is executed in NDMF
                return true;
            }

            var actualEntrypoint = AbletRuntimeUtil.GuessActualEntrypointMaybeCloned(avatarGameObject);

            var platform = PlatformRegistry.Instance.Get<VRChatAvatarSDK3Platform>();
            var arguments = BuildArgumentBuilder.TargetsPlatform(actualEntrypoint, platform)
                .ForPartialAssetBuild(ObjectRetainMode.RetainObjectId, BuildInitiationSourceMode.PlatformBuild);
            var plan = AvatarBuildPlanner.Plan(LayerRegistry.Instance.All())
                .TakeWhile(pass => pass.Layer.DefType != typeof(MaterializingPhase));
            var process = BuildProcess.FromPlan(plan, arguments);
            process.Build(avatarGameObject);
            return true;
        }

#if ABLET_NDMF
        bool IsAbletOnNdmf => EditorSettingsRepository.Instance.Value.IsAbletOnNdmf;
#else
        bool IsAbletOnNdmf => false;
#endif
    }

    class AbletVRChatBuildOptimizer : IVRCSDKPreprocessAvatarCallback
    {
        public int callbackOrder => -1026; // just before RemoveAvatarEditorOnly, and also before NDMF

        public bool OnPreprocessAvatar(GameObject avatarGameObject)
        {
            if (IsAbletOnNdmf)
            {
                // do nothing now, Ablet is executed in NDMF
                return true;
            }
            var actualEntrypoint = AbletRuntimeUtil.GuessActualEntrypointMaybeCloned(avatarGameObject);
            var platform = PlatformRegistry.Instance.Get<VRChatAvatarSDK3Platform>();
            var arguments = BuildArgumentBuilder.TargetsPlatform(actualEntrypoint, platform)
                .ForPartialAssetBuild(ObjectRetainMode.RetainObjectId, BuildInitiationSourceMode.PlatformBuild);
            var plan = AvatarBuildPlanner.Plan(LayerRegistry.Instance.All())
                .SkipWhile(pass => pass.Layer.DefType != typeof(MaterializingPhase));
            var process = BuildProcess.FromPlan(plan, arguments);
            process.Build(avatarGameObject);
            return true;
        }

#if ABLET_NDMF
        bool IsAbletOnNdmf => EditorSettingsRepository.Instance.Value.IsAbletOnNdmf;
#else
        bool IsAbletOnNdmf => false;
#endif

    }

    class AbletVRChatBuildCleanup : IVRCSDKPostprocessAvatarCallback
    {
        public int callbackOrder => 0;

        public void OnPostprocessAvatar()
        {
            // Ablet should delay call this, because Ablet is exporting a prefab instance, in contrast to NDMF
            AssetPersister.DelayClearTempAssets();
        }
    }
}
