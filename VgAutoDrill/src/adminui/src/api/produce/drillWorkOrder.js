import request from "@/utils/request";

// 查询钻孔工单列表
export function listDrillWorkOrder(data) {
  return request({
    url: "/v1/DrillWorkOrder/GetList",
    method: "post",
    data,
  });
}

// 查询钻孔工单详细
export function getDrillWorkOrder(id) {
  return request({
    url: "/v1/DrillWorkOrder/" + id,
    method: "get",
  });
}

// 新增钻孔工单
export function addDrillWorkOrder(data) {
  return request({
    url: "/v1/DrillWorkOrder",
    method: "post",
    data,
    timeout: 1000000,
  });
}

// 修改钻孔工单
export function updateDrillWorkOrder(data) {
  return request({
    url: "/v1/DrillWorkOrder",
    method: "put",
    data,
  });
}

// 删除钻孔工单
export function delDrillWorkOrder(id) {
  return request({
    url: "/v1/DrillWorkOrder/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/DrillWorkOrder",
    method: "delete",
    data,
  });
}
// 钻孔工单分配机台
export function drillTaskAllocate(data) {
  return request({
    url: "/v1/DrillWorkOrder/Allocate",
    method: "put",
    data,
  });
}
// 提交钻孔工单
export function drillWorkOrderCommit(data) {
  return request({
    url: "/v1/DrillWorkOrder/Commit",
    method: "put",
    data,
  });
}
// 查询钻孔任务列表
export function listDrill(data) {
  return request({
    url: "/v1/Task/GetDrillList",
    method: "post",
    data,
    // 超时限制15s
    timeout: 15 * 1000,
  });
}

// 重置
export function drillTaskReset(data) {
  return request({
    url: "/v1/Task/Reset",
    method: "put",
    data,
  });
}

// 拖拽修改生产任务
export function drillTaskEdit(data) {
  return request({
    url: "/v1/Task",
    method: "put",
    data,
  });
}

// 查询生产记录详细
export function getDrillTask(id) {
  return request({
    url: "/v1/Task/" + id,
    method: "get",
  });
}

// 获取可用机台
export function getFitWorkStationList(data) {
  return request({
    url: "/v1/Task/GetFitWorkStationList",
    method: "post",
    data,
  });
}

// 重新计算库存
export function rfreshStockData() {
  return request({
    url: "/v1/DrillWorkOrder/RefreshStockData",
    method: "get",
    timeout: 1000000,
  });
}

// 获取库存流转信息
export function getMaterialData(data) {
  return request({
    url: "/v1/DrillWorkOrder/GetMaterialData",
    method: "post",
    data,
  });
}

// 再次生成任务
export function bulkAddDrillTask(data) {
  return request({
    url: "/v1/DrillWorkOrder/BulkAddDrillTask",
    method: "post",
    data,
    timeout: 1000000,
  });
}

// 批量重算生产任务 /v1/Task/BatchRationalizeTask
export function batchRationalizeTask(data) {
  return request({
    url: "/v1/Task/BatchRationalizeTask",
    method: "put",
    data,
    timeout: 1000000,
  });
}

// 拖拽修改生产任务
export function moveTask(data) {
  return request({
    url: "/v1/Task/MoveTask",
    method: "post",
    data,
    timeout: 1000000,
  });
}
