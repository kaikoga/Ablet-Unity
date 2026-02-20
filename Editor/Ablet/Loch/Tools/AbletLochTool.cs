#if ABLET_LOCH

using System.Reflection;
using Silksprite.Loch;
using Silksprite.Loch.Core;

namespace Ablet.Loch.Tools
{
    public static class AbletLochTool
    {
        public static string Tr(string key)
        {
            var assembly = Assembly.GetCallingAssembly();
            return LochRepository.Instance.Tr(key, assembly);
        }

        public static LocalizedContent Loc(string key)
        {
            var assembly = Assembly.GetCallingAssembly();
            return new LocalizedContent(key, assembly);
        }
    }
}

#else

namespace Ablet.Loch.Tools
{
    public static class AbletLochTool
    {
        public static string Tr(string key)
        {
            return key;
        }

        public static string Loc(string key)
        {
            return key;
        }
    }
}

#endif