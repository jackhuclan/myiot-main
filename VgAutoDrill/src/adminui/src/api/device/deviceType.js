import request  from "@/utils/request";
 
// 查询产品列表
export function listDeviceType(data) {
  return request({
    url: "/v1/DeviceType/GetList",
    method: "post",
    data,
  });
}

// 查询产品详细
export function getDeviceType(DeviceTypeId) {
  return request({
    url: "/v1/DeviceType/" + DeviceTypeId,
    method: "get",
  });
}

// 新增产品
export function addDeviceType(data) {
  return request({
    url: "/v1/DeviceType",
    method: "post",
    data,
  });
}

// 修改产品
export function updateDeviceType(data) {
  return request({
    url: "/v1/DeviceType",
    method: "put",
    data,
  });
}

// 删除产品
export function delDeviceType(deviceTypeId) {
  return request({
    url: "/v1/DeviceType/" + deviceTypeId,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/DeviceType",
    method: "delete",
    data,
  });
}
