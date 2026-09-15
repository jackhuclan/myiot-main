/*
 * 刀具公差表实体类，ATP解析的子类
 */

namespace VgDeviceGateway.Devices.Drill.Other.Dto
{
    public class ToolToleranceTableDTO
    {
        public int tableId { get; set; }
        public float diameter { get; set; }
        public float nDiaTolerance { get; set; }
        public float pDiaTolerance { get; set; }
        public float nLenTolerance { get; set; }
        public float pLenTolerance { get; set; }
        public float warnOfDeviation { get; set; }
        public float stopOfDeviation { get; set; }
        public float kZ1 { get; set; }
        public float kZ2 { get; set; }
        public float kZ3 { get; set; }
        public float kZ4 { get; set; }
        public float kZ5 { get; set; }
        public float kZ6 { get; set; }
    }
}
