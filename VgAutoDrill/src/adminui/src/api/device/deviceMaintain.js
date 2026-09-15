import request  from "@/utils/request";
 // 获取设备维护列表
export function listDeviceMaintain(data) {
  return request({
    url: "/v1/DeviceMaintain/GetList",
    method: "post",
    data,
  });
}

// 获取设备维护详情
export function getDeviceMaintain(DeviceMaintainId) {
  return request({
    url: "/v1/DeviceMaintain/" + DeviceMaintainId,
    method: "get",
  });
}

// 新增设备维护
export function addDeviceMaintain(data) {
  return request({
    url: "/v1/DeviceMaintain",
    method: "post",
    data,
  });
}

// 修改设备维护
export function updateDeviceMaintain(data) {
  return request({
    url: "/v1/DeviceMaintain",
    method: "put",
    data,
  });
}

// 删除设备维护
export function delDeviceMaintain(DeviceMaintainTypeId) {
  return request({
    url: "/v1/DeviceMaintain/" + DeviceMaintainTypeId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/DeviceMaintain",
    method: "delete",
    data,
  });
}
