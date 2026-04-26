using System.Text;
using Ablet.Registries;
using Ablet.Repositories;
using UnityEditor;
using UnityEngine;

namespace Ablet.Debugging.View.Windows
{
    class DebugBuildActivityWindow : EditorWindow
    {
        [MenuItem("Tools/Ablet/Debug/Debug Build Activity", false, 101)]
        static void ShowWindow()
        {
            (GetWindow<DebugBuildActivityWindow>() ?? CreateInstance<DebugBuildActivityWindow>()).Show();
        }

        readonly StringBuilder _sb = new StringBuilder();
        
        void OnGUI()
        {
            if (!BuildActivityRepository.Instance.IsRecording
                && BuildActivityRepository.Instance.GetLastBuildActivity() is { } activity)
            {
                _sb.Clear();
                _sb.AppendLine(activity.catalystId);
                foreach (var layer in activity.layers)
                {
                    var layerId = layer.layerId;
                    var layerName = LayerRegistry.Instance.TryGetById(layer.layerId, out var layerData)
                        ? layerData.DisplayName
                        : "unknown";
                    _sb.AppendLine($"{layerName} : {layer.ticks / 10000f}ms {layerId}");
                }
            }
            GUILayout.TextArea(_sb.ToString());
        }
    }
}
