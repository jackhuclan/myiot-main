// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common.CNC;

namespace VgEAPClient.Common;

//目前只在84系统中使用过 : 目的是在AUTO-LIST数据中找最后一次加工（不论其是否已经完成）的开始时间
//要从AUTO-LIST 找到最后一次的加工，需要进行多次和CNC的 请求回复 往来。
public class CNC8XAutoListReaderAsync
{
    public const bool AUTO_LIST_DETAIL_LOG = true;
    public const string LogHead = "8xAutoList : ";

    public delegate bool IsConnectionOK();

    /// <summary>
    /// 只读取开始时间
    /// </summary>
    /// <param name="job_index"></param>
    /// <returns></returns>
    public delegate Task<DateTime> ReqReadAutoListStartTime(int job_index);

    /// <summary>
    /// 请求读取job_index 对应的整行的信息
    /// </summary>
    /// <param name="job_index"></param>
    /// <returns></returns>
    public delegate Task<AutoListCache.LineData> ReqReadAutoListLine(int job_index, bool readStartTime);

    //init
    public event Action? OnBatchReadEnd;

    private IsConnectionOK? _isConnOKImp = null;
    private ReqReadAutoListStartTime? _readStartTimeImp = null;
    private ReqReadAutoListLine? _readLineImp = null;
    private Action<string> _logDebugImp = LogDebugDummy;

    /// <summary>
    /// AUTO-LIST中一共有多少行？
    /// </summary>
    private int _maxEleCount = 0;

    private int _readCountPerStep = 20;

    //run
    private bool _started = false;
    private bool _ended = false;
    private bool _firstRead = false;

    private int _leftIndex { get; set; } = -1;
    private int _rightIndex { get; set; } = -1;
    private int _curJobIndex { get; set; } = -1;

    private DateTime _startBatchReadTime = DateTime.MinValue;
    private DateTime _lastReqReadTime = DateTime.Now;

    private volatile bool _cncAutoListReaderLoopRunning = false;
    private volatile bool _LoopEnd = false;

    private object _autoListCacheFrontLock = new object();
    private AutoListCache _autoListCacheFront = null;//上一批次读的数据存在这里


    //下面几项数据在每一轮的开始重置
    private Dictionary<int, DateTime> _startTimeReadedDic = new Dictionary<int, DateTime>();//key : line-index
    private HashSet<int> _sendedReadReqSet = new HashSet<int>();//key : line-index
    private AutoListCache _bgCache = null;//批量读取时临时存数据

    public CNC8XAutoListReaderAsync(IsConnectionOK isConnOKImp
        , ReqReadAutoListStartTime readStartTimeImp, ReqReadAutoListLine readLineImp, Action<string> log_debug, int max_ele_count)
    {
        _isConnOKImp = isConnOKImp;
        _readStartTimeImp = readStartTimeImp;
        _readLineImp = readLineImp;
        _logDebugImp = (log_debug != null) ? log_debug : LogDebugDummy;
        _maxEleCount = max_ele_count;

        _readCountPerStep = 25;

        _autoListCacheFront = new AutoListCache(log_debug);
        _bgCache = new AutoListCache(log_debug);
    }

    public bool Start()
    {
        if (null == _isConnOKImp || null == _readStartTimeImp || null == _readLineImp || null == _logDebugImp)
        {
            if (null != _logDebugImp) _logDebugImp("8xAutoList : Start  failed !");
            return false;
        }

        Task.Run(AutoListReaderLoop);

        return true;
    }

    public void Stop()
    {
        _cncAutoListReaderLoopRunning = false;

        while (false == _LoopEnd)
        {
            Thread.Sleep(50);
        }
    }

    private void StartBatchRead()
    {
        if (null == _isConnOKImp || null == _readStartTimeImp || null == _readLineImp || null == _logDebugImp)
        {
            return;
        }

        Reset();

        _started = true;
        _ended = false;
        _firstRead = false;

        _leftIndex = 0;
        _rightIndex = _readCountPerStep - 1;

        _startBatchReadTime = DateTime.Now;
        _lastReqReadTime = DateTime.Now;

        _logDebugImp(LogHead + $"Batch read start :  ////  ////  ////  ////  ////  ////  ////  ////  ////");
    }


    public AutoListCache.LineData? GetLineCopy(int lineIndex)
    {
        if (null == _autoListCacheFront)
            return null;
        else
            return _autoListCacheFront.GetLineDataCopy(lineIndex);
    }

    public int GetCurJobIndex()
    {
        return _curJobIndex;
    }
    public DateTime GetJobStartTime(int jobIndex, DateTime defaultValue)
    {
        lock (_autoListCacheFrontLock)
        {
            var line = _autoListCacheFront.GetLineData(jobIndex);
            if (null == line)
                return defaultValue;
            else
                return line.StartTime;
        }
    }
    public DateTime GetJobEndTime(int jobIndex, DateTime defaultValue)
    {
        lock (_autoListCacheFrontLock)
        {
            var line = _autoListCacheFront.GetLineData(jobIndex);
            if (null == line)
                return defaultValue;
            else
                return line.EndTime;
        }
    }


    private void Reset()
    {
        _started = false;
        _ended = false;
        _firstRead = false;

        _leftIndex = -1;
        _rightIndex = -1;
        _curJobIndex = -1;

        _startBatchReadTime = DateTime.MinValue;
        _lastReqReadTime = DateTime.Now;


        _sendedReadReqSet.Clear();
        _startTimeReadedDic.Clear();

        _bgCache.Clear();
    }

    static void LogDebugDummy(string line)
    {
    }


    private bool IsStarted()
    {
        return _started;
    }

    private bool IsEnded()
    {
        return _ended;
    }
    private async Task AutoListReaderLoop()
    {
        _cncAutoListReaderLoopRunning = true;
        _LoopEnd = false;

        while (_cncAutoListReaderLoopRunning)
        {
            bool connOK = _isConnOKImp();

            if (connOK)
            {
                try
                {
                    if (false == IsStarted())
                    {
                        StartBatchRead();
                    }
                    else
                    {
                        if (IsEnded())
                        {
                            lock (_autoListCacheFrontLock)
                            {
                                _autoListCacheFront.Clear();

                                _bgCache.CopyTo(_autoListCacheFront);
                            }

                            var curJobStartTime = GetJobStartTime(_curJobIndex, DateTimeUtil.Future3000);
                            _logDebugImp?.Invoke(LogHead + $"Batch read end ! cur job idx {_curJobIndex}. start t '{curJobStartTime}'.");

                            OnBatchReadEnd?.Invoke();//给用户一个机会，更新最新的数据

                            Reset();//重置reader，准备下一轮读取
                        }
                        else
                        {
                            await Update();
                        }
                    }

                    Thread.Sleep(5);
                }
                catch (Exception ex)
                {
                    _logDebugImp?.Invoke(LogHead + $"Ex5000182 " + ex.Message);

                    await Task.Delay(2000);
                }
            }//if (connOK)
            else
            {
                await Task.Delay(2000);
            }
        }//while

        _LoopEnd = true;
    }

    private async Task Update()
    {
        if (null == _readLineImp || null == _logDebugImp)
            return;

        if (false == _started)
            return;

        if (false == _firstRead)
        {
            _firstRead = true;

            _logDebugImp(LogHead + $"Update first read.");

            for (int idx = _leftIndex; idx <= _rightIndex; idx++)
            {
                await TryReadOneLine(idx);
            }
        }

        if (IsEnded())
            return;

        if (_sendedReadReqSet.Count > 0)
        {
            if (_sendedReadReqSet.Count != (1 + _rightIndex - _leftIndex))
            {
                _logDebugImp(LogHead + $"Exp3988158 Invalid count in SendedReadReqSet : count {_sendedReadReqSet.Count}. req range {_leftIndex} ~ {_rightIndex}.");
            }

            int get_count = 0;
            foreach (int req_index in _sendedReadReqSet)
            {
                var line = _bgCache.GetLineData(req_index);
                if (null != line)
                {
                    get_count += 1;
                }
            }//for

            if (get_count == _sendedReadReqSet.Count)
            {
                _logDebugImp(LogHead + $"step read done. {_leftIndex} ~ {_rightIndex}.");

                _sendedReadReqSet.Clear();
            }
            else
            {
                if ((DateTime.Now - _lastReqReadTime).TotalSeconds > 5 * 60)
                {
                    _logDebugImp(LogHead + $"step read timeout !  {_leftIndex} ~ {_rightIndex}. Skip.");

                    _sendedReadReqSet.Clear();
                }
            }

            if (0 == _sendedReadReqSet.Count)
            {
                DoneCheck();
            }
        }
        else
        {
            await ReadNextBatch();
        }
    }

    void DoneCheck()
    {
        if (_rightIndex >= _maxEleCount - 1)
        {
            OnEnd(_rightIndex, $"Right Index : {_rightIndex}");
        }
        else
        {
            if (_rightIndex > 3)
            {
                int valid_st_count = 0;
                for (int idx = _rightIndex; idx > _rightIndex - 3; idx--)
                {
                    if (_startTimeReadedDic.ContainsKey(idx))
                    {
                        if (DateTimeUtil.IsValidTime(_startTimeReadedDic[idx]))
                            valid_st_count++;
                    }
                }//for

                if (0 == valid_st_count)
                {//读取到多条连续的无效行， 说明没必要继续读取了
                    OnEnd(_rightIndex, $"Continuous empty lines, right index {_rightIndex}");
                }
            }
        }
    }

    async Task<AutoListCache.LineData?> TryReadOneLine(int index)
    {
        if (null == this._readStartTimeImp)
        {
            return null;
        }
        if (null == this._readLineImp)
        {
            return null;
        }

        AutoListCache.LineData? line = null;
        _lastReqReadTime = DateTime.Now;

        try
        {
            _sendedReadReqSet.Add(index);

            DateTime startTimeOfLine = await this._readStartTimeImp(index);

            _startTimeReadedDic[index] = startTimeOfLine;

            if (DateTimeUtil.IsValidTime(startTimeOfLine))
            {
                line = await this._readLineImp(index, false);

                if (null == line)
                {
                    _logDebugImp(LogHead + $"read line failed ! line {index}.");
                }
            }
            else
            {
                line = new AutoListCache.LineData(index);
            }
            if (null != line)
            {
                line.StartTime = startTimeOfLine;
                _bgCache.SetLineData(line);
            }
        }
        catch (Exception e)
        {
            _logDebugImp(LogHead + $"Exp9300210 " + e.Message);
        }
        return line;
    }

    /*何时停止 : 
    CASE A : 
        ELE N : Start-time is valid ;
        ELE N+1 : Start-time is invalid ;
    CASE B : 
        Start-time of tail line is invalid ;
    */
    async Task ReadNextBatch()
    {
        DoneCheck();

        if (IsEnded())
            return;

        int left_last = _leftIndex;
        int right_last = _rightIndex;

        _leftIndex = right_last + 1;
        _rightIndex = right_last + _readCountPerStep;

        if (_rightIndex > _maxEleCount - 1)
            _rightIndex = _maxEleCount - 1;

        if (_leftIndex > _rightIndex)
            _leftIndex = _rightIndex;

        _logDebugImp(LogHead + $"req step read. {_leftIndex} ~ {_rightIndex}.");
        for (int idx = _leftIndex; idx <= _rightIndex; idx++)
        {
            await TryReadOneLine(idx);
        }
    }

    void OnEnd(int last_job_idx, string reason)
    {
        long sec_since_start = (long)((DateTime.Now - _startBatchReadTime).TotalSeconds);
        string done_desc = last_job_idx >= 0 ? "Succeed" : "Fail";
        _ended = true;

        DateTime max_st = DateTimeUtil.BeginOf1970;
        int max_st_index = 0;
        foreach (var kv in _startTimeReadedDic)
        {
            var st = kv.Value;
            if (DateTimeUtil.IsValidTime(st) && max_st < st)
            {
                max_st = st;
                max_st_index = kv.Key;
            }
        }

        _curJobIndex = max_st_index;
        _logDebugImp(LogHead + $"On End : {done_desc}. cur job idx {max_st_index}({DateTimeUtil.DateTimeToDBString(max_st)}), use sec {sec_since_start}.reason '{reason}'.");
    }


}
