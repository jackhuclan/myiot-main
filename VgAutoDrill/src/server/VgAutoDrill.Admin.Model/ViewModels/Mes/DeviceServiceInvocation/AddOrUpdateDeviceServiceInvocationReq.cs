namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation
{
    public class AddOrUpdateDeviceServiceInvocationReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string? MessageId { get; set; }

        /// <summary>
        /// 请求
        /// </summary>
        public virtual string? RequestTopic { get; set; }
        /// <summary>
        /// 响应
        /// </summary>
        public virtual string? ResponseTopic { get; set; }

        /// <summary>
        /// 载荷
        /// </summary>
        public virtual string? Payload { get; set; }

        /// <summary>
        /// 尝试次数5
        /// </summary>
        public virtual int? Retries { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public virtual string? Reason { get; set; }
        /// <summary>
        /// 是否已处理4
        /// </summary>
        public virtual bool? IsDealed { get; set; } = false;
        /// <summary>
        /// 是否已超时3
        /// </summary>
        public virtual bool? IsTimeout { get; set; } = false;
        /// <summary>
        /// 首次调用时间2
        /// </summary>
        public virtual DateTime? FirstInvocationTimestamp { get; set; }

        /// <summary>
        /// 末次调用时间1
        /// </summary>
        public virtual DateTime? LastInvocationTimestamp { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public virtual int? MqttQualityOfServiceLevel { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        public virtual int? IsUrgent { get; set; }
        /// <summary>
        /// 设备事件route key 
        ///</summary>
        public virtual string? RoutingKey { get; set; }

    }
}
