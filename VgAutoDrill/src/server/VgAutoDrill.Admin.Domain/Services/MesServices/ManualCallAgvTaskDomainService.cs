using SqlSugar;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ManualCallAgvTaskDomainService : BaseDomainService<ManualCallAgvTask>, IManualCallAgvTaskDomainService
    {
        private readonly IManualCallAgvTaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ManualCallAgvTaskDomainService(
            IUnitOfWork unitOfWork,
          IManualCallAgvTaskRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _baseRepository = repository;
        }

        /// <summary>
        /// 根据钻机code查询手动呼叫agv任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        public async Task<List<ManualCallAgvTask>> QueryByDeviceCode(string deviceCode)
        {
            if (string.IsNullOrWhiteSpace(deviceCode))
            {
                return new List<ManualCallAgvTask>();
            }
            return await _repository.QueryAsync(s => s.DeviceCode!.ToLower() == deviceCode.ToLower(), s => s.Id, OrderByType.Asc);
        }


        /// <summary>
        /// 根据库位code查询任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        public async Task<ManualCallAgvTask> QueryByLocationCode(string locationCode)
        {
            if (string.IsNullOrWhiteSpace(locationCode))
            {
                return new ManualCallAgvTask();
            }
            return await _repository.FindSingleAsync(s => s.LocationCode!.ToLower() == locationCode.ToLower());
        }

        public async Task<List<ManualCallAgvTask>> QueryTask(QueryTaskReq req)
        {
            return await _repository.QueryAsync(s => (string.IsNullOrWhiteSpace(req.LocationCode) || s.LocationCode!.ToLower() == req.LocationCode.ToLower())
                                                   && (string.IsNullOrWhiteSpace(req.DeviceCode) || s.DeviceCode!.ToLower() == req.DeviceCode.ToLower())
                                                   , s => s.Id, OrderByType.Asc);

        }
    }
}
