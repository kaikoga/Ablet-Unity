using System;
using Ablet.Utils;
using UnityEngine;

namespace Ablet.DataObjects
{
    [Serializable]
    public abstract class SerializedReferencePath<T>
    where T : Component
    {
        [SerializeField] bool hasValue;
        [SerializeField] string relativePath = "";

        public string? RelativePath
        {
            get => hasValue ? relativePath : null;
            set {
                if (value != null)
                {
                    hasValue = true;
                    relativePath = value;
                }
                else
                {
                    hasValue = false;
                    relativePath = "";
                }
            } 
        }

        public T? ResolveNow(Transform transform) => SerializedReferencePath.ResolveNow<T>(transform, relativePath);
        public T? ResolveFromRootObject(GameObject rootObject) => SerializedReferencePath.ResolveFromRootObject<T>(rootObject, relativePath);
    }

    [Serializable]
    public class SerializedTransformPath : SerializedReferencePath<Transform>
    {
        public static SerializedTransformPath OfRelativeObject(Transform transform)
        {
            return new SerializedTransformPath
            {
                RelativePath = SerializedReferencePath.RelativePath(transform, transform)
            };
        }

        public static SerializedTransformPath OfRootObject(GameObject rootObject, Transform transform) =>
            new SerializedTransformPath
            {
                RelativePath = SerializedReferencePath.RelativePathFromRootObject(rootObject, transform)
            };
    }

    static class SerializedReferencePath
    {
        internal static T? ResolveNow<T>(Transform transform, string relativePath)
            where T : Component
        {
            if (string.IsNullOrEmpty(relativePath)) return null;
            return AbletFacade.TryGetRootObjectFor(transform, out var rootObject)
                ? ResolveFromRootObject<T>(rootObject, relativePath)
                : null;
        }

        internal static string? RelativePath<T>(Transform transform, T obj)
            where T : Component
        {
            return AbletFacade.TryGetRootObjectFor(transform, out var rootObject)
                ? RelativePathFromRootObject(rootObject, obj)
                : null;
        }
        
        internal static T? ResolveFromRootObject<T>(GameObject rootObject, string relativePath)
            where T : Component
        {
            return string.IsNullOrEmpty(relativePath)
                ? null
                : AbletRuntimeUtil.FromRelativePath(rootObject.transform, relativePath)?.GetComponent<T>();
        }

        internal static string RelativePathFromRootObject<T>(GameObject rootObject, T obj)
            where T : Component
        {
            return AbletRuntimeUtil.RelativePath(rootObject.transform, obj.transform);
        }
    }
}