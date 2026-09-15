using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal class DeviceEventReporter : IDeviceEventReporter
{
    private readonly IDeviceManager _deviceHolder;
    private readonly IDeviceEventDelegatorFactory _deviceEventDelegatorFactory;

    public DeviceEventReporter(IDeviceManager deviceHolder,
        IDeviceEventDelegatorFactory deviceEventDelegatorFactory)
    {
        _deviceHolder = deviceHolder;
        _deviceEventDelegatorFactory = deviceEventDelegatorFactory;
    }

    public async Task<DeviceEventReportResponse> Report(DeviceEventReportRequest? request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.EventId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        //查找主叫设备
        var calllerProxy = _deviceHolder.GetOnlineDevice(request.DeviceId);
        if (calllerProxy == null)
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = $"{ErrorCodes.Sys.WRONG_DEVICE_MESSAGE} - {request.DeviceId}"
            };
        }

        var convertedInteractionBehavior = (InteractionBehavior)request.RequestInteractionBehavior;
        if (convertedInteractionBehavior == null)
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"{request.DeviceId} - Invalid request.RequestInteractionBehavior {request.RequestInteractionBehavior}, Cannot convert to InteractionBehavior"
            };
        }

        //事件报告时，不更新、不响应代理的板料信息变化;统一放置在状态报告时，去实现;
        //calllerProxy.SetPanels(request.PayloadPanels);
        ////_ = calllerProxy.PersistPanelsToDb();

        var delegator = _deviceEventDelegatorFactory.CreateEventDelegator(request);
        return await delegator.HandleEvent(request);
    }
}
