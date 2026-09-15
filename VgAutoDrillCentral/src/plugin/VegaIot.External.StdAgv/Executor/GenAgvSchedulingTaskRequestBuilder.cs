using VegaIot.External.AgvEntity.STD;

namespace VegaIot.External.Agv.Executor;

internal class GenAgvSchedulingTaskRequestBuilder
{
    /// <summary>
    /// 构建斯坦德任务发送模型
    /// </summary>
    /// <param name="userCallCodePath">路径集合</param>
    /// <param name="joId">任务</param>
    /// <param name="podLotNum">批次本托盘数量</param>
    /// <param name="podStatus">托盘状态：0-空托盘 1-正常物料 2-异常物料</param>
    /// <param name="materialLot">物料编码</param>
    /// <param name="taskTyp">任务类型</param>
    /// <param name="SiloCode">托盘编号</param>
    /// <returns></returns>
    public static STDGenAgvSchedulingTaskRequestEntity Build(String[] userCallCodePath, long jobId, int? podLotNum, string? podStatus, string? materialLot, string taskTyp, string? SiloCode, string? pnlStatus = "")
    {
        var taskCode = $"VegaTask_{jobId}";
        return new STDGenAgvSchedulingTaskRequestEntity
        {
            reqCode = taskCode,
            reqTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            clientCode = "VEGA01",
            tokenCode = "",
            taskTyp = taskTyp,
            ctnrTyp = "",
            ctnrCode = "",
            wbCode = "",
            userCallCodePath = userCallCodePath,
            podCode = SiloCode,
            podDir = "",
            taskCode = taskCode,
            pnlStatus = pnlStatus,
            data = new GenAgvSchedulingTaskData()
            {
                materialLot = materialLot,
            }
        };
    }
    /// <summary>
    /// 构建斯坦德任务发送模型
    /// </summary>
    /// <param name="userCallCodePath">路径集合</param>
    /// <param name="joId">任务</param>
    /// <param name="podLotNum">批次本托盘数量</param>
    /// <param name="podStatus">托盘状态：0-空托盘 1-正常物料 2-异常物料</param>
    /// <param name="materialLot">物料编码</param>
    /// <param name="taskTyp">任务类型</param>
    /// <param name="SiloCode">托盘编号</param>
    /// <returns></returns>
    public static STDGenAgvSchedulingTaskRequestEntity Build(String[] userCallCodePath, String? jobName, int? podLotNum, string? podStatus, string? materialLot, string taskTyp, string? SiloCode)
    {
        return new STDGenAgvSchedulingTaskRequestEntity
        {
            reqCode = jobName,
            reqTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            clientCode = "VEGA01",
            tokenCode = "",
            taskTyp = taskTyp,
            ctnrTyp = "",
            ctnrCode = "",
            wbCode = "",
            userCallCodePath = userCallCodePath,
            podCode = SiloCode,
            podDir = "",
            taskCode = jobName,
            data = new GenAgvSchedulingTaskData()
            {
                materialLot = materialLot,
            }
        };
    }
}
