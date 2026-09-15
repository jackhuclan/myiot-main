using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Text;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Services
{
    public class AgvRestService : BaseServiceWithoutTree<AgvRest, RestCodeDto, AddOrUpdateRestCodeReq>, IAgvRestService
    {
        private readonly ILogger<AgvRestService> _logger;
        private readonly IAgvRestAndPartDomainService _agvRestAndPartDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public AgvRestService(IAgvRestDomainService domainService,
            IMapper mapper,
            ILoggerFactory loggerFactory,
            IAgvRestAndPartDomainService agvRestAndPartDomainService,
            ISysConfigManager sysConfigManager)
            : base(domainService, mapper)
        {
            _logger = loggerFactory.CreateLogger<AgvRestService>();
            _agvRestAndPartDomainService = agvRestAndPartDomainService;
            _sysConfigManager = sysConfigManager;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<RestCodeDto>>> GetList(GetRestCodeListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<RestCodeDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<AgvRest>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Contains(req.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }
            if (!string.IsNullOrEmpty(req.PreBookAgv))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.PreBookAgv) && p.PreBookAgv.ToLower().Contains(req.PreBookAgv.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.CurrentAgv))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.CurrentAgv) && p.CurrentAgv.ToLower().Contains(req.CurrentAgv.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.Point))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Point) && p.Point.Equals(req.Point));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<AgvRest>, List<RestCodeDto>>(result.ToList());
            return Success<PageDto<RestCodeDto>>(pageDto);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateRestCodeReq req)
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

            var model = _mapper.Map<AgvRest>(req);
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
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateRestCodeReq req)
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

            var model = _mapper.Map<AgvRest>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        public async Task<string> BookAgvRest(string restCode, string agvCode)
        {
            if (string.IsNullOrEmpty(restCode))
            {
                return $"未识别有效的Code,{restCode}！";
            }

            if (string.IsNullOrEmpty(agvCode))
            {
                return $"未识别有效的Code,{agvCode}！";
            }

            var data = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == restCode.ToLower());
            if (data == null)
            {
                return $"未找到对应数据,{restCode}！";
            }

            if (data.Status == 0)
            {
                return $"此休息点已停用,{restCode}！";
            }
            else if (!string.IsNullOrEmpty(data.PreBookAgv))
            {
                return $"此休息点已被预约,{data.PreBookAgv}，{data.PreBookTime}！";
            }
            else
            {
                data.PreBookTime = DateTime.Now;
                data.PreBookAgv = agvCode;

                var result = await _domainService.Update(data);
                if (result)
                {
                    _logger.LogInformation($"{agvCode} 预约休息点 {restCode} 成功！{DateTime.Now}");
                    return string.Empty;
                }
                else
                {
                    return "更新数据库时，预约失败，请稍后再试";
                }
            }
        }

        public async Task<string> UnBookAgvRest(string restCode)
        {
            if (string.IsNullOrEmpty(restCode))
            {
                return $"未识别有效的Code,{restCode}！";
            }

            var data = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == restCode.ToLower());
            if (data == null)
            {
                return $"未找到对应数据,{restCode}！";
            }

            data.PreBookTime = null;
            data.PreBookAgv = string.Empty;
            data.ModifierId = UserId;
            data.ModifyTime = DateTime.Now;

            var result = await _domainService.Update(data);
            if (result)
            {
                _logger.LogInformation($"取消预约休息点 {restCode} 成功！{DateTime.Now}");
                return string.Empty;
            }
            else
            {
                return "更新数据库时，取消预约失败，请稍后再试";
            }
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

            await _agvRestAndPartDomainService.UpdateAsync(p => new AgvRestAndPart
            {
                Status = status,
            }, p => p.RestId == data.Id);

            return Success();
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkInsert(List<AgvRestToExcelDto> list)
        {
            if (list == null || list.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<AgvRest> agvRests = new List<AgvRest>();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Code) || string.IsNullOrEmpty(item.Name))
                {
                    sb.Append($"编码：{item.Code} 或名称：{item.Name} 为空；{Environment.NewLine}");
                    failCount++;
                    continue;
                }

                var isExsitCode = await _domainService.IsExistAsync(p => p.Code == item.Code);
                if (isExsitCode)
                {
                    sb.Append($"编码 {item.Code} 数据库已存在；{Environment.NewLine}");
                    failCount++;
                    continue;
                }

                if (agvRests.Exists(p => p.Code == item.Code))
                {
                    sb.Append($"编码 {item.Code} 导入列表中已存在；{Environment.NewLine}");
                    failCount++;
                    continue;
                }

                AgvRest model = new AgvRest();
                model.Code = item.Code;
                model.Name = item.Name;
                model.Point = item.Point;
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
