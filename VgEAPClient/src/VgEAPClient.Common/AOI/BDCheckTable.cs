// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgEAPClient.Common.AOI;


[Table("BDCheckTable")]
public class BDCheckTable
{
    [Key]
    public int? StaticInfoIndex { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string? czUserName { get; set; }
    /// <summary>
    /// 钻带文件名
    /// </summary>
    public string? czDrillName { get; set; }
    /// <summary>
    /// 条码信息
    /// </summary>
    public string? czQBarCodeInfo { get; set; }
    /// <summary>
    /// AB面
    /// </summary>
    public string? iCheckPlaneAB { get; set; }
    /// <summary>
    /// 检测开始时间(yyyy-MM-dd-HH-mm-ss)
    /// </summary>
    public string? ulBeginCheckTime { get; set; }
    /// <summary>
    /// 检测用时
    /// </summary>
    public int? iCheckUseTime { get; set; }
    /// <summary>
    /// 总孔数
    /// </summary>
    public int? iCheckHoles { get; set; }
    /// <summary>
    /// 漏背钻
    /// </summary>
    public int? iLeakBDHoles { get; set; }
    /// <summary>
    /// 漏通孔
    /// </summary>
    public int? iLeakArcossHoles { get; set; }
    /// <summary>
    /// 全漏
    /// </summary>
    public int? iLeakBothHoles { get; set; }
    /// <summary>
    /// 通孔异物
    /// </summary>
    public int? iForeignHoles { get; set; }
    /// <summary>
    /// 两孔相交
    /// </summary>
    public int? iInterBHHoles { get; set; }
    /// <summary>
    /// 同心度超差
    /// </summary>
    public int? iSameCenterHoles { get; set; }
    /// <summary>
    /// 内径超差
    /// </summary>
    public int? iInRadiusHoles { get; set; }
    /// <summary>
    /// 外径超差
    /// </summary>
    public int? iOutRadiusHoles { get; set; }
    /// <summary>
    /// 最小孔环
    /// </summary>
    public int? iMinRingWidthHoles { get; set; }
    /// <summary>
    /// 同心度公差
    /// </summary>
    public float? fSameCenterTolrance { get; set; }
    /// <summary>
    /// 最小孔环公差
    /// </summary>
    public float? fMinRingWidthTolrance { get; set; }
    /// <summary>
    /// 孔径公差
    /// </summary>
    public int? fHoleDiamTolrance { get; set; }
    /// <summary>
    /// 图片保存路径
    /// </summary>
    public string? czReserver { get; set; }
    /// <summary>
    /// 发送后标志位
    /// </summary>
    public int? iSendFlag { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? szLotNumber { get; set; }
    public string? szGroupNumber { get; set; }
    /// <summary>
    /// 机台号
    /// </summary>
    public string? czReserver2 { get; set; }
    /// <summary>
    /// 板材
    /// </summary>
    public string? czReserver3 { get; set; }
    /// <summary>
    /// 背钻CPK
    /// </summary>
    public string? czReserver4 { get; set; }
    public int? szBoardNumber { get; set; }
    /// <summary>
    /// C/S/CS
    /// </summary>
    public int? nCheckMode { get; set; }
    public int? nComfirmResult { get; set; }
    /// <summary>
    /// 轴号
    /// </summary>
    public int? nReserver7 { get; set; }
    /// <summary>
    /// 结果 1:NG 2:OK
    /// </summary>
    public int? nReserver8 { get; set; }
    public int? nReserver9 { get; set; }
    /// <summary>
    /// 预留字段
    /// </summary>
    public string? czReserver5 { get; set; }
    public string? czReserver6 { get; set; }
    public string? czReserver7 { get; set; }
    public string? czReserver8 { get; set; }
    public string? czReserver9 { get; set; }
    public string? czReserver10 { get; set; }
    public string? czReserver11 { get; set; }
    public string? czReserver12 { get; set; }
    public string? czReserver13 { get; set; }
    public string? czReserver14 { get; set; }
}
