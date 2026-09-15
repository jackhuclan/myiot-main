using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req
{
    public class AptBoxReq
    {
        public string? DeviceCode { get; set; }

        /// <summary>
        /// 配刀组计划No
        /// </summary>
        public string? CutterGroupNo { get; set; }

        /// <summary>
        /// 外部输入的刀盒码集合
        /// </summary>
        public List<string> InputBoxs { get; set; } = new List<string>();
    }
}
