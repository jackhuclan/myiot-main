using VgAutoDrill.Fundation.Iot;

namespace VgDeviceGateway.Devices.Common
{
    public class GetItemListReq
    {
        public virtual string? Code { get; set; }
        public QueryOrderByEnum? QueryOrderBy { get; set; }
    }
}
