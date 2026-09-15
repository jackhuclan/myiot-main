namespace VgAutoDrill.Admin.Model.Enum
{
    public enum DataStatusEnum
    {
        /// <summary>
        /// 
        /// </summary>
        Disable = 0,
        /// <summary>
        /// 
        /// </summary>
        Enable = 1,
    }
    public enum HandleExternalWorkOrderStatusEnum
    {
        /// <summary>
        /// 未处理
        /// </summary>
        NotHandle = 0,
        /// <summary>
        /// 处理成功
        /// </summary>
        HandleSuccess = 1,
        /// <summary>
        /// 处理失败
        /// </summary>
        HandleFail = 2,
    }

    /// <summary>
    /// 板料状态
    /// </summary>
    public enum ProductStatusEnum
    {
        Raw = 30100,
        Ripe = 40100
    }
}
