using System;
using UniGLTF;
using UnityEngine;
using UniVRM10;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.UniVRM10
{
    public static class VRM1Exporter
    {
        [Obsolete]
        public static byte[] ExportVRM1(Vrm10Instance vrm10Instance)
        {
            var settings = ScriptableObject.CreateInstance<VRM10ExportSettings>();
            try
            {
                return ExportVRM1(vrm10Instance, settings);
            }
            finally
            {
                Object.DestroyImmediate(settings);
            }
        }
        
        public static byte[] ExportVRM1(Vrm10Instance vrm10Instance, VRM10ExportSettings settings)
        {
            using var arrayManager = new NativeArrayManager();
            // Based on Vrm10Exporter.Export()

            // ヒエラルキーからジオメトリーを収集
            var converter = new ModelExporter();
            var model = converter.Export(settings.MeshExportSettings, arrayManager, vrm10Instance.gameObject);

            // 右手系に変換
            model.ConvertCoordinate(VrmLib.Coordinates.Vrm1);

            var exporter10 = new Vrm10Exporter(
                settings.MeshExportSettings,
                // Use RuntimeTextureSerializer, because EditorTextureSerializer may destroy asset import settings
                textureSerializer: new RuntimeTextureSerializer()
            );
            var option = new VrmLib.ExportArgs
            {
                sparse = settings.MorphTargetUseSparse,
            };
            exporter10.Export(vrm10Instance.gameObject, model, converter, option, vrm10Instance.Vrm.Meta);

            return exporter10.Storage.ToGlbBytes();
        }
    }
}