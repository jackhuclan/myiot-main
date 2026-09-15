namespace VgAutoDrill.Admin.Model.ViewModels.Mes.PartitionSetting
{
    public class PartitionSettingConstants
    {
        /// <summary>
        /// 最少空位数
        /// </summary>
        public const string MIN_EMPTY_LOCATION_NUM = "MinEmptyLocationNum";

        /// <summary>
        /// 最少空仓数
        /// </summary>
        public const string MIN_EMPTY_BOX_NUM = "MinEmptyBoxNum";

        /// <summary>
        /// 最少空仓数
        /// </summary>
        public const string MAX_EMPTY_BOX_NUM = "MaxEmptyBoxNum";


        /// <summary>
        /// 熟料最少转出数量
        /// </summary>
        public const string MIN_DRILLED_TRACK_OUT_NUM = "MinDrilledTrackOutNum";

        /// <summary>
        /// 料仓中没有钻机中相同的料号时是否立即转出
        /// </summary>
        public const string IS_AUTO_TRACK_OUT_DRILLED_SILO = "IsAutoTrackOutDrilledSilo";

        /// <summary>
        /// 生料转出超时时间
        /// </summary>
        public const string RAW_TRACK_OUT_TIMEOUT_TIME = "RawTrackOutTimeOutTime";

        /// <summary>
        /// 首件最少转出数量
        /// </summary>
        public const string MIN_FIRST_TRACK_OUT_NUM = "MinFirstTrackOutNum";


        /// <summary>
        /// 熟料转出超时(分钟，默认10分钟)
        /// </summary>
        public const string DRILLED_TRACK_OUT_TIME_MINUTES = "DrilledTrackOutTimeMinutes";


        /// <summary>
        /// 转运任务是否由服务端控制(默认：否)
        /// </summary>
        public const string IS_CONTROLLED_BY_SERVER = "IsControlledByServer";

        /// <summary>
        /// 关联的缓冲区代号
        /// </summary>
        public const string RELATED_BUFFER_CODE = "RelatedBufferCode";

    }
}
