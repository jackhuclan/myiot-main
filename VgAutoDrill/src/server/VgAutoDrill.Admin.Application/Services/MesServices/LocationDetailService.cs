using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class LocationDetailService : BaseServiceWithoutTree<LocationDetail, LocationDetailDto, AddOrUpdateLocationDetailReq>, ILocationDetailService
    {
        private readonly ILocationDetailDomainService _locationDetailDomainService;
        private readonly ILocationDetailRepository _locationDetailRepository;

        public LocationDetailService(ILocationDetailDomainService locationDetailDomainService,
            IMapper mapper,
            ILocationDetailRepository locationDetailRepository,
            IUnitOfWork unitWork) : base(locationDetailDomainService, mapper)
        {
            _locationDetailDomainService = locationDetailDomainService;
            _locationDetailRepository = locationDetailRepository;
        }

        public override async Task<ResponseDto<string>> Add(AddOrUpdateLocationDetailReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的数据！");
            }
            if (string.IsNullOrEmpty(req.Code))
            {
                return Fail("库位编码不能为空！");
            }

            var existingRecord = await _locationDetailRepository.GetByCodeAsync(req.Code);
            if (existingRecord.Any(x => x.FloorNum == req.FloorNum))
            {
                return Fail($"库位编码 {req.Code} 的层数 {req.FloorNum} 已存在！");
            }

            var model = _mapper.Map<LocationDetail>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            var result = await _locationDetailDomainService.Add(model);
            if (!result)
            {
                return Fail("新增失败！");
            }

            return Success();
        }

        public async Task<ResponseDto<string>> AddBatch(List<AddOrUpdateLocationDetailReq> req)
        {
            if (req == null || !req.Any())
            {
                return Fail("未识别有效的数据！");
            }

            var models = new List<LocationDetail>();
            foreach (var item in req)
            {
                if (string.IsNullOrEmpty(item.Code))
                {
                    return Fail("库位编码不能为空！");
                }

                var model = _mapper.Map<LocationDetail>(item);
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                models.Add(model);
            }

            var result = await _locationDetailDomainService.BulkInsert(models);
            if (!result)
            {
                return Fail("批量新增失败！");
            }

            return Success();
        }

        public override async Task<ResponseDto<string>> Update(AddOrUpdateLocationDetailReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的数据！");
            }
            if (string.IsNullOrEmpty(req.Code))
            {
                return Fail("库位编码不能为空！");
            }

            var data = await _locationDetailDomainService.QueryByID(req.Id);
            if (data == null)
            {
                return Fail($"未找到ID为 {req.Id} 的库位明细记录！");
            }

            var model = _mapper.Map<LocationDetail>(req);
            model.CreateTime = data.CreateTime;
            model.CreatorId = data.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            var result = await _locationDetailDomainService.Update(model);
            if (!result)
            {
                return Fail("更新失败！");
            }

            return Success();
        }

        public async Task<LocationDetailDto> FindSingle(string code, int? floorNum)
        {
            var data = await _locationDetailRepository.GetByCodeAsync(code);
            var record = data.FirstOrDefault(x => x.FloorNum == floorNum);
            return _mapper.Map<LocationDetailDto>(record);
        }

        public async Task<ResponseDto<List<LocationDetailDto>>> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Fail<List<LocationDetailDto>>("库位编码不能为空！");
            }

            var data = await _locationDetailRepository.GetByCodeAsync(code);
            var result = _mapper.Map<List<LocationDetailDto>>(data);
            return Success(result);
        }

        public async Task<ResponseDto<PageDto<LocationDetailDto>>> GetList(LocationDetailQueryReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var where = PredicateBuilder.True<LocationDetail>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => p.Code.Contains(req.Code));
            }
            if (!string.IsNullOrEmpty(req.Panel))
            {
                where = where.And(p => p.Panel.Contains(req.Panel));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => p.ItemCode.Contains(req.ItemCode));
            }
            if (req.FloorNum.HasValue)
            {
                where = where.And(p => p.FloorNum == req.FloorNum);
            }
            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                where = where.And(p => p.PanelCode.Contains(req.PanelCode));
            }
            if (req.ProductStatus.HasValue)
            {
                where = where.And(p => p.ProductStatus == req.ProductStatus);
            }

            var result = await _locationDetailDomainService.QueryPageAsync(
                where, 
                p => p.Code, 
                OrderByType.Asc, 
                req.PageNum, 
                req.PageSize);

            var data = _mapper.Map<List<LocationDetailDto>>(result.ToList());

            foreach (var item in data)
            {
                item.ProductStatusDesc = item.ProductStatus.Value.GetProductStatusDesc();
            }

            return Success(new PageDto<LocationDetailDto>(req.PageNum, req.PageSize)
            {
                Total = result.TotalCount,
                List = data
            });
        }
    }
}
