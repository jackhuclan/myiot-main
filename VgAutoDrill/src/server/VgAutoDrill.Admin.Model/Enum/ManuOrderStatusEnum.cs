namespace VgAutoDrill.Admin.Model.Enum
{
    /// <summary>
    /// 工单状态枚举
    /// 0、草稿
    /// 1、已审批
    /// 2、已排产
    /// 3、已投产
    /// 4、已完工
    /// </summary>
    public enum ManuOrderStatusEnum
    {
        /// <summary>
        /// 草稿
        /// </summary>
        DRAFT = 0,
        /// <summary>
        /// 已审批
        /// </summary>
        COMMITED = 1,
        /// <summary>
        /// 已排产
        /// </summary>
        SCHEDULED = 2,
        /// <summary>
        /// 已投产
        /// </summary>
        BEGIN = 3,
        /// <summary>
        /// 已完工
        /// </summary>
        FINISH = 4,
    }
}
