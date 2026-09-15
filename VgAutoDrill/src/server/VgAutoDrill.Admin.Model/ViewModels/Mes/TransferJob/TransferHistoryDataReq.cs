using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob
{
    public class TransferHistoryDataReq
    {
        /// <summary>
        /// 迁移数据时间节点
        /// </summary>
        public DateTime? TransferTime { get; set; }
    }
}
