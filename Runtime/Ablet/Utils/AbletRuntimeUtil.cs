using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ablet.Utils
{
    public static class AbletRuntimeUtil
    {
        const string CloneSuffix = "(Clone)";

        public static string GuessOriginalAvatarName(GameObject avatarRoot)
        {
            var avatarName = avatarRoot.name;
            if (avatarName.EndsWith(CloneSuffix))
            {
                avatarName = avatarName.Substring(0, avatarName.Length - CloneSuffix.Length);
            }
            return avatarName;
        }
        
        public static string VrmAuthor => "no name";
        public static string VrmVersion => "0.1.0";

        public static GameObject GuessActualEntrypointMaybeCloned(GameObject maybeEntrypointObject)
        {
            // Some partial build hooks clone avatars outside PrepareManualApplyLayer.
            // In that case IBuildArgument.EntrypointObject is already a clone, so try to get the original
            var path = AbsolutePath(maybeEntrypointObject.transform);
            if (path.EndsWith(CloneSuffix))
            {
                path = path.Substring(0, path.Length - CloneSuffix.Length);
            }
            // 
            // NOTE: Scene is mostly wrong (because nobody seems to care which scene the clone goes to)
            // this is corrected in ErrorReportSerializer.Export()
            return // FromAbsolutePath(path, maybeEntrypointObject.scene)?.gameObject ??
                   FromAbsolutePath(path)?.gameObject
                   ?? maybeEntrypointObject;
        }

        public static string RelativePath(Transform root, Transform child)
        {
            return GetPath(root, child);
        }
        
        public static string AbsolutePath(Transform transform)
        {
            return GetPath(null, transform);
        }

        static string GetPath(Transform? root, Transform child)
        {
            // NOTE: Follows animation path convention
            if (root == child) return "";

            var cursor = child;
            var path = child.gameObject.name;
            while (true)
            {
                cursor = cursor.parent;
                if (cursor == root) break;
                if (!cursor) break;
                path = Path.Combine(cursor.gameObject.name, path);
            }
            return path;
        }

        public static Transform? FromRelativePath(Transform? root, string? relativePath)
        {
            return !root ? null :
                root is null ? null :
                relativePath == null ? null :
                relativePath == "" ? root :
                root.Find(relativePath);
        }

        public static Transform? FromAbsolutePath(string? absolutePath)
        {
            return absolutePath == null ? null :
                GameObject.Find(absolutePath).transform;
        }

        public static Transform? FromAbsolutePath(string? absolutePath, Scene scene)
        {
            if (absolutePath == null)
            {
                return null;
            }
            var relativeIndex = absolutePath.IndexOf("/", StringComparison.Ordinal);
            if (relativeIndex == -1)
            {
                return scene.GetRootGameObjects().FirstOrDefault(obj => obj.name == absolutePath)?.transform;
            }
            var rootName =  absolutePath[..relativeIndex];
            var relativePath = absolutePath[(relativeIndex + 1)..];
            return scene.GetRootGameObjects()
                .Where(obj => obj.name == rootName)
                .Select(obj => FromRelativePath(obj.transform, relativePath))
                .FirstOrDefault();
        }
    }
}
