using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment
{
    public class DeviceDto
    {
        /// <summary>
        ///  主键
        ///</summary>
        public int Id { get; set; }

        /// <summary>
        /// 编号 
        ///</summary>   
        public string? Code { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public int? DeviceTypeId { get; set; }
        /// <summary>
        /// 设备类型代码
        /// </summary>
        public string? DeviceTypeCode { get; set; }

        /// <summary>
        /// 设备供应商
        /// </summary>
        public int? DeviceVendorId { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public virtual string? DeviceBrand { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? DeviceSpec { get; set; }

        /// <summary>
        /// 工位，工作站
        /// </summary>
        public int? WorkStationId { get; set; }
        /// <summary>
        /// 维护周期天
        /// </summary>
        public int? MaintainPeriodDays { get; set; }

        /// <summary>
        /// 设备投产日期
        /// </summary>
        public DateTime? ProductionTime { get; set; }

        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 设备参数
        /// </summary>
        public virtual string? Parameters { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }
        /// <summary>
        /// 轴数
        /// </summary>
        public virtual int? SpindleNum { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual DeviceKind? DeviceKind { get; set; }

        /// <summary>
        /// 交互位置
        /// </summary>
        public virtual InteractionPosition? InteractionPosition { get; set; }

        /// <summary>
        /// 自动机器还是手动机器
        /// </summary>
        public virtual bool? IsAuto { get; set; }


        /// <summary>
        /// 生料库位
        /// </summary>
        public virtual List<string>? RawLocationCodes { get; set; }


        /// <summary>
        /// 熟料库位
        /// </summary>
        public virtual List<string>? ClinkerLocationCodes { get; set; }


        /// <summary>
        /// 最大板长
        /// </summary>
        public virtual int? MaxBoardLength { get; set; }

    }
}
