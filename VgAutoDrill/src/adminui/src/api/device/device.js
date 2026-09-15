import request from "@/utils/request";

// 获取设备列表
export function listDevice(data) {
  return request({
    url: "/v1/Device/GetList",
    method: "post",
    data,
  });
}

// 获取设备详情
export function getDevice(DeviceId) {
  return request({
    url: "/v1/Device/" + DeviceId,
    method: "get",
  });
}

// 新增设备
export function addDevice(data) {
  return request({
    url: "/v1/Device",
    method: "post",
    data,
  });
}

// 修改设备
export function updateDevice(data) {
  return request({
    url: "/v1/Device",
    method: "put",
    data,
  });
}

// 删除设备
export function delDevice(deviceTypeId) {
  return request({
    url: "/v1/Device/" + deviceTypeId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Device",
    method: "delete",
    data,
  });
}

// 禁用

export function disableDevice(params) {
  return request({
    url: "/v1/Device/Disable",
    method: "post",
    params,
  });
}
// 启用
export function enableDevice(params) {
  return request({
    url: "/v1/Device/Enable",
    method: "post",
    params,
  });
}
// 获取设备树形列表
export function treeselect() {
  return request({
    url: "/v1/DeviceType/GetTreeSelect",
    method: "get",
  });
}

// 获取设备状态日志v1/Device/GetDeviceStatusList
export function listDeviceStatusList(data) {
  return request({
    url: "/v1/Device/GetDeviceStatusList",
    method: "post",
    data,
  });
}

// 获取设备服务日志v1/Device/GetDeviceServiceList
export function listDeviceServiceList(data) {
  return request({
    url: "/v1/Device/GetDeviceServiceList",
    method: "post",
    data,
  });
}

// 获取设备属性日志v1/Device/GetDevicePropertyList
export function listDevicePropertyList(data) {
  return request({
    url: "/v1/Device/GetDevicePropertyList",
    method: "post",
    data,
  });
}

// 获取设备事件日志v1/Device/GetDeviceEventList
export function listDeviceEventList(data) {
  return request({
    url: "/v1/Device/GetDeviceEventList",
    method: "post",
    data,
  });
}

// 获取调度配置设备列表
export function getDeviceAndRouteList(data) {
  return request({
    url: "/v1/DeviceAndRoute/GetDeviceAndRouteList",
    method: "post",
    data,
  });
}

// 根据设备编码获取板料列表
export function getPanelList(data) {
  return request({
    url: "/v1/DrillPanelDetail/GetPanelList",
    method: "post",
    params: data,
  });
}
// 添加/修改钻机料仓记录
export function batchAddOrUpdateData(data) {
  return request({
    url: "/v1/DrillPanelDetail/BatchAddOrUpdateData",
    method: "post",
    data,
  });
}

// 绑定板料
export function updateDrillPanelDetail(data) {
  return request({
    url: "/v1/DrillPanelDetail",
    method: "put",
    data,
  });
}
// 清空钻机料仓记录
export function clearData(data) {
  return request({
    url: "/v1/DrillPanelDetail/ClearData",
    method: "post",
    data,
  });
}

// 加载数据
export function loadPanelDetailData(data) {
  return request({
    url: "/v1/DrillPanelDetail/LoadPanelDetailData",
    method: "post",
    data,
  });
}
// 下发设备指令
export function allotsDeviceCommand(data) {
  return request({
    url: "/v1/Device/AllotsDeviceCommand",
    method: "post",
    data,
  });
}
