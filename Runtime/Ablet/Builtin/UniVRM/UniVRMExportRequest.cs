using Ablet.API.V1.Attributes;
using Ablet.DataObjects;

namespace Ablet.Builtin.UniVRM
{
    [AbletPlatform]
    public class UniVRMExportRequest
    {
        public readonly UniVRMExportSetting Setting;
        
        public UniVRMExportRequest(UniVRMExportSetting setting)
        {
            Setting = setting;
        }
        
        public UniVRMExportRequest() : this(new UniVRMExportSetting())
        {
        }
    }
}
