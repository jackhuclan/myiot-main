namespace VgAutoDrill.External.Model.V3
{
    public class ExternalBaseReq
    {
        /// <summary>
        /// 接口功能
        /// </summary>
        public virtual string? Action { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public virtual string? Version { get; set; }
        /// <summary>
        /// 传入参数
        /// </summary>
        public virtual Dictionary<string, object?>? Data { get; set; }
    }
}
