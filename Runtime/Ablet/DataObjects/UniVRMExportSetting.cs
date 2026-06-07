using System;

namespace Ablet.DataObjects
{
    // VRM.VRMExportSettings
    [Serializable]
    public class UniVRMExportSetting
    {
        public bool forceTPose = false;

        public bool poseFreeze = true;

        public bool freezeMeshUseCurrentBlendShapeWeight = true;

        public bool useSparseAccessor = false;

        public bool onlyBlendshapePosition = false;

        public bool reduceBlendshape = false;

        public bool reduceBlendshapeClip = false;

        public bool divideVertexBuffer = false;

        public bool keepVertexColor = false;

        public bool keepAnimation = false;
    }
}