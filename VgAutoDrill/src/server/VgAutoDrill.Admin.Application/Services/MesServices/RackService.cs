using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SqlSugar;
using SqlSugar.Extensions;
using System.Text;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using ThrowHelper = VgAutoDrill.Admin.Application.Helper.ThrowHelper;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class RackService : BaseServiceWithoutTree<Rack, RackDto, AddOrUpdateRackReq>, IRackService
    {
        private readonly IRackDomainService _rackDomainService;
        private readonly ISiloDetailDomainService _siloDetailDomainService;
        private readonly ISiloDomainService _siloDomainService;
        private readonly ILocationDetailDomainService _locationDetailDomainService;
        private readonly SqlSugarScope _sqlSugarScope;
        private readonly IAPIHelper _apiHelper;
        private readonly IScheduleService _scheduleService;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly ISiloService _siloService;
        private readonly ISiloPanelTraceService _siloPanelTraceService;
        private readonly IOptions<InnerOptions> _innerOptions;
        private readonly ILogger<RackService> _logger;

        public RackService(IRackDomainService rackDomain,
            IMapper mapper,
            ISiloDetailDomainService siloDetailDomainService,
            ISiloDomainService siloDomainService,
            ILocationDetailDomainService locationDetailDomainService,
            IScheduleService scheduleService,
            IAPIHelper apiHelper,
            ICentralOnlineDevice centralOnlineDevice,
            ISysConfigManager sysConfigManager,
            ISiloService siloService,
            ISiloPanelTraceService siloPanelTraceService,
            IOptions<InnerOptions> innerOptions,
            ILogger<RackService> logger,
            IUnitOfWork unitWork) : base(rackDomain, mapper)
        {
            _rackDomainService = rackDomain;
            _siloDetailDomainService = siloDetailDomainService;
            _siloDomainService = siloDomainService;
            _locationDetailDomainService = locationDetailDomainService;
            _sqlSugarScope = unitWork.GetDbClient();
            _scheduleService = scheduleService;
            _apiHelper = apiHelper;
            _centralOnlineDevice = centralOnlineDevice;
            _sysConfigManager = sysConfigManager;
            _siloService = siloService;
            _siloPanelTraceService = siloPanelTraceService;
            _innerOptions = innerOptions;
            _logger = logger;
        }
        public async Task<RackDto> FindSingle(string code)
        {
            var entity = await _domainService.FindSingleAsync(x => x.Code.ToLower() == code.ToLower());
            return _mapper.Map<RackDto>(entity);
        }
        public async Task<ResponseDto<PageDto<RackDto>>> GetList(RackQueryReq req)
        {
            if (req == null) ThrowHelper.ThrowArgumentNullException(nameof(req));

            req.PageNum = req.PageNum < 1 ? 1 : req.PageNum;
            req.PageSize = req.PageSize < 1 ? 10 : req.PageSize;
            var pageDto = new PageDto<RackDto>(req.PageNum, req.PageSize);

            var query = _sqlSugarScope.Queryable<Rack, Partition>
                ((r, w) => new object[]
                    {
                        JoinType.Left, r.WareHouseId == w.Id
                    });

            query = query.Where((r, w) => r.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((r, w) => r.Code.ToLower().Contains(req.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                query = query.Where((r, w) => !string.IsNullOrEmpty(r.SiloCode) && r.SiloCode.ToLower().Contains(req.SiloCode.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.PositionCode))
            {
                query = query.Where((r, w) => !string.IsNullOrEmpty(r.PositionCode) && r.PositionCode.ToLower().Contains(req.PositionCode.ToLower()));
            }

            if (req.DeviceKinds != null && req.DeviceKinds.Any())
            {
                query = query.Where((r, w) => r.DeviceKind.HasValue && req.DeviceKinds.Contains((DeviceKind)r.DeviceKind));
            }

            if (req.WareHouseId > 0)
            {
                var sql = $"select id from t_warehouse where find_in_set({req.WareHouseId},ancestors)";
                var wareHouseIdList = _sqlSugarScope.SqlQueryable<Partition>(sql).Select(d => d.Id).ToList();

                query = query.Where((r, w) => r.WareHouseId == req.WareHouseId
                            || (r.WareHouseId.HasValue && wareHouseIdList.Contains(r.WareHouseId.Value)));
            }
            if (!string.IsNullOrEmpty(req.WareHouseCode))
            {
                query = query.Where((r, w) => !string.IsNullOrEmpty(r.WareHouseCode) && r.WareHouseCode.ToLower().Equals(req.WareHouseCode.ToLower()));
            }

            query = query.OrderBy((r, w) => r.Code);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((r, w) => r).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            pageDto.Total = totalCount;
            pageDto.List = _mapper.Map<List<RackDto>>(data.ToList());

            return Success(pageDto);
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<RackFullDataByPartition>>> GetFullDatasByPartition(RackFullQueryReq req)
        {
            if (req == null) ThrowHelper.ThrowArgumentNullException(nameof(req));

            List<RackFullDataByPartition> returnDtos = new List<RackFullDataByPartition>();

            var fullDatas = await GetRackFullDatas(req);
            var categorys = fullDatas.GroupBy(p => p.WareHouseCode).ToList();
            foreach (var item in categorys)
            {
                RackFullDataByPartition model = new RackFullDataByPartition
                {
                    WareHouseCode = item.Key,
                    Racks = item.ToList(),
                };

                returnDtos.Add(model);
            }

            return Success(returnDtos);
        }

        public async Task<ResponseDto<List<RackFullData>>> GetFullDatas(RackFullQueryReq req)
        {
            if (req == null) ThrowHelper.ThrowArgumentNullException(nameof(req));
            var fullDatas = await GetRackFullDatas(req);

            return Success(fullDatas);
        }

        /// <summary>
        /// 验证查询参数的合理性
        /// </summary>
        /// <param name="req">查询请求</param>
        private void ValidateQueryParameters(RackFullQueryReq req)
        {
            if (req == null) return;

            // 检查是否有明显的错误值
            var invalidValues = new[] { "string", "null", "undefined", "" };

            if (!string.IsNullOrEmpty(req.Code) && invalidValues.Contains(req.Code))
            {
                req.Code = null;
            }

            if (!string.IsNullOrEmpty(req.WareHouseCode) && invalidValues.Contains(req.WareHouseCode))
            {
                req.WareHouseCode = null;
            }

            if (!string.IsNullOrEmpty(req.SiloCode) && invalidValues.Contains(req.SiloCode))
            {
                req.SiloCode = null;
            }

            if (!string.IsNullOrEmpty(req.ItemCode) && invalidValues.Contains(req.ItemCode))
            {
                req.ItemCode = null;
            }

            if (req.RouteCodes != null && req.RouteCodes.Any(r => invalidValues.Contains(r)))
            {
                req.RouteCodes = null;
            }
        }

        private async Task<List<RackFullData>> GetRackFullDatas(RackFullQueryReq req)
        {
            // 验证并清理参数
            ValidateQueryParameters(req);

            var centralDatas = await RefreshDataByCentralRacks(req);

            // 构建完整查询
            var query = _sqlSugarScope.Queryable<Rack, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route>
                ((rack, w, rpw, rp, r) => new object[]
                    {
                        JoinType.Left, rack.RelateDeviceCode == w.Code,
                        JoinType.Left, w.Id == rpw.WorkStationId,
                        JoinType.Left, rpw.RouteAndProcessId == rp.Id,
                        JoinType.Left, rp.RouteId== r.Id,
                    });

            query = query.Where((rack, w, rpw, rp, r) => rack.IsDeleted == 0);

            var joinCount = await query.CountAsync();
            if (joinCount == 0)
            {
                return new List<RackFullData>();
            }

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((rack, w, rpw, rp, r) => rack.Code.ToLower().Contains(req.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.WareHouseCode))
            {
                query = query.Where((rack, w, rpw, rp, r) => rack.WareHouseCode.ToLower().Contains(req.WareHouseCode.ToLower()));
            }

            if (req.RouteCodes != null && req.RouteCodes.Count > 0)
            {
                query = query.Where((rack, w, rpw, rp, r) => req.RouteCodes.Contains(r.Code));
            }

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                query = query.Where((rack, w, rpw, rp, r) => !string.IsNullOrEmpty(rack.SiloCode) && rack.SiloCode.ToLower().Contains(req.SiloCode.ToLower()));
            }

            if (req.DeviceKinds != null && req.DeviceKinds.Any())
            {
                query = query.Where((rack, w, rpw, rp, r) => rack.DeviceKind.HasValue && req.DeviceKinds.Contains((DeviceKind)rack.DeviceKind));
            }

            query = query.OrderBy((rack, w, rpw, rp, r) => rack.Code);

            var rackInfo = await query.Select((rack, w, rpw, rp, r) => new RackFullData
            {
                RouteCode = r.Code,
                Code = rack.Code,
                PositionCode = rack.PositionCode,
                RelateDeviceCode = rack.RelateDeviceCode,
                WareHouseCode = rack.WareHouseCode,
                WareHouseId = rack.WareHouseId,
                Id = rack.Id,
                IsHaveSilo = rack.IsHaveSilo,
                SiloCode = rack.SiloCode,
                //InnerPoint = rack.InnerPoint,
                //OutPoint = rack.OutPoint,
                Status = rack.Status,
                //TransInnerPoint = rack.TransInnerPoint,
                //TransOutPoint = rack.TransOutPoint,
                DeviceKind = rack.DeviceKind,
                CreateTime = rack.CreateTime,
                CreatorId = rack.CreatorId,
                ModifierId = rack.ModifierId,
                ModifyTime = rack.ModifyTime,
            }).ToListAsync();

            if (rackInfo == null || rackInfo.Count == 0)
            {
                return new List<RackFullData>();
            }

            if (centralDatas.Data == null || centralDatas.Data.Count == 0)
            {
                // 如果没有中控数据，为所有料架设置默认值
                foreach (var item in rackInfo)
                {
                    item.Appointed = false;
                    item.IsReady = false;
                    item.HasAlarm = false;
                    item.AlarmMessage = null;
                    item.IsAvailableForAgv = false;
                }
                return rackInfo;
            }

            foreach (var item in rackInfo)
            {
                var centralRack = centralDatas.Data.SingleOrDefault(p => !string.IsNullOrEmpty(item.Code)
                && p.Code.ToLower() == item.Code.ToLower());
                if (centralRack == null || centralRack.Panels == null || centralRack.Panels.Count == 0)
                {
                    // 如果没有找到对应的中控数据，设置默认值
                    item.Appointed = false;
                    item.AppointedMessage = null;
                    item.IsReady = false;
                    item.HasAlarm = false;
                    item.AlarmMessage = null;
                    item.IsAvailableForAgv = false;
                    continue;
                }

                // 从中控系统直接获取字段值，如果没有则设置默认值
                item.Appointed = centralRack.Appointed;
                item.AppointedMessage = centralRack.AppointedMessage;
                item.IsReady = centralRack.IsReady ?? false;
                item.HasAlarm = centralRack.HasAlarm ?? false;
                item.AlarmMessage = centralRack.AlarmMessage;
                item.IsAvailableForAgv = centralRack.IsAvailableForAgv ?? false;

                // 处理料仓载料信息
                if (centralRack.Panels != null && centralRack.Panels.Count > 0)
                {
                    var panels = centralRack.Panels
                          .Where(p => p != null && p.SiloCode == item.SiloCode && p.ProductStatus != ProductStatus.EmptyPayload);

                    // 根据物料编码过滤
                    if (!string.IsNullOrEmpty(req.ItemCode))
                    {
                        panels = panels.Where(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.ToLower().Contains(req.ItemCode.ToLower()));
                    }

                    item.SiloInfos.AddRange(panels
                          .GroupBy(p => new { SimpleProductStatus = ProductStatusConstants.SimplifyProductStatus(p.ProductStatus), p.ItemCode, p.SiloCode })
                          .Select(p => new SiloItemSumInfo
                          {
                              ItemCode = p.Key.ItemCode,
                              SimpleProductStatus = p.Key.SimpleProductStatus,
                              ProductStatus = ProductStatusExtensions.SimplifyProductStatusName(p.Key.SimpleProductStatus),
                              SiloCount = p.Count(),
                              SiloCode = p.Key.SiloCode
                          }).ToList());
                }
            }

            return rackInfo;
        }

        private async Task<ResponseDto<List<CentralRackDto>>> RefreshDataByCentralRacks(RackFullQueryReq req)
        {
            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.CENTRAL_GET_SIMPLE_LOCATIONS);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return Fail<List<CentralRackDto>>("CentralGetSimpleLocations！");
            }

            List<CentralRackDto> response = new List<CentralRackDto>();

            try
            {
                var deviceKind = DeviceKind.PanelSiloFork;
                if (req.DeviceKinds.Any())
                {
                    deviceKind = req.DeviceKinds.FirstOrDefault();
                }

                var result = _apiHelper.RequestData($"{urlAdress}{(int)deviceKind}", "Get");
                if (string.IsNullOrEmpty(result))
                {
                    return Fail<List<CentralRackDto>>($"查询数据失败！{urlAdress}");
                }

                response = System.Text.Json.JsonSerializer.Deserialize<List<CentralRackDto>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response == null || response.Count == 0)
                {
                    return Fail<List<CentralRackDto>>("无法解析数据或未查询到数据！");
                }

                var locationCodes = response.Where(p => !string.IsNullOrEmpty(p.Code)).Select(p => p.Code.ToLower()).ToList();
                if (locationCodes != null && locationCodes.Count != 0)
                {
                    var racks = await _domainService.QueryAsync(p => locationCodes.Contains(p.Code.ToLower()), p => p.Code, OrderByType.Asc);
                    if (racks == null || racks.Count == 0)
                    {
                        return Success(response);
                    }

                    var siloCodes = response.Where(p => !string.IsNullOrEmpty(p.SiloCode)).Select(p => p.SiloCode.ToLower()).ToList();
                    if (siloCodes != null && siloCodes.Count != 0)
                    {
                        await _domainService.UpdateAsync(p => new Rack
                        {
                            SiloCode = ""
                        }, p => !string.IsNullOrEmpty(p.SiloCode) && siloCodes.Contains(p.SiloCode.ToLower()));
                    }

                    foreach (var rack in racks)
                    {
                        var centralRack = response.SingleOrDefault(p => p.Code.ToLower() == rack.Code.ToLower());
                        if (centralRack == null)
                        {
                            continue;
                        }
                        rack.RelateDeviceCode = centralRack.DeviceId;
                        rack.SiloCode = centralRack.SiloCode;
                        rack.ModifierId = UserId;
                        rack.ModifyTime = DateTime.Now;
                    }

                    await _domainService.BulkUpdate(racks);
                }
            }
            catch (Exception ex)
            {
                return Fail<List<CentralRackDto>>($"查询接口异常！{ex.Message}");
            }

            return Success(response);
        }

        public async Task<ResponseDto<List<ExternalRackDto>>> GetExternalRackInfo(ExternalRackQueryReq req)
        {
            if (req == null) ThrowHelper.ThrowArgumentNullException(nameof(req));

            var query = _sqlSugarScope.Queryable<Rack>().Where((r) => r.IsDeleted == 0);

            if (req.Status != null)
            {
                query = query.Where((r) => r.Status == req.Status);
            }

            if (!string.IsNullOrEmpty(req.Code))
            {
                query = query.Where((r) => r.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.WareHouseCode))
            {
                query = query.Where((r) => r.WareHouseCode.Contains(req.WareHouseCode));
            }
            if ((await query.CountAsync() == 0))
            {
                return new ResponseDto<List<ExternalRackDto>> { };
            }

            var rackNos = query.ToList();

            List<ExternalRackDto> externalRackPanel = new List<ExternalRackDto>();

            foreach (var rack in rackNos)
            {
                var externalRackInfo = new ExternalRackDto
                {
                    Code = rack.Code,
                    //InnerPoint = rack.InnerPoint,
                    //OutPoint = rack.OutPoint,
                    WareHouseCode = rack.WareHouseCode,
                };

                if (!string.IsNullOrEmpty(rack.SiloCode) && (await _siloDomainService.IsExistAsync(x => x.Code.ToLower() == rack.SiloCode.ToLower())))
                {
                    var silo = await _siloDomainService.FindSingleAsync(x => x.Code.ToLower() == rack.SiloCode.ToLower());
                    var details = await _siloDetailDomainService.QueryAsync(p => p.SiloCode.ToLower() == rack.SiloCode, s => s.FloorNum, OrderByType.Asc);

                    var siloInfo = new SiloInfo
                    {
                        SiloCode = silo.Code,
                        Location = silo.Location,
                        EmptySilo = silo.EmptySilo,
                        Size = silo.Size,
                        FloorCount = silo.FloorCount
                    };

                    details.ForEach(t => siloInfo.SiloDetails.Add(
                        new ExternalSiloDetailDto()
                        {
                            ItemCode = t.ItemCode,
                            FloorNum = t.FloorNum + 1,
                            PanelCode = t.PanelCode,
                            PanelWidth = t.PanelWidth,
                            Pcs = t.Pcs?.ToString(),
                            PinOffset = t.PinOffset,
                            ProductStatus = t.ProductStatus,
                        }));

                    externalRackInfo.ExternalSiloInfo.Add(siloInfo);
                }

                externalRackPanel.Add(externalRackInfo);
            }

            return Success(externalRackPanel);
        }

        public async Task<ResponseDto<string>> UpdateStatus(ExternalAddOrUpdateRackReq req)
        {
            await _sqlSugarScope.Updateable<Rack>()
                   .SetColumns(p => new Rack() { Status = req.Status.Value, ModifyTime = DateTime.Now })
                   .Where(p => p.Code == req.Code)
                   .ExecuteCommandAsync();

            return Success();
        }

        public async Task<List<RackDto>> GetPanelForksWithoutTodoSchedule()
        {
            var result = new List<RackDto>();
            var racks = await _rackDomainService.QueryAsync(x => x.Status == 1 && x.DeviceKind == DeviceKind.PanelSiloFork, x => x.Id, OrderByType.Asc);

            var todoForkSchedules = await _scheduleService.GetScheduleTasks(new Model.ViewModels.Mes.Schedulement.GetScheduleListReq
            {
                RequestDeviceKindList = new List<DeviceKind?> { DeviceKind.PanelSiloFork },
                ScheduledTaskStatusList = new List<ScheduledTaskStatus?>
                {
                    ScheduledTaskStatus.Created,
                    ScheduledTaskStatus.Allocated,
                    ScheduledTaskStatus.Running
                },
                PageSize = int.MaxValue,
            });

            if (todoForkSchedules == null || todoForkSchedules.Data == null || todoForkSchedules.Data.Count == 0)
            {
                return _mapper.Map<List<RackDto>>(racks);
            }

            var forkCodes = todoForkSchedules.Data.Select(f => f.SubDeviceCode).Distinct().ToList();
            var filteredRacks = racks.Where(r => !forkCodes.Contains(r.Code)).ToList();
            return _mapper.Map<List<RackDto>>(filteredRacks);
        }

        public async Task<ResponseDto<string>> DeleteExternalRackInfo(string code)
        {
            var rackInfo = (await _rackDomainService.QueryAsync(p => p.Code.Equals(code), p => p.Code, OrderByType.Asc))?.FirstOrDefault();
            if (rackInfo == null) { return Fail("未查到料架信息!"); }

            await _rackDomainService.DeleteById(rackInfo.Id);

            return Success();
        }
        public override async Task<ResponseDto<string>> Add(AddOrUpdateRackReq req)
        {
            var rackInfo = await _rackDomainService.FindSingleAsync(p => p.Code.Trim().ToLower() == req.Code.Trim().ToLower());
            if (rackInfo != null) return Fail("料架已存在,不可再次新增!");

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                //bool existRackInfo = await _rackDomainService.IsExistAsync(p => p.SiloCode == req.SiloCode);
                //if (existRackInfo)
                //{
                //    return Fail($"该料仓 {req.SiloCode} 已绑定其他料架!");
                //}
            }

            var model = _mapper.Map<Rack>(req);
            model.IsHaveSilo = !string.IsNullOrEmpty(req.SiloCode);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            var result = await _rackDomainService.Add(model);
            //if (result && !string.IsNullOrEmpty(req.SiloCode))
            //{
            //    var siloData = await _siloDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code)
            //    && p.Code.ToLower() == req.SiloCode.ToLower());
            //    if (siloData != null)
            //    {
            //        siloData.Location = req.Code;
            //        if (req.DeviceKind != null)
            //        {
            //            siloData.RelatedDeviceKind = req.DeviceKind;
            //        }
            //        else
            //        {
            //            siloData.RelatedDeviceKind = DeviceKind.PublicPanelSiloWIP;
            //        }
            //        siloData.ModifierId = UserId;
            //        siloData.ModifyTime = DateTime.Now;
            //        await _siloDomainService.Update(siloData);
            //    }
            //}
            return Success();
        }

        public async Task<ResponseDto<string>> AddBatch(List<AddOrUpdateRackReq> reqs)
        {
            if (reqs == null) ThrowHelper.ThrowArgumentNullException(nameof(reqs));

            if (reqs.Select(p => p.Code).Distinct().ToList().Count != reqs.Count) return Fail("参数中有重复Code!");

            List<Rack> racks = new List<Rack>();
            //List<Silo> siloUpdates = new List<Silo>();
            foreach (var req in reqs)
            {
                var rackInfo = await _rackDomainService.FindSingleAsync(p => p.Code.Trim().ToLower() == req.Code.Trim().ToLower());
                if (rackInfo != null) return Fail("料架已存在,不可再次新增!");

                //if (!string.IsNullOrEmpty(req.SiloCode))
                //{
                //bool existRackInfo = await _rackDomainService.IsExistAsync(p => p.SiloCode == req.SiloCode);
                //if (existRackInfo)
                //{
                //    return Fail($"该料仓 {req.SiloCode} 已绑定其他料架!");
                //}

                //if (racks.Exists(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.ToLower() == req.SiloCode.ToLower()))
                //{
                //    return Fail($"该料仓 {req.SiloCode} 重复!");
                //}

                //var siloData = await _siloDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code)
                //&& p.Code.ToLower() == req.SiloCode.ToLower());
                //if (siloData != null)
                //{
                //    siloData.Location = req.Code;
                //    if (req.DeviceKind != null)
                //    {
                //        siloData.RelatedDeviceKind = req.DeviceKind;
                //    }
                //    else
                //    {
                //        siloData.RelatedDeviceKind = DeviceKind.PublicPanelSiloWIP;
                //    }
                //    siloData.ModifierId = UserId;
                //    siloData.ModifyTime = DateTime.Now;
                //    siloUpdates.Add(siloData);
                //}
                //}

                var model = _mapper.Map<Rack>(req);
                model.IsHaveSilo = !string.IsNullOrEmpty(req.SiloCode);
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                racks.Add(model);
            }

            var result = await _rackDomainService.BulkInsert(racks);
            //if (result)
            //{
            //    await _siloDomainService.BulkUpdate(siloUpdates);
            //}

            return Success();
        }

        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateRackReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的数据！");
            }
            if (string.IsNullOrEmpty(req.Code))
            {
                return Fail("库位编码不能为空！");
            }
            // 根据库位编码查找料架记录
            var data = await _rackDomainService.FindSingleAsync(p => p.Code.Trim().ToLower() == req.Code.Trim().ToLower());
            if (data == null)
            {
                return Fail($"未找到编码为 {req.Code} 的库位！");
            }

            // 检查料仓是否存在
            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                if (!await _siloDomainService.IsExistAsync(p => p.Code.ToLower() == req.SiloCode.ToLower()))
                {
                    return Fail($"料仓 {req.SiloCode} 不存在！");
                }
            }
            data.SiloCode = req.SiloCode.Trim();
            data.IsHaveSilo = !string.IsNullOrEmpty(req.SiloCode);
            if (!string.IsNullOrEmpty(req.FeedAGVInnerPoint))
            {
                data.FeedAGVInnerPoint = req.FeedAGVInnerPoint.Trim();
            }
            if (!string.IsNullOrEmpty(req.FeedAGVOutputPoint))
            {
                data.FeedAGVOutputPoint = req.FeedAGVOutputPoint.Trim();
            }
            if (!string.IsNullOrEmpty(req.FeedAGVRestPoint))
            {
                data.FeedAGVRestPoint = req.FeedAGVRestPoint.Trim();
            }
            if (!string.IsNullOrEmpty(req.TransAGVInnerPoint))
            {
                data.TransAGVInnerPoint = req.TransAGVInnerPoint.Trim();
            }
            if (!string.IsNullOrEmpty(req.TransAGVOutputPoint))
            {
                data.TransAGVOutputPoint = req.TransAGVOutputPoint.Trim();
            }
            if (!string.IsNullOrEmpty(req.TransAGVRestPoint))
            {
                data.TransAGVRestPoint = req.TransAGVRestPoint.Trim();
            }
            data.ModifierId = UserId;
            data.ModifyTime = DateTime.Now;
            var result = await _rackDomainService.Update(data);
            if (!result)
            {
                return Fail("更新失败！");
            }

            //更换料仓时，仅当原来没有明细时，才初始化库位明细信息
            var existDetail = await _locationDetailDomainService.IsExistAsync(p => p.Code.ToLower() == req.Code.ToLower());
            if (!existDetail)
            {
                var siloData = await _siloDomainService.FindSingleAsync(p => p.Code.ToLower() == req.SiloCode.ToLower());
                if (siloData != null && siloData.FloorCount.HasValue && siloData.FloorCount.Value > 0)
                {
                    var locationDetails = new List<LocationDetail>();
                    for (int i = 0; i < siloData.FloorCount.Value; i++)
                    {
                        locationDetails.Add(new LocationDetail
                        {
                            Code = req.Code,
                            Panel = string.Empty,
                            ItemCode = string.Empty,
                            FloorNum = i,
                            PanelCode = string.Empty,
                            ProductStatus = ProductStatus.EmptySiloBox,
                            Status = 1,
                            CreatorId = UserId,
                            CreateTime = DateTime.Now,
                            ModifierId = UserId,
                            ModifyTime = DateTime.Now
                        });
                    }

                    if (locationDetails.Any())
                    {
                        await _locationDetailDomainService.BulkInsert(locationDetails);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(req.SiloCode))
                {
                    var details = await _locationDetailDomainService.GetByCodeAsync(req.Code);
                    foreach (var item in details.Where(x => x.ProductStatus == ProductStatus.EmptyPayload))
                    {
                        item.ProductStatus = ProductStatus.EmptySiloBox;
                    }
                    await _locationDetailDomainService.BulkUpdate(details);
                }
            }

            return Success();
        }

        public async Task<ResponseDto<string>> UpdateExternal(ExternalAddOrUpdateRackReq req)
        {
            var rackData = await _rackDomainService.FindSingleAsync(p => p.Code.Trim().ToLower() == req.Code.Trim().ToLower());
            if (rackData == null) return Fail("没有找到该料架信息!");

            //if (!string.IsNullOrEmpty(req.SiloCode) 
            //    && await _rackDomainService.IsExistAsync(p => p.SiloCode == req.SiloCode && p.Code != req.Code))
            //{
            //    return Fail($"该料仓 {req.SiloCode} 已绑定其他料架!");
            //}

            rackData.IsHaveSilo = !string.IsNullOrEmpty(req.SiloCode);
            rackData.SiloCode = req.SiloCode.Trim();

            var result = await _rackDomainService.Update(rackData);
            if (!result)
            {
                return Fail($"更新失败！");
            }

            //if (!string.IsNullOrEmpty(rackData.SiloCode) && await _siloDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code)
            //    && p.Code.ToLower() == rackData.SiloCode.ToLower()))
            //{
            //    var siloData = await _siloDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code)
            //    && p.Code.ToLower() == rackData.SiloCode.ToLower());
            //    if (siloData != null)
            //    {
            //        siloData.Location = string.Empty;
            //        siloData.RelatedDeviceKind = null;
            //        siloData.ModifierId = UserId;
            //        siloData.ModifyTime = DateTime.Now;
            //        await _siloDomainService.Update(siloData);
            //    }
            //}

            //if (!string.IsNullOrEmpty(req.SiloCode))
            //{
            //    var siloData = await _siloDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code)
            //    && p.Code.ToLower() == req.SiloCode.ToLower());
            //    if (siloData != null)
            //    {
            //        siloData.Location = req.Code;
            //        if (req.DeviceKind != null)
            //        {
            //            siloData.RelatedDeviceKind = req.DeviceKind;
            //        }
            //        else
            //        {
            //            siloData.RelatedDeviceKind = DeviceKind.PublicPanelSiloWIP;
            //        }
            //        siloData.ModifierId = UserId;
            //        siloData.ModifyTime = DateTime.Now;
            //        await _siloDomainService.Update(siloData);
            //    }
            //}

            return Success();
        }

        public async Task<ResponseDto<string>> UnBind(UnBindRackAndSiolReq req)
        {
            if (req == null || string.IsNullOrEmpty(req.RackCode))
            {
                return Fail("未识别有效的料架编码！");
            }

            var rackData = await _rackDomainService.FindSingleAsync(p => p.Code == req.RackCode);
            if (rackData == null)
            {
                return Fail("没有找到该料架信息!");
            }

            //if (!string.IsNullOrEmpty(rackData.SiloCode) && await _siloDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code)
            //    && p.Code.ToLower() == rackData.SiloCode.ToLower()))
            //{
            //    var siloData = await _siloDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code)
            //    && p.Code.ToLower() == rackData.SiloCode.ToLower());
            //    if (siloData != null)
            //    {
            //        siloData.Location = string.Empty;
            //        siloData.RelatedDeviceKind = null;
            //        siloData.ModifierId = UserId;
            //        siloData.ModifyTime = DateTime.Now;
            //        await _siloDomainService.Update(siloData);
            //    }
            //}

            rackData.IsHaveSilo = false;
            rackData.SiloCode = "";
            rackData.ModifierId = UserId;
            rackData.ModifyTime = DateTime.Now;

            var updateResult = await _rackDomainService.Update(rackData);
            if (updateResult && !string.IsNullOrEmpty(rackData.RelateDeviceCode))
            {
                var details = await _locationDetailDomainService.GetByCodeAsync(req.RackCode);
                foreach (var item in details)
                {
                    item.ProductStatus = ProductStatus.EmptyPayload;
                    item.PanelCode = string.Empty;
                    item.Panel = string.Empty;
                    item.ItemCode = string.Empty;
                    item.PanelLength = 0;
                    item.PanelWidth = 0;
                    item.Pcs = 0;
                }
                await _locationDetailDomainService.BulkUpdate(details);

                DeviceCommandCentralRequest commandRequest = new DeviceCommandCentralRequest
                {
                    DeviceId = rackData.RelateDeviceCode,
                    Command = "RemoveSiloCommand",
                    Params = new Dictionary<string, object?> { { "DeviceCode", req.RackCode } }
                };

                var urlAddress = await _sysConfigManager.GetStringValue(MESConfigConstants.CENTRAL_ALLOTS_DEVICE_COMMAND);
                if (string.IsNullOrEmpty(urlAddress))
                {
                    return Fail("未识别有效的CentralAllotsDeviceCommand！");
                }

                var commandResult = await _centralOnlineDevice.AllotsDeviceCommand(urlAddress, commandRequest);
                if (!string.IsNullOrEmpty(commandResult))
                {
                    return Fail($"解绑料仓，下发命令失败：{commandResult}！请稍后重试！");
                }
            }

            // 解绑成功后，记录板料追溯
            await RecordPanelTraceForUnbind(req.RackCode);

            return Success();
        }

        public async Task<ResponseDto<string>> BulkInsert(List<RackExcelDto> req)
        {
            if (req == null || req.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<Rack> racks = new List<Rack>();
            List<Silo> siloUpdates = new List<Silo>();
            foreach (var item in req)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    sb.Append("编码：" + item.Code + " 为空；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                var isExsitCode = await _rackDomainService.IsExistAsync(p => p.Code == item.Code);
                if (isExsitCode)
                {
                    sb.Append("编码" + item.Code + " 数据库已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                if (racks.Exists(p => p.Code == item.Code))
                {
                    sb.Append("编码" + item.Code + " 导入列表中已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                bool existRackInfo = await _rackDomainService.IsExistAsync(p => p.SiloCode == item.SiloCode);
                if (existRackInfo)
                {
                    sb.Append($"该料仓 {item.SiloCode} 已绑定其他料架!");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                if (racks.Exists(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.ToLower() == item.SiloCode.ToLower()))
                {
                    sb.Append($"该料仓 {item.SiloCode} 重复!");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                var rack = _mapper.Map<Rack>(item);
                rack.IsHaveSilo = !string.IsNullOrEmpty(item.SiloCode);
                rack.CreateTime = DateTime.Now;
                rack.CreatorId = UserId;
                rack.Status = 1;
                racks.Add(rack);

                //var siloData = await _siloDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code)
                //    && p.Code.ToLower() == item.SiloCode.ToLower());
                //if (siloData != null)
                //{
                //    siloData.Location = item.Code;
                //    if (item.DeviceKind != null)
                //    {
                //        siloData.RelatedDeviceKind = item.DeviceKind;
                //    }
                //    else
                //    {
                //        siloData.RelatedDeviceKind = DeviceKind.PublicPanelSiloWIP;
                //    }
                //    siloData.ModifierId = UserId;
                //    siloData.ModifyTime = DateTime.Now;
                //    siloUpdates.Add(siloData);
                //}
            }

            await _rackDomainService.BulkInsert(racks);

            //await _siloDomainService.BulkUpdate(siloUpdates);

            string message = string.Format("预计导入：{0} 条；成功导入：{1} 条；失败：{2} 条；\r\n", req.Count, racks.Count, failCount);
            message += sb.ToString();

            return Success(message);
        }

        public async Task<int> GetRackStatus(string rackCode)
        {
            if (await _rackDomainService.IsExistAsync(d => d.Code.ToLower() == rackCode.ToLower()))
            {
                var rack = await _rackDomainService.FindSingleAsync(d => d.Code.ToLower() == rackCode.ToLower());
                return rack.Status;
            }
            else
            {
                return 0;
            }
        }

        public async Task<ResponseDto<List<SiloDetailDto>>> GetCentralRackPanels(string locationCode)
        {
            var returnData = new List<SiloDetailDto>();
            if (string.IsNullOrEmpty(locationCode))
            {
                return Fail<List<SiloDetailDto>>("未识别有效的rackCode！");
            }

            locationCode = locationCode.TrimStart().TrimEnd();

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.CENTRAL_GET_RACK_PANELS_URL);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return Fail<List<SiloDetailDto>>("未识别有效的CentralGetRackPanelsUrl！");
            }

            try
            {
                urlAdress = urlAdress + locationCode;
                var result = _apiHelper.RequestData(urlAdress, "Get");
                if (string.IsNullOrEmpty(result))
                {
                    return Fail<List<SiloDetailDto>>($"查询数据失败！{urlAdress}");
                }
                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (response == null)
                {
                    return Fail<List<SiloDetailDto>>("无法解析数据！");
                }

                var panels = response.ContainsKey("panels") ?
                        System.Text.Json.JsonSerializer.Deserialize<List<Fundation.Iot.Models.Panel>>(response["panels"].ObjToString(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) : null;

                if (panels == null || panels.Count == 0)
                {
                    return Fail<List<SiloDetailDto>>("无法解析Panels！");
                }

                List<int> floorNum = new List<int>();
                int i = 1;
                var siloDetails = new List<SiloDetail>();
                foreach (var panel in panels)
                {
                    siloDetails.Add(new SiloDetail
                    {
                        PanelCode = panel.PanelCode,
                        SiloCode = panel.SiloCode,
                        FloorNum = panel.Layer,
                        ItemCode = panel.ItemCode,
                        ProductStatus = panel.ProductStatus,
                        Pcs = panel.Pcs,
                        PinOffset = (decimal)panel.PinOffset,
                        PanelWidth = (decimal)panel.PanelWidth,
                        PanelLength = (decimal)panel.PanelLength,
                        Status = 1,
                        CreatorId = UserId,
                        CreateTime = DateTime.Now,
                        ModifierId = UserId,
                        ModifyTime = DateTime.Now,
                    });

                    if (string.IsNullOrEmpty(panel.PanelCode))
                    {
                        floorNum.Add(i);
                    }
                    i++;
                }

                var panelLimit = response.ContainsKey("panelLimit") ? response["panelLimit"].ObjToString().ToInt() : 0;
                string emptySilo = System.Text.Json.JsonSerializer.Serialize(floorNum);

                if (!string.IsNullOrEmpty(panels[0].SiloCode))
                {
                    string siloCode = panels[0].SiloCode;

                    await _domainService.UpdateAsync(p => new Rack
                    {
                        SiloCode = string.Empty,
                        ModifierId = UserId,
                        ModifyTime = DateTime.Now
                    }, p => p.SiloCode.ToLower() == siloCode.ToLower());

                    await _domainService.UpdateAsync(p => new Rack
                    {
                        SiloCode = siloCode,
                        ModifierId = UserId,
                        ModifyTime = DateTime.Now,
                    }, p => p.Code.ToLower() == locationCode.ToLower());

                    await _siloDomainService.UpdateAsync(p => new Silo
                    {
                        Location = string.Empty
                    }, p => p.Location.ToLower() == locationCode.ToLower());

                    if (!await _siloDomainService.IsExistAsync(p => p.Code.ToLower() == siloCode.ToLower()))
                    {
                        await _siloDomainService.Add(new Silo
                        {
                            Code = siloCode,
                            FloorCount = panelLimit,
                            Location = locationCode,
                            EmptySilo = emptySilo,
                            SiloStatus = SiloStatus.Ready,
                            CreateTime = DateTime.Now,
                            CreatorId = UserId,
                            Status = (int)DataStatusEnum.Enable
                        });
                    }
                    else
                    {
                        await _siloDomainService.UpdateAsync(p => new Silo
                        {
                            FloorCount = panelLimit,
                            Location = locationCode,
                            EmptySilo = emptySilo,
                            SiloStatus = SiloStatus.Ready,
                            ModifierId = UserId,
                            ModifyTime = DateTime.Now,
                        }, p => p.Code.ToLower() == siloCode.ToLower());
                    }

                    await _siloDetailDomainService.DeleteAsync(p => p.SiloCode.ToLower() == siloCode.ToLower());
                    await _siloDetailDomainService.BulkInsert(siloDetails);
                }

                returnData = _mapper.Map<List<SiloDetail>, List<SiloDetailDto>>(siloDetails);
            }
            catch (Exception ex)
            {
                return Fail<List<SiloDetailDto>>($"查询数据失败！{ex}");
            }

            return Success(returnData);
        }

        public async Task<ResponseDto<List<LocationDetailDto>>> SyncLocationPanels(string locationCode)
        {
            var returnData = new List<LocationDetailDto>();
            if (string.IsNullOrEmpty(locationCode))
            {
                return Fail<List<LocationDetailDto>>("未识别有效的locationCode！");
            }

            locationCode = locationCode.Trim();

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.CENTRAL_GET_RACK_PANELS_URL);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return Fail<List<LocationDetailDto>>("未识别有效的CentralGetRackPanelsUrl！");
            }

            try
            {
                urlAdress = urlAdress + locationCode;
                var apiResult = _apiHelper.RequestData(urlAdress, "Get");
                if (string.IsNullOrEmpty(apiResult))
                {
                    return Fail<List<LocationDetailDto>>($"查询数据失败！{urlAdress}");
                }
                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResult);
                if (response == null)
                {
                    return Fail<List<LocationDetailDto>>("无法解析数据！");
                }

                var panels = response.ContainsKey("panels") ?
                        System.Text.Json.JsonSerializer.Deserialize<List<Fundation.Iot.Models.Panel>>(response["panels"].ObjToString(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) : null;

                if (panels == null || panels.Count == 0)
                {
                    return Fail<List<LocationDetailDto>>("无法解析Panels！");
                }

                List<int> floorNum = new List<int>();
                int i = 1;
                var details = new List<LocationDetail>();
                foreach (var panel in panels)
                {
                    details.Add(new LocationDetail
                    {
                        Code = locationCode,
                        PanelCode = panel.PanelCode,
                        FloorNum = panel.Layer,
                        ItemCode = panel.ItemCode,
                        ProductStatus = panel.ProductStatus,
                        Pcs = panel.Pcs,
                        PinOffset = (decimal)panel.PinOffset,
                        PanelWidth = (decimal)panel.PanelWidth,
                        PanelLength = (decimal)panel.PanelLength,
                        Status = 1,
                        CreatorId = UserId,
                        CreateTime = DateTime.Now,
                        ModifierId = UserId,
                        ModifyTime = DateTime.Now,
                    });

                    if (string.IsNullOrEmpty(panel.PanelCode))
                    {
                        floorNum.Add(i);
                    }
                    i++;
                }

                var panelLimit = response.ContainsKey("panelLimit") ? response["panelLimit"].ObjToString().ToInt() : 0;
                var siloCode = string.Empty;
                if (panels.Any(p => !string.IsNullOrEmpty(p.SiloCode)))
                {
                    siloCode = panels.FirstOrDefault(p => !string.IsNullOrEmpty(p.SiloCode)).SiloCode;
                }

                await _domainService.UpdateAsync(p => new Rack
                {
                    SiloCode = siloCode,
                    IsHaveSilo = !string.IsNullOrEmpty(siloCode),
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now,
                }, p => p.Code.ToLower() == locationCode.ToLower());

                //if (!await _siloDomainService.IsExistAsync(p => p.Code.ToLower() == siloCode.ToLower()))
                //{
                //    await _siloDomainService.Add(new Silo
                //    {
                //        Code = siloCode,
                //        FloorCount = panelLimit,
                //        SiloStatus = SiloStatus.Ready,
                //        CreateTime = DateTime.Now,
                //        CreatorId = UserId,
                //        Status = (int)DataStatusEnum.Enable
                //    });
                //}                

                await _locationDetailDomainService.DeleteAsync(p => p.Code.ToLower() == locationCode.ToLower());
                await _locationDetailDomainService.BulkInsert(details);

                returnData = _mapper.Map<List<LocationDetail>, List<LocationDetailDto>>(details);
            }
            catch (Exception ex)
            {
                return Fail<List<LocationDetailDto>>($"查询数据失败！{ex}");
            }

            return Success(returnData);
        }
        /// <summary>
        /// 记录料架解绑时的板料追溯信息
        /// </summary>
        /// <param name="locationCode">料架编码</param>
        /// <returns></returns>
        private async Task RecordPanelTraceForUnbind(string locationCode)
        {
            try
            {
                var locationDetailsWithSilo = await _locationDetailDomainService.GetByCodeWithSiloCodeAsync(locationCode);
                if (locationDetailsWithSilo != null && locationDetailsWithSilo.Any())
                {
                    var panels = new List<VgAutoDrill.Fundation.Iot.Models.Panel>();
                    foreach (var (locationDetail, siloCode) in locationDetailsWithSilo)
                    {
                        var panel = new VgAutoDrill.Fundation.Iot.Models.Panel
                        {
                            PanelCode = locationDetail.PanelCode,
                            ItemCode = locationDetail.ItemCode,
                            BatchCode = locationDetail.BatchCode ?? string.Empty,
                            ProductStatus = locationDetail.ProductStatus ?? ProductStatus.EmptyPayload,
                            Position = locationDetail.FloorNum ?? 0,
                            LocationCode = locationDetail.Code,
                            SiloCode = siloCode,
                            PanelWidth = (float)(locationDetail.PanelWidth ?? 0),
                            PanelLength = (float)(locationDetail.PanelLength ?? 0),
                            PinOffset = (float)(locationDetail.PinOffset ?? 0)
                        };
                        panels.Add(panel);
                    }

                    if (panels.Any())
                    {
                        var panelList = PanelList.FromList(panels);

                        await _siloPanelTraceService.AddFromPanelList(
                            panelList,
                            $"料仓移除",
                            null,
                            null
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "料架解绑时板料追溯记录失败 - 料架编码: {RackCode}", locationCode);
            }
        }
    }
}
