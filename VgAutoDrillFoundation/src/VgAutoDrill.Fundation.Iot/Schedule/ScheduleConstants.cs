namespace VgAutoDrill.Fundation.Iot.Schedule;

public static class ScheduleConstants
{
    public const string PARAMS_TASK_ID = "TaskId";
    public const string PARAMS_TASK_ITEM_CODE = "TaskItemCode";
    public const string PARAMS_TASK_ITEM_COUNT = "TaskItemCount";
    public const string PARAMS_TASK_ROUTE_CODE = "TaskRouteCode";
    /// <summary>
    /// 调度任务的下发时间
    /// </summary>
    public const string PARAMS_TASK_SENDED_TIME = "PARAMS_TASK_SENDED_TIME";

    /// <summary>
    /// 经过尾料计算后，AGV将向钻机下发的生料的板料数量
    /// </summary>
    public const string PARAMS_PLAN_RAW_COUNT = "PlanRawCount";

    public const string PARAMS_TASK_EVENT_ID = "TaskEventId";
    public const string PARAMS_TASK_EVENT_TRACE_ID = "TaskEventTraceId";
    public const string PARAMS_TASK_EVENT_NAME = "TaskEventName";
    public const string PARAMS_TASK_ORDER = "TaskOrder";
    public const string PARAMS_TASK_STATUS = "TaskStatus";
    public const string PARAMS_TASK_ACTION = "TaskAction";
    public const string PARAMS_TASK_CREATE_TIME = "TaskCreateTime";

    public const string PARAMS_TASK_ACTION_CANCEL = "CANCEL";
    public const string PARAMS_TASK_ACTION_WORK = "WORK";
    public const string PARAMS_TASK_ACTION_CHARGE = "CHARGE";
}
