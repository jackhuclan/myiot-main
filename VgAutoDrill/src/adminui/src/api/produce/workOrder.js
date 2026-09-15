import request from "@/utils/request";

// 查询生产记录列表
export function listWorkOrder(data) {
  return request({
    url: "/v1/WorkOrder/GetList",
    method: "post",
    data,
    timeout: 10000,
  });
}

// 查询生产记录详细
export function getWorkOrder(id) {
  return request({
    url: "/v1/WorkOrder/" + id,
    method: "get",
  });
}

// 新增生产记录
export function addWorkOrder(data) {
  return request({
    url: "/v1/WorkOrder",
    method: "post",
    data,
  });
}

// 修改生产记录
export function updateWorkOrder(data) {
  return request({
    url: "/v1/WorkOrder",
    method: "put",
    data,
  });
}

// 修改生产记录
export function commitWorkOrder(data) {
  return request({
    url: "/v1/WorkOrder/Commit",
    method: "put",
    data,
  });
}

// 删除生产记录
export function delWorkOrder(id) {
  return request({
    url: "/v1/WorkOrder/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/WorkOrder",
    method: "delete",
    data,
  });
}
// 向工单内添加routeId、routeCode
export function updateWorkOrderRoute(data) {
  return request({
    url: "/v1/WorkOrder/UpdateRoute",
    method: "put",
    data,
  });
}

// 获取工单分布数据
export function getMOTaskList(data) {
  return request({
    url: "/v1/WorkOrder/GetMOTaskList",
    method: "post",
    data,
    // 超时限制15s
    timeout: 15 * 1000,
  });
}

// 提交任务
export function commitTask(data) {
  return request({
    url: "/v1/WorkOrder/CommitTask",
    method: "put",
    data,
  });
}

// 撤回任务
export function robackTask(data) {
  return request({
    url: "/v1/WorkOrder/RobackTask",
    method: "put",
    data,
  });
}

// 批量生成任务

export function bulkAddDrillTaskByMO(data) {
  return request({
    url: "/v1/DrillWorkOrder/BulkAddDrillTaskByMO",
    method: "post",
    data,
  });
}

// 生产工单标记颜色
export function remarkWorkOrderColor(data) {
  return request({
    url: "/v1/WorkOrder/MoColorRemark",
    method: "put",
    data,
  });
}

// 获取工单板材信息
export function getWorkOrderAndPanelList(data) {
  return request({
    url: "/v1/WorkOrderAndPanel/GetWorkOrderAndPanelList",
    method: "post",
    data,
  });
}

// 清除工单下的机台
export function clearWorkStation(workOrderCode) {
  return request({
    url: "/v1/WorkOrder/ClearWorkStation?workOrderCode=" + workOrderCode,
    method: "put",
  });
}

//SetMoveInTime
export function setMoveInTime(workOrderCode) {
  return request({
    url: "/v1/WorkOrder/SetMoveInTime?workOrderCode=" + workOrderCode,
    method: "put",
  });
}
//SetMoveOutTime
export function setMoveOutTime(workOrderCode) {
  return request({
    url: "/v1/WorkOrder/SetMoveOutTime?workOrderCode=" + workOrderCode,
    method: "put",
  });
}
//SetTrackInTime
export function setTrackInTime(workOrderCode) {
  return request({
    url: "/v1/WorkOrder/SetTrackInTime?workOrderCode=" + workOrderCode,
    method: "put",
  });
}
//SetTrackOutTime
export function setTrackOutTime(workOrderCode) {
  return request({
    url: "/v1/WorkOrder/SetTrackOutTime?workOrderCode=" + workOrderCode,
    method: "put",
  });
}

//校验料号数量
export function verifyWIPItemNum(data) {
  return request({
    url: "/v1/WorkOrder/VerifyWIPItemNum",
    method: "post",
    data,
  });
}
