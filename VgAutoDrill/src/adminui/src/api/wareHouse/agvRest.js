import request from "@/utils/request";
// 查询休息点列表
export function listAgvRest(data) {
  return request({
    url: "/v1/AgvRest/GetList",
    method: "post",
    data,
  });
}

// 查询休息点数据详细
export function getAgvRest(id) {
  return request({
    url: "/v1/AgvRest/" + id,
    method: "get",
  });
}

// 新增休息点
export function addAgvRest(data) {
  return request({
    url: "/v1/AgvRest",
    method: "post",
    data,
  });
}

// 修改休息点
export function updateAgvRest(data) {
  return request({
    url: "/v1/AgvRest",
    method: "put",
    data,
  });
}

// 删除休息点
export function delAgvRest(AgvRestId) {
  return request({
    url: "/v1/AgvRest/" + AgvRestId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/AgvRest",
    method: "delete",
    data,
  });
}

// 禁用

export function disableAgvRest(params) {
  return request({
    url: "/v1/AgvRest/Disable",
    method: "post",
    params,
  });
}
// 启用
export function enableAgvRest(params) {
  return request({
    url: "/v1/AgvRest/Enable",
    method: "post",
    params,
  });
}
