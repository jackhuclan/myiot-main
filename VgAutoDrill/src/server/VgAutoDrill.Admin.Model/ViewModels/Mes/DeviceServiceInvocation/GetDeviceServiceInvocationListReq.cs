namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation
{
    public class GetDeviceServiceInvocationListReq
    {
        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string? MessageId { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public virtual string? Reason { get; set; }
        /// <summary>
        /// 是否已处理
        /// </summary>
        public virtual bool? IsDealed { get; set; } = false;
        /// <summary>
        /// 是否已超时
        /// </summary>
        public virtual bool? IsTimeout { get; set; } = false;
        /// <summary>
        /// 请求
        /// </summary>
        public virtual string? RequestTopic { get; set; }

        /// <summary>
        /// 响应
        /// </summary>
        public virtual string? ResponseTopic { get; set; }


    }
}
