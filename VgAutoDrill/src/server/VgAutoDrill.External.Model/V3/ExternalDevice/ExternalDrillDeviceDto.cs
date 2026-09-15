using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.External.Model.V3.ExternalDevice
{
    public class ExternalDrillDeviceDto
    {
        /// <summary>
        /// 钻机编码
        /// </summary>
        public virtual string? Code { get; set; }
        /// <summary>
        /// 钻机名称
        /// </summary>
        public virtual string? Name { get; set; }
        /// <summary>
        /// 钻机状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }
        /// <summary>
        /// 钻机已提交的任务数量
        /// </summary>
        public virtual int? CommitTask { get; set; }

        /// <summary>
        /// 钻机草稿状态的任务数量
        /// </summary>
        public virtual int? DraftTask { get; set; }

        /// <summary>
        /// 开始打孔的任务编号
        /// </summary>
        public virtual string? DrillingTask { get; set; }

        /// <summary>
        /// 已经分配或者上料的任务编号
        /// </summary>
        public virtual string? WaitWorkTask { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public virtual string? Route { get; set; }

        /// <summary>
        /// 在做任务完成度
        /// </summary>
        public virtual int? Percentage { get; set; }
        /// <summary>
        /// 备料箱buffer是否存在生料
        /// </summary>
        public virtual bool? ExistRawPanel { get; set; }
    }
}
