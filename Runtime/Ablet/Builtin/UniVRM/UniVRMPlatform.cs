using System;
using Ablet.API;
using Ablet.API.Attributes;
using VRM;

namespace Ablet.Builtin.UniVRM
{
    [AbletPlatform]
    public class UniVRMPlatform : IAbletPlatform
    {
        int IAbletDefinitionBase.Priority => 0;
        string IAbletDefinitionBase.Id => "ablet.univrm.vrm0";
        string IAbletDefinitionBase.DisplayName => "UniVRM VRM0";

        bool IAbletPlatform.ApplyOnPlay => true;

        Type IAbletPlatform.EntryPointComponentType => typeof(VRMMeta);
    }
}
