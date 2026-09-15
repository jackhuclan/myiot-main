using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req
{
    public class LoadingAtpFileReq
    {
        /// <summary>
        /// 钻机code
        /// </summary>
        public string? DeviceCode { get; set; }

        /// <summary>
        /// 配刀组计划No
        /// </summary>
        public string? CutterGroupNo { get; set; }
    }
}
