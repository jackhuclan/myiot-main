// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC;
public class CNC8XCmd
{
    public const string DiaFileNameWithDialog = "%S(DiaFileNameWithDialog,0)";


    public const string CM = "CM";
    public const string CM_NO_CONFIRM = "CM@@@";
    public const string FA = "FA";

    public const string FIXXY_Y = "%S(FIXXY_1)";
    public const string FIXXY_X = "%S(FIXXY_2)";


    public const string AutoListMaxElems = "AutoListMaxElems";//AUTO LIST 数组的长度
    public const string JOB_RUN = "JOB_RUN";//实测：此值含义不明
    public const string HSYS55_JobIndex = "HSYS55_JobIndex";//实测：AUTO-LIST界面上选中的条目的编号，暂时无用
    public const string JOB_REAL_INDEX = "JOB_REAL_INDEX";//实测：此值含义不明

    //下面几个来自 AUTO LIST ，通过execute 发送时，需要加INDEX后缀。例如 JOB_STARTTIME(3)
    public const string JOB_STARTTIME = "JOB_STARTTIME";//格式 ： long, unix-time-stamp
    public const string JOB_ENDTIME = "JOB_ENDTIME";//格式 ： long, unix-time-stamp
    public const string JOB_TIME = "JOB_TIME";//加工时长， int， 单位 ： 秒
    public const string JOB_HOLE_CNT = "JOB_HOLE_CNT";//钻孔数， int
}
