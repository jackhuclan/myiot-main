using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.OpenAPI;

namespace VgDeviceGateway.Devices.Common;

public static class DeviceExtensions
{
    public static async Task<List<string>> GetSiloCodeAsync(this Device deivce, string siloMatch = "")
    {
        if (deivce.DeviceDescriptor.Extra.ContainsKey("QuerySilosV2"))
        {
            if (string.IsNullOrEmpty(deivce.DeviceDescriptor.Extra["QuerySilosV2"].ToString()))
                throw new ArgumentNullException("request uri cannot be empty");

            var items = await deivce.HttpRequestInvoker.PostAsJsonAsync<QuerySiloCodeRequest, List<string>>(deivce.DeviceDescriptor.Extra["QuerySilosV2"].ToString(),
                new QuerySiloCodeRequest
                {
                    Code = siloMatch
                });

            return items ?? new();
        }
        else
        {
            if (!deivce.DeviceDescriptor.Extra.TryGetValue("RequestSiloCodes", out var requestSiloCodes))
                throw new KeyNotFoundException("Cannot find key \"RequestSiloCodes\"");

            var siloCode = requestSiloCodes.ToString();
            if (string.IsNullOrEmpty(siloCode))
                throw new ArgumentNullException("request uri cannot be empty");

            return await deivce.HttpRequestInvoker.GetFromJsonAsync<List<string>>(siloCode) ?? new();
        }

        //获取料仓代码 升级
        //if (!deivce.DeviceDescriptor.Extra.ContainsKey("QuerySilosV2"))
        //    throw new KeyNotFoundException("Cannot find key \"QuerySilosV2\"");
        //if (string.IsNullOrEmpty(deivce.DeviceDescriptor.Extra["QuerySilosV2"].ToString()))
        //    throw new ArgumentNullException("request uri cannot be empty");

        //var items = await deivce.HttpRequestInvoker.PostAsJsonAsync<QuerySiloCodeRequest, List<string>>(deivce.DeviceDescriptor.Extra["QuerySilosV2"].ToString(),
        //    new QuerySiloCodeRequest
        //    {
        //        Code = siloMatch
        //    });
        //return items;
    }

    public static bool SiloIsHaveNoRawByPosition(List<Panel> panels)
    {
        return (!panels.Any(s => s.ProductStatus == ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1));
        // && panels.Count(p => p.ProductStatus == ProductStatus.EmptySiloBox) < DeviceDescriptor.Extra["DrillSpindleNum"].ToInt();
    }

    public static ProductStatus ConvertOutPutStatus(List<Panel> panelList)
    {
        ProductStatus status = ProductStatus.Noop;
        // 所有为空
        if (panelList.Any() && panelList.All(p => p.ProductStatus == ProductStatus.EmptySiloBox))
        {
            status = ProductStatus.EmptySiloBox;
        }
        else if (panelList.Any(p => p.ProductStatus == ProductStatus.Finished_PRE_BUFFER))
        {
            status = ProductStatus.Finished_PRE_BUFFER;
        }
        else if (panelList.Any(p => p.ProductStatus == ProductStatus.Finished_POST_BUFFER))
        {
            status = ProductStatus.Finished_POST_BUFFER;
        }
        else if (panelList.Any(p => p.ProductStatus == ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1))
        {
            status = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1;
        }
        else if (!panelList.Any(p => p.ProductStatus == ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1)
               && panelList.Any(p => p.ProductStatus == ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1
               && panelList.Any(p => p.ProductStatus == ProductStatus.EmptySiloBox)))
        {
            status = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1;
        }

        return status;
    }

    public static object GetConfig(this Dictionary<string, object> dic, string key)
    {
        if (dic == null)
        {
            return new { };
        }

        return dic.TryGetValue(key, out var value) ? value : new { };
    }
}

