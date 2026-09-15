using AutoMapper;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Partition;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    ///
    /// </summary>
    public class PartitionService : BaseServiceWithTree<Partition, PartitionTreeDto, PartitionDto, AddOrUpdatePartitionReq>, IPartitionService
    {
        private readonly ILogger<PartitionService> _logger;
        private readonly IPartitionDomainService _partitionDomainService;

        private readonly IPartitionSettingManager _partitionSettingManager;

        /// <summary>
        ///
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public PartitionService(IPartitionDomainService domainService, IPartitionSettingManager partitionSettingManager,
                                           IMapper mapper, ILoggerFactory loggerFactory)
            : base(domainService, mapper)
        {
            _partitionDomainService = domainService;
            _partitionSettingManager = partitionSettingManager;
            _logger = loggerFactory.CreateLogger<PartitionService>();
        }

        public async Task<ResponseDto<Partition>> QueryWithSettingByID(object objId)
        {
            var data = await _partitionDomainService.QueryWithSettingByID(objId);
            return Success(data);
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<PartitionDto>>> GetList(GetPartitionListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<PartitionDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Partition>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.Contains(req.WorkStationCode));
            }

            if (!string.IsNullOrEmpty(req.WorkStationName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkStationName) && p.WorkStationName.Contains(req.WorkStationName));
            }

            if (!string.IsNullOrEmpty(req.Charge))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Charge) && p.Charge.Contains(req.Charge));
            }

            if (!string.IsNullOrEmpty(req.PreBookAgv))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.PreBookAgv) && p.PreBookAgv.ToLower() == req.PreBookAgv.ToLower());
            }

            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            if (req.PartitionKinds != null && req.PartitionKinds.Count > 0)
            {
                where = where.And(p => req.PartitionKinds.Contains(p.PartitionKind));
            }
            if (req.TransportationKind != null)
            {
                where = where.And(p => req.TransportationKind == p.TransportationKind);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<Partition>, List<PartitionDto>>(result.ToList());
            return Success(pageDto);
        }

        public async Task<ResponseDto<List<PartitionTreeDto>>> GetTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<PartitionTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }

            var allCodes = AddChildN(list, 0);

            result.Data = allCodes;
            return result;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdatePartitionReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (req.MinEmptyBoxNum < 0) return Fail("最小空仓数必须为大于或等于0的整数");

            if (req.MaxEmptyBoxNum < 0) return Fail("最多空仓数必须为大于或等于0的整数");

            //所有层级父节点
            var ancestors = "0";
            if (req.ParentId > 0)
            {
                var parentType = await _domainService.QueryByID(req.ParentId);
                if (parentType == null)
                {
                    return Fail<string>(" 父对象不存在!");
                }

                ancestors = parentType.Ancestors + "," + req.ParentId.ToString();
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Partition>(req);
            model.Ancestors = ancestors;
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _partitionDomainService.AddOrUpdateWithSetting(model);
            return Success();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdatePartitionReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (req.MinEmptyBoxNum < 0) return Fail("最小空仓数必须为大于或等于0的整数");

            if (req.MaxEmptyBoxNum < 0) return Fail("最多空仓数必须为大于或等于0的整数");

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

            //所有层级父节点
            var ancestors = "0";
            if (req.ParentId > 0)
            {
                var parentType = await _domainService.QueryByID(req.ParentId);
                if (parentType == null)
                {
                    return Fail<string>(" 父对象不存在!");
                }

                ancestors = parentType.Ancestors + "," + req.ParentId.ToString();
            }

            var model = _mapper.Map<Partition>(req);
            model.Ancestors = ancestors;
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        public async Task<ResponseDto<string>> SetStatus(string code, int status)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Fail("未识别有效的Code！");
            }

            var data = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == code.ToLower());
            if (data == null)
            {
                return Fail("未找到对应数据！");
            }

            data.Status = status;
            data.ModifierId = UserId;
            data.ModifyTime = DateTime.Now;
            await _domainService.Update(data);

            return Success();
        }

        public async Task<string> BookPart(string partitionCode, string agvCode)
        {
            if (string.IsNullOrEmpty(partitionCode))
            {
                return $"未识别有效的Code,{partitionCode}！";
            }

            var data = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == partitionCode.ToLower());
            if (data == null)
            {
                return $"未找到对应数据,{partitionCode}！";
            }

            if (data.Status == 0)
            {
                return $"此分区已停用,{partitionCode}！";
            }
            else if (!string.IsNullOrEmpty(data.PreBookAgv))
            {
                return $"此分区已被预约,{data.PreBookAgv}，{data.PreBookTime}！";
            }
            else
            {
                data.PreBookTime = DateTime.Now;
                data.PreBookAgv = agvCode;

                var result = await _domainService.Update(data);
                if (result)
                {
                    _logger.LogInformation($"{agvCode} 预约分区 {partitionCode} 成功！{DateTime.Now}");
                    return string.Empty;
                }
                else
                {
                    return "更新数据库时，预约失败，请稍后再试";
                }
            }
        }

        public async Task<string> UnBookPart(string partitionCode)
        {
            if (string.IsNullOrEmpty(partitionCode))
            {
                return $"未识别有效的Code,{partitionCode}！";
            }

            var data = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == partitionCode.ToLower());
            if (data == null)
            {
                return $"未找到对应数据,{partitionCode}！";
            }

            data.PreBookTime = null;
            data.PreBookAgv = string.Empty;
            data.ModifierId = UserId;
            data.ModifyTime = DateTime.Now;

            var result = await _domainService.Update(data);
            if (result)
            {
                _logger.LogInformation($"取消预约分区 {partitionCode} 成功！{DateTime.Now}");
                return string.Empty;
            }
            else
            {
                return "更新数据库时，取消预约失败，请稍后再试";
            }
        }
    }
}