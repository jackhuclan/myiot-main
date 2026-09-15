// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgEAPClient.Common.CNC.Status;

public class DrillStatusData
{
    private volatile string _cncStatusText = string.Empty;
    private volatile string _blockText = string.Empty;
    private volatile bool _isBlockTextChanged = false;
    private volatile bool _isProgramStart = false;
    private volatile string _curDrillOrRout = string.Empty;
    private volatile string _cncStatusMo = string.Empty;
    private volatile bool _isBrokenTool = false;
    private volatile string _cncStatusEc = string.Empty;
    private volatile bool _isPullCncToolData = false;
    private volatile bool _isPullProgramData = true;
    private volatile bool _isLoadFileFinished = false;
    private volatile bool _isAlarmStart = false;
    private volatile string _pgmRunStartTime = string.Empty;
    private volatile string _pgmRunEndTime = string.Empty;
    private volatile string _cncError = string.Empty;
    private volatile string _cncComm = string.Empty;
    private volatile string _pgmRunTotalTime = string.Empty;
    private volatile bool _isRequiredLoadFile = false;

    /// <summary>
    /// 是否采集钻孔数/锣程数据
    /// </summary>
    private volatile bool _isPullHitOrRout = false;

    /// <summary>
    /// CncStatus 文本改变事件
    /// </summary>
    public event Action? OnCncStatusTextChanged;

    /// <summary>
    /// Block 状态栏文本改变事件
    /// </summary>
    public event Action? OnBlockTextChanged;

    /// <summary>
    /// 发生断刀事件
    /// </summary>
    public event Action? OnBrokenToolChanged;

    /// <summary>
    /// Cnc状态改变
    /// </summary>
    public event Action? OnCncStatusMoChanged;

    /// <summary>
    /// Cnc状态码改变
    /// </summary>
    public event Action? OnCncStatusEcChanged;

    /// <summary>
    /// Cnc钻/锣第一个孔事件
    /// </summary>
    public event Action? OnCncStartDrillOrRoutEvent;

    /// <summary>
    /// 程序开始事件
    /// </summary>
    public event Action? OnCncProgramStartEvent;

    /// <summary>
    /// 程序结束事件
    /// </summary>
    public event Action? OnCncProgramFinishedEvent;

    /// <summary>
    /// 加载文件结束事件
    /// </summary>
    public event Action? OnCncLoadFileFinished;

    /// <summary>
    /// CNC报警开始事件
    /// </summary>
    public event Action? OnCncAlarmStartEvent;

    /// <summary>
    /// CNC报警结束事件
    /// </summary>
    public event Action? OnCncAlarmEndEvent;

    /// <summary>
    /// M54事件发生
    /// </summary>
    public event Action? OnCncM52StartEvent;

    /// <summary>
    /// 当前Cnc Command
    /// </summary>
    public event Action? OnCncCommChanged;

    /// <summary>
    /// CncError发生变化
    /// </summary>
    public event Action? OnCncErrorChanged;

    /// <summary>
    /// 加载程式文件，DIA文件和ATP文件
    /// </summary>
    /// <param name="PgmFilePath"></param>
    public void CncLoadFile(string PgmFilePath, string DiaFilePath = "", string AtpFilePath = "")
    {
        try
        {
            DrillRequiredLoadProgramDataModel.ClearData();

            if (!string.IsNullOrWhiteSpace(PgmFilePath))
            {
                DrillRequiredLoadProgramDataModel.PgmLoadModel.FilePath = PgmFilePath;
            }
            if (!string.IsNullOrWhiteSpace(DiaFilePath))
            {
                DrillRequiredLoadProgramDataModel.DiaLoadModel.FilePath = DiaFilePath;
            }
            if (!string.IsNullOrWhiteSpace(AtpFilePath))
            {
                DrillRequiredLoadProgramDataModel.AtpLoadModel.FilePath = AtpFilePath;
            }
            IsRequiredLoadFile = true;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, ex.Message);
        }
    }

    public CncLoadResult CncLoadFileResult { get; set; } = new CncLoadResult();

    private void GetLoadResult(bool bIsLoadFileFinished)
    {
        try
        {
            CncLoadFileResult.ClearData();

            if (!bIsLoadFileFinished)
            {
                CncLoadFileResult.PgmLoadResult = "加载失败(8)，加载过程中出现异常";
            }
            else
            {
                if (!string.IsNullOrEmpty(DrillRequiredLoadProgramDataModel.AtpLoadModel.FilePath))
                {
                    CncLoadFileResult.AtpLoadResult += DrillRequiredLoadProgramDataModel.AtpLoadModel.FilePath;

                    if (DrillRequiredLoadProgramDataModel.AtpLoadModel.LoadResult.StartsWith("OK"))
                    {
                        CncLoadFileResult.AtpLoadResult += "  加载成功\r\n";
                    }
                    else
                    {
                        CncLoadFileResult.AtpLoadResult += $"  加载失败(10): {DrillRequiredLoadProgramDataModel.AtpLoadModel.LoadResult.Substring(DrillRequiredLoadProgramDataModel.AtpLoadModel.LoadResult.IndexOf("_") + 1)}\r\n";
                    }
                }

                if (!string.IsNullOrEmpty(DrillRequiredLoadProgramDataModel.DiaLoadModel.FilePath))
                {
                    CncLoadFileResult.DiaLoadResult += DrillRequiredLoadProgramDataModel.DiaLoadModel.FilePath;

                    if (DrillRequiredLoadProgramDataModel.DiaLoadModel.LoadResult.StartsWith("OK"))
                    {
                        CncLoadFileResult.DiaLoadResult += "  加载成功\r\n";
                    }
                    else
                    {
                        CncLoadFileResult.DiaLoadResult += $"  加载失败(11): {DrillRequiredLoadProgramDataModel.DiaLoadModel.LoadResult.Substring(DrillRequiredLoadProgramDataModel.DiaLoadModel.LoadResult.IndexOf("_") + 1)}\r\n";
                    }
                }

                if (!string.IsNullOrEmpty(DrillRequiredLoadProgramDataModel.PgmLoadModel.FilePath))
                {
                    CncLoadFileResult.PgmLoadResult += DrillRequiredLoadProgramDataModel.PgmLoadModel.FilePath;

                    if (DrillRequiredLoadProgramDataModel.PgmLoadModel.LoadResult.StartsWith("OK"))
                    {
                        CncLoadFileResult.PgmLoadResult += "  加载成功\r\n";
                    }
                    else
                    {
                        CncLoadFileResult.PgmLoadResult += $"  加载失败(15): {DrillRequiredLoadProgramDataModel.PgmLoadModel.LoadResult.Substring(DrillRequiredLoadProgramDataModel.PgmLoadModel.LoadResult.IndexOf("_") + 1)}\r\n";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, ex.Message);
        }
    }

    /// <summary>
    /// CncStatus文本
    /// </summary>
    public string CncStatusText
    {
        get => _cncStatusText;

        set
        {
            //bool bChanged = _cncStatusText != value;
            bool bChanged = IsRealCncStatusText(_cncStatusText, value);
            _cncStatusText = value;

            if (bChanged)
            {
                ParseCncStatusText(_cncStatusText);

                OnCncStatusTextChanged?.Invoke();
            }
        }
    }

    /// <summary>
    /// 状态栏文本是否改变
    /// </summary>
    public bool IsBlockTextChanged
    {
        get => _isBlockTextChanged;

        private set
        {
            _isBlockTextChanged = value;
            if (_isBlockTextChanged)
            {
                IsPullHitOrRout = true;
            }
        }
    }

    /// <summary>
    /// 状态栏文本
    /// </summary>
    public string BlockText
    {
        get => _blockText;

        set
        {
            bool bChanged = _blockText != value;
            _blockText = value;

            if (bChanged)
            {
                IsBlockTextChanged = true;

                OnBlockTextChanged?.Invoke();
            }
            else
            {
                IsBlockTextChanged = false;
            }

            if (CncStatusMo.Equals("ALAM") && !_isAlarmStart)
            {
                _isAlarmStart = true;
                CncAlarmData.AlarmId = CncStatusEc;
                CncAlarmData.AlarmDesc = _blockText;
                CncAlarmData.AlarmStartTime = DateTime.Now.ToString();
                OnCncAlarmStartEvent?.Invoke();
            }
            else if (_isAlarmStart && !CncStatusMo.Equals("ALAM"))
            {
                _isAlarmStart = false;
                CncAlarmData.AlarmEndTime = DateTime.Now.ToString();
                OnCncAlarmEndEvent?.Invoke();
            }
        }
    }

    /// <summary>
    /// 当前钻孔数/锣程
    /// </summary>
    public string CurDrillOrRout
    {
        get => _curDrillOrRout;

        set
        {
            bool bChanged = (_curDrillOrRout != value);

            if (bChanged && OnCncStartDrillOrRoutEvent != null)
            {
                float fLastHitOrRout = 0, fCurHitOrRout = 0;

                if (float.TryParse(_curDrillOrRout, out fLastHitOrRout))
                {
                    if (fLastHitOrRout == 0)
                    {
                        if (float.TryParse(value, out fCurHitOrRout) && fCurHitOrRout > 0 && _cncStatusMo.Equals("WORK", StringComparison.OrdinalIgnoreCase))
                        {
                            OnCncStartDrillOrRoutEvent?.Invoke();
                        }
                    }
                }
            }
            _curDrillOrRout = value;
        }
    }

    /// <summary>
    /// 是否发生了断刀
    /// </summary>
    public bool IsBrokenTool
    {
        get => _isBrokenTool;

        set
        {
            _isBrokenTool = value;

            if (_isBrokenTool)
            {
                OnBrokenToolChanged?.Invoke();
                _isBrokenTool = false;
            }
        }
    }

    /// <summary>
    /// Cnc状态
    /// </summary>
    public string CncStatusMo
    {
        get => _cncStatusMo;

        set
        {
            bool bChanged = (_cncStatusMo != value);
            _cncStatusMo = value;

            if (bChanged)
            {
                OnCncStatusMoChanged?.Invoke();
            }
        }
    }

    /// <summary>
    /// Cnc状态码
    /// </summary>
    public string CncStatusEc
    {
        get => _cncStatusEc;

        set
        {
            bool bChanged = _cncStatusEc != value;

            if (bChanged)
            {
                if (value == "0048" || value == "0536936569")
                {
                    IsPullProgramDataOnce = true;
                }

                OnCncStatusEcChanged?.Invoke();
            }
            _cncStatusEc = value;
        }
    }

    /// <summary>
    /// 状态栏背景色
    /// </summary>
    public string CncBlockColor { get; set; } = string.Empty;

    /// <summary>
    /// Cnc轴状态, 例子 “00100111”。注意 ： 1号轴在最右边!
    /// </summary>
    public string CncStatusZs { get; set; } = string.Empty;

    /// <summary>
    /// CncError
    /// </summary>
    public string CncError
    {
        get => _cncError;
        set
        {
            bool bChanged = _cncError != value;
            _cncError = value;

            if (bChanged)
            {
                OnCncErrorChanged?.Invoke();

                if (_cncError.Contains("COMM M52") || _cncError.Contains("Collet clean begin"))
                {
                    OnCncM52StartEvent?.Invoke();
                }
            }
        }
    }

    public string CncComm
    {
        get => _cncComm;
        set
        {
            bool bChanged = _cncComm != value;
            _cncComm = value;

            if (bChanged)
            {
                OnCncCommChanged?.Invoke();
            }
        }
    }

    /// <summary>
    /// 当前程式开始时间
    /// 未开始则为string.Empty, 值格式为 yyyy/MM/dd HH:mm:ss
    /// </summary>
    public string PgmRunStartTime
    {
        get => _pgmRunStartTime;
        set
        {
            bool bChanged = _pgmRunStartTime != value;
            _pgmRunStartTime = value;

            if (bChanged && _pgmRunStartTime != string.Empty)
            {
                _isProgramStart = true;
                OnCncProgramStartEvent?.Invoke();
            }
        }
    }

    /// <summary>
    /// 当前程式结束时间
    /// 未结束则为string.Empty，结束则有值, 值格式为 yyyy/MM/dd HH:mm:ss
    /// </summary>
    public string PgmRunEndTime
    {
        get => _pgmRunEndTime;
        set
        {
            _pgmRunEndTime = value;

            if (_pgmRunEndTime != string.Empty && _isProgramStart)
            {
                PgmRunTotalTime = (DateTime.Parse(PgmRunEndTime) - DateTime.Parse(PgmRunStartTime)).ToString();
                OnCncProgramFinishedEvent?.Invoke();
                _isProgramStart = false;
            }
        }
    }

    /// <summary>
    /// 当前程式运行总计时间
    /// </summary>
    public string PgmRunTotalTime
    {
        get => _pgmRunTotalTime;
        set => _pgmRunTotalTime = value;
    }

    /// <summary>
    /// 断刀数据
    /// </summary>
    public BrokenToolData BrokenToolData { get; set; } = new BrokenToolData();

    /// <summary>
    /// CNCTOOL 数据
    /// </summary>
    public CncToolData CncToolData { get; set; } = new CncToolData();

    /// <summary>
    /// 是否采集程式文件数据（单次）
    /// </summary>
    public bool IsPullProgramDataOnce
    {
        get => _isPullProgramData;
        set => _isPullProgramData = value;
    }

    /// <summary>
    /// 当前加载文件数据
    /// </summary>
    public ProgramDataModel CurProgramData { get; set; } = new ProgramDataModel();

    /// <summary>
    /// 需要加载的文件路径（包括ATP，DIA，PGM）
    /// </summary>
    public CncLoadFileData DrillRequiredLoadProgramDataModel { get; set; } = new CncLoadFileData();

    /// <summary>
    /// 是否开始加载文件
    /// </summary>
    public bool IsRequiredLoadFile
    {
        get => _isRequiredLoadFile;
        set => _isRequiredLoadFile = value;
    }

    /// <summary>
    /// 加载文件是否结束
    /// </summary>
    public bool IsLoadFileFinished
    {
        get => _isLoadFileFinished;
        set
        {
            _isLoadFileFinished = value;
            GetLoadResult(_isLoadFileFinished);
            OnCncLoadFileFinished?.Invoke();
        }
    }

    /// <summary>
    /// CNC 报警数据
    /// </summary>
    public CncAlarmData CncAlarmData { get; set; } = new();

    public bool IsPullHitOrRout
    {
        get => _isPullHitOrRout;
        set => _isPullHitOrRout = value;
    }

    private void ParseCncStatusText(string strCncStatusText)
    {
        try
        {
            if (!string.IsNullOrEmpty(strCncStatusText))
            {
                string[] aryStrStatus = strCncStatusText.Split(",");
                foreach (string item in aryStrStatus)
                {
                    if (item.StartsWith("AR")) { string AR = item.Replace("AR", ""); if (CncStatus.AR != AR) CncStatus.AR = AR; }
                    if (item.StartsWith("AP")) { string AP = item.Replace("AP", ""); if (CncStatus.AP != AP) CncStatus.AP = AP; }
                    if (item.StartsWith("ZS")) { string ZS = item.Replace("ZS", ""); if (CncStatus.ZS != ZS) CncStatus.ZS = ZS; }
                    if (item.StartsWith("MO")) { string MO = item.Replace("MO", ""); if (CncStatus.MO != MO) CncStatus.MO = MO; }
                    if (item.StartsWith("EC")) { string EC = item.Replace("EC", ""); if (CncStatus.EC != EC) CncStatus.EC = EC; }
                    if (item.StartsWith("FN")) { string FN = item.Replace("FN", ""); if (CncStatus.FN != FN) CncStatus.FN = FN; }
                }

                CncStatusMo = CncStatus.MO;
                CncStatusEc = CncStatus.EC;
                CncStatusZs = CncStatus.ZS;
            }
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "ParseCncStatusText Exception - " + ex.Message);
        }
    }

    /// <summary>
    /// 判断CNC状态文本是否发生变化(不判断AR)
    /// </summary>
    /// <param name="Oldtext">旧文本</param>
    /// <param name="Newtext">新文本</param>
    /// <returns></returns>
    private bool IsRealCncStatusText(string Oldtext, string Newtext)
    {
        Newtext = Newtext ?? string.Empty;
        Oldtext = Oldtext ?? string.Empty;
        if (Oldtext.Trim() == Newtext.Trim())
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(Oldtext) || string.IsNullOrWhiteSpace(Newtext))
        {
            return true;
        }
        var os = Oldtext.Split(',').ToList();
        var ns = Newtext.Split(",").ToList();
        os.RemoveAll(o => o.StartsWith("AR"));
        ns.RemoveAll(o => o.StartsWith("AR"));

        var Oldtext_2 = string.Join(",", os);
        var Newtext_2 = string.Join(",", ns);
        if (Oldtext_2.Trim() == Newtext_2.Trim())
        {
            return false;
        }
        else
        {
            return true;
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

    /// <summary>
    /// 程序加工趟数
    /// </summary>
    public int CncPgmNum { get; set; } = 0;

    public static int GetSpindleEnableCountInZS(string zs)
    {
        int count = 0;
        for (int k = zs.Length - 1; k >= 0; k--)
        {
            var c = zs[k];
            if (c == '1')
                count++;
        }
        return count;
    }

    /// <summary>
    /// 程序加工结果
    /// </summary>
    public string sResult { get; set; } = "OK";
    /// <summary>
    /// 条码
    /// </summary>
    public string sBarCode { get; set; } = string.Empty;
    /// <summary>
    /// 图片保存路径
    /// </summary>
    public string sImgUrl { get; set; } = string.Empty;

    public bool IsStopGetRunTime { get; set; } = false;

    public bool IsCanSendPgmEnd { get; set; } = true;

    /// <summary>
    /// Cpk
    /// </summary>
    public string sCpk { get; set; } = string.Empty;
}
