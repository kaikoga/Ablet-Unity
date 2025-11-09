using System;
using Ablet.API;
using Ablet.API.Attributes;
using VRC.SDK3.Avatars.Components;

namespace Ablet.Builtin.VRChat.Avatars
{
    [AbletPlatform]
    public class VRChatAvatarSDK3Platform : IAbletPlatform
    {
        int IAbletDefinitionBase.Priority => 0;
        string IAbletDefinitionBase.Id => "ablet.vrchat.avatar.sdk3";
        string IAbletDefinitionBase.DisplayName => "VRChat";

        bool IAbletPlatform.ApplyOnPlay => true;

        Type IAbletPlatform.EntryPointComponentType => typeof(VRCAvatarDescriptor);
    }
}
