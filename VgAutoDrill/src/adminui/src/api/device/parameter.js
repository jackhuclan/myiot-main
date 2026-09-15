import request  from "@/utils/request";
  
// 获取设备参数列表
export function listDeviceParameter(data) {
  return request({
    url: "/v1/DeviceParameter/GetList",
    method: "post",
    data,
  });
}

// 查询设备参数详细
export function getDeviceParameter(DeviceParameterId) {
  return request({
    url: "/v1/DeviceParameter/" + DeviceParameterId,
    method: "get",
  });
}

// 新增设备参数
export function addDeviceParameter(data) {
  return request({
    url: "/v1/DeviceParameter",
    method: "post",
    data,
  });
}

// 修改设备参数
export function updateDeviceParameter(data) {
  return request({
    url: "/v1/DeviceParameter",
    method: "put",
    data,
  });
}

// 删除设备参数
export function delDeviceParameter(deviceParameterId) {
  return request({
    url: "/v1/DeviceParameter/" + deviceParameterId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/DeviceParameter",
    method: "delete",
    data,
  });
}
// 获取设备树形列表
export function treeselect() {
  return request({
    url: "/v1/DeviceType/GetTreeSelect",
    method: "get",
  });
}
