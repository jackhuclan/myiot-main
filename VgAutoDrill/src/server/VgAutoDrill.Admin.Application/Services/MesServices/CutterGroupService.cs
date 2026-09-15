using AutoMapper;
using Mapster;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npoi.Mapper;
using Org.BouncyCastle.Ocsp;
using SqlSugar;
using System.Text.RegularExpressions;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices.External;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroupDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class CutterGroupService : BaseServiceWithoutTree<CutterGroup, CutterGroupDto, AddOrUpdateCutterGroupReq>, ICutterGroupService
    {
        private readonly ITaskDomainService _taskDomainService;
        private readonly IAPIHelper _apiHelper;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IExternalWorkOrderDomainService _externalWorkOrderDomainService;
        private readonly ICutterCroupDetailDomainService _cutterGroupDetailDomainService;
        private readonly IDeviceDomainService _deviceDomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CutterGroupService> logger;
        private readonly IEncodeBuildRulesService _encodeBuildRulesService;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        /// <summary>
        /// 
        /// </summary>
        public CutterGroupService(ICutterGroupDomainService domainService,
            ITaskDomainService taskDomainService,
            IDeviceDomainService deviceDomainService,
            ISysConfigManager sysConfigManager,
            ILoggerFactory loggerFactory,
            IUnitOfWork unitOfWork,
            IAPIHelper apiHelper,
            ICentralOnlineDevice centralOnlineDevice,
            IExternalWorkOrderDomainService workOrderDomainService,
            ICutterCroupDetailDomainService cutterGroupDetailDomainService,
            IEncodeBuildRulesService encodeBuildRulesService,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _taskDomainService = taskDomainService;
            _deviceDomainService = deviceDomainService;
            _sysConfigManager = sysConfigManager;
            _unitOfWork = unitOfWork;
            _apiHelper = apiHelper;
            _centralOnlineDevice = centralOnlineDevice;
            logger = loggerFactory.CreateLogger<CutterGroupService>();
            _externalWorkOrderDomainService = workOrderDomainService;
            _cutterGroupDetailDomainService = cutterGroupDetailDomainService;
            _encodeBuildRulesService = encodeBuildRulesService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupNos"></param>
        /// <param name="cutterGroupStatus">配刀状态 0-待配刀 1-配刀锁定 </param>
        /// <returns></returns>
        public async Task<List<CutterGroupDto>> List(List<string> groupNos, CutterGroupStatusEnum cutterGroupStatus = 0)
        {
            var result = new List<CutterGroupDto>();
            if (groupNos?.Count <= 0)
            {
                return result;
            }
            var data = await _domainService.QueryAsync(s => s.CutterGroupStatus == cutterGroupStatus
                                                                && s.IsDeleted == 0 && groupNos!.Contains(s.GroupNo ?? ""), s => s.Id, OrderByType.Asc);
            if (data?.Count > 0)
            {
                result = data.Adapt<List<CutterGroupDto>>();
            }
            return result;
        }
        public async Task<List<CutterGroupDto>> List(List<string> groupNos)
        {
            var result = new List<CutterGroupDto>();
            if (groupNos?.Count <= 0)
            {
                return result;
            }
            var data = await _domainService.QueryAsync(s => s.IsDeleted == 0 && groupNos!.Contains(s.GroupNo ?? ""), s => s.Id, OrderByType.Asc);
            if (data?.Count > 0)
            {
                result = data.Adapt<List<CutterGroupDto>>();
            }
            return result;
        }


        public async Task<CutterGroupDto> QueryByGroupNo(string groupNo)
        {
            var result = new CutterGroupDto();
            if (string.IsNullOrWhiteSpace(groupNo))
            {
                return result;
            }
            var data = await _domainService.FindSingleAsync(s => s.IsDeleted == 0 && groupNo.ToLower() == s.GroupNo.ToLower());
            if (data != null)
            {
                result = data.Adapt<CutterGroupDto>();
            }
            return result;
        }
        /// <summary>
        /// 获取配刀组列表(分页)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<CutterGroupDto>>> GetList(GetListReq req)
        {
            if (req == null)
            {
                return Fail<PageDto<CutterGroupDto>>("信息格式错误!");
            }
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var where = PredicateBuilder.True<CutterGroup>();
            if (!string.IsNullOrWhiteSpace(req.CutterGroupNo))
            {
                where = where.And(p => req.CutterGroupNo.ToLower() == p.GroupNo.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(req.DrillNo))
            {
                where = where.And(p => req.DrillNo.ToLower() == p.DrillNo.ToLower());
            }
            if (req.DrillNos?.Count > 0)
            {
                where = where.And(p => req.DrillNos.Contains(p.DrillNo));
            }
            if (req.CutterGroupStatus != null)
            {
                where = where.And(p => req.CutterGroupStatus == p.CutterGroupStatus);
            }
            where = where.And(p => p.IsDeleted == 0);
            var result = new PageDto<CutterGroupDto>(req.PageNum, req.PageSize);
            var data = await _domainService.QueryPageAsync(where, s => s.Id, OrderByType.Desc, req.PageNum, req.PageSize);
            if (data?.Count > 0)
            {
                result.List = _mapper.Map<List<CutterGroupDto>>(data);
                result.Total = data.TotalCount;
                result.PageNum = data.PageIndex;
                result.PageSize = data.PageSize;
            }
            return Success(result);
        }



        public async Task<ResponseDto<List<CutterGroupDetailDto>>> GetDetail(List<string> cutterGroupNos)
        {
            var result = new List<CutterGroupDetailDto>();
            if (cutterGroupNos?.Count <= 0)
            {
                return Fail("配刀组计划No不能为空", result);
            }
            var data = await _cutterGroupDetailDomainService.QueryAsync(s => cutterGroupNos.Contains(s.CutterGroupNo) && s.IsDeleted == 0, s => s.Id, OrderByType.Asc);
            if (data?.Count > 0)
            {
                result = _mapper.Map<List<CutterGroupDetailDto>>(data);
            }
            return Success(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> UpdateCutterGroup(AddOrUpdateCutterGroupReq req)
        {
            if (req.Id <= 0)
            {
                return Fail<bool>("id不能为空");
            }
            var cutterGroup = await _domainService.QueryByID(req.Id);
            if (cutterGroup == null)
            {
                return Fail<bool>("查不到该配刀组计划");
            }
            cutterGroup.CutterGroupStatus = req.CutterGroupStatus;
            cutterGroup.ModifyTime = DateTime.Now;
            cutterGroup.ModifierId = UserId;
            await _domainService.Update(cutterGroup);
            return Success(true);
        }
        /// <summary>
        /// (批量)删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> Delete(List<long> ids)
        {
            if (ids?.Count <= 0)
            {
                return Fail<bool>("id集合不能为空");
            }
            var cutterGroups = await _domainService.QueryAsync(s => ids.Contains(s.Id), s => s.Id, OrderByType.Asc);
            if (cutterGroups?.Count <= 0)
            {
                return Fail<bool>("查不到所选配刀组计划信息");
            }
            var cutterGroupCodes = cutterGroups.Select(s => s.GroupNo).ToList();
            var lockedCutterGroups = cutterGroups.Where(s => s.CutterGroupStatus >= CutterGroupStatusEnum.Locked && s.IsDeleted == 0).ToList();
            if (lockedCutterGroups?.Count > 0)
            {
                return Fail<bool>($"配刀组计划{string.Join(",", lockedCutterGroups.Select(s => s.GroupNo).ToList())}已被锁定，不能删除");
            }
            await DeleteItemAndDetails(cutterGroupCodes!);
            return Success(true);
        }
        /// <summary>
        /// 根据任务code查询排刀计划明细
        /// </summary>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public async Task<CutterGroupDetailDto> DetailByTaskCode(string taskCode)
        {
            if (string.IsNullOrWhiteSpace(taskCode))
            {
                return new CutterGroupDetailDto();
            }
            return await _cutterGroupDetailDomainService.GetDetailByTaskCode(taskCode);
        }
        /// <summary>
        /// 根据任务code批量查询排刀计划明细
        /// </summary>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        public async Task<List<CutterGroupDetailDto>> DetailByTaskCode(List<string> taskCodes)
        {
            if (taskCodes?.Count <= 0)
            {
                return new List<CutterGroupDetailDto>();
            }
            return await _cutterGroupDetailDomainService.GetDetailByTaskCode(taskCodes);
        }
        /// <summary>
        /// 根据任务code查询是否被配刀组计划锁定
        /// </summary>
        /// <param name="taskCodes"></param>
        /// <returns></returns>
        public async Task<(bool IsLoked, List<string> LockedTaskCodes)> IsLockedByTaskCode(List<string> taskCodes)
        {
            var lockedTaskCodes = new List<string>();
            if (taskCodes?.Count > 0)
            {
                foreach (var item in taskCodes)
                {
                    var cutterGroupDetail = await DetailByTaskCode(item);
                    if (!string.IsNullOrWhiteSpace(cutterGroupDetail?.CutterGroupNo))
                    {
                        var cutterGroup = await _domainService.FindSingleAsync(s => s.GroupNo == cutterGroupDetail.CutterGroupNo
                                              && s.CutterGroupStatus >= CutterGroupStatusEnum.Locked);
                        if (cutterGroup != null)
                        {
                            lockedTaskCodes.Add(item);
                        }
                    }
                }
            }
            return (lockedTaskCodes.Count > 0, lockedTaskCodes);
        }

        /// <summary>
        /// 自动生成排刀计划
        /// </summary>
        /// <returns></returns>
        public async Task AutoGenerateCutterGroupData()
        {
            //检查开关
            var autoGenerateSwitch = await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_AUTO_CUTTER);
            if (!autoGenerateSwitch)
            {
                return;
            }
            var tasks = await _taskDomainService.QueryAsync(s => s.TaskStatus == TaskStatusEnum.COMMITED
                                                            && string.IsNullOrEmpty(s.CutterGroupNo), s => s.StartTime, OrderByType.Asc);
            if (tasks?.Count > 0)
            {
                //根据itemName和钻机WorkStationCode分组
                var groupTasks = tasks?.GroupBy(s => new { s.ItemName, s.WorkStationCode }).Select(m =>
                {
                    return m.OrderBy(t => t.StartTime).ToList();
                }).ToList();
                //查询任务上的所有工单
                var worderIds = tasks.Select(s => (long?)s.WorkOrderId).Distinct().ToList();
                var externalWorkOrders = await _externalWorkOrderDomainService.QueryAsync(s => worderIds.Contains(s.InnerOrderId), s => s.Id, OrderByType.Asc);
                //查询任务上的所有钻机
                var deviceCodes = tasks.Select(s => s.WorkStationCode).Distinct().ToList();
                var devices = await _deviceDomainService.QueryAsync(s => deviceCodes.Contains(s.Code), s => s.Id, OrderByType.Asc);
                //查询机型相关配置
                string machineConfig = await _sysConfigManager.GetStringValue(MESConfigConstants.MACHINE_TYPE_CONFIG);
                if (string.IsNullOrWhiteSpace(machineConfig))
                {
                    logger.LogInformation("自动生成配刀计划job:找不到MachineTypeConfig机型相关配置");
                    return;
                }

                var machineConfigs = !string.IsNullOrWhiteSpace(machineConfig) ?
                                JsonConvert.DeserializeObject<Dictionary<string, SysConfigOfMachineTypeConfig>>(machineConfig)
                                : new Dictionary<string, SysConfigOfMachineTypeConfig>();
                foreach (var groupTask in groupTasks)
                {
                    var externalWorkOrder = externalWorkOrders.FirstOrDefault(s => s.InnerOrderId == groupTask.FirstOrDefault()?.WorkOrderId);
                    if (externalWorkOrder == null)
                    {
                        logger.LogInformation($"自动生成配刀计划job:找不到任务{groupTask?.FirstOrDefault().Code}上的工单信息");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(externalWorkOrder?.CutterInfo))
                    {
                        logger.LogInformation($"自动生成配刀计划job:外部工单{externalWorkOrder?.Code}上的CutterInfo为空");
                        continue;
                    }
                    var cutterInfo = JsonConvert.DeserializeObject<Dictionary<string, int>>(externalWorkOrder.CutterInfo);
                    var device = devices.FirstOrDefault(s => s.Code == groupTask.FirstOrDefault()?.WorkStationCode);
                    var roundNum = cutterInfo.TryGetValue(device?.MachineSize ?? "", out int num) ? num : 0;
                    if (roundNum <= 0)
                    {
                        logger.LogInformation($"自动生成配刀计划job:外部工单{externalWorkOrder?.Code}上的CutterInfo信息匹配不到机型{device?.MachineSize}或者匹配的趟数为0");
                        continue;
                    }
                    //先根据物料、钻机、锁定状态重新生成并更新老数据的配刀组计划No，并向不满趟的配刀计划插入配刀计划明细
                    var task = await UpdateGenerateCutterGroupNo(groupTask);

                    //单轴刀盒数量 
                    var cutterBoxNum = machineConfigs.TryGetValue(device?.MachineSize, out SysConfigOfMachineTypeConfig MachineTypeConfig) ? MachineTypeConfig.CutterBoxNum : 0;
                    if (task.Count > 0)
                    {
                        task = task.OrderBy(s => s.StartTime).ToList();
                        for (var i = 0; i <= task.Count / roundNum; i++)//组内再根据趟数分组
                        {
                            var roundGroupTasks = task.Skip(i * roundNum).Take(roundNum).ToList();
                            await InsertCutterGroupAndDetail(roundGroupTasks, device, roundNum, cutterBoxNum);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成新的CutterGroupNo
        /// </summary>
        /// <returns></returns>
        private async Task<string> GenerateNewCutterGroupNo()
        {
            var newGroupNo = await _encodeBuildRulesService.GetEncodeList(new GetEncodeByRulesListReq()
            {
                BuildCount = 1,
                RulesCode = "CUTTER_GROUP_CODE"
            });
            return newGroupNo.Data.FirstOrDefault();
        }
        /// <summary>
        /// 插入排刀组计划信息
        /// </summary>
        /// <param name="roundGroupTasks"></param>
        /// <param name="device"></param>
        /// <param name="roundNum">趟数</param>
        /// <param name="cutterBoxNum">单轴刀盒数量</param>
        /// <returns></returns>
        private async Task InsertCutterGroupAndDetail(List<WorkTask> roundGroupTasks, Device device, int roundNum, int cutterBoxNum)
        {
            if (roundGroupTasks?.Count <= 0)
            {
                return;
            }
            var newGroupNo = await GenerateNewCutterGroupNo();
            var cutterGroup = new CutterGroup()
            {
                GroupNo = newGroupNo,
                ItemName = roundGroupTasks?.FirstOrDefault()?.ItemName,
                DrillName = roundGroupTasks?.FirstOrDefault()?.WorkStationName,
                DrillNo = roundGroupTasks?.FirstOrDefault()?.WorkStationCode,
                PlannedTime = roundGroupTasks?.FirstOrDefault()?.StartTime,//取一组里面最小的StartTime
                GroupDate = DateTime.Now,
                CutterGroupStatus = 0,
                MachineSize = device?.MachineSize,
                AxisCount = (device?.IsDuo ?? false) ? device?.SpindleNum * 2 : device?.SpindleNum,//duo机 要乘以2
                EndAxisCount = (device?.IsDuo ?? false) ? (int)roundGroupTasks?.LastOrDefault().NowWadCount * 2
                                                        : (int)roundGroupTasks?.LastOrDefault().NowWadCount,//duo机 要乘以2
                CutterBoxNum = cutterBoxNum,
                RoundNum = roundGroupTasks.Count,
                RoundNumStandard = roundNum,
                CreateTime = DateTime.Now,
                CreatorId = 1,
                ModifierId = 1,
            };
            var cutterGroupDetails = roundGroupTasks.Select((s, index) => new CutterGroupDetail()
            {
                CutterGroupNo = newGroupNo,
                ItemCode = s.ItemCode,
                TaskCode = s.Code,
                PanelNum = (int)s?.NowWadCount,
                IsFirstCutter = index == 0,//先前已经根据startTime排过序了，只需第一个置为true即可
                CreateTime = DateTime.Now,
                CreatorId = 1,
                ModifierId = 1
            }).ToList();
            roundGroupTasks = roundGroupTasks.Select(s =>
            {
                s.ModifyTime = DateTime.Now;
                s.CutterGroupNo = newGroupNo;
                return s;
            }).ToList();
            _unitOfWork.BeginTran();//加事务
            //插入CutterGroup表
            await _domainService.Add(cutterGroup);
            //插入CutterGroupDetail表
            await _cutterGroupDetailDomainService.BulkInsert(cutterGroupDetails);
            //更新t_task表
            await _taskDomainService.BulkUpdate(roundGroupTasks);
            _unitOfWork.CommitTran();
        }

        /// <summary>
        /// 重新生成排刀计划No
        /// </summary>
        /// <returns></returns>
        private async Task<List<WorkTask>> UpdateGenerateCutterGroupNo(List<WorkTask> task)
        {
            var itemName = task.FirstOrDefault()?.ItemName;
            var deviceCode = task.FirstOrDefault()?.WorkStationCode;
            var addCutterGroupDetails = new List<CutterGroupDetail>();
            var updateTasks = new List<WorkTask>();
            //根据物料和钻机，查询未锁定且不满趟的配刀计划
            var cutterGroups = await _domainService.QueryAsync(s => s.CutterGroupStatus == 0 && s.RoundNumStandard > s.RoundNum//不满趟
                                       && s.ItemName == itemName && s.DrillNo == deviceCode, s => s.Id, OrderByType.Asc);
            //查询明细
            var cutterGroupNos = cutterGroups.Select(s => s.GroupNo).Distinct().ToList();
            var cutterGroupDetails = await _cutterGroupDetailDomainService.QueryAsync(s => cutterGroupNos.Contains(s.CutterGroupNo), s => s.Id, OrderByType.Asc);
            //根据明细表记录的taskCode查询原始任务信息，用于startTime排序确定首刀标识
            var oldTaskCodes = cutterGroupDetails.Select(s => s.TaskCode).Distinct().ToList();
            var oldTasks = await _taskDomainService.QueryAsync(m => oldTaskCodes.Contains(m.Code), p => p.StartTime, OrderByType.Asc);
            if (cutterGroups?.Count() > 0)
            {
                foreach (var item in cutterGroups)
                {
                    var details = cutterGroupDetails.Where(s => s.CutterGroupNo == item.GroupNo).ToList();
                    var addNum = (item.RoundNumStandard - item.RoundNum > 0 ? item.RoundNumStandard - item.RoundNum : 0) ?? 0;
                    var addTask = task.Take(addNum).OrderBy(s => s.StartTime).ToList();
                    //比较新插进来的数据的startTime，如果新插进来的startTime比原来的最早时间更早，则需要对首刀标识重新计算
                    var isNeedFreshFirstCutter = addTask.FirstOrDefault()?.StartTime < oldTasks.FirstOrDefault()?.StartTime;
                    var newGroupNo = await GenerateNewCutterGroupNo();
                    item.GroupNo = newGroupNo;
                    item.RoundNum = item.RoundNum + addTask.Count > item.RoundNumStandard ?
                                                          item.RoundNumStandard : item.RoundNum + addTask.Count;
                    item.GroupDate = DateTime.Now;
                    item.ModifyTime = DateTime.Now;
                    if (details?.Count() > 0)
                    {
                        foreach (var detail in details)
                        {
                            detail.IsFirstCutter = isNeedFreshFirstCutter ? false : detail.IsFirstCutter;
                            detail.CutterGroupNo = newGroupNo;
                            detail.ModifyTime = DateTime.Now;
                        }
                    }
                    //补齐不满趟的配刀计划明细数据，直至满趟为止
                    if (addTask?.Count > 0)
                    {
                        //更新任务表上配刀标识
                        addTask = addTask.Select(s =>
                        {
                            s.ModifyTime = DateTime.Now;
                            s.CutterGroupNo = newGroupNo;
                            return s;
                        }).ToList();
                        updateTasks.AddRange(addTask);
                        var addDetail = addTask.Select((s, index) => new CutterGroupDetail()
                        {
                            CutterGroupNo = newGroupNo,
                            ItemCode = s.ItemCode,
                            TaskCode = s.Code,
                            PanelNum = (int)s?.NowWadCount,
                            IsFirstCutter = isNeedFreshFirstCutter && index == 0,
                            CreateTime = DateTime.Now,
                            CreatorId = 1,
                            ModifierId = 1
                        }).ToList();
                        addCutterGroupDetails.AddRange(addDetail);
                        task.RemoveAll(s => addTask.Any(m => m.Id == s.Id));
                    }
                }
                _unitOfWork.BeginTran();//加事务
                await _domainService.BulkUpdate(cutterGroups);
                await _cutterGroupDetailDomainService.BulkUpdate(cutterGroupDetails);
                await _cutterGroupDetailDomainService.BulkInsert(addCutterGroupDetails);
                await _taskDomainService.BulkUpdate(updateTasks);
                _unitOfWork.CommitTran();
            }
            return task;

        }


        /// <summary>
        /// 根据组code批量删除配刀组计划
        /// </summary>
        /// <param name="groupNos"></param>
        /// <returns></returns>
        public async Task DeleteItemAndDetails(List<string> groupNos)
        {
            if (groupNos?.Count <= 0)
            {
                return;
            }
            var cutterGroups = await _domainService.QueryAsync(s => s.IsDeleted == 0 &&
                           groupNos.Contains(s.GroupNo), s => s.Id, OrderByType.Asc);
            var cutterGroupDetails = await _cutterGroupDetailDomainService.QueryAsync(s => s.IsDeleted == 0 &&
                           groupNos.Contains(s.CutterGroupNo), s => s.Id, OrderByType.Asc);
            var taskCodes = cutterGroupDetails.Select(s => s.TaskCode).ToList();
            var tasks = await _taskDomainService.QueryAsync(s => taskCodes.Contains(s.Code) && s.IsDeleted == 0, s => s.Id, OrderByType.Asc);
            if (tasks?.Count > 0)
            {
                foreach (var task in tasks)
                {
                    task.CutterGroupNo = "";//重置任务上的配刀标识
                    task.ModifyTime = DateTime.Now;
                }
            }
            logger.LogInformation($"要删除的配刀组计划No：{JsonConvert.SerializeObject(groupNos)}");
            _unitOfWork.BeginTran();//加事务
            await _cutterGroupDetailDomainService.BulkDelete(cutterGroupDetails);
            await _domainService.BulkDelete(cutterGroups);
            await _taskDomainService.BulkUpdate(tasks);
            _unitOfWork.CommitTran();
        }

        /// <summary>
        /// 获取配刀组计划atp文件路径
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<CutterGroupAptFileRes>> GetCutterGroupAtpFile(GetCutterGroupAtpFileReq req)
        {
            var result = new CutterGroupAptFileRes() { IsNeedLoad = true };
            if (string.IsNullOrWhiteSpace(req.GroupNo) && string.IsNullOrWhiteSpace(req.DeviceCode))
            {
                return Fail<CutterGroupAptFileRes>("钻机编码不能为空");
            }
            var url = await _sysConfigManager.GetStringValue(MESConfigConstants.CUTTER_GROUP_APT_FILE_URL);
            if (string.IsNullOrWhiteSpace(url))
            {
                return Fail<CutterGroupAptFileRes>("找不到CutterGroupAptFileUrl相关配置");
            }
            if (string.IsNullOrWhiteSpace(req.GroupNo))
            {
                /*
                 * 对于自动钻机来说，钻机会自动获取atp文件并加载，钻机触发时，不知道配刀组计划No,
                 * 只能传钻机Code,所以只能先去任务上获取配刀组计划   
                */
                var currentTask = await _taskDomainService.GetTaskByDevice(new GetDrillOrAgvDeviceInfoReq()
                {
                    PageNum = 1,
                    PageSize = int.MaxValue,
                    DeviceCode = req.DeviceCode,
                    TaskStatusList = [TaskStatusEnum.COMMITED, TaskStatusEnum.BUFFERED, TaskStatusEnum.SENDING]
                });
                if (currentTask != null && currentTask?.List.Count > 0)
                {
                    req.GroupNo = currentTask.List.FirstOrDefault()?.CutterGroupNo;
                    logger.LogInformation($"获取当前钻机最新得任务:{JsonConvert.SerializeObject(currentTask.List.FirstOrDefault())}");
                }
            }
            logger.LogInformation($"获取配刀组计划apt文件路径请求参数:{JsonConvert.SerializeObject(req)}");
            var cutterGroup = await _domainService.FindSingleAsync(s => s.GroupNo.ToLower() == req.GroupNo.ToLower());
            if (cutterGroup == null)
            {
                return Fail<CutterGroupAptFileRes>("查不到该配刀计划");
            }

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.NEED_CHECK_BOXCODES))
            {
                if (cutterGroup.CheckBox != CutterBoxStatusEnum.OK)
                {
                    return Fail<CutterGroupAptFileRes>("请确认刀盒码是否已校验通过!");
                }
            }

            if (cutterGroup.CutterGroupStatus == CutterGroupStatusEnum.LoadCutterAtpFileComplete)//已经加载过的，直接返回
            {
                result.IsNeedLoad = false;
                result.GroupNo = req.GroupNo;
                return Success(result);
            }
            var request = new KwDrillKnifeToolInfoReq()
            {
                EquipmentCode = cutterGroup.DrillNo,
                PlanNo = cutterGroup.GroupNo,
            };
            var requestString = JsonConvert.SerializeObject(request);
            var data = _apiHelper.RequestData<KwDrillKnifeToolInfoRes>(url, "Post", requestString);
            if (data == null || data?.Code != "0")
            {
                return Fail<CutterGroupAptFileRes>($"{data?.Message},调用第三方接口获取apt文件路径请求失败");
            }
            result.AtpFile = data.ReturnValue.Atp;
            result.Boxs = data.ReturnValue.BoxNos;
            result.GroupNo = req.GroupNo;
            result.IsNeedLoad = cutterGroup.CutterGroupStatus != CutterGroupStatusEnum.LoadCutterAtpFileComplete && cutterGroup.CutterGroupStatus >= CutterGroupStatusEnum.Locked;

            cutterGroup.AtpFilePath = data.ReturnValue.Atp;
            cutterGroup.BoxNos = data.ReturnValue.BoxNos;
            cutterGroup.CutterGroupStatus = CutterGroupStatusEnum.LoadCutterAtpFileComplete;//先把状态更新为已加载ATP，如果钻机加载失败，由钻机回退到已锁定状态
            cutterGroup.ModifyTime = DateTime.Now;
            await _domainService.Update(cutterGroup);//请求到的apt文件路径和刀盒集合保存到配刀组计划表里
            return Success(result);
        }

        /// <summary>
        /// 更新配刀组计划状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> UpdateCutterGroupStatus(UpdateCutterGroupStatusReq req)
        {
            var cutterGroup = await _domainService.FindSingleAsync(s => s.GroupNo.ToLower() == req.GroupNo.ToLower());
            if (cutterGroup == null)
            {
                return Fail<bool>("查不到该配刀计划");
            }
            cutterGroup.CutterGroupStatus = req.GroupStatus;
            cutterGroup.ModifyTime = DateTime.Now;
            await _domainService.Update(cutterGroup);
            return Success(true);
        }


    }
}
