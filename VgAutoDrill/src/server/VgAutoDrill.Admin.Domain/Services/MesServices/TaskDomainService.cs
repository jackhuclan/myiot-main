using Mapster;
using SqlSugar;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class TaskDomainService : BaseDomainService<Model.Entites.Mes.WorkTask>, ITaskDomainService
    {
        private readonly ITaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TaskDomainService(IUnitOfWork unitOfWork,
            ITaskRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<TaskDto>> GetList(GetTaskListReq req)
        {
            var pageDto = new PageDto<TaskDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList();
            return pageDto;
        }

        //public async Task<PageDto<WorkTask>> List(GetTaskListReq req)
        //{
        //    var pageDto = new PageDto<WorkTask>(req.PageNum, req.PageSize);
        //    var result = await _repository.GetList(req);
        //    pageDto.Total = result.TotalCount;
        //    pageDto.List = result.ToList();
        //    return pageDto;
        //}

        public async Task<List<WorkTask>> GetDrillTaskList(GetDrillTaskReq req)
        {
            return await _repository.GetDrillTaskList(req);
        }
        public async Task<RouteInfoAndProcessInfo> GetRouteAndProcessList(GetRouteAndProcessByItemReq req)
        {
            var result = await _repository.GetRouteAndProcessList(req);
            return result;
        }

        public async Task<PageDto<TaskDto>> GetEquipmentList(GetTaskListReq req)
        {
            var pageDto = new PageDto<TaskDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetEquipmentList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<TaskDto>>();
            return pageDto;
        }

        public async Task<PageDto<FitWorkStationDto>> GetFitWorkStationList(GetFitWorkStationListReq req)
        {
            var pageDto = new PageDto<FitWorkStationDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetFitWorkStationList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<FitWorkStationDto>>();
            return pageDto;
        }

        public async Task<PageDto<TaskDto>> GetTaskByDevice(GetDrillOrAgvDeviceInfoReq req)
        {
            var pageDto = new PageDto<TaskDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetTaskByDevice(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<TaskDto>>();
            return pageDto;
        }


        public async Task<TaskDto> GetTaskByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return new TaskDto();
            }
            var result = await _repository.FindSingleAsync(s => s.Code!.ToLower() == code.ToLower() && s.IsDeleted == 0);
            return result.Adapt<TaskDto>();
        }
    }
}
