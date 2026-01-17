using System.IO;
using UniVRM10;

namespace Ablet.Builtin.UniVRM10
{
    public static class VRM1FileExporter
    {
        public static void ExportVRM1File(Vrm10Instance vrm10Instance, string filePath)
        {
            ExportVRM1File(VRM1Exporter.ExportVRM1(vrm10Instance), filePath);
        }
        
        public static void ExportVRM1File(byte[] bytes, string filePath)
        {
            File.WriteAllBytes(filePath, bytes);
        }
    }
}
