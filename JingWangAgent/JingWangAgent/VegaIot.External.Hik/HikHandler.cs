using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv.Hik;
using static VgDeviceGateway.Devices.Common.Agv.Hik.HikCarStatus;

namespace VegaIot.External.Hik;

public class HikHandler : DeviceShare<HikTransfer>
{
    private readonly ILogger<HikHandler> _logger;
    private readonly CentralReporter _centralReporter;
    private readonly bool _isSendToKW = true;
    private int delayHikTask = 1000;

    public HikHandler(IServiceProvider serviceProvider,
        IObjectFactory objectFactory,
        ILoggerFactory loggerFactory,
        HikTransfer device) : base(serviceProvider, device)
    {
        _logger = loggerFactory.CreateLogger<HikHandler>();
        _centralReporter = objectFactory.GetOrCreate<CentralReporter>(device);
        _isSendToKW = DeviceDescriptor.Extra["IsSendToKW"].ToBool();
    }

    //海康接口模版: WJ02 料架，点位，Lot 三个绑定， WJ01 料架，点位，Lot 不绑定
    /// <summary>
    /// 下熟料
    /// </summary>
    /// <param name="currentContext"></param>
    /// <param name="TargetPos"></param>
    /// <param name="ClinkerStore"></param>
    /// <returns></returns>
    public async Task UnLoadClinker(CurrentContext currentContext, string TargetPos, string[] ClinkerStore)
    {
        object[] tempArry = new object[2];
        DeviceServiceInvokeRequest deviceServiceInvokeRequest = currentContext.HikTaskRequest;
        deviceServiceInvokeRequest.Params["podStatus"] = "0";

        int sumPcs = 1;
        if (DeviceDescriptor.Extra["IsPcs"].ToBool())  ///device.DeviceDescriptor.Extra["IsPcs"].ToBool() 取是否查询片数配置
        {
            string pcs = await QueryGetWorkOrderInfo(currentContext.InternalLotNo, "PanelCount");
            if (pcs == "" || pcs == "0")
            {
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), "未获取到熟料片数，等待下次获取");
            }

            sumPcs = currentContext.RawPanelCount * pcs.ToInt();
        }
        else
        {
            sumPcs = currentContext.RawPanelCount;
        }

        if (sumPcs == 0)
        {
            var msg = $"Fail,料仓{currentContext.SiloCode}的物料{currentContext.InternalLotNo}下熟料时物料{currentContext.InternalLotNo}没有熟料数量为0，不能执行";
            deviceServiceInvokeRequest.Params["errMsg"] = msg;
            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
            await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Failed, currentContext);
            currentContext.Reset();
            return;
        }

        currentContext.SumPcs = sumPcs;
        deviceServiceInvokeRequest.Params["lotQty"] = sumPcs;
        deviceServiceInvokeRequest.Params["materialLot"] = currentContext.ExternalLotNo;
        deviceServiceInvokeRequest.Params["taskTypes"] = "WJ02";
        deviceServiceInvokeRequest.Params["operationType"] = "下熟料";
        deviceServiceInvokeRequest.Params["SiloCode"] = currentContext.SiloCode;
        deviceServiceInvokeRequest.Params["LocationCode"] = currentContext.LocationCode;

        if (string.IsNullOrEmpty(currentContext.ExternalLotNo)
            || !currentContext.ExternalLotNo.Contains("@")
            || currentContext.ExternalLotNo.Split('@')[0] != currentContext.InternalLotNo)
        {
            var msg2 = $"Fail,料仓{currentContext.SiloCode}的物料{currentContext.InternalLotNo}下熟料时物料{currentContext.InternalLotNo}没有熟料料号或者对内料号与对外料号不一致";
            deviceServiceInvokeRequest.Params["errMsg"] = msg2;

            await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Failed, currentContext);
            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg2);
            _logger.LogDebug(msg2);
            return;
        }

        deviceServiceInvokeRequest.Params["LocationCode"] = currentContext.LocationCode;
        string dr1 = await QueryGetWorkOrderInfo(currentContext.InternalLotNo, "SpecGroup");

        if (string.IsNullOrEmpty(dr1))
        {
            var msg3 = $"{currentContext.TransferId}下熟料时，没有获取到工艺分组，等待下次执行";
            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg3);
            currentContext.Reset();
            return;
        }

        currentContext.SpecGroup = dr1;
        currentContext.HikTaskCode = "Vg" + currentContext.LocationCode + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        var rwpotpres = await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Running, currentContext);

        if (rwpotpres.Code == 0)
        {
            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{currentContext.TransferId}给海康派发 下熟料 的任务");
            delayHikTask = DeviceDescriptor.Extra["DelayHikTask"].ToInt();
            bool created = false;
            bool notMatchedSiloCode = false;

            for (int i = 0; i < ClinkerStore.Length; i++)
            {
                tempArry[0] = TargetPos;
                tempArry[1] = ClinkerStore[i] + "${04}";
                var res = await SendTaskToHik(currentContext, tempArry);

                if (res != null && res.Code == 0)
                {
                    var msg = $"库区{ClinkerStore[i]}找到空位，可以下熟料。熟料来源：库位{currentContext.LocationCode}," +
                        $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}派发成功";
                    _logger.LogInformation(msg);
                    _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                    created = true;
                    break;
                }
                else
                {
                    string temp = !string.IsNullOrEmpty(res.Message) ? res.Message : "";
                    var msg = $"{ClinkerStore[i]}创建海康下熟料任务失败:{temp}";
                    _logger.LogInformation(msg);
                    _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                }

                if (res != null
                    && !string.IsNullOrEmpty(res.Message)
                    && res.Message.Contains("传入货架绑定的物料号")
                    && res.Message.Contains("不一致"))
                {
                    notMatchedSiloCode = true;
                }

                await Task.Delay(delayHikTask);
            }

            if (!created)
            {
                var msg4 = $"库位{currentContext.LocationCode}的下熟料任务," +
                    $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}派发失败";
                _logger.LogInformation(msg4);

                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg4);

                _ = DataExporter.DeviceAlarmReport(new DeviceAlarmReportRequest
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    RequestDeviceKind = DeviceDescriptor.DeviceKind,
                    AlarmCode = "NO_EMPTY_POSITION_FOR_DRILLED_TO_TRACK_OUT",
                    AlarmName = "海康库存找不到空位可以下熟料",
                    AlarmContent = msg4,
                    AlarmLevel = AlarmLevel.Severe,
                    AlarmTime = DateTime.Now,
                    AlarmKind = AlarmKind.Unknown,
                });

                deviceServiceInvokeRequest.Params["errMsg"] = msg4;
                await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Failed, currentContext);

                currentContext.Reset();

                if (notMatchedSiloCode)
                {
                    var bindResult = await UnbindLineSideStock(new BindLineSideStock
                    {
                        reqCode = "VegaTask" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        reqTime = "",
                        clientCode = "",
                        podCode = currentContext.SiloCode,
                        indBind = "0",
                        mapDataCode = "",
                        materialLot = currentContext.ExternalLotNo,
                        materialGroupCode = "",
                        materialCode = "",
                        sysLotNum = "",
                        podLotNum = "",
                        lotStatus = "OK",
                        includeScrap = "0"
                    });

                    if (bindResult == "success")
                    {
                        _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{currentContext.ExternalLotNo}解绑成功");
                    }
                    else
                    {
                        _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{currentContext.ExternalLotNo}解绑失败");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 下空仓
    /// </summary>
    /// <param name="currentContext"></param>
    /// <param name="TargetPos"></param>
    /// <param name="EmptyStore"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public async Task UnLoadEmptySilo(CurrentContext currentContext, string TargetPos, string[] EmptyStore)
    {
        object[] tempArry = new object[2];
        DeviceServiceInvokeRequest deviceServiceInvokeRequest = currentContext.HikTaskRequest;
        deviceServiceInvokeRequest.Params["podStatus"] = "0";
        deviceServiceInvokeRequest.Params["lotQty"] = "0";
        deviceServiceInvokeRequest.Params["materialLot"] = "";
        deviceServiceInvokeRequest.Params["taskTypes"] = "WJ02";
        deviceServiceInvokeRequest.Params["operationType"] = "下空仓";
        deviceServiceInvokeRequest.Params["SiloCode"] = currentContext.SiloCode;

        string rackIndex = (currentContext.RackIndex + 1).ToString();
        deviceServiceInvokeRequest.Params["LocationCode"] = currentContext.LocationCode;

        currentContext.HikTaskCode = "Vg" + currentContext.LocationCode + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        var ReportRes = await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Running, currentContext);

        if (ReportRes.Code == 0)
        {
            delayHikTask = DeviceDescriptor.Extra["DelayHikTask"].ToInt();
            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{currentContext.TransferId}给海康派送 下空仓 的任务");
            bool created = false;

            for (int i = 0; i < EmptyStore.Length; i++)
            {
                tempArry[0] = TargetPos;
                tempArry[1] = EmptyStore[i] + "${04}";

                var res = await SendTaskToHik(currentContext, tempArry);

                if (res != null && res.Code == 0)
                {
                    var msg = $"库区{EmptyStore[i].ToString()}上找到空仓，准备派发给库位{currentContext.LocationCode}," +
                        $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}派发成功";
                    _logger.LogInformation(msg);
                    _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                    created = true;
                    break;
                }
                else
                {
                    string temp = !string.IsNullOrEmpty(res.Message) ? res.Message : "";
                    var msg = $"库区{EmptyStore[i].ToString()}上找不到空仓或者创建海康转入空仓任务失败:{temp}";
                    _logger.LogInformation(msg);
                    _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                }

                await Task.Delay(delayHikTask);
            }

            if (!created)
            {
                var msg = $"库位{currentContext.LocationCode}的下空仓任务," +
                        $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}，创建海康转入空仓任务失败";
                _logger.LogInformation(msg);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                _ = DataExporter.DeviceAlarmReport(new DeviceAlarmReportRequest
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    RequestDeviceKind = DeviceDescriptor.DeviceKind,
                    AlarmCode = "NOT_EXISTING_EMPTY_POSITION_TO_TRACK_OUT",
                    AlarmName = "海康库存无空料仓转出",
                    AlarmContent = msg,
                    AlarmLevel = AlarmLevel.Severe,
                    AlarmTime = DateTime.Now,
                    AlarmKind = AlarmKind.Unknown,
                });
                deviceServiceInvokeRequest.Params["errMsg"] = msg;
                await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Failed, currentContext);
                currentContext.Reset();
            }
        }
    }

    /// <summary>
    /// 上生料
    /// </summary>
    /// <param name="currentContext"></param>
    /// <param name="TargetPos"></param>
    /// <param name="LineStore"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public async Task LoadRaw(CurrentContext currentContext, string TargetPos, string[] LineStore)
    {
        object[] tempArry = new object[2];
        string materialLot = currentContext.InternalLotNo == null ? currentContext.ItemCode : currentContext.InternalLotNo;
        delayHikTask = DeviceDescriptor.Extra["DelayHikTask"].ToInt();
        DeviceServiceInvokeRequest deviceServiceInvokeRequest = currentContext.HikTaskRequest;
        deviceServiceInvokeRequest.Params["LocationCode"] = currentContext.LocationCode;
        string rackIndex = (currentContext.RackIndex + 1).ToString();
        deviceServiceInvokeRequest.Params["materialLot"] = materialLot;

        _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{materialLot}上生料任务{currentContext.TransferId}开始查询海康库存");
        bool created = false;

        for (int i = 0; i < LineStore.Length; i++)
        {
            var lotData = await GetLotData(materialLot, LineStore[i].ToString(), 1);
            if (lotData == null
                || string.IsNullOrEmpty(lotData.lot)
                || lotData.qty == 0)
            {
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"库区{LineStore[i].ToString()}上未查询到Lot:{materialLot}绑定料架的信息");
                continue;
            }

            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"库区{LineStore[i].ToString()}上查询到Lot:{materialLot}绑定料架的信息，准备派发给海康...");

            deviceServiceInvokeRequest.Params["podStatus"] = "1";
            deviceServiceInvokeRequest.Params["lotQty"] = lotData.qty.ToString();
            deviceServiceInvokeRequest.Params["materialLot"] = lotData.lot;
            deviceServiceInvokeRequest.Params["taskTypes"] = "WJ01";
            deviceServiceInvokeRequest.Params["operationType"] = "上生料";
            deviceServiceInvokeRequest.Params["SiloCode"] = "";
            tempArry[0] = lotData.lot + "${01}";
            tempArry[1] = TargetPos;

            currentContext.HikTaskCode = "Vg" + currentContext.LocationCode + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var reportStatus = await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Running, currentContext);
            if (reportStatus.Code == 0)
            {
                var res = await SendTaskToHik(currentContext, tempArry);
                if (res != null && res.Code == 0)
                {
                    var msg2 = $"库区{LineStore[i].ToString()}上查询到Lot:{materialLot}，库位{currentContext.LocationCode}给海康下发上生料任务成功{currentContext.HikTaskCode},料仓号{currentContext.SiloCode}";
                    _logger.LogInformation(msg2);
                    _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg2);
                    created = true;
                    break;
                }
                else
                {
                    string temp = !string.IsNullOrEmpty(res.Message) ? res.Message : "";
                    var msg = $"{LineStore[i]}创建海康任务失败:{temp}";
                    _logger.LogInformation(msg);
                    _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                }
            }
            else
            {
                _logger.LogError($"上报中控ScheduledTaskStatus.Running失败,库位{currentContext.LocationCode}，任务号：{currentContext.TransferId}");
            }

            await Task.Delay(delayHikTask);
        }

        if (!created)
        {
            var msg3 = $"库位{currentContext.LocationCode}的上生料任务," +
                    $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}派发给海康失败";
            _logger.LogInformation(msg3);
            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg3);
            _ = DataExporter.DeviceAlarmReport(new DeviceAlarmReportRequest
            {
                ProductId = DeviceDescriptor.ProductId,
                DeviceId = DeviceDescriptor.DeviceId,
                RequestDeviceKind = DeviceDescriptor.DeviceKind,
                AlarmCode = "NO_UNDRILLED_TO_TRACK_IN",
                AlarmName = "海康库存无所需生料",
                AlarmContent = msg3,
                AlarmLevel = AlarmLevel.Severe,
                AlarmTime = DateTime.Now,
                AlarmKind = AlarmKind.Unknown,
            });
            deviceServiceInvokeRequest.Params["errMsg"] = msg3;
            await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Failed, currentContext);
            currentContext.Reset();
        }
    }

    /// <summary>
    /// 上空仓
    /// </summary>
    /// <param name="currentContext"></param>
    /// <param name="TargetPos"></param>
    /// <param name="EmptyStore"></param>
    /// <returns></returns>
    public async Task LoadEmptySilo(CurrentContext currentContext, string TargetPos, string[] EmptyStore)
    {
        object[] tempArry = new object[2];
        DeviceServiceInvokeRequest deviceServiceInvokeRequest = currentContext.HikTaskRequest;
        deviceServiceInvokeRequest.Params["podStatus"] = "";
        deviceServiceInvokeRequest.Params["lotQty"] = "0";
        deviceServiceInvokeRequest.Params["materialLot"] = "";
        deviceServiceInvokeRequest.Params["taskTypes"] = "WJ01";
        deviceServiceInvokeRequest.Params["operationType"] = "上空仓";
        deviceServiceInvokeRequest.Params["SiloCode"] = "";
        deviceServiceInvokeRequest.Params["LocationCode"] = currentContext.LocationCode;
        string rackIndex = (currentContext.RackIndex + 1).ToString();

        currentContext.HikTaskCode = "Vg" + currentContext.LocationCode + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        var reportRes = await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Running, currentContext);

        if (reportRes.Code == 0)
        {
            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{currentContext.TransferId}给海康派送 上空仓 的任务");
            delayHikTask = DeviceDescriptor.Extra["DelayHikTask"].ToInt();
            bool created = false;

            for (int i = 0; i < EmptyStore.Length; i++)
            {
                tempArry[0] = EmptyStore[i] + "${04}";
                tempArry[1] = TargetPos;

                var res = await SendTaskToHik(currentContext, tempArry);

                if (res != null && res.Code == 0)
                {
                    var msg = $"库位{currentContext.LocationCode}给海康下发上空仓任务成功{currentContext.HikTaskCode},料仓号{currentContext.SiloCode}";
                    _logger.LogInformation(msg);
                    _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                    created = true;
                    break;
                }
                else
                {
                    string temp = !string.IsNullOrEmpty(res.Message) ? res.Message : "";
                    var msg = $"查询到{EmptyStore[i]}没有空仓或者创建海康任务失败:{temp}";
                    _logger.LogInformation(msg);
                    _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                }

                await Task.Delay(delayHikTask);
            }

            if (!created)
            {
                var msg = $"库位{currentContext.LocationCode}的上空仓任务," +
                    $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}派发给海康失败";
                _logger.LogInformation(msg);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                _ = DataExporter.DeviceAlarmReport(new DeviceAlarmReportRequest
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    RequestDeviceKind = DeviceDescriptor.DeviceKind,
                    AlarmCode = "NOT_EXISTING_EMPTY_SILO_TO_TRACK_IN",
                    AlarmName = "海康库存无空料仓转入",
                    AlarmContent = msg,
                    AlarmLevel = AlarmLevel.Severe,
                    AlarmTime = DateTime.Now,
                    AlarmKind = AlarmKind.Unknown,
                });
                deviceServiceInvokeRequest.Params["errMsg"] = msg;
                await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Failed, currentContext);
                currentContext.Reset();
            }
        }
    }

    /// <summary>
    /// 解绑线边仓物料和料仓的绑定
    /// </summary>
    /// <param name="bindLineSideStock"></param>
    /// <returns></returns>
    public async Task<string> UnbindLineSideStock(BindLineSideStock bindLineSideStock)
    {
        var sendTaskToAgv = DeviceDescriptor.Extra["bindLineSideStock"].ToString();//给AGV下任务
        _logger.LogDebug($"绑定-解绑 接口 bindLineSideStock 参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(bindLineSideStock))}");
        var result = await HttpRequestInvoker.PostAsJsonAsync<BindLineSideStock, BindLineSideStockRes?>(sendTaskToAgv, bindLineSideStock) ?? new();
        _logger.LogDebug($"绑定-解绑 接口 bindLineSideStock 的返回结果:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(result))}");

        if (result == null)
        {
            return $"料仓{bindLineSideStock.podCode}与物料{bindLineSideStock.materialLot} 解绑失败";
        }
        else if (result.code == "0")
        {
            return "success";
        }
        else
        {
            return result.message;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ItemCode"></param>
    /// <param name="Region"></param>
    /// <param name="style">0 空料仓，1 料仓有料</param>
    /// <returns></returns>
    private async Task<StockInfoQueryResData> GetLotData(string ItemCode, string Region, int style)
    {
        StockInfoQueryResData? sqr = new StockInfoQueryResData();
        var queryLot = new StockInfoQuery()
        {
            reqCode = "QLot" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            targetPosArea = Region + "${04}"
        };

        var queryDataUrl = DeviceDescriptor.Extra["StockInfoQuery"].ToStr();
        _logger.LogDebug($"查询接口 StockInfoQuery 参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(queryLot))}");
        var queryDataResult = await HttpRequestInvoker.PostAsJsonAsync<StockInfoQuery, StockInfoQueryRes>(queryDataUrl, queryLot);
        //logger.LogDebug($"查询接口 StockInfoQuery 的返回参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(queryDataResult))}");
        if (queryDataResult == null || queryDataResult.code != "0" || queryDataResult.data == null)
        {
            return sqr;
        }

        var lstLot = queryDataResult.data.Deserialize<List<StockInfoQueryResData>>();
        if (style == 1)
        {
            var lstInf = lstLot.Where(t => !string.IsNullOrEmpty(t.lot) && t.lot.ToUpper().Contains(ItemCode.ToUpper())
            && t.qty > 0
                 //  && t.lot.Contains("@")
                 && t.lot.StartsWith(ItemCode)
                 && t.lot.Split('@')[0] == ItemCode)
                .ToList()
                .OrderByDescending(t => t.qty)
                .FirstOrDefault();

            _logger.LogDebug($"查询接口 StockInfoQuery 的返回参数 料号  {ItemCode} 的 lstInf 1:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(lstInf))}");
            return lstInf;
        }
        else
        {
            var lstInf = lstLot.Where(t => string.IsNullOrEmpty(t.lot) && t.qty == 0 && string.IsNullOrEmpty(t.podCode)).FirstOrDefault();
            return lstInf;
        }
    }

    /// <summary>
    /// 海康agv任务完成的回调逻辑
    /// </summary>
    /// <param name="status"></param>
    /// <param name="materialData"></param>
    /// <returns></returns>
    public async Task CallbackHikAgvArrived(HikAgvCallBack status, MaterialData materialData)
    {
        var currentContext = this.InteractingDevice.Locations.Values.Select(x => x.CurrentContext)
            .FirstOrDefault(x => x.HikTaskCode == status.taskCode);

        if (currentContext == null)
        {
            _logger.LogDebug($"找不到对应的currentContext，任务{status.taskCode}上的回调处理失败!");
            return;
        }

        _logger.LogInformation($"收到库位{currentContext.LocationCode}的任务," +
            $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}的回调，任务号={status.taskCode}");

        try
        {
            if (currentContext.IsUpdatingPanels)
            {
                return;
            }

            currentContext.IsUpdatingPanels = true;

            if (currentContext.PanelType == 1 || currentContext.PanelType == 7)
            {
                _ = SendToKWClinker(currentContext);
            }

            await _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{status.taskCode}收到任务完成回调，准备更新库位{currentContext.LocationCode}板料...");

            await FillDataToHikTaskRequest(status, materialData, currentContext);

            await ExtractPanelList(currentContext, status);

            await UpdateTransferLocationPanels(currentContext, status);
        }
        finally
        {
            currentContext.IsUpdatingPanels = false;
        }
    }

    /// <summary>
    /// 海康agv离开库位就回调，从库位转出料仓，比如：1.下熟料 2.下空仓 7.下首件
    /// </summary>
    /// <param name="status"></param>
    /// <param name="materialData"></param>
    /// <returns></returns>
    public async Task Outbin(HikAgvCallBack status, MaterialData materialData)
    {
        var currentContext = this.InteractingDevice.Locations.Values.Select(x => x.CurrentContext)
            .FirstOrDefault(x => x.HikTaskCode == status.taskCode);

        if (currentContext == null)
        {
            _logger.LogDebug($"找不到对应的currentContext，任务{status.taskCode}上的回调处理失败!");
            return;
        }

        if (currentContext.Direction == "in")
        {
            //上传料仓信息给中控
            _ = UploadData(currentContext, materialData, status.podCode, status.taskCode, status.robotCode);
            return;
        }

        _logger.LogInformation($"收到库位{currentContext.LocationCode}的任务," +
            $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}的回调，任务号={status.taskCode}");

        try
        {
            if (currentContext.IsUpdatingPanels)
            {
                return;
            }

            currentContext.IsUpdatingPanels = true;

            if (currentContext.PanelType == 1 || currentContext.PanelType == 7)
            {
                _ = SendToKWClinker(currentContext);
            }

            await _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{status.taskCode}收到outbin回调，准备更新库位{currentContext.LocationCode}板料...");

            await FillDataToHikTaskRequest(status, materialData, currentContext);

            await ExtractPanelList(currentContext, status);

            await UpdateTransferLocationPanels(currentContext, status);
        }
        finally
        {
            currentContext.IsUpdatingPanels = false;
        }
    }

    /// <summary>
    /// 将海康的信息填充到currentContext的HikTaskRequest
    /// </summary>
    /// <param name="status"></param>
    /// <param name="materialData"></param>
    /// <param name="currentContext"></param>
    /// <returns></returns>
    private async Task FillDataToHikTaskRequest(HikAgvCallBack status, MaterialData materialData, CurrentContext currentContext)
    {
        string itemCode = materialData.materialLot.Contains("@") ? materialData.materialLot.Split('@')[0] : materialData.materialLot;
        DeviceServiceInvokeRequest deviceServiceInvokeRequest = currentContext.HikTaskRequest;
        deviceServiceInvokeRequest.Params["taskCode"] = status.taskCode;
        deviceServiceInvokeRequest.Params["Pcs"] = "0";
        deviceServiceInvokeRequest.Params["podLotNum"] = materialData.podLotNum.ToInt();
        deviceServiceInvokeRequest.Params["itemCode"] = itemCode;
        deviceServiceInvokeRequest.Params["IsSensorTest"] = DeviceDescriptor.Extra["IsSensorTest"].ToString();
        deviceServiceInvokeRequest.Params["layerLimit"] = DeviceDescriptor.LayerLimit.ToString();
        deviceServiceInvokeRequest.Params["podCode"] = status.podCode;
        deviceServiceInvokeRequest.Params["TargetPos"] = status.wbCode;
        deviceServiceInvokeRequest.Params["materialLot"] = materialData.materialLot;
        deviceServiceInvokeRequest.Params["lotQty"] = materialData.podLotNum.ToString();
        //AGV CODE
        deviceServiceInvokeRequest.Params["AGVCode"] = status.robotCode;
        deviceServiceInvokeRequest.Params["DeviceCode"] = currentContext.LocationCode;

        //查询PCS接口
        if (currentContext.operationType == 4)
        {
            int pcs = await GetPanelPcs(itemCode, "PanelCount");
            currentContext.Pcs = pcs.ToInt(1);
            currentContext.HikTaskRequest.Params["Pcs"] = pcs;
        }
        _logger.LogInformation($"收到库位{currentContext.LocationCode}的任务," +
            $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}的回调，任务号={status.taskCode}, FillDataToHikTaskRequest成功");
    }

    private async Task<int> GetPanelPcs(string itemCode, string v)
    {
        int res = 1;
        if (DeviceDescriptor.Extra["IsPcs"].ToBool())  ///device.DeviceDescriptor.Extra["IsPcs"].ToBool() 取是否查询片数配置
        {
            string pcs = await QueryGetWorkOrderInfo(itemCode, "PanelCount");
            if (pcs == "0")
            {
                pcs = "1";
                _logger.LogDebug($"片数配置为true， {itemCode} 获取的片数为 {pcs},默认为1");
                _logger.LogCritical($"片数配置为true， {itemCode} 获取的片数为 {pcs}，默认为1");
            }
            res = pcs.ToInt(1);
        }
        return res;
    }

    /// <summary>
    /// 提取板料信息
    /// </summary>
    /// <param name="currentContext"></param>
    /// <param name="arrivedinfo"></param>
    /// <returns></returns>
    private async Task ExtractPanelList(CurrentContext currentContext, HikAgvCallBack arrivedinfo)
    {
        try
        {
            string itemCode = currentContext.HikTaskRequest.Params["itemCode"].ToStr("");
            string pcs = currentContext.HikTaskRequest.Params["Pcs"].ToStr("");
            string podLotNum = currentContext.HikTaskRequest.Params["podLotNum"].ToStr("");
            string materialLot = currentContext.HikTaskRequest.Params["materialLot"].ToStr("");
            string podCode = currentContext.HikTaskRequest.Params["podCode"].ToStr("");
            string taskCode = currentContext.HikTaskRequest.Params["taskCode"].ToStr("");

            int position = currentContext.ShelfIndex;
            //点到区域:1.下熟料 2.下空仓 3.下生料(呼叫AGV上料)  区域到点:4.上生料 5.上空仓 6.上熟料 点到点 7.下首件:
            if (currentContext.operationType == 4)
            {
                #region 上生料 ,最后一层片数不固定

                List<Panel> lst = await GetPanelList(currentContext.HikTaskRequest, position);

                #endregion 上生料 ,最后一层片数不固定

                currentContext.RawPanelCount = string.IsNullOrEmpty(podLotNum) ? 0 : podLotNum.ToInt();
                currentContext.SiloCode = podCode;
                currentContext.ExternalLotNo = itemCode;

                currentContext.HikTaskRequest.Params["SiloCode"] = currentContext.SiloCode;
                currentContext.HikTaskRequest.Params["RawCount"] = Math.Ceiling(podLotNum.ToDouble() / pcs.ToInt()).ToInt();
                currentContext.HikTaskRequest.Params["ForkCode"] = position;
                currentContext.HikTaskRequest.Params["ExternalLotNo"] = materialLot.ToString();
                currentContext.HikTaskRequest.Params["ScheduledStatus"] = ScheduledTaskStatus.Completed;

                currentContext.HikTaskRequest.PayloadPanels.Clear();
                currentContext.HikTaskRequest.PayloadPanels.AddRange(lst);

                _logger.LogInformation($"中转位{currentContext.LocationCode} 任务{taskCode} 完成上生料任务更新本地数据时: 更新panelList完成");
            }
            else if (currentContext.operationType == 5) //上空仓
            {
                List<Panel> lst = new List<Panel>();
                var panelList = InteractingDevice.Locations[position.ToStr()].Panels;
                for (int i = 0; i < panelList.Count(); i++)
                {
                    Panel panel = new Panel();
                    panel.SiloCode = podCode;
                    panel.LotId = "";
                    panel.ItemCode = "";
                    panel.Layer = i;
                    panel.PanelCode = "";
                    panel.ProductStatus = ProductStatus.EmptySiloBox;
                    panel.Position = position;
                    panel.Pcs = 0;
                    lst.Add(panel);
                }

                currentContext.HikTaskRequest.PayloadPanels.Clear();
                currentContext.HikTaskRequest.PayloadPanels.AddRange(lst);

                _logger.LogDebug($"中转位{currentContext.LocationCode} 任务{taskCode} 完成上空仓任务更新本地数据时: 更新panelList完成");
            }
            else if (currentContext.operationType == 3) { }
            else if (currentContext.operationType == 6) { }
            else
            {
                //只处理1，2，7
                var lst = new PanelList();
                bool canReceiveHikMaterial = true;//能否接收海康回调给的物料

                if (currentContext.Direction == "out")
                {
                    //如果料仓已经人工维护后，再收到海康的回调，这个时候不能覆盖人工设置的板料
                    if (PayloadPanels.Where(t => t.SiloCode != arrivedinfo.podCode && t.Position == position).ToList().Count() > 0)
                    {
                        _logger.LogError($"中转位 任务{taskCode} 完成下熟料任务更新本地数据时: 本地中转位数据已经更新");
                        canReceiveHikMaterial = false;
                    }
                }

                if (canReceiveHikMaterial)
                {
                    var panelList = InteractingDevice.Locations[position.ToStr()].Panels;
                    for (int i = 0; i < panelList.Count(); i++)
                    {
                        Panel panel = new Panel();
                        panel.SiloCode = "";
                        panel.LotId = "";
                        panel.ItemCode = "";
                        panel.Layer = i;
                        panel.PanelCode = "";
                        panel.ProductStatus = ProductStatus.EmptyPayload;
                        panel.Pcs = 0;
                        panel.Position = position;
                        lst.Add(panel);
                    }

                    currentContext.HikTaskRequest.PayloadPanels.Clear();
                    currentContext.HikTaskRequest.PayloadPanels.AddRange(lst);
                }

                _logger.LogInformation($"中转位{currentContext.LocationCode} 任务{taskCode} 完成下空仓或者下熟料任务更新本地数据时: 更新panelList完成");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新板料信息失败：" + ex.Message);
        }

        _logger.LogInformation($"收到库位{currentContext.LocationCode}任务," +
            $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}的回调, ExtractPanelList成功");
    }

    private async Task<List<Panel>> GetPanelList(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position)
    {
        string TargetPos = deviceServiceInvokeRequest.Params["TargetPos"].ToStr("");
        string itemCode = deviceServiceInvokeRequest.Params["itemCode"].ToStr("");
        string pcs = deviceServiceInvokeRequest.Params["Pcs"].ToStr("");
        int podLotNum = deviceServiceInvokeRequest.Params["podLotNum"].ToInt();
        string materialLot = deviceServiceInvokeRequest.Params["materialLot"].ToStr("");
        string podCode = deviceServiceInvokeRequest.Params["podCode"].ToStr("");
        string taskCode = deviceServiceInvokeRequest.Params["taskCode"].ToStr("");

        int floors = 1;
        List<Panel> lst = new List<Panel>();
        int res = podLotNum % pcs.ToInt();
        floors = Math.Ceiling(podLotNum.ToDouble() / pcs.ToInt()).ToInt();//叠数，料仓一层放一叠
        int floorQty = res == 0 ? floors : floors - 1;

        if (floorQty > 0)
        {
            var request = new GetNextPanelRequest()
            {
                BeginLayer = 0,
                Count = floorQty,
                ItemCode = itemCode,
                PanelWidth = 622,
                PinOffset = 0,
                ProductStatus = ProductStatus.Finished_PRE_BUFFER,
                SiloCode = podCode,
                Position = position,
                BatchCode = "",
                LotId = materialLot
            };

            _logger.LogDebug($"Position: {position},SiloCode: {podCode},LotId: {materialLot}调用GetNextPanelNumberV2接口 参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(request))}");
            var panelLst = await HttpRequestInvoker.PostAsJsonAsync<GetNextPanelRequest, List<Panel>>(DeviceDescriptor.Extra["GetNextPanelNumberV2"].ToStr(), request);
            _logger.LogDebug($"Position: {position},SiloCode: {podCode},LotId: {materialLot}调用GetNextPanelNumberV2接口 返回数据是:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(panelLst))}");

            if (panelLst == null)
            {
                return lst;
            }

            for (int i = 0; i < floorQty; i++)
            {
                Panel panel = new Panel();
                panel.SiloCode = podCode;
                panel.LotId = materialLot;
                panel.ItemCode = itemCode;
                panel.Layer = i;
                panel.PanelCode = panelLst[i].PanelCode;
                panel.ProductStatus = ProductStatus.Finished_PRE_BUFFER;
                panel.Position = position;
                panel.Pcs = pcs.ToInt();
                lst.Add(panel);
            }
        }

        if (res > 0)
        {
            var onePanelRequest = new GetNextPanelRequest()
            {
                BeginLayer = 0,
                Count = 1,
                ItemCode = itemCode,
                PanelWidth = 622,
                PinOffset = 0,
                ProductStatus = ProductStatus.Finished_PRE_BUFFER,
                SiloCode = podCode,
                Position = position,
                BatchCode = "",
                LotId = materialLot
            };

            _logger.LogDebug($"Position: {position},SiloCode: {podCode},LotId: {materialLot}调用GetNextPanelNumberV2接口 参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(onePanelRequest))}");
            var onePanelResult = await HttpRequestInvoker.PostAsJsonAsync<GetNextPanelRequest, List<Panel>>(DeviceDescriptor.Extra["GetNextPanelNumberV2"].ToStr(), onePanelRequest);
            _logger.LogDebug($"Position: {position},SiloCode: {podCode},LotId: {materialLot}调用GetNextPanelNumberV2接口 返回数据是:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(onePanelResult))}");

            if (onePanelResult == null)
            {
                return lst;
            }

            Panel panel = new Panel();
            panel.SiloCode = podCode;
            panel.LotId = materialLot;
            panel.ItemCode = itemCode;
            panel.Layer = floorQty;
            panel.PanelCode = onePanelResult[0].PanelCode;
            panel.ProductStatus = ProductStatus.Finished_PRE_BUFFER;
            panel.Position = position;
            panel.Pcs = res;
            lst.Add(panel);
        }

        //料仓中没有板料的层
        for (int i = floors; i < deviceServiceInvokeRequest.Params["layerLimit"].ToInt(); i++)
        {
            Panel panel = new Panel();
            panel.SiloCode = podCode;
            panel.LotId = "";
            panel.ItemCode = "";
            panel.Layer = i;
            panel.PanelCode = "";
            panel.ProductStatus = ProductStatus.EmptySiloBox;
            panel.Position = position;
            panel.Pcs = 0;
            lst.Add(panel);
        }

        return lst;
    }

    public async Task<string> QueryTaskStatus(string taskCode, string style, string values)
    {
        try
        {
            string result = "";
            //style = "Hik";
            delayHikTask = DeviceDescriptor.Extra["DelayHikTask"].ToInt();
            for (int i = 0; i < 3; i++)
            {
                QueryTaskStatusData res = await QueryTaskStatus(taskCode, style);
                await Task.Delay(delayHikTask);
                if (res != null)
                {
                    if (values == "AGVCode")
                    {
                        result = res.agvCode;
                    }
                    else
                    {
                        result = res.taskStatus;
                    }

                    return result;
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{taskCode}查询任务状态异常：{ex.Message}");
            _logger.LogDebug($"{taskCode}查询任务状态异常：{ex.Message}");
            return "";
        }
    }

    public async Task<QueryTaskStatusData> QueryTaskStatus(string taskcode, string style)
    {
        try
        {
            switch (style)
            {
                case "Hik":
                    return await QueryHIKTaskStatus(taskcode);
            }
            _logger.LogDebug($"{taskcode}查询任务状态时出现异常");
            _logger.LogError($"{taskcode}查询任务状态时出现异常");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"{taskcode}查询任务状态时出现异常：{ex.Message}");
            _logger.LogError($"{taskcode}查询任务状态时出现异常：{ex.Message}");
            return null;
        }
    }

    private async Task<QueryTaskStatusData?> QueryHIKTaskStatus(string taskcode)
    {
        var query = new QueryTaskStatus()
        {
            //reqCode = "QTS" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            reqCode = this.DeviceDescriptor.DeviceId.Right(10) + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            reqTime = "",
            clientCode = "",
            tokenCode = "",
            agvCode = "",
            taskCodes = new string[] { taskcode }
        };

        var queryDataUrl = DeviceDescriptor.Extra["QueryTaskStatus"].ToStr();
        _logger.LogDebug($"查询任务 {taskcode} 状态接口QueryTaskStatus 参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(query))}");

        var queryDataResult = await HttpRequestInvoker.PostAsJsonAsync<QueryTaskStatus, QueryTaskStatusResponse>(queryDataUrl, query);
        _logger.LogDebug($"查询任务 {taskcode} 状态接口 QueryTaskStatus 的返回参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(queryDataResult))}");

        if (queryDataResult != null
            && queryDataResult.code == "0"
            && queryDataResult.data != null
            && !string.IsNullOrEmpty(queryDataResult.data.ToString()))
        {
            var list = JsonSerializer.Deserialize<List<QueryTaskStatusData>>(queryDataResult.data.ToString()!) ?? new();
            queryDataResult.QueryResult.AddRange(list);

            _logger.LogDebug($"查询任务 {taskcode} 状态接口 QueryTaskStatus 的返回参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(queryDataResult.QueryResult[0]))}");

            if (queryDataResult.QueryResult.Any())
            {
                return queryDataResult.QueryResult[0];
            }
        }

        return null;
    }

    public async Task<string> QueryGetWorkOrderInfo(string itemCode, string type)
    {
        string res = "";
        string msg = "";
        string SpecGroupUrl = string.Format(DeviceDescriptor.Extra["GetGroupCode"].ToString(), itemCode);
        var SpecGroup = await HttpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(SpecGroupUrl);
        if (SpecGroup == null)
        {
            await Task.Delay(2500);
            return "";
        }
        _logger.LogDebug($"{itemCode},{type}查询接口{SpecGroupUrl}接收到返回数据参数{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(SpecGroup).ToString())}");

        if (!SpecGroup.ContainsKey(type) || SpecGroup[type] == null || string.IsNullOrEmpty(SpecGroup[type].ToString()))
        {
            msg = $"接口{SpecGroupUrl}查询结果中无 {type} 或 {type} 为空";
            _logger.LogError(msg);
            _logger.LogDebug(msg);
            if (type == "SpecGroup")
            {
                res = "";
            }
            else //PanelCount
            {
                res = "1";
            }
        }
        else
        {
            res = SpecGroup[type].ToString();
        }
        return res;
    }

    /// <summary>
    /// 更新设备上的板料信息并上报中控
    /// </summary>
    /// <param name="currentContext"></param>
    /// <param name="data"></param>
    /// <param name="arrivedinfo"></param>
    /// <returns></returns>
    private async Task UpdateTransferLocationPanels(CurrentContext currentContext, HikAgvCallBack arrivedinfo)
    {
        string msg = "";
        var deviceServiceInvokeRequest = currentContext.HikTaskRequest;

        await _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"{arrivedinfo.taskCode}开始更新料仓信息");
        _logger.LogInformation($"小车任务 {arrivedinfo.taskCode} 开始更新料仓信息");
        var updateData = await this.InteractingDevice.SetSiloHandler(deviceServiceInvokeRequest);

        _logger.LogInformation($"收到库位{currentContext.LocationCode}的任务," +
            $"InternalLotNo:{currentContext.InternalLotNo},ExternalLotNo:{currentContext.ExternalLotNo}的回调, UpdateTransferLocationPanels.SetSiloHandler成功");

        if (updateData.Code == "SUCCESS")
        {
            await _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(),
                $"{arrivedinfo.taskCode}更新料仓信息成功，料仓号：{currentContext.SiloCode},库位号：{currentContext.LocationCode}收到板料：{deviceServiceInvokeRequest.PayloadPanels.PanelSnapshot}");

            _logger.LogInformation($"{arrivedinfo.taskCode}开始更新料仓任务完成的状态");
            var reportRes = await _centralReporter.ReportTaskStatus(ScheduledTaskStatus.Completed, currentContext);

            if (reportRes.Code == 0)
            {
                msg = $"{arrivedinfo.taskCode}给中控上传AGV任务完成的状态成功";
                _logger.LogInformation(msg);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
            }
            else
            {
                msg = $"{arrivedinfo.taskCode}给中控上传AGV任务完成的状态失败";
                _logger.LogInformation(msg);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
            }
        }
        else
        {
            msg = $"{arrivedinfo.taskCode}更新料仓{currentContext.SiloCode}的板料信息给库位{currentContext.LocationCode}失败,请人工核对";
            await _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
        }

        currentContext.Reset();
    }

    private async Task<BaseResponse> SendTaskToHik(CurrentContext currentContext, object[] tempArry)
    {
        string taskCode = currentContext.HikTaskCode;
        string taskCod = "";
        var data = new MaterialData()
        {
            realTaskCode = "",
            carryPro = "",
            materialGroupCode = "",
            materialLot = currentContext.HikTaskRequest.Params["materialLot"].ToString(),
            materialCode = "",
            carrierTyp = "P4",
            sysLotNum = "",
            podLotNum = currentContext.HikTaskRequest.Params["lotQty"].ToString(),
            lotStatus = "",
            includeScrap = "",
            podStatus = currentContext.HikTaskRequest.Params["podStatus"].ToString()
        };

        var ReqData = new SentTaskToHikAgv()
        {
            reqCode = taskCode,
            reqTime = "",
            clientCode = "",
            tokenCode = "",
            taskTyp = currentContext.HikTaskRequest.Params["taskTypes"].ToString(),
            ctnrTyp = "",
            ctnrCode = "",
            ctnrNum = "",
            taskMode = "",
            wbCode = "",
            userCallCodePath = tempArry,
            podCode = currentContext.HikTaskRequest.Params["SiloCode"].ToString(),
            podDir = "",
            podTyp = "P4",
            materialLot = currentContext.HikTaskRequest.Params["materialLot"].ToString(),
            materialType = "",
            priority = "",
            taskCode = taskCode,
            agvCode = "",
            groupId = "",
            agvTyp = "",
            positionSelStrategy = "",
            data = JsonSerializer.Serialize(data)
        };

        var response = new BaseResponse();
        var sendTaskToAgv = DeviceDescriptor.Extra["sendTaskToHikAgv"].ToString();//给AGV下任务
        _logger.LogDebug($"中转位  派任务接口 sendTaskToHikAgv 参数:{taskCod}{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(ReqData))}");
        var result = await HttpRequestInvoker.PostAsJsonAsync<SentTaskToHikAgv, ResMsg>(sendTaskToAgv, ReqData);
        _logger.LogDebug($"中转位  派任务接口 sendTaskToHikAgv 的返回结果:{taskCod}{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(result))}");

        if (result == null)
        {
            response.Code = 1;
            response.Message = "发送任务给海康失败";
            _logger.LogError(response.Message);
        }
        else if (result.code == "0")
        {
            _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), $"海康执行的任务号是：{taskCode}");
            response.Code = 0;
            response.Message = result.message;
        }
        else
        {
            response.Code = 1;
            response.Message = result.message;
        }

        return response;
    }

    /// <summary>
    /// 给景旺发送熟料下料信息
    /// </summary>
    /// <param name="currentContext"></param>
    /// <returns></returns>
    private async Task<bool> SendToKWClinker(CurrentContext currentContext)
    {
        if (currentContext.operationType == 1 && _isSendToKW)
        {
            //熟料，调用景旺接口 MaterialDistributionNotify
            var url = DeviceDescriptor.Extra["sendToKWUrl"].ToString();
            HttpHelper httpHelp = new HttpHelper();
            string method = "POST";
            MaterialDistributionNotify js = new MaterialDistributionNotify();
            js.ContainerName = currentContext.InternalLotNo;
            js.SonContainerName = currentContext.ExternalLotNo;
            js.EquipmentCode = currentContext.LocationCode;
            js.SpecGroup = currentContext.SpecGroup;
            js.Qty = currentContext.SumPcs;
            js.TaskType = currentContext.operationType.ToString();

            string sBody = JsonSerializer.Serialize(js);
            _logger.LogInformation($"向接口{url}传入参数{JsonSerializer.Serialize(sBody)}");

            string msg = string.Empty;
            int sendCount = 3;

            while (sendCount > 0)
            {
                try
                {
                    var sendToKW = httpHelp.HttpMethod(method, url, sBody);
                    _logger.LogInformation($"接口{url}接收到返回数据参数{JsonSerializer.Serialize(sendToKW)}");

                    if (sendToKW == null)
                    {
                        msg = $"{currentContext.HikTaskCode}下熟料 接口MaterialDistributionNotify返回的数据为空";
                        await _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                        await Task.Delay(1500);
                        sendCount--;
                        continue;
                    }

                    msg = $"{currentContext.HikTaskCode}下熟料 接口MaterialDistributionNotify返回的数据为{JsonSerializer.Serialize(sendToKW)}";
                    await _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    await Task.Delay(1500);
                    sendCount--;
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    /// <summary>
    ///中转位上生料，空料仓上传数据
    /// </summary>
    /// <param name="currentContext"></param>
    /// <param name="materialData"></param>
    /// <param name="siloCode"></param>
    /// <param name="taskCode"></param>
    /// <param name="agvCode"></param>
    /// <returns></returns>
    private async Task<bool> UploadData(CurrentContext currentContext, MaterialData materialData, string siloCode, string taskCode, string agvCode)
    {
        BaseResponse response = new BaseResponse();
        try
        {
            int panelFlood = 0;
            ServiceRequest ReportData = new ServiceRequest();
            ReportData.DeviceId = currentContext.LocationCode;
            ReportData.TraceId = currentContext.TransferJobCode;
            ReportData.ProductId = "";
            ReportData.Params["AGVCode"] = agvCode;
            ReportData.Params["TaskCode"] = taskCode;
            ReportData.Params["SiloCode"] = siloCode;
            string material = string.IsNullOrEmpty(materialData.materialLot) ? "" : materialData.materialLot;
            ReportData.Params["MaterialLot"] = material;
            ReportData.Params["PodLotNum"] = materialData.podLotNum;//原始数量
            if (currentContext.operationType == 4)
            {
                panelFlood = await GetPanelFlood(material.Contains('@') ? material.Split('@')[0] : material, "PanelCount", materialData.podLotNum);
            }

            ReportData.Params["RawCount"] = panelFlood;//转换成叠数后
            _ = await _centralReporter.UpLoadTaskData(ReportData);
        }
        catch (Exception ex)
        {
            response.Code = 1;
            response.Message = ex.Message;
        }

        return true;
    }

    /// <summary>
    /// 料框中一共多少叠板子
    /// </summary>
    /// <param name="itemCode"></param>
    /// <param name="style"></param>
    /// <param name="podLotNum"></param>
    /// <returns></returns>
    private async Task<int> GetPanelFlood(string itemCode, string style, string podLotNum)
    {
        int floors = 0;
        int pcs = await GetPanelPcs(itemCode, "PanelCount");
        if (pcs > 0)
        {
            int res = podLotNum.ToInt() % pcs.ToInt();
            floors = Math.Ceiling(podLotNum.ToDouble() / pcs.ToInt()).ToInt();//叠数，料仓一层放一叠
        }
        return floors;
    }
}
