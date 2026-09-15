using System.ComponentModel;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv
{
    /// <summary>
    /// 库位区域类型
    /// </summary>
    public enum LocationType
    {

        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 生料区
        /// </summary>
        [Description("生料区")]
        RawMaterialLocation = 1,



        /// <summary>
        /// 熟料区
        /// </summary>
        [Description("熟料区")]
        ClinkerMaterialLocation = 2,



    }
}
