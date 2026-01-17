using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;
using Ablet.Registries;

namespace Ablet.Models.Extensions
{
    public static class AbletExtensionExtension
    {
        public static bool TryGetExtensionDef<T>(this IAbletModelBase def, [MaybeNullWhen(false)] out T extDef)
        where T : IAbletExtension
        {
            foreach (var val in ExtensionRegistry.Instance.ForType<T>(def.DefType))
            {
                extDef = (T)val.Def;
                return true;
            }
            extDef = default;
            return false;
        }
    }
}
