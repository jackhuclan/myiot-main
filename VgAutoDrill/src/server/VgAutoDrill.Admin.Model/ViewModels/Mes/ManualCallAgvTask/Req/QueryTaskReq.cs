using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req
{
    public class QueryTaskReq
    {

        /// <summary>
        /// 钻机编号
        /// </summary>
        [Required]
        public string? DeviceCode { get; set; }


        /// <summary>
        /// 库位号
        /// </summary>
        [Required]
        public string? LocationCode { get; set; }
    }
}
