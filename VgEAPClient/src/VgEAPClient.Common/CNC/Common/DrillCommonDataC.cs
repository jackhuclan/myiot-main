// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgEAPClient.Common.CNC.Common;

public class DrillCommonDataC : NotifyPropertyChangedBase
{
    private string _preDuty = string.Empty;
    private string _oPID = string.Empty;
    private string _sAX = string.Empty;
    private string _sAY = string.Empty;
    private string _sAZX = string.Empty;
    private string _sAZY = string.Empty;
    private string _userName = string.Empty;
    private string _userLevel = string.Empty;

    /// <summary>
    /// 上一班次稼动率
    /// </summary>
    public string PreDuty
    {
        get => _preDuty;
        set
        {
            _preDuty = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// OPID
    /// </summary>
    public string OPID
    {
        get => _oPID;
        set
        {
            _oPID = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// X轴涨缩
    /// </summary>
    public string SAX
    {
        get => _sAX;
        set
        {
            _sAX = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Y轴涨缩
    /// </summary>
    public string SAY
    {
        get => _sAY;
        set
        {
            _sAY = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 涨缩中心点X
    /// </summary>
    public string SAZX
    {
        get => _sAZX;
        set
        {
            _sAZX = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 涨缩中心点Y
    /// </summary>
    public string SAZY
    {
        get => _sAZY;
        set
        {
            _sAZY = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// CNC用户名
    /// </summary>
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// CNC用户级别
    /// </summary>
    public string UserLevel
    {
        get => _userLevel;
        set
        {
            _userLevel = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 轴测量数据
    /// </summary>
    public ObservableCollection<SpindleMeasureInfo> ListSpindleMeasureInfo { get; set; } = new();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}

public class SpindleMeasureInfo : NotifyPropertyChangedBase
{
    private string _spindleEnable = string.Empty;
    private string _tMeasureDia = string.Empty;
    private string _tMeasureLen = string.Empty;
    private string _tRunout = string.Empty;
    private string _spindleWorkTimes = string.Empty;
    private string _spindleWorkTotalMinutes = string.Empty;

    public int SpindleId { get; set; } = 0;

    public string SpindleEnable
    {
        get => _spindleEnable;
        set
        {
            _spindleEnable = value;
            OnPropertyChanged();
        }
    }

    public string TMeasureDia
    {
        get => _tMeasureDia;
        set
        {
            _tMeasureDia = value;
            OnPropertyChanged();
        }
    }

    public string TMeasureLen
    {
        get => _tMeasureLen;
        set
        {
            _tMeasureLen = value;
            OnPropertyChanged();
        }
    }

    public string TRunout
    {
        get => _tRunout;
        set
        {
            _tRunout = value;
            OnPropertyChanged();
        }
    }

    public string SpindleWorkTimes
    {
        get => _spindleWorkTimes;
        set
        {
            _spindleWorkTimes = value;
            OnPropertyChanged();
        }
    }

    public string SpindleWorkTotalMinutes
    {
        get => _spindleWorkTotalMinutes;
        set
        {
            _spindleWorkTotalMinutes = value;
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
