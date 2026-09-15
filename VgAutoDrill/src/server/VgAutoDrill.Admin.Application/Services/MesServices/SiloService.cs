using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SqlSugar;
using System.Text;
using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class SiloService : BaseServiceWithoutTree<Silo, SiloDto, AddOrUpdateSiloReq>, ISiloService
    {
        private readonly ISiloDomainService _siloDomainService;
        private readonly ISiloDetailDomainService _siloDetailDomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRackDomainService _rackDomainService;
        private readonly IWorkOrderAndPanelService _workOrderAndPanelService;
        private readonly IConfiguration _configuration;
        private readonly IEncodeBuildRulesService _encodeService;
        public readonly SqlSugarScope _sqlSugarScope;
        private readonly IAPIHelper _apiHelper;
        private readonly ILogger<SiloService> _logger;
        private readonly IPanelService _panelService;
        private readonly IProBoardTraceDomainService _panelDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        private readonly IOptions<InnerOptions> _innerOptions;
        private readonly ILocationDetailRepository _locationDetailRepository;
        private readonly ISiloPanelTraceService _siloPanelTraceService;

        public SiloService(ISiloDomainService siloDomain,
            IMapper mapper,
            ISiloDetailDomainService siloDetailDomain,
            IUnitOfWork unitWork,
            ILoggerFactory loggerFactory,
            IRackDomainService rackDomainService,
            IWorkOrderAndPanelService workOrderAndPanelService,
            IConfiguration configuration,
            IAPIHelper apiHelper,
            IEncodeBuildRulesService encodeBuildRulesService,
            IPanelService panelService,
            IProBoardTraceDomainService boardTraceDomainService,
            ISysConfigManager sysConfigManager,
            ICentralOnlineDevice centralOnlineDevice,
            IOptions<InnerOptions> innerOptions,
            ILocationDetailRepository locationDetailRepository,
            ISiloPanelTraceService siloPanelTraceService)
            : base(siloDomain, mapper)
        {
            _siloDomainService = siloDomain;
            _siloDetailDomainService = siloDetailDomain;
            _unitOfWork = unitWork;
            _apiHelper = apiHelper;
            _rackDomainService = rackDomainService;
            _workOrderAndPanelService = workOrderAndPanelService;
            _configuration = configuration;
            _sqlSugarScope = unitWork.GetDbClient();
            _encodeService = encodeBuildRulesService;
            _logger = loggerFactory.CreateLogger<SiloService>();
            _panelService = panelService;
            _panelDomainService = boardTraceDomainService;
            _sysConfigManager = sysConfigManager;
            _centralOnlineDevice = centralOnlineDevice;
            _innerOptions = innerOptions;
            _locationDetailRepository = locationDetailRepository;
            _siloPanelTraceService = siloPanelTraceService;
        }

        public async Task<ResponseDto<PageDto<SiloDto>>> GetList(SiloQueryReq req)
        {
            if (req == null) ThrowHelper.ThrowArgumentNullException(nameof(req));
            req.PageNum = req.PageNum < 1 ? 1 : req.PageNum;
            req.PageSize = req.PageSize < 1 ? 10 : req.PageSize;
            var pageDto = new PageDto<SiloDto>(req.PageNum, req.PageSize);
            var where = PredicateBuilder.True<Silo>();
            where = where.And(p => p.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => p.Code.ToLower().Contains(req.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(req.VendorName))
            {
                where = where.And(p => p.VendorName.Contains(req.VendorName));
            }
            if (!string.IsNullOrEmpty(req.VendorCode))
            {
                where = where.And(p => p.VendorCode.ToLower().Contains(req.VendorCode.ToLower()));
            }

            if (req.SiloStatus.HasValue)
            {
                where = where.And(p => p.SiloStatus.Equals(req.SiloStatus));
            }
            var siloInfo = await _siloDomainService.QueryPageAsync(where, p => p.Code, OrderByType.Desc, req.PageNum, req.PageSize);
            pageDto.Total = siloInfo.TotalCount;
            pageDto.List = _mapper.Map<List<SiloDto>>(siloInfo?.ToList());

            return Success(pageDto);
        }
        public override async Task<ResponseDto<string>> Add(AddOrUpdateSiloReq req)
        {
            if (req.AddNum <= 0)
            {
                req.AddNum = 1;
            }
            var silos = new List<Silo>();
            var siloDetails = new List<SiloDetail>();
            var newSiloCodes = !string.IsNullOrEmpty(req.Code) && req.AddNum == 1 ? new List<string>() { req.Code }
              : await GenerateNewSiloCode(req.AddNum);//批量先生成siloCode            
            for (var i = 0; i < newSiloCodes.Count(); i++)
            {
                var model = _mapper.Map<Silo>(req);
                model.Code = newSiloCodes[i];
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.Status = (int)DataStatusEnum.Enable;
                model.SiloStatus = SiloStatus.Ready;
                List<int> floorNum = new List<int>();
                for (int m = 0; m < req.FloorCount; m++)
                {
                    var siloDetail = new SiloDetail()
                    {
                        SiloCode = model.Code,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        FloorNum = m,
                        Status = 1
                    };
                    floorNum.Add(m + 1);
                    siloDetails.Add(siloDetail);
                }
                model.EmptySilo = JsonConvert.SerializeObject(floorNum);
                silos.Add(model);

            }
            if (silos.Count > 0 && siloDetails?.Count > 0)
            {
                _unitOfWork.BeginTran();//加事务
                await _domainService.BulkInsert(silos);
                await _siloDetailDomainService.BulkInsert(siloDetails);
                _unitOfWork.CommitTran();
            }
            return Success();
        }
        private async Task<List<string>> GenerateNewSiloCode(int? num = 1)
        {
            var newGroupNo = await _encodeService.GetEncodeList(new GetEncodeByRulesListReq()
            {
                BuildCount = num,
                RulesCode = "SILO_CODE"
            });
            return newGroupNo.Data;
        }
        public async Task<ResponseDto<string>> AddBatch(List<AddOrUpdateSiloReq> reqs)
        {
            if (reqs == null) ThrowHelper.ThrowArgumentNullException(nameof(reqs));

            if (reqs.Select(p => p.Code).Distinct().ToList().Count != reqs.Count) return Fail("参数中有重复Code!");

            List<Silo> silos = new List<Silo>();
            List<SiloDetail> siloDetails = new List<SiloDetail>();
            List<string> deleteSiloDetails = new List<string>();
            foreach (var req in reqs)
            {
                var siloInfo = await _domainService.QueryAsync(p => p.Code == req.Code, p => p.Code, OrderByType.Asc);
                if (siloInfo != null && siloInfo.Count > 0) { return Fail("该料仓已存在,不可重复添加!"); }
                ;

                var model = _mapper.Map<Silo>(req);
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.Status = (int)DataStatusEnum.Enable;
                List<int> floorNum = new List<int>();

                for (int i = 0; i < req.FloorCount; i++)
                {
                    var siloDetail = new SiloDetail()
                    {
                        SiloCode = req.Code,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        FloorNum = i,
                        Status = 1
                    };
                    floorNum.Add(i + 1);
                    siloDetails.Add(siloDetail);
                }
                deleteSiloDetails.Add(req.Code);

                model.EmptySilo = System.Text.Json.JsonSerializer.Serialize(floorNum);
                silos.Add(model);
            }

            await _siloDetailDomainService.DeleteAsync(p => deleteSiloDetails.Contains(p.SiloCode));
            await _siloDetailDomainService.BulkInsert(siloDetails);
            await _domainService.BulkInsert(silos);

            return Success();
        }

        public async Task<ResponseDto<string>> DeleteData(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            await _domainService.DeleteById(id);
            await _siloDetailDomainService.DeleteAsync(p => p.SiloCode == entity.Code);
            //await _rackDomainService.UpdateAsync(p => new Rack() { SiloCode = "", IsHaveSilo = (byte)0, ModifierId = UserId, ModifyTime = DateTime.Now }, p => p.SiloCode == entity.Code);

            return Success("删除成功!");
        }

        public async Task<ResponseDto<string>> DeleteDataList(List<long> delList)
        {
            if (delList == null || delList.Count == 0)
            {
                return Fail("未识别有效的删除数据！");
            }

            object[] deleteList = new object[delList.Count];
            List<string> deleteSiloCode = new List<string>();
            for (int i = 0; i < delList.Count; i++)
            {
                var entity = await _domainService.QueryByID(delList[i]);
                if (entity == null)
                {
                    continue;
                }

                deleteList[i] = delList[i];
                deleteSiloCode.Add(entity.Code);
            }

            var result = await _domainService.DeleteByIds(deleteList);
            if (result)
            {
                await _siloDetailDomainService.DeleteAsync(p => deleteSiloCode.Contains(p.SiloCode));
                //await _rackDomainService.UpdateAsync(p => new Rack() { SiloCode = "", IsHaveSilo = false, ModifierId = UserId, ModifyTime = DateTime.Now }, p => deleteSiloCode.Contains(p.SiloCode));
            }

            return Success();
        }

        public override async Task<ResponseDto<string>> Update(AddOrUpdateSiloReq req)
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

            List<SiloDetail> siloDetails = new List<SiloDetail>();
            var model = _mapper.Map<Silo>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;

            for (int i = 0; i < req.FloorCount - entity.FloorCount; i++)
            {
                var siloDetail = new SiloDetail()
                {
                    SiloCode = req.Code,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                    FloorNum = entity.FloorCount + i,
                    Status = 1
                };
                siloDetails.Add(siloDetail);
            }

            await _siloDetailDomainService.BulkInsert(siloDetails);
            await _siloDetailDomainService.DeleteAsync(p => p.SiloCode == entity.Code && p.FloorNum > req.FloorCount - 1);
            model.EmptySilo = System.Text.Json.JsonSerializer.Serialize((await _siloDetailDomainService.QueryAsync(p => p.SiloCode == entity.Code && p.ProductStatus == 0, p => p.SiloCode, OrderByType.Asc))?
                              .Select(p => p.FloorNum + 1).ToList());
            await _domainService.Update(model);

            return Success();
        }

        public async Task<ResponseDto<List<SiloDetailDto>>> GetSiloDetails(SiloDetailQueryReq req)
        {
            if (req == null) ThrowHelper.ThrowArgumentNullException(nameof(req));

            var query = _sqlSugarScope.Queryable<Silo, SiloDetail>
                   ((s, sd) => new object[]
                   {
                      JoinType.Left, s.Code == sd.SiloCode
                   }).Where((s, sd) => s.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                query = query.Where((s, sd) => sd.SiloCode.Contains(req.SiloCode));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((s, sd) => sd.ItemCode.Contains(req.ItemCode));
            }

            query = query.OrderBy((s, sd) => sd.SiloCode);
            query = query.OrderBy((s, sd) => sd.FloorNum);

            var data = await query.Select((s, sd) => new
            {
                sd.SiloCode,
                sd.ItemCode,
                sd.PanelCode,
                sd.PanelWidth,
                sd.PanelLength,
                sd.FloorNum,
                sd.ProductStatus,
                sd.Pcs,
                sd.PinOffset,
                s.SiloStatus,
                sd.Id,
                sd.CreateTime,
                sd.CreatorId,
                sd.ModifierId,
                sd.ModifyTime,
                sd.Status,
                sd.IsDeleted,

            }).ToListAsync();

            if (data == null || data.Count == 0)
            {
                return Success(new List<SiloDetailDto>());
            }

            List<SiloDetailDto> result = new List<SiloDetailDto>();
            foreach (var item in data)
            {
                var siloDetail = new SiloDetailDto
                {
                    SiloCode = item.SiloCode,
                    ItemCode = item.ItemCode,
                    PanelCode = item.PanelCode,
                    PanelWidth = item.PanelWidth,
                    PanelLength = item.PanelLength,
                    FloorNum = item.FloorNum + 1,
                    ProductStatus = (int)item.ProductStatus,
                    Pcs = item.Pcs?.ToString() ?? string.Empty,
                    PinOffset = item.PinOffset,
                    SiloStatus = item.SiloStatus,
                    Id = item.Id,
                    CreateTime = item.CreateTime,
                    CreatorId = item.CreatorId,
                    ModifierId = item.ModifierId,
                    ModifyTime = item.ModifyTime,
                    Status = item.Status,
                    IsDeleted = item.IsDeleted == 0 ? false : true,
                };
                result.Add(siloDetail);
            }

            return Success(result);
        }

        public async Task<ResponseDto<List<SiloDetailDto>>> GetSiloDetailsByLocation(string location)
        {
            if (string.IsNullOrEmpty(location)) return Fail<List<SiloDetailDto>>("未识别有效的Location！");

            var query = _sqlSugarScope.Queryable<Silo, SiloDetail>
                   ((s, sd) => new object[]
                   {
                      JoinType.Left, s.Code == sd.SiloCode
                   }).Where((s, sd) => s.IsDeleted == 0 && !string.IsNullOrEmpty(s.Location) && s.Location.ToLower() == location.ToLower());

            query = query.OrderBy((s, sd) => sd.SiloCode);
            query = query.OrderBy((s, sd) => sd.FloorNum);

            var data = await query.Select((s, sd) => new
            {
                sd.SiloCode,
                sd.ItemCode,
                sd.PanelCode,
                sd.PanelWidth,
                sd.PanelLength,
                sd.FloorNum,
                sd.ProductStatus,
                sd.Pcs,
                sd.PinOffset,
                s.SiloStatus,
                sd.Id,
                sd.CreateTime,
                sd.CreatorId,
                sd.ModifierId,
                sd.ModifyTime,
                sd.Status,
                sd.IsDeleted,

            }).ToListAsync();

            if (data == null || data.Count == 0)
            {
                return Success(new List<SiloDetailDto>());
            }

            List<SiloDetailDto> result = new List<SiloDetailDto>();
            foreach (var item in data)
            {
                var siloDetail = new SiloDetailDto
                {
                    SiloCode = item.SiloCode,
                    ItemCode = item.ItemCode,
                    PanelCode = item.PanelCode,
                    PanelWidth = item.PanelWidth,
                    PanelLength = item.PanelLength,
                    FloorNum = item.FloorNum + 1,
                    ProductStatus = (int)item.ProductStatus,
                    Pcs = item.Pcs?.ToString() ?? string.Empty,
                    PinOffset = item.PinOffset,
                    SiloStatus = item.SiloStatus,
                    Id = item.Id,
                    CreateTime = item.CreateTime,
                    CreatorId = item.CreatorId,
                    ModifierId = item.ModifierId,
                    ModifyTime = item.ModifyTime,
                    Status = item.Status,
                    IsDeleted = item.IsDeleted == 0 ? false : true,
                };
                result.Add(siloDetail);
            }

            return Success(result);
        }

        public async Task<ResponseDto<List<LocationDetailDto>>> AddOrUpdate(AddOrUpdateSiloDetailReq req)
        {
            try
            {
                if (req == null)
                {
                    return Fail<List<LocationDetailDto>>("信息格式错误!");
                }

                if (string.IsNullOrEmpty(req.SiloCode) || !await _domainService.IsExistAsync(p => p.Code.ToLower() == req.SiloCode.ToLower()))
                {
                    return Fail<List<LocationDetailDto>>($"未找到料仓{req.SiloCode}!");
                }

                var oldSiloDetails = await _siloDetailDomainService.QueryAsync(p => p.SiloCode == req.SiloCode, p => p.SiloCode, OrderByType.Asc);
                List<SiloDetail> siloDetails = new List<SiloDetail>();
                List<Model.Entites.Mes.TracePanel> panels = new List<Model.Entites.Mes.TracePanel>();
                for (int i = 0; i < req.FloorCount; i++)
                {
                    string panelCode = oldSiloDetails.FirstOrDefault(p => p.FloorNum == req.BeginFloorNum + i - 1)?.PanelCode;
                    if (string.IsNullOrEmpty(panelCode))
                    {
                        panelCode = (await _encodeService.GetEncodeList(new GetEncodeByRulesListReq() { RulesCode = "PANEL_CODE", BuildCount = 1 }))?.Data?[0];
                        panels.Add(new Model.Entites.Mes.TracePanel
                        {
                            PanelCode = panelCode,
                            ItemCode = req.ItemCode,
                            ProductStatus = (ProductStatus)req.ProductStatus,
                            BatchCode = req.BatchCode,
                            PanelWidth = req.PanelWidth,
                            PanelLength = req.PanelLength,
                            PinOffset = req.PinOffset,
                            LocationCode = req.LocationCode,
                            SiloCode = req.SiloCode, // 添加料仓编码
                            Status = (int)DataStatusEnum.Enable,
                            CreateTime = DateTime.Now,
                            CreatorId = UserId,
                        });
                    }
                    else if (!await _panelDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.PanelCode) && p.PanelCode.ToLower().Equals(panelCode.ToLower())))
                    {
                        panels.Add(new Model.Entites.Mes.TracePanel
                        {
                            PanelCode = panelCode,
                            ItemCode = req.ItemCode,
                            ProductStatus = (ProductStatus)req.ProductStatus,
                            BatchCode = req.BatchCode,
                            PanelWidth = req.PanelWidth,
                            PanelLength = req.PanelLength,
                            PinOffset = req.PinOffset,
                            LocationCode = req.LocationCode,
                            SiloCode = req.SiloCode, // 添加料仓编码
                            Status = (int)DataStatusEnum.Enable,
                            CreateTime = DateTime.Now,
                            CreatorId = UserId,
                        });
                    }
                    var siloDetail = _mapper.Map<SiloDetail>(req);
                    siloDetail.SiloCode = req.SiloCode;
                    siloDetail.PanelCode = panelCode;
                    siloDetail.ItemCode = req.ItemCode;
                    siloDetail.PanelWidth = req.PanelWidth;
                    siloDetail.PanelLength = req.PanelLength;
                    siloDetail.PinOffset = req.PinOffset;
                    siloDetail.Pcs = int.TryParse(req.Pcs, out var pcsValue) ? pcsValue : null;
                    siloDetail.CreateTime = DateTime.Now;
                    siloDetail.CreatorId = UserId;
                    siloDetail.FloorNum = req.BeginFloorNum + i - 1;
                    siloDetail.Status = 1;
                    siloDetail.ProductStatus = req.ProductStatus.Value;
                    siloDetail.ModifyTime = DateTime.Now;
                    siloDetail.ModifierId = UserId;
                    siloDetails.Add(siloDetail);
                }

                await _panelDomainService.BulkInsert(panels);

                var x = _sqlSugarScope.Storageable(siloDetails).WhereColumns(p => new { p.SiloCode, p.FloorNum }).ToStorage();
                x.AsInsertable.IgnoreColumns(z => new { z.ModifierId, z.ModifyTime }).ExecuteCommand();
                x.AsUpdateable.IgnoreColumns(z => new { z.CreateTime, z.CreatorId }).ExecuteCommand();

                // 插入 location_detail 表数据
                if (!string.IsNullOrEmpty(req.LocationCode))
                {
                    await InsertLocationDetailData(req, siloDetails);
                }

                var siloInfo = (await _domainService.QueryAsync(p => p.Code == req.SiloCode, p => p.Code, OrderByType.Asc)).FirstOrDefault();
                if (siloInfo != null)
                {
                    siloInfo.EmptySilo = System.Text.Json.JsonSerializer.Serialize((await _siloDetailDomainService.QueryAsync(p => p.SiloCode == req.SiloCode && p.ProductStatus == 0, p => p.SiloCode, OrderByType.Asc))?
                              .Select(p => p.FloorNum + 1).ToList());
                    await _domainService.Update(siloInfo);
                }

                // 返回 location_detail 表的数据
                if (!string.IsNullOrEmpty(req.LocationCode))
                {
                    var locationDetails = await _locationDetailRepository.GetByCodeAsync(req.LocationCode);
                    var locationDetailDtos = _mapper.Map<List<LocationDetailDto>>(locationDetails);
                    return Success(locationDetailDtos);
                }
                else
                {
                    // 如果没有 LocationCode，返回空的 LocationDetailDto 列表
                    return Success(new List<LocationDetailDto>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddOrUpdate 方法执行失败: {Message}", ex.Message);
                return Fail<List<LocationDetailDto>>($"操作失败: {ex.Message}");
            }
        }

        public async Task<ResponseDto<string>> AllotsPanelData(AllotsPanelDataReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的入参！");
            }
            if (string.IsNullOrEmpty(req.DeviceCode))
            {
                return Fail("未识别有效的DeviceCode！");
            }
            var urlAddress = await _sysConfigManager.GetStringValue(MESConfigConstants.CENTRAL_ALLOTS_PANEL_DATA);
            if (string.IsNullOrEmpty(urlAddress))
            {
                return Fail("未识别有效的AllotsPanelData！");
            }

            try
            {
                var rack = await _rackDomainService.FindSingleAsync(x => x.Code.ToLower() == req.DeviceCode.ToLower());
                if (rack != null && !string.IsNullOrEmpty(rack.RelateDeviceCode))
                {
                    req.RackCode = rack.Code;
                }
                string requestJson = JsonConvert.SerializeObject(req);
                var result = _apiHelper.RequestData(urlAddress, "post", requestJson);
                if (string.IsNullOrEmpty(result))
                {
                    // 下发成功后，记录板料追溯
                    await RecordPanelTraceForAllots(req);
                    return Success();
                }
                else
                {
                    return Fail(result);
                }
            }
            catch (Exception e)
            {
                return Fail(e.Message);
            }
        }

        public async Task<ResponseDto<string>> UpdateExternalSiloInfo(ExternalUpdateSiloReq req)
        {
            await _sqlSugarScope.Updateable<Silo>()
               .SetColumns(p => new Silo() { Status = req.Status.Value, ModifyTime = DateTime.Now })
               .Where(p => p.Code == req.Code)
               .ExecuteCommandAsync();

            return Success();
        }

        public async Task<ResponseDto<string>> DeleteExternalSiloInfo(string code)
        {
            var siloInfo = (await _siloDomainService.QueryAsync(p => p.Code.Equals(code), p => p.Code, OrderByType.Asc))?.FirstOrDefault();
            if (siloInfo == null) { return Fail("未查到料仓信息!"); }

            return await Delete(siloInfo.Id);
        }

        public async Task<ResponseDto<List<SiloInfo>>> GetExternalSiloInfo(ExternalSiloQueryReq req)
        {
            List<SiloInfo> result = new List<SiloInfo>();
            var query = _sqlSugarScope.Queryable<Silo, SiloDetail>
                   ((s, sd) => new object[]
                   {
                      JoinType.Left, s.Code == sd.SiloCode
                   })
                    .Where((s, sd) => s.IsDeleted == 0);

            query = query.WhereIF(!string.IsNullOrEmpty(req.Code), (s, sd) => s.Code.Contains(req.Code));
            var queryResult = query.Select((s, sd) =>
             new
             {
                 s.Code,
                 s.FloorCount,
                 s.Size,
                 s.SiloStatus,
                 s.RelatedDeviceKind,
                 s.Location,
                 s.EmptySilo,
                 FloorNum = sd.FloorNum + 1,
                 sd.ItemCode,
                 sd.PanelCode,
                 sd.ProductStatus,
                 sd.Pcs,
                 sd.PanelWidth,
                 sd.PanelLength,
                 sd.PinOffset,
             }).ToList();

            if (queryResult != null)
            {
                var siloNos = queryResult.Select(t => t.Code).Distinct().ToList();
                foreach (var siloNo in siloNos)
                {
                    var siloInfo = new SiloInfo
                    {
                        SiloCode = siloNo,
                        SiloStatus = queryResult.FirstOrDefault(p => p.Code == siloNo)?.SiloStatus,
                        RelatedDeviceKind = queryResult.FirstOrDefault(p => p.Code == siloNo)?.RelatedDeviceKind,
                        Location = queryResult.FirstOrDefault(p => p.Code == siloNo)?.Location,
                        EmptySilo = queryResult.FirstOrDefault(p => p.Code == siloNo)?.EmptySilo,
                        Size = queryResult.FirstOrDefault(p => p.Code == siloNo)?.Size,
                        FloorCount = queryResult.FirstOrDefault(p => p.Code == siloNo)?.FloorCount,
                        SiloDetails = queryResult.Where(p => p.Code == siloNo)
                                                      .Select(t => new ExternalSiloDetailDto()
                                                      {
                                                          ItemCode = t.ItemCode,
                                                          FloorNum = t.FloorNum,
                                                          PanelCode = t.PanelCode,
                                                          PanelWidth = t.PanelWidth,
                                                          PanelLength = t.PanelLength,
                                                          Pcs = t.Pcs?.ToString(),
                                                          PinOffset = t.PinOffset,
                                                          ProductStatus = t.ProductStatus,
                                                      }).ToList()
                    };

                    result.Add(siloInfo);
                }
            }

            return Success(result);
        }

        public async Task<ResponseDto<string>> AddOrUpdateExternalSiloWithPanel(ExternalAddOrUpdateSiloWithPanelReq req)
        {
            if (req == null || req.SiloDetails == null)
            {
                return Fail("信息格式错误!");
            }

            var silo = (await _siloDomainService.QueryAsync(p => p.Code == req.Code, p => p.Code, OrderByType.Asc))?.FirstOrDefault();
            if (silo == null) return Fail("没有找到该料仓信息!");

            var siloDetails = _mapper.Map<List<SiloDetail>>(req.SiloDetails);
            siloDetails.ForEach(p =>
            {
                p.Status = 1;
                p.FloorNum -= 1;
                p.CreateTime = DateTime.Now;
                p.CreatorId = UserId;
                p.ModifierId = UserId;
                p.ModifyTime = DateTime.Now;
            });

            var x = _sqlSugarScope.Storageable(siloDetails).WhereColumns(p => new { p.SiloCode, p.FloorNum }).ToStorage();
            x.AsInsertable.IgnoreColumns(z => new { z.ModifierId, z.ModifyTime }).ExecuteCommand();
            x.AsUpdateable.IgnoreColumns(z => new { z.CreateTime, z.CreatorId }).ExecuteCommand();

            var siloInfo = (await _domainService.QueryAsync(p => p.Code == req.Code, p => p.Code, OrderByType.Asc)).FirstOrDefault();
            siloInfo.EmptySilo = System.Text.Json.JsonSerializer.Serialize((await _siloDetailDomainService.QueryAsync(p => p.SiloCode == req.Code && p.ProductStatus == 0, p => p.SiloCode, OrderByType.Asc))?
                      .Select(p => p.FloorNum + 1).ToList());
            await _domainService.Update(siloInfo);

            return Success();
        }

        public async Task<ResponseDto<string>> UpdateExternal(ExternalAddOrUpdateSiloReq req)
        {
            var siloInfo = await _domainService.QueryAsync(p => p.Code == req.Code, p => p.Code, OrderByType.Asc);
            if (siloInfo == null || siloInfo.Count <= 0) return Fail("没有找到该料仓信息!");

            var newSiloInfo = _mapper.Map<AddOrUpdateSiloReq>(req);
            newSiloInfo.Id = siloInfo.FirstOrDefault().Id;
            return await Update(newSiloInfo);
        }

        public async Task<ResponseDto<string>> UnBindPanel(ExternalSiloUnBindReq req)
        {
            try
            {
                if (req?.UnBindSiloDetails == null || !req.UnBindSiloDetails.Any())
                {
                    return Fail("解绑参数不能为空！");
                }

                var firstUnBindDetail = req.UnBindSiloDetails.FirstOrDefault();
                if (string.IsNullOrEmpty(firstUnBindDetail?.LocationCode))
                {
                    return Fail("库位编码不能为空！");
                }

                // 从 location_detail 表删除数据
                foreach (var unBindDetail in req.UnBindSiloDetails)
                {
                    if (unBindDetail.FloorNum.HasValue && !string.IsNullOrEmpty(unBindDetail.LocationCode))
                    {
                        var targetFloorNum = unBindDetail.FloorNum.Value; 

                        // 查询要删除的记录
                        var locationDetails = await _locationDetailRepository.QueryAsync(
                            p => p.Code == unBindDetail.LocationCode && p.FloorNum == targetFloorNum,
                            null, OrderByType.Asc);

                        if (locationDetails != null && locationDetails.Any())
                        {
                            // 获取板料编码，用于清除 panel 表数据
                            var panelCodes = locationDetails.Where(x => !string.IsNullOrEmpty(x.PanelCode))
                                                          .Select(x => x.PanelCode)
                                                          .Distinct()
                                                          .ToList();

                            // 清空 location_detail 记录数据（保留 floor_num）
                            foreach (var record in locationDetails)
                            {
                                await _locationDetailRepository.UpdateAsync(p => new LocationDetail()
                                {
                                    PanelCode = string.Empty,
                                    ItemCode = string.Empty,
                                    ProductStatus = ProductStatus.EmptySiloBox, // 空状态
                                    Pcs = null,
                                    PanelWidth = null,
                                    PanelLength = null,
                                    PinOffset = null,
                                    Panel = string.Empty,
                                    ModifierId = UserId,
                                    ModifyTime = DateTime.Now
                                }, p => p.Id == record.Id);
                            }
                            _logger.LogInformation($"已清空库位 {unBindDetail.LocationCode} 第 {unBindDetail.FloorNum} 层的绑定记录");

                            // 清除 panel 表中的 locationCode
                            foreach (var panelCode in panelCodes)
                            {
                                try
                                {
                                    // 直接使用 SQL 更新 panel 表，清除 locationCode
                                    await _sqlSugarScope.Updateable<Model.Entites.Mes.TracePanel>()
                                        .SetColumns(p => new Model.Entites.Mes.TracePanel()
                                        {
                                            LocationCode = string.Empty,
                                            ModifierId = UserId,
                                            ModifyTime = DateTime.Now
                                        })
                                        .Where(p => p.PanelCode == panelCode)
                                        .ExecuteCommandAsync();

                                    _logger.LogInformation($"已清除板料 {panelCode} 的 locationCode 信息");
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, $"清除板料 {panelCode} 信息失败: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"库位 {unBindDetail.LocationCode} 第 {unBindDetail.FloorNum} 层没有绑定记录");
                        }
                    }
                }

                return Success("解绑成功！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UnBindPanel 方法执行失败: {Message}", ex.Message);
                return Fail($"解绑失败: {ex.Message}");
            }
        }

        public async Task<ResponseDto<string>> UnBindAllPanel(ExternalSiloAllUnBindReq req)
        {
            var siloInfo = (await _domainService.QueryAsync(p => p.Code == req.SiloCode, p => p.Code, OrderByType.Asc)).FirstOrDefault();
            if (siloInfo == null)
            {
                return Fail($"系统中未查询到{req.SiloCode}与板料的绑定关系");
            }

            var preCheck = await CheckExternalSetSiloStatusReq(req.SiloCode);
            if (preCheck.Code == ResponseCode.Fail)
            {
                return preCheck;
            }

            await _sqlSugarScope.Updateable<SiloDetail>()
                  .SetColumns(p => new SiloDetail() { ItemCode = "", PanelCode = "", PanelWidth = 0, PanelLength = 0, Pcs = 0, ProductStatus = 0, PinOffset = 0, ModifierId = UserId, ModifyTime = DateTime.Now })
                  .Where(p => p.SiloCode == req.SiloCode).ExecuteCommandAsync();

            siloInfo.EmptySilo = System.Text.Json.JsonSerializer.Serialize((await _siloDetailDomainService.QueryAsync(p => p.SiloCode == req.SiloCode && p.ProductStatus == 0, p => p.SiloCode, OrderByType.Asc))?
                      .Select(p => p.FloorNum + 1).ToList());
            await _domainService.Update(siloInfo);
            return Success();
        }

        public async Task<ResponseDto<WorkOrderAndPanelDto>> GetPanelInfo(SiloAndPanelReq req)
        {
            if (req == null || string.IsNullOrEmpty(req.PanelCode)) return Fail<WorkOrderAndPanelDto>("参数异常!");

            var panelInfo = await _workOrderAndPanelService.GetWorkOrderAndPanelInfors(new GetWorkOrderAndPanelInforsReq()
            {
                PanelCode = req.PanelCode
            });

            return Success(panelInfo?.FirstOrDefault());
        }

        public async Task<ResponseDto<BindSingleResultDto>> IsBindSingle(SiloAndPanelBindReq req)
        {
            try
            {
                if (req == null
                    || string.IsNullOrEmpty(req.PanelCode)
                    || req.FloorNum < 0
                    )
                    return Fail<BindSingleResultDto>("参数异常!");

                if (string.IsNullOrEmpty(req.LocationCode))
                    return Fail<BindSingleResultDto>("库位编号不能为空!");

                // 检查 location_detail 表中是否存在该板料编码
                var existingRecords = await _locationDetailRepository.QueryAsync(p => p.PanelCode == req.PanelCode, null, OrderByType.Asc);

                if (existingRecords != null && existingRecords.Any())
                {
                    return Fail<BindSingleResultDto>("当前板料已绑定过库位", new BindSingleResultDto
                    {
                        AlertLevel = AlertLevel.Warning,
                    });
                }

                // 检查通过后直接调用 BindSingle 方法
                return await BindSingle(req);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "IsBindSingle 方法执行失败: {Message}", ex.Message);
                return Fail<BindSingleResultDto>($"检查板料绑定状态失败: {ex.Message}");
            }
        }

        public async Task<ResponseDto<BindSingleResultDto>> BindSingle(SiloAndPanelBindReq req)
        {
            try
            {
                if (req == null
                    || string.IsNullOrEmpty(req.PanelCode)
                    || req.FloorNum < 0
                    )
                    return Fail<BindSingleResultDto>("参数异常!");

                if (string.IsNullOrEmpty(req.LocationCode))
                    return Fail<BindSingleResultDto>("库位编号不能为空!");

                var targetFloorNum = req.FloorNum.Value;

                var panelInfo = (await _panelService.GetList(new GetPanelListReq() { PanelCode = req.PanelCode }))?.Data.List.FirstOrDefault();
                if (panelInfo == null) return Fail<BindSingleResultDto>("没有查到板料信息", new BindSingleResultDto
                {
                    AlertLevel = AlertLevel.Error,
                });

                // 清空该板料的所有库位绑定记录（保留 floor_num）
                try
                {
                    // 先查询出要清空的记录
                    var existingRecords = await _locationDetailRepository.QueryAsync(p => p.PanelCode == req.PanelCode, null, OrderByType.Asc);
                    if (existingRecords != null && existingRecords.Any())
                    {
                        // 清空记录数据，保留 floor_num
                        foreach (var record in existingRecords)
                        {
                            await _locationDetailRepository.UpdateAsync(p => new LocationDetail()
                            {
                                PanelCode = string.Empty,
                                ItemCode = string.Empty,
                                ProductStatus = ProductStatus.EmptySiloBox, // 空状态
                                Pcs = null,
                                PanelWidth = null,
                                PanelLength = null,
                                PinOffset = null,
                                Panel = string.Empty,
                                ModifierId = UserId,
                                ModifyTime = DateTime.Now
                            }, p => p.Id == record.Id);
                        }
                        _logger.LogInformation($"已清空板料 {req.PanelCode} 的 {existingRecords.Count} 条库位绑定记录");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "清空板料绑定记录失败: {Message}", ex.Message);
                    return Fail<BindSingleResultDto>($"清空板料绑定记录失败: {ex.Message}");
                }

                // 清空目标层的现有数据（如果有其他板料）
                try
                {
                    var targetLayerRecords = await _locationDetailRepository.QueryAsync(p => p.Code == req.LocationCode && p.FloorNum == targetFloorNum, null, OrderByType.Asc);
                    if (targetLayerRecords != null && targetLayerRecords.Any())
                    {
                        // 清空记录数据，保留 floor_num
                        foreach (var record in targetLayerRecords)
                        {
                            await _locationDetailRepository.UpdateAsync(p => new LocationDetail()
                            {
                                PanelCode = string.Empty,
                                ItemCode = string.Empty,
                                ProductStatus = ProductStatus.EmptySiloBox, // 空状态
                                Pcs = null,
                                PanelWidth = null,
                                PanelLength = null,
                                PinOffset = null,
                                Panel = string.Empty,
                                ModifierId = UserId,
                                ModifyTime = DateTime.Now
                            }, p => p.Id == record.Id);
                        }
                        _logger.LogInformation($"已清空库位 {req.LocationCode} 第 {targetFloorNum} 层的现有数据");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "清空目标层数据失败: {Message}", ex.Message);
                    return Fail<BindSingleResultDto>($"清空目标层数据失败: {ex.Message}");
                }

                // 更新目标层的绑定关系
                try
                {
                    await UpdateLocationDetailSingle(req, panelInfo, targetFloorNum);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "更新库位绑定记录失败: {Message}", ex.Message);
                    return Fail<BindSingleResultDto>($"更新库位绑定记录失败: {ex.Message}");
                }

                // 更新 panel 表的 locationCode
                try
                {
                    await _sqlSugarScope.Updateable<Model.Entites.Mes.TracePanel>()
                        .SetColumns(p => new Model.Entites.Mes.TracePanel()
                        {
                            LocationCode = req.LocationCode,
                            ModifierId = UserId,
                            ModifyTime = DateTime.Now
                        })
                        .Where(p => p.PanelCode == req.PanelCode)
                        .ExecuteCommandAsync();

                    _logger.LogInformation($"已更新板料 {req.PanelCode} 的 locationCode 为 {req.LocationCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "更新板料 locationCode 失败: {Message}", ex.Message);
                    return Fail<BindSingleResultDto>($"更新板料 locationCode 失败: {ex.Message}");
                }

                // 返回绑定结果
                var result = new BindSingleResultDto
                {
                    LocationCode = req.LocationCode
                };

                // 获取当前库位上所有的板料信息
                try
                {
                    result.LocationDetails = await GetLocationPanelDetailsByCode(req.LocationCode);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "获取库位板料信息失败: {Message}", ex.Message);
                    return Fail<BindSingleResultDto>($"获取库位板料信息失败: {ex.Message}");
                }

                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BindSingle 方法执行失败: {Message}", ex.Message);
                return Fail<BindSingleResultDto>($"操作失败: {ex.Message}");
            }
        }

        public async Task<ResponseDto<string>> BulkInsert(List<SiloExcelDto> req)
        {
            if (req == null || req.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<SiloDetail> siloDetails = new List<SiloDetail>();
            List<Silo> silos = new List<Silo>();
            List<string> deleteSiloDetails = new List<string>();
            foreach (var item in req)
            {
                List<int> floorNum = new List<int>();
                if (string.IsNullOrEmpty(item.Code))
                {
                    sb.Append("编码：" + item.Code + " 为空；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                var isExsitCode = await _domainService.IsExistAsync(p => p.Code == item.Code);
                if (isExsitCode)
                {
                    sb.Append("编码" + item.Code + " 数据库已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                if (silos.Exists(p => p.Code == item.Code))
                {
                    sb.Append("编码" + item.Code + " 导入列表中已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                for (int i = 0; i < item.FloorCount; i++)
                {
                    var siloDetail = new SiloDetail()
                    {
                        SiloCode = item.Code,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        FloorNum = i,
                        Status = 1
                    };

                    floorNum.Add(i + 1);
                    siloDetails.Add(siloDetail);
                }
                deleteSiloDetails.Add(item.Code);

                var silo = _mapper.Map<Silo>(item);
                silo.EmptySilo = System.Text.Json.JsonSerializer.Serialize(floorNum);
                silo.CreateTime = DateTime.Now;
                silo.CreatorId = UserId;
                silo.Status = (int)DataStatusEnum.Enable;
                silos.Add(silo);
            }

            await _siloDetailDomainService.DeleteAsync(p => deleteSiloDetails.Contains(p.SiloCode));
            await _siloDetailDomainService.BulkInsert(siloDetails);
            await _domainService.BulkInsert(silos);

            string message = string.Format("预计导入：{0} 条；成功导入：{1} 条；失败：{2} 条；\r\n", req.Count, silos.Count, failCount);
            message += sb.ToString();

            return Success(message);
        }

        public async Task<ResponseDto<string>> SetManual(ExternalSetSiloStatusReq req)
        {
            if (req == null)
            {
                return Fail("空参数异常!");
            }

            var preCheck = await CheckExternalSetSiloStatusReq(req.SiloCode);
            if (preCheck.Code == ResponseCode.Fail)
            {
                return preCheck;
            }

            var silo = await _siloDomainService.FindSingleAsync(s => s.Code.ToLower() == req.SiloCode.ToLower());
            if (silo.SiloStatus == SiloStatus.Auto)
            {
                _logger.LogError($"As the silo [{silo.Code.ToLower()}] status is [{silo.SiloStatus}], can not be set to Manual");
                return Fail($"As the silo [{silo.Code.ToLower()}] status is [{silo.SiloStatus}], can not be set to Manual");
            }
            if (string.IsNullOrEmpty(silo.Location))
            {
                _logger.LogError($"As the silo [{silo.Code.ToLower()}] Location is empty, can not be set to Manual");
                return Fail($"As the silo [{silo.Code.ToLower()}] Location is empty, can not be set to Manual");
            }

            silo.SiloStatus = SiloStatus.Manual;
            silo.ModifyTime = DateTime.Now;
            var updateResult = await _siloDomainService.Update(silo);
            if (updateResult)
            {
                return Success();
            }
            else
            {
                _logger.LogError($"数据库更新时未成功，请重试");
                return Fail($"数据库更新时未成功，请重试");
            }
        }

        public async Task<ResponseDto<string>> SetReady(ExternalSetSiloStatusReq req)
        {
            if (req == null)
            {
                return Fail("异常!");
            }
            var preCheck = await CheckExternalSetSiloStatusReq(req.SiloCode);
            if (preCheck.Code == ResponseCode.Fail)
            {
                return preCheck;
            }

            var silo = await _siloDomainService.FindSingleAsync(s => s.Code.ToLower() == req.SiloCode.ToLower());
            if (silo.SiloStatus != SiloStatus.Manual
                && silo.SiloStatus != SiloStatus.None)
            {
                _logger.LogError($"As the silo [{silo.Code.ToLower()}] status is [{silo.SiloStatus}], can not be set to Ready");
                return Fail($"As the silo [{silo.Code.ToLower()}] status is [{silo.SiloStatus}], can not be set to Ready");
            }

            //调用中控指令，下发料仓信息
            var sendResult = await SendSiloPanelsToDevice(silo);
            if (!sendResult)
            {
                _logger.LogError($"下发料仓信息失败，请稍后再试");
                return Fail($"下发料仓信息失败，请稍后再试");
            }

            silo.SiloStatus = SiloStatus.Ready;
            silo.ModifyTime = DateTime.Now;
            var updateResult = await _siloDomainService.Update(silo);
            if (updateResult)
            {
                return Success();
            }
            else
            {
                return Fail($"数据库更新时未成功，请重试");
            }
        }
        public async Task<SiloStatus> GetSiloStatus(string siloCode)
        {
            if (await _siloDomainService.IsExistAsync(d => d.Code.ToLower() == siloCode.ToLower()))
            {
                var silo = await _siloDomainService.FindSingleAsync(d => d.Code.ToLower() == siloCode.ToLower());
                return silo.SiloStatus;
            }
            else
            {
                return SiloStatus.None;
            }
        }

        public async Task<ResponseDto<string>> SetAuto(ExternalSetSiloStatusReq req)
        {
            if (req == null)
            {
                return Fail("异常!");
            }
            var preCheck = await CheckExternalSetSiloStatusReq(req.SiloCode);
            if (preCheck.Code == ResponseCode.Fail)
            {
                return preCheck;
            }

            var silo = await _siloDomainService.FindSingleAsync(s => s.Code.ToLower() == req.SiloCode.ToLower());
            if (silo.SiloStatus != SiloStatus.Ready)
            {
                _logger.LogError($"As the silo [{silo.Code.ToLower()}] status is [{silo.SiloStatus}], can not be set to Auto");
                return Fail($"As the silo [{silo.Code.ToLower()}] status is [{silo.SiloStatus}], can not be set to Auto");
            }
            if (string.IsNullOrWhiteSpace(silo.Location))
            {
                _logger.LogError($"As the silo [{silo.Code.ToLower()}] Location is empty, can not be set to Auto");
                return Fail($"As the silo [{silo.Code.ToLower()}] Location is empty, can not be set to Auto");
            }

            silo.SiloStatus = SiloStatus.Auto;
            silo.ModifyTime = DateTime.Now;
            var updateResult = await _siloDomainService.Update(silo);
            if (updateResult)
            {
                return Success();
            }
            else
            {
                return Fail($"数据库更新时未成功，请重试");
            }
        }
        private async Task<ResponseDto<string>> CheckExternalSetSiloStatusReq(string siloCode)
        {
            if (string.IsNullOrEmpty(siloCode))
            {
                return Fail("参数siloCode异常!");
            }

            if (!(await _siloDomainService.IsExistAsync(s => s.Code.ToLower() == siloCode.ToLower())))
            {
                return Fail($"不存在此料仓，silo code [{siloCode}]!");
            }

            return Success();
        }
        private async Task<bool> SendSiloPanelsToDevice(Silo silo)
        {
            bool mockTest = true;
            if (_configuration["AppConfig:MockSettingSilo"] != null)
            {
                if (_configuration["AppConfig:MockSettingSilo"].ToLower() == "true")
                {
                    mockTest = true;
                }
                else
                {
                    mockTest = false;
                }
            }
            if (mockTest)
            {
                return true;
            }
            if (string.IsNullOrWhiteSpace(silo.Location))
            {
                _logger.LogError($"As the silo [{silo.Code.ToLower()}] Location is empty, can not be set to Ready");
                return false;
            }
            //var panels = await GetSiloPanels(silo.Code, silo.FloorCount);
            var urlAddress = _configuration["AppConfig:ReBindSilo"];
            if (string.IsNullOrEmpty(urlAddress) || string.IsNullOrWhiteSpace(silo.Location))
            {
                return false;
            }
            if (!(await _rackDomainService.IsExistAsync(x => x.Code.ToLower() == silo.Location.ToLower())))
            {
                return false;
            }

            var rack = await _rackDomainService.FindSingleAsync(x => x.Code.ToLower() == silo.Location.ToLower());
            if (rack == null || string.IsNullOrEmpty(rack.RelateDeviceCode))
            {
                return false;
            }

            var queryCondition = $"?siloCode={silo.Code}&deviceId={rack.RelateDeviceCode}&rackCode={silo.Location}";
            var result = _apiHelper.RequestData(urlAddress + queryCondition);
            if (string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ResponseDto<List<SiloDetailDto>>> MovePanelToOtherSilo(MovePanelToOtherSiloReq req)
        {
            if (req == null)
            {
                return Fail<List<SiloDetailDto>>("未识别有效的入参!");
            }
            if (string.IsNullOrEmpty(req.PanelCode) || string.IsNullOrEmpty(req.TargetSiloCode))
            {
                return Fail<List<SiloDetailDto>>("未识别有效的PanelCode、TargetSiloCode!");
            }

            SiloDetail siloDetail = new SiloDetail();
            if (await _siloDetailDomainService.IsExistAsync(p => p.PanelCode.ToLower() == req.PanelCode.ToLower()))
            {
                siloDetail = await _siloDetailDomainService.FindSingleAsync(p => p.PanelCode.ToLower() == req.PanelCode.ToLower());
            }

            if (siloDetail == null)
            {
                siloDetail.PanelCode = req.PanelCode;
            }
            else
            {
                await _siloDetailDomainService.UpdateAsync(p => new SiloDetail
                {
                    PanelCode = null,
                    ItemCode = null,
                    PanelWidth = null,
                    PanelLength = null,
                    Pcs = null,
                    PinOffset = null,
                    ProductStatus = ProductStatus.EmptySiloBox,
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now,
                }, p => p.PanelCode.ToLower() == req.PanelCode.ToLower());
            }

            await _siloDetailDomainService.UpdateAsync(p => new SiloDetail
            {
                PanelCode = siloDetail.PanelCode,
                ItemCode = siloDetail.ItemCode,
                PanelWidth = siloDetail.PanelWidth,
                PanelLength = siloDetail.PanelLength,
                Pcs = siloDetail.Pcs,
                PinOffset = siloDetail.PinOffset,
                ProductStatus = siloDetail.ProductStatus,
                ModifierId = UserId,
                ModifyTime = DateTime.Now,
            }, p => p.SiloCode.ToLower() == req.TargetSiloCode.ToLower() && p.FloorNum == req.TargetFloor);

            var returnData = await GetSiloDetails(new SiloDetailQueryReq
            {
                SiloCode = req.TargetSiloCode
            });

            return returnData;
        }

        public async Task<ResponseDto<string>> AgvBindSilo(string deviceCode, string siloCode)
        {
            if (string.IsNullOrEmpty(deviceCode))
            {
                return Fail("未识别有效的设备!");
            }

            if (!string.IsNullOrEmpty(siloCode))
            {
                // 检查AGV多料仓绑定开关配置
                var allowAgvMultiSilo = _innerOptions.Value.AllowAgvMultiSilo;

                // 如果开关关闭，执行原有的卡控逻辑
                if (!allowAgvMultiSilo)
                {
                    if (await _rackDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.ToLower() == siloCode.ToLower()))
                    {
                        return Fail("该料仓已绑定其他库位!");
                    }

                    if (await _domainService.IsExistAsync(p => p.Code.ToLower() == siloCode.ToLower()
                    && !string.IsNullOrEmpty(p.Location) && p.Location.ToLower() != deviceCode.ToLower()))
                    {
                        return Fail("该料仓已绑定其他库位!");
                    }
                }
                // 如果开关打开，只检查料仓是否存在，不检查绑定关系
                else
                {
                    if (!await _domainService.IsExistAsync(p => p.Code.ToLower() == siloCode.ToLower()))
                    {
                        return Fail($"料仓 {siloCode} 不存在!");
                    }
                }

                // 如果开关关闭，清空设备原有绑定的料仓（原有逻辑）
                if (!allowAgvMultiSilo)
                {
                    await _siloDomainService.UpdateAsync(p => new Silo
                    {
                        Location = string.Empty,
                        ModifierId = UserId,
                        ModifyTime = DateTime.Now,
                    }, p => !string.IsNullOrEmpty(p.Location) && p.Location.ToLower() == deviceCode.ToLower());
                }

                // 更新料仓的Location绑定
                var siloData = await _siloDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == siloCode.ToLower());
                if (siloData != null)
                {
                    // 如果开关关闭，直接设置Location（原有逻辑）
                    if (!allowAgvMultiSilo)
                    {
                        siloData.Location = deviceCode;
                    }
                    // 如果开关打开，允许料仓切换库位
                    else
                    {
                        // 如果料仓已有其他库位绑定，先清空原有绑定
                        if (!string.IsNullOrEmpty(siloData.Location) && siloData.Location.ToLower() != deviceCode.ToLower())
                        {
                            // 清空原库位绑定的料仓
                            await _siloDomainService.UpdateAsync(p => new Silo
                            {
                                Location = string.Empty,
                                ModifierId = UserId,
                                ModifyTime = DateTime.Now,
                            }, p => !string.IsNullOrEmpty(p.Location) && p.Location.ToLower() == siloData.Location.ToLower());
                        }

                        // 设置新的库位绑定
                        siloData.Location = deviceCode;
                    }

                    siloData.ModifierId = UserId;
                    siloData.ModifyTime = DateTime.Now;
                    await _siloDomainService.Update(siloData);
                }
            }
            else
            {
                DeviceCommandCentralRequest commandRequest = new DeviceCommandCentralRequest
                {
                    DeviceId = deviceCode,
                    Command = "RemoveSiloCommand",
                    Params = new Dictionary<string, object?> { { "DeviceCode", deviceCode } }
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

                await _siloDomainService.UpdateAsync(p => new Silo
                {
                    Location = string.Empty,
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now,
                }, p => !string.IsNullOrEmpty(p.Location) && p.Location.ToLower() == deviceCode.ToLower());
            }

            return Success();
        }

        /// <summary>
        /// 插入 location_detail 表数据
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <param name="siloDetails">料仓明细列表</param>
        /// <returns></returns>
        private async Task InsertLocationDetailData(AddOrUpdateSiloDetailReq req, List<SiloDetail> siloDetails)
        {
            if (string.IsNullOrEmpty(req.LocationCode) || string.IsNullOrEmpty(req.SiloCode))
                return;

            // 根据 siloCode 查询该料仓的总层数
            var siloInfo = await _domainService.FindSingleAsync(p => p.Code == req.SiloCode);
            if (siloInfo == null)
            {
                _logger.LogWarning($"未找到料仓信息: {req.SiloCode}");
                return;
            }

            var siloFloorCount = siloInfo.FloorCount;
            _logger.LogInformation($"料仓 {req.SiloCode} 总层数: {siloFloorCount}");

            // 查询该库位现有的所有记录
            var existingLocationDetails = await _locationDetailRepository.GetByCodeAsync(req.LocationCode);

            // 获取本次请求涉及的层号范围
            var requestFloorNums = siloDetails.Select(sd => sd.FloorNum).ToList();
            _logger.LogInformation($"本次请求涉及的层号: {string.Join(", ", requestFloorNums)}");

            // 智能合并策略：保留旧数据，更新新数据
            List<LocationDetail> locationDetails = new List<LocationDetail>();

            // 根据料仓总层数生成对应数量的 location_detail 记录
            for (int i = 0; i < siloFloorCount; i++)
            {
                // 获取对应层数的 siloDetail 数据（如果存在）
                var siloDetail = siloDetails.FirstOrDefault(sd => sd.FloorNum == i);

                // 获取该层的旧数据（如果存在）
                var oldLocationDetail = existingLocationDetails?.FirstOrDefault(ed => ed.FloorNum == i);

                LocationDetail locationDetail;

                // 如果本次请求涉及该层，使用新数据
                if (siloDetail != null)
                {
                    locationDetail = new LocationDetail
                    {
                        Code = req.LocationCode,
                        Panel = siloDetail.PanelCode ?? string.Empty,
                        ItemCode = siloDetail.ItemCode ?? string.Empty,
                        FloorNum = i,
                        PanelCode = siloDetail.PanelCode,
                        ProductStatus = siloDetail.ProductStatus,
                        Pcs = siloDetail.Pcs,
                        PanelWidth = siloDetail.PanelWidth,
                        PanelLength = siloDetail.PanelLength,
                        PinOffset = siloDetail.PinOffset,
                        Status = 1,
                        IsDeleted = 0,
                        CreateTime = oldLocationDetail?.CreateTime ?? DateTime.Now,
                        CreatorId = oldLocationDetail?.CreatorId ?? UserId,
                        ModifyTime = DateTime.Now,
                        ModifierId = UserId
                    };
                    _logger.LogInformation($"FloorNum {i}: 使用新数据 (ItemCode: {siloDetail.ItemCode})");
                }
                // 如果本次请求不涉及该层，保留旧数据
                else if (oldLocationDetail != null)
                {
                    locationDetail = new LocationDetail
                    {
                        Code = req.LocationCode,
                        Panel = oldLocationDetail.Panel ?? string.Empty,
                        ItemCode = oldLocationDetail.ItemCode ?? string.Empty,
                        FloorNum = i,
                        PanelCode = oldLocationDetail.PanelCode,
                        ProductStatus = oldLocationDetail.ProductStatus,
                        Pcs = oldLocationDetail.Pcs,
                        PanelWidth = oldLocationDetail.PanelWidth,
                        PanelLength = oldLocationDetail.PanelLength,
                        PinOffset = oldLocationDetail.PinOffset,
                        Status = oldLocationDetail.Status,
                        IsDeleted = oldLocationDetail.IsDeleted,
                        CreateTime = oldLocationDetail.CreateTime,
                        CreatorId = oldLocationDetail.CreatorId,
                        ModifyTime = DateTime.Now,  // 更新时间戳
                        ModifierId = UserId
                    };
                    _logger.LogInformation($"FloorNum {i}: 保留旧数据 (ItemCode: {oldLocationDetail.ItemCode})");
                }
                // 如果本次请求不涉及该层，且没有旧数据，使用默认值
                else
                {
                    locationDetail = new LocationDetail
                    {
                        Code = req.LocationCode,
                        Panel = string.Empty,
                        ItemCode = string.Empty,
                        FloorNum = i,
                        PanelCode = null,
                        ProductStatus = ProductStatus.EmptySiloBox,
                        Pcs = null,
                        PanelWidth = null,
                        PanelLength = null,
                        PinOffset = null,
                        Status = 1,
                        IsDeleted = 0,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        ModifyTime = DateTime.Now,
                        ModifierId = UserId
                    };
                    _logger.LogInformation($"FloorNum {i}: 使用默认值（空层）");
                }

                locationDetails.Add(locationDetail);
            }

            // 先删除该库位的所有旧记录
            if (existingLocationDetails.Any())
            {
                await _locationDetailRepository.DeleteAsync(p => p.Code == req.LocationCode);
                _logger.LogInformation($"已删除库位 {req.LocationCode} 的 {existingLocationDetails.Count} 条旧记录");
            }

            // 批量插入 location_detail 数据（包含旧数据和新数据）
            if (locationDetails.Any())
            {
                await _locationDetailRepository.BulkInsert(locationDetails);
                _logger.LogInformation($"成功插入 {locationDetails.Count} 条 location_detail 数据，LocationCode: {req.LocationCode}，基于料仓 {req.SiloCode} 的 {siloFloorCount} 层");
            }
        }

        /// <summary>
        /// 更新单个 location_detail 记录
        /// </summary>
        /// <param name="req">绑定请求</param>
        /// <param name="panelInfo">板料信息</param>
        /// <param name="targetFloorNum">目标层数（从0开始）</param>
        private async Task UpdateLocationDetailSingle(SiloAndPanelBindReq req, PanelDto panelInfo, int targetFloorNum)
        {
            if (string.IsNullOrEmpty(req.LocationCode))
                return;

            // 更新指定层的 location_detail 记录
            await _locationDetailRepository.UpdateAsync(p => new LocationDetail()
            {
                PanelCode = req.PanelCode,
                ItemCode = panelInfo.ItemCode,
                ProductStatus = panelInfo.ProductStatus,
                Pcs = panelInfo.Pcs,
                PanelWidth = panelInfo.PanelWidth,
                PanelLength = (decimal?)panelInfo.PanelLength, // 转换 float 到 decimal?
                PinOffset = panelInfo.PinOffset,
                Panel = req.PanelCode ?? string.Empty,
                ModifierId = UserId,
                ModifyTime = DateTime.Now
            }, p => p.Code == req.LocationCode && p.FloorNum == targetFloorNum);

            _logger.LogInformation($"已更新库位 {req.LocationCode} 第 {targetFloorNum} 层的板料信息: {req.PanelCode}");
        }


        /// <summary>
        /// 根据库位编号获取所有板料信息
        /// </summary>
        /// <param name="locationCode">库位编号</param>
        /// <returns>库位板料明细列表</returns>
        private async Task<List<LocationPanelDetailDto>> GetLocationPanelDetailsByCode(string locationCode)
        {
            var locationDetails = await _locationDetailRepository.GetByCodeAsync(locationCode);
            var result = new List<LocationPanelDetailDto>();

            foreach (var locationDetail in locationDetails.OrderBy(x => x.FloorNum))
            {
                // 从 panel 表获取 product_status 信息
                ProductStatus panelProductStatus = ProductStatus.EmptySiloBox;
                string productStatusDesc = "0";

                if (!string.IsNullOrEmpty(locationDetail.PanelCode))
                {
                    var panelInfo = (await _panelService.GetList(new GetPanelListReq() { PanelCode = locationDetail.PanelCode }))?.Data.List.FirstOrDefault();
                    if (panelInfo != null)
                    {
                        panelProductStatus = panelInfo.ProductStatus;
                        productStatusDesc = ((int)panelInfo.ProductStatus).ToString(); // 转换为数字字符串
                    }
                }

                result.Add(new LocationPanelDetailDto
                {
                    PanelCode = locationDetail.PanelCode,
                    SiloCode = null, // 不再需要 siloCode
                    FloorNum = locationDetail.FloorNum, // 直接使用数据库中的层数
                    ItemCode = locationDetail.ItemCode,
                    PanelWidth = locationDetail.PanelWidth,
                    PanelLength = locationDetail.PanelLength,
                    PinOffset = locationDetail.PinOffset,
                    Pcs = locationDetail.Pcs,
                    ProductStatus = (int)panelProductStatus, // 转换为 int 值
                    ProductStatusDesc = productStatusDesc // 使用 panel 表的 product_status 数值
                });
            }

            return result;
        }

        /// <summary>
        /// 解析产品状态字符串为枚举
        /// </summary>
        /// <param name="productStatusStr">产品状态字符串</param>
        /// <returns>产品状态枚举</returns>
        private ProductStatus ParseProductStatus(string? productStatusStr)
        {
            if (string.IsNullOrEmpty(productStatusStr))
                return ProductStatus.EmptySiloBox;

            if (int.TryParse(productStatusStr, out int statusValue))
            {
                if (Enum.IsDefined(typeof(ProductStatus), statusValue))
                {
                    return (ProductStatus)statusValue;
                }
            }

            return ProductStatus.EmptySiloBox;
        }


        /// <summary>
        /// 记录板料下发时的板料追溯信息
        /// </summary>
        /// <param name="req">下发请求</param>
        /// <returns></returns>
        private async Task RecordPanelTraceForAllots(AllotsPanelDataReq req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.RackCode))
                {
                    return;
                }

                var locationDetailsWithSilo = await _locationDetailRepository.GetByCodeWithSiloCodeAsync(req.RackCode);
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
                            $"板料下发",
                            null,
                            null
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "板料下发时追溯记录失败 - 设备编码: {DeviceCode}, 料架编码: {RackCode}", req.DeviceCode, req.RackCode);
            }
        }

    }
}
