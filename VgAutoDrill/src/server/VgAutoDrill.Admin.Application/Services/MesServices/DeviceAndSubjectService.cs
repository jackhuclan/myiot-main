using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 设备点检项目模板
    /// </summary>
    public class DeviceAndSubjectService : BaseServiceWithoutTree<DeviceAndSubject, DeviceAndSubjectDto, AddOrUpdateDeviceAndSubjectReq>, IDeviceAndSubjectService
    {
        private readonly IDeviceAndSubjectDomainService _rdomainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public DeviceAndSubjectService(IDeviceAndSubjectDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _rdomainService = domainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceAndSubjectDto>>> GetList(GetDeviceAndSubjectListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _rdomainService.GetList(req);
            return Success(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddDatas(AddOrUpdateDeviceAndSubjectReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误");
            }
            if (req.DeviceId == 0 || req.SubjectIds == null || !req.SubjectIds.Any())
            {
                return Fail("设备ID和点检项目ID必填");
            }

            //删除原有关联关系
            await _domainService.DeleteAsync(p => p.DeviceId == req.DeviceId);

            List<DeviceAndSubject> addList = new List<DeviceAndSubject>();
            foreach (var subjectId in req.SubjectIds)
            {
                DeviceAndSubject model = new DeviceAndSubject();
                model.SubjectId = subjectId;
                model.DeviceId = req.DeviceId;
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.Status = (int)DataStatusEnum.Enable;

                addList.Add(model);
            }
            await _domainService.BulkInsert(addList);
            return Success();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateDatas(AddOrUpdateDeviceAndSubjectReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误");
            }
            if (req.DeviceId == 0 || req.SubjectIds == null || !req.SubjectIds.Any())
            {
                return Fail("设备ID和点检项目ID必填");
            }

            //删除模板中deviceID相关联数据，重新添加模板
            await _domainService.DeleteAsync(p => p.DeviceId == req.DeviceId);

            List<DeviceAndSubject> addList = new List<DeviceAndSubject>();
            foreach (var subjectId in req.SubjectIds)
            {
                DeviceAndSubject model = new DeviceAndSubject();
                model.SubjectId = subjectId;
                model.DeviceId = req.DeviceId;
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.Status = (int)DataStatusEnum.Enable;

                addList.Add(model);
            }

            await _domainService.BulkInsert(addList);
            return Success();
        }
    }
}