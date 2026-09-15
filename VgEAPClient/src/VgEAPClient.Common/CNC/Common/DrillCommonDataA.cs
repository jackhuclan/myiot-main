// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgEAPClient.Common.CNC.Common;

public class DrillCommonDataA : NotifyPropertyChangedBase
{
    private string _duty = string.Empty;
    private string _shiftOnlineTime = string.Empty;
    private string _shiftWorkingTime = string.Empty;
    private string _shiftWaitingTime = string.Empty;
    private string _shiftErrorTime = string.Empty;
    private string _programPath = string.Empty;
    private string _xYPosition = string.Empty;
    private string _curDrillOrRout = string.Empty;
    private string _totalDrillOrRout = string.Empty;
    private string _curSpindleStatus = string.Empty;
    private string _curToolId = string.Empty;
    private string _curToolD = string.Empty;
    private string _curToolS = string.Empty;
    private string _curToolF = string.Empty;
    private string _curToolR = string.Empty;
    private string _curToolN = string.Empty;
    private string _curToolB = string.Empty;
    private string _curToolZ = string.Empty;
    private string _curToolA = string.Empty;
    private string _curToolSegM = string.Empty;
    private string _curToolChipl = string.Empty;
    private string _cncRunProgress = string.Empty;
    private string _block = string.Empty;
    private string _step = string.Empty;
    private string _drillZ = string.Empty;
    private string _drillFV = string.Empty;

    /// <summary>
    /// 程式路径改变
    /// </summary>
    public event Action? OnCncPgmFilePathChanged;

    /// <summary>
    /// 稼动率
    /// </summary>
    public string Duty
    {
        get => _duty;
        set
        {
            _duty = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 班次上线时间
    /// </summary>
    public string ShiftOnlineTime
    {
        get => _shiftOnlineTime;
        set
        {
            _shiftOnlineTime = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 班次工作时间
    /// </summary>
    public string ShiftWorkingTime
    {
        get => _shiftWorkingTime;
        set
        {
            _shiftWorkingTime = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 班次待机时间
    /// </summary>
    public string ShiftWaitingTime
    {
        get => _shiftWaitingTime;
        set
        {
            _shiftWaitingTime = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 班次报错时间
    /// </summary>
    public string ShiftErrorTime
    {
        get => _shiftErrorTime;
        set
        {
            _shiftErrorTime = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前程式路径
    /// </summary>
    public string ProgramPath
    {
        get => _programPath;
        set
        {
            bool bChanged = _programPath != value;
            _programPath = value;

            if (bChanged)
            {
                OnCncPgmFilePathChanged?.Invoke();
            }

            OnPropertyChanged();
        }
    }

    public string XPosition { get; set; } = string.Empty;
    public string YPosition { get; set; } = string.Empty;

    /// <summary>
    /// 机台坐标XY 格式：X0.000Y0.000
    /// </summary>
    public string XYPosition
    {
        get => _xYPosition;
        set
        {
            bool bChanged = _xYPosition != value;
            _xYPosition = value;
            OnPropertyChanged();

            if (bChanged && _xYPosition != string.Empty)
            {
                XPosition = _xYPosition.Split("Y")[0].Substring(1);
                YPosition = _xYPosition.Split("Y")[1];
            }
        }
    }

    /// <summary>
    /// 当前加工孔数/锣程,钻机：/孔数 , 锣机：/m
    /// </summary>
    public string CurDrillOrRout
    {
        get => _curDrillOrRout;
        set
        {
            _curDrillOrRout = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 总计加工孔数/锣程,钻机：/孔数 , 锣机：/m
    /// </summary>
    public string TotalDrillOrRout
    {
        get => _totalDrillOrRout;
        set
        {
            _totalDrillOrRout = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前轴状态
    /// </summary>
    public string CurSpindleStatus
    {
        get => _curSpindleStatus;
        set
        {
            _curSpindleStatus = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀号
    /// </summary>
    public string CurToolId
    {
        get => _curToolId;
        set
        {
            _curToolId = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀直径
    /// </summary>
    public string CurToolD
    {
        get => _curToolD;
        set
        {
            _curToolD = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀轴转速
    /// </summary>
    public string CurToolS
    {
        get => _curToolS;
        set
        {
            _curToolS = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀进刀速
    /// </summary>
    public string CurToolF
    {
        get => _curToolF;
        set
        {
            _curToolF = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀退刀速
    /// </summary>
    public string CurToolR
    {
        get => _curToolR;
        set
        {
            _curToolR = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀寿命
    /// </summary>
    public string CurToolN
    {
        get => _curToolN;
        set
        {
            _curToolN = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀已使用寿命
    /// </summary>
    public string CurToolB
    {
        get => _curToolB;
        set
        {
            _curToolB = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀钻孔深度
    /// </summary>
    public string CurToolZ
    {
        get => _curToolZ;
        set
        {
            _curToolZ = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀等待时间
    /// </summary>
    public string CurToolA
    {
        get => _curToolA;
        set
        {
            _curToolA = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 当前刀分段模式
    /// </summary>
    public string CurToolSegM
    {
        get => _curToolSegM;
        set
        {
            _curToolSegM = value;
            OnPropertyChanged();
        }
    }
    /// <summary>
    /// 当前刀加载芯片
    /// </summary>
    public string CurToolChipl
    {
        get => _curToolChipl;
        set
        {
            _curToolChipl = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Cnc运行进度
    /// </summary>
    public string CncRunProgress
    {
        get => _cncRunProgress;
        set
        {
            _cncRunProgress = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Block
    /// </summary>
    public string Block
    {
        get => _block;
        set
        {
            _block = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Step
    /// </summary>
    public string Step
    {
        get => _step;
        set
        {
            _step = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Z
    /// </summary>
    public string DrillZ
    {
        get => _drillZ;
        set
        {
            _drillZ = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 象限
    /// </summary>
    public string DrillFV
    {
        get => _drillFV;
        set
        {
            _drillFV = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 班次总钻孔数/锣程，仅支持9X
    /// </summary>
    public string ShiftTotalDrillOrRout { get; set; } = string.Empty;

    /// <summary>
    /// 班次运行次数,仅支持9X
    /// </summary>
    public string ShiftRunCount { get; set; } = string.Empty;

    /// <summary>
    /// 班次换刀时间，仅支持9X
    /// </summary>
    public string ShiftToolChangeTime { get; set; } = string.Empty;

    /// <summary>
    /// 轴数，9X从CNC获取，8X从配置文件获取
    /// </summary>
    public string SpindleCount { get; set; } = string.Empty;

    public bool IsCncRunning { get; set; } = false;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }

    public DateTime LastGetTime { get; set; } = DateTime.MinValue;

    public string LastName { get; set; } = string.Empty;
}
