using AutoMapper;
using Mapster;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    ///
    /// </summary>
    public class DevicePanelService : BaseServiceWithoutTree<DevicePanel, DevicePanelDto, AddOrUpdateDevicePanelReq>, IDevicePanelService
    {
        private readonly IDevicePanelDomainService devicePanelDomainService;
        private readonly IDevicePanelHistoryDomainService _hisDomainService;
        private readonly IDeviceDomainService _deviceDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IAPIHelper _apiHelper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="hisDomainService"></param>
        /// <param name="deviceDomainService"></param>
        /// <param name="mapper"></param>
        public DevicePanelService(IDevicePanelDomainService domainService,
            IDevicePanelHistoryDomainService hisDomainService,
            IDeviceDomainService deviceDomainService,
            IMapper mapper,
            ISysConfigManager sysConfigManager,
            IAPIHelper apiHelper)
            : base(domainService, mapper)
        {
            this.devicePanelDomainService = domainService;
            this._hisDomainService = hisDomainService;
            _deviceDomainService = deviceDomainService;
            _sysConfigManager = sysConfigManager;
            _apiHelper = apiHelper;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DevicePanelDto>>> GetList(GetDevicePanelListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DevicePanelDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DevicePanel>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }
            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DeviceCode) && p.DeviceCode.Contains(req.DeviceCode));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.LotId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.LotId) && p.LotId.Contains(req.LotId));
            }
            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.PanelCode) && p.PanelCode.Contains(req.PanelCode));
            }
            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.Contains(req.SiloCode));
            }
            if (req.ProductStatus > 0)
            {
                where = where.And(p => p.ProductStatus == req.ProductStatus);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DevicePanel>, List<DevicePanelDto>>(result.ToList());
            return Success(pageDto);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inputPanels"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> BulkInsert(List<AddOrUpdateDevicePanelReq> inputPanels)
        {
            if (inputPanels == null || inputPanels.Count == 0)
            {
                return Success(false);
            }
            var deviceCode = inputPanels.First().DeviceCode.ToLower();
            var panelCodes = inputPanels.Select(x => x.PanelCode.ToLower());

            var newDevicePanels = new List<DevicePanel>();
            var originalPanels = await _domainService.QueryAsync(p => p.DeviceCode.ToLower() == deviceCode, p => p.Id, OrderByType.Asc);

            foreach (var item in inputPanels)
            {
                if (!originalPanels.Any(p => p.DeviceCode.ToLower() == item.DeviceCode.ToLower()
                    && p.PanelCode.ToLower() == item.PanelCode.ToLower()
                    && p.ProductStatus == item.ProductStatus))
                {
                    newDevicePanels.Add(new DevicePanel
                    {
                        DeviceCode = item.DeviceCode,
                        PanelCode = item.PanelCode,
                        SiloCode = item.SiloCode,
                        ItemCode = item.ItemCode,
                        LotId = item.LotId,
                        BatchCode = item.BatchCode,
                        Layer = item.Layer,
                        Position = item.Position,
                        ProductStatus = item.ProductStatus,
                        LocationCode = item.LocationCode,
                        PinOffset = item.PinOffset,
                        PanelWidth = item.PanelWidth,
                        DrillState = item.DrillState,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId
                    });
                }
            }

            var deletedPanels = originalPanels.Where(old => !inputPanels.Any(p => p.DeviceCode.ToLower() == old.DeviceCode.ToLower()
                && p.ItemCode.ToLower() == old.ItemCode.ToLower()
                && p.ProductStatus == old.ProductStatus)).Select(old => old.Id);

            if (deletedPanels.Any())
            {
                await _domainService.DeleteByIds(Array.ConvertAll<long, object>(deletedPanels.ToArray(), s => (object)s));
            }

            var oldHisPanels = await _hisDomainService.QueryAsync(p => p.DeviceCode.ToLower() == deviceCode && panelCodes.Contains(p.PanelCode.ToLower()),
                p => p.Id, OrderByType.Asc);

            var hisInsertList = new List<DevicePanelHistory>();
            foreach (var item in inputPanels)
            {
                if (!oldHisPanels.Any(h => h.PanelCode.ToLower() == item.PanelCode.ToLower()
                        && h.ItemCode.ToLower() == item.ItemCode.ToLower()
                        && h.ProductStatus == item.ProductStatus))
                {
                    var his = item.Adapt<DevicePanelHistory>();
                    his.DevicePanelTime = his.CreateTime;
                    hisInsertList.Add(his);
                }
            }

            if (hisInsertList.Any())
            {
                await _hisDomainService.BulkInsert(hisInsertList);
            }

            var result = await _domainService.BulkInsert(newDevicePanels);
            return Success(result);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateDevicePanelReq req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req));
            }

            var model = new DevicePanel
            {
                DeviceCode = req.DeviceCode,
                PanelCode = req.PanelCode,
                SiloCode = req.SiloCode,
                ItemCode = req.ItemCode,
                LotId = req.LotId,
                BatchCode = req.BatchCode,
                Layer = req.Layer,
                Position = req.Position,
                ProductStatus = req.ProductStatus,
                LocationCode = req.LocationCode,
                PanelWidth = req.PanelWidth,
                PinOffset = req.PinOffset,
                DrillState = req.DrillState,
                CreateTime = DateTime.Now,
                CreatorId = UserId
            };

            var hisModel = model.Adapt<DevicePanelHistory>();
            hisModel.DevicePanelTime = hisModel.CreateTime;
            await _hisDomainService.Add(hisModel);

            var result = await _domainService.Add(model);
            if (result)
            {
                return Success();
            }
            else
            {
                return Fail("添加失败！");
            }
        }
    }
}