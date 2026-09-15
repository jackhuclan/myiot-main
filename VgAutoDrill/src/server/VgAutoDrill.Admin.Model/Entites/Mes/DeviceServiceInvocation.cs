using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    [SugarTable("t_device_service_invocation")]
    public class DeviceServiceInvocation : BaseEntity
    {

        /// <summary>
        /// MessageId
        /// </summary>
        [SugarColumn(ColumnName = "message_id")]
        public virtual string? MessageId { get; set; }

        /// <summary>
        /// 请求
        /// </summary>
        [SugarColumn(ColumnName = "request_topic")]
        public virtual string? RequestTopic { get; set; }

        /// <summary>
        /// 响应
        /// </summary>
        [SugarColumn(ColumnName = "response_topic")]
        public virtual string? ResponseTopic { get; set; }

        /// <summary>
        /// 载荷
        /// </summary>
        [SugarColumn(ColumnName = "payload")]
        public virtual string? Payload { get; set; }

        /// <summary>
        /// 尝试次数
        /// </summary>
        [SugarColumn(ColumnName = "retries")]
        public virtual int? Retries { get; set; }


        [SugarColumn(ColumnName = "reason")]
        /// <summary>
        /// 原因
        /// </summary>
        public virtual string? Reason { get; set; }
        [SugarColumn(ColumnName = "is_dealed")]
        /// <summary>
        /// 是否已处理
        /// </summary>
        public virtual bool? IsDealed { get; set; } = false;
        [SugarColumn(ColumnName = "is_timeout")]
        /// <summary>
        /// 是否已超时
        /// </summary>
        public virtual bool? IsTimeout { get; set; } = false;

        /// <summary>
        /// 首次调用时间
        /// </summary>
        [SugarColumn(ColumnName = "first_invocation_timestamp")]
        public virtual DateTime? FirstInvocationTimestamp { get; set; }


        /// <summary>
        /// 末次调用时间
        /// </summary>
        [SugarColumn(ColumnName = "last_invocation_timestamp")]
        public virtual DateTime? LastInvocationTimestamp { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        [SugarColumn(ColumnName = "is_urgent")]
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// Mqtt 等级
        /// </summary>
        [SugarColumn(ColumnName = "mqtt_quality_of_service_level")]
        public virtual int? MqttQualityOfServiceLevel { get; set; }
        /// <summary>
        /// 设备事件route key 
        ///</summary>
        [SugarColumn(ColumnName = "routing_key")]
        public virtual string? RoutingKey { get; set; }

    }
}
