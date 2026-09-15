using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class AddOrUpdateExternalWorkOrderReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 工单编码
        /// </summary>
        public virtual string? Code { get; set; }
        /// <summary>
        /// 来源类型
        /// 订单或者库存,
        /// 后端直接赋值 客户订单
        /// </summary>
        public virtual string? OrderSource { get; set; }

        /// <summary>
        /// 来源单据
        /// </summary>
        [Required]
        public virtual string? SourceCode { get; set; }
        /// <summary>
        /// 产品名称
        /// 无值时，用ItemCode填写
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [Required]
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public virtual string? IncodeNumber { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelLength { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// 未给值时，使用预存的系统单位 code==Panel
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 生产数量,pcs 片数
        /// 使用此字段 填写内部工单表的QuantityChanged
        /// </summary>
        [Required]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 待排产叠数，
        /// 
        /// </summary>
        [Required]
        public virtual decimal? WadCount { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string? ClientName { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public virtual string? ClientCode { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// 叠板层数，
        /// 板料的默认层数，实际个别Panel的层数可能小于这个值
        /// </summary>
        [Required]
        public virtual decimal? PanelCount { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [Required]
        public virtual int? LayerNum { get; set; }

        /// <summary>
        /// 孔数，
        /// 用于预估任务的耗时，无值时按10000计算
        /// </summary>
        public virtual decimal? DrillCount { get; set; } = 10000;

        /// <summary>
        /// 产品大类code，
        /// P01,或者其他
        /// </summary>
        public virtual string? ProductCategoryCode { get; set; }

        /// <summary>
        /// 工艺路线编号，
        /// A0001,或者其他
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }

        /// <summary>
        /// 建议机台数
        /// </summary>
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        public virtual string? SpecGroup { get; set; }

        /// <summary>
        /// 转换前文件路径
        /// </summary>
        public virtual string? BeforeDrillFilePath { get; set; }

        /// <summary>
        /// 转换后文件路径
        /// </summary>
        public virtual string? AfterDrillFilePath { get; set; }

        /// <summary>
        /// 板料二维码
        /// </summary>
        public virtual string? BarCode { get; set; }

        /// <summary>
        /// 传入的钻机的设备代号，可选
        /// 必须都在内部系统，如果有任何一条不存在，则标记处理失败；
        /// </summary>
        public virtual List<string> DeviceCodes { get; set; } = new List<string>();

        /// <summary>
        /// 板料号，
        /// 可选，
        /// 如果给板料时，那么必填三个字段 板料号、物料号、pcs数
        /// </summary>
        public virtual List<ExternalAddOrUpdatePanelReq> Panels { get; set; } = new List<ExternalAddOrUpdatePanelReq>() { };



        /// <summary>
        /// 工序编码(博敏需求)
        /// </summary>
        public string? ProcessCode { get; set; }
    }
}
