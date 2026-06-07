using System;
using UnityEngine;
using VRM;
using Object = UnityEngine.Object;
using VRMEditorExporter = Ablet.Builtin.UniVRM.Exporter.VRMEditorExporter;

namespace Ablet.Builtin.UniVRM
{
    public static class VRM0Exporter
    {
        [Obsolete]
        public static byte[] ExportVRM0(VRMMeta vrmMeta)
        {
            var settings = ScriptableObject.CreateInstance<VRMExportSettings>();
            try
            {
                return ExportVRM0(vrmMeta, settings);
            }
            finally
            {
                Object.DestroyImmediate(settings);
            }
        }
        
        public static byte[] ExportVRM0(VRMMeta vrmMeta, VRMExportSettings settings)
        {
            return VRMEditorExporter.Export(vrmMeta.gameObject, vrmMeta.Meta, settings);
        }
    }
}