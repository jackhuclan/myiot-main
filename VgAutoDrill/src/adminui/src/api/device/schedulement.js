import request from "@/utils/request";

// 查询调度记录列表
export function listSchedulement(data) {
  return request({
    url: "/v1/Schedule/GetList",
    method: "post",
    data,
    // 一小时
    timeout: 60 * 60 * 1000,
  });
}

// 查询调度详细
export function getSchedulement(schedulementId) {
  return request({
    url: "/v1/Schedule/" + schedulementId,
    method: "get",
  });
}

// 新增调度记录
export function addSchedulement(data) {
  return request({
    url: "/v1/Schedule",
    method: "post",
    data,
  });
}

// 修改调度记录
export function updateSchedulement(data) {
  return request({
    url: "/v1/Schedule",
    method: "put",
    data,
  });
}

// 删除调度记录
export function delSchedulement(deviceId) {
  return request({
    url: "/v1/Schedule/" + deviceId,
    method: "delete",
  });
}

// 取消按钮
export function updateCentralTask(data) {
  return request({
    url: "/v1/Schedule/UpdateCentralTask",
    method: "put",
    data,
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Schedule",
    method: "delete",
    data,
  });
}

// 重新发起(调用另一个服务器接口已跨域)
export function relaunch(data) {
  return request({
    url: "/v1/Schedule/UpdateRepublish",
    method: "put",
    data,
  });
}

// 批量取消

export function bulkCanceled(data) {
  return request({
    url: "/v1/Schedule/BulkCanceled",
    method: "put",
    data,
  });
}

// 获取调度记录明细

export function getDetailData(schedulementId) {
  return request({
    url: "/v1/Schedule/GetScheduleLogs/" + schedulementId,
    method: "get",
  });
}
//同步设备板料明细
export function synchronousPanelData(params) {
  return request({
    url: "/v1/DrillPanelDetail/SynchronousPanelData",
    method: "post",
    params,
  });
}
//下发板料数据到设备
export function allotsPanelData(data) {
  return request({
    url: "/v1/DrillPanelDetail/AllotsPanelData",
    method: "post",
    data,
  });
}

// 异常处理移动板料
export function movePanel(data) {
  return request({
    url: "/v1/DrillPanelDetail/MovePanel",
    method: "post",
    data,
  });
}

// 查询料仓载料信息
export function getSiloDetailsByLocation(params) {
  return request({
    url: "/v1/Silo/GetSiloDetailsByLocation",
    method: "post",
    params,
  });
}

//设置紧急
export function setUrgent(schedulementId) {
  return request({
    url: "/v1/Schedule/SetScheduleUrgent?id=" + schedulementId,
    method: "get",
  });
}

// 导出
export function downLoadList(data) {
  return request({
    url: "/v1/Schedule/DownLoadList",
    method: "post",
    data,
    responseType: "blob",
  });
}
