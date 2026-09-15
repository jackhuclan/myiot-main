namespace VgAutoDrill.Admin.Model.Enum
{
    /// <summary>
    /// 生产任务状态枚举
    /// </summary>
    public enum TaskStatusEnum
    {
        /// <summary>
        /// 草稿
        /// </summary>
        DRAFT = 0,
        /// <summary>
        /// 已提交
        /// 数值从1变更为10
        /// </summary>
        COMMITED = 10,
        /// <summary>
        /// 派送中
        /// </summary>
        SENDING = 20,
        /// <summary>
        /// Buffer就位
        /// </summary>
        BUFFERED = 30,
        /// <summary>
        /// 已开始
        /// 数值从2变更为40
        /// </summary>
        BEGIN = 40,
        /// <summary>
        /// 已完成
        /// 数值从3变更为50
        /// </summary>
        FINISH = 50
    }
}
