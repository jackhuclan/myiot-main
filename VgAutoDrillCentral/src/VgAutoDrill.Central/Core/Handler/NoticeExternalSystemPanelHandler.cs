using System.Text;
using System.Text.Json;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominRequest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominResponse;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Event;

namespace VgAutoDrill.Central.Core.Handler;

internal class NoticeExternalSystemPanelHandler : IEventHandler<PanelChangedEvent>
{
    private readonly ILogger<NoticeExternalSystemPanelHandler> _logger;
    private readonly IAPIHelper _apiHelper;
    private readonly ISysConfigManager _sysConfigManager;

    public NoticeExternalSystemPanelHandler(ILoggerFactory loggerFactory,
        IAPIHelper apiHelper,
        ISysConfigManager sysConfigManager)
    {
        _logger = loggerFactory.CreateLogger<NoticeExternalSystemPanelHandler>();
        _apiHelper = apiHelper;
        _sysConfigManager = sysConfigManager;
    }

    /// <summary>
    /// 通知第三方系统板料加工信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(IReceiveContext<PanelChangedEvent> context, CancellationToken cancellationToken)
    {
        if (!await _sysConfigManager.GetBoolValue(MESConfigConstants.ENABLE_NOTICE_EXTERNAL_SYSTEM_PANELS_DATA))
            return;

        if (context.Message.ChangedLocation.HostDevice == null)
            return;

        if (!context.Message.ChangedLocation.IsDrillLocation)
            return;

        var panels = context.Message.ChangedLocation.OriginPanels;
        var productId = context.Message.ChangedLocation.HostDevice.ProductId;
        var deviceId = context.Message.ChangedLocation.HostDevice.DeviceId;
        var message = new StringBuilder();

        if (panels == null || !panels.Any(x => !string.IsNullOrEmpty(x.PanelCode)))
        {
            _logger.LogInformation($"机台:{deviceId}，二维码信息均为空，无需通知第三方系统.\r\n");
            return;
        }

        var panelLayer = panels.FirstOrDefault();
        if (panelLayer == null)
        {
            _logger.LogInformation($"机台:{deviceId}, 无板料数据，不需要推送第三方系统!");
            return;
        }

        if (panelLayer.Layer != 0 && panelLayer.Layer != 2)
        {
            _logger.LogInformation($"机台:{deviceId}, 板料层级{panelLayer.Layer}非BUFFER上层(0)或下层(2)，不需要推送第三方系统!");
            return;
        }

        //if (panels.FirstOrDefault().Layer != 0 || panels.FirstOrDefault()!.Layer != 2)
        //{
        //    _logger.LogInformation($"机台:{deviceId},板料信息非BUFFER上层或者下层板料数据，不需要推送第三方系统! \r\n");
        //    return;
        //}

        _logger.LogDebug($"机台:{deviceId},板料信息详情:{JsonSerializer.Serialize(panels)}.\r\n");

        var panelGroup = new List<PanelBarcodeItems>();
        foreach (var panel in panels)
        {
            var detail = new PanelBarcodeItems
            {
                AxleNum = panel.Position,
                AluminumSheetQrCode = panel.PanelCode,
            };

            message.AppendLine($"机台:{deviceId},轴号:{panel.Position},板料二维码:{panel.PanelCode},板料状态:{panel.ProductStatus}.\r\n");

            panelGroup.Add(detail);
        }

        _logger.LogInformation($"机台:{deviceId},物料:{productId},加工详情信息:{message}.r\n");

        var drillWithPanelsData = new GetDrillWithPanelsReq
        {
            MachineCode = deviceId,
            ActionType = panels.FirstOrDefault()!.Layer == 0 ? 10 : 20,
            PanelBarcodeItems = panelGroup
        };

        string reqStr = JsonSerializer.Serialize(drillWithPanelsData);
        var url = await _sysConfigManager.GetStringValue(MESConfigConstants.URL_NOTICE_EXTERNAL_SYSTEM_PANELS_DATA, SysConfigCategoryEnum.None, false);
        var data = _apiHelper.RequestData(url, "post", reqStr);
        if (!string.IsNullOrWhiteSpace(data))
        {
            var result = JsonSerializer.Deserialize<DrillWithPanelsResponseDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result != null
                && result.data != null
                && result.data.Success != null
                && result.data.Success.ToLower() == "true")
            {
                _logger.LogInformation($"机台:{deviceId},板料加工数据，发送成功.\r\n");
            }
            else if (result != null && result.data != null)
            {
                _logger.LogInformation($"机台:{deviceId},板料加工数据，发送失败。详情信息:{result.data.Content}.\r\n");
            }
        }
    }
}
