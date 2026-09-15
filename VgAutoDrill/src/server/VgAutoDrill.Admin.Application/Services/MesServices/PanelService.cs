using AutoMapper;
using Mapster;
using SqlSugar;
using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Panel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    ///
    /// </summary>
    public class PanelService : BaseServiceWithoutTree<Model.Entites.Mes.TracePanel, PanelDto, AddOrUpdatePanelReq>, IPanelService
    {
        private readonly IDevicePanelDomainService _devicePanelService;
        private readonly IDevicePanelHistoryDomainService _hisPanelService;
        private readonly IEncodeBuildRulesService encodeService;
        public readonly IProBoardTraceDomainService _panelDomainService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="devicePanelDomainService"></param>
        /// <param name="devicePanelHistoryDomainService"></param>
        /// <param name="mapper"></param>
        public PanelService(IProBoardTraceDomainService domainService,
            IDevicePanelDomainService devicePanelDomainService,
            IDevicePanelHistoryDomainService devicePanelHistoryDomainService,
            IEncodeBuildRulesService encodeService,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _devicePanelService = devicePanelDomainService;
            _hisPanelService = devicePanelHistoryDomainService;
            this.encodeService = encodeService;
            _panelDomainService = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<PanelDto>>> GetList(GetPanelListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<PanelDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Model.Entites.Mes.TracePanel>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }

            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }

            if (!string.IsNullOrEmpty(req.LocationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.LocationCode) && p.LocationCode.Contains(req.LocationCode));
            }

            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.PanelCode) && p.PanelCode.Contains(req.PanelCode));
            }

            if (req.ProductStatusList != null && req.ProductStatusList.Count > 0)
            {
                List<ProductStatus> queryList = await GetQueryProductStatusList(req.ProductStatusList);
                if (queryList != null && queryList.Count > 0)
                {
                    where = where.And(p => queryList.Contains(p.ProductStatus));
                }
            }

            if (req.ProductStatus.HasValue)
            {
                where = where.And(p => p.ProductStatus == req.ProductStatus);
            }

            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<PanelDto>>();

            foreach (var item in pageDto.List)
            {
                item.ProductStatusDesc = item.ProductStatus.GetProductStatusDesc();
            }

            return Success(pageDto);
        }

        private async Task<List<ProductStatus>> GetQueryProductStatusList(List<ProductStatus> productStatusList)
        {
            List<ProductStatus> queryList = new List<ProductStatus>();
            foreach (var item in productStatusList)
            {
                switch (item)
                {
                    case ProductStatus.EmptyPayload:
                        queryList.AddRange(ProductStatusConstants.EmptyPayload);
                        break;

                    case ProductStatus.EmptySiloBox:
                        queryList.AddRange(ProductStatusConstants.EmptySiloBox);
                        break;

                    case ProductStatus.Finished_DRILL:
                        queryList.AddRange(ProductStatusConstants.Finished_DRILL);
                        break;

                    case ProductStatus.Finished_PIN:
                        queryList.AddRange(ProductStatusConstants.Finished_PIN);
                        break;
                }
            }
            return queryList;
        }

        /// <summary>
        /// 获取树形数据列表（主：板料；明细：流转记录）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<PanelFullPropertiesTreeDto>>> GetFullTreeList(GetPanelListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<PanelFullPropertiesTreeDto>(req.PageNum, req.PageSize);

            if (string.IsNullOrEmpty(req.LocationCode))
            {
                var result = await GetList(req);
                if (result == null || result.Data == null || result.Data.List == null || result.Data.List.Count == 0)
                {
                    return Success(pageDto);
                }

                pageDto.Total = result.Data.Total;
                pageDto.List = result.Data.List.Adapt<List<PanelFullPropertiesTreeDto>>();
            }
            else
            {
                if (req.ProductStatusList != null && req.ProductStatusList.Count > 0)
                {
                    List<ProductStatus> queryList = await GetQueryProductStatusList(req.ProductStatusList);
                    if (queryList != null && queryList.Count > 0)
                    {
                        req.ProductStatusList = queryList;
                    }
                }

                var result = await _panelDomainService.GetPanelList(req);
                if (result == null || result.List == null)
                {
                    return Success(pageDto);
                }

                pageDto.Total = result.Total;
                pageDto.List = result.List.Adapt<List<PanelFullPropertiesTreeDto>>();
            }

            #region//20240614 策略变更：查询板料流转记录，只从历史记录中获取
            //获取设备负载板料历史数据
            List<string?> queryHis = pageDto.List == null ? new List<string?>() : pageDto.List.Select(p => p.PanelCode).ToList();
            var hisPanels = await _hisPanelService.GetListByPanel(queryHis, req);

            foreach (var item in pageDto.List)
            {
                item.ProductStatusDesc = item.ProductStatus.GetProductStatusDesc();

                if (hisPanels != null && hisPanels.Count > 0)
                {
                    var hisPanelData = hisPanels.Where(p => p.PanelCode == item.PanelCode).ToList();
                    if (hisPanelData != null)
                    {
                        hisPanelData.ForEach(async p =>
                        {
                            p.DevicePanelTime = p.DevicePanelTime == null ? p.CreateTime : p.DevicePanelTime;
                            p.ProductStatusDesc = item.ProductStatus.GetProductStatusDesc();
                        });
                        item.Children.AddRange(hisPanelData);
                    }
                }
            }

            #endregion

            return Success(pageDto);
        }

        /// <summary>
        /// 获取板料追溯List
        /// </summary>
        /// <returns></returns>
        public async Task<List<PanelToExcelDto>> GetToExcelList(GetPanelListReq req)
        {
            List<PanelToExcelDto> list = new List<PanelToExcelDto>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 100) req.PageSize = 1000;

            //获取板料数据
            List<PanelDto> panels = new List<PanelDto>();
            if (string.IsNullOrEmpty(req.LocationCode))
            {
                var result = await GetList(req);
                if (result == null || result.Data == null || result.Data.List == null || result.Data.List.Count == 0)
                {
                    return list;
                }

                panels = result.Data.List;
            }
            else
            {
                if (req.ProductStatusList != null && req.ProductStatusList.Count > 0)
                {
                    List<ProductStatus> queryList = await GetQueryProductStatusList(req.ProductStatusList);
                    if (queryList != null && queryList.Count > 0)
                    {
                        req.ProductStatusList = queryList;
                    }
                }

                var result = await _panelDomainService.GetPanelList(req);
                if (result == null || result.List == null || result.List.Count == 0)
                {
                    return list;
                }

                panels = result.List;
            }

            //获取设备负载板料历史数据
            List<string?> queryHis = panels.Select(p => p.PanelCode).ToList();
            var hisPanels = await _hisPanelService.GetListByPanel(queryHis, req);

            foreach (var panelData in panels)
            {
                string ProductStatusDesc = panelData.ProductStatus.GetProductStatusDesc();
                list.Add(new PanelToExcelDto
                {
                    BatchCode = panelData.BatchCode,
                    BoardLocation = panelData.BoardLocation,
                    PanelCode = panelData.PanelCode,
                    ItemCode = panelData.ItemCode,
                    StationId = panelData.StationId,
                    PanelLength = panelData.PanelLength,
                    PinOffset = panelData.PinOffset,
                    PanelWidth = panelData.PanelWidth,
                    Pcs = panelData.Pcs,
                    ProductStatus = ProductStatusDesc,
                });

                if (hisPanels != null && hisPanels.Count > 0)
                {
                    var hisPanelData = hisPanels.Where(p => p.PanelCode == panelData.PanelCode).ToList();
                    if (hisPanelData != null)
                    {
                        foreach (var item in hisPanelData)
                        {
                            PanelToExcelDto model = new PanelToExcelDto();
                            model.BatchCode = panelData.BatchCode;
                            model.BoardLocation = panelData.BoardLocation;
                            model.DeviceCode = item.DeviceCode;
                            model.PanelWidth = item.PanelWidth;
                            model.PinOffset = item.PinOffset;
                            model.DevicePanelTime = item.DevicePanelTime == null ? item.CreateTime : (DateTime)item.DevicePanelTime;
                            model.ProductStatus = item.ProductStatus.GetProductStatusDesc();
                            model.ItemCode = panelData.ItemCode;
                            model.PanelCode = item.PanelCode;
                            model.Pcs = panelData.Pcs;
                            model.StationId = panelData.StationId;

                            list.Add(model);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdatePanelReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.PanelCode == req.PanelCode);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Model.Entites.Mes.TracePanel>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);
            return Success();
        }

        /// <summary>
        /// 根据传入的物料信息，生成panel编号
        /// </summary>
        /// <returns></returns>
        public async Task<List<PanelDto>> GeneratePanels(BatchInsertPanelReq batchInsertPanelReq)
        {
            var list = new List<PanelDto>();
            if (batchInsertPanelReq == null)
            {
                return list;
            }
            else if (batchInsertPanelReq.BeginLayer < 0
                || batchInsertPanelReq.Count <= 0
                || string.IsNullOrEmpty(batchInsertPanelReq.ItemCode))
            {
                return list;
            }

            var panelCodeQuery = await encodeService.GetEncodeList(new GetEncodeByRulesListReq() { RulesCode = "PANEL_CODE", BuildCount = batchInsertPanelReq.Count });
            var panelEntities = new List<Model.Entites.Mes.TracePanel>();
            var panelCodes = panelCodeQuery.Data;
            var currentLayer = batchInsertPanelReq.BeginLayer;
            foreach (var code in panelCodes)
            {
                var panel = _mapper.Map<Model.Entites.Mes.TracePanel>(batchInsertPanelReq);
                panel.ProductStatus = batchInsertPanelReq.ProductStatus;
                panel.PanelCode = code;
                panel.CreateTime = DateTime.Now;
                panel.ModifyTime = DateTime.Now;
                panel.Status = 1;
                panel.CreatorId = UserId;
                panel.ModifierId = UserId;
                panelEntities.Add(panel);
            }

            var result = await _domainService.BulkInsert(panelEntities);
            if (result)
            {
                var panels = await _domainService.QueryAsync(t => panelCodes.Contains(t.PanelCode) && t.IsDeleted == 0 && t.Status == 1, t => t.Id, OrderByType.Asc);
                list = panels.Adapt<List<PanelDto>>();
            }
            return list;
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdatePanelReq req)
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

            var isExsitCode = await _domainService.IsExistAsync(p => p.PanelCode == req.PanelCode && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Model.Entites.Mes.TracePanel>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        public async Task<ResponseDto<string>> BatchAddData(List<AddOrUpdatePanelReq> req)
        {
            var lst = _mapper.Map<List<Model.Entites.Mes.TracePanel>>(req);
            foreach (var panel in lst)
            {
                panel.CreatorId = UserId;
            }
            await _domainService.BulkInsert(lst);

            return Success();
        }
    }
}