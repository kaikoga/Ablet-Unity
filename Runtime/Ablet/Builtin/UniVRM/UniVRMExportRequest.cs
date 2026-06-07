using Ablet.API;
using Ablet.DataObjects;

namespace Ablet.Builtin.UniVRM
{
    public class UniVRMExportRequest : IAbletInput
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
