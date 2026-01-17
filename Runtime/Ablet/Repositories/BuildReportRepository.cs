using System.Collections.Generic;
using System.Linq;
using Ablet.API;
using Ablet.Models.Serialized;

namespace Ablet.Repositories
{
#if UNITY_EDITOR
    class EditorBuildReportRepository : UnityEditor.ScriptableSingleton<EditorBuildReportRepository>
    {
        public List<SerializedBuildReport> editorBuildReports = new List<SerializedBuildReport>();
    }
#endif

    public class BuildReportRepository
    {
        public static readonly BuildReportRepository Instance = new BuildReportRepository();

#if UNITY_EDITOR
        List<SerializedBuildReport> BuildReports => EditorBuildReportRepository.instance.editorBuildReports;
#else
        List<SerializedBuildReport> runtimeBuildReports = new List<SerializedBuildReport>();
        List<SerializedBuildReport> BuildReports => runtimeBuildReports;
#endif

        public IEnumerable<SerializedEntrypointReference> EntrypointReferences() => BuildReports.Select(x => x.entrypointRef).Distinct();
        public bool IsMultiScene { get; private set; }

        public delegate void ChangedHandler(bool notifyUser, SerializedEntrypointReference? withEntrypointRef);
        public event ChangedHandler? Changed;
       
        void OnChange(bool notifyUser, SerializedEntrypointReference? withEntrypointRef)
        {
            IsMultiScene = EntrypointReferences().Select(entrypointRef => entrypointRef.SceneName).Distinct().Count() > 1;
            Changed?.Invoke(notifyUser, withEntrypointRef);
        }

        BuildReportRepository()
        {
            IsMultiScene = EntrypointReferences().Select(entrypointRef => entrypointRef.SceneName).Distinct().Count() > 1;
        }

        public void Add(SerializedBuildReport buildReport)
        {
            var discriminator = buildReport.payload is IAbletSerializedBuildReportPayload.WithDiscriminator d ? d.Discriminator : null;
            var hasPriority = buildReport.payload is IAbletSerializedBuildReportPayload.WithPriority p ? p.Priority : (int?)null;

            for (var i = 0; i < BuildReports.Count; i++)
            {
                var report = BuildReports[i];
                if (report.entrypointRef != buildReport.entrypointRef
                    || report.layerId != buildReport.layerId)
                {
                    continue;
                }
                if (discriminator is { }
                    && report.payload is IAbletSerializedBuildReportPayload.WithDiscriminator withDiscriminator
                    && withDiscriminator.Discriminator != discriminator)
                {
                    continue;
                }
                
                if (hasPriority is { } priority
                    && report.payload is IAbletSerializedBuildReportPayload.WithPriority withPriority
                    && withPriority.Priority > priority)
                {
                    return;
                }
                BuildReports[i] = buildReport;
                OnChange(true, buildReport.entrypointRef);
                return;
            }
            BuildReports.Add(buildReport);
            OnChange(true, buildReport.entrypointRef);
        }

        public void Clear<T>(SerializedEntrypointReference entrypointRef)
        where T : IAbletSerializedBuildReportPayload
        {
            var buildReports = BuildReports.Where(report => report.entrypointRef != entrypointRef || report.payload.GetType() != typeof(T)).ToList();
            BuildReports.Clear();
            BuildReports.AddRange(buildReports);
            OnChange(false, null);
        }

        public IEnumerable<SerializedBuildReportList> ForEntrypoints(SerializedEntrypointReference[] entrypointRefs)
        {
            return entrypointRefs
                .Select(entrypointRef => new SerializedBuildReportList
                {
                    entrypointRef = entrypointRef,
                    buildReports = BuildReports.Where(report => entrypointRef == report.entrypointRef).ToList()
                });
        }

        public void Clear()
        {
            BuildReports.Clear();
            OnChange(false, null);
        }
    }

}
