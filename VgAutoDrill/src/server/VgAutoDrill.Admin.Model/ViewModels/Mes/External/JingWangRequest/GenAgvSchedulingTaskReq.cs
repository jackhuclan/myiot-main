namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingWangRequest
{
    public class GenAgvSchedulingTaskReq
    {
        /// <summary>
        /// 请求编码
        /// </summary>
        public virtual string? reqCode { get; set; }

        public virtual string podTyp { get; set; } = "P4";

        public virtual string clientCode { get; set; } = "MES";

        /// <summary>
        /// 任务类型
        /// </summary>
        public virtual string? taskTyp { get; set; }

        public string[] userCallCodePath { get; set; }

        public GenAgvSchedulingTaskData? data { get; set; }
    }

    public class GenAgvSchedulingTaskData
    {
        public virtual string? carrierTyp { get; set; } = "P4";

        public virtual string? materialLot { get; set; }

        public virtual string? sysLotNum { get; set; }

        public virtual string? podLotNum { get; set; }

        public virtual string? podStatus { get; set; } = "1";
    }
}
