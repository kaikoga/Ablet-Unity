using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using UnityEngine;

namespace Ablet.Builtin.VRChat.Avatars
{
    [AbletSubplatform]
    public class VRChatAvatarSDK3AndroidSubplatform : IAbletSubplatform
    {
        string IAbletDefinition.Id => BuiltinPlatformIds.VRChatAvatarSDK3Android;
        string IAbletDefinition.DisplayName => "VRChat Mobile";
        string IAbletSubplatform.PlatformId => BuiltinPlatformIds.VRChatAvatarSDK3;

        int IAbletSubplatform.Priority => 0;

        public bool IsAvailable => true;

        public bool IsPreferredSubplatform(GameObject entrypointObject) => false;
    }
}
