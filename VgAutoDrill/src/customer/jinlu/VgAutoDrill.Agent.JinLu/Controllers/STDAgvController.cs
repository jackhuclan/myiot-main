using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Fundation.Vendor.STD;

namespace VgAutoDrill.Agent.Controllers
{
    [ApiController]
    [Route("api/v2")]
    public class STDAgvController : ControllerBase
    {
        private static IStdAgvDeviceHolder _stdAgvDeviceHolder = new StdAgvDeviceHolder();
        private readonly IDeviceProvider _deviceProvider;
        public STDAgvController(IDeviceProvider deviceProvider)
        {
            this._deviceProvider = deviceProvider;
        }

        [HttpGet("hello")]
        public string hello()
        {
            return "hello";
        }

        [HttpPost("action/command", Name = "Notification")]
        public object Notification(CallBackEntity status)
        {
            Console.WriteLine($"Arrived input:{JsonSerializer.Serialize(status)}");

            //vehicleId(车辆id，int类型)、
            //vehicleName(车辆名字，string类型)、
            //action(发送的自定义动作数组，int数组)、
            //mapId（地图id，int类型）、
            //stationNo（站点id，int类型）、
            //stationName（站点名字，string类型）、
            //orderId（订单id，long类型）

            _stdAgvDeviceHolder.AddStdAgv(status.order_id.ToStr(), status);
            var device = _deviceProvider.GetDevice(status.vehicle_name);
            if (device.ActionStatus == ActionStatus.Done)
            {
                device.ActionStatus = ActionStatus.ToDo;
                return new { Code = 200 };
            }
            else
            {
                return new { Code = 201 };
            }
        }

        /// <summary>
        /// get online agv
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpGet("online", Name = "GetStdAgv")]
        public CallBackEntity GetStdAgv(string orderid)
        {
            var entity = _stdAgvDeviceHolder.GetStdAgv(orderid);
            Console.WriteLine(entity);
            return entity;
        }
    }
}
