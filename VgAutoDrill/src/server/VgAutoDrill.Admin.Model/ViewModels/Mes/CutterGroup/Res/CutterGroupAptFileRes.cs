using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res
{
    public class CutterGroupAptFileRes
    {
        /// <summary>
        /// 配刀组计划No
        /// </summary>
        public string? GroupNo { get; set; }

        /// <summary>
        /// apt文件路径
        /// </summary>
        public string? AtpFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNeedLoad { get; set; } = false;

        /// <summary>
        /// 刀盒集合(英文逗号隔开)
        /// </summary>
        public string? Boxs { get; set; }
    }
}
