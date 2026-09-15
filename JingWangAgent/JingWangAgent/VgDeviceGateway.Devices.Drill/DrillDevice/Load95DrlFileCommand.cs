// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.DrillDevice;
internal class Load95DrlFileCommand
{

    private readonly ILogger<Load95DrlFileCommand> logger;
    private readonly DefaultDrill _drillDevice;

    public Load95DrlFileCommand(ILogger<Load95DrlFileCommand> logger, DefaultDrill drillDevice)
    {
        this.logger = logger;
        _drillDevice = drillDevice;
    }

    public async Task<DeviceServiceInvokeResponse> SetScannerLot(DeviceServiceInvokeRequest request)
    {
        string msg = string.Empty;
        string itemCode = string.Empty;
        try
        {
            Int32 strPgmState;
            try
            {
                strPgmState = _drillDevice.cnc84Command.ReadCncNode<Int32>("ns=4;s=UI/normalized/new/ProgramState");
            }
            catch (Exception ex)
            {
                msg = $"SetScannerLot 读取程式执行状态失败，Node=ns=4;s=UI/normalized/new/ProgramState, ex={ex.Message}";
                logger.LogError(msg);
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = msg
                };
            }
            logger.LogInformation($"SetScannerLot 读取程式执行状态为 {strPgmState}");
            if (strPgmState == 0 || strPgmState == 1 || strPgmState == 5)
            {
                logger.LogInformation($"SetScannerLot SetDrillCommand SetDrillHandler request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
                if (request == null || request.Params == null || !request.Params.ContainsKey("ItemCode") || string.IsNullOrEmpty(request.Params["ItemCode"].ToString()))
                {
                    msg = "SetScannerLot 中控下发的批次号(SetScannerLot)为空";
                    logger.LogError(msg);
                    return new DeviceServiceInvokeResponse
                    {
                        Code = ErrorCodes.Sys.FAIL,
                        Message = msg
                    };
                }
                logger.LogInformation($"SetScannerLot 收到中控下发的Lot:{JsonSerializer.Serialize(request.Params["ItemCode"].ToString(), new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
                itemCode = request.Params["ItemCode"].ToString();
                DrillInfo drillInf;
                try
                {
                    drillInf = _drillDevice.DrillFilePathLocator.GetFilePath($"{itemCode}");
                    if (drillInf == null)
                    {
                        msg = $"SetScannerLot GetFilePath 从数据库获取{itemCode} 的 DrillInfo 为空";
                        logger.LogError(msg);
                        return new DeviceServiceInvokeResponse
                        {
                            Code = ErrorCodes.Sys.FAIL,
                            Message = msg
                        };
                    }
                }
                catch (Exception ee)
                {
                    msg = $"SetScannerLot GetFilePath 从数据库获取{itemCode} 的DrillInfo 失败 {ee.Message}";
                    logger.LogError(msg);
                    return new DeviceServiceInvokeResponse
                    {
                        Code = ErrorCodes.Sys.FAIL,
                        Message = msg
                    };
                }

                logger.LogInformation($"SetScannerLot 数据库交互获取数据 {itemCode}, {JsonSerializer.Serialize(drillInf)}");
                if (string.IsNullOrWhiteSpace(drillInf.DrlPath))
                {
                    msg = $"SetScannerLot 和数据库交互获取钻带文件不存在,数据库返回数据{itemCode}, {JsonSerializer.Serialize(drillInf)}";
                    logger.LogError(msg);
                    return new DeviceServiceInvokeResponse
                    {
                        Code = ErrorCodes.Sys.FAIL,
                        Message = msg
                    };
                }
                //向中控要转化后的路径
                var drillPathUrl = string.Format(_drillDevice.DeviceDescriptor.Extra["GetAfterDrillPath"].ToStr(), _drillDevice.DeviceDescriptor.DeviceId, drillInf.DrlPath, itemCode);
                var drillPathResult = await _drillDevice.HttpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(drillPathUrl);
                if (drillPathResult == null)
                {
                    msg = $"SetScannerLot 向中控请求获取转化后钻带程序 {drillPathUrl} 返回为空";
                    logger.LogError(msg);
                    _drillDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_GET_AFTER_DRILL_PATH_URL");
                    return new DeviceServiceInvokeResponse
                    {
                        Code = ErrorCodes.Sys.FAIL,
                        Message = msg
                    };
                }
                logger.LogError($"SetScannerLot 向中控请求转化后{drillPathUrl}的返回值为{JsonSerializer.Serialize(drillPathResult)}");
                if (!drillPathResult.ContainsKey("AfterDrillPath") || string.IsNullOrEmpty(drillPathResult["AfterDrillPath"].ToString()))
                {
                    msg = $"SetScannerLot 向中控请求{drillPathUrl} 返回值中 不包括 AfterDrillPath 或 AfterDrillPath为空，返回值为{JsonSerializer.Serialize(drillPathResult)}";
                    logger.LogError(msg);
                    _drillDevice.cnc84Command.SetCncComand($"DSP,CENTRAL_NOT_CONTAINSKEY_AFTERDRILLPATH");
                    return new DeviceServiceInvokeResponse
                    {
                        Code = ErrorCodes.Sys.FAIL,
                        Message = msg
                    };
                }
                string afterDrillPath = drillPathResult["AfterDrillPath"].ToStr();
                string drillPath = afterDrillPath;
                string diaPath = drillInf.DiaPath;
                if (!LoadFileToCNC84(drillPath, diaPath))
                {
                    msg = $"SetScannerLot 加载程序到CNC 失败，itemcode={itemCode}, drl={drillPath}, dia={diaPath}";
                    logger.LogError(msg);
                    _drillDevice.cnc84Command.SetCncComand($"DSP, LOAD_DRILL_FAIL");
                    return new DeviceServiceInvokeResponse
                    {
                        Code = ErrorCodes.Sys.FAIL,
                        Message = msg
                    };
                }
                else
                {
                    msg = $"SetScannerLot 加载文件成功，itemcode={itemCode}, drl={drillPath}, dia={diaPath}";
                    logger.LogInformation(msg);
                    return new DeviceServiceInvokeResponse
                    {
                        Code = ErrorCodes.Sys.SUCCESS,
                        Message = msg
                    };
                }
            }
            else
            {
                msg = $"SetScannerLot 设备为工作或停止状态，不能加载文件，programState={strPgmState}";
                logger.LogError(msg);
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = msg
                };
            }

            /*SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = DeviceDescriptor.Extra["ConnLocalDBStr"].ToString(),
                DbType = SqlSugar.DbType.MySql,
                IsAutoCloseConnection = true
            });
            Db.Open();

            logger.LogError($"开始写入ScannerLot: {lotCode}");
            var affectedRows = Db.Insertable(new PC_SIGNAL_STATUS() { CREATETIME = DateTime.Now, SIGNALCODE = "SCANLOTUPLOAD", SIGNALDES = "设备通知EAP扫描LOT号", SIGNALVALUE = $"{lotCode}" }).ExecuteCommand();
            if (affectedRows > 0)
            {
                msg = $"PC_SIGNAL_STATUS 写入数据OK:SIGNALCODE =  SCANLOTUPLOAD,SIGNALDES = 设备通知EAP扫描LOT号, SIGNALVALUE ={lotCode}";
                logger.LogDebug(msg);
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.SUCCESS,
                    Message = msg
                };
            }
            else
            {
                msg = $"PC_SIGNAL_STATUS 写入数据Fail :SIGNALCODE =  SCANLOTUPLOAD,SIGNALDES = 设备通知EAP扫描LOT号, SIGNALVALUE ={lotCode}";
                logger.LogError(msg);
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = msg
                };
            }*/
        }
        catch (Exception ex)
        {
            msg = $"SetScannerLot 获取itemcode加载文件失败{itemCode}, {ex.Message}";
            logger.LogError(msg);
            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = msg
            };
        }
    }

    private bool LoadFileToCNC84(string drillPath, string diaPath = "")
    {
        _drillDevice.cnc84Command.SetCncComand($"DSP, LOADING_FILE");
        Thread.Sleep(1000);
        if (!LoadFile(drillPath, diaPath))
        {
            return false;
        }
        return true;
    }

    public bool LoadFile(string drilPath, string? diaPath = "")
    {
        if (!string.IsNullOrWhiteSpace(drilPath) && !ValidateDrlFileSame(drilPath))
        {
            _drillDevice.cnc84Command.SetCncComand("CM@@@");
            if (!ValidateCmDrlFile())
            {
                logger.LogError(" 加载文件 发送CM@@@ 执行失败");
                return false;
            }
            Thread.Sleep(1000);
            _drillDevice.cnc84Command.SetCncComand("CD@@@");
            if (!ValidateCdFile())
            {
                logger.LogError(" 加载文件  发送CD@@@ 执行失败");
                return false;
            }
            Thread.Sleep(1000);
            //加载dia文件
            //加载drl文件
            _drillDevice.cnc84Command.SendLoadFile(diaPath, "DIAMETERTABLE");
            Thread.Sleep(3000);
            if (!ValidateDiaFile(diaPath))
            {
                logger.LogError(" 加载文件  验证加载dia文件失败");
                return false;
            }
            Thread.Sleep(3000);
            _drillDevice.cnc84Command.SendLoadFile(drilPath, "PROGRAM");
            Thread.Sleep(3000);
            //SendLoadFile(drilPath, "PROGRAM");
            if (!ValidateDrlFile(drilPath))
            {
                logger.LogError(" 加载文件  验证加载pro文件失败");
                return false;
            }
            //验证是否重新加载
            if (!ValidateDrlImg())
            {
                logger.LogError(" 加载文件  解析程序失败");
                return false;
            }
        }
        return true;
    }

    private bool ValidateDrlImg()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        do
        {
            Thread.Sleep(1000);
            string blockText = _drillDevice.cnc84Command.ReadCncNode<string>("ns=4;s=UI/origin/RosiInfo/BlockText");
            logger.LogDebug($"ValidateDrlImg  {blockText}");
            if (!string.IsNullOrEmpty(blockText) && (blockText.Contains("等待开始") || blockText.Contains("Waiting for start")))
            {
                logger.LogDebug($"ValidateDrlImg  验证结束");
                stopwatch.Stop();
                return true;
            }
        } while (stopwatch.ElapsedMilliseconds < _drillDevice.DeviceDescriptor.Extra["ValidateCmFileTimeout"].ToLong());
        return false;
    }

    private bool ValidateDrlFile(string drilFilePath)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var netProName = string.Empty;
        do
        {
            Thread.Sleep(100);
            netProName = _drillDevice.cnc84Command.GetACTProgram();
            if (string.IsNullOrEmpty(netProName))
            {
                continue;
            }
            var modifyDrilFilePath = drilFilePath.Trim().Replace("\\", "").Replace("/", "").ToLower().Trim();
            var modifyNextProName = netProName.Trim().Replace("\\", "").Replace("/", "").ToLower().Trim();
            logger.LogInformation($"校验程序名 modifyDrilFilePath:{modifyDrilFilePath} modifyNextProName{modifyNextProName}");
            if (modifyNextProName.Equals(modifyDrilFilePath, StringComparison.CurrentCultureIgnoreCase))
            {
                stopwatch.Stop();
                return true;
            }
        } while (stopwatch.ElapsedMilliseconds < _drillDevice.DeviceDescriptor.Extra["ValidateDrlFileTimeout"].ToLong());
        return false;
    }

    private bool ValidateCdFile()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var netProName = string.Empty;
        do
        {
            Thread.Sleep(1000);
            netProName = _drillDevice.cnc84Command.GetDiaFileNameWithDialog();
            if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName))
            {
                stopwatch.Stop();
                logger.LogError($"CD@@@ 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                return true;
            }
            _drillDevice.cnc84Command.SetCncComand("CD@@@");
        } while (stopwatch.ElapsedMilliseconds < _drillDevice.DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
        return false;
    }

    private bool ValidateCmDrlFile()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var netProName = string.Empty;
        do
        {
            Thread.Sleep(1000);
            netProName = _drillDevice.cnc84Command.GetACTProgram();
            if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName) || "????".Equals(netProName))
            {
                stopwatch.Stop();
                logger.LogError($"CM@@@ 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                return true;
            }
            _drillDevice.cnc84Command.SetCncComand("CM@@@");
        } while (stopwatch.ElapsedMilliseconds < _drillDevice.DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
        return false;
    }

    private bool ValidateDrlFileSame(string drlFilePath)
    {
        var drlName = _drillDevice.cnc84Command.GetACTProgram();
        if (string.IsNullOrEmpty(drlName))
        {
            return false;
        }
        if ($"{drlFilePath.Replace("\\", "").Replace("/", "").ToLower()}".Equals(drlName.Replace("\\", "").Replace("/", "").ToLower(), StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    private bool ValidateDiaFile(string diaFilePath)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var diaName = string.Empty;
        do
        {
            Thread.Sleep(50);
            //opcUaClient.ReadNode<String>("ns=4;s=UI/origin/FilesAndPaths/ToolFiles/DiameterGroup/DiameterTable");
            diaName = _drillDevice.cnc84Command.GetRuntimeString("%S(DiaFileNameWithDialog)");
            if (string.IsNullOrEmpty(diaName))
            {
                continue;
            }
            if ($"1:{diaFilePath.Replace("\\", "").Replace("/", "").ToLower()}".Equals(diaName.Replace("\\", "").Replace("/", "").ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                logger.LogError($"ValidateDiaFile 用时 {stopwatch.ElapsedMilliseconds} Millisecond");
                stopwatch.Stop();
                return true;
            }
        } while (stopwatch.ElapsedMilliseconds < _drillDevice.DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
        stopwatch.Stop();
        return false;
    }
}
