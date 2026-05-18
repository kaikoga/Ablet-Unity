using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using UnityEngine;

namespace Ablet.Builtin.VRChat.Avatars
{
    [AbletSubplatform]
    public class VRChatAvatarSDK3IosSubplatform : IAbletSubplatform
    {
        string IAbletDefinition.Id => BuiltinPlatformIds.VRChatAvatarSDK3Ios;
        string IAbletDefinition.DisplayName => "VRChat iOS";
        string IAbletSubplatform.PlatformId => BuiltinPlatformIds.VRChatAvatarSDK3;

        int IAbletSubplatform.Priority => 2;

        public bool IsAvailable => true;

#if UNITY_IOS
        public bool IsPreferredSubplatform(GameObject entrypointObject) => true;
#else
        public bool IsPreferredSubplatform(GameObject entrypointObject) => false;
#endif
    }
}
