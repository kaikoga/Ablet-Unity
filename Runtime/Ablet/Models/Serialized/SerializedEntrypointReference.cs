using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ablet.API.V1.Building;
using Ablet.Building;
using Ablet.Querying;
using Ablet.Repositories;
using Ablet.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ablet.Models.Serialized
{
    [Serializable]
    public class SerializedEntrypointReference : IEquatable<SerializedEntrypointReference>
    {
        [SerializeField] string scenePath;
        [SerializeField] string path;

        public SerializedEntrypointReference(string scenePath, string path)
        {
            this.scenePath = scenePath;
            this.path = path;
        }

        public static SerializedEntrypointReference From(GameObject entrypointObject)
        {
            if (BuildContext.Current != null)
            {
                throw new InvalidOperationException("SerializedEntrypointReference.From() should not be run inside build, please use SerializedEntrypointReference.FromContext()");
            }
            return new SerializedEntrypointReference(
                entrypointObject.scene.path,
                AbletRuntimeUtil.AbsolutePath(entrypointObject.transform)
            );
        }

        public static SerializedEntrypointReference FromContext(IBuildContext context)
        {
            return FromContext(context.Argument.EntrypointObject, context.CurrentRootObject);
        }

        public static SerializedEntrypointReference FromContext(GameObject entrypointObject, GameObject rootObject)
        {
            var actualEntrypoint = AbletRuntimeUtil.GuessActualEntrypointMaybeCloned(entrypointObject);

            var entrypointScenePath = actualEntrypoint.scene.path;
            var rootObjectScenePath = rootObject.scene.path;
            var activeScenePath = SceneManager.GetActiveScene().path;
            var resolvedScenePath = entrypointScenePath != activeScenePath ? entrypointScenePath : rootObjectScenePath;

            return new SerializedEntrypointReference(
                resolvedScenePath,
                AbletRuntimeUtil.AbsolutePath(actualEntrypoint.transform)
            );
        }

        public string ScenePath => scenePath;
        public string SceneName => System.IO.Path.GetFileNameWithoutExtension(scenePath);
        public string Name => System.IO.Path.GetFileName(path);
        public string FullName => $"{SceneName}:{Name}";
        public string DisplayName => BuildReportRepository.Instance.IsMultiScene ? FullName : Name;

        public bool TryResolve([MaybeNullWhen(false)] out GameObject obj)
        {
            obj = AQueryContext.Immediate.GetLoadedScenes()
                .Where(scene => scene.path == scenePath)
                .FindGameObjects(path)
                .ResolveNow().FirstOrDefault();
            return obj;
        }

        public override string ToString() => DisplayName;

        public bool Equals(SerializedEntrypointReference? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return scenePath == other.scenePath && path == other.path;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((SerializedEntrypointReference)obj);
        }
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return HashCode.Combine(scenePath, path);
            // ReSharper enable NonReadonlyMemberInGetHashCode
        }
        public static bool operator ==(SerializedEntrypointReference? left, SerializedEntrypointReference? right)
        {
            return Equals(left, right);
        }
        public static bool operator !=(SerializedEntrypointReference? left, SerializedEntrypointReference? right)
        {
            return !Equals(left, right);
        }
    }
}
