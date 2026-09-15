namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingWangRequest
{
    public class GenAgvSchedulingTaskBatchReq
    {
        /// <summary>
        /// 请求编码（必填）
        /// </summary>
        public virtual string? ReqCode { get; set; }

        /// <summary>
        /// 任务组编号，每次请求唯一不重复（必填）
        /// </summary>
        public virtual string? TaskGroupCode { get; set; }

        /// <summary>
        /// 呼叫站点（必填）
        /// </summary>
        public virtual string? WbCode { get; set; }

        /// <summary>
        /// 任务类型（必填）
        /// </summary>
        public virtual string? TaskTyp { get; set; }

        public virtual List<GenAgvSchedulingTaskGroups>? TaskGroups { get; set; }
    }

    public class GenAgvSchedulingTaskGroups
    {
        public string[] UserCallCodePath { get; set; }

        public string? MaterialLot { get; set; }
    }
}
