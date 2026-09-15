// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgEAPClient.Common.CNC.Common;

public class DrillCommonDataB : NotifyPropertyChangedBase
{
    private string _diaFilePath = string.Empty;
    private string _atpFilePath = string.Empty;
    private string _drillH = string.Empty;
    private string _drillQ = string.Empty;
    private string _drillK = string.Empty;
    private string _drillKi = string.Empty;
    private string _cncVersion = string.Empty;
    private string _spindleYaw = string.Empty;

    /// <summary>
    /// 当前直径文件路径
    /// </summary>
    public string DiaFilePath
    {
        get => _diaFilePath;
        set
        {
            _diaFilePath = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀具文件路径
    /// </summary>
    public string AtpFilePath
    {
        get => _atpFilePath;
        set
        {
            _atpFilePath = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 钻孔高度
    /// </summary>
    public string DrillH
    {
        get => _drillH;
        set
        {
            _drillH = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 快钻高度
    /// </summary>
    public string DrillQ
    {
        get => _drillQ;
        set
        {
            _drillQ = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 控深深度
    /// </summary>
    public string DrillK
    {
        get => _drillK;
        set
        {
            _drillK = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 深度补偿
    /// </summary>
    public string DrillKi
    {
        get => _drillKi;
        set
        {
            _drillKi = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 主轴偏摆
    /// </summary>
    public string SpindleYaw
    {
        get => _spindleYaw;
        set
        {
            _spindleYaw = value;
            OnPropertyChanged();
        }
    }

    public string CncVersion
    {
        get => _cncVersion;
        set
        {
            _cncVersion = value;
            OnPropertyChanged();
        }
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
