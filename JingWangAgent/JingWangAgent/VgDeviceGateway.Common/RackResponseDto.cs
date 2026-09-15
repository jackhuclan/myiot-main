namespace VgDeviceGateway.Devices.Common
{
    public class ExternalRackQueryReq
    {
        /// <summary>
        /// 所属仓库编号
        /// </summary>
        public virtual string WareHouseCode { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary
        public virtual string Code { get; set; }

        /// <summary>
        /// 状态 0-禁用 1-启用
        /// </summary>
        public virtual int Status { get; set; }
    }

    public class ResponseDto<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { set; get; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { set; get; } = "";

        /// <summary>
        /// Data
        /// </summary>
        public T Data { set; get; }
    }

    public class ExternalRackDto
    {
        /// <summary>
        /// 所属仓库编号
        /// </summary>
        public virtual string? WareHouseCode { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 内点
        /// </summary>
        public virtual string? InnerPoint { get; set; }

        /// <summary>
        /// 外点
        /// </summary>
        public virtual string? OutPoint { get; set; }

        /// <summary>
        /// 料仓信息
        /// </summary>
        public List<SiloInfo> ExternalSiloInfo { get; set; } = new List<SiloInfo>();

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }

    public class SiloInfo
    {
        /// <summary>
        /// 料仓编号
        /// </summary>
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 空层
        /// </summary>
        public string? EmptySilo { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        public string? Size { get; set; }

        /// <summary>
        /// 层数
        /// </summary>

        public int? FloorCount { get; set; }

        /// <summary>
        /// 料仓载料信息
        /// </summary>
        public List<ExternalSiloDetailDto> SiloDetails { get; set; } = new List<ExternalSiloDetailDto>();
    }

    public class ExternalSiloDetailDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 板料编码
        /// </summary>
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 层号
        /// </summary>
        public virtual int? FloorNum { get; set; }

        /// <summary>
        /// 板料类型 30100-生料 40100-熟料
        /// </summary>
        public virtual string? ProductStatus { get; set; }

        /// <summary>
        /// 每叠块数
        /// </summary>
        public virtual string? Pcs { get; set; }

        /// <summary>
        /// 板宽
        /// </summary>
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        public virtual decimal? PinOffset { get; set; }
    }

    public class QueryLocationResponse
    {
        /// <summary>
        /// 库位编号
        /// </summary>
        public string? Code { get; set; }

        public string? SiloCode { get; set; }

        public string? DeviceId { get; set; }
        public string? PositionCode { get; set; }
        /// <summary>
        /// 大车内点
        /// </summary>
        public string? FeedAGVInnerPoint { get; set; }
        /// <summary>
        /// 大车外点
        /// </summary>
        public string? FeedAGVOutputPoint { get; set; }

        /// <summary>
        /// 大车休息点
        /// </summary>
        public string? FeedAGVRestPoint { get; set; }

        /// <summary>
        /// 小车内点
        /// </summary>
        public string? TransAGVInnerPoint { get; set; }

        /// <summary>
        /// 小车外点
        /// </summary>
        public string? TransAGVOutputPoint { get; set; }

        /// <summary>
        /// 小车休息点
        /// </summary>
        public string? TransAGVRestPoint { get; set; }
    }

}
