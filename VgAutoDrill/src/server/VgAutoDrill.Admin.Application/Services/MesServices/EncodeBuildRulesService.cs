using AutoMapper;
using VgAutoDrill.Admin.Application.Cache.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 编码生成规则
    /// </summary>
    public class EncodeBuildRulesService : BaseServiceWithoutTree<EncodeBuildRules, EncodeBuildRulesDto, AddOrUpdateEncodeBuildRulesReq>, IEncodeBuildRulesService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUnitMeasureDomainService _unitMeasureDomainService;
        private readonly IMdItemTypeDomainService _itemTypeService;
        private readonly IItemDomainService _itemDomainService;
        private readonly IWorkOrderDomainService _workOrderDomainService;
        private readonly IClientDomainService _clientDomainService;
        private readonly IVendorDomainService _vendorDomainService;
        private readonly IWorkstationDomainService _workstationDomainService;
        private readonly IProBoardTraceDomainService _proBoardTraceDomainService;
        private readonly ITaskDomainService _taskDomainService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="unitMeasureDomainService"></param>
        /// <param name="itemTypeService"></param>
        /// <param name="itemDomainService"></param>
        /// <param name="workOrderDomainService"></param>
        /// <param name="clientDomainService"></param>
        /// <param name="vendorDomainService"></param>
        /// <param name="workstationDomainService"></param>
        /// <param name="proBoardTraceDomainService"></param>
        /// <param name="taskDomainService"></param>
        /// <param name="mapper"></param>
        public EncodeBuildRulesService(IEncodeBuildRulesDomainService domainService,
            IUnitMeasureDomainService unitMeasureDomainService,
            IMdItemTypeDomainService itemTypeService,
            IItemDomainService itemDomainService,
            IWorkOrderDomainService workOrderDomainService,
            IClientDomainService clientDomainService,
            IVendorDomainService vendorDomainService,
            IWorkstationDomainService workstationDomainService,
            IProBoardTraceDomainService proBoardTraceDomainService,
            ITaskDomainService taskDomainService,
            IRedisCacheManager redisCache,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _cacheManager = redisCache;
            _unitMeasureDomainService = unitMeasureDomainService;
            _itemTypeService = itemTypeService;
            _itemDomainService = itemDomainService;
            _workOrderDomainService = workOrderDomainService;
            _clientDomainService = clientDomainService;
            _vendorDomainService = vendorDomainService;
            _workstationDomainService = workstationDomainService;
            _proBoardTraceDomainService = proBoardTraceDomainService;
            _taskDomainService = taskDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<EncodeBuildRulesDto>>> GetList(GetEncodeBuildRulesListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<EncodeBuildRulesDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<EncodeBuildRules>();
            where = where.And(p => p.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.RulesName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.RulesName) && p.RulesName.Contains(req.RulesName));
            }
            if (!string.IsNullOrEmpty(req.RulesCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.RulesCode) && p.RulesCode.Contains(req.RulesCode));
            }
            if (!string.IsNullOrEmpty(req.Prefix))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Prefix) && p.Prefix.Contains(req.Prefix));
            }
            if (!string.IsNullOrEmpty(req.Suffix))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Suffix) && p.Suffix.Contains(req.Suffix));
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<EncodeBuildRules>, List<EncodeBuildRulesDto>>(result.ToList());

            if (pageDto.List != null && pageDto.List.Count > 0)
            {
                foreach (var item in pageDto.List)
                {
                    SetModelValue(item);
                }
            }

            return Success(pageDto);
        }

        /// <summary>
        /// 给ExampleCode 和CurrentCode 赋值
        /// </summary>
        /// <param name="item"></param>
        private void SetModelValue(EncodeBuildRulesDto item)
        {
            string number = "1";
            if (item.IsPadded == 1 && item.NumberLength != null)
            {
                number = number.PadLeft((int)item.NumberLength, '0');
            }
            item.ExampleCode = item.Prefix + GetDateNumber(item) + number + item.Suffix;

            string routingKey = item.RulesCode + DateTime.Now.Date.ToString("yyyyMMdd");
            var value = _cacheManager.Get(routingKey);
            if (!string.IsNullOrEmpty(value.ToStr()))
            {
                var currentStr = value.ToStr().Replace("\"", "");
                if (item.IsPadded == 1 && item.NumberLength != null)
                {
                    currentStr = currentStr.PadLeft((int)item.NumberLength, '0');
                }
                item.CurrentCode = item.Prefix + GetDateNumber(item) + currentStr.ToStr() + item.Suffix;
            }
            else
            {
                item.CurrentCode = item.ExampleCode;
            }
        }

        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<EncodeBuildRulesDto>> NewQueryByID(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail<EncodeBuildRulesDto>("信息不存在!");
            }
            if (entity.IsDeleted == 1)
            {
                return Fail<EncodeBuildRulesDto>("信息错误!");
            }
            var model = _mapper.Map<EncodeBuildRulesDto>(entity);

            SetModelValue(model);

            return Success(model);
        }

        /// <summary>
        /// 根据编码规则生成编码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<string>>> GetEncodeList(GetEncodeByRulesListReq req)
        {
            List<string> result = new List<string>();

            if (req == null || string.IsNullOrEmpty(req.RulesCode))
            {
                return Success(result);
            }
            if (req.BuildCount < 1 || req.BuildCount == null)
            {
                req.BuildCount = 1;
            }
            var rule = await _domainService.FindSingleAsync(p => p.RulesCode == req.RulesCode);
            if (rule == null)
            {
                return Success(result);
            }

            result = await GetEncode(rule, (int)req.BuildCount);
            while (result.Count < req.BuildCount)
            {
                result.AddRange(await GetEncode(rule, ((int)req.BuildCount) - result.Count));
            }

            return Success(result);
        }

        /// <summary>
        /// 生成编码
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="bulidCount"></param>
        /// <returns></returns>
        private async Task<List<string>> GetEncode(EncodeBuildRules rule, int bulidCount)
        {
            List<string> enCodeList = BuildEncodeList(rule, bulidCount);

            List<string> existEncode = new List<string>();
            foreach (var encode in enCodeList)
            {
                //判断数据库是否存在该编码
                bool isExist = false;
                switch (rule.RulesCode)
                {
                    case "UNITMEASURE_CODE": //计量单位
                        isExist = await _unitMeasureDomainService.IsExistAsync(p => p.Code == encode);
                        break;
                    case "ITEMTYPE_CODE":    //物料分类
                        isExist = await _itemTypeService.IsExistAsync(p => p.Code == encode);
                        break;
                    case "ITEM_CODE":        //物料
                        isExist = await _itemDomainService.IsExistAsync(p => p.Code == encode);
                        break;
                    case "WORKSTATION_CODE": //工作站
                        isExist = await _workstationDomainService.IsExistAsync(p => p.Code == encode);
                        break;
                    case "CLIENT_CODE":      //客户管理
                        isExist = await _clientDomainService.IsExistAsync(p => p.Code == encode);
                        break;
                    case "VENDOR_CODE":      //供应商
                        isExist = await _vendorDomainService.IsExistAsync(p => p.Code == encode);
                        break;
                    case "WORKORDER_CODE":   //生产工单
                        isExist = await _workOrderDomainService.IsExistAsync(p => p.Code == encode);
                        break;
                    case "PANEL_CODE":       //板料追溯
                        isExist = await _proBoardTraceDomainService.IsExistAsync(p => p.PanelCode == encode);
                        break;
                    case "TASK_CODE":        //任务
                        isExist = await _taskDomainService.IsExistAsync(p => p.Code == encode);
                        break;
                    default:
                        isExist = false;
                        break;
                }
                if (isExist)
                {
                    existEncode.Add(encode);
                }
            }

            if (existEncode.Count > 0)
            {
                enCodeList.RemoveAll(p => existEncode.Contains(p));
            }

            return enCodeList;
        }

        /// <summary>
        /// 生成编码集合
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="bulidCount"></param>
        /// <returns></returns>
        private List<string> BuildEncodeList(EncodeBuildRules rule, int bulidCount)
        {
            List<string> result = new List<string>();

            //获取流水号（只存最新的）
            string routingKey = rule.RulesCode + GetDateNumber(rule);
            var value = _cacheManager.Get(routingKey);
            var cacheTime = GetCacheTime(rule);
            for (int i = 1; i < bulidCount + 1; i++)
            {
                string number = string.Empty;

                if (!string.IsNullOrEmpty(value.ToStr()))
                {
                    string newValue = (value.ToStr().Replace("\"", "").ToInt() + i).ToString();
                    switch (rule.IsPadded)
                    {
                        case 0:
                            number = newValue;
                            break;
                        case 1:
                            if (rule.NumberLength == null || rule.NumberLength < newValue.Length)
                            {
                                number = newValue;
                            }
                            else
                            {
                                number = newValue.PadLeft((int)rule.NumberLength, '0');
                            }
                            break;
                        default:
                            number = newValue;
                            break;
                    }

                    if (i == bulidCount)
                    {
                        _cacheManager.Set(routingKey, newValue, cacheTime);  //缓存24小时
                    }
                }
                else
                {
                    if (rule.IsPadded == 1)
                    {
                        number = rule.NumberLength == null ? i.ToStr() : i.ToStr().PadLeft((int)rule.NumberLength, '0');
                    }
                    else
                    {
                        number = i.ToStr();
                    }

                    if (i == bulidCount)
                    {
                        _cacheManager.Set(routingKey, i.ToStr(), cacheTime);  //缓存24小时
                    }
                }

                string encode = rule.Prefix + GetDateNumber(rule) + number + rule.Suffix;
                result.Add(encode);
            }

            return result;
        }
        /// <summary>
        /// 根据编码规则获取缓存时间
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>

        private int GetCacheTime(EncodeBuildRules rule)
        {
            var cacheTime = 0;
            if (rule.HasYear == true)
            {
                cacheTime = 60 * 24 * 30 * 365;
            }
            if (rule.HasMonth == true)
            {
                cacheTime = 24 * 60 * 30;
            }
            if (rule.HasDay == true)
            {
                cacheTime = 24 * 60;
            }
            return cacheTime;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateEncodeBuildRulesReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.RulesCode == req.RulesCode);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<EncodeBuildRules>(req);
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
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateEncodeBuildRulesReq req)
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

            var isExsitCode = await _domainService.IsExistAsync(p => p.RulesCode == req.RulesCode && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<EncodeBuildRules>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        /// <summary>
        /// 计算日期构成的代码组成
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private string GetDateNumber(EncodeBuildRulesDto rule)
        {
            var result = string.Empty;

            if (rule.HasYear == true)
            {
                result += $"{DateTime.Now.Date.ToString("yyyy")}";
            }
            if (rule.HasMonth == true)
            {
                result += $"{DateTime.Now.Date.ToString("MM")}";
            }
            if (rule.HasDay == true)
            {
                result += $"{DateTime.Now.Date.ToString("dd")}";
            }

            return result;
        }
        /// <summary>
        /// 计算日期构成的代码组成
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private string GetDateNumber(EncodeBuildRules rule)
        {
            var result = string.Empty;

            if (rule.HasYear == true)
            {
                result += $"{DateTime.Now.Date.ToString("yyyy")}";
            }
            if (rule.HasMonth == true)
            {
                result += $"{DateTime.Now.Date.ToString("MM")}";
            }
            if (rule.HasDay == true)
            {
                result += $"{DateTime.Now.Date.ToString("dd")}";
            }

            return result;
        }
    }
}

