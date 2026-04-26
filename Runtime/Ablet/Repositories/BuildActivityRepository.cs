using System;
using System.Collections.Generic;
using Ablet.Models;
using Ablet.Models.Serialized;
using UnityEngine;

namespace Ablet.Repositories
{
#if UNITY_EDITOR
    class EditorBuildActivityRepository : UnityEditor.ScriptableSingleton<EditorBuildActivityRepository>
    {
        public List<SerializedBuildActivity> editorBuildActivity = new List<SerializedBuildActivity>();
    }
#endif

    public class BuildActivityRepository
    {
        public static readonly BuildActivityRepository Instance = new BuildActivityRepository();

        public bool IsRecording { get; private set; }
#if UNITY_EDITOR
        List<SerializedBuildActivity> BuildActivities => EditorBuildActivityRepository.instance.editorBuildActivity;
#else
        List<SerializedBuildActivity> runtimeBuildActivities = new List<SerializedBuildActivity>();
        List<SerializedBuildActivity> BuildActivities => runtimeBuildActivities;
#endif

        public SerializedBuildActivity? GetLastBuildActivity() => BuildActivities.Count > 0 ? BuildActivities[^1] : null;

        BuildActivityRepository()
        {
        }

        void Start(string catalystId)
        {
            BuildActivities.Add(new SerializedBuildActivity(catalystId));
            IsRecording = true;
        }

        public void Record(AbletLayer layer, long ticks)
        {
            // Debug.Log($"[Ablet] <{layer.DisplayName}> layer processed in {ticks / 10000f}ms");
            var layerId = layer.Id;
            BuildActivities[^1].layers.Add(new SerializedLayerActivity(layerId, ticks));
        }

        internal RecordScope CreateScope(string catalystId)
        {
            Instance.Start(catalystId);
            return new RecordScope();
        }

        internal class RecordScope : IDisposable
        {
            public void Dispose()
            {
                Instance.IsRecording = false;
            }
        } 
    }
}
