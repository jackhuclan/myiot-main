import request from "@/utils/request";

// 查询工艺流程列表
export function listRoute(data) {
  return request({
    url: "/v1/Route/GetList",
    method: "post",
    data,
  });
}

// 查询工艺流程详细
export function getRoute(id) {
  return request({
    url: "/v1/Route/" + id,
    method: "get",
  });
}

// 新增工艺流程
export function addRoute(data) {
  return request({
    url: "/v1/Route",
    method: "post",
    data,
  });
}

// 修改工艺流程
export function updateRoute(data) {
  return request({
    url: "/v1/Route",
    method: "put",
    data,
  });
}

// 删除工艺流程
export function delRoute(id) {
  return request({
    url: "/v1/Route/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Route",
    method: "delete",
    data,
  });
}

// 审批工艺路线
export function vettingRoute(data) {
  return request({
    url: "/v1/Route/VettingRoute",
    method: "post",
    data,
  });
}

// 取消审批工艺路线
export function cancelVettingRoute(data) {
  return request({
    url: "/v1/Route/CancelVettingRoute",
    method: "post",
    data,
  });
}

// 搜索时所使用的路线查询
export function getDropSelectDatas(data) {
  return request({
    url: "/v1/Route/GetDropSelectDatas",
    method: "post",
    data,
  });
}
