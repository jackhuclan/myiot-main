import request  from "@/utils/request";
 
// 获取设备网关列表
export function listDeviceGateway(data) {
  return request({
    url: "/v1/DeviceGateway/GetDeviceGateway",
    method: "post",
    data,
  });
}

// 获取设备网关详情
export function getDeviceGateway(id) {
  return request({
    url: "/v1/DeviceGateway/" + id,
    method: "get",
  });
}

// 新增设备网关
export function addDeviceGateway(data) {
  return request({
    url: "/v1/DeviceGateway/GetDeviceGatewayAddOrUpdate",
    method: "post",
    data,
  });
}

// 修改设备网关
export function updateDeviceGateway(data) {
  return request({
    url: "/v1/DeviceGateway/UpdateDeviceGateway",
    method: "post",
    data,
  });
}

// 删除设备网关
export function delDeviceGateway(id) {
  return request({
    url: "/v1/DeviceGateway/" + id,
    method: "delete",
  });
}

// 获取网关版本
export function getDeviceGatewayVerson(data) {
  return request({
    url: "/v1/DeviceGateway/GetVerson",
    method: "post",
    data,
    // 10秒
    timeout: 10000,
  });
}

// 网关安装接口

export function installPackages(data) {
  return request({
    url: "/v1/DeviceGateway/InstallPackages",
    method: "post",
    data,
    // 两分钟
    timeout: 120000,
  });
}
