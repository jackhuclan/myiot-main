namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class RouteAndProcessInfoByDeviceDto
    {
        /// <summary>
        /// 传入的两个设备是否配对成功
        /// </summary>
        public virtual bool IsMatching { get; set; } = false;

        /// <summary>
        /// 设备所在的工艺路线以及工序
        /// </summary>
        public virtual List<RouteAndProcessListByDevice> RouteAndProcessLists { get; set; } = new List<RouteAndProcessListByDevice>();
    }

    public class RouteAndProcessListByDevice
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public virtual long? ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { set; get; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string? ProcessCode { set; get; }

        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工作站编码
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }
    }
}
