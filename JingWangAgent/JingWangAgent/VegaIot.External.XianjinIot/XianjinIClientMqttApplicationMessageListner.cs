using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgDeviceGateway.Devices.Agv;

namespace VegaIot.External.XianjinIot;

internal class XianjinIClientMqttApplicationMessageListner : DefaultClientMqttApplicationMessageListener
{
    private readonly IDeviceProvider deviceProvider_;
    //DeviceDescriptor deviceDescriptor_;

    public XianjinIClientMqttApplicationMessageListner(IMqttClient mqttClient,
        //DeviceDescriptor deviceDescriptor,
        IDeviceProvider deviceProvider,
        ILoggerFactory loggerFactory) :
        base(mqttClient, deviceProvider, loggerFactory)
    {
        deviceProvider_ = deviceProvider;
        //deviceDescriptor_ = deviceDescriptor;
    }

    public override async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        //var device = deviceProvider_.GetDevice(deviceDescriptor_.DeviceId) as DefaultDrill;
        //var deviceDrill = deviceProvider_.Devices[0] as DefaultDrill;
        var deviceAgv = deviceProvider_.Devices[0] as DefaultAgv;

        #region walk_location

        if (arg.ApplicationMessage.Topic.Contains("walk_location"))
        {
            var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
            {
                ProductId = deviceAgv.ProductId,
                DeviceId = deviceAgv.DeviceId,
                ServiceId = Topics.Services.AGV_MOVE_SERVICE_ID,
                CallerRequestInteractionDirection = InteractionPosition.Rear,
                CallerRequestMaterialKind = MaterialKind.Panel,
                Params = new Dictionary<string, object?>
                    {
                       { "PayloadSegment",arg.ApplicationMessage.PayloadSegment}
                    }
            };

            await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        }

        #endregion walk_location

        #region adjust_height

        if (arg.ApplicationMessage.Topic.Contains("adjust_height"))
        {
            var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
            {
                ProductId = deviceAgv.ProductId,
                DeviceId = deviceAgv.DeviceId,
                ServiceId = Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID,
                CallerRequestInteractionDirection = InteractionPosition.Rear,
                CallerRequestMaterialKind = MaterialKind.PanelSilo,
                Params = new Dictionary<string, object?>
                    {
                       { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
                    }
            };

            await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        }

        #endregion adjust_height

        #region load_silo

        if (arg.ApplicationMessage.Topic.Contains("load_silo"))
        {
            var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
            {
                ProductId = deviceAgv.ProductId,
                DeviceId = deviceAgv.DeviceId,
                ServiceId = Topics.Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID,
                CallerRequestInteractionDirection = InteractionPosition.Rear,
                CallerRequestMaterialKind = MaterialKind.PanelSilo,
                Params = new Dictionary<string, object?>
                    {
                       { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
                    }
            };

            await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        }

        #endregion load_silo

        #region up_silo_floor

        if (arg.ApplicationMessage.Topic.Contains("up_silo_floor"))
        {
            var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
            {
                ProductId = deviceAgv.ProductId,
                DeviceId = deviceAgv.DeviceId,
                ServiceId = Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID,
                CallerRequestInteractionDirection = InteractionPosition.Rear,
                CallerRequestMaterialKind = MaterialKind.PanelSilo,
                Params = new Dictionary<string, object?>
                    {
                       { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
                    }
            };

            await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        }

        #endregion up_silo_floor

        #region push_panel

        if (arg.ApplicationMessage.Topic.Contains("push_panel"))
        {
            var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
            {
                ProductId = deviceAgv.ProductId,
                DeviceId = deviceAgv.DeviceId,
                ServiceId = Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID,
                CallerRequestInteractionDirection = InteractionPosition.Rear,
                CallerRequestMaterialKind = MaterialKind.Panel,
                Params = new Dictionary<string, object?>
                    {
                       { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
                    }
            };

            await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        }

        #endregion push_panel

        //#region receive_panel
        //if (arg.ApplicationMessage.Topic.Contains("receive_panel"))
        //{
        //    //var preReceiveMaterialsPayload = JsonSerializer.Deserialize<PreReceiveMaterialsPayload>(arg.ApplicationMessage.PayloadSegment);

        //    var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
        //    {
        //        ProductId = device.ProductId,
        //        DeviceId = device.DeviceId,
        //        ServiceId = Topics.Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID,
        //        CallerRequestInteractionDirection = InteractionPosition.Rear,
        //        CallerRequestMaterialKind = MaterialKind.Panel,
        //        Params = new Dictionary<string, object?>
        //            {
        //                { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
        //                { "Position",1},//,rearDrill2849Context.splineNum
        //                { "IsLoadAndUnload",0},//0优化CT用
        //                { "SpindlePosition",1},//item.SpindleId;
        //                { "IsFirstStep",true},//,step == 1
        //                { "IsLastStep",false}
        //            }

        //    };

        //    await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        //}
        //#endregion

        //#region CreateTask
        //if (arg.ApplicationMessage.Topic.Contains("drilling/create_task"))
        //{
        //    //var preReceiveMaterialsPayload = JsonSerializer.Deserialize<PreReceiveMaterialsPayload>(arg.ApplicationMessage.PayloadSegment);
        //    var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
        //    {
        //        ProductId = device.ProductId,
        //        DeviceId = device.DeviceId,
        //        ServiceId = Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID,
        //        CallerRequestInteractionDirection = InteractionPosition.Rear,
        //        CallerRequestMaterialKind = MaterialKind.Panel,
        //        Params = new Dictionary<string, object?>
        //            {
        //                { "PayloadSegment",arg.ApplicationMessage.PayloadSegment}
        //            }
        //    };

        //    await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        //}
        //#endregion

        //#region pre_receive_materials
        //if (arg.ApplicationMessage.Topic.Contains("drilling/pre_receive_materials"))
        //{
        //    //var preReceiveMaterialsPayload = JsonSerializer.Deserialize<PreReceiveMaterialsPayload>(arg.ApplicationMessage.PayloadSegment);
        //    var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
        //    {
        //        ProductId = device.ProductId,
        //        DeviceId = device.DeviceId,
        //        ServiceId = Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID,
        //        CallerRequestInteractionDirection = InteractionPosition.Rear,
        //        CallerRequestMaterialKind = MaterialKind.Panel,
        //        Params = new Dictionary<string, object?>
        //            {
        //                { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
        //                { "Position",1},//,rearDrill2849Context.splineNum
        //                { "IsLoadAndUnload",0},//0优化CT用
        //                { "SpindlePosition",1},//item.SpindleId;
        //                { "IsFirstStep",true},//,step == 1
        //                { "IsLastStep",false}
        //            }
        //    };

        //    await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        //}
        //#endregion

        //#region receive_materials
        //if (arg.ApplicationMessage.Topic.Contains("drilling/receive_materials"))
        //{
        //    //var preReceiveMaterialsPayload = JsonSerializer.Deserialize<PreReceiveMaterialsPayload>(arg.ApplicationMessage.PayloadSegment);

        //    var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
        //    {
        //        ProductId = device.ProductId,
        //        DeviceId = device.DeviceId,
        //        ServiceId = Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID,
        //        CallerRequestInteractionDirection = InteractionPosition.Rear,
        //        CallerRequestMaterialKind = MaterialKind.Panel,
        //        Params = new Dictionary<string, object?>
        //            {
        //                { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
        //                { "Position",1},//,rearDrill2849Context.splineNum
        //                { "IsLoadAndUnload",0},//0优化CT用
        //                { "SpindlePosition",1},//item.SpindleId;
        //                { "IsFirstStep",true},//,step == 1
        //                { "IsLastStep",false}
        //            }

        //    };

        //    await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        //}
        //#endregion

        //#region pre_unloading
        //if (arg.ApplicationMessage.Topic.Contains("drilling/pre_unloading"))
        //{
        //    //var preUnloadingPayload = JsonSerializer.Deserialize<PreUnloadingPayload>(arg.ApplicationMessage.PayloadSegment);
        //    var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
        //    {
        //        ProductId = device.ProductId,
        //        DeviceId = device.DeviceId,
        //        ServiceId = Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID,
        //        CallerRequestInteractionDirection = InteractionPosition.Rear,
        //        CallerRequestMaterialKind = MaterialKind.Panel,
        //        Params = new Dictionary<string, object?>
        //            {
        //                { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
        //                { "Position",1},//,rearDrill2849Context.splineNum
        //                { "IsLoadAndUnload",0},//0优化CT用
        //                { "SpindlePosition",1},//item.SpindleId;
        //                { "IsFirstStep",true},//,step == 1
        //                { "IsLastStep",false}
        //            }
        //    };
        //    await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        //}
        //#endregion

        //#region unloading

        //if (arg.ApplicationMessage.Topic.Contains("drilling/unloading"))
        //{
        //    //var preReceiveMaterialsPayload = JsonSerializer.Deserialize<UnloadingPayload>(arg.ApplicationMessage.PayloadSegment);
        //    var deviceServiceInvokeRequest = new DeviceServiceInvokeRequest()
        //    {
        //        ProductId = device.ProductId,
        //        DeviceId = device.DeviceId,
        //        ServiceId = Topics.Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID,
        //        CallerRequestInteractionDirection = InteractionPosition.Rear,
        //        CallerRequestMaterialKind = MaterialKind.Panel,
        //        Params = new Dictionary<string, object?>
        //            {
        //                { "PayloadSegment",arg.ApplicationMessage.PayloadSegment},
        //                { "Position",1},//,rearDrill2849Context.splineNum
        //                { "IsLoadAndUnload",0},//0优化CT用
        //                { "SpindlePosition",1},//item.SpindleId;
        //                { "IsFirstStep",true},//,step == 1
        //                { "IsLastStep",false}
        //            }

        //    };
        //    await Services[arg.ApplicationMessage.Topic].Invoke(deviceServiceInvokeRequest);
        //}
        //#endregion
    }
}
