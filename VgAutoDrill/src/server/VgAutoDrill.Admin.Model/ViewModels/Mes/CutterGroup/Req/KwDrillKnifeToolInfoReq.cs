using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req
{
    public class KwDrillKnifeToolInfoReq
    {
        /// <summary>
        /// 设备code
        /// </summary>
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 配刀组计划No
        /// </summary>
        public string? PlanNo { get; set; }
    }
}
