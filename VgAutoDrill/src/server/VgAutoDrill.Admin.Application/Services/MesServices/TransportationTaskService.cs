using AutoMapper;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Linq.Expressions;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class TransportationTaskService : BaseServiceWithoutTree<TransferJob, TransferJobDto, AddOrUpdateTransferJobReq>, ITransportationTaskService
    {
        private readonly ITransportationTaskDomainService _transportationTaskDomainService;
        private readonly ITransportationHistoryTaskDomainService _transportationHistoryTaskDomainService;
        private readonly IWorkOrderService _workOrderService;
        private readonly ILogger<TransportationTaskService> _logger;
        private readonly ITransferJobLogDomainService _transferJobLogDomainService;
        private readonly IScheduleDomainService _scheduleDomainService;
        private readonly IRackDomainService _rackDomainService;
        private readonly ISysConfigManager _sysConfigManager;

        /// <summary>
        ///
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public TransportationTaskService(ITransportationTaskDomainService domainService,
            IWorkOrderService workOrderService,
            ILogger<TransportationTaskService> logger,
            ITransferJobLogDomainService transferJobLogDomainService,
            IMapper mapper,
            ITransportationHistoryTaskDomainService transportationHistoryTaskDomainService,
            IScheduleDomainService scheduleDomainService,
            IRackDomainService rackDomainService,
            ISysConfigManager sysConfigManager)
            : base(domainService, mapper)
        {
            _transportationTaskDomainService = domainService;
            _workOrderService = workOrderService;
            _logger = logger;
            _transferJobLogDomainService = transferJobLogDomainService;
            _transportationHistoryTaskDomainService = transportationHistoryTaskDomainService;
            _scheduleDomainService = scheduleDomainService;
            _rackDomainService = rackDomainService;
            _sysConfigManager = sysConfigManager;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<TransferJobDto>>> GetList(GetTransferJobListReq req)
        {
            var pageDto = new PageDto<TransferJobDto>(req.PageNum, req.PageSize);
            var where = GenerateQueryExpression(req);

            var result = await _domainService.QueryPageAsync(where, q => q.Id, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<TransferJob>, List<TransferJobDto>>(result.ToList());

            if (pageDto.List.Count > 0)
            {
                var forkCodes = pageDto.List.Where(s => !string.IsNullOrWhiteSpace(s.ForkCode)).Select(s => s.ForkCode).Distinct().ToList();
                if (forkCodes?.Count > 0)
                {
                    var racks = await _rackDomainService.QueryAsync(s => forkCodes.Contains(s.Code), r => r.Id, OrderByType.Asc);
                    if (racks?.Count > 0)
                    {
                        foreach (var item in pageDto.List)
                        {
                            var positionCodes = racks.Where(s => s.Code == item.ForkCode && !string.IsNullOrWhiteSpace(s.PositionCode))
                                                     .Select(s => s.PositionCode).Distinct().ToList();
                            item.PositionCodes = positionCodes?.Count > 0 ? string.Join(",", positionCodes) : "";
                        }
                    }
                }
            }
            return Success(pageDto);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateTransferJobReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (string.IsNullOrEmpty(req.ForkCode) || !req.InteractionSequence.HasValue)
            {
                return Fail("未识别有效的料架编号或交互方式 !");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            List<ScheduledTaskStatus> scheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Allocated,
                ScheduledTaskStatus.Delivered,
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.PartCompleted,
                ScheduledTaskStatus.Running,
                ScheduledTaskStatus.WaitingForAgv
            };

            if (!string.IsNullOrEmpty(req.ForkCode)
                && await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ForkCode) && p.ForkCode.ToLower() == req.ForkCode.ToLower()
                && scheduledTaskStatusList.Contains((ScheduledTaskStatus)p.ScheduledTaskStatus)))
            {
                return Fail($"{req.ForkCode} 已存在料仓任务!");
            }

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.IS_CHECK_SILO_USED))
            {
                if (!string.IsNullOrEmpty(req.SiloCode)
                    && await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.ToLower() == req.SiloCode.ToLower()
                    && scheduledTaskStatusList.Contains((ScheduledTaskStatus)p.ScheduledTaskStatus)))
                {
                    return Fail($"已存在料仓 {req.SiloCode}!");
                }
            }

            if (req.InteractionSequence.HasValue && !string.IsNullOrEmpty(req.ForkCode)
                && !await _scheduleDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.SubDeviceCode) && p.SubDeviceCode.ToLower() == req.ForkCode.ToLower()
                && p.InteractionSequence == req.InteractionSequence && p.ScheduledTaskStatus == ScheduledTaskStatus.Created))
            {
                return Fail($"调度记录不存在 {req.ForkCode} 交互方式 {req.InteractionSequence} 调度状态为已上报的记录!");
            }

            if (!string.IsNullOrEmpty(req.ForkCode)
                && !await _rackDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == req.ForkCode.ToLower()
                && (p.DeviceKind == DeviceKind.PanelSiloFork || p.DeviceKind == DeviceKind.PublicPanelSiloWIP)))
            {
                return Fail($"{req.ForkCode} 设备类别不是叉齿或者线边仓库区!");
            }

            var model = _mapper.Map<TransferJob>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;

            //补充料仓任务的行为模式
            if (!model.TransferBehavior.HasValue)
            {
                if (model.TransportationKind == TransportationKind.EmptySilo && model.InteractionSequence == InteractionSequence.LoadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.OutsideToFork, MaterialKind.EmptyPanelSilo, PanelKind.Unspecified);
                }
                else if (model.TransportationKind == TransportationKind.EmptySilo && model.InteractionSequence == InteractionSequence.UnloadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.ForkToOutside, MaterialKind.EmptyPanelSilo, PanelKind.Unspecified);
                }
                else if (model.TransportationKind == TransportationKind.Raw && model.InteractionSequence == InteractionSequence.LoadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.OutsideToFork, MaterialKind.PanelSilo, PanelKind.Undrilled);
                }
                else if (model.TransportationKind == TransportationKind.Clinker && model.InteractionSequence == InteractionSequence.UnloadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.ForkToOutsideUnPin, MaterialKind.PanelSilo, PanelKind.Drilled);
                }
                else if (model.TransportationKind == TransportationKind.First && model.InteractionSequence == InteractionSequence.UnloadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.ForkToOutsideUnPin, MaterialKind.PanelSilo, PanelKind.FirstDrilled);
                }
            }

            var jobId = await _domainService.AddReturnId(model);

            return Success();
        }

        public async Task<ResponseDto<int>> AddWithReturnId(AddOrUpdateTransferJobReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!", 0);
            }

            if (string.IsNullOrEmpty(req.ForkCode) || !req.InteractionSequence.HasValue)
            {
                return Fail("未识别有效的料架编号或交互方式 !", 0);
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!", 0);
            }

            List<ScheduledTaskStatus> scheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Allocated,
                ScheduledTaskStatus.Delivered,
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.PartCompleted,
                ScheduledTaskStatus.Running,
                ScheduledTaskStatus.WaitingForAgv
            };

            if (!string.IsNullOrEmpty(req.ForkCode)
                && await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ForkCode) && p.ForkCode.ToLower() == req.ForkCode.ToLower()
                && scheduledTaskStatusList.Contains((ScheduledTaskStatus)p.ScheduledTaskStatus)))
            {
                return Fail($"{req.ForkCode} 已存在料仓任务!", 0);
            }

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.IS_CHECK_SILO_USED))
            {
                if (!string.IsNullOrEmpty(req.SiloCode)
                    && await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.ToLower() == req.SiloCode.ToLower()
                    && scheduledTaskStatusList.Contains((ScheduledTaskStatus)p.ScheduledTaskStatus)))
                {
                    return Fail($"已存在料仓 {req.SiloCode}!", 0);
                }
            }

            if (req.InteractionSequence.HasValue && !string.IsNullOrEmpty(req.ForkCode)
                && !await _scheduleDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.SubDeviceCode) && p.SubDeviceCode.ToLower() == req.ForkCode.ToLower()
                && p.InteractionSequence == req.InteractionSequence && p.ScheduledTaskStatus == ScheduledTaskStatus.Created))
            {
                return Fail($"调度记录不存在 {req.ForkCode} 交互方式 {req.InteractionSequence} 调度状态为已上报的记录!", 0);
            }

            if (!string.IsNullOrEmpty(req.ForkCode)
                && !await _rackDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == req.ForkCode.ToLower()
                && (p.DeviceKind == DeviceKind.PanelSiloFork || p.DeviceKind == DeviceKind.PublicPanelSiloWIP)))
            {
                return Fail($"{req.ForkCode} 设备类别不是叉齿或者线边仓库区!", 0);
            }

            var model = _mapper.Map<TransferJob>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;

            //补充料仓任务的行为模式
            if (!model.TransferBehavior.HasValue)
            {
                if (model.TransportationKind == TransportationKind.EmptySilo && model.InteractionSequence == InteractionSequence.LoadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.OutsideToFork, MaterialKind.EmptyPanelSilo, PanelKind.Unspecified);
                }
                else if (model.TransportationKind == TransportationKind.EmptySilo && model.InteractionSequence == InteractionSequence.UnloadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.ForkToOutside, MaterialKind.EmptyPanelSilo, PanelKind.Unspecified);
                }
                else if (model.TransportationKind == TransportationKind.Raw && model.InteractionSequence == InteractionSequence.LoadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.OutsideToFork, MaterialKind.PanelSilo, PanelKind.Undrilled);
                }
                else if (model.TransportationKind == TransportationKind.Clinker && model.InteractionSequence == InteractionSequence.UnloadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.ForkToOutsideUnPin, MaterialKind.PanelSilo, PanelKind.Drilled);
                }
                else if (model.TransportationKind == TransportationKind.First && model.InteractionSequence == InteractionSequence.UnloadOnly)
                {
                    model.TransferBehavior = SiloTransferBehavior.Make(TransferPathKind.ForkToOutsideUnPin, MaterialKind.PanelSilo, PanelKind.FirstDrilled);
                }
            }

            var jobId = await _domainService.AddReturnId(model);

            return Success(jobId, string.Empty);
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateTransferJobReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            if (req.ScheduledTaskStatus.HasValue && req.ScheduledTaskStatus == ScheduledTaskStatus.Created)
            {
                List<ScheduledTaskStatus> scheduledTaskStatusList = new List<ScheduledTaskStatus>
                {
                    ScheduledTaskStatus.Allocated,
                    ScheduledTaskStatus.Delivered,
                    ScheduledTaskStatus.Created,
                    ScheduledTaskStatus.PartCompleted,
                    ScheduledTaskStatus.Running,
                    ScheduledTaskStatus.WaitingForAgv
                };
                if (!string.IsNullOrEmpty(req.ForkCode)
                    && await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ForkCode) && p.ForkCode.ToLower() == req.ForkCode.ToLower()
                    && scheduledTaskStatusList.Contains((ScheduledTaskStatus)p.ScheduledTaskStatus)
                    && p.Id != req.Id))
                {
                    return Fail($"{req.ForkCode} 已存在料仓任务!");
                }
                if (!string.IsNullOrEmpty(req.SiloCode)
                    && await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.ToLower() == req.SiloCode.ToLower()
                    && scheduledTaskStatusList.Contains((ScheduledTaskStatus)p.ScheduledTaskStatus)
                    && p.Id != req.Id))
                {
                    return Fail($"已存在料仓 {req.SiloCode}!");
                }
                if (req.InteractionSequence.HasValue && !string.IsNullOrEmpty(req.ForkCode)
                    && !await _scheduleDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.SubDeviceCode) && p.SubDeviceCode.ToLower() == req.ForkCode.ToLower()
                    && p.InteractionSequence == req.InteractionSequence && p.ScheduledTaskStatus == ScheduledTaskStatus.Created))
                {
                    return Fail($"调度记录不存在 {req.ForkCode} 交互方式 {req.InteractionSequence} 调度状态为已上报的记录!");
                }
            }

            var model = _mapper.Map<Model.Entites.Mes.TransferJob>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        /// <summary>
        /// -1 -- 异常中止
        /// -2 -- 取消（排队的任务，可以被取消）
        /// 1 -- 已上报
        /// 3 -- 开始调度
        /// 4 -- 调度完成
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<string> UpdateTransportation(AddOrUpdateTransferJobReq req)
        {
            // 未提供status值时，退出方法
            if (req.ScheduledTaskStatus == ScheduledTaskStatus.None)
            {
                _logger.LogError($"UpdateTransportation, code:{req.Code},未提供status值时，退出方法");
                return $"UpdateTransportation, code:{req.Code},未提供status值时，退出方法";
            }

            //防止FindSingleAsync异常
            if (!await _domainService.IsExistAsync(x => x.Code.ToLower() == req.Code.ToLower()))
            {
                _logger.LogError($"UpdateTransportation, code:{req.Code},不存在该项");
                return $"UpdateTransportation, code:{req.Code},不存在该项";
            }

            var transTask = await _domainService.FindSingleAsync(x => x.Code.ToLower() == req.Code.ToLower());
            if (transTask == null)
            {
                _logger.LogError($"UpdateTransportation, code:{req.Code},不存在该项");
                return $"UpdateTransportation, code:{req.Code},不存在该项";
            }

            if (!string.IsNullOrEmpty(req.HkResponse))
            {
                transTask.HkResponse = req.HkResponse;
            }

            if (!string.IsNullOrEmpty(req.HikResponseKey))
            {
                transTask.HikResponseKey = req.HikResponseKey;
            }

            if (req.ScheduledTaskStatus == ScheduledTaskStatus.Running)
            {
                if (transTask.ScheduledTaskStatus != ScheduledTaskStatus.Created)
                {
                    _logger.LogWarning($"UpdateTransportation, code:{req.Code},状态变更非法,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                    return $"UpdateTransportation, code:{req.Code},状态变更非法,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}";
                }

                transTask.AllocateTime = DateTime.Now;
                transTask.RunningTime = DateTime.Now;
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Completed) //AGV 配送完成
            {
                if (transTask.ScheduledTaskStatus == ScheduledTaskStatus.Failed
                    || transTask.ScheduledTaskStatus == ScheduledTaskStatus.Canceled
                    || transTask.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
                {
                    _logger.LogWarning($"UpdateTransportation, code:{req.Code},状态变更非法,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                    return $"UpdateTransportation, code:{req.Code},状态变更非法,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}";
                }

                if (transTask.InteractionSequence == InteractionSequence.LoadOnly)
                {
                    transTask.ExternalLotNo = req.ExternalLotNo;
                    transTask.SiloCode = req.SiloCode;
                    transTask.RawCount = req.RawCount;
                }

                transTask.CompletedTime = DateTime.Now;
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Failed) //重复报告异常时
            {
                if (transTask.ScheduledTaskStatus == ScheduledTaskStatus.Failed
                    || transTask.ScheduledTaskStatus == ScheduledTaskStatus.Canceled
                    || transTask.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
                {
                    _logger.LogWarning($"UpdateTransportation, code:{req.Code},状态变更非法-重复报告异常,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                    return $"UpdateTransportation, code:{req.Code},状态变更非法-重复报告异常,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}";
                }

                transTask.FailedTime = DateTime.Now;
            }
            else if (req.ScheduledTaskStatus == ScheduledTaskStatus.Canceled) //取消
            {
                if (transTask.ScheduledTaskStatus == ScheduledTaskStatus.Failed
                    || transTask.ScheduledTaskStatus == ScheduledTaskStatus.Canceled
                    || transTask.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
                {
                    _logger.LogWarning($"UpdateTransportation, code:{req.Code},状态变更非法,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
                    return $"UpdateTransportation, code:{req.Code},状态变更非法,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}";
                }

                transTask.CanceledTime = DateTime.Now;
                if (!string.IsNullOrEmpty(req.RelatedDrillTrace))
                {
                    transTask.RelatedDrillTrace = req.RelatedDrillTrace;
                }
                _logger.LogWarning($"UpdateTransportation, code:{req.Code},取消料仓转运计划,old:{transTask.ScheduledTaskStatus},new:{req.ScheduledTaskStatus}");
            }

            transTask.ScheduledTaskStatus = req.ScheduledTaskStatus;
            transTask.ModifyTime = DateTime.Now;
            await _domainService.Update(transTask);

            if (req.ScheduledTaskStatus == ScheduledTaskStatus.Canceled)
            {
                ////添加后台处理任务
            }

            //AGV 配送完成 ,根据不同情况，调整生产工单
            if (req.ScheduledTaskStatus == ScheduledTaskStatus.Completed
                && !string.IsNullOrEmpty(transTask.InternalLotNo))
            {
                if (transTask.TransportationKind == TransportationKind.Raw)
                {
                    await _workOrderService.SetMoveInTime(transTask.InternalLotNo, DateTime.Now);
                }
                else if (transTask.TransportationKind == TransportationKind.Clinker
                    || transTask.TransportationKind == TransportationKind.First)
                {
                    //工单全部下机（已有track out time)，并且是最后一个料仓转出时
                    if (await CanMoveOut(transTask.InternalLotNo))
                    {
                        await _workOrderService.SetMoveOutTime(transTask.InternalLotNo, DateTime.Now);
                    }
                }
            }

            return string.Empty;
        }

        public async Task<bool> Exsist(string code)
        {
            return await _domainService.IsExistAsync(x => x.Code.ToLower() == code.ToLower());
        }

        public async Task<TransferJobDto> FindSingle(string code)
        {
            if (await Exsist(code))
            {
                var entity = await _domainService.FindSingleAsync(x => x.Code.ToLower() == code.ToLower());
                return _mapper.Map<TransferJobDto>(entity);
            }
            else
            {
                return null;
            }
        }

        public async Task<int> ClearTimeoutTransportationTask()
        {
            var queryCondition = GenerateQueryExpression(new GetTransferJobListReq
            {
                TimeOutSecondes = 60 * 10,
            });

            var tasks = await _domainService.QueryAsync(queryCondition, x => x.Id, SqlSugar.OrderByType.Asc);
            var udpateList = new List<TransferJob>();
            foreach (var task in tasks)
            {
                task.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
                task.CanceledTime = DateTime.Now;
                udpateList.Add(task);
            }

            if (await _domainService.BulkUpdate(udpateList))
            {
                return udpateList.Count;
            }

            return 0;
        }

        private async Task<bool> CanMoveOut(string internalLotNo)
        {
            if (!string.IsNullOrEmpty(internalLotNo) && await _workOrderService.Exsist(internalLotNo))
            {
                var order = await _workOrderService.FindSingle(internalLotNo);
                return order.WadCount == await SumClinker(internalLotNo);
            }

            return false;
        }

        private async Task<int?> SumClinker(string innerLotNo)
        {
            var clinkerTypes = new List<TransportationKind> {
                 TransportationKind.Clinker,
                  TransportationKind.First,
            };
            var tasks = await _transportationTaskDomainService.QueryAsync(x => x.InternalLotNo.ToLower() == innerLotNo.ToLower()
                                                                && x.InteractionSequence == InteractionSequence.UnloadOnly
                                                                && x.TransportationKind.HasValue
                                                                && clinkerTypes.Contains((TransportationKind)x.TransportationKind),
                            x => x.Id,
                            SqlSugar.OrderByType.Asc);
            return tasks.Sum(x => x.ClinkerCount);
        }

        private static Expression<Func<TransferJob, bool>> GenerateQueryExpression(GetTransferJobListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var where = PredicateBuilder.True<TransferJob>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.InternalLotNo))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.InternalLotNo) && p.InternalLotNo.ToUpper().Contains(req.InternalLotNo.ToUpper()));
            }

            if (!string.IsNullOrEmpty(req.ExternalLotNo))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ExternalLotNo) && p.ExternalLotNo.Contains(req.ExternalLotNo));
            }

            if (!string.IsNullOrEmpty(req.WarehouseCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WarehouseCode) && p.WarehouseCode.Contains(req.WarehouseCode));
            }

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.Contains(req.SiloCode));
            }

            if (!string.IsNullOrEmpty(req.ForkCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ForkCode) && p.ForkCode.Contains(req.ForkCode));
            }

            if (!string.IsNullOrEmpty(req.RelatedDrillTrace))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.RelatedDrillTrace) && p.RelatedDrillTrace.Contains(req.RelatedDrillTrace));
            }

            if (req.InteractionSequence.HasValue)
            {
                where = where.And(p => p.InteractionSequence == req.InteractionSequence);
            }

            if (req.ScheduledTaskStatusList != null && req.ScheduledTaskStatusList.Count > 0)
            {
                where = where.And(p => p.ScheduledTaskStatus != null && req.ScheduledTaskStatusList.Contains((ScheduledTaskStatus)p.ScheduledTaskStatus));
            }

            if (req.TimeOutSecondes > 0)
            {
                where = where.And(p => p.ScheduledTaskStatus == ScheduledTaskStatus.Created && p.CreateTime < DateTime.Now.AddSeconds(-1 * req.TimeOutSecondes.Value));
            }

            if (req.TransportationKind.HasValue)
            {
                where = where.And(p => p.TransportationKind == req.TransportationKind);
            }

            if (req.IsUrgent != null)
            {
                where = where.And(p => p.IsUrgent == req.IsUrgent);
            }

            if (req.JobId != null)
            {
                where = where.And(p => p.JobId == req.JobId);
            }

            if (req.PlanId != null)
            {
                where = where.And(p => p.PlanId == req.PlanId);
            }

            if (!string.IsNullOrEmpty(req.StartDeviceId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.StartDeviceId) && p.StartDeviceId.ToLower().Contains(req.StartDeviceId.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.EndDeviceId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.EndDeviceId) && p.EndDeviceId.ToLower().Contains(req.EndDeviceId.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.StartLocationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.StartLocationCode) && p.StartLocationCode.ToLower().Contains(req.StartLocationCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.EndLocationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.EndLocationCode) && p.EndLocationCode.ToLower().Contains(req.EndLocationCode.ToLower()));
            }

            if (req.StartScheduleId > 0)
            {
                where = where.And(p => p.StartScheduleId == req.StartScheduleId);
            }
            if (req.EndScheduleId > 0)
            {
                where = where.And(p => p.EndScheduleId == req.EndScheduleId);
            }
            if (req.TransferBehavior > 0)
            {
                where = where.And(p => p.TransferBehavior == req.TransferBehavior);
            }

            if (!string.IsNullOrEmpty(req.HkResponse))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.HkResponse) && p.HkResponse.ToLower().Contains(req.HkResponse.ToLower()));
            }

            if (req.IsManual != null)
            {
                where = where.And(p => p.IsManual == req.IsManual);
            }

            if (!string.IsNullOrEmpty(req.ClinkerRemark))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ClinkerRemark) && p.ClinkerRemark.ToLower().Contains(req.ClinkerRemark.ToLower()));
            }
            if (req.StartTime != null)
            {
                where = where.And(p => p.CreateTime >= req.StartTime.Value);
            }
            if (req.EndTime != null)
            {
                where = where.And(p => p.CreateTime <= req.EndTime.Value);
            }
            return where;
        }

        public async Task<ResponseDto<TransferJobLogsDto>> QueryLogsByID(long id)
        {
            TransferJobLogsDto result = null;
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                var hisEntity = await _transportationHistoryTaskDomainService.QueryByID(id);
                if (hisEntity == null)
                {
                    return Fail<TransferJobLogsDto>("信息不存在!");
                }
                else
                {
                    result = _mapper.Map<TransferJobLogsDto>(hisEntity);
                }
            }
            else
            {
                result = _mapper.Map<TransferJobLogsDto>(entity);
            }

            var details = await _transferJobLogDomainService.QueryAsync(p => p.MasterId == id, p => p.Id, OrderByType.Desc);
            if (details != null)
            {
                result.TransferJobLogDtos = _mapper.Map<List<TransferJobLog>, List<TransferJobLogDto>>(details.ToList());

                var transferJobs = await _domainService.QueryAsync(s => s.Id == id, p => p.Id, OrderByType.Asc);
                if (transferJobs?.Count > 0)
                {
                    var forkCode = transferJobs.FirstOrDefault()?.ForkCode;
                    var racks = await _rackDomainService.QueryAsync(s => s.Code == forkCode, p => p.Id, OrderByType.Asc);
                    if (racks?.Count > 0)
                    {
                        foreach (var item in result.TransferJobLogDtos)
                        {
                            item.PositionCode = racks.FirstOrDefault()?.PositionCode;
                        }
                    }
                }
            }
            return Success(result);
        }

        public async Task<ResponseDto<string>> CancelSingle(long id, bool isForce = false)
        {
            var data = await _domainService.QueryByID(id);
            if (data == null)
            {
                return Fail($"未找到数据 {id}!");
            }

            if (isForce)
            {
                if (data.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
                    && data.ScheduledTaskStatus != ScheduledTaskStatus.Created
                    && data.ScheduledTaskStatus != ScheduledTaskStatus.Running)
                {
                    return Fail($"{id} 调度状态不支持取消操作!");
                }
            }
            else
            {
                if (data.ScheduledTaskStatus != ScheduledTaskStatus.Allocated && data.ScheduledTaskStatus != ScheduledTaskStatus.Created)
                {
                    return Fail($"{id} 调度状态不支持取消操作!");
                }
            }

            _logger.LogInformation($"ID:{data.Id},取消料仓任务,old:{data.ScheduledTaskStatus},new:{ScheduledTaskStatus.Canceled}");

            data.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
            data.CanceledTime = DateTime.Now;
            data.ModifierId = UserId;
            data.ModifyTime = DateTime.Now;

            var result = await _domainService.Update(data);
            if (!result)
            {
                return Fail($"{id} 取消失败，请稍后重试!");
            }
            else
            {
                await AddLog(data.Id, $"料仓任务已被取消,ID:{data.Id}");
            }

            return Success();
        }

        public async Task<ResponseDto<string>> BulkCancel(List<long> ids, bool isForce = false)
        {
            if (ids?.Count <= 0)
            {
                return Fail($"需要取消的任务id集合不能为空!");
            }
            var list = await _domainService.QueryAsync(s => ids.Contains(s.Id), s => s.Id, OrderByType.Asc);

            var logs = new List<AddOrUpdateTransferJobLogReq>();

            foreach (var data in list)
            {
                if (isForce)
                {
                    if (data.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
                        && data.ScheduledTaskStatus != ScheduledTaskStatus.Created
                        && data.ScheduledTaskStatus != ScheduledTaskStatus.Running)
                    {
                        return Fail($"{data.Id} 调度状态不支持取消操作!");
                    }
                }
                else
                {
                    if (data.ScheduledTaskStatus != ScheduledTaskStatus.Allocated && data.ScheduledTaskStatus != ScheduledTaskStatus.Created)
                    {
                        return Fail($"{data.Id} 调度状态不支持取消操作!");
                    }
                }
                _logger.LogWarning($"TransferJob CancelSingle, code:{data.Code},取消料仓任务,old:{data.ScheduledTaskStatus},new:{ScheduledTaskStatus.Canceled}");
                data.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
                data.CanceledTime = DateTime.Now;
                data.ModifierId = UserId;
                data.ModifyTime = DateTime.Now;

                logs.Add(new AddOrUpdateTransferJobLogReq { MasterId = data.Id, Message = $"料仓任务已被取消,ID:{data.Id}" });
            }

            var result = await _domainService.BulkUpdate(list);

            if (result)
            {
                await AddLog(logs);
            }
            else
            {
                return Fail("批量提交时发生错误了");
            }

            return Success();
        }

        public async Task<string> UpdateTransportationByHikResponseKey(AddOrUpdateTransferJobReq req)
        {
            if (!await _domainService.IsExistAsync(x => x.HikResponseKey.ToLower() == req.HikResponseKey.ToLower()))
            {
                _logger.LogError($"UpdateTransportation, hikResponseKey:{req.HikResponseKey},不存在该项");
                return $"UpdateTransportation, hikResponseKey:{req.HikResponseKey},不存在该项";
            }

            var transTask = await _domainService.FindSingleAsync(x => x.HikResponseKey.ToLower() == req.HikResponseKey.ToLower());
            if (transTask == null)
            {
                _logger.LogError($"UpdateTransportation, hikResponseKey:{req.HikResponseKey},不存在该项");
                return $"UpdateTransportation, hikResponseKey:{req.HikResponseKey},不存在该项";
            }
            if (!string.IsNullOrEmpty(req.AllocatedAgv))
            {
                transTask.AllocatedAgv = req.AllocatedAgv;
            }

            if (!string.IsNullOrEmpty(req.HkResponse))
            {
                transTask.HkResponse = req.HkResponse;
            }

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                transTask.SiloCode = req.SiloCode;
            }

            if (!string.IsNullOrEmpty(req.ExternalLotNo))
            {
                transTask.ExternalLotNo = req.ExternalLotNo;
            }

            transTask.RawCount = req.RawCount;
            transTask.ModifyTime = DateTime.Now;

            await _domainService.Update(transTask);

            return string.Empty;
        }

        public async Task<TransferJobDto> FindSingleByHikResponseKey(string hikResponseKey)
        {
            var isExsist = await _domainService.IsExistAsync(x => x.HikResponseKey.ToLower() == hikResponseKey.ToLower());

            if (isExsist)
            {
                var entity = await _domainService.FindSingleAsync(x => x.HikResponseKey.ToLower() == hikResponseKey.ToLower());
                return _mapper.Map<TransferJobDto>(entity);
            }
            else
            {
                return null;
            }
        }
        public async Task RegularDeleteHisData()
        {
            DateTime recordsTime = DateTime.Now.AddMonths(-12);
            await _transportationHistoryTaskDomainService.DeleteAsync(p => p.CreateTime < recordsTime);
        }

        public async Task<List<TransportationTaskToExcelDto>> GetToExcelList(GetTransferJobListReq req)
        {
            var resultDtos = new List<TransportationTaskToExcelDto>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 100) req.PageSize = 65536;

            var result = await GetList(req);
            if (result == null || result.Data == null || result.Data.List.Count == 0)
            {
                return resultDtos;
            }
            resultDtos = _mapper.Map<List<TransportationTaskToExcelDto>>(result.Data.List);
            return resultDtos;
        }

        public async Task<List<TransportationTaskToExcelDto>> GetToHistoryExcelList(GetTransferJobListReq req)
        {
            var resultDtos = new List<TransportationTaskToExcelDto>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 100) req.PageSize = 65536;

            var result = await GetHistoryList(req);
            if (result == null || result.Data == null || result.Data.List.Count == 0)
            {
                return resultDtos;
            }
            resultDtos = _mapper.Map<List<TransportationTaskToExcelDto>>(result.Data.List);
            return resultDtos;
        }

        /// <summary>
        /// 转移调度历史数据
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TransferTransportationHistoryTaskData(TransferJobHistoryDataReq req)
        {
            var swaitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.TRANSSFER_TRANSPORTATION_HISTORY_SWITCH);
            if (!swaitch)
            {
                return false;
            }
            var time = req.TransferTime != null ? req.TransferTime : DateTime.Now.AddDays(-3);
            var data = await _transportationTaskDomainService.QueryAsync(s => s.CreateTime <= time, r => r.Id, OrderByType.Asc);
            _logger.LogInformation($"转移料仓任务历史数据:查询到{time}之前有{data?.Count()}条数据");
            if (data?.Count() > 0)
            {
                var history = _mapper.Map<List<TransportationHistoryTask>>(data);
                var historyIds = history.Select(r => r.Id).ToList();
                var beforeInsertExistDatas = await _transportationHistoryTaskDomainService.QueryAsync(s => historyIds.Contains(s.Id), r => r.Id, OrderByType.Asc);
                if (beforeInsertExistDatas?.Count > 0)//过滤掉未知原因没有删除成功的数据，防止重复添加，引起主键冲突
                {
                    history = history.Where(s => !beforeInsertExistDatas.Any(m => m.Id == s.Id)).ToList();//取差集
                }
                //批量插入历史表
                for (var i = 0; i <= history.Count / 2000; i++)
                {
                    var insertDatas = history.Skip(i * 2000).Take(2000).ToList();
                    if (insertDatas?.Count > 0)
                    {
                        await _transportationHistoryTaskDomainService.BulkInsert(insertDatas);
                    }
                }
                _logger.LogInformation($"转移料仓任务历史数据:插入t_transportation_history_task表{history?.Count()}条数据");
                //批量删除原有表数据
                var ids = data.Select(r => r.Id).ToList();
                var afterInsertExistDatas = await _transportationHistoryTaskDomainService.QueryAsync(s => ids.Contains(s.Id), r => r.Id, OrderByType.Asc);
                if (afterInsertExistDatas?.Count > 0)//检验已添加的数据
                {
                    var deleteDatas = data.Where(r => afterInsertExistDatas.Any(m => m.Id == r.Id)).ToList();
                    for (var i = 0; i <= deleteDatas.Count / 2000; i++)
                    {
                        var removeDatas = deleteDatas.Skip(i * 2000).Take(2000).ToList();
                        if (removeDatas?.Count > 0)
                        {
                            await _transportationTaskDomainService.BulkDelete(removeDatas);
                        }
                    }
                    _logger.LogInformation($"转移料仓任务历史数据:删除t_transportation_task表{deleteDatas?.Count()}条数据");
                }
            }
            return true;
        }

        public async Task<ResponseDto<PageDto<TransferJobDto>>> GetHistoryList(GetTransferJobListReq req)
        {
            var pageDto = new PageDto<TransferJobDto>(req.PageNum, req.PageSize);
            var where = GenerateQueryHistoryDataExpression(req);

            var result = await _transportationHistoryTaskDomainService.QueryPageAsync(where, q => q.Id, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<TransportationHistoryTask>, List<TransferJobDto>>(result.ToList());
            if (pageDto.List.Count > 0)
            {
                var forkCodes = pageDto.List.Where(s => !string.IsNullOrWhiteSpace(s.ForkCode)).Select(s => s.ForkCode).Distinct().ToList();
                if (forkCodes?.Count > 0)
                {
                    var racks = await _rackDomainService.QueryAsync(s => forkCodes.Contains(s.Code), r => r.Id, OrderByType.Asc);
                    if (racks?.Count > 0)
                    {
                        foreach (var item in pageDto.List)
                        {
                            var positionCodes = racks.Where(s => s.Code == item.ForkCode && !string.IsNullOrWhiteSpace(s.PositionCode))
                                                     .Select(s => s.PositionCode).Distinct().ToList();
                            item.PositionCodes = positionCodes?.Count > 0 ? string.Join(",", positionCodes) : "";
                        }
                    }
                }
            }
            return Success(pageDto);
        }

        public async Task<ResponseDto<TransferJobDto>> GetHistory(long id)
        {
            var entity = await _transportationHistoryTaskDomainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<TransferJobDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<TransferJobDto>("信息已标记为删除!");
            }
            var model = _mapper.Map<TransferJobDto>(entity);

            return Success(model);
        }

        private async Task AddLog(long masterId, string message)
        {
            if (masterId == 0 || string.IsNullOrEmpty(message)) return;

            var log = new TransferJobLog
            {
                MasterId = masterId,
                Message = message.Length > 5000 ? message.Substring(0, 5000) : message,
                CreateTime = DateTime.Now,
                CreatorId = UserId,
                Status = (int)DataStatusEnum.Enable,
            };

            await _transferJobLogDomainService.Add(log);
        }

        private async Task AddLog(List<AddOrUpdateTransferJobLogReq> logs)
        {
            if (logs == null || logs.Count == 0)
            {
                return;
            }

            List<TransferJobLog> addList = new List<TransferJobLog>();

            foreach (var log in logs)
            {
                if (log.MasterId == null || log.MasterId == 0)
                {
                    continue;
                }

                addList.Add(new TransferJobLog
                {
                    MasterId = log.MasterId,
                    Message = log.Message.Length > 5000 ? log.Message.Substring(0, 5000) : log.Message,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                    Status = (int)DataStatusEnum.Enable,
                });
            }

            await _transferJobLogDomainService.BulkInsert(addList);
        }

        private static Expression<Func<TransportationHistoryTask, bool>> GenerateQueryHistoryDataExpression(GetTransferJobListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var where = PredicateBuilder.True<TransportationHistoryTask>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.InternalLotNo))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.InternalLotNo) && p.InternalLotNo.ToUpper().Contains(req.InternalLotNo.ToUpper()));
            }

            if (!string.IsNullOrEmpty(req.ExternalLotNo))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ExternalLotNo) && p.ExternalLotNo.Contains(req.ExternalLotNo));
            }

            if (!string.IsNullOrEmpty(req.WarehouseCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WarehouseCode) && p.WarehouseCode.Contains(req.WarehouseCode));
            }

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.Contains(req.SiloCode));
            }

            if (!string.IsNullOrEmpty(req.ForkCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ForkCode) && p.ForkCode.Contains(req.ForkCode));
            }

            if (!string.IsNullOrEmpty(req.RelatedDrillTrace))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.RelatedDrillTrace) && p.RelatedDrillTrace.Contains(req.RelatedDrillTrace));
            }

            if (req.InteractionSequence.HasValue)
            {
                where = where.And(p => p.InteractionSequence == req.InteractionSequence);
            }

            if (req.ScheduledTaskStatusList != null && req.ScheduledTaskStatusList.Count > 0)
            {
                where = where.And(p => p.ScheduledTaskStatus != null && req.ScheduledTaskStatusList.Contains((ScheduledTaskStatus)p.ScheduledTaskStatus));
            }

            if (req.TimeOutSecondes > 0)
            {
                where = where.And(p => p.ScheduledTaskStatus == ScheduledTaskStatus.Created && p.CreateTime < DateTime.Now.AddSeconds(-1 * req.TimeOutSecondes.Value));
            }

            if (req.TransportationKind.HasValue)
            {
                where = where.And(p => p.TransportationKind == req.TransportationKind);
            }

            if (req.IsUrgent != null)
            {
                where = where.And(p => p.IsUrgent == req.IsUrgent);
            }

            if (req.JobId != null)
            {
                where = where.And(p => p.JobId == req.JobId);
            }

            if (req.PlanId != null)
            {
                where = where.And(p => p.PlanId == req.PlanId);
            }

            if (!string.IsNullOrEmpty(req.StartDeviceId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.StartDeviceId) && p.StartDeviceId.ToLower().Contains(req.StartDeviceId.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.EndDeviceId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.EndDeviceId) && p.EndDeviceId.ToLower().Contains(req.EndDeviceId.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.StartLocationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.StartLocationCode) && p.StartLocationCode.ToLower().Contains(req.StartLocationCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.EndLocationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.EndLocationCode) && p.EndLocationCode.ToLower().Contains(req.EndLocationCode.ToLower()));
            }

            if (req.StartScheduleId > 0)
            {
                where = where.And(p => p.StartScheduleId == req.StartScheduleId);
            }
            if (req.EndScheduleId > 0)
            {
                where = where.And(p => p.EndScheduleId == req.EndScheduleId);
            }
            if (req.TransferBehavior > 0)
            {
                where = where.And(p => p.TransferBehavior == req.TransferBehavior);
            }

            if (!string.IsNullOrEmpty(req.HkResponse))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.HkResponse) && p.HkResponse.ToLower().Contains(req.HkResponse.ToLower()));
            }

            if (req.IsManual != null)
            {
                where = where.And(p => p.IsManual == req.IsManual);
            }

            if (!string.IsNullOrEmpty(req.ClinkerRemark))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ClinkerRemark) && p.ClinkerRemark.ToLower().Contains(req.ClinkerRemark.ToLower()));
            }
            if (req.StartTime != null)
            {
                where = where.And(p => p.CreateTime >= req.StartTime);
            }
            if (req.EndTime != null)
            {
                where = where.And(p => p.CreateTime <= req.EndTime);
            }
            return where;
        }

        public async Task<List<TransferJobDto>> QueryAsync(Expression<Func<TransferJob, bool>> predicate, Expression<Func<TransferJob, object>> orderByExpression, OrderByType orderByType)
        {
            var jobs = await _domainService.QueryAsync(predicate, orderByExpression, orderByType);

            return _mapper.Map<List<TransferJobDto>>(jobs);
        }
    }
}