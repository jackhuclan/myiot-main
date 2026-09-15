using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class GetExterWorkOrderListReq : Page
    {
        /// <summary>
        /// 外部工单编号
        /// <summary>
        public virtual string? SourceCode { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        public virtual string? SpecGroup { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 请求日期
        /// </summary>
        public virtual DateTime? RequestDate { get; set; }

        public virtual int? IsErrorData { get; set; }

        public virtual HandleExternalWorkOrderStatusEnum? ExternalStatus { get; set; }
    }
}
