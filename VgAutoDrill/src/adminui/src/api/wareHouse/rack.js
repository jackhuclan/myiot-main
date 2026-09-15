import request from "@/utils/request";
// 查询库位列表
export function listRack(data) {
  return request({
    url: "/v1/Rack/GetList",
    method: "post",
    data,
  });
}
// 获取库位详细列表
export function getFullDatas(data) {
  return request({
    url: "/v1/Rack/GetFullDatas",
    method: "post",
    data: {
      ...data,
      status: data.status === "" ? undefined : data.status,
    },
  });
}
// 查询库位数据详细
export function getRack(id) {
  return request({
    url: "/v1/Rack/" + id,
    method: "get",
  });
}

// 新增库位
export function addRack(data) {
  return request({
    url: "/v1/Rack/Add",
    method: "post",
    data,
  });
}

// 修改库位
export function updateRack(data) {
  return request({
    url: "/v1/Rack/Update",
    method: "post",
    data,
  });
}

// 删除库位
export function delRack(RackId) {
  return request({
    url: "/v1/Rack/" + RackId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Rack",
    method: "delete",
    data,
  });
}

// 解绑库位与料仓

export function unBind(data) {
  return request({
    url: "/v1/Rack/UnBind",
    method: "post",
    data,
  });
}

// 启用

export function enableRack(data) {
  return request({
    url: "/v1/Rack/Enable",
    method: "post",
    data,
  });
}

// 禁用

export function disableRack(data) {
  return request({
    url: "/v1/Rack/Disable",
    method: "post",
    data,
  });
}

// 根据库位号获取板料列表

export function getCentralRackPanels(code) {
  return request({
    url: "/v1/Rack/GetCentralRackPanels",
    method: "post",
    params: { locationCode: code },
  });
}
