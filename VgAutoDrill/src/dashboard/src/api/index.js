import request from "@/utils/request";
// 获取设备统计数据
export function getDeviceStats() {
  return request({
    url: "/v1/BigScreen/GetDeviceStats",
    method: "get",
  });
}
// 获取所有设备告警数据
export function getDeviceAlarmStats(query) {
  return request({
    url: "/v1/BigScreen/GetDeviceAlarmStats?deviceId="+query,
    method: "get",
  });
}
// 获取所有板料追踪数据
export function getPanelStats(query) {
  return request({
    url: "/v1/BigScreen/GetPanelStats?deviceId="+query,
    method: "get",
  });
}
// 获取调度日志数据
export function getDeviceServiceStatsList() {
  return request({
    url: "/v1/BigScreen/GetDeviceServiceStatsList",
    method: "get",
  });
}
// 获取所有工单进度数据
export function getWorkOrderStats() {
  return request({
    url: "/v1/BigScreen/GetWorkOrderStats",
    method: "get",
  });
}

// 获取最近一周产量统计数据
export function getFeedBackStats() {
  return request({
    url: "/v1/BigScreen/GetFeedBackStats",
    method: "get",
  });
}

// 获取生产量情况统计数据
export function getTaskStats() {
  return request({
    url: "/v1/BigScreen/GetTaskStats",
    method: "get",
  });
}


// 获取工单统计数据
export function getWorkOrderRateStats() {
  return request({
    url: "/v1/BigScreen/GetWorkOrderRateStats",
    method: "get",
  });
}


// 获取设备运行情况统计
export function getDeviceStatusStats(query) {
  return request({
    url: "/v1/BigScreen/GetDeviceStatusStats?deviceId="+query,
    method: "get",
  });
}


// 获取加工时间统计统计
export function getDeviceProcessingStats(query) {
  return request({
    url: "/v1/BigScreen/GetDeviceProcessingStats?deviceId="+query,
    method: "get",
  });
}



// 获取设备稼动率统计
export function getDeviceMovementStats(query) {
  return request({
    url: "/v1/BigScreen/GetDeviceMovementStats?deviceId="+query,
    method: "get",
  });
}


// 获取设备开机率统计
export function getDeviceUptimeStats(query) {
  return request({
    url: "/v1/BigScreen/GetDeviceUptimeStats?deviceId="+query,
    method: "get",
  });
}