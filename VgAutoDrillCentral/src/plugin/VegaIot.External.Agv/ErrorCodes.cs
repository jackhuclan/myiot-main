namespace VegaIot.External.Agv;

public class ErrorCodes
{
    public const int SUCCESS = 0;
    public const int ERR_CODE = 1;
    public const string NOT_FIND_CENTRAL_TASK = "未找到中控下发的任务";
    public const string NOT_FIND_FORK_LOCATION = "未找到中转位位置";
    public const string NOT_FIND_FORK = "在线设备未查询到中转位";
    public const string NOT_FIND_DEVICE_LOCATION = "未查询到设备位置";
    public const string NOT_FIND_DEVICE = "未查询到在线设备信息";
    public const string ALREADY_SEND = "任务已下发完成，不能重复下发";
    public const string NOT_FIND_PIN_OR_UNPIN_POSITION = "未查询到设备位置";
    public const string TRANSFER_JOB_START_DEVICE_IS_NOT_EMPTY = "TransferJob开始设备不能为空";
    public const string TRANSFER_JOB_END_DEVICE_IS_NOT_EMPTY = "TransferJob结束设备不能为空";
    public const string HIK_METHOD_IS_NULL = "传入参数method不能为空";
    public const string MATERIAL_DATA_IS_NULL = "data数据不能为空";
}
