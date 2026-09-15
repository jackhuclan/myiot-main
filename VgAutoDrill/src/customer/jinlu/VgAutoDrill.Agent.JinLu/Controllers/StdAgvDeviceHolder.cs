using System.Text.Json.Nodes;

namespace VgAutoDrill.Agent.Controllers
{
    public class StdAgvDeviceHolder : IStdAgvDeviceHolder
    {
        private Dictionary<string, CallBackEntity> arrivedAGVs = new();

        public CallBackEntity GetStdAgv(string orderid)
        {
            if (arrivedAGVs.TryGetValue(orderid, out var jobj))
            {
                return jobj;
            }

            return new CallBackEntity();
        }

        public void AddStdAgv(string orderid, CallBackEntity obj)
        {
            arrivedAGVs[orderid] = obj;
        }
    }
    public interface IStdAgvDeviceHolder
    {
        CallBackEntity GetStdAgv(string orderid);

        void AddStdAgv(string orderid, CallBackEntity obj);
    }
}
