using AutoMapper;
using SqlSugar.Extensions;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFileDetail;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemAtpFileDetailService : BaseServiceWithoutTree<ItemAtpFileDetail, ItemAtpFileDetailDto, AddOrUpdateItemAtpFileDetailReq>, IItemAtpFileDetailService
    {
        private readonly IItemAtpFileDomainService _domainATPService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="domainATPService"></param>
        /// <param name="mapper"></param>
        public ItemAtpFileDetailService(IItemAtpFileDetailDomainService domainService, IItemAtpFileDomainService domainATPService, IMapper mapper)
            : base(domainService, mapper)
        {
            _domainATPService = domainATPService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemAtpFileDetailDto>>> GetList(GetItemAtpFileDetailListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ItemAtpFileDetailDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ItemAtpFileDetail>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (req.Diameter != null)
            {
                where = where.And(p => p.Diameter == req.Diameter);
            }

            if (req.ItemAtpFileId > 0)
            {
                where = where.And(p => p.ItemAtpFileId == req.ItemAtpFileId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.OrderNum, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<ItemAtpFileDetail>, List<ItemAtpFileDetailDto>>(result.ToList());
            return Success<PageDto<ItemAtpFileDetailDto>>(pageDto);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateCheck(AddOrUpdateItemAtpFileDetailReq req)
        {
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            var model = _mapper.Map<ItemAtpFileDetail>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.OrderNum = entity.OrderNum;
            model.ItemAtpFileId = entity.ItemAtpFileId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        /// <summary>
        /// 更新数据集合
        /// </summary>
        /// <param name="reqList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateList(List<AddOrUpdateItemAtpFileDetailReq> reqList)
        {
            if (reqList == null || reqList.Count == 0)
            {
                return Fail("信息不存在!");
            }

            foreach (var item in reqList)
            {
                var entity = await _domainService.QueryByID(item.Id);
                if (entity == null)
                {
                    return Fail(item.Code + " 信息不存在!");
                }
            }

            List<ItemAtpFileDetail> updateList = new List<ItemAtpFileDetail>();
            foreach (var req in reqList)
            {
                var entity = await _domainService.QueryByID(req.Id);
                if (entity != null)
                {
                    var model = _mapper.Map<ItemAtpFileDetail>(req);
                    model.CreateTime = entity.CreateTime;
                    model.CreatorId = entity.CreatorId;
                    model.OrderNum = entity.OrderNum;
                    model.ItemAtpFileId = entity.ItemAtpFileId;
                    model.ModifierId = UserId;
                    model.ModifyTime = DateTime.Now;
                    updateList.Add(model);
                }
            }
            await _domainService.BulkUpdate(updateList);

            return Success();
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<List<List<ItemAtpFileDetailDto>>>>> GetMultiList(GetItemAtpFileDetailListReq req)
        {
            List<List<List<ItemAtpFileDetailDto>>> returnList = new List<List<List<ItemAtpFileDetailDto>>>();

            //先查询主表，获取行、列、刀盘数，返回特定集合
            var whereM = PredicateBuilder.True<ItemAtpFile>();
            whereM = whereM.And(p => p.IsDeleted == 0);

            if (req.ItemAtpFileId > 0)
            {
                whereM = whereM.And(p => p.Id == req.ItemAtpFileId);
            }
            else
            {
                return Success(returnList);
            }

            var result = await _domainATPService.QueryPageAsync(whereM, q => q.CreateTime, SqlSugar.OrderByType.Desc, 1, int.MaxValue);
            if (result == null || result.Count != 1) //ID是唯一主键，根据ID查询有且仅有一条数据
            {
                return Success(returnList);
            }

            int row = result[0].RowsLimit.ObjToInt();
            int column = result[0].ColumnsLimit.ObjToInt();
            int disk = result[0].DiskCount.ObjToInt();

            if (row == 0 || column == 0 || disk == 0)
            {
                return Success(returnList);
            }

            //查询详细数据
            int singleDiskLimit = row * column;
            for (int i = 1; i < disk + 1; i++)
            {
                int start = row * column * (i - 1);

                var where = PredicateBuilder.True<ItemAtpFileDetail>();
                where = where.And(p => p.IsDeleted == 0);

                if (req.ItemAtpFileId > 0)
                {
                    where = where.And(p => p.ItemAtpFileId == req.ItemAtpFileId);
                }

                where = where.And(p => p.OrderNum > start);

                var data = await _domainService.QueryPageAsync(where, q => q.OrderNum, SqlSugar.OrderByType.Asc, 1, singleDiskLimit);

                var existRepeatOrderNum = data.DistinctBy(p => p.OrderNum).ToList();
                if (existRepeatOrderNum != null && existRepeatOrderNum.Count != data.Count)
                {
                    continue;  //存在重复的OrderNum，则不返回任何信息
                }

                List<List<ItemAtpFileDetailDto>> pList = new List<List<ItemAtpFileDetailDto>>();

                //显示钻刀排序序号的算法，
                //todo, 根据参数，分别支持四种排刀的方案，从左下（目前），左上，右下，右上开始排刀
                for (int j = row; j > 0; j--)
                {
                    List<ItemAtpFileDetailDto> rList = new List<ItemAtpFileDetailDto>();

                    for (int k = 0; k < column; k++)
                    {
                        int num = j + k * row + (i - 1) * row * column;
                        var model = _mapper.Map<ItemAtpFileDetailDto>(data.SingleOrDefault(p => p.OrderNum == num));
                        rList.Add(model);
                    }

                    pList.Add(rList);
                }

                returnList.Add(pList);
            }
            return Success(returnList);
        }
    }
}