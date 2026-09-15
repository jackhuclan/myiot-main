using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class DeviceServiceInvocationService : BaseServiceWithoutTree<DeviceServiceInvocation, DeviceServiceInvocationDto, AddOrUpdateDeviceServiceInvocationReq>, IDeviceServiceInvocationService
    {
        public readonly IDeviceServiceInvocationDomainService _deviceServiceInvocation;
        public readonly IUnitOfWork _unitOfWork;
        public DeviceServiceInvocationService(
            IDeviceServiceInvocationDomainService domainService,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(domainService, mapper)
        {
            _deviceServiceInvocation = domainService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<string>> AddDeviceServiceInvocationList(List<AddOrUpdateDeviceServiceInvocationReq> req)
        {
            if (req == null || req.Count() == 0)
            {
                return Fail("数据格式错误");
            }
            List<string> list = new List<string>();

            var Id = req.Select(t => t.Id);

            if (Id.Count() < req.Count())
            {
                return Fail("数据中有重复数据，不能操作");
            }

            List<long> lst = Id.Distinct().ToList();
            foreach (var item in Id.Distinct())
            {
                var temp = _domainService.IsExistAsync(t => t.Id == item).Result;
                if (temp)
                {
                    list.Add("Y");
                }
                else
                {
                    list.Add("N");
                }
            }

            list.Distinct();//list中的string == Y 时，更新数据，是N时，添加数据
            if (list.Count() > 1)
            {
                return Fail("批量操作不能既添加又更新数据");
            }

            if (list[0] == "Y")//更新数据
            {
                var res = await UpdateDate(req);
                return res;
            }
            else//添加数据
            {
                var res = await InsertDate(req);
                return res;
            }
        }

        private async Task<ResponseDto<string>> UpdateDate(List<AddOrUpdateDeviceServiceInvocationReq> req)
        {
            var db = _unitOfWork.GetDbClient();
            if (req == null || req.Count() == 0)
            {
                return Fail("数据格式错误");
            }
            List<DeviceServiceInvocation> lst = new List<DeviceServiceInvocation>();

            foreach (var device in req)
            {
                var result = await _domainService.IsExistAsync(t => t.Id == device.Id);
                if (!result)
                {
                    return Fail($"系统中不存在的数据：{device.MessageId},不能更新");
                }
            }

            for (int i = 0; i < req.Count(); i++)
            {
                var res = await _domainService.UpdateAsync(p => new DeviceServiceInvocation()
                {
                    Retries = req[i].Retries,
                    IsDealed = req[i].IsDealed,
                    IsTimeout = req[i].IsTimeout,
                    FirstInvocationTimestamp = req[i].FirstInvocationTimestamp,
                    LastInvocationTimestamp = req[i].LastInvocationTimestamp,
                    ModifierId = UserId,
                    ModifyTime = DateTime.Now
                }, p => p.Id == req[i].Id);
                if (!res)
                {
                    return Fail("更新数据失败");
                }
            }
            return Success();
        }

        private async Task<ResponseDto<string>> InsertDate(List<AddOrUpdateDeviceServiceInvocationReq> req)
        {
            if (req == null || req.Count() == 0)
            {
                return Fail("数据格式错误");
            }

            for (int i = 0; i < req.Count(); i++)
            {
                var result = await _domainService.IsExistAsync(t => t.Id == req[i].Id);
                if (result)
                {
                    return Fail($"系统中已经存在的数据：{req[i].Id},不能再次添加");
                }
            }
            List<DeviceServiceInvocation> lst = new List<DeviceServiceInvocation>();
            for (int i = 0; i < req.Count(); i++)
            {
                DeviceServiceInvocation model = new DeviceServiceInvocation();
                model.MessageId = req[i].MessageId;
                model.RequestTopic = req[i].RequestTopic;
                model.ResponseTopic = req[i].ResponseTopic;
                model.Payload = req[i].Payload;
                model.Retries = req[i].Retries;
                model.Reason = req[i].Reason;
                model.IsTimeout = false;
                model.IsDealed = false;
                model.MqttQualityOfServiceLevel = req[i].MqttQualityOfServiceLevel;
                //model.FirstInvocationTimestamp = req[i].FirstInvocationTimestamp;
                //model.LastInvocationTimestamp = req[i].LastInvocationTimestamp;
                model.IsUrgent = req[i].IsUrgent;
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.RoutingKey = req[i].RoutingKey;
                lst.Add(model);
            }
            var rest = await _domainService.BulkInsert(lst);
            if (!rest)
            {
                return Fail<string>("添加数据表失败");
            }
            return Success();
        }

        public async Task<List<DeviceServiceInvocationDto>> GetDeviceServiceInvocationList(GetDeviceServiceInvocationListReq req)
        {
            var db = _unitOfWork.GetDbClient();
            var query = db.Queryable<DeviceServiceInvocation>().Where(t => t.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.Reason))
            {
                query = query.Where(t => req.Reason == t.Reason);
            }

            if (!string.IsNullOrEmpty(req.MessageId))
            {
                query = query.Where(t => t.MessageId == req.MessageId);
            }

            if (!string.IsNullOrEmpty(req.RequestTopic))
            {
                query = query.Where(t => t.RequestTopic.Contains(req.RequestTopic));
            }

            if (!string.IsNullOrEmpty(req.ResponseTopic))
            {
                query = query.Where(t => t.ResponseTopic.Contains(req.ResponseTopic));
            }

            if (req.IsTimeout != true)
            {
                query = query.Where(t => t.IsTimeout == req.IsTimeout);
            }

            if (req.IsDealed != true)
            {
                query = query.Where(t => t.IsDealed == req.IsDealed);
            }

            var result = query.OrderByDescending(t => t.IsUrgent).OrderByDescending(t => t.Retries);
            var data = await query.Select(t => new DeviceServiceInvocationDto
            {
                Id = t.Id,
                MessageId = t.MessageId,
                RequestTopic = t.RequestTopic,
                ResponseTopic = t.ResponseTopic,
                Payload = t.Payload,
                Retries = t.Retries,
                Reason = t.Reason,
                IsUrgent = t.IsUrgent,
                FirstInvocationTimestamp = t.FirstInvocationTimestamp,
                LastInvocationTimestamp = t.LastInvocationTimestamp,
                CreateTime = t.CreateTime,
                RoutingKey = t.RoutingKey,
            }).ToListAsync();

            return data.ToList();
        }

        public async Task<ResponseDto<PageDto<DeviceServiceInvocationDto>>> GetDeviceServiceInvocations(GetDeviceServiceInvocationsReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DeviceServiceInvocationDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DeviceServiceInvocation>();
            where = where.And(p => p.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.MessageId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.MessageId) && p.MessageId.Contains(req.MessageId));
            }

            if (!string.IsNullOrEmpty(req.Reason))
            {
                where = where.And(t => req.Reason == t.Reason);
            }

            if (!string.IsNullOrEmpty(req.ResponseTopic))
            {
                where = where.And(p => p.ResponseTopic == req.ResponseTopic);
            }

            if (!string.IsNullOrEmpty(req.RequestTopic))
            {
                where = where.And(p => p.RequestTopic == req.RequestTopic);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Id, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DeviceServiceInvocation>, List<DeviceServiceInvocationDto>>(result.ToList());
            return Success(pageDto);
        }

        public async Task<ResponseDto<string>> DeleteDeviceServiceInvocationList(List<int> req)
        {
            if (req == null || req.Count() == 0)
            {
                return Fail("数据格式错误");
            }

            object[] tempArr = new object[req.Count()];
            for (int i = 0; i < req.Count(); i++)
            {

                var selectRes = _domainService.QueryByID(req[i]);
                if (selectRes.Result != null)
                {
                    var mopanel = _mapper.Map<DeviceServiceInvocation>(selectRes.Result);
                    tempArr[i] = mopanel.Id;
                }
            }
            if (tempArr.Length == 0)
            {
                return Fail("要删除数据不存在");
            }
            var result = await _domainService.DeleteByIds(tempArr);
            if (result)
            {
                return Success();
            }
            else
            {
                return Fail("删除失败");
            }
        }
        public async Task RegularDeleteData()
        {
            DateTime recordsTime = DateTime.Now.AddDays(-2);
            await _domainService.DeleteAsync(p => p.CreateTime < recordsTime);
        }
    }
}
