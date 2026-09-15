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
using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Hik;

/// <summary>
/// 中控任务处理器
/// </summary>
public class CentralTransferJobProcessor : DeviceShare<SiloShelf>
{
    private readonly ILogger<CentralTransferJobProcessor> _logger;
    private readonly DeviceDescriptor _deviceDescriptor;
    private readonly string[] LineStore;//生料仓区
    private readonly string[] EmptyStore; //空料仓区
    private readonly string[] ClinkerStore;//熟料仓区
    private readonly string[] FirstStore;
    private readonly HikHandler _hikHandler;
    private readonly CentralReporter _centralReporter;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

    public CentralTransferJobProcessor(IServiceProvider serviceProvider,
        IObjectFactory objectFactory,
        ILoggerFactory loggerFactory,
        SiloShelf device)
        : base(serviceProvider, device)
    {
        _logger = loggerFactory.CreateLogger<CentralTransferJobProcessor>();
        _deviceDescriptor = DeviceDescriptor;
        _hikHandler = objectFactory.GetOrCreate<HikHandler>(InteractingDevice);
        _centralReporter = objectFactory.GetOrCreate<CentralReporter>(InteractingDevice);

        LineStore = _deviceDescriptor.Extra["LineStore"].ToString().Split(',');
        ////空料仓区
        EmptyStore = _deviceDescriptor.Extra["EmptyStore"].ToString().Split(',');
        ////熟料仓区
        ClinkerStore = _deviceDescriptor.Extra["ClinkerStore"].ToString().Split(',');
        FirstStore = _deviceDescriptor.Extra["FirstStore"].ToString().Split(',');
    }

    /// <summary>
    /// 向中控请求任务
    /// </summary>
    /// <returns></returns>
    public async Task ProcessCentralJob()
    {
        var processedJobIds = InteractingDevice.Locations.Values.Select(x => x.CurrentContext.TransferId).ToList();
        string urltemp = _deviceDescriptor.Extra["GetCentralControllTask"].ToString();
        string ForkGroup = _deviceDescriptor.Extra["ForkGroup"].ToString();
        string url = string.Format(urltemp, ForkGroup);
        //向中控领任务，上下料，上下料仓  FockGroup
        //logger.LogDebug($"中转位 领取任务接口 {url} 参数:{System.Text.RegularExpressions.Regex.Unescape(ForkGroup)}");
        var res = await HttpRequestInvoker.GetFromJsonAsync<List<TransferJob>>(url);
        _logger.LogInformation($"中转位领取任务接口返回参数:{JsonSerializer.Serialize(res)}");

        if (res == null || res.Count() < 1) return;

        var list = res.Where(t => t.ScheduledTaskStatus == ScheduledTaskStatus.Created && !processedJobIds.Contains(t.Id))
                .ToList();

        var currentContexts = InteractingDevice.Locations.Values.Select(x => x.CurrentContext).ToList();

        for (int i = 0; i < list.Count(); i++)
        {
            var currentContext = currentContexts.FirstOrDefault(x => x.LocationCode == list[i].ForkCode);
            var locationCodes = InteractingDevice.Locations.Values.Select(x => x.CurrentContext.LocationCode).ToList();
            if (string.IsNullOrWhiteSpace(list[i].ForkCode)
                || !locationCodes.Contains(list[i].ForkCode)
              || currentContext == null)
            {
                _logger.LogInformation($"不存在list[{i}].ForkCode={list[i].ForkCode}的库位");
                continue;
            }

            try
            {
                _autoResetEvent.WaitOne();
                bool hasSentHIK = !string.IsNullOrWhiteSpace(currentContext.HikTaskCode);
                if (hasSentHIK)
                {
                    continue;
                }

                int temp = GetOperationType(list[i].InteractionSequence, list[i].TransportationKind);
                string direction = (temp == 1 || temp == 2 || temp == 7) ? "out" : "in";

                // 1.下熟料 2.下空仓 7.下首件
                if ((temp == 1 || temp == 2 || temp == 7) && string.IsNullOrEmpty(list[i].SiloCode))
                {
                    _logger.LogError($"中转位 {currentContext.LocationCode} 呼叫的任务 {temp} 没有料仓号，不能执行");
                    continue;
                }

                //InternalLotNo 内部lot ，ExternalLotNo 外部lot
                currentContext.InternalLotNo = list[i].InternalLotNo;
                currentContext.ExternalLotNo = list[i].ExternalLotNo;
                currentContext.operationType = temp;
                currentContext.SiloCode = list[i].SiloCode;
                currentContext.RawPanelCount = GetQty(temp, list[i]);
                currentContext.TransferJobCode = list[i].Code;
                currentContext.TransferId = list[i].Id;
                currentContext.Region = list[i].PartitionCode;
                currentContext.Direction = direction;

                _logger.LogInformation($"中转位{currentContext.LocationCode}下发任务给海康：TransferId={currentContext.TransferId}," +
                    $"InternalLotNo={currentContext.InternalLotNo}");
                await DeliverTaskToHikAGV(currentContext);
            }
            finally
            {
                _autoResetEvent.Set();
            }

            await Task.Delay(5 * 1000);
        }
    }

    /// <summary>
    /// 生料熟料数量
    /// </summary>
    /// <param name="temp"></param>
    /// <param name="transportationTask"></param>
    /// <returns></returns>
    private int GetQty(int temp, TransferJob transportationTask)
    {   //点到区域： 1.下熟料 2.下空仓  3.下生料(呼叫AGV上料)  区域到点： 4.上生料 5.上空仓 6.上熟料,7.下首件
        int res = 0;
        if (temp == 1 || temp == 7)
        {
            res = transportationTask.ClinkerCount.ToInt();
        }
        else if (temp == 4)
        {
            res = transportationTask.RawCount.ToInt();
        }

        return res;
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    /// <param name="interactionSequence"></param>
    /// <param name="transportationKind"></param>
    /// <returns></returns>
    private int GetOperationType(InteractionSequence? interactionSequence, TransportationKind? transportationKind)
    {
        // InteractionSequence :LoadOnly,UnloadOnly, TransportationKind: 空仓EmptySilo, 生料Raw ,熟料Clinker ,首件First
        int i = 0;
        if (InteractionSequence.LoadOnly == interactionSequence)
        {
            switch (transportationKind)
            {
                case TransportationKind.EmptySilo:
                    i = 5; break;
                case TransportationKind.Raw://生料
                    i = 4; break;
                default: return i;
            }
        }
        else if (InteractionSequence.UnloadOnly == interactionSequence)
        {
            switch (transportationKind)
            {
                case TransportationKind.EmptySilo:
                    i = 2; break;
                case TransportationKind.Clinker://熟料
                    i = 1; break;
                case TransportationKind.First:
                    i = 7; break;
                default: return i;
            }
        }
        return i;
    }

    /// <summary>
    /// 派发任务给海康agv
    /// </summary>
    /// <param name="currentContext"></param>
    /// <returns></returns>
    private async Task<string> DeliverTaskToHikAGV(CurrentContext currentContext)
    {
        //点到区域： 1.下熟料 2.下空仓  3.下生料(呼叫AGV上料)  区域到点： 4.上生料 5.上空仓 6.上熟料,7.下首件
        string msg = "";
        if (string.IsNullOrEmpty(currentContext.LocationCode))
        {
            //currentContext.LocationCode = "PanelFork" + (currentContext.RackIndex + 1).ToString("D3");
            currentContext.LocationCode = InteractingDevice.DeviceId + (currentContext.RackIndex + 1).ToString("D3");
        }
        string[] TargetPosArry = _deviceDescriptor.Extra["TransShelfInnerPositionList"].ToString().Split(',');
        string TargetPos = TargetPosArry[currentContext.RackIndex];

        if (currentContext.operationType == 5 || currentContext.operationType == 2)
        {
            currentContext.ItemCode = "";
        }

        switch (currentContext.operationType)
        {
            case 1: //1.下熟料
                var msg1 = $"库位{currentContext.LocationCode}的下熟料任务TransferId={currentContext.TransferId}," +
                    $"InternalLotNo={currentContext.InternalLotNo},ExternalLotNo={currentContext.ExternalLotNo}正在派发...";
                _logger.LogInformation(msg1);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg1);
                await _hikHandler.UnLoadClinker(currentContext, TargetPos, ClinkerStore);
                break;

            case 2: //2.下空仓
                var msg2 = $"库位{currentContext.LocationCode}的下空仓任务TransferId={currentContext.TransferId}," +
                    $"InternalLotNo={currentContext.InternalLotNo},ExternalLotNo={currentContext.ExternalLotNo}正在派发...";
                _logger.LogInformation(msg2);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg2);
                await _hikHandler.UnLoadEmptySilo(currentContext, TargetPos, EmptyStore);
                break;

            case 4: //4.上生料
                var msg4 = $"库位{currentContext.LocationCode}的上生料任务TransferId={currentContext.TransferId}," +
                    $"InternalLotNo={currentContext.InternalLotNo},ExternalLotNo={currentContext.ExternalLotNo}正在派发...";
                _logger.LogInformation(msg4);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg4);
                await _hikHandler.LoadRaw(currentContext, TargetPos, LineStore);
                break;

            case 5: //5.上空仓
                var msg5 = $"库位{currentContext.LocationCode}的上空仓任务TransferId={currentContext.TransferId}," +
                    $"InternalLotNo={currentContext.InternalLotNo},ExternalLotNo={currentContext.ExternalLotNo}正在派发...";
                _logger.LogInformation(msg5);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg5);
                await _hikHandler.LoadEmptySilo(currentContext, TargetPos, EmptyStore);
                break;

            case 7: //5.首件
                var msg7 = $"库位{currentContext.LocationCode}的下首件任务TransferId={currentContext.TransferId}," +
                    $"InternalLotNo={currentContext.InternalLotNo},ExternalLotNo={currentContext.ExternalLotNo}正在派发...";
                _logger.LogInformation(msg7);
                _ = _centralReporter.ReportHikTaskLogToCentral(currentContext.TransferId.ToString(), msg7);
                await _hikHandler.UnLoadClinker(currentContext, TargetPos, FirstStore);
                break;

            default:
                msg = "未知的操作类型";
                _logger.LogError($"中转位 {msg}");
                break;
        }

        return msg;
    }

    public async Task<string> QueryTaskStatus(string taskCode)
    {
        try
        {
            var res = await _hikHandler.QueryTaskStatus(taskCode, "Hik", "TaskStatus");//TaskStatus
            if (res != null)
            {
                return res;
            }
            return $"{taskCode}查询任务状态失败";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ex.Message;
        }
    }
}
