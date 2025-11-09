using System.Linq;
using Ablet.Building;
using Ablet.Builtin;
using Ablet.Builtin.VRChat.Avatars;
using Ablet.Planning;
using Ablet.Repositories;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;

namespace Ablet.Hooks.VRChat
{
    class AbletVRChatBuildPreprocessor : IVRCSDKPreprocessAvatarCallback
    {
        // before NDMF, because we may want to initiate Ablet Build from NDMF Apply on Play
        // also see NdmfApplyOnPlay
        public int callbackOrder => -12000;

        public bool OnPreprocessAvatar(GameObject avatarGameObject)
        {
            if (IsAbletOnNdmf)
            {
                // do nothing now, Ablet is executed in NDMF
                return true;
            }
            var platform = PlatformRepository.Instance.Get<VRChatAvatarSDK3Platform>();
            var arguments = BuildArgument.FromPartialBuild(platform, avatarGameObject);
            var plan = BuildPlanner.Plan(LayerRepository.Instance.All())
                .TakeWhile(pass => pass.Layer is not MaterializingPhase);
            var process = BuildProcess.FromPlan(plan, arguments);
            process.Build();
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
            var platform = PlatformRepository.Instance.Get<VRChatAvatarSDK3Platform>();
            var arguments = BuildArgument.FromPartialBuild(platform, avatarGameObject);
            var plan = BuildPlanner.Plan(LayerRepository.Instance.All())
                .SkipWhile(pass => pass.Layer is not MaterializingPhase);
            var process = BuildProcess.FromPlan(plan, arguments);
            process.Build();
            return true;
        }
        
#if ABLET_NDMF
        bool IsAbletOnNdmf => EditorSettingsRepository.Instance.Value.IsAbletOnNdmf;
#else
        bool IsAbletOnNdmf => false;
#endif

    }
}
