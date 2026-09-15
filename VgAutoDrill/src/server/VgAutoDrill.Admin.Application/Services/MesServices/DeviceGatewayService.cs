using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceGateway;


namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class DeviceGatewayService : BaseServiceWithoutTree<DeviceGateway, DeviceGatewayDto, AddOrUpdateDeviceGatewayReq>, IDeviceGatewayService
    {
        private readonly IConfiguration _configuration;
        private readonly IAPIHelper _apiHelper;
        private readonly IAPIHelper _getInfoFromAPIHelper;
        public DeviceGatewayService(IBaseDomainService<DeviceGateway> domainService,
                                   IAPIHelper getInfoFromAPIHelper,
                                    IAPIHelper apiHelper,
                                    IConfiguration configuration,
        IMapper mapper) : base(domainService, mapper)
        {
            _getInfoFromAPIHelper = getInfoFromAPIHelper;
            _configuration = configuration;
            _apiHelper = apiHelper;


        }
        public List<DeviceGateway> lst = new List<DeviceGateway>();
        /// <summary>
        /// 添加设备网关
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseDto<string>> AddDeviceGateway(AddOrUpdateDeviceGatewayReq req)
        {
            if (req == null)
            {
                return Fail("数据格式错误");
            }
            if (req.Id > 0)
            {
                var rest = _domainService.QueryByID(req.Id);
                if (rest.Result != null)
                {
                    return Fail($"{req.Id}已经存在，不能添加");
                }
            }
            if (!string.IsNullOrEmpty(req.Code))
            {
                var list = await _domainService.QueryAsync(p => p.Code == req.Code && p.Id == req.Id, p => p.Id, SqlSugar.OrderByType.Asc);
                if (list != null && list.Count > 0)
                {
                    return Fail($"{req.Code}已经存在，不能添加");
                }
            }
            else
            {
                req.Code = req.Name;
            }

            var model = _mapper.Map<DeviceGateway>(req);
            model.Code = req.Code;
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Add(model);
            return Success();
        }

        private async Task<ResponseDto<string>> UpdateAppDate(string version, DeviceGateway result)
        {
            if (result != null)
            {
                var model = _mapper.Map<DeviceGateway>(result);
                model.ModifierId = UserId;
                model.ModifyTime = DateTime.Now;
                model.SetupTime = DateTime.Now;
                model.CVersion = version;//当前版本
                model.AVersion = version;//可用版本
                await _domainService.Update(model);
                return Success();
            }
            else
            {
                return Fail("接口未找到有效数据");
            }
        }

        public async Task<ResponseDto<PageDto<DeviceGatewayDto>>> GetVerson(DeviceGatewayReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DeviceGatewayDto>(req.PageNum, req.PageSize);
            if (req == null)
            {
                return Fail<PageDto<DeviceGatewayDto>>("数据类型错误");
            }
            if (req.Id < 1)
            {
                return Fail<PageDto<DeviceGatewayDto>>("数据格式错误");
            }

            var res = _domainService.QueryByID(req.Id);
            if (res == null)
            {
                return Fail<PageDto<DeviceGatewayDto>>("查询的数据在系统中未找到");
            }

            var str = _configuration["AppConfig:GetDeviceGatewayVersion"];
            string urlAddress = res.Result.VisitWebsite + _configuration["AppConfig:GetDeviceGatewayVersion"];
            if (string.IsNullOrEmpty(urlAddress))
            {
                return Fail<PageDto<DeviceGatewayDto>>("config, 未配置 AppVersion！");
            }

            if (string.IsNullOrEmpty(res.Result.AppName))
            {
                return Fail<PageDto<DeviceGatewayDto>>("该条数据的应用名未在系统中找到！");
            }

            if (!IsUrl(res.Result.VisitWebsite))
            {
                return Fail<PageDto<DeviceGatewayDto>>("该条数据的网关地址无效！");
            }

            urlAddress = urlAddress + "?AppName=" + res.Result.AppName;
            try
            {
                var result = _apiHelper.RequestData(urlAddress, "Get");
                if (result == null)
                {
                    return Fail<PageDto<DeviceGatewayDto>>("CheckPackages接口未查询到结果！");
                }

                JObject jsonStr = JObject.Parse(result);
                var code = jsonStr["Code"];
                if (code.ToString() != "200")
                {
                    return Fail<PageDto<DeviceGatewayDto>>("获取版本号失败");
                }

                var temp = jsonStr["Data"];
                var data = System.Text.Json.JsonSerializer.Deserialize<List<DeviceGateWayApp>>(temp.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var versonList = data.ToList();
                if (versonList == null || versonList.Count() <= 0)
                {
                    return Fail<PageDto<DeviceGatewayDto>>("CheckPackages接口未查询到结果！");
                }

                pageDto.Total = versonList.Count();
                List<DeviceGatewayDto> lst = new List<DeviceGatewayDto>();
                for (int i = 0; i < versonList.Count(); i++)
                {
                    DeviceGatewayDto dto = new DeviceGatewayDto();
                    dto.AppName = versonList.Select(t => t.AppName).ToList()[i];
                    dto.CVersion = versonList.Select(t => t.AppVersion).ToList()[i].ToString();
                    lst.Add(dto);
                }

                pageDto.List = _mapper.Map<List<DeviceGatewayDto>>(lst);
                return Success(pageDto);
            }
            catch (Exception ex)
            {
                return Fail<PageDto<DeviceGatewayDto>>($"获取版本号时失败{ex.Message}");
            }

        }

        private bool IsUrl(string? visitWebsite)
        {
            const string pattern = @"^(http|https):\/\/([a-zA-Z0-9]+[-\.]?)+(:[0-9]{1,5})?(\/.*)?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(visitWebsite);
        }

        public async Task<ResponseDto<string>> InstallPackages(DeviceGatewayReq req)
        {
            if (req == null)
            {
                return Fail($"数据类型错误");
            }
            if (req.Id < 1)
            {
                return Fail($"输入的数据类型错误");
            }

            if (string.IsNullOrEmpty(req.AVersion))
            {
                return Fail($"必须输入版本号才能安装");
            }

            var res = _domainService.QueryByID(req.Id);
            if (res == null)
            {
                return Fail("查询的数据在系统中未找到");
            }
            try
            {
                string urlAddress = res.Result.VisitWebsite + _configuration["AppConfig:SetUpDeviceGatewayApp"];
                if (string.IsNullOrEmpty(urlAddress))
                {
                    return Fail("未找到可用的网关地址 ！");
                }

                string str = string.Empty;
                Dictionary<string, object> dir = new Dictionary<string, object>();
                string dic = res.Result.parameters;
                str = urlAddress + $"?AppName={res.Result.AppName}&version={req.AVersion}";
                dir = JsonConvert.DeserializeObject<Dictionary<string, object>>(res.Result.parameters);
                var result = await _getInfoFromAPIHelper.PostFromJsonAsync<UpdateResponse>(str, dir);
                if (result == null)
                {
                    return Fail("InstallPackages接口未查询到结果！");
                }

                if (result.Code != "200")
                {
                    return Fail($"InstallPackages接口报错: {result.Message}！");
                }

                var selectRes = await _domainService.QueryAsync(p => p.Name.ToLower() == req.Name.ToLower() && p.IsDeleted == 0, p => p.Name, SqlSugar.OrderByType.Asc);
                var checkRes = await UpdateAppDate(req.AVersion, res.Result);
                if (checkRes.Code.ToString() != "Success")
                {
                    return Fail(checkRes.Message);
                }

                return Success();
            }
            catch (Exception ex)
            {
                return Fail($"下载安装应用时失败：{ex.Message}");
            }

        }

        private bool IsValidVersionFormat(string aVersion)
        {
            try
            {
                Regex regex = new Regex(@"^\d+(\.\d+){0,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                bool rest = regex.IsMatch(aVersion);
                return rest;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseDto<PageDto<DeviceGatewayDto>>> DeviceGatewayList(DeviceGatewayReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DeviceGatewayDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DeviceGateway>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => p.Code.ToUpper().Contains(req.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => p.Name.ToUpper().Contains(req.Name.ToUpper()));
            }

            var result = await _domainService.QueryPageAsync(where, p => p.Id, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DeviceGateway>, List<DeviceGatewayDto>>(result.ToList());

            return Success(pageDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseDto<string>> UpdateDeviceGateway(AddOrUpdateDeviceGatewayReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }
            if (req.Id > 0)
            {
                var rest = _domainService.QueryByID(req.Id);
                if (rest == null)
                {
                    return Fail("信息不存在!,不能更新");
                }
            }
            else
            {
                req.Id = lst[0].Id;
                req.Code = lst[0].Code;
                req.CreateTime = lst[0].CreateTime;
                req.CreatorId = lst[0].CreatorId;
            }

            var model = _mapper.Map<DeviceGateway>(req);
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            model.AVersion = req.CVersion;
            await _domainService.Update(model);

            return Success();
        }

        public async Task<ResponseDto<string>> DeviceGatewayAddOrUpdate(AddOrUpdateDeviceGatewayReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }
            var list = await _domainService.QueryAsync(p => p.Name == req.Name, p => p.Name, SqlSugar.OrderByType.Asc);
            if (list.Count > 0)
            {
                lst.Clear();
                lst = list;
                var rest = await UpdateDeviceGateway(req);//更新
                if (rest.Code.ToString() != "Success")
                {
                    return Fail(rest.Message);
                }
                else
                {
                    return Success();
                }

            }
            else
            {
                var rest = await AddDeviceGateway(req);//添加
                if (rest.Code.ToString() != "Success")
                {
                    return Fail(rest.Message);
                }
                else
                {
                    return Success();
                }

            }
        }
    }
}
