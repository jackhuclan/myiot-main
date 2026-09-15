using System.Text.Json;
using VegaIot.External.AgvEntity.Hik;

namespace VegaIot.External.Agv.Executor;

internal class GenAgvSchedulingTaskRequestBuilder
{
    /// <summary>
    /// 构建海康任务发送模型
    /// </summary>
    /// <param name="userCallCodePath">路径集合</param>
    /// <param name="joId">任务</param>
    /// <param name="podLotNum">批次本托盘数量</param>
    /// <param name="podStatus">托盘状态：0-空托盘 1-正常物料 2-异常物料</param>
    /// <param name="materialLot">物料编码</param>
    /// <param name="taskTyp">任务类型</param>
    /// <param name="SiloCode">托盘编号</param>
    /// <returns></returns>
    public static GenAgvSchedulingTaskRequest Build(object[] userCallCodePath, long jobId, int podLotNum, string podStatus, string materialLot, string taskTyp, string SiloCode)
    {
        var taskCode = $"VegaTask_{jobId}";
        var materialData = new MaterialData
        {
            realTaskCode = "",
            carryPro = "",
            materialGroupCode = "",
            materialLot = materialLot,
            materialCode = "",
            carrierTyp = "P4",
            sysLotNum = "",
            podLotNum = podLotNum.ToString(),
            lotStatus = "",
            includeScrap = "",
            podStatus = podStatus//托盘状态
        };

        return new GenAgvSchedulingTaskRequest
        {
            reqCode = taskCode,
            reqTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
            clientCode = "",
            tokenCode = "",
            taskTyp = taskTyp,
            ctnrTyp = "",
            ctnrCode = "",
            ctnrNum = "",
            taskMode = "",
            wbCode = "",
            userCallCodePath = userCallCodePath,
            podCode = SiloCode,
            podDir = "",
            podTyp = "P4",
            materialLot = materialData.materialLot,
            materialType = "",
            priority = "",
            taskCode = taskCode,
            agvCode = "",
            groupId = "",
            agvTyp = "",
            positionSelStrategy = "",
            data = JsonSerializer.Serialize(materialData)
        };
    }
}
