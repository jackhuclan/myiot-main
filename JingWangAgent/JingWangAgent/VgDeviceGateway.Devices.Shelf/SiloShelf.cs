using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Shelf;

public class SiloShelf : Device
{
    private readonly ILogger<SiloShelf> logger;
    public int spindleNum = 3;
    public readonly int layerLimit = 16;
    public volatile bool allowAllAgv = true;
    public ILoadMaterialInteractionPolicy AGVToShelfSiloShelfLoadPolicy;
    public IUnloadMaterialInteractionPolicy AGVToShelfSiloShelfUnloadPolicy;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    public bool IsCanLoopSync = false;
    public volatile bool _fromLocal = true;
    public string shelfAGV = string.Empty;
    /// <summary>
    /// 获取成品物料代码
    /// </summary>
    public List<MaterialModel> MaterialModels { get; set; } = new List<MaterialModel>();

    public List<string> SiloCodes { get; set; } = new List<string>();

    /// <summary>
    /// 库位
    /// </summary>
    public virtual Dictionary<string, LocationData<SiloShelf>> Locations { get; set; } = new();

    public string TransactionMessages { get; set; }
    public string ShelfIndex { get; set; } = "1";
    public List<QueryLocationResponse> RackInfos { get; set; } = new List<QueryLocationResponse>();

    public SiloShelf(DeviceDescriptor deviceDescriptor, IServiceProvider serviceProvider, IDeviceEngine deviceEngine)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
        logger = LoggerFactory.CreateLogger<SiloShelf>();

        layerLimit = deviceDescriptor.LayerLimit.ToInt();
        spindleNum = DeviceDescriptor.SpindleNum.ToInt();
        _fromLocal = deviceDescriptor.Extra["IsStandAlone"].ToBool();

        Connector.ConnectFunc = (device) => Task.FromResult(true);
        CollectDataFunc = (d) => PropertyContainer[d.GetType()].CollectPropertyValues();
        shelfAGV = deviceDescriptor.Extra.GetConfig("ShelfAGV").ToStr();
        AddCommand("SetSiloCommand", (r) => SetSiloHandler(r));
        AddCommand("RemoveSiloCommand", (r) => RemoveSiloHandler(r));
        AddCommand("ResetStatusCommand", (r) => ResetStatusHandler(r));
    }

    protected override async Task Initialize()
    {
        if (!_fromLocal)
            await GetRackInfo();

        InitLocationData();
        CreateShelfHandler();
        await GetPanelItemCodeAsync();
        SiloCodes.Add("");
        SiloCodes.AddRange((await this.GetSiloCodeAsync())!);
    }

    protected virtual void CreateShelfHandler()
    {
        ShelfHandlers.CreateShelfHandler(this);
    }

    protected override async Task OnApplicationStarted()
    {
        await DeviceStore.LoadPayloadPanels(this);
        logger.LogInformation($"PayloadPanels2: 加载完成");
        LoadPanels();
    }

    public void LoadPanels()
    {
        try
        {
            _autoResetEvent.WaitOne();
            foreach (var location in Locations)
            {
                location.Value?.LoadPanels();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "加载LoadPanels时 出错了");
        }
        finally
        {
            IsCanLoopSync = true;
            _autoResetEvent.Set();
        }
    }

    public async Task ReportPanels()
    {
        foreach (var location in Locations)
        {
            try
            {
                var request = new DevicePanelChangedRequest
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    PanelList = location.Value.Panels,
                };
                var result = await DataExporter.DevicePanelChangedReport(request);

                if (result == null)
                {
                    logger.LogError($"重新上报板料时，和服务器断开连接，上报失败");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReportPanels时 出错了");
            }
        }
    }

    protected override async Task OnApplicationStopping()
    {
        await DeviceStore.SavePayloadPanels(this);
    }

    public async Task<DeviceServiceInvokeResponse> SetSiloHandler(DeviceServiceInvokeRequest request)
    {
        logger.LogInformation($"SetSiloHandler request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");

        try
        {
            // int position = CheckRemoteParams(request);
            // return await Locations[$"{position.ToStr()}"].SetSiloHandler(request!);
            return await Locations.Values.FirstOrDefault(x => x.LocationCode == request.LocationCode)?.SetSiloHandler(request!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"SetSiloHandler Error: \r\n {ex.Message}");

            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = ex.Message,
            };
        }
    }

    public async Task<DeviceServiceInvokeResponse> ResetStatusHandler(DeviceServiceInvokeRequest request)
    {
        logger.LogInformation($"ResetStatusHandler request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");

        try
        {
            //int position = CheckRemoteParams(request);

            //return await Locations[$"{position.ToStr()}"].ResetStatusHandler(request!);
            return await Locations.Values.FirstOrDefault(x => x.LocationCode == request.LocationCode)?.ResetStatusHandler(request!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ResetStatusHandler Error: \r\n {ex.Message}");

            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = ex.Message,
            };
        }
    }

    public async Task<DeviceServiceInvokeResponse> RemoveSiloHandler(DeviceServiceInvokeRequest request)
    {
        logger.LogInformation($"RemoveSiloHandler request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");

        try
        {
            //int position = CheckRemoteParams(request);

            //return await Locations[$"{position.ToStr()}"].RemoveSiloHandler(request!);
            return await Locations.Values.FirstOrDefault(x => x.LocationCode == request.LocationCode)?.RemoveSiloHandler(request!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"RemoveSiloHandler Error: \r\n {ex.Message}");

            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = ex.Message,
            };
        }
    }

    protected int CheckRemoteParams(DeviceServiceInvokeRequest request)
    {
        if (request == null) throw new Exception("下发参数 request 异常 !");

        string? deviceCode = request?.Params?["DeviceCode"].ToStr();
        if (string.IsNullOrEmpty(deviceCode)) throw new Exception("参数错误: DeviceCode 为空");

        int position = deviceCode.Substring(deviceCode.Length - 3).ToInt();
        if (position <= 0) throw new Exception("无效的DeviceCode");

        // if (!request!.PayloadPanels.Exists(p=>p.Position==position)) throw new Exception("DeviceCode的 Position 与 PayloadPanels 中的Position 不一致!");
        return position;
    }

    public async Task<List<QueryLocationResponse>> GetRackInfo()
    {
        var rackInfo = await HttpRequestInvoker.GetFromJsonAsync<List<QueryLocationResponse>>(string.Format(DeviceDescriptor.Extra["GetRackInfo"].ToStr(), DeviceId));
        if (rackInfo != null && rackInfo.Any())
        {
            RackInfos = rackInfo;
            spindleNum = RackInfos.Count;
        }
        else { spindleNum = 0; }

        return RackInfos;
    }

    public async Task GetPanelItemCodeAsync(QueryOrderByEnum queryOrder = QueryOrderByEnum.OrderByCreateTimeDesc, string matchItemCode = "")
    {
        if (MaterialModels == null)
        {
            MaterialModels = new List<MaterialModel>() { };
        }

        if (!Locations.Any())
        {
            logger.LogInformation("没有配置库位，退出处理；end to GetPanelItemCodeAsync");
            return;
        }

        MaterialModels.Clear();
        MaterialModels.Add(new MaterialModel() { Name = "", Code = "" });
        logger.LogInformation("begin to GetPanelItemCodeAsync");
        var condition = new GetItemListReq { QueryOrderBy = queryOrder, Code = matchItemCode };
        var MaterialModel = (await HttpRequestInvoker.PostAsJsonAsync<GetItemListReq, List<MaterialModel>>(CentralWebOptions.GetItemCode, condition));
        if (!string.IsNullOrEmpty(Locations[ShelfIndex].CurrentContext.MaterialMatchCode))
        {
            MaterialModel = MaterialModel.Where(p => p.Code.Contains(Locations[ShelfIndex].CurrentContext.MaterialMatchCode)).ToList();
        }

        MaterialModels.AddRange(MaterialModel);
        logger.LogInformation("end to GetPanelItemCodeAsync");
    }

    public void setNewPanelLog(PanelList panels, string shelfName)
    {
        if (!string.IsNullOrEmpty(panels[0].SiloCode))
        {
            logger.LogWarning($"=======中转位{shelfName}的料仓{panels[0].SiloCode}的物料信息被维护位{JsonSerializer.Serialize(panels)}=============");
        }
        else
        {
            logger.LogWarning($"=======中转位{shelfName}的物料信息被维护位: 无料仓 =============");
        }
    }
    public override Task<DeviceServiceInvokeResponse> CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        logger.LogInformation($"CompleteSchedule_Request: {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");

        string? shelfIndex = deviceServiceInvokeRequest?.EventId?.Split('#')[1];

        Locations[shelfIndex.ToStr()].CompleteSchedule(deviceServiceInvokeRequest!);

        return base.CompleteSchedule(deviceServiceInvokeRequest!);
    }

    public override Task<DeviceServiceInvokeResponse> CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        string? shelfIndex = deviceServiceInvokeRequest?.EventId?.Split('#')[1];

        Locations[shelfIndex.ToStr()].CancelSchedule(deviceServiceInvokeRequest!);

        return base.CancelSchedule(deviceServiceInvokeRequest!);
    }

    public override async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        var result = await PropertyContainer[GetType()].ReadProperties(deviceServiceInvokeRequest);

        return result;
    }

    public override async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await PropertyContainer[GetType()].WriteProperties(deviceServiceInvokeRequest);
    }

    protected virtual void InitLocationData()
    {
        if (_fromLocal)
        {
            Locations = new Dictionary<string, LocationData<SiloShelf>>(spindleNum);
            for (int i = 1; i <= spindleNum; i++)
            {
                Locations[$"{i}"] = ObjectFactory.CreateObject<LocationData<SiloShelf>>(this);
                Locations[$"{i}"].Panels.CollectionChanged += OnPanelsChanged;
                Locations[$"{i}"].Position = i.ToStr();
                Locations[$"{i}"].LocationCode = $"{DeviceId}{i.ToStr().PadLeft(3, '0')}";
                Locations[$"{i}"].CurrentContext.ShelfIndex = i;
                Locations[$"{i}"].CurrentContext.RackIndex = i - 1;
                Locations[$"{i}"].CurrentContext.LocationCode = Locations[$"{i}"].LocationCode;
                Locations[$"{i}"].CurrentContext.SiloCode = string.Empty;
                Locations[$"{i}"].LoadLocalPointConfig();
            }
        }
        else
        {
            if (RackInfos.Count == 0)
            {
                logger.LogError("未能找到对应的库位，程序已退出");
                return;
            }

            Locations = new Dictionary<string, LocationData<SiloShelf>>(RackInfos.Count);
            var locations = RackInfos.OrderBy(x => x.Code).ToList();
            for (int i = 1; i <= locations.Count; i++)
            {
                var rack = locations[i - 1];
                Locations[$"{i}"] = ObjectFactory.CreateObject<LocationData<SiloShelf>>(this);
                Locations[$"{i}"].Panels.CollectionChanged += OnPanelsChanged;
                Locations[$"{i}"].LocationCode = rack.Code ?? string.Empty;
                Locations[$"{i}"].CurrentContext.SiloCode = rack.SiloCode ?? string.Empty;
                Locations[$"{i}"].Position = i.ToStr();
                Locations[$"{i}"].CurrentContext.ShelfIndex = i;
                Locations[$"{i}"].CurrentContext.RackIndex = i - 1;
                Locations[$"{i}"].CurrentContext.LocationCode = rack.Code.ToStr();
                Locations[$"{i}"].FeedAGVOutputPoint = rack.FeedAGVOutputPoint;
                Locations[$"{i}"].FeedAGVInnerPoint = rack.FeedAGVInnerPoint;
                Locations[$"{i}"].FeedAGVRestPoint = rack.FeedAGVRestPoint;
                Locations[$"{i}"].TransAGVOutputPoint = rack.TransAGVOutputPoint;
                Locations[$"{i}"].TransAGVInnerPoint = rack.TransAGVInnerPoint;
                Locations[$"{i}"].TransAGVRestPoint = rack.TransAGVRestPoint;
                Locations[$"{i}"].PositonCode = rack.PositionCode;
            }
        }
    }

    protected override Task<DevicePanelChangedResponse> OnPanelsChanged(string locationCode)
    {
        var location = Locations.Values.FirstOrDefault(x => x.LocationCode == locationCode);
        if (location != null)
        {
            DeviceStore.SavePayloadPanels(location.Panels);
            return Task.FromResult(new DevicePanelChangedResponse
            {
                Code = ErrorCodes.Sys.SUCCESS
            });
        }

        return Task.FromResult(new DevicePanelChangedResponse
        {
            Code = ErrorCodes.Sys.FAIL
        });
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToShelfSiloShelfLoadPolicy.PrepareLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToShelfSiloShelfLoadPolicy.InvokeLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToShelfSiloShelfLoadPolicy.CompleteLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToShelfSiloShelfUnloadPolicy.PrepareUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToShelfSiloShelfUnloadPolicy.InvokeUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToShelfSiloShelfUnloadPolicy.CompleteUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public override async Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public override async Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public override async Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public async Task<string> ForeCallAgv(int SpindleNum = 1)
    {
        return await Locations[SpindleNum.ToStr()].ForeCallAgv();
    }
}
