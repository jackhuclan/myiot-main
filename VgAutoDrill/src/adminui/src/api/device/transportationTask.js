import request from "@/utils/request";

// 查询料仓任务信息列表
export function listTransportationTask(data) {
  return request({
    url: "/v1/TransportationTask/GetList",
    method: "Post",
    data,
    // 一小时
    timeout: 60 * 60 * 1000,
  });
}

// 查询料仓任务信息详细
export function getTransportationTask(CutterId) {
  return request({
    url: "/v1/TransportationTask/" + CutterId,
    method: "get",
  });
}
// 查询料仓任务信息日志
export function getTransferJobLog(CutterId) {
  return request({
    url: "/v1/TransportationTask/GetTransferJobLog/" + CutterId,
    method: "get",
  });
}
// 新增料仓任务信息
export function addTransportationTask(data) {
  return request({
    url: "/v1/TransportationTask",
    method: "post",
    data,
  });
}

// 修改料仓任务信息
export function updateTransportationTask(data) {
  return request({
    url: "/v1/TransportationTask",
    method: "put",
    data,
  });
}

// 取消料仓任务/v1/TransportationTask/CancelSingle/1
export function cancelSingle(id) {
  return request({
    url: "/v1/TransportationTask/CancelSingle/" + id,
    method: "put",
  });
}

// 批量取消料仓任务

export function bulkCancel(data) {
  return request({
    url: "/v1/TransportationTask/BulkCancel",
    method: "post",
    data,
  });
}
// 删除料仓任务信息
export function delTransportationTask(id) {
  return request({
    url: "/v1/TransportationTask/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/TransportationTask",
    method: "delete",
    data,
  });
}
// 查询料仓任务历史信息列表
export function listHistory(data) {
  return request({
    url: "/v1/TransportationTask/GetHistoryList",
    method: "Post",
    data,
    // 一小时
    timeout: 60 * 60 * 1000,
  });
}

// 获取料仓任务历史详细信息
export function getHistory(historyId) {
  return request({
    url: "/v1/TransportationTask/GetHistory/" + historyId,
    method: "get",
  });
}
