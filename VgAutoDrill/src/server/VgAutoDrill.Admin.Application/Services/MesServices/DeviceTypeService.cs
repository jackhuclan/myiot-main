using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType;
using VgAutoDrill.Admin.Model.ViewModels.Req.Equipment;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 设备管理
    /// </summary>
    public class DeviceTypeService : BaseServiceWithTree<DeviceType, DeviceTypeTreeDto, DeviceTypeInfoDto, AddOrUpdateDeviceTypeReq>, IDeviceTypeService
    {
        private readonly IDeviceDomainService _deviceDomainService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="deviceDomainService"></param>
        /// <param name="mapper"></param>
        public DeviceTypeService(IDeviceTypeDomainService domainService, IDeviceDomainService deviceDomainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _deviceDomainService = deviceDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceTypeDto>>> GetEquipmentTypeList(GetEquipmentTypeListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DeviceTypeDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DeviceType>();
            where = where.And(p => p.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }
            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DeviceType>, List<DeviceTypeDto>>(result.ToList());
            return Success(pageDto);
        }

        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<DeviceTypeTreeDto>>> GetEquipmentTypeTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<DeviceTypeTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }

            var allCodes = AddChildN(list, 0);

            result.Data = allCodes;
            return result;
        }

        /// <summary>
        /// 删除信息
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

            var isExsitDevice = await _deviceDomainService.IsExistAsync(p => p.DeviceTypeId == id);
            if (isExsitDevice)
            {
                return Fail("存在设备信息，不允许删除!");
            }

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                return Success("");
            }
            return Fail("删除失败");
        }

        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteDataList(List<long> idList)
        {
            if (idList != null)
            {
                object[] deleteList = new object[idList.Count];
                for (int i = 0; i < idList.Count; i++)
                {
                    var entity = await _domainService.QueryByID(idList[i]);
                    if (entity == null)
                    {
                        continue;
                    }

                    var isExsitDevice = await _deviceDomainService.IsExistAsync(p => p.DeviceTypeId == idList[i]);
                    if (isExsitDevice)
                    {
                        return Fail(entity.Code + " 存在设备信息，不允许删除!");
                    }

                    deleteList[i] = idList[i];
                }

                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {
                    return Success("");
                }
            }
            return Fail("删除失败");
        }
    }
}
