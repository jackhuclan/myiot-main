// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Drill.DrillDevice;
internal class Load95AtpCommand
{
    private readonly ILogger<Load95AtpCommand> logger;
    private readonly DefaultDrill _drillDevice;

    public Load95AtpCommand(ILogger<Load95AtpCommand> logger, DefaultDrill drillDevice)
    {
        this.logger = logger;
        _drillDevice = drillDevice;
    }


    public async Task<DeviceServiceInvokeResponse> LoadAtpFile(DeviceServiceInvokeRequest request)
    {
        var response = DeviceServiceInvokeResponse.FAIL;

        try
        {
            logger.LogInformation($"SetDrillCommand LoadAptFile request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
            if (request == null || request.Params == null)
            {
                response.Message = "中控下发的request 为空 ";
                logger.LogInformation($"LoadAptFile COMMAND {response.Message}");
                return response;
            }
            if (!request.Params.ContainsKey("AtpFile") || string.IsNullOrEmpty(request.Params["AtpFile"].ToString()))
            {
                response.Message = "中控下发的AtpFile 为空";
                logger.LogInformation($"LoadAptFile COMMAND {response.Message}");
                return response;
            }
            if (!request.Params.ContainsKey("GroupNo") || string.IsNullOrEmpty(request.Params["GroupNo"].ToString()))
            {
                response.Message = "中控下发的GroupNo 为空";
                logger.LogInformation($"LoadAptFile  COMMAND {response.Message}");
                return response;
            }
            if (isWorking())
            {
                response.Message = "cnc 在工作状态不能加载atp";
                logger.LogInformation($"LoadAptFile  COMMAND {response.Message}");
                return response;
            }

            string atpPath = request.Params["AtpFile"].ToString();
            string groupNo = request.Params["GroupNo"].ToString();
            var loadResult = false;
            try
            {
                loadResult = LoadAtpFile(atpPath);
                if (loadResult)
                {
                    logger.LogInformation($"LoadAptFile  COMMAND 加载成功");
                    return DeviceServiceInvokeResponse.SUCCESS;
                }
                response.Message = "cnc  加载atp失败";
                logger.LogInformation($"LoadAptFile  {response.Message}");
            }
            finally
            {
                logger.LogInformation($"LoadAptFile 加载的结果 {loadResult}");
                if (!loadResult)
                {
                    await ReportResult(groupNo, CUTTERGROUPSTATUS.Lock);
                    logger.LogInformation($"上报atp加载失败  组id {groupNo}");
                }
            }
        }
        catch (Exception ee)
        {
            logger.LogInformation($"LoadAptFile 异常 {ee.Message}");
            response.Message = ee.Message;
        }

        return response;
    }



    private async Task ReportResult(string strGroupNo, CUTTERGROUPSTATUS cUTTERGROUPSTATUS)
    {
        try
        {
            string strUpdateCutterUrl = _drillDevice.DeviceDescriptor.Extra["UpdateCutterGroupStatus"].ToStr();
            UpdateCutterGroupRequest updateCutterGroupRequest = new UpdateCutterGroupRequest() { groupNo = strGroupNo, groupStatus = (int)cUTTERGROUPSTATUS };
            await _drillDevice.HttpRequestInvoker.PostAsJsonAsync<UpdateCutterGroupRequest, UpdateCutterGroupResponse>(strUpdateCutterUrl, updateCutterGroupRequest);
        }
        catch (Exception ee)
        {
            logger.LogError($" 上报atp加载结束  {ee.Message}");
        }
    }

    private bool isWorking()
    {
        var progState = _drillDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/TimeWidget/ProgramState");
        logger.LogError($" 上报atp加载结束  {progState}");
        return new int[] { 2, 3, 4 }.Contains(progState);
    }

    private bool LoadAtpFile(string atpPath)
    {
        bool IsLoadSuccess = false;
        try
        {
            var atpName = _drillDevice.cnc84Command.GetAtpFileName();
            if (!string.IsNullOrEmpty(atpName))
            {
                _drillDevice.cnc84Command.SetCncComand("CA@@@");
                if (!ValidateCaFile())
                {
                    logger.LogError(" 加载atp文件 发送CA@@@ 执行失败");
                    return IsLoadSuccess;
                }
                Thread.Sleep(1000);
            }
            _drillDevice.cnc84Command.SendLoadFile(atpPath, "ATP");
            Thread.Sleep(1000);
            if (ValidateAtpFile(atpPath))
            {
                IsLoadSuccess = true;
                return IsLoadSuccess;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message + " 加载文件 发送CA@@@ 执行异常");
        }

        return IsLoadSuccess;
    }

    private bool ValidateCaFile()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var netProName = string.Empty;
        do
        {
            Thread.Sleep(1000);
            netProName = _drillDevice.cnc84Command.GetAtpFileName();
            if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName))
            {
                stopwatch.Stop();
                logger.LogError($"CA@@@ 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                return true;
            }
            _drillDevice.cnc84Command.SetCncComand("CA@@@");
        } while (stopwatch.ElapsedMilliseconds < _drillDevice.DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
        return false;
    }

    private bool ValidateAtpFile(string atpFilePath)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var atpName = string.Empty;
        do
        {
            Thread.Sleep(50);
            //opcUaClient.ReadNode<String>("ns=4;s=UI/origin/FilesAndPaths/ToolFiles/DiameterGroup/DiameterTable");
            atpName = _drillDevice.cnc84Command.GetAtpFileName();
            if (string.IsNullOrEmpty(atpName))
            {
                continue;
            }
            if ($"{atpFilePath.Replace("\\", "").Replace("/", "").ToLower()}".Equals(atpName.Replace("\\", "").Replace("/", "").ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                logger.LogError($"ValidateAtpFile 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                stopwatch.Stop();
                return true;
            }
        } while (stopwatch.ElapsedMilliseconds < _drillDevice.DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
        stopwatch.Stop();
        return false;
    }
}
