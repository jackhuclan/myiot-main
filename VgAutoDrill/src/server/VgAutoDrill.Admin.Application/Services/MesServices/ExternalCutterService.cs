using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{

    public class ExternalCutterService : BaseService, IExternalCutterService
    {
        private readonly ICutterGroupDomainService _domainService;
        private readonly ICutterCroupDetailDomainService _domainCutterDetailService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ExternalCutterService(ICutterGroupDomainService domainService,
            ICutterCroupDetailDomainService domainCutterDetailService,
            IMapper mapper)
        {
            _domainService = domainService;
            _mapper = mapper;
            _domainCutterDetailService = domainCutterDetailService;
        }

        public async Task<ResponseDto<string>> Check(ExternalCutterGroupReq req)
        {
            var result = await _domainService.FindSingleAsync(p => p.GroupNo == req.GroupNo && p.Status == 1 && p.IsDeleted == 0);
            if (result == null)
            {
                return Fail("未找到该配刀计划或者已变更！");
            }

            return Success();
        }

        public async Task<ResponseDto<string>> Lock(ExternalCutterGroupReq req)
        {
            var result = await _domainService.FindSingleAsync(p => p.GroupNo == req.GroupNo && p.Status == 1 && p.IsDeleted == 0);
            if (result == null)
            {
                return Fail("未找到该配刀计划或者已变更！");
            }

            result.CutterGroupStatus = CutterGroupStatusEnum.Locked;
            result.LockedTime = DateTime.Now;
            result.ModifyTime = DateTime.Now;
            await _domainService.Update(result);

            return Success();
        }

        public async Task<ResponseDto<string>> Unlock(ExternalCutterGroupReq req)
        {
            var result = await _domainService.FindSingleAsync(p => p.GroupNo == req.GroupNo && p.Status == 1 && p.IsDeleted == 0);
            if (result == null)
            {
                return Fail("未找到该配刀计划或者已变更！");
            }

            result.CutterGroupStatus = (int)CutterGroupStatusEnum.UnLocked;
            result.LockedTime = null;
            result.ModifyTime = DateTime.Now;
            await _domainService.Update(result);

            return Success();
        }

        public async Task<ResponseDto<List<ExternaCutterlTaskDto>>> GetPlanList(List<string> deviceCodes)
        {
            List<ExternaCutterlTaskDto> result = new List<ExternaCutterlTaskDto>();

            var cutterGroupInfo = await _domainService.QueryAsync(p => deviceCodes.Contains(p.DrillNo) && p.Status == 1 && p.IsDeleted == 0
            && p.CutterGroupStatus == 0,
            p => p.CreateTime,
            OrderByType.Asc);

            if (cutterGroupInfo != null && cutterGroupInfo.Any())
            {
                var cutterDetailInfo = await _domainCutterDetailService.QueryAsync(p => p.Status == 1 && p.IsDeleted == 0
                && cutterGroupInfo.Select(t => t.GroupNo).Contains(p.CutterGroupNo), p => p.Id, OrderByType.Desc);

                result = _mapper.Map<List<ExternaCutterlTaskDto>>(cutterGroupInfo);
                result.ForEach(p =>
                {
                    var cutterDetail = cutterDetailInfo.Where(c => c.CutterGroupNo == p.GroupNo).ToList();
                    if (cutterDetail != null && cutterDetail.Any())
                    {
                        p.CutterGroupInfo = cutterDetail.Select(t => new CutterGroupDetails()
                        {
                            ItemCode = t.ItemCode,
                            Count = t.PanelNum

                        }).ToList();
                    }
                });
            }

            return Success(result);
        }
    }
}
