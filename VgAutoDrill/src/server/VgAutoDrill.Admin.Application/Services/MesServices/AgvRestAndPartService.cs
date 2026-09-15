using AutoMapper;
using SqlSugar;
using System.Text;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class AgvRestAndPartService : BaseServiceWithoutTree<AgvRestAndPart, RestAndPartDto, AddOrUpdateRestAndPartReq>, IAgvRestAndPartService
    {
        private readonly IAgvRestAndPartDomainService _agvRestAndPartDomainService;
        private readonly IAgvRestDomainService _agvRestDomainService;
        private readonly IPartitionDomainService _partitionDomainService;
        private readonly IRouteDomainService _routeDomainService;
        private readonly ISysConfigManager _sysConfigManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public AgvRestAndPartService(IAgvRestAndPartDomainService domainService,
            IAgvRestDomainService agvRestDomainService,
            IMapper mapper,
            IPartitionDomainService partitionDomainService,
            IRouteDomainService routeDomainService,
            ISysConfigManager sysConfigManager)
         : base(domainService, mapper)
        {
            _agvRestAndPartDomainService = domainService;
            _agvRestDomainService = agvRestDomainService;
            _partitionDomainService = partitionDomainService;
            _routeDomainService = routeDomainService;
            _sysConfigManager = sysConfigManager;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RestAndPartDto>>> GetList(GetRestAndPartListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _agvRestAndPartDomainService.GetList(req);
            return Success(result);
        }

        /// <summary>
        ///获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<RestAndPartDto>> QueryDataByID(long id)
        {
            var result = await _agvRestAndPartDomainService.QueryByID(id);
            return Success(result);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateRestAndPartReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (string.IsNullOrEmpty(req.RestCode))
            {
                return Fail("未识别有效的休息点编码！");
            }

            var agvRestData = await _agvRestDomainService.FindSingleAsync(p => p.Code.ToLower() == req.RestCode.ToLower());
            if (agvRestData == null)
            {
                return Fail("未识别有效的休息点数据！");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.PartCode == req.PartCode && p.RestId == agvRestData.Id
            && p.RouteCode == req.RouteCode && p.AgvDeviceKind == req.AgvDeviceKind);
            if (isExsitCode)
            {
                return Fail($"休息点 {req.RestCode} 已存在相同的分区 {req.PartCode}、工艺路线 {req.RouteCode}、AGV类型 {req.AgvDeviceKind}!");
            }

            var model = _mapper.Map<AgvRestAndPart>(req);
            model.RestId = agvRestData.Id;
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
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateRestAndPartReq req)
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

            if (string.IsNullOrEmpty(req.RestCode))
            {
                return Fail("未识别有效的休息点编码！");
            }

            var agvRestData = await _agvRestDomainService.FindSingleAsync(p => p.Code.ToLower() == req.RestCode.ToLower());
            if (agvRestData == null)
            {
                return Fail("未识别有效的休息点数据！");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.PartCode == req.PartCode && p.RestId == agvRestData.Id
            && p.RouteCode == req.RouteCode && p.AgvDeviceKind == req.AgvDeviceKind && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail($"休息点 {req.RestCode} 已存在相同的分区 {req.PartCode}、工艺路线 {req.RouteCode}、AGV类型 {req.AgvDeviceKind}!");
            }

            var model = _mapper.Map<AgvRestAndPart>(req);
            model.RestId = agvRestData.Id;
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkInsert(List<RestAndPartToExcelDto> list)
        {
            if (list == null || list.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

            var restCodeList = list.Select(p => p.RestCode.ToLower()).Distinct().ToList();
            var restDatas = await _agvRestDomainService.QueryAsync(p => restCodeList.Contains(p.Code.ToLower()), p => p.Code, OrderByType.Asc);
            if (restDatas == null || restDatas.Count == 0)
            {
                return Fail("导入失败！无法识别休息点编码！");
            }

            var partCodeList = list.Select(p => p.PartCode.ToLower()).Distinct().ToList();
            var partDatas = await _partitionDomainService.QueryAsync(p => partCodeList.Contains(p.Code.ToLower()), p => p.Code, OrderByType.Asc);
            if (partDatas == null || partDatas.Count == 0)
            {
                return Fail("导入失败！无法识别分区编码！");
            }

            var routeCodeList = list.Select(p => p.RouteCode.ToLower()).Distinct().ToList();
            var routeDatas = await _routeDomainService.QueryAsync(p => routeCodeList.Contains(p.Code.ToLower()), p => p.Code, OrderByType.Asc);
            if (routeDatas == null || routeDatas.Count == 0)
            {
                return Fail("导入失败！无法识别工艺路线编码！");
            }

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<AgvRestAndPart> agvRests = new List<AgvRestAndPart>();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.RestCode) || string.IsNullOrEmpty(item.PartCode) || !Enum.IsDefined(typeof(DeviceKind), item.AgvDeviceKind))
                {
                    sb.Append($"休息点编码：{item.RestCode} 或分区编码：{item.PartCode} 为空；或AGV类型：{item.AgvDeviceKind} 无法识别；{Environment.NewLine}");
                    failCount++;
                    continue;
                }

                var agvRestExsist = restDatas.Exists(p => p.Code.ToLower() == item.RestCode.ToLower());
                if (!agvRestExsist)
                {
                    sb.Append($"数据库未找到 {item.RestCode} 对应的休息点数据！；{Environment.NewLine}");
                    failCount++;
                    continue;
                }
                var agvRestData = restDatas.SingleOrDefault(p => p.Code.ToLower() == item.RestCode.ToLower());
                if (agvRestData == null)
                {
                    sb.Append($"数据库未找到 {item.RestCode} 对应的休息点数据！；{Environment.NewLine}");
                    failCount++;
                    continue;
                }

                var partExsist = partDatas.Exists(p => p.Code.ToLower() == item.PartCode.ToLower());
                if (!partExsist)
                {
                    sb.Append($"数据库未找到 {item.PartCode} 对应的分区数据！；{Environment.NewLine}");
                    failCount++;
                    continue;
                }
                var partData = partDatas.SingleOrDefault(p => p.Code.ToLower() == item.RestCode.ToLower());
                if (partData == null)
                {
                    sb.Append($"数据库未找到 {item.PartCode} 对应的分区数据！；{Environment.NewLine}");
                    failCount++;
                    continue;
                }

                var routeExsist = routeDatas.Exists(p => p.Code.ToLower() == item.RouteCode.ToLower());
                if (!routeExsist)
                {
                    sb.Append($"数据库未找到 {item.RouteCode} 对应的工艺路线数据！；{Environment.NewLine}");
                    failCount++;
                    continue;
                }

                var isExsitCode = await _domainService.IsExistAsync(p => p.PartCode == item.PartCode && p.RestId == agvRestData.Id
                && p.RouteCode == item.RouteCode && p.AgvDeviceKind == (DeviceKind)item.AgvDeviceKind);
                if (isExsitCode)
                {
                    sb.Append($"休息点 {item.RestCode} 数据库已存在相同的分区 {item.PartCode}、工艺路线 {item.RouteCode}、AGV类型 {item.AgvDeviceKind}! {Environment.NewLine}");
                    failCount++;
                    continue;
                }

                if (agvRests.Exists(p => p.PartCode == item.PartCode && p.RestId == agvRestData.Id
                && p.RouteCode == item.RouteCode && p.AgvDeviceKind == (DeviceKind)item.AgvDeviceKind))
                {
                    sb.Append($"休息点 {item.RestCode} 导入列表中已存在相同的分区 {item.PartCode}、工艺路线 {item.RouteCode}、AGV类型 {item.AgvDeviceKind}! {Environment.NewLine}");
                    failCount++;
                    continue;
                }

                AgvRestAndPart model = new AgvRestAndPart();
                model.RestId = agvRestData.Id;
                model.PartCode = item.PartCode;
                model.PartName = partData.Name;
                model.RouteCode = item.RouteCode;
                model.AgvDeviceKind = (DeviceKind)item.AgvDeviceKind;
                model.Priority = item.Priority;
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
                agvRests.Add(model);
            }
            var result = await _domainService.BulkInsert(agvRests);
            if (!result)
            {
                return Fail("导入失败！");
            }

            string str = string.Format($"预计导入：{list.Count} 条；成功导入：{list.Count - failCount} 条；失败：{failCount} 条；{Environment.NewLine}");
            str = str + sb.ToString();

            return Success(str);
        }
    }
}
