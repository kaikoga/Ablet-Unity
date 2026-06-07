using Ablet.API;
using Ablet.DataObjects;

namespace Ablet.Builtin.UniVRM10
{
    public class UniVRM10ExportRequest : IAbletInput
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
