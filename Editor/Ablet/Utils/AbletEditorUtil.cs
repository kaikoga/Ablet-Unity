using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Ablet.Utils
{
    public static class AbletEditorUtil
    {
        public static void OpenInExplorer(string directoryPath)
        {
#if UNITY_EDITOR_WIN
            Process.Start("explorer.exe", directoryPath);
#else
            Process.Start("open", directoryPath);
#endif
        }

        public static void CreateFolderRecursive(string folderPath)
        {
            var path = folderPath.Split("/");
            if (path.Length < 2)
            {
                return;
            }
            if (path[0] != "Assets")
            {
                return;
            }
            var dir = "Assets";
            foreach (var folder in path.Skip(1))
            {
                var child = Path.Join(dir, folder);
                if (!AssetDatabase.IsValidFolder(child))
                {
                    AssetDatabase.CreateFolder(dir, folder);
                }
                dir = child; 
            }
        }
    }
}
