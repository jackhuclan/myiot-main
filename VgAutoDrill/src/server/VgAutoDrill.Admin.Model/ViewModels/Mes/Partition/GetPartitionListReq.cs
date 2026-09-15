using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Partition
{
    public class GetPartitionListReq : Page
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }

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

        public virtual int? Status { get; set; }
        public virtual string? PreBookAgv { get; set; }

        /// <summary>
        /// 分区类别,默认0未知，1私有分区，2公共分区
        /// </summary>
        public virtual List<PartitionKind>? PartitionKinds { get; set; }

        /// <summary>
        /// 料仓类型
        /// </summary>
        public virtual TransportationKind? TransportationKind { get; set; }

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
    }
}