using AutoMapper;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System.Text;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem.VegaRawMaterial;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemService : BaseServiceWithTree<Item, ItemTreeDto, ItemDto, AddOrUpdateItemReq>, IItemService
    {
        private readonly IItemDomainService _mdItemDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly ILogger<ItemService> _logger;
        private readonly IExternalWorkOrderService _externalWorkOrderService;
        private readonly ITransportationTaskService _transportationTaskService;
        private readonly IConfiguration _configuration;
        private readonly IAPIHelper _apiHelper;
        private readonly InnerOptions _innerOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public ItemService(IItemDomainService domainService,                   
            IServiceProvider serviceProvider,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _mdItemDomainService = domainService;
            _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
            _configuration = serviceProvider.GetRequiredService<IConfiguration>();
            _logger = serviceProvider.GetRequiredService<ILogger<ItemService>>();
            _apiHelper = serviceProvider.GetRequiredService<IAPIHelper>();
            _externalWorkOrderService = serviceProvider.GetRequiredService<IExternalWorkOrderService>();
            _transportationTaskService = serviceProvider.GetRequiredService<ITransportationTaskService>();
            _innerOptions = serviceProvider.GetRequiredService<IOptions<InnerOptions>>().Value;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemDto>>> GetList(GetItemListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _mdItemDomainService.PageList(req);
            return Success(result);
        }
        /// <summary>
        /// 获取产品代码列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<ItemDto>> GetProductCodes(QueryItemCodeRequest req)
        {
            return await _mdItemDomainService.GetProductCodes(req);
        }

        /// <summary>
        /// 获取树形数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemFullPropertiesTreeDto>>> GetFullTreeList(GetItemListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            //先查询父类数据
            var result = await _mdItemDomainService.PageFullTreeList(req);

            //查询所有非父类数据
            var data = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1
            && q.ParentId != 0, q => q.Id, OrderByType.Asc);

            var children = data.ToList().Adapt<List<ItemFullPropertiesTreeDto>>();

            //查询子类数据，填充到Children中
            if (result.List != null && result.List.Count > 0
                && children != null && children.Count > 0)
            {
                await AddChildren(result.List, children);
            }
            return Success(result);
        }

        private async System.Threading.Tasks.Task AddChildren(List<ItemFullPropertiesTreeDto> list, List<ItemFullPropertiesTreeDto> childrenList)
        {
            foreach (var item in list)
            {
                var data = childrenList.Where(p => p.ParentId == item.Id).ToList();

                item.Children = data;

                if (item.Children != null && item.Children.Count > 0)
                {
                    await AddChildren(item.Children, childrenList);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<ItemTreeDto>>> GetTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<ItemTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }

            var allCodes = AddChildN(list, 0);

            result.Data = allCodes;

            return result;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkInsert(List<ItemToExcelDto> list)
        {
            if (list == null || list.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<Item> items = new List<Item>();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Code) || string.IsNullOrEmpty(item.Name))
                {
                    sb.Append("编码：" + item.Code + " 或名称：" + item.Name + " 为空；");
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

                if (items.Exists(p => p.Code == item.Code))
                {
                    sb.Append("编码" + item.Code + " 导入列表中已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                // 验证板料长度要求
                var validationResult = ValidatePanelLengthRequirement(item.PanelLength);
                if (validationResult.Code != ResponseCode.Success)
                {
                    sb.AppendLine(string.Format("第{0}行：{1}", list.IndexOf(item) + 1, validationResult.Message));
                    failCount++;
                    continue;
                }

                Item model = new Item();
                model.Code = item.Code;
                model.Name = item.Name;
                model.ItemOrProduct = item.ItemOrProduct;
                model.ItemTypeId = item.ItemTypeId;
                model.ProductCategoryCode = item.ProductCategoryCode;
                model.ProductCategoryId = item.ProductCategoryId;
                model.ProductCategoryName = item.ProductCategoryName;
                model.Specification = item.Specification;
                model.UnitOfMeasure = item.UnitOfMeasure;
                model.WarehouseCode = item.WarehouseCode;
                model.WarehouseId = item.WarehouseId;
                model.WarehouseName = item.WarehouseName;
                model.ParentId = item.ParentID;
                if (importStatus)
                {
                    model.Status = 1;
                }
                else
                {
                    model.Status = 0;
                }
                model.CreatorId = UserId;
                model.CreateTime = DateTime.Now;
                items.Add(model);
            }
            var result = await _domainService.BulkInsert(items);
            if (!result)
            {
                return Fail("导入失败！");
            }

            string str = string.Format("预计导入：{0} 条；成功导入：{1} 条；失败：{2} 条；\r\n", list.Count, list.Count - failCount, failCount);
            str = str + sb.ToString();

            return Success(str);
        }

        /// <summary>
        /// 添加物料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public override async Task<ResponseDto<string>> Add(AddOrUpdateItemReq req)
        {
            // 验证板料长度要求
            var validationResult = ValidatePanelLengthRequirement(req.PanelLength);
            if (validationResult.Code != ResponseCode.Success)
            {
                return validationResult;
            }

            return await base.Add(req);
        }

        /// <summary>
        /// 修改物料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public override async Task<ResponseDto<string>> Update(AddOrUpdateItemReq req)
        {
            // 验证板料长度要求
            var validationResult = ValidatePanelLengthRequirement(req.PanelLength);
            if (validationResult.Code != ResponseCode.Success)
            {
                return validationResult;
            }

            return await base.Update(req);
        }

        public async Task<ItemDto> FindSingleAsync(string itemCode, string incodeNumber)
        {
            if (string.IsNullOrEmpty(itemCode) && string.IsNullOrEmpty(incodeNumber))
            {
                return null;
            }
            var where = PredicateBuilder.True<Item>();
            if (!string.IsNullOrEmpty(incodeNumber))
            {
                where = where.And(d => d.IncodeNumber.ToLower() == incodeNumber.ToLower());
            }
            if (!string.IsNullOrEmpty(itemCode))
            {
                where = where.And(d => d.Code.ToLower() == itemCode.ToLower());
            }
            if (!(await _domainService.IsExistAsync(where)))
            {
                return null;
            }

            var item = await _domainService.FindSingleAsync(where);
            return _mapper.Map<ItemDto>(item);
        }

        /// <summary>
        /// 根据编码获取物料信息
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public async Task<ItemDto> GetItemAsync(string itemCode)
        {
            if (string.IsNullOrEmpty(itemCode))
            {
                return null;
            }
            if (!await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Equals(itemCode.ToLower())))
            {
                return null;
            }

            var item = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Equals(itemCode.ToLower()));
            return _mapper.Map<ItemDto>(item);
        }

        /// <summary>
        /// 更新物料绑定的钻带参数文件路径
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="drillFilePath"></param>
        /// <returns></returns>
        public async Task<ResponseDto<ItemDto>> SetItemDrillFilePath(string itemCode, string drillFilePath)
        {
            if (string.IsNullOrEmpty(itemCode) || string.IsNullOrEmpty(drillFilePath))
            {
                return Fail<ItemDto>("未识别有效的itemCode或drillFilePath");
            }
            if (!await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Equals(itemCode.ToLower())))
            {
                return Fail<ItemDto>("未找到" + itemCode + "对应的数据");
            }

            var item = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Equals(itemCode.ToLower()));
            if (item == null)
            {
                return Fail<ItemDto>("未找到" + itemCode + "对应的数据");
            }

            item.DrillFilePath = drillFilePath;
            item.ModifierId = UserId;
            item.ModifyTime = DateTime.Now;
            var result = await _domainService.Update(item);
            if (!result)
            {
                return Fail<ItemDto>("更新失败！");
            }
            return Success(_mapper.Map<ItemDto>(item));
        }

        /// <summary>
        /// 获取维嘉生料
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<GetVegaRawMaterialDto>>> GetVegaRawMaterial(GetVegaRawMaterialReq req)
        {
            var result = new ResponseDto<List<GetVegaRawMaterialDto>>();
            if (req?.Location <= 0)
            {
                return result;
            }
            switch (req?.Location)
            {
                case 1:
                    result.Data = await GetRawMaterialOfPanelSiloShelf(req);//线边仓
                    break;
                case 2:
                    result.Data = await GetRawMaterialOfTransportation(req);//线边仓转入中转位
                    break;
                case 3://中转位
                    result.Data = await GetRawMaterialOfAgvOrPanelSiloFork(req.ItemCode, DeviceKind.PanelSiloFork);
                    break;
                case 4://Agv
                    result.Data = await GetRawMaterialOfAgvOrPanelSiloFork(req.ItemCode, DeviceKind.BackPanelAgv);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 线边仓生料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<GetVegaRawMaterialDto>> GetRawMaterialOfPanelSiloShelf(GetVegaRawMaterialReq req)
        {
            var result = new List<GetVegaRawMaterialDto>();
            var data = await _externalWorkOrderService.GetStockInfoList(new MesStockInfoQueryReq()
            {
                Lot = req.ItemCode
            });
            if (data != null && data?.Data?.Count > 0)
            {
                result = data.Data.Select(m => new GetVegaRawMaterialDto()
                {
                    ItemCode = m.lot,
                    SiloCode = m.podCode,
                    LocationCode = m.address,
                    LocationName = m.addressName,
                    RawMaterialCount = (int)m.qty
                }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取线边仓转入中转位生料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<GetVegaRawMaterialDto>> GetRawMaterialOfTransportation(GetVegaRawMaterialReq req)
        {
            var result = new List<GetVegaRawMaterialDto>();
            var data = await _transportationTaskService.GetList(new GetTransferJobListReq()
            {
                InternalLotNo = req.ItemCode,
                TransportationKind = TransportationKind.Raw,
                ScheduledTaskStatusList = new List<ScheduledTaskStatus> { ScheduledTaskStatus.Running },
            });
            if (data?.Data?.List?.Count > 0)
            {
                result = data?.Data?.List.Select(m => new GetVegaRawMaterialDto()
                {
                    ItemCode = m.InternalLotNo,
                    RawMaterialCount = m.RawCount,
                    LocationCode = m.ForkCode,
                    SiloCode = m.SiloCode
                }).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取agv或者中转位生料
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="deviceKind"></param>
        /// <returns></returns>
        public async Task<List<GetVegaRawMaterialDto>> GetRawMaterialOfAgvOrPanelSiloFork(string itemCode, DeviceKind deviceKind)
        {
            var result = new List<GetVegaRawMaterialDto>();
            if (deviceKind <= 0)
            {
                return result;
            }
            var url = _configuration["AppConfig:DeviceLocation"];
            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.LogInformation("请检查获取设备库位信息信息的url配置:[AppConfig:DeviceLocation]");
                return result;
            }
            url = string.Format(url, (int)Enum.Parse(typeof(DeviceKind), deviceKind.ToString()));
            var data = _apiHelper.RequestData(url);
            if (!string.IsNullOrWhiteSpace(data))
            {
                var vegaRawMaterials = JsonSerializer.Deserialize<List<DeviceLocationDto>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                vegaRawMaterials = vegaRawMaterials.Where(m => string.IsNullOrWhiteSpace(itemCode) ||
                (m.UndrilledItemCodes?.Count > 0 && m.UndrilledItemCodes.Exists(n => n.ToUpper().Contains(itemCode.ToUpper()))))
                .Select(m =>
                {
                    m.UndrilledItemCodes = m.UndrilledItemCodes.Where(n => string.IsNullOrWhiteSpace(itemCode) || n.ToUpper().Contains(itemCode.ToUpper())).ToList();
                    return m;
                }).ToList();
                if (vegaRawMaterials?.Count > 0)
                {
                    result = vegaRawMaterials.SelectMany(item =>
                    {
                        return item.UndrilledItemCodes.Select(m => new GetVegaRawMaterialDto()
                        {
                            ItemCode = m,
                            LocationCode = item.Code,
                            SiloCode = item.SiloCode,
                            RawMaterialCount = item.Panels.Count(n => n.ItemCode == m && ProductStatusConstants.Finished_PIN.Contains(n.ProductStatus))
                        }).ToList();
                    }).ToList();
                }
            }
            return result;
        }

        /// <summary>
        /// 重写QueryByID方法，获取包含用户姓名的物料信息
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>物料信息</returns>
        public override async Task<ResponseDto<ItemDto>> QueryByID(long id)
        {
            var entity = await _mdItemDomainService.GetItemWithUserInfo(id);
            if (entity == null)
            {
                return Fail<ItemDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<ItemDto>("信息错误!");
            }
            var model = _mapper.Map<ItemDto>(entity);

            return Success(model);
        }
        
        /// <summary>
        /// 验证板料长度要求
        /// </summary>
        /// <param name="panelLength">板料长度</param>
        /// <returns>验证结果</returns>
        private ResponseDto<string> ValidatePanelLengthRequirement(float panelLength)
        {
            try
            {
                // 从InnerOptions读取是否启用板料长度验证
                bool enablePanelLengthRequirement = _innerOptions.EnablePanelLengthRequirement;

                if (enablePanelLengthRequirement)
                {
                    // 检查是否为默认值0（可能表示未设置）
                    if (panelLength == 0)
                    {
                        return new ResponseDto<string>
                        {
                            Code = ResponseCode.Fail,
                            Message = "启用板料长度要求时，板料长度不能为空或0！"
                        };
                    }
                    
                    // 验证大于0
                    if (panelLength <= 0)
                    {
                        return new ResponseDto<string>
                        {
                            Code = ResponseCode.Fail,
                            Message = "启用板料长度要求时，板料长度必须大于0！"
                        };
                    }
                }
                return new ResponseDto<string>
                {
                    Code = ResponseCode.Success,
                    Message = "验证通过"
                };
            }
            catch (Exception)
            {
                // 如果配置读取失败，默认不启用验证
                return new ResponseDto<string>
                {
                    Code = ResponseCode.Success,
                    Message = "验证通过"
                };
            }
        }
    }
}
