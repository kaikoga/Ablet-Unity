using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Models;
using Ablet.Registries.Base;
using Ablet.Utils.Reflection;

namespace Ablet.Registries
{
    class ExtensionRegistry : ModelRegistryBase<IAbletExtension, AbletExtension>
    {
        public static readonly ExtensionRegistry Instance = new ExtensionRegistry();

        Dictionary<(Type, Type), AbletExtension[]> _byForType = new Dictionary<(Type, Type), AbletExtension[]>();

        ExtensionRegistry()
        {
            Collect(new ModelCollector<IAbletExtension, AbletExtensionAttribute, AbletExtension>(def => new AbletExtension(def)));
        }

        public IEnumerable<AbletExtension> ForType<T>(Type defType)
            where T : IAbletExtension
        {
            return ForTypeInternal(typeof(T), defType);
        }

        IEnumerable<AbletExtension> ForTypeInternal(Type extType, Type defType)
        {
            if (_byForType.TryGetValue((extType, defType), out var value))
            {
                return value;
            }
            value = Unordered()
                .Where(ext => defType == ext.ForType)
                .Where(ext => extType.IsAssignableFrom(ext.DefType))
                .ToArray();
            _byForType.Add((extType, defType), value);
            return value;
        }

        public override IEnumerable<AbletExtension> All() => Unordered();
    }
}
