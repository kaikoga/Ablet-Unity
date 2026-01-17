using System;
using System.Collections.Generic;
using System.Linq;

namespace Ablet.View
{
    class EditorBuildReporterStateRepository : UnityEditor.ScriptableSingleton<EditorBuildReporterStateRepository>
    {
        public List<BuildReporterState> buildReporterStates = new List<BuildReporterState>();
    }

    [Serializable]
    class BuildReporterState
    {
        public string id;
        public bool enabled;

        public BuildReporterState(string id, bool enabled)
        {
            this.id = id;
            this.enabled = enabled;
        }
    }

    class BuildReporterStateRepository
    {
        public static readonly BuildReporterStateRepository Instance = new BuildReporterStateRepository();
        
        public event Action? Changed;

        public IEnumerable<BuildReporterState> All()
        {
            var buildReporterStates = EditorBuildReporterStateRepository.instance.buildReporterStates;
            return BuildReporterRegistry.Instance.All()
                .Select(reporter => buildReporterStates.FirstOrDefault(state => state.id == reporter.Id) ?? new BuildReporterState(reporter.Id, true));
        }

        public bool GetEnabled(string id)
        {
            return All().FirstOrDefault(state => state.id == id)?.enabled ?? true;
        }

        public void SetEnabled(string id, bool value)
        {
            EditorBuildReporterStateRepository.instance.buildReporterStates.RemoveAll(s => s.id == id);
            EditorBuildReporterStateRepository.instance.buildReporterStates.Add(new BuildReporterState(id, value));
            Changed?.Invoke();
        }
    }
}
