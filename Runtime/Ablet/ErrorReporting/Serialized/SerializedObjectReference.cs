using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ablet.Registries;
using Ablet.Utils.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting.Serialized
{
    [Serializable]
    public class SerializedObjectReference : IEquatable<SerializedObjectReference>
    {
        public string source = "";
        public string path = "";
        public string type = "";
        public string name = "";

        public bool TryResolveRelative(GameObject rootObject, [MaybeNullWhen(false)] out Object obj)
        {
            if (TypeCollector.ByAssemblyQualifiedName(type) is { } t)
            {
                foreach (var assetResolver in AssetResolverRegistry.Instance.All().Where(resolver => resolver.Source == source))
                {
                    if (assetResolver.TryResolve(path, t, name, rootObject, out obj))
                    {
                        return true;
                    }
                }
            }
            obj = null;
            return false;
        }

        public Object? ResolveRelative(GameObject rootObject)
        {
            TryResolveRelative(rootObject, out var obj);
            return obj;
        }

        public bool Equals(SerializedObjectReference? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return source == other.source && path == other.path && type == other.type && name == other.name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((SerializedObjectReference)obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return HashCode.Combine(source, path, type, name);
            // ReSharper enable NonReadonlyMemberInGetHashCode
        }

        public static bool operator ==(SerializedObjectReference? left, SerializedObjectReference? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SerializedObjectReference? left, SerializedObjectReference? right)
        {
            return !Equals(left, right);
        }
    }
}
