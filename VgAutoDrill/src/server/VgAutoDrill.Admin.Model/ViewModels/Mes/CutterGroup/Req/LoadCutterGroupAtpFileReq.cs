using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req
{
    public class LoadCutterGroupAtpFileReq
    {
        /// <summary>
        /// 配刀组计划编码
        /// </summary>
        public string? GroupNo { get; set; }

        /// <summary>
        /// 钻机编码
        /// </summary>
        public string? DeviceCode { get; set; }
    }
}
