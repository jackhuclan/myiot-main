// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;
using MQTTnet.Client;
using Vegalot.External.XianjinIot.Agv.Models.Report;
using Vegalot.External.XianjinIot.Common.Models;
using Vegalot.External.XianjinIot.Common;
using Vegalot.External.XianjinIot.Agv.Models.Ack;
using Vegalot.External.XianjinIot.Agv.Models.Command;
using System.Collections.Concurrent;
using StackExchange.Redis;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Vegalot.External.XianjinIot.Agv.Command;
internal class BaseSimpleCommand : SimpleCommand<XianjInIotDefaultAgv>
{
    private readonly IMqttClient _mqttClient;
    public ConcurrentDictionary<string, bool> CommandsStatus = new ConcurrentDictionary<string, bool>();//线程安全
    public BaseSimpleCommand(IServiceProvider serviceProvider, XianjInIotDefaultAgv device, IMqttClient mqttClient, 
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _mqttClient = mqttClient;
    }

    public override Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        => throw new NotImplementedException();

    /// <summary>
    /// 初始化plc寄存器业务点位
    /// </summary>
    /// <param name="slaveID"></param>
    /// <param name="registerPoints"></param>
    /// <param name="deviceStatus"></param>
    /// <returns></returns>
    protected async Task InitializePlcRegisterPoints(byte slaveID, List<ushort> registerPoints, string taskCode)
    {
        foreach(var ponit in registerPoints)
        {
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, ponit, 0);
        }
        if (CommandsStatus.Count >= 100)//防止无限膨胀
        {
            CommandsStatus.Clear();
        }
        CommandsStatus.AddOrUpdate(taskCode, key=> true, (key, oldValue) => true);//第二个参数是不存在情况下的值，第三个参数是存在情况下的值       
        await Task.CompletedTask;
    }

    protected async Task RemoveTaskCodeRecord(string taskCode)
    {
        bool value;
        CommandsStatus.TryRemove(taskCode, out value);//超时或者初始化，终止信号循环接收，要及时清理掉字典里的taskCode
        await Task.CompletedTask;
    }
    /// <summary>
    /// 初始化agent,跳出等待plc信号的循环
    /// </summary>
    /// <param name="slaveID"></param>
    /// <returns></returns>
    protected async Task<bool> InitializeAgvAgent(byte slaveID)
    {
        var value = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 3054, 1);//agv和agv代理同步初始化       
        if (value[0] == 1)
        {   
            _ = Task.Run(async () =>//检测到agv同步初始化信号，立即返回，新开线程并等待3秒后复位，目的是等待其他指令也跳出循环
            {
                var data= InteractingDevice.modbusIpMaster.ReadHoldingRegisters(slaveID, 3054, 1);
                if (data[0] == 0)
                {
                    return;
                }
                await Task.Delay(3000);
                InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, 3054, 0);//接收到后，复位

            });
            return true;
        }
        await Task.CompletedTask;
        return false;
    }

    /// <summary>
    /// 检测agv是否在工作任务中
    /// </summary>
    /// <returns></returns>
    protected async Task<bool> CheckCommandIsWorking(string ackTopic,string taskCode)
    {
         await Task.CompletedTask;
        return CommandsStatus.ContainsKey(taskCode);
        //if (InteractingDevice.Status == DeviceStatus.Working)
        //{
        //    var agvAdjustHeightAckPayload = new AgvAdjustHeightAckPayload()
        //    {
        //        body = new AgvAdjustHeightAckBody()
        //        {
        //            sn = InteractingDevice.DeviceId,
        //            taskCode = taskCode,
        //            code = 500,
        //            msg = "AGV正在执行任务中，请等待AGV完成任务后再下发指令",
        //        }
        //    };
        //    await _mqttClient.PublishStringAsyncEnhance(ackTopic, agvAdjustHeightAckPayload, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
        //    return true;
        //}

    }
}
