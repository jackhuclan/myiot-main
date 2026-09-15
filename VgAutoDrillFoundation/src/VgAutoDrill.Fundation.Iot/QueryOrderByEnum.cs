using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot;

/// <summary>
/// 查询排序方式
/// </summary>
public enum QueryOrderByEnum
{
    /// <summary>
    /// 原有排序方式
    /// </summary>
    [Description("原有排序方式")]
    Original = 0,
    /// <summary>
    /// 创建时间正序
    /// </summary>
    [Description("创建时间正序")]
    OrderByCreateTimeASC = 1,
    /// <summary>
    /// 创建时间倒序
    /// </summary>
    [Description("创建时间倒序")]
    OrderByCreateTimeDesc = 2,
    /// <summary>
    /// 编码正序
    /// </summary>
    [Description("编码正序")]
    OrderByCodeASC = 3,
    /// <summary>
    /// 编码倒序
    /// </summary>
    [Description("编码倒序")]
    OrderByCodeDesc = 4,
    /// <summary>
    /// Id正序
    /// </summary>
    [Description("Id正序")]
    OrderByIdASC = 5,
    /// <summary>
    /// Id倒序
    /// </summary>
    [Description("Id倒序")]
    OrderByIdDesc = 6,
    /// <summary>
    /// 开始时间正序
    /// </summary>
    [Description("开始时间正序")]
    OrderByStartTimeASC = 7,
    /// <summary>
    /// 开始时间倒序
    /// </summary>
    [Description("开始时间倒序")]
    OrderByStartTimeDesc = 8,
}
