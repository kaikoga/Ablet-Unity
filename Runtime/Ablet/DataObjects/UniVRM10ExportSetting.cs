using System;

namespace Ablet.DataObjects
{
    // UniVRM10.VRM10ExportSettings
    [Serializable]
    public class UniVRM10ExportSetting
    {
        public readonly bool reduceBlendshape = false;

        public readonly bool reduceBlendshapeClip = false;

        public bool morphTargetUseSparse = true;

        public bool freezeMesh = true;

        public bool freezeMeshKeepRotation = false;

        public bool freezeMeshUseCurrentBlendShapeWeight = false;
    }
}
