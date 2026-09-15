import request  from "@/utils/request";
 
// 查询报工记录列表
export function listFeedback(data) {
  return request({
    url: "/v1/FeedBack/GetList",
    method: "Post",
    data,
  });
}

// 查询报工记录详细
export function getFeedback(id) {
  return request({
    url: "/v1/FeedBack/" + id,
    method: "get",
  });
}

// 新增报工记录
export function addFeedback(data) {
  return request({
    url: "/v1/FeedBack",
    method: "post",
    data,
  });
}

// 修改报工记录
export function updateFeedback(data) {
  return request({
    url: "/v1/FeedBack",
    method: "put",
    data,
  });
}

// 删除报工记录
export function delFeedback(id) {
  return request({
    url: "/v1/FeedBack/" + id,
    method: "delete",
  });
}

// 报工审批
export function commitFeedback(data) {
  return request({
    url: "/v1/FeedBack/Commit",
    data,
    method: "put",
  });
}
// 撤销审批
export function revokeCommit(data) {
  return request({
    url: "/v1/FeedBack/RevokeCommit",
    data,
    method: "put",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/FeedBack",
    method: "delete",
    data,
  });
}

// 获取生成任务列表

export function listTaskList(data) {
  return request({
    url: "/v1/FeedBack/GetTaskList",
    method: "Post",
    data,
  });
}