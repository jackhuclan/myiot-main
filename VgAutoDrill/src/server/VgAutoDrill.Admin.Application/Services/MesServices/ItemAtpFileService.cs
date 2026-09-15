using AutoMapper;
using SqlSugar;
using SqlSugar.Extensions;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// ATP文件
    /// </summary>
    public class ItemAtpFileService : BaseServiceWithoutTree<ItemAtpFile, ItemAtpFileDto, AddOrUpdateItemAtpFileReq>, IItemAtpFileService
    {
        private readonly IItemAtpFileDomainService _itemAtpFileService;
        private readonly IItemAtpFileDetailDomainService _detailService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="detailService"></param>
        /// <param name="mapper"></param>
        public ItemAtpFileService(IItemAtpFileDomainService domainService, IItemAtpFileDetailDomainService detailService, IMapper mapper)
            : base(domainService, mapper)
        {
            _itemAtpFileService = domainService;
            _detailService = detailService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemAtpFileDto>>> GetList(GetItemAtpFileListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ItemAtpFileDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ItemAtpFile>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.AtpFileName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.AtpFileName) && p.AtpFileName.Contains(req.AtpFileName));
            }

            if (!string.IsNullOrEmpty(req.ATPParameters))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ATPParameters) && p.ATPParameters.Contains(req.ATPParameters));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }

            if (!string.IsNullOrEmpty(req.ItemName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }

            if (req.IsGenerated != null)
            {
                where = where.And(p => p.IsGenerated == req.IsGenerated);
            }

            if (req.ItemId > 0)
            {
                where = where.And(p => p.ItemId == req.ItemId);
            }

            if (req.ItemDrillFileId > 0)
            {
                where = where.And(p => p.ItemDrillFileId == req.ItemDrillFileId);
            }

            if (req.CutterConfigMasterId > 0)
            {
                where = where.And(p => p.CutterConfigMasterId == req.CutterConfigMasterId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<ItemAtpFile>, List<ItemAtpFileDto>>(result.ToList());
            return Success<PageDto<ItemAtpFileDto>>(pageDto);
        }

        /// <summary>
        /// 根据ItemTypeId获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ItemAtpFileDto>>> GetEquipmentList(GetItemAtpFileListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _itemAtpFileService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public virtual async Task<ResponseDto<string>> AddCheck(AddOrUpdateItemAtpFileReq req)
        {
            var model = _mapper.Map<ItemAtpFile>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);

            //同步生成明细表
            int row = model.RowsLimit.ObjToInt();
            int column = model.ColumnsLimit.ObjToInt();
            int disk = model.DiskCount.ObjToInt();
            if (row != 0 && column != 0 && disk != 0)
            {
                List<ItemAtpFileDetail> addList = new List<ItemAtpFileDetail>();
                int count = row * column * disk;

                for (int i = 1; i < count + 1; i++)
                {
                    var detailModel = new ItemAtpFileDetail();
                    detailModel.ItemAtpFileId = model.Id;
                    detailModel.OrderNum = i;
                    detailModel.CreateTime = DateTime.Now;
                    detailModel.CreatorId = UserId;
                    detailModel.Status = (int)DataStatusEnum.Enable;
                    addList.Add(detailModel);
                }
                await _detailService.BulkInsert(addList);

            }
            return Success();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public virtual async Task<ResponseDto<string>> UpdateCheck(AddOrUpdateItemAtpFileReq req)
        {
            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            var model = _mapper.Map<ItemAtpFile>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);

            //同步生成明细表
            int oldrow = entity.RowsLimit.ObjToInt();
            int oldcolumn = entity.ColumnsLimit.ObjToInt();
            int olddisk = entity.DiskCount.ObjToInt();

            int newrow = model.RowsLimit.ObjToInt();
            int newcolumn = model.ColumnsLimit.ObjToInt();
            int newdisk = model.DiskCount.ObjToInt();

            if (oldrow == newrow && oldcolumn == newcolumn)
            {
                if (olddisk < newdisk) //刀盘数变多，新增
                {
                    int start = newrow * newcolumn * olddisk;
                    int end = newrow * newcolumn * newdisk;
                    List<ItemAtpFileDetail> addList = new List<ItemAtpFileDetail>();

                    for (int i = start + 1; i <= end; i++)
                    {
                        var detailModel = new ItemAtpFileDetail();
                        detailModel.ItemAtpFileId = model.Id;
                        detailModel.OrderNum = i;
                        detailModel.CreateTime = DateTime.Now;
                        detailModel.CreatorId = UserId;
                        detailModel.Status = (int)DataStatusEnum.Enable;
                        addList.Add(detailModel);
                    }
                    await _detailService.BulkInsert(addList);

                }
                else if (olddisk > newdisk) //刀盘数减少，删除
                {
                    int start = newrow * newcolumn * newdisk;

                    //直接删除
                    await _detailService.DeleteAsync(p => p.ItemAtpFileId == req.Id && p.OrderNum > start);

                    //根据配置todo软删除
                }
            }
            else
            {
                //行和列任一个变化，删除已存在的明细，重新增加
                await _detailService.DeleteAsync(p => p.ItemAtpFileId == req.Id);

                if (newrow != 0 && newcolumn != 0 && newdisk != 0)
                {
                    int count = newrow * newcolumn * newdisk;
                    List<ItemAtpFileDetail> addList = new List<ItemAtpFileDetail>();

                    for (int i = 1; i < count + 1; i++)
                    {
                        var detailModel = new ItemAtpFileDetail();
                        detailModel.ItemAtpFileId = model.Id;
                        detailModel.OrderNum = i;
                        detailModel.CreateTime = DateTime.Now;
                        detailModel.CreatorId = UserId;
                        detailModel.Status = (int)DataStatusEnum.Enable;
                        addList.Add(detailModel);
                    }
                    await _detailService.BulkInsert(addList);

                }
            }
            return Success();
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
            var result = await _domainService.DeleteById(id);
            if (result)
            {
                await _detailService.DeleteAsync(p => p.ItemAtpFileId == id);
                return Success("删除成功");
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

                    deleteList[i] = idList[i];
                }

                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {
                    await _detailService.DeleteAsync(p => p.ItemAtpFileId != null && idList.Contains((long)p.ItemAtpFileId));
                    return Success("");
                }
            }
            return Fail("删除失败");

        }

        /// <summary>
        /// 生成ATP文件
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<string>> GenerateATP(string filePath, long Id)
        {
            var entity = await _domainService.FindSingleAsync(p => p.Id == Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            return Fail("TODO,尚未实现此功能!");

            //string fileName = string.IsNullOrEmpty(entity.ItemCode) ? "ATP" + DateTime.Now.ToShortDateString() + Id + ".atp" : entity.ItemCode + ".atp";
            //filePath = Path.Combine(filePath, fileName);

            //GenerateATPFileHelper generateATPFileHelper = new GenerateATPFileHelper();
            //generateATPFileHelper.GenerateATPfile(filePath);

            //if (File.Exists(filePath))
            //{
            //    entity.ModifierId = UserId;
            //    entity.ModifyTime = DateTime.Now;
            //    entity.ATPFilePath = filePath;
            //    entity.AtpFileName = fileName;
            //    entity.IsGenerated = 1;
            //    entity.GenerateTime = DateTime.Now;
            //    await _domainService.Update(entity);
            //}
            //return Success();
        }
    }
}