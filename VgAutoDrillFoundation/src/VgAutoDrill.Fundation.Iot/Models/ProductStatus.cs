using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 成品状态
/// </summary>
public enum ProductStatus
{
    /// <summary>
    /// 空操作
    /// </summary>
    [Description("空操作")]
    Noop = -1,

    /// <summary>
    /// 空负载
    /// </summary>
    [Description("空负载")]
    EmptyPayload = 0,

    /// <summary>
    /// 空料盒
    /// </summary>
    [Description("空料盒")]
    EmptySiloBox = 1,

    /// <summary>
    /// 进pin之前的原料
    /// </summary>
    [Description("进pin之前的原料")]
    Raw = 10000,

    PRE_PIN_TRANSFER_AGV_OUTPUT_1 = 10100,
    PRE_PIN_TRANSFER_AGV_OUTPUT_2 = 10200,
    PRE_PIN_TRANSFER_AGV_OUTPUT_3 = 10300,
    PRE_PIN_TRANSFER_AGV_OUTPUT_4 = 10400,
    PRE_PIN_TRANSFER_AGV_OUTPUT_5 = 10500,
    PRE_PIN_TRANSFER_AGV_OUTPUT_6 = 10600,
    PRE_PIN_TRANSFER_AGV_OUTPUT_7 = 10700,
    PRE_PIN_TRANSFER_AGV_OUTPUT_8 = 10800,
    PRE_PIN_TRANSFER_AGV_OUTPUT_9 = 10900,
    PRE_PIN_TRANSFER_AGV_OUTPUT_10 = 11000,

    /// <summary>
    /// 完成pin包装
    /// </summary>
    [Description("完成pin包装")]
    Finished_PIN = 20000,

    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_1 = 20100,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_2 = 20200,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_3 = 20300,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_4 = 20400,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_5 = 20500,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_6 = 20600,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_7 = 20700,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_8 = 20800,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_9 = 20900,
    PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_10 = 21000,

    /// <summary>
    /// 从生料仓出货
    /// </summary>
    [Description("从生料仓出货")]
    Finished_PRE_BUFFER = 30000,

    PRE_DRILL_TRANSFER_AGV_OUTPUT_1 = 30100,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_2 = 30200,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_3 = 30300,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_4 = 30400,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_5 = 30500,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_6 = 30600,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_7 = 30700,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_8 = 30800,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_9 = 30900,
    PRE_DRILL_TRANSFER_AGV_OUTPUT_10 = 31000,

    /// <summary>
    /// 等待钻孔
    /// </summary>
    [Description("等待钻孔")]
    WaitingForDrill = 38000,

    /// <summary>
    /// 正在钻孔
    /// </summary>
    [Description("正在钻孔")]
    Drilling = 39000,

    /// <summary>
    /// 钻机完成加工
    /// </summary>
    [Description("钻机完成加工")]
    Finished_DRILL = 40000,

    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1 = 40100,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_2 = 40200,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_3 = 40300,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_4 = 40400,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_5 = 40500,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_6 = 40600,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_7 = 40700,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_8 = 40800,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_9 = 40900,
    PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_10 = 41000,

    /// <summary>
    /// 从熟料仓出货
    /// </summary>
    [Description("从熟料仓出货")]
    Finished_POST_BUFFER = 50000,

    PRE_UNPIN_TRANSFER_AGV_OUTPUT_1 = 50100,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_2 = 50200,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_3 = 50300,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_4 = 50400,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_5 = 50500,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_6 = 50600,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_7 = 50700,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_8 = 50800,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_9 = 50900,
    PRE_UNPIN_TRANSFER_AGV_OUTPUT_10 = 51000,

    /// <summary>
    /// 从拆pin进出货
    /// </summary>
    [Description("从拆pin进出货")]
    Finished_UNPIN = 60000,
}
