namespace VgDeviceGateway.Devices.Common
{
    public static class StdAgvStatus
    {
        /// <summary>
        /// "连接未建立的状态"
        /// </summary>
        public const string UNKNOWN = "UNKNOWN";

        /// <summary>
        /// 连接已经建立,车辆不处于error状态但是也不能执行任务
        /// </summary>
        public const string UNAVAILABLE = "UNAVAILABLE";

        /// <summary>
        /// 没有执行动作也没有执行移动的状态
        /// </summary>
        public const string IDLE = " IDLE";

        /// <summary>
        /// 正在执行动作或者移动
        /// </summary>
        public const string EXECUTING = " EXECUTING";

        /// <summary>
        ///  车辆状态出错
        /// </summary>
        public const string ERROR = "ERROR";

        /// <summary>
        /// 充电状态
        /// </summary>
        public const string CHARGING = "CHARGING";

        /// <summary>
        /// 避障状态
        /// </summary>
        public const string PAUSE = "PAUSE";
    }
}
