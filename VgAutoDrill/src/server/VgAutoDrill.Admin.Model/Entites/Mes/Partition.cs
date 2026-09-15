using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 分区表
    ///</summary>
    [SugarTable("t_warehouse")]
    public class Partition : BaseEntityWithTree
    {
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 工位位置
        /// </summary>
        [SugarColumn(ColumnName = "work_station_id")]
        public virtual long? WorkStationId { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        [SugarColumn(ColumnName = "work_station_code")]
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        [SugarColumn(ColumnName = "work_station_name")]
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [SugarColumn(ColumnName = "charge")]
        public virtual string? Charge { get; set; }

        /// <summary>
        /// 预约AGV
        /// </summary>
        [SugarColumn(ColumnName = "pre_book_agv")]
        public virtual string? PreBookAgv { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        [SugarColumn(ColumnName = "pre_book_time")]
        public virtual DateTime? PreBookTime { get; set; }

        /// <summary>
        /// 分区类别,默认0未知，1私有分区，2公共分区
        /// </summary>
        [SugarColumn(ColumnName = "partition_kind")]
        public virtual PartitionKind PartitionKind { get; set; } = PartitionKind.Unknown;

        /// <summary>
        ///
        /// </summary>
        [SugarColumn(ColumnName = "transportation_kind")]
        public virtual TransportationKind TransportationKind { get; set; } = TransportationKind.None;

        /// <summary>
        ///最少空位数
        /// </summary>
        [SugarColumn(ColumnName = "min_empty_location_num")]
        public virtual int? MinEmptyLocationNum { get; set; }

        /// <summary>
        ///最少空仓数
        /// </summary>
        [SugarColumn(ColumnName = "min_empty_box_num")]
        public virtual int? MinEmptyBoxNum { get; set; }

        /// <summary>
        ///最多空仓数
        /// </summary>
        [SugarColumn(ColumnName = "max_empty_box_num")]
        public virtual int? MaxEmptyBoxNum { get; set; }

        /// <summary>
        ///熟料最少转出数量
        /// </summary>
        [SugarColumn(ColumnName = "min_drilled_track_out_num")]
        public virtual int? MinDrilledTrackOutNum { get; set; }

        /// <summary>
        ///料仓中没有钻机中相同的料号时是否立即转出
        /// </summary>
        [SugarColumn(ColumnName = "is_auto_track_out_drilled_silo")]
        public virtual bool? IsAutoTrackOutDrilledSilo { get; set; }

        /// <summary>
        ///生料转出超时时间
        /// </summary>
        [SugarColumn(ColumnName = "raw_track_out_timeout_time")]
        public virtual int? RawTrackOutTimeOutTime { get; set; }

        /// <summary>
        ///首件最少转出数量
        /// </summary>
        [SugarColumn(ColumnName = "min_first_track_out_num")]
        public virtual int? MinFirstTrackOutNum { get; set; }

        /// <summary>
        /// 熟料转出超时(分钟，默认10分钟)
        /// </summary>
        [SugarColumn(ColumnName = "drilled_track_out_time_minutes")]
        public virtual int? DrilledTrackOutTimeMinutes { get; set; } = 10;

        /// <summary>
        /// 转运任务是否由服务端控制(默认：否)
        /// </summary>
        [SugarColumn(ColumnName = "is_controlled_by_server")]
        public virtual bool? IsControlledByServer { get; set; } = false;

        /// <summary>
        /// 关联的缓冲区代号
        /// </summary>
        [SugarColumn(ColumnName = "related_buffer_code")]
        public virtual string? RelatedBufferCode { get; set; }

        /// <summary>
        /// 分区配置(实体导航,一对多)
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(PartitionSetting.partition_id), nameof(Id))]
        public virtual List<PartitionSetting>? Settings { get; private set; }

        /// <summary>
        /// 分区配置赋值
        /// </summary>
        /// <param name="settings"></param>
        public void SetPartitionSetting(List<PartitionSetting> settings)
        {
            Settings = settings;
        }
    }
}