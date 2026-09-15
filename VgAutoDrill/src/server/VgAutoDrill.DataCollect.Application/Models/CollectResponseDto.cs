namespace VgAutoDrill.DataCollect.Application.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectResponseDto<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public CollectResponseCode Code { set; get; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { set; get; } = "";
        /// <summary>
        /// Data
        /// </summary>
        public T? Data { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CollectResponseCode
    {
        /// <summary>
        /// 
        /// </summary>
        Success = 0,
        /// <summary>
        /// 
        /// </summary>
        Fail = 500
    }
}
