using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Collections.Concurrent;
using System.ComponentModel;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SysConfigService : BaseServiceWithoutTree<SysConfig, SysConfigDto, AddOrUpdateSysConfigReq>, ISysConfigService
    {
        private static ConcurrentDictionary<string, SysConfig> _sysConfigs = new();

        private readonly List<string> configType;
        private readonly IAlarmDomainService _alarmDomainService;
        private readonly ILogger<SysConfigService> _logger;
        private readonly IOptions<InnerOptions> _innerOptions;
        private List<string> _noAlarmForUsers = new List<string>();
        private List<string> _sampleOrderForOnlyNotifyUsers = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public SysConfigService(ISysConfigDomainService domainService,
            IAlarmDomainService alarmDomainService,
            IMapper mapper,
            IOptions<InnerOptions> options,
            ILogger<SysConfigService> logger)
            : base(domainService, mapper)
        {
            configType = new List<string> { "int", "string", "enum", "bool" };
            _alarmDomainService = alarmDomainService;
            _logger = logger;
            _innerOptions = options;
            var innerOptions = options.Value;
            if (innerOptions.NoAlarmForUsers.Any())
            {
                var users = innerOptions.NoAlarmForUsers.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (users.Any())
                {
                    _noAlarmForUsers.AddRange(users);
                }
            }

            if (!string.IsNullOrEmpty(innerOptions.SampleOrderForOnlyNotifyUsers))
            {
                var users = innerOptions.SampleOrderForOnlyNotifyUsers.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (users.Any())
                {
                    _sampleOrderForOnlyNotifyUsers.AddRange(users);
                }
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<SysConfigDto>>> GetList(GetSysConfigListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var pageDto = new PageDto<SysConfigDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<SysConfig>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ConfigCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.Contains(req.ConfigCode));
            }
            if (req.IsQuickConfig.HasValue)
            {
                where = where.And(p => p.IsQuickConfig == req.IsQuickConfig);
            }
            if (req.IsSystem.HasValue)
            {
                where = where.And(p => p.IsSystem == req.IsSystem);
            }
            if (req.Category.HasValue)
            {
                where = where.And(p => p.Category == req.Category);
            }
            if (!string.IsNullOrEmpty(req.ConfigDescript))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ConfigDescript) && p.ConfigDescript.Contains(req.ConfigDescript));
            }
            var result = await _domainService.QueryPageAsync(where, p => p.ConfigCode, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<SysConfig>, List<SysConfigDto>>(result.ToList());
            return Success(pageDto);
        }

        public async Task<ResponseDto<List<SysConfigTreeDto>>> GetTreeList(GetSysConfigListReq req)
        {
            var returnData = new List<SysConfigTreeDto>();
            var where = PredicateBuilder.True<SysConfig>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ConfigCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.Contains(req.ConfigCode));
            }
            if (!string.IsNullOrEmpty(req.ConfigDescript))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ConfigDescript) && p.ConfigDescript.Contains(req.ConfigDescript));
            }
            if (req.IsQuickConfig.HasValue)
            {
                where = where.And(p => p.IsQuickConfig == req.IsQuickConfig);
            }
            if (req.IsSystem.HasValue)
            {
                where = where.And(p => p.IsSystem == req.IsSystem);
            }
            if (req.Category.HasValue && req.Category != SysConfigCategoryEnum.None)
            {
                switch (req.Category)
                {
                    case SysConfigCategoryEnum.Default:
                        where = where.And(p => p.Category == SysConfigCategoryEnum.Default || p.Category == SysConfigCategoryEnum.Device
                        || p.Category == SysConfigCategoryEnum.Central || p.Category == SysConfigCategoryEnum.Alarm
                        || p.Category == SysConfigCategoryEnum.WorkOrderTask);
                        break;

                    default:
                        where = where.And(p => p.Category == req.Category);
                        break;
                }
            }

            var result = await _domainService.QueryAsync(where, p => p.ConfigCode, SqlSugar.OrderByType.Asc);

            if (result == null || result.Count == 0)
            {
                return Success(returnData);
            }

            returnData.Add(new SysConfigTreeDto
            {
                Id = int.MaxValue,
                ConfigCode = "000",
                ConfigValue = "全部",
                ConfigDescript = "全部",
                Category = SysConfigCategoryEnum.None,
                Children = new List<SysConfigTreeDto>(),
            });

            var categorys = result.GroupBy(p => p.Category).ToList();
            int idData = 1;
            List<SysConfigTreeDto> defaultChildren = new List<SysConfigTreeDto>();
            foreach (var item in categorys)
            {
                if (!Enum.IsDefined(typeof(SysConfigCategoryEnum), item.Key))
                {
                    continue;
                }

                var cateData = _mapper.Map<List<SysConfig>, List<SysConfigTreeDto>>(item.ToList());
                string configCode = Enum.GetName(typeof(SysConfigCategoryEnum), item.Key);
                string descript = GetDescriptionByEnum((SysConfigCategoryEnum)item.Key);

                if (item.Key == SysConfigCategoryEnum.Device
                    || item.Key == SysConfigCategoryEnum.Central
                    || item.Key == SysConfigCategoryEnum.Alarm
                    || item.Key == SysConfigCategoryEnum.WorkOrderTask)
                {
                    if (!returnData[0].Children.Exists(p => p.ConfigCode == Enum.GetName(typeof(SysConfigCategoryEnum), SysConfigCategoryEnum.Default)))
                    {
                        string strCode = Enum.GetName(typeof(SysConfigCategoryEnum), SysConfigCategoryEnum.Default);
                        string strDescipt = GetDescriptionByEnum(SysConfigCategoryEnum.Default);
                        returnData[0].Children.Add(new SysConfigTreeDto
                        {
                            Id = int.MaxValue - idData,
                            ConfigCode = strCode,
                            ConfigValue = strCode,
                            ConfigDescript = strDescipt,
                            Category = SysConfigCategoryEnum.Default,
                            Children = new List<SysConfigTreeDto>(),
                        });
                        idData++;
                    }
                    defaultChildren.Add(new SysConfigTreeDto
                    {
                        Id = int.MaxValue - idData,
                        ConfigCode = configCode,
                        ConfigValue = configCode,
                        ConfigDescript = descript,
                        Category = item.Key,
                        Children = cateData,
                    });
                }
                else
                {
                    returnData[0].Children.Add(new SysConfigTreeDto
                    {
                        Id = int.MaxValue - idData,
                        ConfigCode = configCode,
                        ConfigValue = configCode,
                        ConfigDescript = descript,
                        Category = item.Key,
                        Children = cateData,
                    });
                }

                idData++;
            }

            var defaultData = returnData[0].Children.SingleOrDefault(p => p.ConfigCode == Enum.GetName(typeof(SysConfigCategoryEnum), SysConfigCategoryEnum.Default));
            if (defaultData != null)
            {
                defaultData.Children.AddRange(defaultChildren);
            }

            return Success(returnData);
        }

        public async Task<ResponseDto<List<TreeDto>>> GetSysConfigCategoryEnums()
        {
            var result = new List<TreeDto>();

            var children1 = new List<TreeDto>();
            var children2 = new List<TreeDto>();
            Array array = Enum.GetValues(typeof(SysConfigCategoryEnum));
            foreach (SysConfigCategoryEnum item in array)
            {
                if (item == SysConfigCategoryEnum.None)
                {
                    result.Add(new TreeDto
                    {
                        Id = (int)item,
                        Label = GetDescriptionByEnum(item),
                        Children = new List<TreeDto>()
                    });
                }
                else if (item == SysConfigCategoryEnum.Chongda
                    || item == SysConfigCategoryEnum.Kinwong
                    || item == SysConfigCategoryEnum.Default)
                {
                    children1.Add(new TreeDto
                    {
                        Id = (int)item,
                        Label = GetDescriptionByEnum(item),
                        Children = new List<TreeDto>()
                    });
                }
                else
                {
                    children2.Add(new TreeDto
                    {
                        Id = (int)item,
                        Label = GetDescriptionByEnum(item),
                        Children = new List<TreeDto>()
                    });
                }
            }
            var defaultData = children1.SingleOrDefault(p => p.Label == GetDescriptionByEnum(SysConfigCategoryEnum.Default));
            if (defaultData != null)
            {
                defaultData.Children.AddRange(children2);
            }
            result[0].Children.AddRange(children1);

            return Success(result);
        }

        public string GetDescriptionByEnum(SysConfigCategoryEnum enumValue)
        {
            string value = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs.Length == 0)
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        /// <summary>
        /// 获取枚举配置项
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<SysConfigInfoByEnumType>> GetSysConfigEnumList(GetSysConfigByEnumType req)
        {
            if (req == null)
            {
                return Fail<SysConfigInfoByEnumType>("信息格式错误!");
            }

            if (req.ConfigCode == null)
            {
                return Fail<SysConfigInfoByEnumType>("未传入有效的ConfigCode!");
            }

            if (!await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.ToLower().Equals(req.ConfigCode.ToLower())
            && !string.IsNullOrEmpty(p.ConfigType) && p.ConfigType.ToLower().Equals("enum")))
            {
                return Fail<SysConfigInfoByEnumType>("未查询到相关数据!");
            }

            var data = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.ToLower().Equals(req.ConfigCode.ToLower())
            && !string.IsNullOrEmpty(p.ConfigType) && p.ConfigType.ToLower().Equals("enum"));
            if (data == null)
            {
                return Fail<SysConfigInfoByEnumType>("未查询到相关数据!");
            }

            SysConfigInfoByEnumType result = new SysConfigInfoByEnumType();
            result.ConfigCode = data.ConfigCode;
            result.ConfigType = data.ConfigType;
            result.ConfigValue = data.ConfigValue;
            result.ConfigDescript = data.ConfigDescript;
            result.Remark = data.Remark;
            result.Category = data.Category;
            result.CreateTime = data.CreateTime;
            result.CreatorId = data.CreatorId;
            result.Id = data.Id;
            result.Status = data.Status;

            result.SysConfigEnums = new List<SysConfigEnum>();

            if (data.ConfigEnumValue != null)
            {
                var enumValueList = data.ConfigEnumValue.Trim().Split(';');
                if (enumValueList.Length > 0)
                {
                    foreach (var enumValue in enumValueList)
                    {
                        var value = enumValue.Trim().Split(',');
                        if (value.Length != 2)
                        {
                            continue;
                        }
                        result.SysConfigEnums.Add(new SysConfigEnum
                        {
                            ConfigEnumValue = value[0],
                            ConfigEnumDescript = value[1],
                        });
                    }
                }
            }

            return Success(result);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateSysConfigReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            if (string.IsNullOrEmpty(req.ConfigCode) || string.IsNullOrEmpty(req.ConfigValue) || string.IsNullOrEmpty(req.ConfigType))
            {
                return Fail("ConfigCode、ConfigValue、ConfigType不能为空!");
            }
            if (!req.Category.HasValue)
            {
                req.Category = SysConfigCategoryEnum.Default;
            }

            var exsitCode = await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ConfigCode) &&
            p.ConfigCode.ToLower().Equals(req.ConfigCode.ToLower()));
            if (exsitCode)
            {
                return Fail("ConfigCode已存在！");
            }

            if (!configType.Contains(req.ConfigType.ToLower()))
            {
                return Fail("配置项类型只能为：int 、 string 、 enum 、 bool");
            }

            if (req.ConfigType.ToLower().Equals("enum") && string.IsNullOrEmpty(req.ConfigEnumValue))
            {
                return Fail("配置项类型为枚举类型，需填写ConfigEnumValue！");
            }

            SysConfig model = _mapper.Map<SysConfig>(req);
            model.CreatorId = UserId;
            model.CreateTime = DateTime.Now;
            model.Status = (int)DataStatusEnum.Enable;

            var result = await _domainService.Add(model);
            if (!result)
            {
                return Fail("添加失败！");
            }

            await InitializeSysConfigs();

            return Success();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateSysConfigReq req)
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

            if (string.IsNullOrEmpty(req.ConfigCode) || string.IsNullOrEmpty(req.ConfigValue) || string.IsNullOrEmpty(req.ConfigType))
            {
                return Fail("ConfigCode、ConfigValue、ConfigType不能为空!");
            }
            if (!req.Category.HasValue)
            {
                req.Category = entity.Category.HasValue ? entity.Category : SysConfigCategoryEnum.Default;
            }

            if (await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ConfigCode) &&
            p.ConfigCode.ToLower().Equals(req.ConfigCode.ToLower()) && p.Id != req.Id))
            {
                return Fail($"{req.ConfigCode}已存在！");
            }

            var model = _mapper.Map<SysConfig>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            model.ConfigType = entity.ConfigType;
            model.ConfigCode = entity.ConfigCode;
            model.ConfigEnumValue = entity.ConfigEnumValue;

            await _domainService.Update(model);

            await InitializeSysConfigs();

            return Success();
        }

        /// <summary>
        /// 根据编码获取配置项数值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ResponseDto<object>> GetDataByCode(string code, SysConfigCategoryEnum category = SysConfigCategoryEnum.None, bool isReadCache = true)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Fail<object>("未识别有效的入参！");
            }
            var data = new SysConfig();
            if (!isReadCache)//读取数据库，顺带更新内存字典
            {
                data = await GetSysConfigByCode(code);
                if (data == null)
                {
                    return Fail<object>("未查询到数据！");
                }
                AddOrUpdateConfig(data);
            }
            else//读取内存字典
            {
                if (_sysConfigs.ContainsKey(code))//如果存在
                {
                    data = await GetCacheSysConfigByCode(code);
                }
                else//如果不存在，新增的配置在内存字典中有可能查询不到，去读数据库确认下
                {
                    data = await GetSysConfigByCode(code);
                    if (data == null)//数据库里面也没有，就返回
                    {
                        return Fail<object>("未查询到数据！");
                    }
                    AddOrUpdateConfig(data);
                }
            }
            if (data == null)
            {
                return Fail<object>("未查询到数据！");
            }
            if (string.IsNullOrEmpty(data.ConfigType) || string.IsNullOrEmpty(data.ConfigValue))
            {
                return Fail<object>("未识别有效的ConfigType和ConfigValue！");
            }
            var result = new ResponseDto<object>() { Code = ResponseCode.Success };
            switch (data.ConfigType.ToLower())
            {
                case "string":
                    result.Data = data.ConfigValue.ToString();
                    break;

                case "int":
                    int num = 0;
                    int.TryParse(data.ConfigValue, out num);
                    result.Data = num;
                    break;

                case "bool":
                    result.Data = data.ConfigValue.ToLower().Equals("true") || data.ConfigValue == "1";
                    break;

                case "enum":
                    result.Data = new SysConfigEnum()
                    {
                        ConfigEnumValue = data.ConfigValue,
                        ConfigEnumDescript = data.ConfigDescript
                    };
                    break;
            }
            return result;
        }

        /// <summary>
        /// 根据configCode获取配置信息
        /// </summary>
        /// <param name="configCode"></param>
        /// <returns></returns>
        private async Task<SysConfig> GetSysConfigByCode(string configCode)
        {
            return await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.ConfigCode) &&
                                        !string.IsNullOrEmpty(configCode) && p.ConfigCode.ToLower().Equals(configCode.ToLower()));
        }

        private async Task<SysConfig> GetCacheSysConfigByCode(string configCode)
        {
            return _sysConfigs.FirstOrDefault(p => !string.IsNullOrEmpty(p.Key) &&
              !string.IsNullOrEmpty(configCode) && p.Key.ToLower().Equals(configCode.ToLower())).Value;
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

            if (entity.IsSystem == true)
            {
                return Fail("系统自带的配置项不允许删除!");
            }

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                await InitializeSysConfigs();

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

                    if (entity.IsSystem == true)
                    {
                        return Fail(entity.ConfigCode + " 系统自带的配置项不允许删除!");
                    }
                    deleteList[i] = idList[i];
                }

                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {
                    await InitializeSysConfigs();

                    return Success("");
                }
            }
            return Fail("删除失败");

        }

        /// <summary>
        /// 获取告警信息等及时信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<SysTimelyInformationDto>> GetSysTimelyInformation()
        {
            SysTimelyInformationDto result = new SysTimelyInformationDto();

            _logger.LogInformation($"[GetSysTimelyInformation] ===== 开始处理用户 {UserName} 的告警请求 =====");
            _logger.LogInformation($"[GetSysTimelyInformation] 当前用户: {UserName}");
            _logger.LogInformation($"[GetSysTimelyInformation] NoAlarmForUsers: [{string.Join(",", _noAlarmForUsers)}]");
            _logger.LogInformation($"[GetSysTimelyInformation] SampleOrderForOnlyNotifyUsers: [{string.Join(",", _sampleOrderForOnlyNotifyUsers)}]");

            if (_noAlarmForUsers.Contains(UserName))
            {
                _logger.LogWarning($"[GetSysTimelyInformation] ❌ 用户 {UserName} 在 NoAlarmForUsers 列表中，不显示告警");
                result.AlarmTimelyInfo = new AlarmTimelyInfo
                {
                    Title = "告警及时信息",
                    Description = "当前用户不显示告警信息",
                    AlarmInfoList = new List<AlarmDto>()
                };
                _logger.LogInformation($"[GetSysTimelyInformation] ===== 用户 {UserName} 被 NoAlarmForOnlyNotifyUsers 过滤，返回空告警 =====");
                return Success(result);
            }

            bool isSampleOrderForOnlyNotifyUsersEnabled = _sampleOrderForOnlyNotifyUsers.Any();
            _logger.LogInformation($"[GetSysTimelyInformation] SampleOrderForOnlyNotifyUsers 是否启用: {isSampleOrderForOnlyNotifyUsersEnabled}");

            result.AlarmTimelyInfo = new AlarmTimelyInfo();
            result.AlarmTimelyInfo.Title = "告警及时信息";
            result.AlarmTimelyInfo.Description = "显示告警及时信息，需尽快处理";
            result.AlarmTimelyInfo.AlarmInfoList = new List<AlarmDto>();

            bool isShowInfo = false;
            var responseUseTimelyAlarm = await GetDataByCode("ISSHOWTIMELYINFORMATION");
            if (responseUseTimelyAlarm.Code != 0 || responseUseTimelyAlarm.Data == null)
            {
                _logger.LogWarning($"[GetSysTimelyInformation] ❌ 获取 ISSHOWTIMELYINFORMATION 配置失败");
                result.AlarmTimelyInfo.Description = "系统配置获取失败，无法显示告警信息";
                return Success(result);
            }
            isShowInfo = responseUseTimelyAlarm.Data.ToBool();
            _logger.LogInformation($"[GetSysTimelyInformation] ISSHOWTIMELYINFORMATION: {isShowInfo}");

            // ISSHOWTIMELYINFORMATION 是全局开关，优先级最高
            if (!isShowInfo)
            {
                _logger.LogWarning($"[GetSysTimelyInformation] ❌ 系统配置不显示告警信息，所有用户都无法查看告警");
                result.AlarmTimelyInfo.Description = "系统配置不显示告警信息";
                return Success(result);
            }

            var xMinutes = 5;
            var responseXMinutes = await GetDataByCode("ShowLatestXMinutesWarnings");
            if (responseXMinutes.Code != 0 || responseXMinutes.Data == null)
            {
                xMinutes = 5;
            }
            else
            {
                xMinutes = responseXMinutes.Data.ToInt();
            }

            // 分别查询两种类型的告警：
            // 1. notreadyfork开头的告警 - 按原来的时间限制配置
            var notReadyForkAlarms = await _alarmDomainService.QueryAsync(
                p => p.IsDeleted == 0 &&
                     p.CreateTime <= DateTime.Now.AddMinutes(-1 * xMinutes) &&
                     p.Status == 1 &&
                     p.IsHandled != true &&
                     !string.IsNullOrEmpty(p.AlarmCode) &&
                     p.AlarmCode.StartsWith("notreadyfork", StringComparison.OrdinalIgnoreCase),
                p => p.CreateTime,
                SqlSugar.OrderByType.Desc);

            // 2. 其他类型的告警 - 立即触发（无时间限制）
            var immediateAlarms = await _alarmDomainService.QueryAsync(
                p => p.IsDeleted == 0 &&
                     p.Status == 1 &&
                     p.IsHandled != true &&
                     (string.IsNullOrEmpty(p.AlarmCode) ||
                      !p.AlarmCode.StartsWith("notreadyfork", StringComparison.OrdinalIgnoreCase)),
                p => p.CreateTime,
                SqlSugar.OrderByType.Desc);

            var alarmData = new List<Alarm>();
            if (notReadyForkAlarms != null) alarmData.AddRange(notReadyForkAlarms);
            if (immediateAlarms != null) alarmData.AddRange(immediateAlarms);

            alarmData = alarmData.OrderByDescending(a => a.CreateTime).ToList();

            _logger.LogInformation($"[GetSysTimelyInformation] 查询告警数量 - notreadyfork告警: {notReadyForkAlarms?.Count ?? 0}, 立即触发告警: {immediateAlarms?.Count ?? 0}, 总计: {alarmData.Count}");

            if (alarmData != null && alarmData.Any())
            {

                // 检查SampleOrder告警功能是否启用
                bool isSampleOrderAlarmEnabled = _innerOptions.Value.EnableSampleOrderAlarm;
                _logger.LogInformation($"[GetSysTimelyInformation] EnableSampleOrderAlarm: {isSampleOrderAlarmEnabled}, TimeLimitHours: {_innerOptions.Value.SampleOrderAlarmTimeLimitHours}, BufferMinutes: {_innerOptions.Value.SampleOrderAlarmBufferMinutes}, WorkOrderPrefix: {_innerOptions.Value.SampleOrderAlarmWorkOrderPrefix}");

                // 如果SampleOrder告警功能被禁用，过滤掉所有SampleOrder类型的告警
                if (!isSampleOrderAlarmEnabled)
                {
                    var originalCount = alarmData.Count;
                    var filteredAlarms = alarmData.Where(a => !a.AlarmCode.Contains("SampleOrder", StringComparison.OrdinalIgnoreCase)).ToList();
                    var removedCount = originalCount - filteredAlarms.Count;
                    alarmData = filteredAlarms;
                    _logger.LogInformation($"[GetSysTimelyInformation] SampleOrder告警功能已禁用，过滤前告警数量: {originalCount}, 过滤后非SampleOrder告警数量: {filteredAlarms.Count}, 移除SampleOrder告警数量: {removedCount}");
                }
                else
                {
                    // SampleOrder告警功能启用时的过滤逻辑：
                    // 1. NoAlarmForUsers 用户：已经在前面过滤，不显示任何告警
                    // 2. SampleOrderForOnlyNotifyUsers 用户：可以查看包括 SampleOrder 在内的所有告警
                    // 3. 其他用户：可以查看除了 SampleOrder 之外的其他告警

                    if (isSampleOrderForOnlyNotifyUsersEnabled)
                    {
                        if (_sampleOrderForOnlyNotifyUsers.Contains(UserName))
                        {
                            // SampleOrderForOnlyNotifyUsers 的用户可以看到所有类型的告警
                            _logger.LogInformation($"[GetSysTimelyInformation] 用户 {UserName} 在 SampleOrderForOnlyNotifyUsers 列表中，将显示所有类型的告警");
                        }
                        else
                        {
                            // 其他用户看不到 SampleOrder 类型的告警
                            var originalCount2 = alarmData.Count;
                            var filteredAlarms2 = alarmData.Where(a => !a.AlarmCode.Contains("SampleOrder", StringComparison.OrdinalIgnoreCase)).ToList();
                            var removedCount2 = originalCount2 - filteredAlarms2.Count;
                            alarmData = filteredAlarms2;
                            _logger.LogInformation($"[GetSysTimelyInformation] 用户 {UserName} 不在 SampleOrderForOnlyNotifyUsers 列表中，过滤前告警数量: {originalCount2}, 过滤后非SampleOrder告警数量: {filteredAlarms2.Count}, 移除SampleOrder告警数量: {removedCount2}");
                        }
                    }
                    else
                    {
                        // SampleOrderForOnlyNotifyUsers 未启用，所有用户都能看到所有类型的告警
                        _logger.LogInformation($"[GetSysTimelyInformation] SampleOrderForOnlyNotifyUsers 未启用，所有用户都能看到所有类型的告警");
                    }
                }

                if (alarmData.Any())
                {
                    result.AlarmTimelyInfo.AlarmInfoList = _mapper.Map<List<Alarm>, List<AlarmDto>>(alarmData.ToList());
                    result.AlarmTimelyInfo.Description = $"显示 {alarmData.Count} 条告警信息，需尽快处理";
                    _logger.LogInformation($"[GetSysTimelyInformation] ===== 用户 {UserName} 将看到 {result.AlarmTimelyInfo.AlarmInfoList.Count} 条告警信息 =====");
                }
                else
                {
                    result.AlarmTimelyInfo.Description = "当前没有需要处理的告警信息";
                    _logger.LogInformation($"[GetSysTimelyInformation] ===== 用户 {UserName} 没有告警信息 =====");
                }
            }
            else
            {
                result.AlarmTimelyInfo.Description = "当前没有需要处理的告警信息";
                _logger.LogInformation($"[GetSysTimelyInformation] ===== 用户 {UserName} 没有告警信息 =====");
            }

            return Success(result);
        }


        public async Task<ResponseDto<string>> SaveBasicSysData(List<AddOrUpdateSysConfigReq> reqs)
        {
            if (reqs == null || reqs.Count == 0)
            {
                return Fail("未识别有效的入参！");
            }

            List<SysConfig> updateList = new List<SysConfig>();
            foreach (var req in reqs)
            {
                var entity = await _domainService.QueryByID(req.Id);
                if (entity == null)
                { continue; }

                var model = _mapper.Map<SysConfig>(entity);
                model.ConfigValue = req.ConfigValue;
                model.ModifierId = UserId;
                model.ModifyTime = DateTime.Now;
                updateList.Add(model);
            }

            var result = await _domainService.BulkUpdate(updateList);
            if (!result)
            {
                return Fail("保存失败！");
            }

            await InitializeSysConfigs();

            return Success();
        }

        public async Task InitializeSysConfigs(bool isForceAllRefresh = true)
        {
            if (isForceAllRefresh)
            {
                var newConfigs = await _domainService.QueryAsync(p => p.IsDeleted == 0, p => p.Id, SqlSugar.OrderByType.Asc);
                foreach (var config in newConfigs)
                {
                    AddOrUpdateConfig(config);
                }
            }
            else
            {
                var existCodes = _sysConfigs.Values.Select(x => x.ConfigCode).ToList();
                var newConfigs = await _domainService.QueryAsync(p => p.IsDeleted == 0 && existCodes.Contains(p.ConfigCode), p => p.Id, SqlSugar.OrderByType.Asc);
                foreach (var config in newConfigs)
                {
                    AddOrUpdateConfig(config);
                }
            }
        }

        private void AddOrUpdateConfig(SysConfig config)
        {
            if (_sysConfigs.ContainsKey(config.ConfigCode))
            {
                _sysConfigs[config.ConfigCode] = config;
            }
            else
            {
                _sysConfigs.TryAdd(config.ConfigCode, config);
            }
        }
    }
}
