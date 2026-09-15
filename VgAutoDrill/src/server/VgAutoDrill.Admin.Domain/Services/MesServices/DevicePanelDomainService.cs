using SqlSugar;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DevicePanelDomainService : BaseDomainService<DevicePanel>, IDevicePanelDomainService
    {
        private readonly IDevicePanelRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DevicePanelDomainService(IUnitOfWork unitOfWork,
            IDevicePanelRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 清空指定设备的负载panel
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public async Task<int> Clear(string deviceCode)
        {
            var db = _unitOfWork.GetDbClient();
            string strSql = @"DELETE  from t_device_panel where device_code=@deviceCode";
            var paramList = new List<SugarParameter>
            {
                  new SugarParameter("@deviceCode",deviceCode,System.Data.DbType.String),
            };

            return await db.Ado.ExecuteCommandAsync(strSql, paramList);
        }
    }
}
