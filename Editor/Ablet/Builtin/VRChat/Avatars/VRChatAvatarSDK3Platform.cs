using System;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Extensions.Platform;
using UnityEditor;
using UnityEngine;
using VRC.Core;
using VRC.SDK3.Avatars.Components;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.VRChat.Avatars
{
    [AbletExtension]
    class VRChatAvatarSDK3Extension : IConvertiblePlatformExtension
    {
        Type IAbletExtension.ForType => typeof(VRChatAvatarSDK3Platform);

        void IConvertiblePlatformExtension.MarkAsAvatarRoot(GameObject avatarRoot)
        {
            if (!avatarRoot.TryGetComponent(out VRCAvatarDescriptor _))
            {
                var vrcAvatar = avatarRoot.AddComponent<VRCAvatarDescriptor>();
                // Initialize array SerializeFields with empty array instances
                EditorUtility.CopySerialized(vrcAvatar, vrcAvatar);
            }
        }

        void IConvertiblePlatformExtension.MaterializeAsAvatarRoot(GameObject avatarRoot)
        {
            // do nothing
        }

        void IConvertiblePlatformExtension.UnmarkAsAvatarRoot(GameObject avatarRoot)
        {
            if (avatarRoot.TryGetComponent(out VRCAvatarDescriptor vrcAvatar))
            {
                Object.DestroyImmediate(vrcAvatar);
            }
            if (avatarRoot.TryGetComponent(out PipelineManager pipelineManager))
            {
                Object.DestroyImmediate(pipelineManager);
            }
        }
    }
}
