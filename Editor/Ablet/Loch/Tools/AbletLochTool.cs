using System;
using UnityEngine.UIElements;

#if ABLET_LOCH

using System.Reflection;
using Silksprite.Loch;
using Silksprite.Loch.UIElements.Tools;

namespace Ablet.Loch.Tools
{
    public static class AbletLochTool
    {
        public static string Tr(string key) => Loc(key, Assembly.GetCallingAssembly()).Tr;

        public static LocalizedContent Loc(string key) => Loc(key, Assembly.GetCallingAssembly());

        static LocalizedContent Loc(string key, Assembly assembly) => new LocalizedContent(key, assembly);
        
        public static T LocalizeWith<T>(this T container, Type type) where T : VisualElement => LochElementTool.LocalizeWith(container, type);

        public static void Localize<T>(this VisualElement container) => LochElementTool.Localize<T>(container);
    }
}

#else

namespace Ablet.Loch.Tools
{
    public static class AbletLochTool
    {
        public static string Tr(string key) => key;

        public static string Loc(string key) => key;

        public static T LocalizeWith<T>(this T container, Type type) where T : VisualElement => container;

        public static void Localize<T>(this VisualElement container) { }
    }
}

#endif