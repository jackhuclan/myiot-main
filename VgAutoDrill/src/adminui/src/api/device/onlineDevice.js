import request  from "@/utils/request";
 
// 获取设备列表
export function listOnlineDevice(data) {
  return request({
    url: "/v1/Device/GetCentralOnlineDevice",
    method: "post",
    data,
  });
}

// 解除锁定
export function cleanLockerData(data) {
  return request({
    url: "/v1/Device/CleanLockerData",
    method: "post",
    data,
  });
}
// 重置信号
export function resetAGVSignal(data) {
  return request({
    url: "/v1/Device/ResetAgvCallLimit",
    method: "post",
    data,
  });
}

// 获取在线详细信息

export function getOnlineDeviceInfo(deviceId) {
  return request({
    url: "/v1/Device/GetOnlineDeviceInfo?deviceId=" + deviceId,
    method: "get",
  });
}
