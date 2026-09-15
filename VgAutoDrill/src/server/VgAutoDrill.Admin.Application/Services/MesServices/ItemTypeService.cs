using AutoMapper;
using Mapster;
using SqlSugar;
using System.Text;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItemType;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemTypeService : BaseServiceWithTree<ItemType, ItemTypeTreeDto, ItemTypeDto, AddOrUpdateItemTypeReq>, IItemTypeService
    {
        private readonly IItemDomainService _itemDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="itemDomainService"></param>
        /// <param name="mapper"></param>
        public ItemTypeService(IMdItemTypeDomainService domainService,
            IItemDomainService itemDomainService,
            ISysConfigManager sysConfigManager,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _itemDomainService = itemDomainService;
            _sysConfigManager = sysConfigManager;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemTypeDto>>> GetList(GetItemTypeListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ItemTypeDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ItemType>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (req.ItemOrProduct != null)
            {
                where = where.And(p => p.ItemOrProduct == req.ItemOrProduct);
            }

            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<ItemType>, List<ItemTypeDto>>(result.ToList());
            return Success(pageDto);
        }

        /// <summary>
        /// 获取树形数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemTypeFullPropertiesTreeDto>>> GetFullTreeList(GetItemTypeListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ItemTypeFullPropertiesTreeDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ItemType>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (req.ItemOrProduct != null)
            {
                where = where.And(p => p.ItemOrProduct == req.ItemOrProduct || p.ItemOrProduct == 0);
            }

            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<ItemTypeFullPropertiesTreeDto>>();

            //查询所有非父类数据
            var data = await _domainService.QueryAsync(where.And(p => p.ParentId != 0), q => q.Id, OrderByType.Asc);

            var children = data.ToList().Adapt<List<ItemTypeFullPropertiesTreeDto>>();

            //查询子类数据，填充到Children中
            if (pageDto.List != null && pageDto.List.Count > 0
                && children != null && children.Count > 0)
            {
                var parentData = pageDto.List.Where(p => p.ParentId == 0).ToList();
                if (parentData.Count > 0)
                {
                    await AddChildren(parentData, children);
                    pageDto.List = parentData;
                }
            }

            return Success(pageDto);
        }
        private async System.Threading.Tasks.Task AddChildren(List<ItemTypeFullPropertiesTreeDto> list, List<ItemTypeFullPropertiesTreeDto> childrenList)
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
        public async Task<ResponseDto<List<ItemTypeTreeDto>>> GetTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<ItemTypeTreeDto>>();
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
        public async Task<ResponseDto<string>> BulkInsert(List<ItemTypeToExcelDto> list)
        {
            if (list == null || list.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<ItemType> ItemTypes = new List<ItemType>();
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

                if (ItemTypes.Exists(p => p.Code == item.Code))
                {
                    sb.Append("编码" + item.Code + " 导入列表中已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                ItemType model = new ItemType();
                model.Code = item.Code;
                model.Name = item.Name;
                model.ParentId = item.ParentID;
                model.ItemOrProduct = item.ItemOrProduct;
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
                ItemTypes.Add(model);
            }
            var result = await _domainService.BulkInsert(ItemTypes);
            if (!result)
            {
                return Fail("导入失败！");
            }

            string str = string.Format("预计导入：{0} 条；成功导入：{1} 条；失败：{2} 条；\r\n", list.Count, list.Count - failCount, failCount);
            str = str + sb.ToString();

            return Success(str);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteData(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var isExsitItem = await _itemDomainService.IsExistAsync(p => p.ItemTypeId == id);
            if (isExsitItem)
            {
                return Fail("存在物料产品信息，不允许删除!");
            }

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                return Success("");
            }
            return Fail("删除失败");
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteDataList(List<long> idList)
        {
            if (idList != null)
            {
                object[] deleteList = new object[idList.Count];
                for (int i = 0; i < idList.Count; i++)
                {
                    var entity = await _domainService.QueryByID(idList[i]);
                    if (entity == null)
                    {
                        continue;
                    }

                    var isExsitItem = await _itemDomainService.IsExistAsync(p => p.ItemTypeId == idList[i]);
                    if (isExsitItem)
                    {
                        return Fail(entity.Code + " 存在物料产品信息，不允许删除!");
                    }

                    deleteList[i] = idList[i];
                }

                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {
                    return Success("");
                }
            }
            return Fail("删除失败");
        }
    }
}
