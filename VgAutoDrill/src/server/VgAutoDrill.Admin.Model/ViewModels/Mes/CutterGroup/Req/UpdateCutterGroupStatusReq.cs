using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req
{
    public class UpdateCutterGroupStatusReq
    {
        public string? GroupNo { get; set; }

        public CutterGroupStatusEnum? GroupStatus { get; set; }
    }
}
