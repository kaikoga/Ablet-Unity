using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API.V1.Building;
using Ablet.ErrorReporting.Ephemeral;
using Ablet.Models.Serialized;
using Ablet.Registries;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.ErrorReporting.Serialized
{
    static class ErrorReportSerializer
    {
        public static IEnumerable<SerializedBuildReport> Export(IBuildContext context)
        {
            var artifact = context.GetOrCreateArtifact<ErrorReportArtifact>();
            var entrypointRef = SerializedEntrypointReference.FromContext(context);

            return artifact.Logs.GroupBy(log => log.Layer?.Id)
                .Select(group => new SerializedBuildReport(
                    entrypointRef,
                    group.Key ?? "",
                    new SerializedErrorReport
                    {
                        log = group.Select(log => log.Export()).ToList()
                    }
                ));
        }

        static SerializedErrorLog Export(this ErrorLog log)
        {
            string BuildStackTrace(Exception e) => e.InnerException is { } inner ? $"{e.StackTrace}\n{BuildStackTrace(inner)}" : e.StackTrace;

            var (message, exception) = log.Exception switch
            {
                { } ex => (ex.Message, new SerializedExceptionLog
                {
                    message = ex.Message,
                    stacktrace = $"{BuildStackTrace(ex)}\n{log.ExtraStackTrace}"
                }),
                _ => (log.Message, new SerializedExceptionLog())
            };
            return new SerializedErrorLog
            {
                kind = log.Kind,
                message = message,
                interests = log.SerializedInterests.Select(ExportAsObjectChain).ToArray(),
                exception = exception
            };
        }

        static SerializedObjectChain ExportAsObjectChain(this SerializedObjectReference objectReference)
        {
            if (ErrorReportContextScope.Current?.ObjectChainRepository is { } chainRepository)
            {
                return new SerializedObjectChain
                {
                    source = objectReference,
                    elements = chainRepository.ToChain(objectReference).Select(Export).ToArray()
                };
            }
            return new SerializedObjectChain
            {
                source = objectReference,
                elements = new []
                {
                    new SerializedObjectChainElement(objectReference, "")
                }
            };
        }

        static SerializedObjectChainElement Export(ObjectChainElement element)
        {
            return new SerializedObjectChainElement(element.Obj, element.Layer?.Id ?? "");
        }

        public static IEnumerable<SerializedObjectReference> Export(IEnumerable<Object> interests, GameObject rootObject) =>
            interests.Select(context => Export(context, rootObject));

        public static SerializedObjectReference Export(Object interest, GameObject rootObject)
        {
            if (!interest)
            {
                return new SerializedObjectReference();
            }
            var type = interest.GetType().AssemblyQualifiedName ?? "";
            var name = interest.name;
            foreach (var assetResolver in AssetResolverRegistry.Instance.All())
            {
                if (assetResolver.TryGetPath(interest, rootObject, out var path))
                {
                    return new SerializedObjectReference
                    {
                        source = assetResolver.Source,
                        path = path,
                        type = type,
                        name = name
                    };
                }
            }
            return new SerializedObjectReference
            {
                source = "",
                path = "",
                type = type,
                name = name
            };
        }
    }
}
