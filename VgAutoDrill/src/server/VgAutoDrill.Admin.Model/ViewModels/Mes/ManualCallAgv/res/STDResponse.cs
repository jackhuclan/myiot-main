namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.res
{
    /// <summary>
    /// STD物流接口响应实体
    /// </summary>
    public class STDResponse
    {
        #region -- 属性 --
        /// <summary>
        /// 状态编号 0:成功
        /// /// </summary>
        public Int32 code { get; set; } = -1;
        /// <summary>
        /// 返回消息
        /// </summary>
        public String message { get; set; } = string.Empty;
        /// <summary>
        /// 请求编号
        /// </summary>
        public String? reqCode { get; set; }
        /// <summary>
        /// 自定义返回
        /// </summary>
        public String? data { get; set; }
        #endregion
    }
}
