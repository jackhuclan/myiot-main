using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res
{
    public class KwDrillKnifeToolInfoRes
    {

        public string? Code { get; set; }


        public string? Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ReturnValue ReturnValue { get; set; } = new ReturnValue();


    }

    public class ReturnValue
    {
        public string? PlanNo { get; set; }

        public string? Atp { get; set; }

        public string? BoxNos { get; set; }
    }
}
