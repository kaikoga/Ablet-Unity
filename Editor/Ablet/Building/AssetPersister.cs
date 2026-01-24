using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Ablet.Building
{
    public static class AssetPersister
    {
        internal const string OutputPath = "Assets/AbletOutput";
        internal const string TempPath = "Assets/AbletOutput/__Temp__";

        public static IEnumerable<string> GetManualAssetPaths()
        {
            return AssetDatabase.FindAssets("t:Prefab", new[] { OutputPath })
                .Select(AssetDatabase.GUIDToAssetPath);
        }

        public static void ClearManualAssets()
        {
            AssetDatabase.DeleteAsset(OutputPath);
        }

        public static void ClearTempAssets()
        {
            AssetDatabase.DeleteAsset(TempPath);
        }

        public static void DelayClearTempAssets()
        {
            EditorApplication.delayCall += ClearTempAssets;
        }
    }
}
