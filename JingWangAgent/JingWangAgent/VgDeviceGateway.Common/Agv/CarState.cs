namespace VgDeviceGateway.Devices.Common.Agv;

public class CarState
{
    public int ErrorCode { get; set; }//0:正常 ⾮0:异常码
    public string? Message { get; set; }//提⽰信息.
    public List<CarModel>? Result { get; set; }//信息主体
}

public class CarModel
{
    public string? CarType { get; set; } //车辆类型
    public string? IP { get; set; }// 车辆 IP 地址
    public string? Name { get; set; }//车名
    public string? CurrentStation { get; set; }//当前站点
    public string? PreviousStation { get; set; }//上一个站点
    public string? TargetStation { get; set; }//通行点
    public string? EndStation { get; set; }//路径终点(1.9.11)
    public bool? IsArrivalAtTerminal { get; set; }//是否到达终点
    public float? Battery { get; set; }//电量百分比
    public bool? IsLowBattery { get; set; }//是否低电量
    public bool? CanDispatch { get; set; }//是否可调度
    public string TaskId { get; set; }//任务 id
    public string? Status { get; set; }
    /*  小车状态
        Unknown:未知,小车未连接系统
        Connected:连接上.没有路径时得状态.
        Running:运行中. 有路线小车在行驶过中
        ArrivalAtTerminal. 小车到达目标点的状态
        Charging:充电中
        Waiting:等待中, 多车情况下.一个车等待另一个车
    */
    public string? CarSelfStatus { get; set; }
    public string? Warnings { get; set; }
    /*  异常信息.信息有:
        前障碍物报警,中障碍物报警,后障碍物报警,
        叉货光电(左叉), 叉货光电(右叉),货物光电,
        不可用,低电量,机械防撞被碰撞,红灯报警, 自动充电异常
     */
    public string? Version { get; set; }//小车版本
    public string? OccupiedRoute { get; set; }//管制占用的路径
    public string? Route { get; set; }//路径信息（A->B→C） A B C 为站点名称
    public string? NextStation { get; set; }//下一个站点，没有站点时为 null
    public decimal? Angle { get; set; }//当前路径角度，没有路径时为 0
    public decimal? X { get; set; }//小车当前 x 坐标
    public decimal? Y { get; set; }//小车当前 y 坐标
    public decimal? Theta { get; set; }//小车当前角度（弧度）[- 3.14, 3.14]
    public List<int>? Alarms { get; set; }//报警错误码集合
    public decimal? EstimatedArrivalTime { get; set; }//剩余运行时间
    public string? IseeConfigBattery { get; set; }//Isee 配置默认充电电量范围,小于最小值充电,充到最大值
}

public class TaskRequest
{
    public string FromStation { get; set; }
    public string ToStation { get; set; }
    public string Type { get; set; }
    public int Priority { get; set; }
    public string Rest_Station { get; set; }
    public string ThirdPartyOrder { get; set; }
    public string ExpectCar { get; set; }

    public string ActParam { get; set; }
}

public class TaskResponse
{
    public int ErrorCode { get; set; }//0:正常 ⾮0:异常码
    public string? Message { get; set; }//提⽰信息.
    public TaskModel? Result { get; set; }//信息主体
}

public class ChargeRequest
{
    public string CarName { get; set; }
}

public class ChargeResponse
{
    public bool Result { get; set; }
    public int ErrorCode { get; set; }
    public string Message { get; set; }
}

public class TaskModel
{
    public int Id { get; set; }//INT  任务id.
    public string FromStation { get; set; }//string任务起点
    public string ToStation { get; set; }//string任务终点
    public string OrderId { get; set; }//string订单任务id
    public int OrderSort { get; set; }//string订单任务顺序
    public string Type { get; set; }//string任务类型
    public string Status { get; set; }//string任务状态.
    public string NonExecution { get; set; }//:未执⾏
    public string Received { get; set; }//:取货中
    public string Processing { get; set; }//:放货中
    public string Complished { get; set; }//:完成
    public string Cancelled { get; set; }//:取消
    public string Paused { get; set; }//:暂停
    public string CallFrom { get; set; }//string呼叫源
    public string CarName { get; set; }//string接取⼩⻋名称
    public int Priority { get; set; }//int任务优先级.0-1000最⾼
    public string CarType { get; set; }//string指定⻋型
    public string ExpectCar { get; set; }//string期望⻋名
    public string ThirdPartyOrder { get; set; }//string第三⽅订单
    public string CargoAttribute { get; set; }//string (json)"

    public string CreateDate { get; set; }//datetime创建时间
    public string ComplishedDate { get; set; }//datetime完成时间
    public string RecievedDate { get; set; }//datetime接到任务时间
    public string ProcessingDate { get; set; }//datetime到达起点时间
    public string Extend1 { get; set; }//1-9string扩展字段
    public string Extend2 { get; set; }//1-9string扩展字段
    public string Extend3 { get; set; }//1-9string扩展字段
    public string Extend4 { get; set; }//1-9string扩展字段
    public string Extend5 { get; set; }//1-9string扩展字段
    public string Extend6 { get; set; }//1-9string扩展字段
    public string Extend7 { get; set; }//1-9string扩展字段
    public string Extend8 { get; set; }//1-9string扩展字段
    public string Extend9 { get; set; }//1-9string扩展字段
    public string Rest_Station { get; set; }//string休息站点
    public string TaskId { get; set; }//任务ID
}

public class TaskParams
{
    public string Height { get; set; }
}
