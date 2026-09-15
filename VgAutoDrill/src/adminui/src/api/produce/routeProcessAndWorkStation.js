import request from "@/utils/request";

// 查询工艺关联工作站列表
export function listRouteProcessAndWorkStation(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/GetList",
    method: "post",
    data: data,
  });
}

// 查询工艺关联工作站详细
export function getRouteProcessAndWorkStation(id) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/" + id,
    method: "get",
  });
}

//添加的时候判断工作站是否绑定其他工艺路线
export function exsitWorkStation(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/ExsitWorkStation",
    method: "post",
    data: data,
  });
}
//添加的时候判断工作站是否绑定其他工艺路线
export function exsitWorkStationByRoute(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/ExsitWorkStationByRoute",
    method: "post",
    data: data,
  });
}
// 新增工艺关联工作站
export function addRouteProcessAndWorkStation(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation",
    method: "post",
    data: data,
  });
}

// 修改工艺关联工作站
export function updateRouteProcessAndWorkStation(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation",
    method: "put",
    data: data,
  });
}

// 删除工艺关联工作站
export function delRouteProcessAndWorkStation(id) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation",
    method: "delete",
    data,
  });
}

// 根据工艺路线。工序获取工作站

export function getFitWorkStationListByRoute(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/GetFitWorkStationListByRoute",
    method: "post",
    data,
  });
}
