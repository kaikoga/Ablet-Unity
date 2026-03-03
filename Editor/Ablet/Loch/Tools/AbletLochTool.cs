using JetBrains.Annotations;
using UnityEngine.SocialPlatforms;

#if ABLET_LOCH

using System.Reflection;
using Silksprite.Loch;

namespace Ablet.Loch.Tools
{
    public static class AbletLochTool
    {
        public static string Tr(string key) => Loc(key, Assembly.GetCallingAssembly()).Tr;

        public static LocalizedContent Loc(string key) => Loc(key, Assembly.GetCallingAssembly());

        static LocalizedContent Loc(string key, Assembly assembly) => new LocalizedContent(key, assembly);
    }
}

#else

namespace Ablet.Loch.Tools
{
    public static class AbletLochTool
    {
        public static string Tr(string key) => key;

        public static string Loc(string key) => key;
    }
}

#endif