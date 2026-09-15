using AutoMapper;
using Microsoft.Extensions.Logging;
using Npoi.Mapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Schedule;
using MesItem = VgAutoDrill.Central.Core.Mes.Item;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

internal class ItemAdapter : IItemAdapter
{
    private readonly IItemService _itemService;
    private readonly IMapper _mapper;
    private readonly ILogger<ItemAdapter> _logger;
    private readonly IUnitMeasureDomainService _unitService;
    private readonly IProductCategoryDomainService _productCategoryService;

    public ItemAdapter(IItemService itemService,
        ILogger<ItemAdapter> logger,
        IUnitMeasureDomainService unitService,
        IProductCategoryDomainService productCategoryService,
        IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
        _logger = logger;
        _unitService = unitService;
        _productCategoryService = productCategoryService;
    }

    public Task<string> LoadPanelInfo(PanelList panels)
    {
        var items = new List<MesItem>();
        panels.Where(t => !string.IsNullOrEmpty(t.ItemCode))
            .Select(x => x.ItemCode)
            .Distinct()
            .ForEach(async (x) =>
            {
                var item = await GetItemDataAsync(x);
                items.Add(item);
            });

        panels.UpdatePanelInfo(p => !string.IsNullOrEmpty(p.ItemCode), p =>
        {
            var item = items.FirstOrDefault(x => x.Code == p.ItemCode);
            if (item != null)
            {
                p.PanelLength = item.PanelLength;
                p.PanelWidth = item.PanelWidth;
            }
        });

        return Task.FromResult(string.Empty);
    }

    public async Task<MesItem> GetItemAsync(string itemCode, string incodeNumber)
    {
        var itemDto = await _itemService.FindSingleAsync(itemCode, incodeNumber);
        if (itemDto == null)
        {
            return null;
        }

        MesItem resultData = new MesItem
        {
            Code = itemDto.Code,
            IncodeNumber = itemDto.IncodeNumber,
            PanelLength = itemDto.PanelLength,
            PanelCount = itemDto.PanelCount,
            DrillFilePath = itemDto.DrillFilePath,
        };
        return resultData;
    }

    public async Task<MesItem> GetItemDataAsync(string itemCode)
    {
        var itemDto = await _itemService.GetItemAsync(itemCode);
        if (itemDto == null)
        {
            return null;
        }

        MesItem resultData = new MesItem
        {
            Code = itemDto.Code,
            IncodeNumber = itemDto.IncodeNumber,
            PanelLength = itemDto.PanelLength,
            PanelCount = itemDto.PanelCount,
            DrillFilePath = itemDto.DrillFilePath,
            PanelWidth = itemDto.PanelWidth
        };
        return resultData;
    }

    public async Task<List<QueryItemCodeResponse>> GetProductCodes(QueryItemCodeRequest request)
    {
        var condition = new QueryItemCodeRequest
        {
            Code = request.Code,
            QueryOrderBy = QueryOrderByEnum.OrderByCodeASC
        };
        if (request.QueryOrderBy.HasValue)
        {
            condition.QueryOrderBy = request.QueryOrderBy;
        }

        var items = await _itemService.GetProductCodes(condition);

        return items.Select(x => new QueryItemCodeResponse
        {
            Code = x.Code,
            Name = x.Name
        }).ToList();
    }

    public async Task<ResponseDto<MesItem>> SetItemDrillFilePath(string itemCode, string drillFilePath)
    {
        var itemDto = await _itemService.SetItemDrillFilePath(itemCode, drillFilePath);
        if (itemDto == null || itemDto.Data == null)
        {
            return new ResponseDto<MesItem>
            {
                Code = ResponseCode.Fail,
                Message = "保存失败！"
            };
        }

        MesItem resultData = new MesItem
        {
            Code = itemDto.Data.Code,
            IncodeNumber = itemDto.Data.IncodeNumber,
            PanelLength = itemDto.Data.PanelLength,
            PanelCount = itemDto.Data.PanelCount,
            DrillFilePath = itemDto.Data.DrillFilePath,
        };
        return new ResponseDto<MesItem>
        {
            Code = itemDto.Code,
            Message = itemDto.Message,
            Data = resultData
        };
    }

    public async Task<object> GetItemCode(QueryItemCodeRequest request)
    {
        //防止客户端误传其他参数，限定以下条件
        var condition = new GetItemListReq
        {
            ItemOrProduct = 2,
            Status = 1,
            PageSize = 50,
            QueryOrderBy = QueryOrderByEnum.OrderByCodeASC
        };

        if (request.QueryOrderBy.HasValue)
        {
            condition.QueryOrderBy = request.QueryOrderBy;
        }

        if (!string.IsNullOrEmpty(request.Code))
        {
            condition.Code = request.Code;
        }

        var items = await _itemService.GetList(condition);
        return items.Data.List.Select(x => new { x.Code, x.Name, x.PanelWidth, x.PanelLength });
    }

    public async Task<bool> ProduceItemData(MesItem item)
    {
        if (item == null)
        {
            _logger.LogInformation("ProduceItemData item is null !");
            return false;
        }
        if (string.IsNullOrEmpty(item.Code))
        {
            _logger.LogInformation("ProduceItemData itemCode is null !");
            return false;
        }
        if (item.PanelCount == null || item.PanelCount == 0)
        {
            _logger.LogInformation("ProduceItemData PanelCount is null or 0!");
            return false;
        }
        // 校验单位
        if (!await _unitService.IsExistAsync(p => p.Code == item.UnitOfMeasure))
        {
            _logger.LogInformation($"ProduceItemData 没有匹配到单位：{item.UnitOfMeasure} 的数据,请先在中控系统中确认！");
            return false;
        }

        //大类
        var productCatory = await _productCategoryService.FindSingleAsync(p => p.Code == item.ProductCategoryCode);
        if (productCatory == null)
        {
            _logger.LogInformation($"ProduceItemData 没有匹配到大类：{item.ProductCategoryCode} 的数据,请先在中控系统中确认！");
            return false;
        }

        var returnDto = false;

        var entity = await _itemService.GetItemAsync(item.Code);
        if (entity == null)
        {
            var result = await _itemService.Add(new AddOrUpdateItemReq
            {
                Code = item.Code,
                Name = item.Code,
                PanelCount = item.PanelCount,
                PanelLength = item.PanelLength,
                PanelWidth = item.PanelWidth,
                DrillFilePath = item.DrillFilePath,
                Status = 1,
                IncodeNumber = item.IncodeNumber,
                ItemOrProduct = item.ItemOrProduct,
                ItemTypeId = 4,
                UnitOfMeasure = item.UnitOfMeasure,
                ProductCategoryCode = productCatory.Code,
                ProductCategoryId = (int)productCatory.Id,
                ProductCategoryName = productCatory.Name,
                LayerNum = (int)item.PanelCount,
            });

            if (result != null && result.Code == ResponseCode.Success)
            {
                returnDto = true;
            }
            else
            {
                string str = result != null ? result.Message : string.Empty;
                _logger.LogInformation($"ProduceItemData add item {item.Code} fail ,reason {str}!");
            }
        }
        else
        {
            var result = await _itemService.Update(new AddOrUpdateItemReq
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                PanelCount = item.PanelCount,
                PanelLength = item.PanelLength,
                PanelWidth = item.PanelWidth,
                DrillFilePath = item.DrillFilePath,
                Status = entity.Status,
                IncodeNumber = item.IncodeNumber,
                ItemOrProduct = item.ItemOrProduct,
                ItemTypeId = entity.ItemTypeId,
                UnitOfMeasure = item.UnitOfMeasure,
                ProductCategoryCode = productCatory.Code,
                ProductCategoryId = (int)productCatory.Id,
                ProductCategoryName = productCatory.Name,
                LayerNum = (int)item.PanelCount,
            });

            if (result != null && result.Code == ResponseCode.Success)
            {
                returnDto = true;
            }
            else
            {
                string str = result != null ? result.Message : string.Empty;
                _logger.LogInformation($"ProduceItemData update item {item.Code} fail ,reason {str}!");
            }
        }

        return returnDto;
    }
}
