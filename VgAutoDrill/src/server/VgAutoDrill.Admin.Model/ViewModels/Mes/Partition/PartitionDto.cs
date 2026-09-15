using VgAutoDrill.Admin.Model.ViewModels.Mes.PartitionSetting;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Partition
{
    public class PartitionDto : BaseDto
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 工位位置
        /// </summary>
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public virtual string? Charge { get; set; }

        /// <summary>
        /// 父对象Id(0表示是根对象)
        ///</summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 所有层级父节点
        /// </summary>
        public virtual string? Ancestors { get; set; }

        /// <summary>
        /// 预约AGV
        /// </summary>
        public virtual string? PreBookAgv { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public virtual DateTime? PreBookTime { get; set; }

        /// <summary>
        /// 分区类别,默认0未知，1私有分区，2公共分区
        /// </summary>
        public virtual PartitionKind PartitionKind { get; set; }

        /// <summary>
        /// 料仓类型
        /// </summary>
        public virtual TransportationKind TransportationKind { get; set; }

        /// <summary>
        ///最少空位数
        /// </summary>
        public virtual int? MinEmptyLocationNum { get; set; }

        /// <summary>
        ///最少空仓数
        /// </summary>
        public virtual int? MinEmptyBoxNum { get; set; }

        /// <summary>
        ///最多空仓数
        /// </summary>
        public virtual int? MaxEmptyBoxNum { get; set; }

        /// <summary>
        ///熟料最少转出数量
        /// </summary>
        public virtual int? MinDrilledTrackOutNum { get; set; }

        /// <summary>
        ///料仓中没有钻机中相同的料号时是否立即转出
        /// </summary>
        public virtual bool? IsAutoTrackOutDrilledSilo { get; set; }

        /// <summary>
        ///生料转出超时时间
        /// </summary>
        public virtual int? RawTrackOutTimeOutTime { get; set; }

        /// <summary>
        ///首件最少转出数量
        /// </summary>
        public virtual int? MinFirstTrackOutNum { get; set; }

        /// <summary>
        /// 熟料转出超时(分钟，默认10分钟)
        /// </summary>
        public virtual int? DrilledTrackOutTimeMinutes { get; set; } = 10;

        /// <summary>
        /// 转运任务是否由服务端控制(默认：否)
        /// </summary>
        public virtual bool? IsControlledByServer { get; set; } = false;

        /// <summary>
        /// 关联的缓冲区代号
        /// </summary>
        public virtual string? RelatedBufferCode { get; set; }


        public virtual List<PartitionSettingDto>? Settings { get; set; }
    }
}