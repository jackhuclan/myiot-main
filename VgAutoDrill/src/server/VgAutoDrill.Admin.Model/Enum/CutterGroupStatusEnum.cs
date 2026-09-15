using System.ComponentModel;

namespace VgAutoDrill.Admin.Model.Enum
{
    /// <summary>
    /// 0、未锁定
    /// 1、锁定
    /// 20、刀盒校验完成
    /// 30、加载钻带文件完成
    /// 40、加载配刀Atp文件完成
    /// </summary>
    public enum CutterGroupStatusEnum
    {
        /// <summary>
        /// 未锁定
        /// </summary>
        [Description("未锁定")]
        UnLocked = 0,

        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Locked = 1,

        /// <summary>
        /// 刀盒校验完成
        /// </summary>
        [Description("刀盒校验完成")]
        VerifyCutterBoxComplete = 20,

        /// <summary>
        /// 加载钻带文件完成
        /// </summary>
        [Description("加载钻带文件完成")]
        LoadDrillFileComplete = 30,

        /// <summary>
        /// 加载配刀Atp文件完成
        /// </summary>
        [Description("加载配刀Atp文件完成")]
        LoadCutterAtpFileComplete = 40,
    }

    public enum CutterBoxStatusEnum
    {
        /// <summary>
        /// 没有校验
        /// </summary>
        [Description("没有校验")]
        Nothing = -1,
        /// <summary>
        /// 校验失败
        /// </summary>
        [Description("校验失败")]
        Fail = 0,

        /// <summary>
        /// 校验成功
        /// </summary>
        [Description("校验成功")]
        OK = 1
    }
}
