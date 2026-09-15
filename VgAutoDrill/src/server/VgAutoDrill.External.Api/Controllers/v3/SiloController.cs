using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.External.Application.Interfaces.V3;
using VgAutoDrill.External.Model.V3;

namespace VgAutoDrill.External.WebApi.Controllers.v3
{
    [Route("api/v3/external/silo")]
    [ApiController]
    public class SiloController : ControllerBase
    {
        private readonly IExternalSiloServiceV3 _externalSiloService;
        private readonly ILogger<SiloController> logger;

        public SiloController(IExternalSiloServiceV3 externalSiloService, ILoggerFactory loggerFactory)
        {
            _externalSiloService = externalSiloService;
            logger = loggerFactory.CreateLogger<SiloController>();
        }

        /// <summary>
        /// 料仓/料架接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ExternalInterface(ExternalBaseReq req)
        {
            var result = new ResponseDto<string>();
            if (req == null)
            {
                result.Code = ResponseCode.Fail;
                result.Message = "未识别有效的入参！";
                return Ok(result);
            }

            if (string.IsNullOrEmpty(req.Action))
            {
                result.Code = ResponseCode.Fail;
                result.Message = "未识别有效的Action！";
                return Ok(result);
            }

            if (string.IsNullOrEmpty(req.Version))
            {
                result.Code = ResponseCode.Fail;
                result.Message = "未识别有效的Version！";
                return Ok(result);
            }

            if (req.Data == null || req.Data.Count == 0)
            {
                result.Code = ResponseCode.Fail;
                result.Message = "未识别有效的Data！";
                return Ok(result);
            }
            logger.LogInformation($"SiloController_ExternalInterface : " + req.Action + " BeginTime " + DateTime.Now);

            try
            {
                string action = req.Action.ToUpper();
                switch (action)
                {
                    case "ADD_SILO":
                        if (!req.Data.ContainsKey("AddSiloParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的新增数据！";
                            return Ok(result);
                        }
                        var addSilo = req.Data["AddSiloParam"];
                        if (addSilo == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的新增数据！";
                            return Ok(result);
                        }
                        var addSiloData = JsonConvert.DeserializeObject<List<ExternalAddOrUpdateSiloReq>>(addSilo.ToString());
                        if (addSiloData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的AddSiloParam！";
                            return Ok(result);
                        }

                        var addSiloResult = await _externalSiloService.AddSiloBatch(addSiloData);
                        return Ok(addSiloResult);

                    case "UPDATE_SILO":
                        if (!req.Data.ContainsKey("UpdateSiloParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的修改数据！";
                            return Ok(result);
                        }
                        var updateSilo = req.Data["UpdateSiloParam"];
                        if (updateSilo == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的修改数据！";
                            return Ok(result);
                        }
                        var updateSiloData = JsonConvert.DeserializeObject<ExternalAddOrUpdateSiloReq>(updateSilo.ToString());
                        if (updateSiloData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的UpdateSiloParam！";
                            return Ok(result);
                        }

                        var updateSiloResult = await _externalSiloService.UpdateSiloExternal(updateSiloData);
                        return Ok(updateSiloResult);

                    case "DELETE_SILO":
                        if (!req.Data.ContainsKey("DeleteSiloParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的删除数据！";
                            return Ok(result);
                        }
                        var deleteSilo = req.Data["DeleteSiloParam"];
                        if (deleteSilo == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的删除数据！";
                            return Ok(result);
                        }
                        var deleteSiloData = JsonConvert.DeserializeObject<string>(deleteSilo.ToString());
                        if (string.IsNullOrEmpty(deleteSiloData))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的DeleteSiloParam！";
                            return Ok(result);
                        }

                        var deleteSiloResult = await _externalSiloService.DeleteExternalSiloInfo(deleteSiloData);
                        return Ok(deleteSiloResult);

                    case "GET_SILO_INFO":
                        if (!req.Data.ContainsKey("GetSiloInfoParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的查询条件！";
                            return Ok(result);
                        }
                        var getSilo = req.Data["GetSiloInfoParam"];
                        if (getSilo == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的查询条件！";
                            return Ok(result);
                        }
                        var getSiloData = JsonConvert.DeserializeObject<ExternalSiloQueryReq>(getSilo.ToString());
                        if (getSiloData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的GetSiloInfoParam！";
                            return Ok(result);
                        }

                        var getSiloResult = await _externalSiloService.GetExternalSiloInfo(getSiloData);
                        return Ok(getSiloResult);

                    case "SILO_BIND_PANEL":
                        if (!req.Data.ContainsKey("SiloBindPanelParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的绑定数据！";
                            return Ok(result);
                        }
                        var bindPanel = req.Data["SiloBindPanelParam"];
                        if (bindPanel == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的绑定数据！";
                            return Ok(result);
                        }
                        var bindData = JsonConvert.DeserializeObject<ExternalAddOrUpdateSiloWithPanelReq>(bindPanel.ToString());
                        if (bindData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SiloBindPanelParam！";
                            return Ok(result);
                        }

                        var bindResult = await _externalSiloService.AddOrUpdateExternalSiloWithPanel(bindData);
                        return Ok(bindResult);

                    case "SILO_UNBIND_PANEL":
                        if (!req.Data.ContainsKey("SiloUnBindPanelParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的解绑数据！";
                            return Ok(result);
                        }
                        var unBind = req.Data["SiloUnBindPanelParam"];
                        if (unBind == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的解绑数据！";
                            return Ok(result);
                        }
                        var unBindData = JsonConvert.DeserializeObject<ExternalSiloUnBindReq>(unBind.ToString());
                        if (unBindData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SiloUnBindPanelParam！";
                            return Ok(result);
                        }

                        var unBindResult = await _externalSiloService.UnBindPanel(unBindData);
                        return Ok(unBindResult);

                    case "UNBIND_ALL_PANEL":
                        if (!req.Data.ContainsKey("UnBindAllPanelParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的解绑数据！";
                            return Ok(result);
                        }
                        var unBindAll = req.Data["UnBindAllPanelParam"];
                        if (unBindAll == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的解绑数据！";
                            return Ok(result);
                        }
                        var unBindAllData = JsonConvert.DeserializeObject<ExternalSiloAllUnBindReq>(unBindAll.ToString());
                        if (unBindAllData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的UnBindAllPanelParam！";
                            return Ok(result);
                        }

                        var unBindAllResult = await _externalSiloService.UnBindAllPanel(unBindAllData);
                        return Ok(unBindAllResult);

                    case "SET_MANUAL":
                        if (!req.Data.ContainsKey("SetManualParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的设置手动数据！";
                            return Ok(result);
                        }
                        var setM = req.Data["SetManualParam"];
                        if (setM == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的设置手动数据！";
                            return Ok(result);
                        }
                        var setMData = JsonConvert.DeserializeObject<ExternalSetSiloStatusReq>(setM.ToString());
                        if (setMData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SetManualParam！";
                            return Ok(result);
                        }

                        var setMResult = await _externalSiloService.SetManual(setMData);
                        return Ok(setMResult);

                    case "SET_READY":
                        if (!req.Data.ContainsKey("SetReadyParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的设置就绪数据！";
                            return Ok(result);
                        }
                        var setR = req.Data["SetReadyParam"];
                        if (setR == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的设置就绪数据！";
                            return Ok(result);
                        }
                        var setRData = JsonConvert.DeserializeObject<ExternalSetSiloStatusReq>(setR.ToString());
                        if (setRData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SetReadyParam！";
                            return Ok(result);
                        }

                        var setRResult = await _externalSiloService.SetReady(setRData);
                        return Ok(setRResult);

                    case "GET_RACK_INFO":
                        if (!req.Data.ContainsKey("GetRackInfoParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的查询条件！";
                            return Ok(result);
                        }
                        var getRack = req.Data["GetRackInfoParam"];
                        if (getRack == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的查询条件！";
                            return Ok(result);
                        }
                        var getRackParam = JsonConvert.DeserializeObject<ExternalRackQueryReq>(getRack.ToString());
                        if (getRackParam == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的GetRackInfoParam！";
                            return Ok(result);
                        }

                        var getRackResult = await _externalSiloService.GetExternalRackInfo(getRackParam);
                        return Ok(getRackResult);

                    case "ADD_RACK":
                        if (!req.Data.ContainsKey("AddRackParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的新增数据！";
                            return Ok(result);
                        }
                        var addRack = req.Data["AddRackParam"];
                        if (addRack == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的新增数据！";
                            return Ok(result);
                        }
                        var addRackParam = JsonConvert.DeserializeObject<List<AddOrUpdateRackReq>>(addRack.ToString());
                        if (addRackParam == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的AddRackParam！";
                            return Ok(result);
                        }

                        var addRackResult = await _externalSiloService.AddRackBatch(addRackParam);
                        return Ok(addRackResult);

                    case "UPDATE_RACK":
                        if (!req.Data.ContainsKey("UpdateRackParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的修改数据！";
                            return Ok(result);
                        }
                        var updateRack = req.Data["UpdateRackParam"];
                        if (updateRack == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的修改数据！";
                            return Ok(result);
                        }
                        var updateRackParam = JsonConvert.DeserializeObject<ExternalAddOrUpdateRackReq>(updateRack.ToString());
                        if (updateRackParam == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的UpdateRackParam！";
                            return Ok(result);
                        }

                        var updateRackResult = await _externalSiloService.UpdateRackExternal(updateRackParam);
                        return Ok(updateRackResult);

                    case "DELETE_RACK_INFO":
                        if (!req.Data.ContainsKey("DeleteRackInfo"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的删除数据！";
                            return Ok(result);
                        }
                        var deleteRack = req.Data["DeleteRackInfo"];
                        if (deleteRack == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的删除数据！";
                            return Ok(result);
                        }
                        var deleteRackCode = JsonConvert.DeserializeObject<string>(deleteRack.ToString());
                        if (string.IsNullOrEmpty(deleteRackCode))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的DeleteRackInfo！";
                            return Ok(result);
                        }

                        var deleteRackResult = await _externalSiloService.DeleteExternalRackInfo(deleteRackCode);
                        return Ok(deleteRackResult);

                    default:
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的Action！";
                        return Ok(result);
                }
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.Fail;
                result.Message = ex.Message;
            }
            return Ok(result);
        }
    }
}
