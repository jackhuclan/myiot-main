# 下行Topic
- 读取设备属性: /{productId}/{deviceId}/properties/read
- 修改设备属性: /{productId}/{deviceId}/properties/write
- 调用设备功能: /{productId}/{deviceId}/service/{serviceName}/invoke

# 上行Topic
- 读取属性回复: /{productId}/{deviceId}/properties/read/reply
- 修改属性回复: /{productId}/{deviceId}/properties/write/reply
- 调用设备功能: /{productId}/{deviceId}/service/{serviceName}/invoke/reply
- 上报设备事件: /{productId}/{deviceId}/event/{eventId}
- 上报设备属性: /{productId}/{deviceId}/properties/report
- 上报设备物模型: /{productId}/{deviceId}/metadata/report
