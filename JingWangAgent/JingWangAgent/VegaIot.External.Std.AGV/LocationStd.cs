using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Store;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static Quartz.Logging.OperationName;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VegaIot.External.Std.Shelf;

public class LocationStd : LocationData<StdShelf>
{
    public LocationStd(IServiceProvider serviceProvider, IDeviceStore deviceStore, StdShelf device)
        : base(serviceProvider, deviceStore, device)
    {

    }

    public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        await ReportingProcess($"{LocationCode} CompleteUnloadMaterial Start : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}", GetTraceId(deviceServiceInvokeRequest!));

        deviceServiceInvokeRequest!.Params["UnloadingPanel"] = new SwapPanel() { SpindleId = Position.ToInt(), PanelList = Panels };

        logger.LogInformation($"CompleteUnloadMaterial {Position}号料架 下料完成 UnloadingPanel 赋值：{JsonSerializer.Serialize(deviceServiceInvokeRequest!.Params["UnloadingPanel"])}");

        await BindLineSideStock(Panels.FirstOrDefault()?.SiloCode!, PositonCode!, "0", deviceServiceInvokeRequest);

        await SetNoPayload();

        logger.LogInformation($"CompleteUnloadMaterial {Position}号料架 下料完成后 PayloadPanels {JsonSerializer.Serialize(Panels)}");

        IsAgvWorking = false;
        CurrentContext.IsExistSilo = false;

        await ReportingProcess($"{LocationCode} CompleteUnloadMaterial Finish : IsAgvWorking :{IsAgvWorking}, (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}", GetTraceId(deviceServiceInvokeRequest!));
        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(Panels), deviceServiceInvokeRequest.Params);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        if (deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
        {
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(Panels));
        }

        await ReportingProcess($"{LocationCode} CompleteLoadMaterial Start : (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}", GetTraceId(deviceServiceInvokeRequest!));



        var operationEntity = new List<Panel> { };

        if (!deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
        {
            logger.LogError($"CompleteLoadMaterial 上生料获取的LoadingPanel的信息 为空");
            await SetEmptySiloInLocation();
        }
        else
        {
            logger.LogInformation($"CompleteLoadMaterial 上生料获取的LoadingPanel的信息：{deviceServiceInvokeRequest.Params["LoadingPanel"]}");
            var loadSiloInfo = JsonSerializer.Deserialize<SwapPanel>(deviceServiceInvokeRequest.Params["LoadingPanel"]?.ToString()!
           , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            operationEntity = loadSiloInfo?.PanelList;

            logger.LogInformation($"CompleteLoadMaterial {Position}号料架 上料完成未修改前 PayloadPanels {JsonSerializer.Serialize(Panels)}");

            if (operationEntity != null && operationEntity.Count > 0)
            {
                bool isAllClicker = DeviceExtensions.SiloIsHaveNoRawByPosition(operationEntity);
                logger.LogInformation($"CompleteLoadMaterial isAllClicker:{isAllClicker}");

                ItemPanelInfoWhenCompleteLoadMaterial.Clear();
                for (int i = 0; i < operationEntity.Count; i++)
                {
                    await ModifyPanels(isAllClicker, operationEntity[i], deviceServiceInvokeRequest, ItemPanelInfoWhenCompleteLoadMaterial);
                }

                await UpdateSiloInfoByPosition(operationEntity);
            }
            else
            {
                logger.LogDebug($"上料仓获取的板材信息为空");
                await ReportingProcess($"{LocationCode} (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)} CompleteLoadMaterial Fail_Message :上生料获取的板材信息为 null。。。", GetTraceId(deviceServiceInvokeRequest!));
                await SetEmptySiloInLocation();
            }

            logger.LogDebug($"CompleteLoadMaterial (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}, {Position} 号料架 上料完成后 PayloadPanels {JsonSerializer.Serialize(Panels)}");
        }

        await BindLineSideStock(operationEntity?.FirstOrDefault()?.SiloCode!, PositonCode!, "1", deviceServiceInvokeRequest);
        IsAgvWorking = false;
        CurrentContext.IsExistSilo = true;

        await ReportingProcess($"{LocationCode} CompleteLoadMaterial (CurrentEventTraceId={GetTraceId(deviceServiceInvokeRequest!)}, Finish (IsAgvWorking = {IsAgvWorking})。。。", GetTraceId(deviceServiceInvokeRequest!));
        return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels));
    }

    #region 绑定托盘库位
    protected async Task BindLineSideStock(string siloCode, string positionCode, string operationCode, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        if ( deviceServiceInvokeRequest.Params.ContainsKey("AgvKind") && deviceServiceInvokeRequest.Params["AgvKind"].ToStr() == "Std")
        {
            return;
        }

        try
        {
            var _Request200 = new BindSiloStockEntity()
            {
                clientCode = "VEGA001",
                indBind = operationCode,
                podCode = siloCode,
                reqCode = $"{LocationCode}_{DateTime.Now.Ticks.ToString()}",
                reqTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                mapDataCode = positionCode
            };

            logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor:task:{LocationCode}: 料架与料仓绑定 - BindLineSideStock: 请求参数: \r\n {JsonSerializer.Serialize(_Request200)}");


            var res200 = await HttpRequestInvoker.PostAsJsonAsync<BindSiloStockEntity?, STDResponse>(DeviceDescriptor.Extra["BindLineSideStockUrl"].ToStr(), _Request200, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

            logger.LogInformation($"StdAgvScheduler_DrilledFromForkToOutsideExecutor : {LocationCode} 料架与料仓绑定 - BindLineSideStock: 返回内容: \r\n {JsonSerializer.Serialize(res200, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            })}");
        }
        catch (Exception ex)
        {
            logger.LogError($"{LocationCode}: BindLineSideStock Error", ex);
        }
    }

    #endregion 绑定托盘库位
}
