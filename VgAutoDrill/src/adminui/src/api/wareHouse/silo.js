import request from "@/utils/request";
// 查询料仓列表
export function listSilo(data) {
  return request({
    url: "/v1/Silo/GetList",
    method: "post",
    data,
  });
}
// 查询料仓数据详细
export function getSilo(id) {
  return request({
    url: "/v1/Silo/" + id,
    method: "get",
  });
}

// 新增料仓
export function addSilo(data) {
  return request({
    url: "/v1/Silo/Add",
    method: "post",
    data,
  });
}

// 修改料仓
export function updateSilo(data) {
  return request({
    url: "/v1/Silo/Update",
    method: "post",
    data,
  });
}

// 删除料仓
export function delSilo(SiloId) {
  return request({
    url: "/v1/Silo/" + SiloId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Silo",
    method: "delete",
    data,
  });
}

// 查询料仓载料信息
export function listSiloDetails(data) {
  return request({
    url: "/v1/Silo/GetSiloDetails",
    method: "post",
    data,
  });
}
// 添加或者更新料仓板料信息
export function addOrUpdateSiloDetails(data) {
  return request({
    url: "/v1/Silo/AddOrUpdate",
    method: "post",
    data,
  });
}
// 解绑料仓与板料
export function unBindPanel(data) {
  return request({
    url: "/v1/Silo/UnBindPanel",
    method: "post",
    data,
  });
}

// 一键解绑料仓与板料
export function unBindAllPanel(data) {
  return request({
    url: "/v1/Silo/UnBindAllPanel",
    method: "post",
    data,
  });
}

// 扫码绑定料仓与板料
export function bindSingle(data) {
  return request({
    url: "/v1/Silo/BindSingle",
    method: "post",
    data,
  });
}
// 检查板料是否绑定 并执行绑定
export function isBindSingle(data) {
  return request({
    url: "/v1/Silo/IsBindSingle",
    method: "post",
    data,
  });
}
// 根据库位编码获取库位明细
export function getLocationDetail(code) {
  return request({
    url: "/v1/LocationDetail/GetLocationDetail/" + code,
    method: "get",
  });
}

// 设置手动
export function setManual(data) {
  return request({
    url: "/v1/Silo/SetManual",
    method: "post",
    data,
  });
}

// 设置就绪
export function setReady(data) {
  return request({
    url: "/v1/Silo/SetReady",
    method: "post",
    data,
  });
}

// 移动板料到指定料仓层

export function movePanelToOtherSilo(data) {
  return request({
    url: "/v1/Silo/MovePanelToOtherSilo",
    method: "post",
    data,
  });
}

// AGV绑定、解绑料仓
export function agvBindSilo(data) {
  return request({
    url: "/v1/Silo/AgvBindSilo",
    method: "post",
    params: data,
  });
}
