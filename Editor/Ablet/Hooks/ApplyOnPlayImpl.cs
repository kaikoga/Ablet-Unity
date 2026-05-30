using System.Linq;
using Ablet.Building;
using Ablet.Builtin;
using Ablet.Hooks.Default;
using Ablet.Models;
using Ablet.Querying;
using Ablet.Registries;
using UnityEngine;

namespace Ablet.Hooks
{
    public static class ApplyOnPlayImpl
    {
        static bool _includeVRChat;

        public static void OnRuntimeInitializeOnLoad(bool includeVRChat)
        {
            _includeVRChat = includeVRChat;
            AbletAwaker.OnAbletAwake += ApplyOnPlay;
            var awaker = new GameObject("AbletAwaker");
            awaker.AddComponent<AbletAwaker>();
            Object.DontDestroyOnLoad(awaker);
        }

        static void ApplyOnPlay()
        {
            // naively skip inactive avatar
            var allPlatforms = PlatformRegistry.Instance.All().ToArray();
            var applicablePlatformIds = allPlatforms.Where(platform => platform.ApplyOnPlay)
                .Where(platform => _includeVRChat || platform.Id != BuiltinPlatformIds.VRChatAvatarSDK3)
                .Select(platform => platform.Id)
                .ToArray();
            AQueryContext.Immediate.GetSceneEntrypoints(allPlatforms, false)
                .Where(r => applicablePlatformIds.Contains(r.platform.Id))
                .Observe(r =>
            {
                var arguments = BuildArgumentBuilder.TargetsPlatform(r.gameObject, r.platform).ForApplyOnPlay();
                AbletFacade.BuildWithArguments(arguments);
            });
        }
    }
}
