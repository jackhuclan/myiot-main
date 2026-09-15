using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement
{
    public class GetScheduleListReq : Page
    {
        public long? Id { get; set; }
        public string? TraceId { get; set; }
        public string? SourceDeviceId { set; get; }

        /// <summary>
        /// 创建时间-开始时间
        /// </summary>
        public DateTime? StartTime { set; get; }

        /// <summary>
        /// 创建时间-结束时间
        /// </summary>
        public DateTime? EndTime { set; get; }
        /// <summary>
        /// 是否已全部下发任务
        /// </summary>
        public virtual bool? IsAllPanelSent { get; set; }
        /// <summary>
        /// 调度任务状态
        /// </summary>
        public virtual List<ScheduledTaskStatus?>? ScheduledTaskStatusList { get; set; } = new();

        /// <summary>
        /// 工艺路线
        /// </summary>
        public virtual List<string?>? RouteCodeList { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        public virtual string? TaskId { get; set; }

        /// <summary>
        /// 设备事件route key 
        ///</summary>
        public virtual string? RoutingKey { get; set; }

        /// <summary>
        /// 是否根据ID倒序
        /// </summary>
        public virtual bool OrderByIDDesc { get; set; } = true;
        /// <summary>
        /// 是否紧急
        /// </summary>
        public virtual int? IsUrgent { get; set; }
        /// <summary>
        /// 是否是辅助设备请求
        /// </summary>
        public virtual bool? IsAuxiliary { get; set; }
        /// <summary>
        /// 是否为先执行
        /// </summary>
        public virtual bool? IsMaster { get; set; }
        public virtual bool? IsBarcodeOk { get; set; }
        public virtual bool? HasAgvSetted { get; set; }
        public virtual bool? HasRawItemSetted { get; set; }
        public virtual long? MasterScheduleId { get; set; }

        /// <summary>
        /// 调度设备
        /// </summary>
        public virtual string? RequireDeviceId { get; set; }

        /// <summary>
        /// 交互方式
        /// </summary>
        public virtual List<InteractionSequence?>? InteractionSequenceList { get; set; }
        public virtual List<DeviceKind?>? RequestDeviceKindList { get; set; }
        /// <summary>
        /// 库位编号
        /// </summary>
        public virtual string? SubDeviceCode { get; set; }

        /// <summary>
        /// 调度开始时间-开始时间
        /// </summary>
        public DateTime? RunningStartTime { set; get; }

        /// <summary>
        /// 调度开始时间-结束时间
        /// </summary>
        public DateTime? RunningEndTime { set; get; }

        /// <summary>
        /// 料仓编码
        /// </summary>
        public virtual string? AGVPayloadPanels { get; set; }

        /// <summary>
        /// 是否来自大屏页面
        /// </summary>
        public virtual bool IsFromBigScreenWeb { get; set; }

        /// <summary>
        /// 数据排序方式 
        /// 1、创建时间正序
        /// 2、创建时间倒序
        /// 3、编码正序
        /// 4、编码倒序
        /// 5、Id正序
        /// 6、Id倒序
        /// </summary>
        public QueryOrderByEnum? QueryOrderBy { get; set; }
    }
}
