// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using Org.BouncyCastle.Utilities;
using Quartz;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.Other.Jobs
{
    public class StatisticsJob : IJob
    {
        private readonly ILogger<StatisticsJob> _logger;
        private readonly IDeviceProvider _deviceProvider;

        public StatisticsJob(ILogger<StatisticsJob> logger, IDeviceProvider deviceProvider)
        {
            _logger = logger;
            _deviceProvider = deviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                string? deviceId = context.JobDetail.JobDataMap.GetString("DeviceId");
                if (deviceId == null)
                {
                    ThrowHelper.ThrowArgumentNullException("DeviceId is null in StatisticsJob");
                }


                _logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]:  StatisticsJob is executing at {deviceId} ...");
                var device = (DefaultDrill)_deviceProvider.GetDevice(deviceId);
                try
                {
                    //获取稼动率 //开机时间 //工作时间  //等待时间  //报警时间 //结束到开始总时间  //清洗夹头时间 
                    #region systemTime
                    //Thread.Sleep(1000);
                    if (device.cnc84Command == null)
                    {
                        return;
                    }
                    Double[] dataValueUtilisation = device.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Utilisation");//当班嫁动率 OK
                    Double[] dataValueWorkingTime = device.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.WorkingTime");//OK
                    Double[] dataValueWaitingTime = device.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.WaitingTime");//OK
                    Double[] dataValueErrorTime = device.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.ErrorTime");//OK
                    Double[] dataValueActiveTime = device.cnc84Command.ReadCncNode<Double[]>("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.ActiveTime");//OK
                    TimeSpan timeSpan = TimeSpan.FromMilliseconds(dataValueWorkingTime[1]);
                    var worktime = timeSpan.TotalMinutes.ToString("#0");
                    timeSpan = TimeSpan.FromMilliseconds(dataValueWaitingTime[1]);
                    var waittime = timeSpan.TotalMinutes.ToString("#0");
                    timeSpan = TimeSpan.FromMilliseconds(dataValueErrorTime[1]);
                    var errortime = timeSpan.TotalMinutes.ToString("#0");
                    timeSpan = TimeSpan.FromMilliseconds(dataValueActiveTime[1]);
                    var opentime = timeSpan.TotalMinutes.ToString("#0");
                    var duty = Math.Round(dataValueUtilisation[0], 0).ToString();
                    #endregion
                    #region endToStart
                    string dateString = "0001/01/01 08:00:00";
                    var originDateTime = DateTime.Parse(dateString);
                    DateTime now = DateTime.Now;
                    DateTime today = DateTime.Today;
                    double endToStartTime = 0;

                    DateTime[] dataValueRunStartTime = device.cnc84Command.ReadCncNode<DateTime[]>("ns=4;s=UI/origin/AutoList/AutoList.List/AutoList.RunStartTime");//程序开始时间
                    DateTime[] dataValueRunEndTime = device.cnc84Command.ReadCncNode<DateTime[]>("ns=4;s=UI/origin/AutoList/AutoList.List/AutoList.RunEndTime");//程序结束时间
                    dataValueRunStartTime = dataValueRunStartTime.Select(s => s.AddHours(8)).ToArray();
                    dataValueRunEndTime = dataValueRunEndTime.Select(s => s.AddHours(8)).ToArray();
                    if (dataValueRunStartTime.All(s => s == originDateTime))
                    {
                        endToStartTime += now.Subtract(today).TotalMinutes;
                        return;
                    }
                    string str = "";
                    for (global::System.Int32 i = 0; i < dataValueRunStartTime.Length; i++)
                    {
                        str += $"开始时间 {dataValueRunStartTime[i]} ----------结束时间 {dataValueRunEndTime[i]}\r\n";
                    }
                    _logger.LogError($"StatisticsJob\r\n  {str}");




                    //

                    var lastRunStartTime = dataValueRunStartTime.Max();
                    var lastRunStartTimeIndex = Array.FindIndex(dataValueRunStartTime, s => s == dataValueRunStartTime.Max());
                    var lastRunEndTime = dataValueRunEndTime.Max();
                    var lastRunEndTimeIndex = Array.FindIndex(dataValueRunEndTime, s => s == dataValueRunEndTime.Max());

                    int index = Array.FindIndex(dataValueRunStartTime, d => d.Subtract(today).Ticks >= 0);
                    if (index == -1 && (lastRunEndTimeIndex == lastRunStartTimeIndex))
                    {
                        endToStartTime += now.Subtract(today).TotalMinutes;
                        return;
                    }
                    if (index - 1 >= 0)
                    {
                        index = index - 1;
                    }

                    dataValueRunStartTime = dataValueRunStartTime.Skip(index).ToArray();
                    dataValueRunEndTime = dataValueRunEndTime.Skip(index).ToArray();

                    lastRunEndTime = dataValueRunEndTime.Max();
                    lastRunStartTime = dataValueRunStartTime.Max();
                    if (dataValueRunStartTime[0].Subtract(today).Ticks > 0)
                    {
                        endToStartTime += dataValueRunStartTime[0].Subtract(today).TotalMinutes;
                        if (dataValueRunStartTime[0] == lastRunStartTime && dataValueRunEndTime[0] != originDateTime && dataValueRunEndTime[0] == lastRunEndTime)
                        {
                            endToStartTime += now.Subtract(dataValueRunEndTime[0]).TotalMinutes;
                        }
                    }

                    for (global::System.Int32 i = 1; i < dataValueRunStartTime.Length; i++)
                    {
                        if (dataValueRunStartTime[i] == originDateTime)
                        {
                            continue;
                        }

                        if (dataValueRunEndTime[i - 1].Subtract(today).Ticks < 0)
                        {
                            endToStartTime += dataValueRunStartTime[i].Subtract(today).TotalMinutes;
                        }
                        else
                        {
                            endToStartTime += dataValueRunStartTime[i].Subtract(dataValueRunEndTime[i - 1]).TotalMinutes;

                        }
                        if (dataValueRunStartTime[i] == lastRunStartTime && (dataValueRunEndTime[i] == lastRunEndTime) && lastRunEndTime != originDateTime)
                        {
                            endToStartTime += now.Subtract(dataValueRunEndTime[i]).TotalMinutes;
                        }

                    }

                    #endregion

                    StatisticsTimeModel statisticsTimeModel = new StatisticsTimeModel()
                    {
                        Worktime = worktime,
                        Waittime = waittime,
                        Errortime = errortime,
                        Opentime = opentime,
                        Duty = duty,
                        CollectClearTime = DefaultDrill.CollectCleanTime.ToString("#0"),
                        EndToStartTime = endToStartTime.ToString("#0"),
                        DeviceId = deviceId,
                        DateString = today.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    _logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]:  {JsonSerializer.Serialize<StatisticsTimeModel>(statisticsTimeModel)}");
                 
                    _logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]:  StatisticsJob is executing success.");
                }
                catch (Exception e)
                {
                    _logger.LogError($"[{DateTime.Now.ToLongTimeString()}]:  StatisticsJob is executing fail {e.Message}.");
                }
                finally
                {
                    
                }
            });
        }
    }
}
