using AutoMapper;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using SqlSugar.Extensions;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class DrillPanelDetailService : BaseServiceWithoutTree<DrillPanelDetail, DrillPanelDetailDto, AddOrUpdateDrillPanelDetailReq>, IDrillPanelDetailService
    {
        private readonly IDrillPanelDetailDomainService _drillPanelDetailDomainService;
        private readonly IDeviceDomainService _deviceDomainService;
        private readonly IEncodeBuildRulesService _encodeService;
        private readonly IProBoardTraceDomainService _panelDomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlSugarScope _sqlSugarScope;
        private readonly IPanelService _panelService;
        private readonly ISiloDetailDomainService _siloDetailDomainService;
        private readonly ISiloDomainService _siloDomainService;
        private readonly ISiloService _siloService;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        private readonly IConfiguration _configuration;
        private readonly IRackDomainService _rackDomainService;
        private readonly ITaskDomainService _taskDomainService;
        private readonly IDrillRateFactorDomainService _drillRateFactorDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IDeviceRecordsService _deviceRecordsService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public DrillPanelDetailService(
            IDrillPanelDetailDomainService domainService,
            IDeviceDomainService deviceDomainService,
            IEncodeBuildRulesService encodeService,
            IProBoardTraceDomainService panelDomainService,
            IUnitOfWork unitOfWork,
            IPanelService panelService,
            ISiloDetailDomainService siloDetailDomainService,
            ISiloDomainService siloDomainService,
            ISiloService siloService,
            ICentralOnlineDevice centralOnlineDevice,
            IConfiguration configuration,
            IRackDomainService rackDomainService,
            ITaskDomainService taskDomainService,
            IDrillRateFactorDomainService drillRateFactorDomainService,
            ISysConfigManager sysConfigManager,
            IDeviceRecordsService deviceRecordsService,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _drillPanelDetailDomainService = domainService;
            _deviceDomainService = deviceDomainService;
            _encodeService = encodeService;
            _panelDomainService = panelDomainService;
            _unitOfWork = unitOfWork;
            _sqlSugarScope = unitOfWork.GetDbClient();
            _panelService = panelService;
            _siloDetailDomainService = siloDetailDomainService;
            _siloDomainService = siloDomainService;
            _siloService = siloService;
            _centralOnlineDevice = centralOnlineDevice;
            _configuration = configuration;
            _rackDomainService = rackDomainService;
            _taskDomainService = taskDomainService;
            _drillRateFactorDomainService = drillRateFactorDomainService;
            _sysConfigManager = sysConfigManager;
            _deviceRecordsService = deviceRecordsService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DrillPanelDetailDto>>> GetList(GetDrillPanelDetailListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DrillPanelDetailDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DrillPanelDetail>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DeviceCode) && p.DeviceCode.Contains(req.DeviceCode));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.PanelCode) && p.PanelCode.Contains(req.PanelCode));
            }
            if (req.ProductStatus.HasValue)
            {
                where = where.And(p => p.ProductStatus == req.ProductStatus);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DrillPanelDetail>, List<DrillPanelDetailDto>>(result.ToList());
            return Success(pageDto);
        }

        public async Task<ResponseDto<List<DrillPanelDetailInfo>>> GetPanelList(string deviceCode)
        {
            if (string.IsNullOrEmpty(deviceCode))
            {
                return Fail<List<DrillPanelDetailInfo>>("未识别有效的DeivceCode!");
            }

            var data = await _domainService.QueryAsync(p => p.DeviceCode.ToLower() == deviceCode.ToLower(), p => p.Id, OrderByType.Asc);
            if (data == null || data.Count == 0)
            {
                return Success(new List<DrillPanelDetailInfo>());
            }

            List<DrillPanelDetailInfo> result = new List<DrillPanelDetailInfo>();
            var layer = data.Select(x => x.Layer).Distinct().ToList();
            if (layer == null || layer.Count == 0)
            {
                return Success(result);
            }

            foreach (var item in layer)
            {
                var panelDetails = data.Where(p => p.Layer == item).ToList();
                result.Add(new DrillPanelDetailInfo
                {
                    Layer = item,
                    PanelDetailDtos = _mapper.Map<List<DrillPanelDetail>, List<DrillPanelDetailDto>>(panelDetails)
                });
            }

            return Success(result);
        }

        public async Task<ResponseDto<string>> AddData(AddOrUpdateDrillPanelDetailReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的数据！");
            }
            if (string.IsNullOrEmpty(req.DeviceCode) || string.IsNullOrEmpty(req.PanelCode) || req.SplindleIndex == null || req.SplindleIndex == 0)
            {
                return Fail("未识别有效的DeviceCode、PanelCode或SplindleIndex！");
            }

            if (await _drillPanelDetailDomainService.IsExistAsync(p => p.DeviceCode.ToLower() == req.DeviceCode.ToLower()
                && p.Layer == req.Layer && p.SplindleIndex == req.SplindleIndex))
            {
                return Fail($"{req.DeviceCode} {req.Layer} {req.SplindleIndex} 已存在！");
            }

            var panelInfo = (await _panelService.GetList(new GetPanelListReq() { PanelCode = req.PanelCode }))?.Data.List.FirstOrDefault();
            if (panelInfo == null) return Fail("没有查到板料信息");

            if (await _drillPanelDetailDomainService.IsExistAsync(p => p.PanelCode.ToLower() == req.PanelCode.ToLower() && p.IsDeleted == 0))
            {
                return Fail($"{req.PanelCode} 该板料已存在！");
            }

            var model = _mapper.Map<DrillPanelDetail>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;

            await _drillPanelDetailDomainService.Add(model);

            return Success();
        }

        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateDrillPanelDetailReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的数据！");
            }
            if (string.IsNullOrEmpty(req.DeviceCode) || string.IsNullOrEmpty(req.PanelCode) || req.SplindleIndex == null || req.SplindleIndex == 0)
            {
                return Fail("未识别有效的DeviceCode、PanelCode或SplindleIndex！");
            }

            var data = await _drillPanelDetailDomainService.QueryByID(req.Id);
            if (data == null)
            {
                return Fail("未找到数据！");
            }

            var panelInfo = (await _panelService.GetList(new GetPanelListReq() { PanelCode = req.PanelCode }))?.Data.List.FirstOrDefault();
            if (panelInfo == null) return Fail("没有查到板料信息");

            if (await _drillPanelDetailDomainService.IsExistAsync(p => p.PanelCode.ToLower() == req.PanelCode.ToLower() && p.Id != req.Id && p.IsDeleted == 0))
            {
                return Fail($"{req.PanelCode} 该板料已存在！");
            }

            var model = _mapper.Map<DrillPanelDetail>(req);
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            model.CreatorId = data.CreatorId;
            model.CreateTime = data.CreateTime;

            await _drillPanelDetailDomainService.Update(model);

            return Success();
        }

        public async Task<ResponseDto<string>> BatchLoadPanelDetailData()
        {
            var deviceList = await _deviceDomainService.QueryAsync(p => p.DeviceTypeCode.ToLower().Contains("drill"), p => p.Code, OrderByType.Asc);
            if (deviceList == null || deviceList.Count == 0)
            {
                return Success();
            }

            List<DrillPanelDetail> addList = new List<DrillPanelDetail>();
            List<string> deleteCodes = new List<string>();
            foreach (var deviceData in deviceList)
            {
                if (deviceData.InteractionPosition == null || deviceData.SpindleNum == null || deviceData.SpindleNum == 0)
                {
                    continue;
                }

                if (await _drillPanelDetailDomainService.IsExistAsync(p => p.DeviceCode.ToLower() == deviceData.Code.ToLower()))
                {
                    var panelDetails = await _drillPanelDetailDomainService.QueryAsync(p => p.DeviceCode.ToLower() == deviceData.Code.ToLower()
                    , p => p.DeviceCode, OrderByType.Asc);

                    if (deviceData.InteractionPosition == InteractionPosition.Front)
                    {
                        if (panelDetails.Count != deviceData.SpindleNum)
                        {
                            deleteCodes.Add(deviceData.Code.ToLower());
                            addList.AddRange(await SetPanelDetailData(new LoadDrillPanelDetailReq
                            {
                                DeviceCode = deviceData.Code,
                            }, (int)deviceData.SpindleNum, InteractionPosition.Front));
                        }
                    }
                    else
                    {
                        if (panelDetails.Count != deviceData.SpindleNum * 3)
                        {
                            deleteCodes.Add(deviceData.Code.ToLower());
                            addList.AddRange(await SetPanelDetailData(new LoadDrillPanelDetailReq
                            {
                                DeviceCode = deviceData.Code,
                            }, (int)deviceData.SpindleNum, InteractionPosition.Rear));
                        }
                    }
                }
                else
                {
                    addList.AddRange(await SetPanelDetailData(new LoadDrillPanelDetailReq
                    {
                        DeviceCode = deviceData.Code,
                    }, (int)deviceData.SpindleNum, (InteractionPosition)deviceData.InteractionPosition));
                }
            }
            _unitOfWork.BeginTran();

            await _drillPanelDetailDomainService.DeleteAsync(p => deleteCodes.Contains(p.DeviceCode.ToLower()));

            await _drillPanelDetailDomainService.BulkInsert(addList);

            _unitOfWork.CommitTran();

            return Success();
        }

        public async Task<ResponseDto<string>> LoadPanelDetailData(LoadDrillPanelDetailReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的数据！");
            }
            if (string.IsNullOrEmpty(req.DeviceCode))
            {
                return Fail("未识别有效的DeviceCode！");
            }

            var deviceData = await _deviceDomainService.FindSingleAsync(p => p.Code.ToLower() == req.DeviceCode.ToLower());
            if (deviceData == null)
            {
                return Fail("无法识别设备！");
            }

            if (deviceData.InteractionPosition == null || deviceData.SpindleNum == null || deviceData.SpindleNum == 0)
            {
                return Fail("无法识别设备的交互位置和轴数！");
            }

            var addList = await SetPanelDetailData(req, (int)deviceData.SpindleNum, (InteractionPosition)deviceData.InteractionPosition);

            _unitOfWork.BeginTran();

            await _drillPanelDetailDomainService.DeleteAsync(p => p.DeviceCode.ToLower() == req.DeviceCode.ToLower());

            await _drillPanelDetailDomainService.BulkInsert(addList);

            _unitOfWork.CommitTran();

            return Success();
        }

        private async Task<List<DrillPanelDetail>> SetPanelDetailData(LoadDrillPanelDetailReq req, int spindleNum, InteractionPosition interactionPosition)
        {
            List<DrillPanelDetail> addList = new List<DrillPanelDetail>();

            for (int i = 0; i < spindleNum; i++)
            {
                addList.Add(new DrillPanelDetail
                {
                    DeviceCode = req.DeviceCode,
                    ItemCode = req.ItemCode,
                    Pcs = req.Pcs,
                    PanelWidth = req.PanelWidth,
                    Layer = 0,
                    SplindleIndex = i + 1,
                    Status = (int)DataStatusEnum.Enable,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                });
            }

            if (interactionPosition == InteractionPosition.Rear)
            {
                for (int i = 0; i < spindleNum; i++)
                {
                    addList.Add(new DrillPanelDetail
                    {
                        DeviceCode = req.DeviceCode,
                        ItemCode = req.ItemCode,
                        Pcs = req.Pcs,
                        PanelWidth = req.PanelWidth,
                        Layer = 1,
                        SplindleIndex = i + 1,
                        Status = (int)DataStatusEnum.Enable,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                    });
                }
                for (int i = 0; i < spindleNum; i++)
                {
                    addList.Add(new DrillPanelDetail
                    {
                        DeviceCode = req.DeviceCode,
                        ItemCode = req.ItemCode,
                        Pcs = req.Pcs,
                        PanelWidth = req.PanelWidth,
                        Layer = 2,
                        SplindleIndex = i + 1,
                        Status = (int)DataStatusEnum.Enable,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                    });
                }
            }

            return addList;
        }

        public async Task<ResponseDto<string>> BatchAddOrUpdateData(BatchAddOrUpdateDrillPanelDetailReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (req.Layer == null || string.IsNullOrEmpty(req.DeviceCode) || string.IsNullOrEmpty(req.ItemCode))
            {
                return Fail("未识别有效的Layer、DeviceCode或ItemCode！");
            }
            if (!req.ProductStatus.HasValue || req.ProductStatus == ProductStatus.EmptyPayload)
            {
                switch (req.Layer)
                {
                    case 0:
                        req.ProductStatus = ProductStatus.WaitingForDrill;
                        break;
                    case 1:
                        req.ProductStatus = ProductStatus.Drilling;
                        break;
                    case 2:
                        req.ProductStatus = ProductStatus.Finished_DRILL;
                        break;
                }
            }
            if (req.BeginSplindleNum == null || req.BeginSplindleNum == 0)
            {
                return Fail("未识别有效的BeginSplindleNum！");
            }
            if (req.SplindleCount == null || req.SplindleCount == 0)
            {
                return Fail("未识别有效的SplindleCount！");
            }

            var deviceData = await _deviceDomainService.FindSingleAsync(p => p.Code.ToLower() == req.DeviceCode.ToLower());
            if (deviceData == null)
            {
                return Fail($"无法识别设备 {req.DeviceCode}！");
            }

            if (deviceData.InteractionPosition == null || deviceData.SpindleNum == null || deviceData.SpindleNum == 0)
            {
                return Fail($"无法识别设备 {req.DeviceCode} 的交互位置和轴数！");
            }

            if (!await _drillPanelDetailDomainService.IsExistAsync(p => p.DeviceCode.ToLower() == req.DeviceCode.ToLower() && p.Layer == req.Layer))
            {
                await LoadPanelDetailData(new LoadDrillPanelDetailReq
                {
                    DeviceCode = req.DeviceCode
                });
            }

            var oldSiloDetails = await _drillPanelDetailDomainService.QueryAsync(p => p.DeviceCode.ToLower() == req.DeviceCode.ToLower()
                                        && p.Layer == req.Layer, p => p.DeviceCode, OrderByType.Asc);
            List<DrillPanelDetail> drillPanelDetails = new List<DrillPanelDetail>();
            List<Model.Entites.Mes.TracePanel> panels = new List<Model.Entites.Mes.TracePanel>();
            for (int i = 0; i < req.SplindleCount; i++)
            {
                if (req.BeginSplindleNum + i > deviceData.SpindleNum)
                {
                    break;
                }
                string panelCode = oldSiloDetails.FirstOrDefault(p => p.SplindleIndex == req.BeginSplindleNum + i)?.PanelCode;
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
                        PinOffset = req.PinOffset,
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
                        PinOffset = req.PinOffset,
                        Status = (int)DataStatusEnum.Enable,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                    });
                }
                var drillPanelDetail = _mapper.Map<DrillPanelDetail>(req);
                drillPanelDetail.DeviceCode = req.DeviceCode;
                drillPanelDetail.Layer = (int)req.Layer;
                drillPanelDetail.PanelCode = panelCode;
                drillPanelDetail.SplindleIndex = req.BeginSplindleNum + i;
                drillPanelDetail.ProductStatus = req.ProductStatus.Value;
                drillPanelDetail.CreateTime = DateTime.Now;
                drillPanelDetail.CreatorId = UserId;
                drillPanelDetail.Status = 1;
                drillPanelDetail.ModifyTime = DateTime.Now;
                drillPanelDetail.ModifierId = UserId;
                drillPanelDetails.Add(drillPanelDetail);
            }

            await _panelDomainService.BulkInsert(panels);

            var x = _sqlSugarScope.Storageable(drillPanelDetails).WhereColumns(p => new { p.DeviceCode, p.Layer, p.SplindleIndex }).ToStorage();
            x.AsInsertable.IgnoreColumns(z => new { z.ModifierId, z.ModifyTime }).ExecuteCommand();
            x.AsUpdateable.IgnoreColumns(z => new { z.CreateTime, z.CreatorId }).ExecuteCommand();

            return Success();
        }

        public async Task<ResponseDto<string>> ClearData(ClearDrillPanelDetailReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }
            if (string.IsNullOrEmpty(req.DeviceCode))
            {
                return Fail("未识别有效的DeviceCode！");
            }

            var where = PredicateBuilder.True<DrillPanelDetail>();
            where = where.And(p => p.DeviceCode.ToLower() == req.DeviceCode.ToLower());

            if (req.Layer != null)
            {
                where = where.And(p => p.Layer == req.Layer);
            }

            if (req.SplindleIndexs != null && req.SplindleIndexs.Count != 0)
            {
                if (req.Layer == null)
                {
                    return Fail("未识别有效的Layer！");
                }
                where = where.And(p => req.SplindleIndexs.Contains((int)p.SplindleIndex));
            }

            await _drillPanelDetailDomainService.UpdateAsync(p => new DrillPanelDetail()
            {
                ItemCode = null,
                PanelCode = null,
                ProductStatus = null,
                Pcs = null,
                PanelWidth = null,
                PinOffset = null,
                BatchCode = null,
                ModifierId = UserId,
                ModifyTime = DateTime.Now
            }, where);

            return Success();
        }

        public async Task<ResponseDto<string>> MovePanel(MovePanelDetailDto? startData, MovePanelDetailDto? targetData)
        {
            if (startData == null || targetData == null)
            {
                return Fail("未识别有效的startData或targetData！");
            }
            if (string.IsNullOrEmpty(startData.LocationCode) || !startData.RelatedDeviceKind.HasValue
                || startData.LocationIndex == null || startData.LocationIndex == 0)
            {
                return Fail("startData未识别有效的LocationCode、RelatedDeviceKind或LocationIndex！");
            }
            if (string.IsNullOrEmpty(targetData.LocationCode) || !targetData.RelatedDeviceKind.HasValue
                || targetData.LocationIndex == null || targetData.LocationIndex == 0)
            {
                return Fail("targetData未识别有效的LocationCode、RelatedDeviceKind或LocationIndex！");
            }

            string startDeviceKind = Enum.GetName(typeof(DeviceKind), startData.RelatedDeviceKind);
            string targetDeviceKind = Enum.GetName(typeof(DeviceKind), targetData.RelatedDeviceKind);
            if (startDeviceKind.ToLower().Contains("drill") && startData.Layer == null)
            {
                return Fail("startData未识别有效的Layer！");
            }
            if (targetDeviceKind.ToLower().Contains("drill") && targetData.Layer == null)
            {
                return Fail("targetData未识别有效的Layer！");
            }

            if (startDeviceKind.ToLower().Contains("drill"))
            {
                var startPanelDetail = await _drillPanelDetailDomainService.FindSingleAsync(p => p.DeviceCode.ToLower() == startData.LocationCode.ToLower()
                && p.Layer == startData.Layer && p.SplindleIndex == startData.LocationIndex);
                if (startPanelDetail == null)
                {
                    return Fail("未找到startData数据！");
                }
                if (string.IsNullOrEmpty(startPanelDetail.PanelCode))
                {
                    return Fail("未找到startData对应的板料数据！");
                }

                if (targetDeviceKind.ToLower().Contains("drill"))
                {
                    if (await _drillPanelDetailDomainService.IsExistAsync(p => p.DeviceCode.ToLower() == targetData.LocationCode.ToLower()
                    && p.PanelCode.ToLower() == startPanelDetail.PanelCode.ToLower()))
                    {
                        return Fail($"目标设备已存在板料数据 {startPanelDetail.PanelCode}！");
                    }

                    var targetPanelDetail = await _drillPanelDetailDomainService.FindSingleAsync(p => p.DeviceCode.ToLower() == targetData.LocationCode.ToLower()
                    && p.Layer == targetData.Layer && p.SplindleIndex == targetData.LocationIndex);
                    if (targetPanelDetail == null)
                    {
                        return Fail("未找到targetData数据");
                    }
                    if (!string.IsNullOrEmpty(targetPanelDetail.PanelCode))
                    {
                        return Fail($"目标位置已存在板料数据 {targetPanelDetail.PanelCode}！");
                    }

                    targetPanelDetail.PanelCode = startPanelDetail.PanelCode;
                    targetPanelDetail.PanelWidth = startPanelDetail.PanelWidth;
                    targetPanelDetail.PinOffset = startPanelDetail.PinOffset;
                    targetPanelDetail.Pcs = startPanelDetail.Pcs;
                    targetPanelDetail.BatchCode = startPanelDetail.BatchCode;
                    targetPanelDetail.ItemCode = startPanelDetail.ItemCode;
                    targetPanelDetail.ProductStatus = startPanelDetail.ProductStatus;
                    targetPanelDetail.ModifierId = UserId;
                    targetPanelDetail.ModifyTime = DateTime.Now;

                    await _drillPanelDetailDomainService.Update(targetPanelDetail);
                }
                else
                {
                    if (await _siloDetailDomainService.IsExistAsync(p => p.SiloCode.ToLower() == targetData.LocationCode.ToLower()
                    && p.PanelCode.ToLower() == startPanelDetail.PanelCode.ToLower()))
                    {
                        return Fail($"目标设备已存在板料数据 {startPanelDetail.PanelCode}！");
                    }

                    var targetPanelDetail = await _siloDetailDomainService.FindSingleAsync(p => p.SiloCode.ToLower() == targetData.LocationCode.ToLower()
                    && p.FloorNum == targetData.LocationIndex - 1);
                    if (targetPanelDetail == null)
                    {
                        return Fail("未找到targetData数据");
                    }
                    if (!string.IsNullOrEmpty(targetPanelDetail.PanelCode))
                    {
                        return Fail($"目标位置已存在板料数据 {targetPanelDetail.PanelCode}！");
                    }

                    targetPanelDetail.PanelCode = startPanelDetail.PanelCode;
                    targetPanelDetail.PanelWidth = startPanelDetail.PanelWidth;
                    targetPanelDetail.PinOffset = startPanelDetail.PinOffset;
                    targetPanelDetail.Pcs = startPanelDetail.Pcs;
                    targetPanelDetail.ItemCode = startPanelDetail.ItemCode;
                    targetPanelDetail.ModifierId = UserId;
                    targetPanelDetail.ModifyTime = DateTime.Now;
                    if (startData.Layer == 0)
                    {
                        targetPanelDetail.ProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1;
                    }
                    else if (startData.Layer == 2)
                    {
                        targetPanelDetail.ProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1;
                    }

                    await _siloDetailDomainService.Update(targetPanelDetail);

                    var siloInfo = (await _siloDomainService.QueryAsync(p => p.Code == targetData.LocationCode, p => p.Code, OrderByType.Asc)).FirstOrDefault();
                    siloInfo.EmptySilo = System.Text.Json.JsonSerializer.Serialize(
                        (await _siloDetailDomainService.QueryAsync(p => p.SiloCode == targetData.LocationCode && p.ProductStatus == 0, p => p.SiloCode, OrderByType.Asc))?
                              .Select(p => p.FloorNum + 1).ToList());
                    await _siloDomainService.Update(siloInfo);
                }

                await _drillPanelDetailDomainService.UpdateAsync(p => new DrillPanelDetail()
                {
                    ItemCode = null,
                    PanelCode = null,
                    ProductStatus = null,
                    Pcs = null,
                    PanelWidth = null,
                    PinOffset = null,
                    BatchCode = null,
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now
                }, p => p.DeviceCode.ToLower() == startData.LocationCode.ToLower()
                && p.Layer == startData.Layer && p.SplindleIndex == startData.LocationIndex);
            }
            else
            {
                var startPanelDetail = await _siloDetailDomainService.FindSingleAsync(p => p.SiloCode.ToLower() == startData.LocationCode.ToLower()
                && p.FloorNum == startData.LocationIndex - 1);
                if (startPanelDetail == null)
                {
                    return Fail("未找到startData数据！");
                }
                if (string.IsNullOrEmpty(startPanelDetail.PanelCode))
                {
                    return Fail("未找到startData对应的板料数据！");
                }

                if (targetDeviceKind.ToLower().Contains("drill"))
                {
                    if (await _drillPanelDetailDomainService.IsExistAsync(p => p.DeviceCode.ToLower() == targetData.LocationCode.ToLower()
                    && p.PanelCode.ToLower() == startPanelDetail.PanelCode.ToLower()))
                    {
                        return Fail($"目标设备已存在板料数据 {startPanelDetail.PanelCode}！");
                    }
                    var targetPanelDetail = await _drillPanelDetailDomainService.FindSingleAsync(p => p.DeviceCode.ToLower() == targetData.LocationCode.ToLower()
                    && p.Layer == targetData.Layer && p.SplindleIndex == targetData.LocationIndex);
                    if (targetPanelDetail == null)
                    {
                        return Fail("未找到targetData数据");
                    }
                    if (!string.IsNullOrEmpty(targetPanelDetail.PanelCode))
                    {
                        return Fail($"目标位置已存在板料数据 {targetPanelDetail.PanelCode}！");
                    }

                    targetPanelDetail.PanelCode = startPanelDetail.PanelCode;
                    targetPanelDetail.PanelWidth = startPanelDetail.PanelWidth;
                    targetPanelDetail.PinOffset = startPanelDetail.PinOffset;
                    targetPanelDetail.Pcs = startPanelDetail.Pcs;
                    targetPanelDetail.ItemCode = startPanelDetail.ItemCode;
                    targetPanelDetail.ProductStatus = startPanelDetail.ProductStatus;
                    targetPanelDetail.ModifierId = UserId;
                    targetPanelDetail.ModifyTime = DateTime.Now;

                    await _drillPanelDetailDomainService.Update(targetPanelDetail);
                }
                else
                {
                    if (await _siloDetailDomainService.IsExistAsync(p => p.SiloCode.ToLower() == targetData.LocationCode.ToLower()
                    && p.PanelCode.ToLower() == startPanelDetail.PanelCode.ToLower()))
                    {
                        return Fail($"目标设备已存在板料数据 {startPanelDetail.PanelCode}！");
                    }

                    var targetPanelDetail = await _siloDetailDomainService.FindSingleAsync(p => p.SiloCode.ToLower() == targetData.LocationCode.ToLower()
                    && p.FloorNum == targetData.LocationIndex - 1);
                    if (targetPanelDetail == null)
                    {
                        return Fail("未找到targetData数据");
                    }
                    if (!string.IsNullOrEmpty(targetPanelDetail.PanelCode))
                    {
                        return Fail($"目标位置已存在板料数据 {targetPanelDetail.PanelCode}！");
                    }

                    targetPanelDetail.PanelCode = startPanelDetail.PanelCode;
                    targetPanelDetail.PanelWidth = startPanelDetail.PanelWidth;
                    targetPanelDetail.PinOffset = startPanelDetail.PinOffset;
                    targetPanelDetail.Pcs = startPanelDetail.Pcs;
                    targetPanelDetail.ItemCode = startPanelDetail.ItemCode;
                    targetPanelDetail.ProductStatus = startPanelDetail.ProductStatus;
                    targetPanelDetail.ModifierId = UserId;
                    targetPanelDetail.ModifyTime = DateTime.Now;

                    await _siloDetailDomainService.Update(targetPanelDetail);

                    var siloInfoT = (await _siloDomainService.QueryAsync(p => p.Code == targetData.LocationCode, p => p.Code, OrderByType.Asc)).FirstOrDefault();
                    siloInfoT.EmptySilo = System.Text.Json.JsonSerializer.Serialize(
                        (await _siloDetailDomainService.QueryAsync(p => p.SiloCode == targetData.LocationCode && p.ProductStatus == 0, p => p.SiloCode, OrderByType.Asc))?
                              .Select(p => p.FloorNum + 1).ToList());
                    await _siloDomainService.Update(siloInfoT);
                }

                await _siloDetailDomainService.UpdateAsync(p => new SiloDetail()
                {
                    ItemCode = null,
                    PanelCode = null,
                    ProductStatus = ProductStatus.EmptySiloBox,
                    Pcs = null,
                    PanelWidth = null,
                    PinOffset = null,
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now
                }, p => p.SiloCode.ToLower() == startData.LocationCode.ToLower()
                && p.FloorNum == startData.LocationIndex - 1);

                var siloInfo = (await _siloDomainService.QueryAsync(p => p.Code == startData.LocationCode, p => p.Code, OrderByType.Asc)).FirstOrDefault();
                siloInfo.EmptySilo = System.Text.Json.JsonSerializer.Serialize(
                    (await _siloDetailDomainService.QueryAsync(p => p.SiloCode == startData.LocationCode && p.ProductStatus == 0, p => p.SiloCode, OrderByType.Asc))?
                          .Select(p => p.FloorNum + 1).ToList());
                await _siloDomainService.Update(siloInfo);
            }

            return Success();
        }

        public async Task<ResponseDto<string>> SynchronousPanelData(string deviceCode)
        {
            if (string.IsNullOrEmpty(deviceCode))
            {
                return Fail("未识别有效的deviceCode！");
            }
            var data = await GetOnlineDeviceInfo(deviceCode);
            if (data == null || data.Code != 0 || data.Data == null)
            {
                return Fail("同步失败！");
            }

            var deviceData = data.Data;
            if (string.IsNullOrEmpty(deviceData.ProductId))
            {
                return Fail("无法识别设备的ProductId，同步失败！");
            }

            if (deviceData.ProductId.ToLower().Equals("drill"))
            {
                if (!await _drillPanelDetailDomainService.IsExistAsync(p => p.DeviceCode == deviceCode))
                {
                    var addData = await LoadPanelDetailData(new LoadDrillPanelDetailReq
                    {
                        DeviceCode = deviceCode
                    });
                    if (addData.Code != 0)
                    {
                        return Fail($"同步失败！{deviceCode} {addData.Message}");
                    }
                }
                if (deviceData.PayloadPanels == null || deviceData.PayloadPanels.Count == 0)
                {
                    return Fail("无法识别设备的PayloadPanels，同步失败！");
                }

                var drillPanelDetailList = await _drillPanelDetailDomainService.QueryAsync(p => p.DeviceCode == deviceCode, p => p.Layer, OrderByType.Asc);
                if (drillPanelDetailList == null || drillPanelDetailList.Count == 0)
                {
                    return Fail("同步失败！");
                }

                List<DrillPanelDetail> updateDrillPanelDetail = new List<DrillPanelDetail>();
                foreach (var item in deviceData.PayloadPanels)
                {
                    var drillPanel = drillPanelDetailList.SingleOrDefault(p => p.Layer == item.Layer && p.SplindleIndex == item.Position);
                    if (drillPanel == null)
                    {
                        continue;
                    }

                    drillPanel.PinOffset = (decimal)item.PinOffset;
                    drillPanel.ItemCode = item.ItemCode;
                    drillPanel.PanelCode = item.PanelCode;
                    drillPanel.PanelWidth = (decimal)item.PanelWidth;
                    drillPanel.ProductStatus = item.ProductStatus;
                    drillPanel.BatchCode = item.BatchCode;
                    drillPanel.ModifierId = UserId;
                    drillPanel.ModifyTime = DateTime.Now;

                    updateDrillPanelDetail.Add(drillPanel);
                }

                await _drillPanelDetailDomainService.BulkUpdate(updateDrillPanelDetail);
            }
            else
            {
                if (deviceData.PayloadPanels == null || deviceData.PayloadPanels.Count == 0)
                {
                    return Fail("无法识别设备的PayloadPanels，同步失败！");
                }
                if (string.IsNullOrEmpty(deviceData.PayloadPanels[0].SiloCode))
                {
                    return Fail("无法识别SiloCode，同步失败！");
                }

                if (!await _siloDomainService.IsExistAsync(p => p.Code == deviceData.PayloadPanels[0].SiloCode))
                {
                    await _siloService.Add(new AddOrUpdateSiloReq
                    {
                        Code = deviceData.PayloadPanels[0].SiloCode,
                        FloorCount = deviceData.PayloadPanels.Count,
                        Location = deviceCode,
                        RelatedDeviceKind = deviceData.Descriptor.DeviceKind,
                    });
                }

                var siloData = await _siloDomainService.FindSingleAsync(p => p.Code == deviceData.PayloadPanels[0].SiloCode);
                if (siloData == null)
                {
                    return Fail("同步失败！");
                }

                if (siloData.FloorCount != deviceData.PayloadPanels.Count)
                {
                    await _siloDetailDomainService.DeleteAsync(p => p.SiloCode == siloData.Code);

                    siloData.FloorCount = deviceData.PayloadPanels.Count;
                    await _siloDomainService.Update(siloData);
                }

                if (!await _siloDetailDomainService.IsExistAsync(p => p.SiloCode == siloData.Code))
                {
                    List<SiloDetail> siloDetails = new List<SiloDetail>();
                    for (int i = 0; i < deviceData.PayloadPanels.Count; i++)
                    {
                        var siloDetail = new SiloDetail()
                        {
                            SiloCode = siloData.Code,
                            CreateTime = DateTime.Now,
                            CreatorId = UserId,
                            FloorNum = i,
                            Status = 1
                        };
                        siloDetails.Add(siloDetail);
                    }

                    await _siloDetailDomainService.BulkInsert(siloDetails);
                }

                var siloDetailList = await _siloDetailDomainService.QueryAsync(p => p.SiloCode == siloData.Code, p => p.FloorNum, OrderByType.Asc);
                if (siloDetailList == null || siloDetailList.Count == 0)
                {
                    return Fail("同步失败！");
                }

                List<SiloDetail> updateSiloDetail = new List<SiloDetail>();
                foreach (var item in deviceData.PayloadPanels)
                {
                    var siloDetail = siloDetailList.SingleOrDefault(p => p.FloorNum == item.Layer);
                    if (siloDetail == null)
                    {
                        continue;
                    }

                    siloDetail.ProductStatus = item.ProductStatus;
                    siloDetail.PanelCode = item.PanelCode;
                    siloDetail.PanelWidth = (decimal)item.PanelWidth;
                    siloDetail.PinOffset = (decimal)item.PinOffset;
                    siloDetail.ItemCode = item.ItemCode;
                    siloDetail.ModifierId = UserId;
                    siloDetail.ModifyTime = DateTime.Now;

                    updateSiloDetail.Add(siloDetail);
                }

                await _siloDetailDomainService.BulkUpdate(updateSiloDetail);

                var siloInfo = (await _siloDomainService.QueryAsync(p => p.Code == siloData.Code, p => p.Code, OrderByType.Asc)).FirstOrDefault();
                siloInfo.EmptySilo = System.Text.Json.JsonSerializer.Serialize(
                    (await _siloDetailDomainService.QueryAsync(p => p.SiloCode == siloData.Code && p.ProductStatus == 0, p => p.SiloCode, OrderByType.Asc))?
                          .Select(p => p.FloorNum + 1).ToList());
                await _siloDomainService.Update(siloInfo);
            }

            return Success();
        }

        /// <summary>
        /// 根据设备编码获取在线设备详细信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<ResponseDto<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(string deviceId)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    return Fail<CentralOnlineDeviceDto>("deviceId未填写！");
                }

                //获取中控在线列表，刷新数据状态
                var deviceData = await _centralOnlineDevice.GetOnlineDevicesAndRoute();

                if (deviceData == null)
                {
                    return Fail<CentralOnlineDeviceDto>("result, 未查询到结果！");
                }
                else
                {
                    var returnData = deviceData.Where(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Equals(deviceId.Trim().ToLower())).ToList();

                    if (returnData == null || returnData.Count == 0)
                    {
                        return Fail<CentralOnlineDeviceDto>("未查询到数据！");
                    }
                    return Success(returnData[0]);
                }
            }
            catch (Exception)
            {
                return Fail<CentralOnlineDeviceDto>("异常终止！");
            }
        }

        /// <summary>
        /// 根据设备编码获取在线设备详细信息
        /// </summary>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<CentralOnlineDeviceDto>>> GetOnlineDeviceInfoByDeviceCodes(List<string> deviceIds)
        {
            try
            {
                if (deviceIds == null || deviceIds.Count == 0)
                {
                    return Fail<List<CentralOnlineDeviceDto>>("deviceId未填写！");
                }

                //获取中控在线列表，刷新数据状态
                var deviceData = await _centralOnlineDevice.GetOnlineDevicesAndRoute();

                if (deviceData == null)
                {
                    return Fail<List<CentralOnlineDeviceDto>>("result, 未查询到结果！");
                }
                else
                {
                    var returnData = deviceData.Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceIds.Contains(p.DeviceId.ToLower())).ToList();

                    if (returnData == null || returnData.Count == 0)
                    {
                        return Fail<List<CentralOnlineDeviceDto>>("未查询到数据！");
                    }
                    return Success(returnData);
                }
            }
            catch (Exception)
            {
                return Fail<List<CentralOnlineDeviceDto>>("异常终止！");
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

            var msg = await _siloService.AllotsPanelData(req);
            return msg;
        }

        public async Task<ResponseDto<PageDto<DrillPanelFullData>>> GetDrillPanelFullData(GetDeviceListReq req)
        {
            req.PageNum = req.PageNum < 1 ? 1 : req.PageNum;
            req.PageSize = req.PageSize < 1 ? 10 : req.PageSize;
            var pageDto = new PageDto<DrillPanelFullData>(req.PageNum, req.PageSize);

            var drillDevices = await _deviceDomainService.GetDrills(req);
            pageDto.Total = drillDevices.TotalCount;
            pageDto.List = _mapper.Map<List<DrillPanelFullData>>(drillDevices.ToList());

            if (pageDto.List == null || pageDto.List.Count == 0)
            {
                return Success(pageDto);
            }

            var drillCodeList = pageDto.List.Select(p => p.Code.ToLower()).ToList();
            if (drillCodeList == null || drillCodeList.Count == 0)
            {
                return Success(pageDto);
            }

            var taskDatas = await _taskDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.WorkStationCode)
            && drillCodeList.Contains(p.WorkStationCode.ToLower())
            && (p.TaskStatus == TaskStatusEnum.COMMITED || p.TaskStatus == TaskStatusEnum.SENDING || p.TaskStatus == TaskStatusEnum.BUFFERED || p.TaskStatus == TaskStatusEnum.BEGIN)
            && p.IsDeleted == 0, p => p.Code, OrderByType.Asc);

            var drillRateFactors = await GetRateReasonDatas(drillCodeList);
            //var drillRecords = await GetRecordsSummarys(drillCodeList);

            var deviceOnlineDatas = await GetOnlineDeviceInfoByDeviceCodes(drillCodeList);
            if (deviceOnlineDatas == null || deviceOnlineDatas.Data == null || deviceOnlineDatas.Data.Count == 0)
            {
                return Success(pageDto);
            }

            foreach (var item in pageDto.List)
            {
                item.OverView = $"剩余 0 条任务";
                if (taskDatas != null && taskDatas.Count > 0)
                {
                    var todoTasks = taskDatas.Where(p => p.WorkStationCode.ToLower() == item.Code.ToLower()
                                    && (p.TaskStatus == TaskStatusEnum.COMMITED || p.TaskStatus == TaskStatusEnum.SENDING))
                        .OrderBy(x => x.StartTime)
                        .ToList();
                    if (todoTasks.Any())
                    {
                        item.OverView = $"下个料号：{todoTasks.FirstOrDefault().ItemCode}，共剩余 {todoTasks.Count} 条任务";
                    }
                    else
                    {
                        item.OverView = $"缺少排产任务，请尽快排产！";
                    }

                    var beginTasks = taskDatas.Where(p => p.WorkStationCode.ToLower() == item.Code.ToLower()
                        && p.TaskStatus == TaskStatusEnum.BEGIN)
                         .OrderByDescending(x => x.StartTime)
                         .ToList();
                    if (beginTasks != null && beginTasks.Count > 0)
                    {
                        item.TaskDuration = $"开始中料号 {beginTasks[0].ItemCode} 需要时长 {beginTasks[0].Duration} 分钟;";
                    }
                    else
                    {
                        item.TaskDuration = $"没有开始中任务;";
                    }

                    var bufferTasks = taskDatas.Where(p => p.WorkStationCode.ToLower() == item.Code.ToLower()
                    && p.TaskStatus == TaskStatusEnum.BUFFERED).ToList();
                    if (bufferTasks != null && bufferTasks.Count > 0)
                    {
                        item.TaskDuration = item.TaskDuration + $"生料仓,待命料号 {bufferTasks[0].ItemCode}";
                    }
                }
                else
                {
                    item.TaskDuration = $"没有开始中任务";
                }

                item.AlarmTime = $"异常时长：0 分钟";
                item.WithoutTaskTime = $"未排产时长：0 分钟";
                item.WaitTime = $"等待时长：无生料时长：0 分钟；有熟料时长：0 分钟";
                item.LoadOrUnLoadTime = $"上下料时长：0 分钟";

                if (drillRateFactors != null && drillRateFactors.Count > 0)
                {
                    var alarmTimeData = drillRateFactors.Where(p => p.DeviceId.ToLower() == item.Code.ToLower()
                    && p.Reason == DrillRateFactorReason.DrillAlarm).ToList();
                    if (alarmTimeData != null && alarmTimeData.Count > 0)
                    {
                        string htime = Convert.ToInt32((alarmTimeData[0].EndTime - alarmTimeData[0].StartTime).Value.Hours).ToString();
                        string mtime = Convert.ToInt32((alarmTimeData[0].EndTime - alarmTimeData[0].StartTime).Value.Minutes).ToString();
                        item.AlarmTime = $"异常时长：{alarmTimeData[0].StartTime.ToDate().ToLongTimeString()} -- {alarmTimeData[0].EndTime.ToDate().ToLongTimeString()} ({htime}小时{mtime}分钟)";
                    }

                    var withoutTaskTimeData = drillRateFactors.Where(p => p.DeviceId.ToLower() == item.Code.ToLower()
                    && p.Reason == DrillRateFactorReason.WithoutPendingWorkOrders).ToList();
                    if (withoutTaskTimeData != null && withoutTaskTimeData.Count > 0)
                    {
                        string htime = Convert.ToInt32((withoutTaskTimeData[0].EndTime - withoutTaskTimeData[0].StartTime).Value.Hours).ToString();
                        string mtime = Convert.ToInt32((withoutTaskTimeData[0].EndTime - withoutTaskTimeData[0].StartTime).Value.Minutes).ToString();
                        item.WithoutTaskTime = $"未排产时长：{withoutTaskTimeData[0].StartTime.ToDate().ToLongTimeString()} -- {withoutTaskTimeData[0].EndTime.ToDate().ToLongTimeString()} ({htime}小时{mtime}分钟)";
                    }

                    var noBoardData = drillRateFactors.Where(p => p.DeviceId.ToLower() == item.Code.ToLower()
                    && p.Reason == DrillRateFactorReason.BufferRawChangeNoBoard).ToList();
                    if (noBoardData != null && noBoardData.Count > 0)
                    {
                        int noBoardTime = Convert.ToInt32((noBoardData[0].EndTime - noBoardData[0].StartTime).Value.TotalMinutes);
                        int loadCompleteTime = 0;
                        int exsistClinkerTime = 0;

                        var loadCompleteData = drillRateFactors.Where(p => p.DeviceId.ToLower() == item.Code.ToLower()
                        && p.Reason == DrillRateFactorReason.BufferRawChangeNoBoard
                        && p.StartTime >= noBoardData[0].EndTime).ToList();
                        if (loadCompleteData != null && loadCompleteData.Count > 0)
                        {
                            loadCompleteTime = Convert.ToInt32((loadCompleteData[0].EndTime - loadCompleteData[0].StartTime).Value.TotalMinutes);

                            var exsistClinkerData = drillRateFactors.Where(p => p.DeviceId.ToLower() == item.Code.ToLower()
                            && p.Reason == DrillRateFactorReason.BufferClinkerChangeExist
                            && p.StartTime >= loadCompleteData[0].EndTime).ToList();
                            if (exsistClinkerData != null && exsistClinkerData.Count > 0)
                            {
                                exsistClinkerTime = Convert.ToInt32((exsistClinkerData[0].EndTime - exsistClinkerData[0].StartTime).Value.TotalMinutes);
                            }
                        }

                        item.LoadOrUnLoadTime = $"上下料时长：{noBoardTime + loadCompleteTime + exsistClinkerTime}分钟";

                        item.WaitTime = $"等待时长：无生料时长：{noBoardTime}分钟；有熟料时长：{exsistClinkerTime}分钟";
                    }
                }

                var onlineData = deviceOnlineDatas.Data.SingleOrDefault(p => p.DeviceId.ToLower() == item.Code.ToLower());
                if (onlineData == null)
                {
                    continue;
                }
                item.isOnline = true;
                item.RouteCode = onlineData.RouteCode;
                if (onlineData.Descriptor != null)
                {
                    item.DeviceConsoleAddress = onlineData.Descriptor.HostAddress;
                }
                if (onlineData.Properties != null)
                {
                    item.IsWarning = onlineData.Properties.ContainsKey("IsWarning") ? onlineData.Properties["IsWarning"].ObjToBool() : false;
                    item.IsLoadingOrUnLoading = onlineData.Properties.ContainsKey("IsLoadingOrUnLoading")
                        ? onlineData.Properties["IsLoadingOrUnLoading"].ObjToBool() : false;
                    item.CallAgvMessage = onlineData.Properties.ContainsKey("CallAgvMessage") ? onlineData.Properties["CallAgvMessage"].ToString() : "";
                    item.Percentage = onlineData.Properties.ContainsKey("Drill_Percentage") ? onlineData.Properties["Drill_Percentage"].ToInt() : 0;
                }

                var panels = onlineData.PayloadPanels.ToList();
                if (panels == null || panels.Count == 0)
                {
                    continue;
                }

                var groupData = panels.GroupBy(p => new { p.Layer, p.ItemCode }).ToList();
                var rawSummaryInfo = new DrillSiloItemSumInfo
                {
                    Layer = "生料",
                    ItemCode = "无",
                    Position = "",
                };
                var drillSummaryInfo = new DrillSiloItemSumInfo
                {
                    Layer = "钻机",
                    ItemCode = "无",
                    Position = "",
                };
                var drilledSummaryInfo = new DrillSiloItemSumInfo
                {
                    Layer = "熟料",
                    ItemCode = "无",
                    Position = "",
                };

                if (panels.Any(x => x.Layer == 0 && !string.IsNullOrEmpty(x.ItemCode)))
                {
                    var positions = panels.Where(p => p.Layer == 0 && !string.IsNullOrEmpty(p.ItemCode)).Select(p => p.Position).ToList();
                    rawSummaryInfo.Position = string.Join('、', positions);
                    rawSummaryInfo.ItemCode = panels.FirstOrDefault(x => x.Layer == 0 && !string.IsNullOrEmpty(x.ItemCode))?.ItemCode;
                }

                if (panels.Any(x => x.Layer == 1 && !string.IsNullOrEmpty(x.ItemCode)))
                {
                    var positions = panels.Where(p => p.Layer == 1 && !string.IsNullOrEmpty(p.ItemCode)).Select(p => p.Position).ToList();
                    drillSummaryInfo.Position = string.Join('、', positions);
                    drillSummaryInfo.ItemCode = panels.FirstOrDefault(x => x.Layer == 1 && !string.IsNullOrEmpty(x.ItemCode))?.ItemCode;
                }

                if (panels.Any(x => x.Layer == 2 && !string.IsNullOrEmpty(x.ItemCode)))
                {
                    var positions = panels.Where(p => p.Layer == 2 && !string.IsNullOrEmpty(p.ItemCode)).Select(p => p.Position).ToList();
                    drilledSummaryInfo.Position = string.Join('、', positions);
                    drilledSummaryInfo.ItemCode = panels.FirstOrDefault(x => x.Layer == 2 && !string.IsNullOrEmpty(x.ItemCode))?.ItemCode;
                }

                item.SiloInfos.Add(rawSummaryInfo);
                item.SiloInfos.Add(drillSummaryInfo);
                item.SiloInfos.Add(drilledSummaryInfo);
            }

            return Success(pageDto);

        }

        private async Task<List<DrillRateFactor>> GetRateReasonDatas(List<string> deviceCodes)
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = await GetStartDate();

            var drillRateFactors = await _drillRateFactorDomainService.GetLastRateReasonDatas(deviceCodes, startDate, endDate);
            return drillRateFactors;
        }

        private async Task<List<DeviceRecordsSummaryDto>> GetRecordsSummarys(List<string> deviceCodes)
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = await GetStartDate();

            var datas = await _deviceRecordsService.GetSummaryList(new GetDeviceRecordsSummaryListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue,
                DeviceCodeList = deviceCodes,
                QueryStartTime = startDate,
                QueryEndTime = endDate,
            });

            if (datas == null || datas.Data == null || datas.Data.List == null)
            {
                return new List<DeviceRecordsSummaryDto>();
            }

            return datas.Data.List;
        }

        private async Task<DateTime> GetStartDate()
        {
            DateTime startDate = DateTime.Now.Date;

            string morning = string.Empty;
            string middle = string.Empty;
            string night = string.Empty;

            var sailings = await _sysConfigManager.GetStringValue(MESConfigConstants.SAILINGS);
            if (!string.IsNullOrEmpty(sailings) && sailings.Contains("|"))
            {
                var arrSailings = sailings.Split('|');
                if (arrSailings.Length > 1)
                {
                    for (int i = 0; i < arrSailings.Length; i++)
                    {
                        if (arrSailings[i].StartsWith("0-"))
                        {
                            morning = arrSailings[i].Replace("0-", "");
                        }
                        else if (arrSailings[i].StartsWith("1-"))
                        {
                            middle = arrSailings[i].Replace("1-", "");
                        }
                        else if (arrSailings[i].StartsWith("2-"))
                        {
                            night = arrSailings[i].Replace("2-", "");
                        }
                    }
                }
            }

            DateTime morningDate = DateTime.Now.Date.AddMinutes(await GetMinute(morning));
            DateTime middleDate = DateTime.Now.Date.AddMinutes(await GetMinute(middle));
            DateTime nightDate = DateTime.Now.Date.AddMinutes(await GetMinute(night));

            if (!string.IsNullOrEmpty(morning) && DateTime.Now >= morningDate && (DateTime.Now < middleDate || DateTime.Now < nightDate))
            {
                startDate = morningDate;
            }
            else if (!string.IsNullOrEmpty(middle) && DateTime.Now >= middleDate && DateTime.Now < nightDate)
            {
                startDate = middleDate;
            }
            else if (!string.IsNullOrEmpty(night) && DateTime.Now >= nightDate && DateTime.Now < DateTime.Now.AddDays(1).Date.AddMinutes(await GetMinute(morning)))
            {
                startDate = nightDate;
            }
            else if (!string.IsNullOrEmpty(night) && DateTime.Now < morningDate)
            {
                startDate = nightDate.AddDays(-1);
            }

            return startDate;
        }

        private async Task<int> GetMinute(string sailingsTime)
        {
            int minute = 0;
            if (sailingsTime.Contains(":"))
            {
                var arrDatas = sailingsTime.Split(':');
                if (arrDatas.Length > 1)
                {
                    int h = 0;
                    int.TryParse(arrDatas[0], out h);
                    int m = 0;
                    int.TryParse(arrDatas[1], out m);

                    minute = h * 60 + m;
                }
            }
            else if (sailingsTime.Contains("："))
            {
                var arrDatas = sailingsTime.Split('：');
                if (arrDatas.Length > 1)
                {
                    int h = 0;
                    int.TryParse(arrDatas[0], out h);
                    int m = 0;
                    int.TryParse(arrDatas[1], out m);

                    minute = h * 60 + m;
                }
            }

            return minute;
        }

        public async Task<List<DeviceDataToScreen>> GetDeviceDatasToScreen(GetDeviceListReq? request)
        {
            List<DeviceDataToScreen> result = new List<DeviceDataToScreen>();

            var deviceData = await _centralOnlineDevice.GetOnlineDevices();
            if (deviceData == null)
            {
                return new List<DeviceDataToScreen>();
            }
            else
            {
                var onlineDrillData = deviceData
                    .Where(p => !string.IsNullOrEmpty(p.ProductId)
                        && p.ProductId.ToLower().Equals("drill"));

                if (request != null && request.DeviceCodes.Any())
                {
                    onlineDrillData = onlineDrillData.Where(x => request.DeviceCodes.Any(y => y.ToLower() == x.DeviceId.ToLower()));
                }

                if (onlineDrillData == null || !onlineDrillData.Any())
                {
                    return new List<DeviceDataToScreen>();
                }

                var drillCodeList = onlineDrillData.Where(p => !string.IsNullOrEmpty(p.DeviceId)).Select(p => p.DeviceId.ToLower()).ToList();
                if (drillCodeList == null || drillCodeList.Count == 0)
                {
                    return new List<DeviceDataToScreen>();
                }

                List<TaskStatusEnum> taskStatusEnums = new List<TaskStatusEnum>
                {
                    TaskStatusEnum.COMMITED,
                    TaskStatusEnum.SENDING,
                    TaskStatusEnum.BUFFERED,
                    TaskStatusEnum.BEGIN
                };

                var taskDatas = await _taskDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.WorkStationCode)
                && drillCodeList.Contains(p.WorkStationCode.ToLower())
                && p.TaskStatus.HasValue && taskStatusEnums.Contains((TaskStatusEnum)p.TaskStatus)
                && p.IsDeleted == 0, p => p.Code, OrderByType.Asc);

                var drillRecords = await GetRecordsSummarys(drillCodeList);

                foreach (var drillData in onlineDrillData)
                {
                    if (string.IsNullOrEmpty(drillData.DeviceId))
                        continue;

                    var model = new DeviceDataToScreen
                    {
                        DeviceCode = drillData.DeviceId,
                        Percentage = drillData.Percentage,
                        DeviceStatus = drillData.DeviceStatus,
                        DrillState = drillData.DrillState
                    };

                    var todoTasks = taskDatas.Where(p => p.WorkStationCode.ToLower() == drillData.DeviceId.ToLower()
                                    && (p.TaskStatus == TaskStatusEnum.COMMITED || p.TaskStatus == TaskStatusEnum.SENDING))
                        .OrderBy(x => x.StartTime)
                        .ToList();
                    if (todoTasks != null && todoTasks.Any())
                    {
                        model.NextItemCode = todoTasks[0].ItemCode;
                    }

                    var beginTasks = taskDatas.Where(p => p.WorkStationCode.ToLower() == drillData.DeviceId.ToLower()
                        && p.TaskStatus == TaskStatusEnum.BEGIN)
                         .OrderByDescending(x => x.StartTime)
                         .ToList();
                    if (beginTasks != null && beginTasks.Any())
                    {
                        model.NowItemCode = beginTasks[0].ItemCode;
                    }

                    var records = drillRecords.Where(p => p.DeviceCode.ToLower() == drillData.DeviceId.ToLower()).ToList();
                    if (records != null && records.Any())
                    {
                        model.Duty = records[0].Duty;
                        model.DutyRate = records[0].DutyRate;
                    }

                    result.Add(model);
                }

                result = result.OrderBy(p => p.DeviceCode).ToList();
            }

            return result;
        }
    }
}