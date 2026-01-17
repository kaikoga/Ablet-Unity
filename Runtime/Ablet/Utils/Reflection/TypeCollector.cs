using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ablet.API.V1.Attributes;

namespace Ablet.Utils.Reflection
{
    public static class TypeCollector
    {
        static readonly Lazy<Type[]> LazyAllTypes = new Lazy<Type[]>(() => AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).ToArray());

        static Type[] AllTypes => LazyAllTypes.Value;
            
        static TypeCollector()
        {
            var initializeOnLoadMethods = AllTypes
                .SelectMany(type =>
                {
                    try
                    {
                        return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    }
                    catch (TypeLoadException)
                    {
                        return Enumerable.Empty<MethodInfo>();
                    }
                })
                .Where(method =>
                {
                    try
                    {
                        return method.GetCustomAttribute<AbletInitializeOnLoadMethodAttribute>() != null;
                    }
                    catch (TypeLoadException)
                    {
                        return false;
                    }
                }); 
            foreach (var method in initializeOnLoadMethods)
            {
                method?.Invoke(null, Array.Empty<object>());
            }
        }

        public static IEnumerable<Type> Collect<TMarker>()
            where TMarker : Attribute
        {
            return AllTypes.Where(type => type.GetCustomAttribute<TMarker>() != null);
        }

        public static Type? ByAssemblyQualifiedName(string assemblyQualifiedName)
        {
            return AllTypes.FirstOrDefault(type => type.AssemblyQualifiedName == assemblyQualifiedName);
        }
    }

}
