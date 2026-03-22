using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API.V1.Building;
using Ablet.Building;
using Ablet.ErrorReporting.Serialized;
using Ablet.Models;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting.Ephemeral
{
    class ObjectChainRepository
    {
        readonly List<ObjectChainMapping> _mappings = new List<ObjectChainMapping>();

        SerializedObjectReference ToInput(SerializedObjectReference source)
        {
            while (_mappings.FirstOrDefault(v => v.To == source) is { } mapping)
            {
                source = mapping.From;
            }
            return source;
        }

        internal IEnumerable<ObjectChainElement> ToChain(SerializedObjectReference source)
        {
            var mappings = new List<ObjectChainMapping>();
            for (var from = source; _mappings.FirstOrDefault(v => v.To == from) is { } mapping; from = mapping.From)
            {
                mappings.Add(mapping);
            }
            mappings.Reverse();
            for (var to = source; _mappings.FirstOrDefault(v => v.From == to) is { } mapping; to = mapping.To)
            {
                mappings.Add(mapping);
            }
            if (mappings.Count == 0)
            {
                yield return new ObjectChainElement(source, null);
            }
            else
            {
                yield return new ObjectChainElement(mappings[0].From, null);
                foreach (var mapping in mappings)
                {
                    yield return new ObjectChainElement(mapping.To, mapping.Layer);
                }
            }
        }

        internal void Add(IBuildContext context, Object from, Object to)
        {
            if (!from || !to)
            {
                return;
            }
            var rootObject = context.CurrentRootObject;
            var currentLayer = BuildContext.PassScope.CurrentPass?.Layer;
            var refFrom = ErrorReportSerializer.Export(from, rootObject);
            var refTo = ErrorReportSerializer.Export(to, rootObject);
            if (ToInput(refFrom) == refTo)
            {
                throw new InvalidOperationException("Loop detected in object mapping");
            }
            _mappings.Add(new ObjectChainMapping(refFrom, refTo, currentLayer));
        }

        class ObjectChainMapping
        {
            public readonly SerializedObjectReference From;
            public readonly SerializedObjectReference To;
            public readonly AbletLayer? Layer;

            public ObjectChainMapping(SerializedObjectReference from, SerializedObjectReference to, AbletLayer? layer)
            {
                this.From = from;
                this.To = to;
                this.Layer = layer;
            }
        }
    }
}
