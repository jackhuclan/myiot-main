using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 分区类别,默认0未知，1私有分区，2公共分区
/// </summary>
public enum PartitionKind
{
    /// <summary>
    /// 未指定
    /// </summary>
    [Description("未指定")]
    Unknown = 0,

    /// <summary>
    /// 1私有分区，基于插齿位的分区
    /// </summary>
    [Description("私有分区，基于插齿位的分区")]
    Private = 1,

    /// <summary>
    /// 2公共分区，基于公共缓存区
    /// </summary>
    [Description("公共分区，基于公共缓存区")]
    Public = 2,
}
