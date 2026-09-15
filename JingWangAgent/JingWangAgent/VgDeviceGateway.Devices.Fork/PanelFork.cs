// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Fork;

public class PanelFork : Device
{
    private readonly ILogger<PanelFork> _logger;
    private volatile int spindleNum = 3;
    public volatile int layerLimit = 18;

    /// <summary>
    /// 获取成品物料代码
    /// </summary>
    public List<MaterialModel> MaterialModels { get; set; } = new List<MaterialModel>();

    public List<string> SiloCodes { get; set; } = new List<string>();

    /// <summary>
    /// 库位
    /// </summary>
    public Dictionary<int, ForkLocationData<PanelFork>> Locations { get; set; } = new();

    public int ShelfIndex { get; set; } = 1;
    public List<ExternalRackDto> RackInfos { get; set; } = new List<ExternalRackDto>();

    public PanelFork(DeviceDescriptor deviceDescriptor,
        IServiceProvider serviceProvider,
        IDeviceEngine deviceEngine)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
        _logger = LoggerFactory.CreateLogger<PanelFork>();
        spindleNum = DeviceDescriptor.SpindleNum.ToInt();
        layerLimit = DeviceDescriptor.LayerLimit.ToInt();
        InitLocationData();

        Connector.ConnectFunc = (device) => Task.FromResult(true);
        CollectDataFunc = (d) => PropertyContainer[d.GetType()].CollectPropertyValues();
        PropertyContainer.AddHandler<PanelForkPropertyHandler, PanelFork>(this);
        EventContainer.AddHandler<PanelForkEventHandler, PanelFork>(this);
        AddCommand("SetSiloCommand", (r) => SetSiloHandler(r));
    }

    private void InitLocationData()
    {
        for (int i = 1; i <= spindleNum; i++)
        {
            Locations[i] = ObjectFactory.CreateObject<ForkLocationData<PanelFork>>(this);
            Locations[i].Position = i;
            Locations[i].LocationCode = GetLocationCode(i);
            Locations[i].SetConfigs();
        }
    }

    protected override async Task Initialize()
    {
        await GetPanelItemCodeAsync();

        SiloCodes.Add("");
        SiloCodes.AddRange(await this.GetSiloCodeAsync());
    }

    protected override async Task OnApplicationStarted()
    {
        try
        {
            var locationScheduleInfos = await this.HttpRequestInvoker.GetFromJsonAsync<List<LocationScheduleInfo>>(string.Format(CentralWebOptions.ScheduleLocation, DeviceId)) ?? new List<LocationScheduleInfo>();
            if (locationScheduleInfos.Any())
            {
                foreach (var locationScheduleInfo in locationScheduleInfos)
                {
                    var location = Locations.Values.FirstOrDefault(x => x.LocationCode == locationScheduleInfo.LocationCode);
                    if (location != null)
                    {
                        location.TranscationId = locationScheduleInfo.TraceId ?? string.Empty;
                        location.Panels = locationScheduleInfo.Panels;
                        location.TransAGVInnerPoint = locationScheduleInfo.TransInnerPoint;
                        location.TransAGVOutputPoint = locationScheduleInfo.TransOutPoint;
                    }
                }
            }
            else
            {
                for (int i = 1; i <= spindleNum; i++)
                {
                    Locations[i].Panels = Panel.NoSilo.PanelForSingleSpindle(i, 0, layerLimit);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public string GetLocationCode(int position) => $"{DeviceDescriptor.DeviceId}{position.ToString().PadLeft(3, '0')}";

    public async Task<DeviceServiceInvokeResponse> SetSiloHandler(DeviceServiceInvokeRequest request)
    {
        try
        {
            if (request == null
                || request.PayloadPanels == null
                || !request.Params.TryGetValue("DeviceCode", out var deviceCode))
                throw new Exception("下发参数 request 异常 !");

            string? locationCode = deviceCode.ToStr();
            if (string.IsNullOrEmpty(locationCode))
                throw new Exception("参数错误: DeviceCode 为空");

            int position = locationCode.Substring(locationCode.Length - 3).ToInt();
            if (position <= 0 || !Locations.ContainsKey(position)) throw new Exception("无效的DeviceCode");

            return await Locations[position].SetSiloHandler(request!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"SetSiloHandler Error: \r\n {ex.Message}");

            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = ex.Message,
            };
        }
    }

    public async Task<List<ExternalRackDto>> GetRackInfo()
    {
        var rackInfo = await HttpRequestInvoker.PostAsJsonAsync<ExternalRackQueryReq, ResponseDto<List<ExternalRackDto>>>(
            DeviceDescriptor.Extra["GetRackInfo"].ToStr(),
            new ExternalRackQueryReq() { });

        if (rackInfo != null && rackInfo.Code == 0 && rackInfo.Data != null && rackInfo.Data.Any())
        {
            RackInfos = rackInfo.Data;
            spindleNum = RackInfos.Count;
        }
        else
        {
            spindleNum = 1;
        }

        return RackInfos;
    }

    public async Task GetPanelItemCodeAsync(QueryOrderByEnum queryOrder = QueryOrderByEnum.OrderByCreateTimeDesc, string matchItemCode = "")
    {
        if (MaterialModels == null)
        {
            MaterialModels = new List<MaterialModel>() { };
        }

        MaterialModels.Clear();
        MaterialModels.Add(new MaterialModel() { Name = "", Code = "" });
        MaterialModels.Add(new MaterialModel() { Name = "ww", Code = "334ff" });
        _logger.LogInformation("begin to GetPanelItemCodeAsync");
        var condition = new GetItemListReq { QueryOrderBy = queryOrder, Code = matchItemCode };
        var MaterialModel = await HttpRequestInvoker.PostAsJsonAsync<GetItemListReq, List<MaterialModel>>(CentralWebOptions.GetItemCode, condition) ?? new List<MaterialModel>();
        if (!string.IsNullOrEmpty(Locations[ShelfIndex].CurrentContext.MaterialMatchCode))
        {
            MaterialModel = MaterialModel.Where(p => p.Code.Contains(Locations[ShelfIndex].CurrentContext.MaterialMatchCode)).ToList();
        }

        MaterialModels.AddRange(MaterialModel);
        _logger.LogInformation("end to GetPanelItemCodeAsync");
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await Locations[deviceServiceInvokeRequest.GetPosition()].CompleteSchedule(deviceServiceInvokeRequest);
    }

    public override Task<DeviceServiceInvokeResponse> CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return Locations[deviceServiceInvokeRequest.GetPosition()].CancelSchedule(deviceServiceInvokeRequest!);
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await Locations[deviceServiceInvokeRequest.GetPosition()].PrepareLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await Locations[deviceServiceInvokeRequest.GetPosition()].InvokeLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await Locations[deviceServiceInvokeRequest.GetPosition()].CompleteLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await Locations[deviceServiceInvokeRequest.GetPosition()].PrepareUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await Locations[deviceServiceInvokeRequest.GetPosition()].InvokeUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await Locations[deviceServiceInvokeRequest.GetPosition()].CompleteUnloadMaterial(deviceServiceInvokeRequest);
    }
}
