# 1.4.0.1
# What's Changed
- [x] 更新VgAutoDrill.Fundation.CNC84里面类的错误命名空间
---
# 1.4.0.3
--AGV基类 增加是否可分配任务的函数

# 1.4.0.4
CentralWebOptions增加ScheduleStart等属性

#1.4.0.6
Note：重要更新，所有发布的客户端，必须同步更新
1.ProductStatus枚举值，升级为xxx00, 五位数
2.Device类增加mqtt下发方法CancelSchedule，钻机设备需要重载此方法

#1.4.0.7
回滚ProductStatus枚举值

#1.4.0.8
设备基类增加 完成调度任务的响应方法

#1.9.9.1
支持钻机任务完成的接口
支持AGV上报调度明细的接口

#1.9.9.4
获取物料代码，用于AGV加载物料

#1.9.27.1
DeviceProvider 自动检查 配置中的descriptor.ProductId是否正确.

#1.9.28.2
CNC95 update

#1.10.7.1
CNC接口,新增方法SetSpindleMask(int maskNum)
删除基类中的钻孔状态定义PanelDrillState

#1.10.13.1
设备描述中增加代理软件的端口

#1.10.19.1
CNC95 ADD new method
#1.10.19.2
data export 添加各个方法是否上报clickhouse 数据库服务器的开关。

#1.12.26.1-pro
中控增加新功能：
1. 自动分区换刀，获取换刀信息；
2. 设备代理自动更新功能；

#2.1.26.1-preview
设备初始化Panel信息时，为了清楚、方便的知道方法的使用场景，加了以上后缀

#2.2.1.2-preview
mqtt客户端，默认不订阅mqtt的日志，仅在配置中显式声明后，才会写mqtt的日志。
 "MqttServerOptions": {
    "Host": "192.168.102.115",
    "Port": 1883,
    "EnableLogMqttClient": true,  //客户端 增加了此配置项
    "KeepAlivePeriod": 5
  }
