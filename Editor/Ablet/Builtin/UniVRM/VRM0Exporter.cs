using UnityEngine;
using VRM;
using VRMEditorExporter = Ablet.Builtin.UniVRM.Exporter.VRMEditorExporter;

namespace Ablet.Builtin.UniVRM
{
    public static class VRM0Exporter
    {
        public static byte[] ExportVRM0(VRMMeta vrmMeta)
        {
            var settings = ScriptableObject.CreateInstance<VRMExportSettings>();
            try
            {
                return VRMEditorExporter.Export(vrmMeta.gameObject, vrmMeta.Meta, settings);
            }
            finally
            {
                Object.DestroyImmediate(settings);
            }
        }
    }
}