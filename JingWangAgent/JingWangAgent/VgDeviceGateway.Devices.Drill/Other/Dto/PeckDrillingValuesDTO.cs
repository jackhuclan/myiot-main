/*
 * 分段钻实体类，ATP解析的子类
 */

namespace VgDeviceGateway.Devices.Drill.Other.Dto
{
    public class PeckDrillingValuesDTO
    {
        public int toolNumber { get; set; }
        public int partialStrokeNumber { get; set; }
        public float lowerPlane { get; set; }
        public float retractionPlane { get; set; }
        public float infeedRate { get; set; }
        public float retractRate { get; set; }
    }
}
