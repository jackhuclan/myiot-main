using System.ComponentModel;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv
{
    /// <summary>
    /// 操作类型  1、上料     2、退空盘   3、上机解绑 
    ///           4、叫空盘   5、下料     6、下机绑定
    /// </summary>
    public enum AgvOperateType
    {
        /// <summary>
        /// 上料
        /// </summary>
        [Description("上料")]
        UploadRawMaterial = 1,

        /// <summary>
        /// 退空盘
        /// </summary>
        [Description("退空盘")]
        UnloadEmptyFork,

        /// <summary>
        /// 上机解绑
        /// </summary>
        [Description("上机解绑")]
        Unbind,

        /// <summary>
        /// 叫空盘
        /// </summary>
        [Description("叫空盘")]
        UploadEmptyFork,

        /// <summary>
        /// 下料
        /// </summary>
        [Description("下料")]
        UnloadClinker,

        /// <summary>
        /// 下机绑定
        /// </summary>
        [Description("下机绑定")]
        Bind,

    }
}
