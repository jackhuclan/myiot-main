using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CheckRecords;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 检验记录
    /// </summary>
    public class CheckRecordsService : BaseServiceWithoutTree<CheckRecords, CheckRecordsDto, AddOrUpdateCheckRecordsReq>, ICheckRecordsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public CheckRecordsService(ICheckRecordsDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {

        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<CheckRecordsDto>>> GetList(GetCheckRecordsListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<CheckRecordsDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<CheckRecords>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.UserName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.UserName) && p.UserName.Contains(req.UserName));
            }

            if (!string.IsNullOrEmpty(req.IsCheckOk))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.IsCheckOk) && p.IsCheckOk.Equals(req.IsCheckOk));
            }

            if (!string.IsNullOrEmpty(req.TaskName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.TaskName) && p.TaskName.Contains(req.TaskName));
            }

            if (!string.IsNullOrEmpty(req.TaskCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.TaskCode) && p.TaskCode.Contains(req.TaskCode));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderName) && p.WorkOrderName.Contains(req.WorkOrderName));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.Contains(req.WorkOrderCode));
            }

            if (req.CheckTime != null)
            {
                where = where.And(p => p.CheckTime != null && p.CheckTime.Value.Date.Equals(req.CheckTime.Value.Date));
            }

            if (req.TaskId > 0)
            {
                where = where.And(p => p.TaskId == req.TaskId);
            }

            if (req.WorkOrderId > 0)
            {
                where = where.And(p => p.WorkOrderId == req.WorkOrderId);
            }

            if (req.UserId > 0)
            {
                where = where.And(p => p.UserId == req.UserId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<CheckRecords>, List<CheckRecordsDto>>(result.ToList());
            return Success<PageDto<CheckRecordsDto>>(pageDto);
        }

        /// <summary>
        /// 更新检验结果
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateCheck(UpdateCheckRecordsOkReq req)
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

            if (!string.IsNullOrEmpty(entity.IsCheckOk) && entity.IsCheckOk != "-1")
            {
                //已经检验过的数据，仅质检组长可变更

            }

            entity.CheckTime = DateTime.Now;
            entity.IsCheckOk = req.IsCheckOk;
            entity.Remark = req.Remark;
            entity.UserId = UserId;
            entity.UserName = UserName;
            await _domainService.Update(entity);

            return Success();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateCheckRecordsReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误！");
            }
            if (req.TaskId > 0)
            {
                var result = await _domainService.IsExistAsync(p => p.TaskId == req.TaskId && p.IsCheckOk == "-1");
                if (result)
                {
                    return Fail("检验记录不能重复生成！");
                }
            }

            var model = _mapper.Map<CheckRecords>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            if (string.IsNullOrEmpty(model.IsCheckOk) || model.IsCheckOk == "-1")
            {
                model.IsCheckOk = "-1";
            }
            else
            {
                model.CheckTime = DateTime.Now;
            }
            await _domainService.Add(model);
            return Success();
        }
    }
}

