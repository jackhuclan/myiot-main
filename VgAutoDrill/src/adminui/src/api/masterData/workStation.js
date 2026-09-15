import request from "@/utils/request";

// 查询工作站列表
export function listWorkstation(data) {
  return request({
    url: "/v1/Workstation/GetList",
    method: "post",
    data,
  });
}

// 查询工作站详细
export function getWorkstation(Id) {
  return request({
    url: "/v1/Workstation/" + Id,
    method: "get",
  });
}

// 新增工作站
export function addWorkstation(data) {
  return request({
    url: "/v1/Workstation",
    method: "post",
    data,
  });
}

// 修改工作站
export function updateWorkstation(data) {
  return request({
    url: "/v1/Workstation",
    method: "put",
    data,
  });
}

// 删除工作站
export function delWorkstation(id) {
  return request({
    url: "/v1/Workstation/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Workstation",
    method: "delete",
    data,
  });
}

// 获取车间数据

export function workshopTreeselect() {
  return request({
    url: "/v1/Workshop/GetList",
    method: "post",
    data: {
      name: undefined,
    },
  });
}

// 查询工序与工作站表
export function listRouteProcessAndWorkStation(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/GetList",
    method: "post",
    data,
  });
}

// 根据工作站获取关联的工艺路线

export function getRoutesByWorkStation(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/GetRoutesByWorkStation",
    method: "post",
    data,
  });
}
// 根据工序编码获取工艺路线结果
export function getRoutesByProcess(data) {
  return request({
    url: "/v1/RouteAndProcess/GetRoutesByProcess",
    method: "post",
    data,
  });
}
// 添加工作站关联工艺路线
export function addDataByWorkStation(data) {
  return request({
    url: "/v1/RouteProcessAndWorkStation/AddDataByWorkStation",
    method: "post",
    data,
  });
}
