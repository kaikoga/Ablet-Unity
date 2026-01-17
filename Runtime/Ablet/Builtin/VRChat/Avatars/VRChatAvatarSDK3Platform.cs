using System;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Extensions.Platform;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Ablet.Builtin.VRChat.Avatars
{
    [AbletPlatform]
    public class VRChatAvatarSDK3Platform : IAbletPlatform
    {
        string IAbletDefinition.Id => BuiltinPlatformIds.VRChatAvatarSDK3;
        string IAbletDefinition.DisplayName => "VRChat";

        int IAbletPlatform.Priority => 0;

        Type IAbletPlatform.EntrypointComponentType => typeof(VRCAvatarDescriptor);
        bool IAbletPlatform.FilterEntrypoint(Component component) => true;
    }

    [AbletExtension]
    class VRChatAvatarSDK3Extension : IApplyOnPlaySupportExtension
    {
        Type IAbletExtension.ForType => typeof(VRChatAvatarSDK3Platform);
    }
}
