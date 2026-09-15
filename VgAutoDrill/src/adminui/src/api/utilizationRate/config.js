import request from "@/utils/request";

//获取设备保养配置
export function getDeviceMaintenanceConfigs() {
  return request({
    url: "/v1/DeviceRecords/GetDeviceMaintenanceConfigs",
    method: "post",
  });
}

// 保存设备保养配置
export function saveDeviceMaintenanceConfigs(data) {
  return request({
    url: "/v1/DeviceRecords/SaveDeviceMaintenanceConfigs",
    method: "post",
    data,
  });
}
