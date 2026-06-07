using Ablet.API.V1.Attributes;
using Ablet.DataObjects;

namespace Ablet.Builtin.UniVRM10
{
    [AbletPlatform]
    public class UniVRM10ExportRequest
    {
        public readonly UniVRM10ExportSetting Setting;
        
        public UniVRM10ExportRequest(UniVRM10ExportSetting setting)
        {
            Setting = setting;
        }

        public UniVRM10ExportRequest() : this(new UniVRM10ExportSetting())
        {
        }
    }
}
