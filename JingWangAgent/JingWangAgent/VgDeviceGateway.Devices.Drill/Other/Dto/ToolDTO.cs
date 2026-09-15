namespace VgDeviceGateway.Devices.Drill.Other.Dto
{
    [Serializable]
    public class ToolDTO
    {
        public int toolId { get; set; }//T
        public float toolDiameter { get; set; }//D
        public int toolType { get; set; }//E
        public float spindleSpeed { get; set; }//S
        public float infeedZAxis { get; set; }//F
        public float retractZAxis { get; set; }//R
        public int waitTime { get; set; }//A
        public float workPlaneAdjustment { get; set; }//Z
        public int toolLife { get; set; }//N
        public int drillToolLife { get; set; }//B
        public float routerToolLife { get; set; }//C
        public float toolLifeMonitoring { get; set; }//
        public float compensationDiameter { get; set; }//
        public float routingFeedRate { get; set; }//V
        public float routerWear { get; set; }//W
        public float circularRoutingFeedRate { get; set; }//
        public List<bool> toolFunctions { get; set; } = Enumerable.Range(1, 12).Select(f => false).ToList();//
        public List<float> drillMethod { get; set; } = Enumerable.Range(1, 6).Select(f => new float()).ToList();//
        public List<int> magazines { get; set; } = new List<int>();//
    }
}
