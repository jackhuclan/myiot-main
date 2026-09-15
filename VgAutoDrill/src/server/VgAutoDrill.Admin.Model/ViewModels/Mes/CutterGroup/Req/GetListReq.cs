using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req
{
    public class GetListReq : Page
    {
        /// <summary>
        /// 配刀组计划No
        /// </summary>
        public string? CutterGroupNo { get; set; }

        /// <summary>
        /// 钻机编号
        /// </summary>
        public string? DrillNo { get; set; }



        public List<string>? DrillNos { get; set; }

        /// <summary>
        /// 配刀状态 0-待配刀 1-配刀锁定 
        /// </summary>
        public CutterGroupStatusEnum? CutterGroupStatus { get; set; }


    }
}
