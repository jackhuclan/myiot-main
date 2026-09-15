using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using VegaIot.External.Agv.Executor;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.StdAgv.Executor.ForkOutside;

/// <summary>
/// 熟料从内部中转位运转外部熟料区
/// </summary>
internal class DrilledFromForkToOutsideExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly ILogger<DrilledFromForkToOutsideExecutor> _logger;

    public DrilledFromForkToOutsideExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _workOrderTaskAdapter = serviceProvider.GetRequiredService<IWorkOrderTaskAdapter>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DrilledFromForkToOutsideExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        job.HkResponse = String.Empty;
        _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id}, begin to ExecuteAsync==========================\r\n");

        var podStatus = "0";//托盘状态

        // 检查熟料板数量，确保不为空
        var podLotNum = job.ClinkerCount ?? 0;
        //var specGroup = await _workOrderTaskAdapter.GetWorkOrderInfo(job.InternalLotNo!);
        //if (_stdAgvConfig.IsCustomPanelCount() && specGroup.ContainsKey("PanelCount"))
        //{
        //    podLotNum *= (int)specGroup["PanelCount"]!;
        //}

        _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},ClinkerCount:{job.ClinkerCount},InternalLotNo:{job.InternalLotNo},specGroup:,podLotNum:{podLotNum}.\r\n");

        if (podLotNum == 0)
        {
            job.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            await _transferPlanManager.TryUpdateTransferJob(job);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"Fail,料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}熟料数量为0，不能执行"
            });

            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}数量为0.\r\n");

            return false;
        }

        // 料号检查
        if (string.IsNullOrEmpty(job.ExternalLotNo)
            || !job.ExternalLotNo.Contains("@"))
        {
            job.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            await _transferPlanManager.TryUpdateTransferJob(job);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"Fail,料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}没有熟料料号"
            });

            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}没有料号.\r\n");

            return false;
        }

        // 料号检查
        if (job.ExternalLotNo.Split('@')[0] != job.InternalLotNo)
        {
            job.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            await _transferPlanManager.TryUpdateTransferJob(job);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"Fail,料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}对内料号与对外料号不一致"
            });

            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},料仓{job.SiloCode}的物料{job.InternalLotNo}下熟料时物料{job.InternalLotNo}对内料号与对外料号不一致.\r\n");

            return false;
        }

        // 起始(中转位)位置
        //     var fromLocation = new StdLocation();
        var fromLocation = new Location();
        var podEntity = new List<PodInfoEntity>();
        if (!string.IsNullOrEmpty(job.StartLocationCode)
            && _locationManager.TryGetLocation(job.StartLocationCode, out var startLocation)
            && startLocation != null)
        {
            // fromLocation.positionCode = startLocation.PositionCode;
            fromLocation = startLocation;
            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},Start location: {startLocation.TransAGVInnerPoint}.\r\n");
            podEntity = fromLocation.Panels.DrilledPanels.Select(x => new PodInfoEntity()
            {
                boxLayerNo = x.Layer + 1,
                foldQrCode = x.PanelCode,
                lotNo = x.ItemCode,
                pnlStatus = "2"
            }).ToList();
        }
        else
        {
            _logger.LogError($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},TryGetLocation error .\r\n");
            return false;
        }

        var url = await _stdAgvConfig.GetGenAgvSchedulingTaskUrl();

        var isSuccess = false;
        var bindLineSideStockUrl = await _stdAgvConfig.GetBindLineSideStockUrl();
        var _BindPodAndMatUrl = await _stdAgvConfig.GetBindPodAndMatUrl();
        var _BoxBindFoldUrl = await _stdAgvConfig.GetBoxBindFoldUrl();

        #region 绑定托盘库位
        //var _Request200 = new BindSiloStockEntity()
        //{
        //    clientCode = "VEGA001",
        //    indBind = "1",
        //    podCode = fromLocation.Panels?.DrilledPanels?.FirstOrDefault()?.SiloCode,
        //    reqCode = $"Vega_Stock3_{DateTime.Now.Ticks.ToString()}",
        //    reqTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
        //    mapDataCode = fromLocation.PositionCode!
        //};

        //_logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor:task:{job.Id}: 料架与料仓绑定 - BindLineSideStock: 请求参数: \r\n {JsonSerializer.Serialize(_Request200)}");

        //await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        //{
        //    TransferJobId = job.Id,
        //    Message = $"料架与料仓绑定 - BindLineSideStock: task:{job.Id}: url: {bindLineSideStockUrl},请求参数: \r\n {JsonSerializer.Serialize(_Request200)} "
        //});

        //var res200 = await _httpRequestInvoker.PostAsJsonAsync<BindSiloStockEntity?, STDResponse>(bindLineSideStockUrl, _Request200, new JsonSerializerOptions()
        //{
        //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        //});

        //_logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor : task:{job.Id}: 料架与料仓绑定 - BindLineSideStock: 返回内容: \r\n {JsonSerializer.Serialize(res200, new JsonSerializerOptions()
        //{
        //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        //})}");

        //await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        //{
        //    TransferJobId = job.Id,
        //    Message = $"料架与料仓绑定 - BindLineSideStock: 返回内容: \r\n {JsonSerializer.Serialize(res200, new JsonSerializerOptions()
        //    {
        //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        //    })} "
        //});

        //if (res200 == null || res200.code != 0)
        //{
        //    job.HkResponse = res200?.message;
        //    _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id}: 料架与料仓绑定,异常信息: {res200?.message}.\r\n");
        //    await _transferPlanManager?.TryUpdateTransferJob(job);
        //    return isSuccess;
        //}
        #endregion
        // 料架与物料解绑（接口2.10）
        var _Request210 = new BindSiloEntity()
        {
            clientCode = "VEGA001",
            indBind = "0",
            podCode = fromLocation.Panels?.DrilledPanels?.FirstOrDefault()?.SiloCode,
            reqCode = $"Vega_Mat3_{DateTime.Now.Ticks.ToString()}",
            reqTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
        };

        _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor:task:{job.Id}: 货架与物料解绑 - bindPodAndMat: 请求参数: \r\n {JsonSerializer.Serialize(_Request210)}");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = job.Id,
            Message = $"货架与物料解绑 - bindPodAndMat: task:{job.Id}: url: {_BindPodAndMatUrl},请求参数: \r\n {JsonSerializer.Serialize(_Request210)} "
        });

        var res210 = await _httpRequestInvoker.PostAsJsonAsync<BindSiloEntity?, STDResponse>(_BindPodAndMatUrl, _Request210, new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });

        _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor : task:{job.Id}: 货架与物料解绑 - bindPodAndMat: 返回内容: \r\n {JsonSerializer.Serialize(res210, new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        })}");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = job.Id,
            Message = $"货架与物料解绑 - bindPodAndMat: 返回内容: \r\n {JsonSerializer.Serialize(res210, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            })} "
        });

        STDResponse? res215 = null;

        if (res210 != null && res210.code == 0)
        {
            var _Request215 = new BindSiloBoxEntity()
            {
                clientCode = "VEGA001",
                indBind = "1",
                podCode = fromLocation.Panels.DrilledPanels.FirstOrDefault().SiloCode,
                reqCode = $"Vega_Bold3_{DateTime.Now.Ticks.ToString()}",
                reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                podList = podEntity
            };

            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor: task:{job.Id}: 托盘（料箱号）信息绑定铝片信息 - BoxBindFold: 请求参数: \r\n {JsonSerializer.Serialize(_Request215)}");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"托盘（料箱号）信息绑定铝片信息 - BoxBindFold: url: {_BoxBindFoldUrl}, 请求参数: \r\n {JsonSerializer.Serialize(_Request215)} "
            });

            res215 = await _httpRequestInvoker.PostAsJsonAsync<BindSiloBoxEntity?, STDResponse>(_BoxBindFoldUrl, _Request215, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor: task:{job.Id}: 托盘（料箱号）信息绑定铝片信息 - BoxBindFold: 返回内容: \r\n {JsonSerializer.Serialize(res215)}");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"托盘（料箱号）信息绑定铝片信息 - BoxBindFold: 返回内容: \r\n {JsonSerializer.Serialize(res215, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                })} "
            });

            if (res215 == null || res215.code != 0)
            {
                job.HkResponse = res215?.message;
                _logger.LogError($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id}: 托盘（料箱号）信息绑定铝片,异常信息: {res215?.message}.\r\n");
                await _transferPlanManager?.TryUpdateTransferJob(job);
                return isSuccess;
            }
        }
        else
        {
            job.HkResponse = res210?.message;
            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id}: 料架与物料解绑,异常信息: {res210?.message}.\r\n");
            await _transferPlanManager?.TryUpdateTransferJob(job);
            return isSuccess;
        }

        //熟料区域
        var clinkerStoreArray = (await _stdAgvConfig.GetClinkerStore()).Split(',');
        _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor: task:{job.Id}: 熟料区域编码：clinkerStoreArray: {JsonSerializer.Serialize(clinkerStoreArray)}");

        foreach (var item in clinkerStoreArray)
        {
            var path = new String[] { fromLocation.PositionCode, $"{item}" };
            var taskTyp = "F10";
            var siloCode = job.SiloCode ?? "";
            var materialLot = job.InternalLotNo;
            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, podLotNum, podStatus, materialLot, taskTyp, siloCode);

            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor: task:{job.Id}: 下熟料任务: - genAgvSchedulingTask: 请求参数: \r\n {JsonSerializer.Serialize(request)}");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"下熟料任务: - genAgvSchedulingTask: 请求参数: \r\n {JsonSerializer.Serialize(request)} "
            });


            var ret = await _httpRequestInvoker.PostAsJsonAsync<STDGenAgvSchedulingTaskRequestEntity, STDResponse>(url, request, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

            _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor: task:{job.Id}: 下熟料任务: - genAgvSchedulingTask: 返回内容: \r\n {JsonSerializer.Serialize(ret)}");
            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"下熟料任务: - genAgvSchedulingTask: 返回内容: \r\n {JsonSerializer.Serialize(ret, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                })} "
            });

            if (ret != null
                && ret.code == 0
                && job.StartScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(job.StartScheduleId.Value, out var startSchedule)
                && startSchedule != null
                && !string.IsNullOrEmpty(job.StartDeviceId)
                && !string.IsNullOrEmpty(startSchedule.Code))
            {

                isSuccess = true;

                _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},send request data to Std,Time:{DateTime.Now}.\r\n");

                job.EndLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.reqCode;
                job.HkResponse = JsonSerializer.Serialize(ret, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });
                await _transferPlanManager.TryUpdateTransferJob(job);

                _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},StartSchedule{startSchedule.Id}----SetScheduleAsWorking:TraceId{startSchedule.Code}.\r\n");

                await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
                {
                    DeviceId = job.StartDeviceId,
                    TraceId = startSchedule.Code,
                    Params = new Dictionary<string, object?>
                    {
                        {"CarCurrentPos",job.StartLocationCode},
                        {"BehaivorName",$"调度路线：从内部中转位:{job.StartLocationCode}【{startSchedule.Id}】到外部熟料区域:{item}.\r\n"}
                    }
                });

                break;
            }
            else
            {
                job.HkResponse = string.Join('丨', JsonSerializer.Serialize(ret, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                }));
                await _transferPlanManager.TryUpdateTransferJob(job);
            }
        }

        _logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor,task:{job.Id},end to ExecuteAsync==================================\r\n");

        return isSuccess;
    }
}
