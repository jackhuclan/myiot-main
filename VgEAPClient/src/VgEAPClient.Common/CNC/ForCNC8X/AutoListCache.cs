// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC;
public class AutoListCache
{
    public class LineData
    {
        public int Index { get; set; } = -1;

        //加工开始 本地时间
        public DateTime StartTime { get; set; } = DateTimeUtil.BeginOf1970;
        //加工结束 本地时间
        public DateTime EndTime { get; set; } = DateTimeUtil.BeginOf1970;

        //本次JOB打孔数量
        public long HoleCnt { get; set; } = 0;

        //本次JOB的加工时长（分钟）
        public double WorkDurMin { get; set; } = 0;

        //本次JOB的异常停止时长（分钟）
        public double StopDurMin { get; set; } = 0;

        public LineData(int index)
        {
            this.Index = index;

        }
        public LineData Clone()
        {
            var clone = new LineData(this.Index);

            clone.StartTime = this.StartTime;
            clone.EndTime = this.EndTime;
            clone.HoleCnt = this.HoleCnt;
            clone.WorkDurMin = this.WorkDurMin;

            return clone;
        }

        public bool HasStarted()
        {
            return DateTimeUtil.IsValidTime(this.StartTime);
        }
    }

    //init
    private Action<string> _logDebug;
    //run
    private object _dataLock = new object();
    private Dictionary<int, LineData> _lineDic = new Dictionary<int, LineData>();//key : line index; 

    public AutoListCache(Action<string> logDebugImp)
    {
        _logDebug = (null != logDebugImp) ? logDebugImp : LogDebugDummy;
    }
    static void LogDebugDummy(string line)
    {
    }

    public LineData? GetLineDataCopy(int index)
    {
        lock (_dataLock)
        {
            LineData? data = null;
            _lineDic.TryGetValue(index, out data);
            return data;
        }
    }

    public void Clear()
    {
        lock (_dataLock)
        {
            _lineDic.Clear();
        }
    }

    public void CopyTo(AutoListCache des)
    {
        lock (_dataLock)
        {
            des._lineDic.Clear();
            foreach (var kv in _lineDic)
            {
                var line = kv.Value;
                var line_copy = line.Clone();
                des._lineDic[line_copy.Index] = line_copy;
            }
        }
    }

    public LineData? GetLineData(int index)
    {
        lock (_dataLock)
        {
            LineData? data = null;
            _lineDic.TryGetValue(index, out data);
            return data;
        }
    }
    public void SetLineData(LineData data)
    {
        lock (_dataLock)
        {
            if (null != data && data.Index >= 0)
                _lineDic[data.Index] = data;
        }
    }

    public void PrintAutoListCache()
    {
        /*_logger.LogDebug($"Print auto list cache : ");
        lock (_dataLock)
        {
            foreach (var kv in _jobStartTimeCache)
            {
                int jobIndex = kv.Key;
                DateTime startTime = kv.Value;

                DateTime endTime = DateTime.MinValue;
                _jobEndTimeCache.TryGetValue(jobIndex, out endTime);

                long holeCnt = 0;
                _jobHoleCntCache.TryGetValue(jobIndex, out holeCnt);

                double durMin = 0;
                _jobWorkDurMinCache.TryGetValue(jobIndex, out durMin);

                _logDebugImp($"Auto list {jobIndex} : {startTime} ~ {endTime}. hole {holeCnt}, dur-m {durMin}.");
            }//for
        }//lock
        */
    }
}
