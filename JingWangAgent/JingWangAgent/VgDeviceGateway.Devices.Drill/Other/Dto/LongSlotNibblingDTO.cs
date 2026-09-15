/*
 * 长槽蚕食实体类，ATP解析的子类
 */

namespace VgDeviceGateway.Devices.Drill.Other.Dto
{
    public class LongSlotNibblingDTO
    {
        public int toolId { get; set; }
        public int use { get; set; }
        public float minD { get; set; }
        public float maxD { get; set; }
        public float type { get; set; }
        public float TMET_Roughn { get; set; }
        public float InfeedFirst { get; set; }
        public float InfeedFast { get; set; }
        public float InfeedSlow { get; set; }
        public float TIN_Roughn { get; set; }
    }
}
