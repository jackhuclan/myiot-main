// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot.Models;
using VgDeviceGateway.Devices.Drill;

namespace Vegalot.External.XianjinIot.Drill.Command.Drill;
internal class BaseSimpleCommand : SimpleCommand<DefaultDrill>
{
    public ConcurrentDictionary<string, bool> CommandsStatus = new ConcurrentDictionary<string, bool>();//线程安全

    public BaseSimpleCommand(IServiceProvider serviceProvider, DefaultDrill device, CommandDescriptor commandDescriptor) : base(serviceProvider, device, commandDescriptor)
    {
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
    protected async Task AddTaskCode(string taskCode)
    {
        if (CommandsStatus.Count >= 100)//防止无限膨胀
        {
            CommandsStatus.Clear();
        }
        CommandsStatus.AddOrUpdate(taskCode, key => true, (key, oldValue) => true);//第二个参数是不存在情况下的值，第三个参数是存在情况下的值       
        await Task.CompletedTask;
    }

    protected async Task RemoveTaskCodeRecord(string taskCode)
    {
        CommandsStatus.TryRemove(taskCode, out _);//超时或者初始化，终止信号循环接收，要及时清理掉字典里的taskCode
        await Task.CompletedTask;
    }
    /// <summary>
    /// 检测命令正在执行中
    /// </summary>
    /// <returns></returns>
    protected async Task<bool> CheckCommandIsWorking(string taskCode)
    {
        await Task.CompletedTask;
        return CommandsStatus.ContainsKey(taskCode);
    }
}
