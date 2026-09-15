using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal class ScanPanelCodeEventDelegator : IDeviceEventDelegator
{
    private readonly ILogger<ScanPanelCodeEventDelegator> _logger;
    private readonly IItemAdapter _itemAdapter;
    private readonly IDrillFilePathLocator _drillFilePathLocator;

    public ScanPanelCodeEventDelegator(IItemAdapter itemAdapter,
        IDrillFilePathLocator drillFilePathLocator,
        ILoggerFactory loggerFactory)
    {
        _itemAdapter = itemAdapter;
        _drillFilePathLocator = drillFilePathLocator;
        _logger = loggerFactory.CreateLogger<ScanPanelCodeEventDelegator>();
    }

    public Task<DeviceEventReportResponse> HandleEvent(DeviceEventReportRequest request)
    {
        if (!request.Params.ContainsKey("ItemCode"))
        {
            return Task.FromResult(new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"参数错误，缺少ItemCode"
            });
        }

        var itemCode = request.Params["ItemCode"]?.ToString();
        var drillPath = _drillFilePathLocator.GetFilePath(itemCode);
        //_itemAdapter.save drillPath to db

        var response = new DeviceEventReportResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Params = request.Params,
        };
        response.Params["DrillFilePath"] = drillPath;

        return Task.FromResult(response);
    }
}
