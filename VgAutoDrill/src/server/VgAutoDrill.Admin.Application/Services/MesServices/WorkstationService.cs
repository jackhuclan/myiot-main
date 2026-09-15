using AutoMapper;
using System.Text;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkstationService : BaseServiceWithoutTree<WorkStation, WorkstationDto, AddOrUpdateWorkstationReq>, IWorkstationService
    {
        private readonly IWorkstationDomainService _workstationDomainService;
        private readonly IDeviceDomainService _deviceDomainService;
        private readonly IRouteProcessAndWorkStationDomainService _routeProcessAndWorkStationDomainService;
        private readonly ISysConfigManager _sysConfigManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        /// <param name="deviceDomainService"></param>
        /// <param name="routeProcessAndWorkStationDomainService"></param>
        public WorkstationService(IWorkstationDomainService domainService,
            IMapper mapper,
            IDeviceDomainService deviceDomainService,
            IRouteProcessAndWorkStationDomainService routeProcessAndWorkStationDomainService,
            ISysConfigManager sysConfigManager)
            : base(domainService, mapper)
        {
            _workstationDomainService = domainService;
            _deviceDomainService = deviceDomainService;
            _routeProcessAndWorkStationDomainService = routeProcessAndWorkStationDomainService;
            _sysConfigManager = sysConfigManager;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<WorkstationDto>>> GetList(GetWorkstationListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<WorkstationDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<WorkStation>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }
            if (req.WorkshopId > -1)
            {
                where = where.And(p => p.WorkshopId == req.WorkshopId);
            }
            if (!string.IsNullOrEmpty(req.WorkshopCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkshopCode) && p.WorkshopCode.Contains(req.WorkshopCode));
            }
            if (!string.IsNullOrEmpty(req.WorkshopName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkshopName) && p.WorkshopName.Contains(req.WorkshopName));
            }

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.Equals(req.ProcessCode));
            }
            if (!string.IsNullOrEmpty(req.ProcessName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessName) && p.ProcessName.Contains(req.ProcessName));
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<WorkStation>, List<WorkstationDto>>(result.ToList());
            return Success<PageDto<WorkstationDto>>(pageDto);
        }

        /// <summary>
        /// 设备负载查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<WorkStationLoadTaskDto>>> GetWorkStationLoadTask(GetWorkStationLoadTaskReq req)
        {
            if (req == null)
            {
                return Fail<PageDto<WorkStationLoadTaskDto>>("未识别有效的入参！");
            }
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _workstationDomainService.GetWorkStationLoadTask(req);
            return Success(result);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkInsert(List<WorkstationToExcelDto> list)
        {
            if (list == null || list.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<WorkStation> workstations = new List<WorkStation>();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Code) || string.IsNullOrEmpty(item.Name))
                {
                    sb.Append("编码：" + item.Code + " 或名称：" + item.Name + " 为空；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                var isExsitCode = await _domainService.IsExistAsync(p => p.Code == item.Code);
                if (isExsitCode)
                {
                    sb.Append("编码" + item.Code + " 数据库已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                if (workstations.Exists(p => p.Code == item.Code))
                {
                    sb.Append("编码" + item.Code + " 导入列表中已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                WorkStation model = new WorkStation();
                model.Code = item.Code;
                model.Name = item.Name;
                model.WorkshopCode = item.WorkshopCode;
                model.WorkshopName = item.WorkshopName;
                model.WorkshopId = item.WorkshopId;
                model.Remark = item.Remark;
                if (importStatus)
                {
                    model.Status = 1;
                }
                else
                {
                    model.Status = 0;
                }
                model.CreatorId = UserId;
                model.CreateTime = DateTime.Now;
                workstations.Add(model);
            }
            var result = await _domainService.BulkInsert(workstations);
            if (!result)
            {
                return Fail("导入失败！");
            }

            string str = string.Format("预计导入：{0} 条；成功导入：{1} 条；失败：{2} 条；\r\n", list.Count, list.Count - failCount, failCount);
            str = str + sb.ToString();

            return Success(str);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateWorkstationReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<WorkStation>(req);
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
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateWorkstationReq req)
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

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<WorkStation>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
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

            bool isExsit = await _deviceDomainService.IsExistAsync(p => p.WorkStationId == id && p.IsDeleted == 0);
            if (isExsit)
            {
                return Fail("存在关联的设备未删除!");
            }

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                return Success("");
            }
            return Fail("删除失败");
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteDataList(List<long> idList)
        {
            if (idList == null)
            {
                return Fail("信息不存在!");
            }

            object[] deleteList = new object[idList.Count];
            for (int i = 0; i < idList.Count; i++)
            {
                var entity = await _domainService.QueryByID(idList[i]);
                if (entity == null)
                {
                    continue;
                }

                bool isExsit = await _deviceDomainService.IsExistAsync(p => p.WorkStationId == idList[i] && p.IsDeleted == 0);
                if (isExsit)
                {
                    return Fail(idList[i] + " 存在关联的设备未删除!");
                }

                deleteList[i] = idList[i];
            }

            var result = await _domainService.DeleteByIds(deleteList);
            if (!result)
            {
                return Fail("删除失败");
            }

            return Success("删除成功");
        }
    }
}
