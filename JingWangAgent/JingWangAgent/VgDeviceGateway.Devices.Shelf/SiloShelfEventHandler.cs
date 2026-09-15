using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Shelf;

public class SiloShelfEventHandler : AbstractEventHandler<SiloShelf>
{
    private readonly ILogger<SiloShelfEventHandler> logger;

    public SiloShelfEventHandler(ILogger<SiloShelfEventHandler> logger,
        IServiceProvider serviceProvider,
        SiloShelf siloShelf)
        : base(serviceProvider, siloShelf)
    {
        this.logger = logger;
    }

    private static int seed = 1;

    public override void AddWatchingEvents()
    {
        // WatchingProperties.Property("TranscationChange")
        //.When(p => p.NewValue.ToBool() == true)
        //.TriggerAlways(() =>
        //{
        //   StringBuilder sbl = new StringBuilder();
        //    foreach (var location in InteractingDevice.Locations.Values)
        //    {
        //        sbl.Append($"{location.Position}号料架:{location.TransactionMessage} \r\n");
        //    }

        //    InteractingDevice.TransactionMessages = sbl.ToString();
        //});

        foreach (var location in InteractingDevice.Locations.Values)
        {
            WatchingProperties.Properties($"IsReady{location.Position}", $"IsCanCallAgv{location.Position}", $"IsAgvWorking{location.Position}")
            .When(proteryties =>
            {
                return proteryties.Property($"IsReady{location.Position}").NewValue.ToBool()
                       && proteryties.Property($"IsCanCallAgv{location.Position}").NewValue.ToBool()
                       && !proteryties.Property($"IsAgvWorking{location.Position}").NewValue.ToBool();
            })
            .TriggerAlways(async () =>
            {
                try
                {
                    if (location.IsBusy)
                    {
                        return;
                    }

                    location.IsBusy = true;

                    Random random = new Random(seed);
                    var randomNumber = random.Next(10, 1000);
                    await Task.Delay(10 * randomNumber);

                    var result = await location.CallAgv(true);
                    location.TransactionMessage = result?.Message!;
                }
                catch (Exception ee)
                {
                    logger.LogError($"{location.LocationCode} 呼叫AGV异常 ： {ee.Message} ");
                    location.TransactionMessage = $"呼叫AGV异常 ： {ee.Message}";
                }

                location.IsBusy = false;
                WatchingProperties.Property("TranscationChange").SetValue(true);
            });
        }

        WatchingProperties.Property("MqttConnected")
           .PostCondition(p => !p.NewValue.ToBool())
           .TriggerAlways(() =>
           {
               logger.LogDebug($"{DateTime.Now.ToShortTimeString()}  mqtt连接断开!!! ");
               foreach (var shelf in InteractingDevice.Locations)
               {
                   shelf.Value.IsCanCallAgv = true;
               }
           });

        WatchingProperties.Property("MqttConnected")
         .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
         .TriggerAlways(async () =>
         {
             logger.LogInformation($"{DateTime.Now.ToShortTimeString()}: Reconnect_Mqtt :  监听到mqtt重新连接成功!，触发事件! ");

             InteractingDevice.LoadPanels();

             await InteractingDevice.ReportPanels();
         });
    }
}
