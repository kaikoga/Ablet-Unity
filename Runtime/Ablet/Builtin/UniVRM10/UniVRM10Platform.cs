using System;
using Ablet.API;
using Ablet.API.Attributes;
using UniVRM10;

namespace Ablet.Builtin.UniVRM10
{
    [AbletPlatform]
    public class UniVRM10Platform : IAbletPlatform
    {
        int IAbletDefinitionBase.Priority => 0;
        string IAbletDefinitionBase.Id => "ablet.univrm.vrm1";
        string IAbletDefinitionBase.DisplayName => "UniVRM VRM1";

        bool IAbletPlatform.ApplyOnPlay => true;

        Type IAbletPlatform.EntryPointComponentType => typeof(Vrm10Instance);
    }
}
