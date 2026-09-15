using AutoMapper;
using Mapster;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class MaterialStockService : BaseServiceWithoutTree<MaterialStock, MaterialStockDto, AddOrUpdateMaterialStockReq>, IMaterialStockService
    {
        private readonly IMaterialStockDomainService _materialStockService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public MaterialStockService(IMaterialStockDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _materialStockService = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<MaterialStockDto>>> GetList(GetMaterialStockListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<MaterialStockDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<MaterialStock>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.Contains(req.SiloCode));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.ItemName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }

            if (!string.IsNullOrEmpty(req.WarehouseCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WarehouseCode) && p.WarehouseCode.Contains(req.WarehouseCode));
            }
            if (!string.IsNullOrEmpty(req.WarehouseName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WarehouseName) && p.WarehouseName.Contains(req.WarehouseName));
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<MaterialStock>, List<MaterialStockDto>>(result.ToList());
            return Success<PageDto<MaterialStockDto>>(pageDto);
        }

        /// <summary>
        /// 根据ItemTypeId获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<MaterialStockDto>>> GetEquipmentList(GetMaterialStockListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _materialStockService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 根据ItemTypeId获取树形数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<MaterialStockFullPropertiesTreeDto>>> GetEquipmentTreeList(GetMaterialStockListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _materialStockService.GetTreeList(req);

            //查询所有非父类数据
            var data = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1
            && !string.IsNullOrEmpty(q.InOrOut) && q.InOrOut.ToLower().Equals("out"), q => q.Id, OrderByType.Asc);

            var children = data.ToList().Adapt<List<MaterialStockFullPropertiesTreeDto>>();

            //查询子类数据，填充到Children中
            if (result.List != null && result.List.Count > 0
                && children != null && children.Count > 0)
            {
                foreach (var item in result.List)
                {
                    var childrenData = children.Where(p => p.ParentId == item.Id).ToList();

                    item.Children = childrenData.ToList().Adapt<List<MaterialStockFullPropertiesTreeDto>>();
                }
            }

            return Success(result);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateMaterialStockReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }
            var model = _mapper.Map<MaterialStock>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;

            //出库的时候，需要刷新 父类的库存现有量
            if (!string.IsNullOrEmpty(model.InOrOut) && model.InOrOut.ToLower().Equals("out") && model.ParentId != null)
            {
                //model.QuantityOnhand = null;
                var result = await _domainService.QueryByID(model.ParentId);
                if (result != null)
                {
                    result.QuantityOnhand -= model.QuantityTransaction;
                    result.ModifierId = UserId;
                    result.ModifyTime = DateTime.Now;
                    await _domainService.Update(result);
                }

                var resOnlineQty = await _domainService.QueryAsync(t => t.ParentId == model.ParentId && t.IsDeleted == 0, t => t.Id, SqlSugar.OrderByType.Asc);
                if (resOnlineQty != null)
                {
                    List<MaterialStock> list = new List<MaterialStock>();
                    list = resOnlineQty.ToList();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        list[i].QuantityOnhand = req.QuantityOnhand;
                    }
                    await _domainService.BulkUpdate(list);
                }
            }
            else if (!string.IsNullOrEmpty(model.InOrOut) && model.InOrOut.ToLower().Equals("in"))
            {
                model.QuantityOnhand = model.QuantityTransaction;
            }

            await _domainService.Add(model);
            return Success();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateMaterialStockReq req)
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

            var model = _mapper.Map<MaterialStock>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }
    }
}
