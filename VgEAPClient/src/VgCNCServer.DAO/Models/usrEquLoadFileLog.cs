// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgCNCServer.DAO.Models;

[Table("usrEquLoadFileLog")]
public class usrEquLoadFileLog
{
    [Key]
    public string sInnerID { get; set; } = string.Empty;
    /// <summary>
    /// 机台编码
    /// </summary>
    public string sEquipmentID { get; set; } = string.Empty;
    /// <summary>
    /// 直径文件
    /// </summary>
    public string sDiaFile { get; set; } = string.Empty;
    /// <summary>
    /// 刀盘文件
    /// </summary>
    public string sAtpFile { get; set; } = string.Empty;
    /// <summary>
    /// 加工程序
    /// </summary>
    public string sProgramFile { get; set; } = string.Empty;
    /// <summary>
    /// 加载状态(0=未加载;1=已加载;2=过期废弃)
    /// </summary>
    public int iLoadStatus { get; set; }
    /// <summary>
    /// 记录时间
    /// </summary>
    public DateTime dtTime { get; set; }

    /// <summary>
    /// 来源机台编码
    /// </summary>
    public string sEquipmentIDSrc { get; set; } = string.Empty;
    /// <summary>
    /// 操作类型(CncLoadFile;CncStart;CncStop)
    /// </summary>
    public string sOpType { get; set; } = string.Empty;
}
