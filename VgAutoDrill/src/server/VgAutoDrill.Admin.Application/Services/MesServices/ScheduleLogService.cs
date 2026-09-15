using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class ScheduleLogService : BaseServiceWithoutTree<ScheduleLog, ScheduleLogDto, AddOrUpdateScheduleLogReq>, IScheduleLogService
    {
        private readonly IScheduleLogDomainService _schedulementDetailDomainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public ScheduleLogService(IScheduleLogDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _schedulementDetailDomainService = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ScheduleLogDto>>> GetList(GetScheduleLogListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _schedulementDetailDomainService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkInsert(List<AddOrUpdateScheduleLogReq> req)
        {
            if (req == null || req.Count == 0)
            {
                return Fail("信息格式错误!");
            }

            List<ScheduleLog> addList = new List<ScheduleLog>();
            foreach (var item in req)
            {
                if (item.MasterId == null || item.MasterId == 0)
                {
                    continue;
                }
                addList.Add(new ScheduleLog
                {
                    MasterId = item.MasterId,
                    Message = item.Message.Length > 5000 ? item.Message.Substring(0, 5000) : item.Message,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                    Status = (int)DataStatusEnum.Enable,
                });
            }

            await _domainService.BulkInsert(addList);

            return Success();
        }
        public async Task RegularDeleteData()
        {
            DateTime recordsTime = DateTime.Now.AddMonths(-2);
            await _domainService.DeleteAsync(p => p.CreateTime < recordsTime);
        }
    }
}
