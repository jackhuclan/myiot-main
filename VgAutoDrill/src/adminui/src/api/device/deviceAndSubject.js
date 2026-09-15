import request from "@/utils/request";
// 获取设备点检项目列表
export function listDeviceAndSubject(data) {
  return request({
    url: "/v1/DeviceAndSubject/GetList",
    method: "post",
    data,
  });
}

// 获取设备点检项目详情
export function getDeviceAndSubject(DeviceAndSubjectId) {
  return request({
    url: "/v1/DeviceAndSubject/" + DeviceAndSubjectId,
    method: "get",
  });
}

// 新增设备点检项目
export function addDeviceAndSubject(data) {
  return request({
    url: "/v1/DeviceAndSubject",
    method: "post",
    data,
  });
}

// 修改设备点检项目
export function updateDeviceAndSubject(data) {
  return request({
    url: "/v1/DeviceAndSubject",
    method: "put",
    data,
  });
}

// 删除设备点检项目
export function delDeviceAndSubject(DeviceAndSubjectTypeId) {
  return request({
    url: "/v1/DeviceAndSubject/" + DeviceAndSubjectTypeId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/DeviceAndSubject",
    method: "delete",
    data,
  });
}
