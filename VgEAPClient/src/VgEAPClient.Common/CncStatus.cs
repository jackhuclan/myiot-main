// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

public class CncStatus
{
    public static string AR { get; set; } = "";//当前运行时间
    public static string AH { get; set; } = "";//当前钻孔行程计数器
    public static string AB { get; set; } = "";//当前程序块编号
    public static string AS { get; set; } = "";//当前步数
    public static string AP { get; set; } = "";//当前程序执行进度的百分比
    public static string ZS { get; set; } = "";//所选 Z 轴的掩码
    public static string MO { get; set; } = "";//机器状态
    public static string EC { get; set; } = "";//现有事件编号和事件消息的事件
    public static string FN { get; set; } = "";//当前部件程序的文件名
}
