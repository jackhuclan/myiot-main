using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class ManualCallAgvTaskService : BaseServiceWithoutTree<ManualCallAgvTask, ManualCallAgvTaskDto, AddOrUpdateManualCallAgvTaskReq>, IManualCallAgvTaskService
    {
        private readonly IManualCallAgvTaskDomainService _manualCallAgvTaskDomainService;
        private readonly IAPIHelper _apiHelper;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        private readonly ILogger<ManualCallAgvTaskService> _logger;
        private readonly ICutterGroupDomainService _cutterGroupDomainService;

        public ManualCallAgvTaskService(IManualCallAgvTaskDomainService domainService, 
            IAPIHelper apiHelper,
            ISysConfigManager sysConfigManager,
            ICentralOnlineDevice centralOnlineDevice,
            ILogger<ManualCallAgvTaskService> logger,
            ICutterGroupDomainService cutterGroupDomainService,
            IMapper mapper) : base(domainService, mapper)
        {
            _manualCallAgvTaskDomainService = domainService;
            _apiHelper = apiHelper;
            _sysConfigManager = sysConfigManager;
            _centralOnlineDevice = centralOnlineDevice;
            _logger = logger;
            _cutterGroupDomainService = cutterGroupDomainService;
        }
        /// <summary>
        /// 根据钻机code查询手动呼叫agv任务信息
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<ManualCallAgvTaskDto>>> QueryByDeviceCode(string deviceCode)
        {
            var result = new List<ManualCallAgvTaskDto>();
            if (string.IsNullOrWhiteSpace(deviceCode))
            {
                return Fail("钻机编码不能为空", result);
            }
            var data = await _manualCallAgvTaskDomainService.QueryByDeviceCode(deviceCode);
            if (data?.Count > 0)
            {
                result = _mapper.Map<List<ManualCallAgvTaskDto>>(data);
            }
            return Success(result);
        }

        /// <summary>
        /// 更新手动呼叫agv任务信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> UpdateManualCallAgvTask(AddOrUpdateManualCallAgvTaskReq req)
        {
            if (req.Id <= 0)
            {
                return Fail<bool>("任务信息id不能为空");
            }
            var manualCallAgvTask = await _manualCallAgvTaskDomainService.QueryByID(req.Id);
            if (manualCallAgvTask == null)
            {
                return Fail<bool>("查询不到该呼叫agv任务信息");
            }
            manualCallAgvTask.ItemCode = req.ItemCode;
            manualCallAgvTask.PodCode = req.PodCode;
            manualCallAgvTask.IsBind = req.IsBind;
            manualCallAgvTask.TaskStatus = req.TaskStatus;
            manualCallAgvTask.TaskStatusDescription = req.TaskStatus.GetDescription();
            manualCallAgvTask.ModifierId = UserId;
            manualCallAgvTask.ModifyTime = DateTime.Now;
            await _manualCallAgvTaskDomainService.Update(manualCallAgvTask);
            return Success(true);
        }

        /// <summary>
        /// 根据库位code查询任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        public async Task<ManualCallAgvTaskDto> GetAgvTaskByLocationCode(string locationCode)
        {
            var result = new ManualCallAgvTaskDto();
            var data = await QueryByLocationCode(locationCode);
            if (data != null)
            {
                result = _mapper.Map<ManualCallAgvTaskDto>(data);
            }
            return result;
        }


        /// <summary>
        /// 根据库位code查询任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        public async Task<ManualCallAgvTask> QueryByLocationCode(string locationCode)
        {
            return await _manualCallAgvTaskDomainService.QueryByLocationCode(locationCode);
        }


        /// <summary>
        /// 更新任务信息
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task<bool> Update(ManualCallAgvTask task)
        {
            return await _manualCallAgvTaskDomainService.Update(task);
        }

        /// <summary>
        /// 获取刀盒码
        /// </summary>
        /// <param name="groupNo">配刀组计划编码</param>
        /// <param name="deviceCode">设备编码</param>
        /// <returns></returns>
        public async Task<ResponseDto<List<string>>> GetAptBoxBarcode(string groupNo, string deviceCode)
        {
            if (string.IsNullOrWhiteSpace(deviceCode))
            {
                return Fail("设备编码不能为空", new List<string>());
            }
            if (string.IsNullOrWhiteSpace(groupNo))
            {
                return Fail("配刀组计划编码不能为空", new List<string>());
            }

            try
            {
                var url = await _sysConfigManager.GetStringValue(MESConfigConstants.CUTTER_GROUP_APT_FILE_URL);
                if (string.IsNullOrWhiteSpace(url))
                {
                    return Fail("找不到CutterGroupAptFileUrl相关配置", new List<string>());
                }

                var request = new KwDrillKnifeToolInfoReq()
                {
                    EquipmentCode = deviceCode,
                    PlanNo = groupNo,
                };
                var requestString = JsonConvert.SerializeObject(request);
                var data = _apiHelper.RequestData<KwDrillKnifeToolInfoRes>(url, "Post", requestString);
                
                if (data == null || data?.Code != "0")
                {
                    return Fail($"{data?.Message},调用第三方接口获取刀盒码请求失败", new List<string>());
                }

                // 从返回结果中提取刀盒码列表
                var boxNosString = data.ReturnValue?.BoxNos ?? string.Empty;
                var boxNos = string.IsNullOrEmpty(boxNosString) 
                    ? new List<string>() 
                    : boxNosString.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
                
                // 如果刀盒码为空，返回错误信息
                if (boxNos == null || !boxNos.Any())
                {
                    return Fail("没有获取到二维码", new List<string>());
                }
                
                return Success(boxNos);
                
                // 临时使用测试数据
                // var boxNos = new List<string> { "123", "456", "789" };
                // return Success(boxNos);
            }
            catch (Exception ex)
            {
                return Fail($"获取刀盒码异常: {ex.Message}", new List<string>());
            }
        }

        /// <summary>
        /// 比对刀盒码
        /// </summary>
        /// <param name="aptBoxReq">刀盒码比对请求</param>
        /// <returns>比对结果：ok或ng</returns>
        public async Task<ResponseDto<string>> ConfirmAptBoxBarcode(AptBoxReq aptBoxReq)
        {
            if (string.IsNullOrWhiteSpace(aptBoxReq.DeviceCode))
            {
                return Fail("设备编码不能为空", string.Empty);
            }
            if (string.IsNullOrWhiteSpace(aptBoxReq.CutterGroupNo))
            {
                return Fail("配刀组计划编码不能为空", string.Empty);
            }
            if (aptBoxReq.InputBoxs == null || !aptBoxReq.InputBoxs.Any())
            {
                return Fail("输入的刀盒码不能为空", string.Empty);
            }

            try
            {
                //获取预期的刀盒码
                var expectedBoxResult = await GetAptBoxBarcode(aptBoxReq.CutterGroupNo, aptBoxReq.DeviceCode);
                if (expectedBoxResult.Code != ResponseCode.Success)
                {
                    return Fail($"获取预期刀盒码失败: {expectedBoxResult.Message}", string.Empty);
                }

                var expectedBoxs = expectedBoxResult.Data ?? new List<string>();

                // 临时写死刀盒码用于测试
                // var expectedBoxs = new List<string> { "123", "456", "789" };
                var inputBoxs = aptBoxReq.InputBoxs;

                // 比对刀盒码
                var compareResult = CompareBoxCodesWithMessage(expectedBoxs, inputBoxs);
                
                _logger.LogInformation($"刀盒码比对结果 - 设备: {aptBoxReq.DeviceCode}, 配刀组: {aptBoxReq.CutterGroupNo}, 预期: [{string.Join(",", expectedBoxs)}], 输入: [{string.Join(",", inputBoxs)}], 结果: {compareResult.Result}, 详情: {compareResult.Message}");

                // 如果校验成功，更新配刀组计划的check_box字段
                if (compareResult.Result == "ok")
                {
                    await UpdateCutterGroupCheckBox(aptBoxReq.CutterGroupNo, CutterBoxStatusEnum.OK); // 1表示校验成功
                }
                else
                {
                    await UpdateCutterGroupCheckBox(aptBoxReq.CutterGroupNo, CutterBoxStatusEnum.Fail); // 0表示校验失败
                }

                return Success(compareResult.Result, compareResult.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"比对刀盒码异常 - 设备: {aptBoxReq.DeviceCode}, 配刀组: {aptBoxReq.CutterGroupNo}");
                return Fail($"比对刀盒码异常: {ex.Message}", string.Empty);
            }
        }

        /// <summary>
        /// 比对刀盒码并返回详细匹配信息
        /// </summary>
        /// <param name="expectedBoxs">预期的刀盒码列表</param>
        /// <param name="inputBoxs">输入的刀盒码列表</param>
        /// <returns>包含匹配结果和详细信息的对象</returns>
        private (string Result, string Message) CompareBoxCodesWithMessage(List<string> expectedBoxs, List<string> inputBoxs)
        {
            if (expectedBoxs == null || inputBoxs == null)
            {
                return ("ng", "预期刀盒码列表或输入刀盒码列表为空");
            }

            // 如果预期列表为空，认为输入不匹配
            if (!expectedBoxs.Any())
            {
                return ("ng", "预期刀盒码列表为空");
            }

            // 标准化处理
            var expectedNormalized = expectedBoxs.Select(x => x?.Trim().ToUpperInvariant()).Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x).ToList();
            var inputNormalized = inputBoxs.Select(x => x?.Trim().ToUpperInvariant()).Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x).ToList();

            // 完全匹配
            if (expectedNormalized.SequenceEqual(inputNormalized))
            {
                return ("ok", "刀盒码匹配成功");
            }

            // 分析不匹配的原因
            var missingInInput = expectedNormalized.Except(inputNormalized).ToList();
            var extraInInput = inputNormalized.Except(expectedNormalized).ToList();

            var message = "刀盒码不匹配：";
            
            if (missingInInput.Any())
            {
                message += $"缺少预期刀盒码: [{string.Join(", ", missingInInput)}]";
            }
            
            if (extraInInput.Any())
            {
                if (missingInInput.Any()) message += "；";
                message += $"包含多余刀盒码: [{string.Join(", ", extraInInput)}]";
            }

            return ("ng", message);
        }

        /// <summary>
        /// 更新配刀组计划的check_box字段
        /// </summary>
        /// <param name="groupNo">配刀组计划编码</param>
        /// <param name="checkBox">校验状态(-1:没有校验,0:校验失败,1:校验成功)</param>
        /// <returns></returns>
        private async Task UpdateCutterGroupCheckBox(string groupNo, CutterBoxStatusEnum checkBox)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(groupNo))
                {
                    _logger.LogWarning("配刀组计划编码为空，无法更新check_box字段");
                    return;
                }

                var cutterGroup = await _cutterGroupDomainService.FindSingleAsync(c => c.GroupNo.ToLower() == groupNo.ToLower());
                if (cutterGroup == null)
                {
                    _logger.LogWarning($"找不到配刀组计划: {groupNo}");
                    return;
                }

                cutterGroup.CheckBox = checkBox;
                cutterGroup.ModifyTime = DateTime.Now;
                await _cutterGroupDomainService.Update(cutterGroup);
                
                _logger.LogInformation($"更新配刀组计划check_box字段成功 - 配刀组: {groupNo}, 状态: {checkBox}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新配刀组计划check_box字段失败 - 配刀组: {groupNo}, 状态: {checkBox}");
            }
        }
    }
}
