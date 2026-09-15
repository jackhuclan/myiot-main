import request from "@/utils/request";
// 首页设备统计
export function getHomeDeviceData() {
  return request({
    url: "/v1/DeviceSchedule/GetHomeDeviceData",
    method: "get",
  });
}
// 获取设备列表
export function getDeviceList(data) {
  return request({
    url: "/v1/DeviceSchedule/GetDeviceList",
    method: "post",
    data,
  });
}
// 获取最近一周产量统计数据
export function getFeedBackStats() {
  return request({
    url: "/v1/BigScreen/GetFeedBackStats",
    method: "get",
  });
}

// 统计近一周agv设备调度数量
export function getSchedulementDeviceStats() {
  return request({
    url: "/v1/DeviceSchedule/GetScheduleDeviceStats",
    method: "get",
  });
}
// 统计近一周agv设备调度数量(完成/异常)
export function getSchedulementDeviceStatusStats() {
  return request({
    url: "/v1/DeviceSchedule/GetScheduleDeviceStatusStats",
    method: "get",
  });
}
// 查询生产记录列表
export function getWorkOrderList(data) {
  return request({
    url: "/v1/DeviceSchedule/GetWorkOrderList",
    method: "post",
    data,
  });
}
