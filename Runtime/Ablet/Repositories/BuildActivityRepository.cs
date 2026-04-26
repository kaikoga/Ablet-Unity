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

#if UNITY_EDITOR
        List<SerializedBuildActivity> BuildActivities => EditorBuildActivityRepository.instance.editorBuildActivity;
#else
        List<SerializedBuildActivity> runtimeBuildActivities = new List<SerializedBuildActivity>();
        List<SerializedBuildActivity> BuildActivities => runtimeBuildActivities;
#endif

        BuildActivityRepository()
        {
        }

        public void Restart(string catalystId)
        {
            BuildActivities.Add(new SerializedBuildActivity(catalystId));
        }

        public void Record(AbletLayer layer, long ticks)
        {
            Debug.Log($"[Ablet] <{layer.DisplayName}> layer processed in {ticks / 10000f}ms");
            var layerId = layer.Id;
            BuildActivities[^1].layers.Add(new SerializedLayerActivity(layerId, ticks));
        }
    }
}
