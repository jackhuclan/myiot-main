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
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.StdAgv.Executor.ForkOutside;

/// <summary>
/// 熟料从内部中转位运转外部熟料区
/// </summary>
internal class UndrilledFromForkToOutsideExecutor : ITransferSiloExecutor
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly ILocationManager _locationManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly ILogger<UndrilledFromForkToOutsideExecutor> _logger;

    public UndrilledFromForkToOutsideExecutor(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _workOrderTaskAdapter = serviceProvider.GetRequiredService<IWorkOrderTaskAdapter>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromForkToOutsideExecutor>();
    }

    public async Task<bool> Execute(TransferJob job)
    {
        job.HkResponse = String.Empty;
        _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor,task:{job.Id}, begin to ExecuteAsync==========================\r\n");

        // 起始(中转位)位置
        var fromLocation = new Location();
        var podEntity = new List<PodInfoEntity>();
        if (!string.IsNullOrEmpty(job.StartLocationCode)
            && _locationManager.TryGetLocation(job.StartLocationCode, out var startLocation)
            && startLocation != null)
        {
            fromLocation = startLocation;
            _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor,task:{job.Id},Start location: {startLocation.PositionCode}.\r\n");
            podEntity = fromLocation.Panels.AllPanelsNotHaveFirst.Select(x => new PodInfoEntity()
            {
                boxLayerNo = x.Layer + 1,
                foldQrCode = x.PanelCode,
                lotNo = x.ItemCode,
                pnlStatus = ProductStatusConstants.Finished_PIN.Contains(x.ProductStatus) ? "1" : "2"
            }).ToList();
        }
        else
        {
            _logger.LogError($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor,task:{job.Id},TryGetLocation error.\r\n");
            return false;
        }

        var url = await _stdAgvConfig.GetGenAgvSchedulingTaskUrl();
        var bindLineSideStockUrl = await _stdAgvConfig.GetBindLineSideStockUrl();
        var isSuccess = false;

        var _BindPodAndMatUrl = await _stdAgvConfig.GetBindPodAndMatUrl();
        var _BoxBindFoldUrl = await _stdAgvConfig.GetBoxBindFoldUrl();

        // 料架与物料解绑（接口2.10）
        var _Request210 = new BindSiloEntity()
        {
            clientCode = "VEGA001",
            indBind = "0",
            podCode = fromLocation.Panels?.UndrilledPanels?.FirstOrDefault()?.SiloCode,
            reqCode = $"Vega_Mat1_{DateTime.Now.Ticks.ToString()}",
            reqTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
        };

        _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor:task:{job.Id}: 货架与物料解绑 - bindPodAndMat: 请求参数: \r\n {JsonSerializer.Serialize(_Request210)}");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = job.Id,
            Message = $"货架与物料解绑 - bindPodAndMat: task:{job.Id}: url: {_BindPodAndMatUrl},请求参数: \r\n {JsonSerializer.Serialize(_Request210)} "
        });

        var res210 = await _httpRequestInvoker.PostAsJsonAsync<BindSiloEntity?, STDResponse>(_BindPodAndMatUrl, _Request210, new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });

        _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor : task:{job.Id}: 货架与物料解绑 - bindPodAndMat: 返回内容: \r\n {JsonSerializer.Serialize(res210, new JsonSerializerOptions()
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
                podCode = fromLocation.Panels?.UndrilledPanels?.FirstOrDefault()?.SiloCode,
                reqCode = $"Vega_Bold1_{DateTime.Now.Ticks.ToString()}",
                reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                podList = podEntity
            };

            _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor: task:{job.Id}: 托盘（料箱号）信息绑定铝片信息 - BoxBindFold: 请求参数: \r\n {JsonSerializer.Serialize(_Request215)}");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"托盘（料箱号）信息绑定铝片信息 - BoxBindFold: url: {_BoxBindFoldUrl}, 请求参数: \r\n {JsonSerializer.Serialize(_Request215)} "
            });

            res215 = await _httpRequestInvoker.PostAsJsonAsync<BindSiloBoxEntity?, STDResponse>(_BoxBindFoldUrl, _Request215, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

            _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor: task:{job.Id}: 托盘（料箱号）信息绑定铝片信息 - BoxBindFold: 返回内容: \r\n {JsonSerializer.Serialize(res215)}");

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
                _logger.LogError($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor,task:{job.Id}: 托盘（料箱号）信息绑定铝片,异常信息: {res215?.message}.\r\n");
                await _transferPlanManager?.TryUpdateTransferJob(job);
                return isSuccess;
            }
        }
        else
        {
            job.HkResponse = res210?.message;
            _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor,task:{job.Id}: 料架与物料解绑,异常信息: {res210?.message}.\r\n");
            await _transferPlanManager?.TryUpdateTransferJob(job);
            return isSuccess;
        }

        //目标区域
        var clinkerStoreArray = (await _stdAgvConfig.GetLineStore()).Split(',');
        _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor: task:{job.Id}: 目标区域编码：LineStore: {JsonSerializer.Serialize(clinkerStoreArray)}");

        foreach (var item in clinkerStoreArray)
        {
            var path = new String[] { fromLocation.PositionCode, $"{item}" };
            var taskTyp = "F15";
            var siloCode = job.SiloCode ?? "";
            var materialLot = job.InternalLotNo;
            var request = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Id, 0, null, materialLot, taskTyp, siloCode);

            _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor: task:{job.Id}: 下生料任务: - genAgvSchedulingTask: 请求参数: \r\n {JsonSerializer.Serialize(request)}");

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"下生料任务: - genAgvSchedulingTask: 请求参数: \r\n {JsonSerializer.Serialize(request)} "
            });


            var ret = await _httpRequestInvoker.PostAsJsonAsync<STDGenAgvSchedulingTaskRequestEntity, STDResponse>(url, request, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

            _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor: task:{job.Id}: 下生料任务: - genAgvSchedulingTask: 返回内容: \r\n {JsonSerializer.Serialize(ret)}");
            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = job.Id,
                Message = $"下生料任务: - genAgvSchedulingTask: 返回内容: \r\n {JsonSerializer.Serialize(ret, new JsonSerializerOptions()
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

                _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor,task:{job.Id},send request data to Std,Time:{DateTime.Now}.\r\n");

                job.EndLocationCode = item;
                job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
                job.HikResponseKey = ret.reqCode;
                job.HkResponse = JsonSerializer.Serialize(ret, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });
                await _transferPlanManager.TryUpdateTransferJob(job);

                _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor,task:{job.Id},StartSchedule{startSchedule.Id}----SetScheduleAsWorking:TraceId{startSchedule.Code}.\r\n");

                await _scheduleTaskManager.SetScheduleAsWorking(new StartScheduleTaskRequest
                {
                    DeviceId = job.StartDeviceId,
                    TraceId = startSchedule.Code,
                    Params = new Dictionary<string, object?>
                    {
                        {"CarCurrentPos",job.StartLocationCode},
                        {"BehaivorName",$"调度路线：从内部中转位:{job.StartLocationCode}【{startSchedule.Id}】到外部生料区域:{item}.\r\n"}
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

        _logger.LogInformation($"StdAgvScheduler_UndrilledFromForkToOutsideExecutor,task:{job.Id},end to ExecuteAsync==================================\r\n");

        return isSuccess;
    }
}
