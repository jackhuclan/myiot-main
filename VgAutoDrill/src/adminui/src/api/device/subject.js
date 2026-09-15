import request from "@/utils/request";

// 获取点检项目列表
export function listSubject(data) {
  return request({
    url: "/v1/Subject/GetList",
    method: "post",
    data,
  });
}

// 获取点检项目详情
export function getSubject(SubjectId) {
  return request({
    url: "/v1/Subject/" + SubjectId,
    method: "get",
  });
}

// 新增点检项目
export function addSubject(data) {
  return request({
    url: "/v1/Subject",
    method: "post",
    data,
  });
}

// 修改点检项目
export function updateSubject(data) {
  return request({
    url: "/v1/Subject",
    method: "put",
    data,
  });
}

// 删除点检项目
export function delSubject(SubjectTypeId) {
  return request({
    url: "/v1/Subject/" + SubjectTypeId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Subject",
    method: "delete",
    data,
  });
}
