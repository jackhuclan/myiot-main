using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class TransferJobLogService : BaseServiceWithoutTree<TransferJobLog, TransferJobLogDto, AddOrUpdateTransferJobLogReq>, ITransferJobLogService
    {
        private readonly ITransferJobLogDomainService _transferJobLogDomainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public TransferJobLogService(ITransferJobLogDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _transferJobLogDomainService = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<TransferJobLogDto>>> GetList(GetTransferJobLogListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _transferJobLogDomainService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkInsert(List<AddOrUpdateTransferJobLogReq> req)
        {
            if (req == null || req.Count == 0)
            {
                return Fail("信息格式错误!");
            }

            List<TransferJobLog> addList = new List<TransferJobLog>();
            foreach (var item in req)
            {
                if (item.MasterId == null || item.MasterId == 0)
                {
                    continue;
                }
                addList.Add(new TransferJobLog
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
            DateTime recordsTime = DateTime.Now.AddMonths(-3);
            await _domainService.DeleteAsync(p => p.CreateTime < recordsTime);
        }
    }
}

