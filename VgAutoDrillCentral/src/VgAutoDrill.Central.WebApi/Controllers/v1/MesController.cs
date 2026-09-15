using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.WebApi.Controllers.v1;

[Route("v1/central/mes")]
[ApiController]
public class MesController
{
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILogger<MesController> _logger;

    // 固定刀盘最小口径
    private readonly double _bigCutterMinValue = 0.55;

    private readonly IItemAdapter _itemAdapter;

    // 物料需求的钻孔数据
    private readonly Dictionary<string, List<CutterRequirement>> _itemDrillContents = new()
    {
        {
            "item01",
            new List<CutterRequirement>
            {
                new CutterRequirement
                {
                    PgmDiameter = 0.25,
                    NeedDrillCount = 60
                },
                new CutterRequirement
                {
                    PgmDiameter = 3.0,
                    NeedDrillCount = 20
                },
                new CutterRequirement
                {
                    PgmDiameter = 4.0,
                    NeedDrillCount = 5
                },
            }
        },
        {
            "item02",
            new List<CutterRequirement>
            {
                new CutterRequirement
                {
                    PgmDiameter = 0.25,
                    NeedDrillCount = 60
                },
                new CutterRequirement
                {
                    PgmDiameter = 3.0,
                    NeedDrillCount = 20
                },
                new CutterRequirement
                {
                    PgmDiameter = 4.0,
                    NeedDrillCount = 5
                },
            }
        }
    };

    // 刀具寿命定义
    private readonly List<CutterLifeDefine> _cutterLifeDefines = new()
    {
        new CutterLifeDefine
        {
            PgmDiameter=0.25,
            MoCount =0,
            Life = 15
        },
        new CutterLifeDefine
        {
            PgmDiameter=3.0,
            MoCount =0,
            Life = 10
        },
        new CutterLifeDefine
        {
            PgmDiameter=4,
            MoCount =0,
            Life = 20
        },
    };

    public MesController(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MesController>();
        _workOrderTaskAdapter = serviceProvider.GetRequiredService<IWorkOrderTaskAdapter>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _itemAdapter = serviceProvider.GetRequiredService<IItemAdapter>();
    }

    /// <summary>
    /// 查询调度记录的状态
    /// 如果是取消、完成、异常中止时，设备端按对应的业务规则处理；
    /// 如果是其他状态时，保持等待；
    /// 不存在此记录，设备端按取消处理
    /// </summary>
    /// <param name="traceId"></param>
    /// <returns></returns>
    [HttpGet("QuerySchedule")]
    public ScheduledTaskStatus? QuerySchedule(string traceId)
    {
        var schedule = _scheduleTaskManager.FindScheduleByTraceId(traceId);
        if (schedule != null)
        {
            return schedule.ScheduledTaskStatus;
        }
        else
        {
            return ScheduledTaskStatus.Canceled;
        }
    }

    /// <summary>
    /// 获取物料代码
    /// Original = 0,
    /// OrderByCreateTimeASC = 1,
    /// OrderByCreateTimeDesc = 2,
    /// OrderByCodeASC = 3,
    /// OrderByCodeDesc = 4
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetItemCode")]
    public async Task<object> GetItemCode(QueryItemCodeRequest request)
    {
        return await _itemAdapter.GetItemCode(request);
    }

    /// <summary>
    /// 根据物料编码获取板长、板宽
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    [HttpGet("GetItemInfo")]
    public async Task<object> GetItemInfo(string itemCode)
    {
        if (string.IsNullOrWhiteSpace(itemCode)) return string.Empty;
        var item = await _itemAdapter.GetItemDataAsync(itemCode);
        return new { item?.Code, item?.PanelWidth, item?.PanelLength };
    }

    /// <summary>
    /// 报告生产任务，已开始
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("beginTask", Name = "BeginTask")]
    public async Task<BeginTaskResponse> BeginTask(BeginTaskRequest request)
    {
        _logger.LogInformation($"BeginTask,request:{JsonSerializer.Serialize(request)}");
        ThrowHelper.ThrowArgumentNullException(request);
        if (string.IsNullOrEmpty(request.TraceId) && !request.Params.ContainsKey("TaskCode"))
        {
            _logger.LogWarning($"BeginTask TraceId or TaskCode not found. request:{JsonSerializer.Serialize(request)}");
            return new BeginTaskResponse
            {
                Code = ErrorCodes.Sys.MISSING_TRACE_ID_CODE,
                Message = ErrorCodes.Sys.MISSING_TRACE_ID_MESSAGE
            };
        }

        return await _workOrderTaskAdapter.BeginTask(request);
    }

    /// <summary>
    /// 报告生产任务，已结束
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("finishTask", Name = "FinishTask")]
    public async Task<FinishTaskResponse> FinishTask(FinishTaskRequest request)
    {
        _logger.LogInformation($"FinishTask,request:{JsonSerializer.Serialize(request)}");
        ThrowHelper.ThrowArgumentNullException(request);
        if (string.IsNullOrEmpty(request.TraceId) && !request.Params.ContainsKey("TaskCode"))
        {
            _logger.LogWarning($"FinishTask TraceId or TaskCode not found. request:{JsonSerializer.Serialize(request)}");
            return new FinishTaskResponse
            {
                Code = ErrorCodes.Sys.MISSING_TRACE_ID_CODE,
                Message = ErrorCodes.Sys.MISSING_TRACE_ID_MESSAGE
            };
        }

        return await _workOrderTaskAdapter.FinishTask(request);
    }

    /// <summary>
    /// 获取排刀信息
    /// 调试模式，输入指定物料代码，获取预定的排刀信息；
    /// 生产模式，根据traceid，查找调度记录中的物料和任务信息，查找对应的排刀信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("queryCutterArrange", Name = "QueryCutterArrange")]
    public async Task<ApplyCutterArrangeResponse> QueryCutterArrange(ApplyCutterArrangeRequest request)
    {
        _logger.LogInformation($"QueryCutterArrange,request:{JsonSerializer.Serialize(request)}");
        ThrowHelper.ThrowArgumentNullException(request);

        var response = new ApplyCutterArrangeResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = "获取成功",
            Data = new CutterArrange
            {
                WipId = DateTime.Now.ToString("yyyyMMddHHmmss"),
                LifeDefines = _cutterLifeDefines,
                Boxes = new List<BoxAlterDefine>
                {
                    new BoxAlterDefine
                    {
                        BoxIndex = 3,
                    },
                    new BoxAlterDefine
                    {
                        BoxIndex = 4,
                    },
                }
            }
        };

        if (string.IsNullOrEmpty(request.ItemCode))
        {
            response.Code = ErrorCodes.Sys.FAIL;
            response.Message = "未提供物料代码";
            response.Data = null;
        }
        else if (!_itemDrillContents.ContainsKey(request.ItemCode.Trim().ToLower()))
        {
            response.Code = ErrorCodes.Sys.FAIL;
            response.Message = $"未查到此物料的排刀信息，{request.ItemCode}";
            response.Data = null;
        }
        else
        {
            response.Data.Arranges = new List<CutterArrangeDefine>();

            //分析请求中的剩余刀盘的寿命信息
            //todo
            //todo,限定小刀的默认刀盘起始位置，起始site 101， 从小往大
            var startSite = 101;
            var startBoxIndex = 3;
            //todo,限定大刀的起始刀盘位置，200，从大往小
            var endSite = 200;
            var endBoxIndex = 4;
            //todo,需求刀具的总数，必须小于可排刀的位数
            var requirements = _itemDrillContents[request.ItemCode.Trim().ToLower()];
            foreach (var requirement in requirements)
            {
                var lifeDefine = _cutterLifeDefines.FirstOrDefault(x => x.PgmDiameter == requirement.PgmDiameter);
                if (lifeDefine == null)
                {
                    _logger.LogWarning($"{request.DeviceId},{request.ItemCode},PgmDiameter {requirement.PgmDiameter} not define");
                    continue;
                }

                if (requirement.PgmDiameter < this._bigCutterMinValue)
                {
                    var count = Math.Ceiling((double)requirement.NeedDrillCount / lifeDefine.Life);
                    for (var i = 0; i < count; i++)
                    {
                        response.Data.Arranges.Add(new CutterArrangeDefine
                        {
                            BoxIndex = startBoxIndex,
                            Site = startSite++,
                            PgmDiameter = requirement.PgmDiameter,
                            MoCount = 0,
                            Life = lifeDefine.Life,
                        });
                    }
                }
                else //大刀
                {
                    //计算剩余寿命是否够用
                    var biggerCutter = request.Data.Arranges.FirstOrDefault(x => x.PgmDiameter == requirement.PgmDiameter);
                    if (biggerCutter == null)
                    {
                        //按新刀处理
                        response.Data.Arranges.Add(new CutterArrangeDefine
                        {
                            BoxIndex = endBoxIndex,
                            Site = endSite--,
                            PgmDiameter = requirement.PgmDiameter,
                            MoCount = 0,
                            Life = lifeDefine.Life,
                        });
                    }
                    else if (requirement.NeedDrillCount > biggerCutter.Life)
                    {
                        var count = Math.Ceiling((double)(requirement.NeedDrillCount - biggerCutter.Life) / lifeDefine.Life);
                        for (var i = 0; i < count; i++)
                        {
                            response.Data.Arranges.Add(new CutterArrangeDefine
                            {
                                BoxIndex = endBoxIndex,
                                Site = endSite--,
                                PgmDiameter = requirement.PgmDiameter,
                                MoCount = 0,
                                Life = lifeDefine.Life,
                            });
                        }
                    }
                }
            }
        }

        _logger.LogInformation($"QueryCutterArrange,{request.DeviceId},{request.ItemCode},response:{JsonSerializer.Serialize(response)}");

        return await Task.FromResult(response);
    }
}
