using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.NotificationRecord;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class NotifyService : BaseServiceWithoutTree<Notify, NotifyDto, AddOrUpdateNotifyReq>, INotifyService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public NotifyService(INotifyDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<NotifyDto>>> GetList(GetNotifyListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<NotifyDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Notify>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.NotifyCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.NotifyCode) && p.NotifyCode.Contains(req.NotifyCode));
            }
            if (!string.IsNullOrEmpty(req.NotifyName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.NotifyName) && p.NotifyName.Contains(req.NotifyName));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<Notify>, List<NotifyDto>>(result.ToList());
            return Success(pageDto);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateNotifyReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.NotifyCode == req.NotifyCode);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Notify>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);
            return Success();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateNotifyReq req)
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

            var isExsitCode = await _domainService.IsExistAsync(p => p.NotifyCode == req.NotifyCode && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Notify>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }
    }
}
