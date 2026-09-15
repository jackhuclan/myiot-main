using VegaIot.External.AgvEntity.STD;

namespace VegaIot.External.Agv.Controllers.STD;

public class STDAgvDeviceHolder : IStdAgvDeviceHolder
{
    private Dictionary<string, STDCallBackEntity> arrivedAGVs = new();

    public STDCallBackEntity GetStdAgv(string orderid)
    {
        if (arrivedAGVs.TryGetValue(orderid, out var jobj))
        {
            return jobj;
        }

        return new STDCallBackEntity();
    }

    public void AddStdAgv(string orderid, STDCallBackEntity obj)
    {
        arrivedAGVs[orderid] = obj;
    }
}

public interface IStdAgvDeviceHolder
{
    STDCallBackEntity GetStdAgv(string orderid);

    void AddStdAgv(string orderid, STDCallBackEntity obj);
}