import request  from "@/utils/request";
 // 获取设备维护明细列表
export function listDeviceMaintainDetail(data) {
  return request({
    url: "/v1/DeviceMaintainDetail/GetList",
    method: "post",
    data,
  });
}

// 获取设备维护明细详情
export function getDeviceMaintainDetail(DeviceMaintainDetailId) {
  return request({
    url: "/v1/DeviceMaintainDetail/" + DeviceMaintainDetailId,
    method: "get",
  });
}

// 新增设备维护明细
export function addDeviceMaintainDetail(data) {
  return request({
    url: "/v1/DeviceMaintainDetail",
    method: "post",
    data,
  });
}

// 修改设备维护明细
export function updateDeviceMaintainDetail(data) {
  return request({
    url: "/v1/DeviceMaintainDetail",
    method: "put",
    data,
  });
}

// 删除设备维护明细
export function delDeviceMaintainDetail(DeviceMaintainDetailTypeId) {
  return request({
    url: "/v1/DeviceMaintainDetail/" + DeviceMaintainDetailTypeId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/DeviceMaintainDetail",
    method: "delete",
    data,
  });
}
