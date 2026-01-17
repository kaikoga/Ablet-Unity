using System.IO;
using VRM;

namespace Ablet.Builtin.UniVRM
{
    public static class VRM0FileExporter
    {
        public static void ExportVRM0File(VRMMeta vrmMeta, string filePath)
        {
            ExportVRM0File(VRM0Exporter.ExportVRM0(vrmMeta), filePath);
        }

        public static void ExportVRM0File(byte[] bytes, string filePath)
        {
            File.WriteAllBytes(filePath, bytes);
        }
    }
}
