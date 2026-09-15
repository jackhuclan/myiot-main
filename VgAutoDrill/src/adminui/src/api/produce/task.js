import request from "@/utils/request";

// 查询生产任务列表
export function listTask(data) {
  return request({
    url: "/v1/Task/GetList",
    method: "post",
    data,
  });
}

// 根据ItemTypeId查询生产任务表
export function listTaskByItemTypeId(data) {
  return request({
    url: "/v1/Task/GetEquipmentList",
    method: "post",
    data,
  });
}

// 查询生产任务详细
export function getTask(notificationRecordId) {
  return request({
    url: "/v1/Task/" + notificationRecordId,
    method: "get",
  });
}

// 新增生产任务
export function addTask(data) {
  return request({
    url: "/v1/Task",
    method: "post",
    data,
  });
}

// 修改生产任务
export function updateTask(data) {
  return request({
    url: "/v1/Task",
    method: "put",
    data,
  });
}

// 删除生产任务
export function delTask(id) {
  return request({
    url: "/v1/Task/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Task",
    method: "delete",
    data,
  });
}

// 查询工序信息
export function listAllProcess(data) {
  return request({
    url: "/v1/Task/GetRouteAndProcessList",
    method: "post",
    data,
  });
}

// 获取树形结构
export function treeSelect() {
  return request({
    url: "/v1/Task/GetTreeSelect/",
    method: "get",
  });
}

// 获取甘特图数据
export function getGanttTaskList(data) {
  return request({
    url: "/v1/Task/GetGanttTaskList",
    method: "post",
    data,
  });
}

// 保存甘特图   /v1/Task/PutGantt

export function updateGantt(data) {
  return request({
    url: "/v1/Task/PutGantt",
    method: "put",
    data,
  });
}

// 生成检验记录
export function addCheckRecords(data) {
  return request({
    url: "/v1/CheckRecords",
    method: "post",
    data,
  });
}

// 提交按钮
export function commitTask(data) {
  return request({
    url: "/v1/Task/Commit",
    method: "put",
    data,
  });
}
// 撤销审批
export function revokeCommit(data) {
  return request({
    url: "/v1/Task/RevokeCommit",
    data,
    method: "put",
  });
}

// 修改任务状态
export function updateByOutSide(data) {
  return request({
    url: "/v1/Task/UpdateByOutSide",
    data,
    method: "put",
  });
}

// 批量修改任务

export function updateListByOutSide(data) {
  return request({
    url: "/v1/Task/UpdateListByOutSide",
    data,
    method: "put",
  });
}

// 判断多个任务是否属于同一工序，同一路线
export function verifyTaskBelongOneRouteAndProcess(data) {
  return request({
    url: "/v1/Task/VerifyTaskBelongOneRouteAndProcess",
    data,
    method: "post",
  });
}

// 转发任务到其他工作站
export function transferTask(data) {
  return request({
    url: "/v1/Task/TransferTask",
    data,
    method: "put",
  });
}

// 查询任务历史列表
export function listTaskHisList(data) {
  return request({
    url: "/v1/Task/GetTaskHisList",
    method: "post",
    data,
  });
}
