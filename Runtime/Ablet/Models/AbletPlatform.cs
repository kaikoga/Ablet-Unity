using System;
using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;
using Ablet.API.V1.Extensions.Platform;
using Ablet.Models.Extensions;
using UnityEngine;

namespace Ablet.Models
{
    public class AbletPlatform : IAbletIdModelBase
    {
        readonly IAbletPlatform _def;

        public Type DefType => _def.GetType();

        public string Id => _def.Id;
        public string DisplayName => _def.DisplayName;
        public int Priority => _def.Priority;
        public Type EntrypointComponentType => _def.EntrypointComponentType;
        public bool FilterEntrypoint(Component component) => _def.FilterEntrypoint(component);

        public bool ApplyOnPlay => this.TryGetExtensionDef<IApplyOnPlaySupportExtension>(out _);
        public bool TryGetConverter([MaybeNullWhen(false)] out IConvertiblePlatformExtension converter) => this.TryGetExtensionDef(out converter);

        public AbletPlatform(IAbletPlatform def) => _def = def;
    }
}
