using System.Collections.Generic;
using System.Linq;
using Ablet.Repositories;
using UnityEditor;
using UnityEditor.Build;

namespace Ablet.ProjectSettings
{
    public static class AbletDefineSymbolsApplier
    {
        const string PreferAblet = "PREFER_ABLET";

        [InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            Apply();
        }

        public static void Apply()
        {
            var preferAblet = IsPreferAblet;

            foreach (var target in ActiveBuildTargets())
            {
                var namedTarget = NamedBuildTarget.FromBuildTargetGroup(target);
                PlayerSettings.GetScriptingDefineSymbols(namedTarget, out var defines);

                var newDefines = BuildDefines(defines.Where(x => !string.IsNullOrEmpty(x)), preferAblet).ToArray();
                if (!defines.SequenceEqual(newDefines))
                {
                    PlayerSettings.SetScriptingDefineSymbols(namedTarget, newDefines);
                }
            }
        }

#if ABLET_NDMF
        static bool IsPreferAblet => EditorSettingsRepository.Instance.Value.PreferAblet;
#else
        static bool IsPreferAblet => true;
#endif

        static IEnumerable<BuildTargetGroup> ActiveBuildTargets()
        {
            yield return BuildTargetGroup.Standalone;
            yield return BuildTargetGroup.Android;
            yield return BuildTargetGroup.iOS;
        }

        static IEnumerable<string> BuildDefines(IEnumerable<string> defines, bool preferAblet)
        {
            foreach (var define in defines)
            {
                switch (define)
                {
                    case PreferAblet:
                        if (!preferAblet) continue;
                        break;
                }
                yield return define;
            }
            if (preferAblet) yield return PreferAblet;
        }
    }
}
