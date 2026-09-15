import request from "@/utils/request";
// 查询系统配置列表
export function listSysConfig(data) {
  return request({
    url: "/v1/SysConfig/GetList",
    method: "post",
    data,
  });
}

// 查询系统配置详细
export function getSysConfig(id) {
  return request({
    url: "/v1/SysConfig/" + id,
    method: "get",
  });
}

// 新增系统配置
export function addSysConfig(data) {
  return request({
    url: "/v1/SysConfig",
    method: "post",
    data,
  });
}

// 修改系统配置
export function updateSysConfig(data) {
  return request({
    url: "/v1/SysConfig",
    method: "put",
    data,
  });
}

// 删除系统配置
export function delSysConfig(id) {
  return request({
    url: "/v1/SysConfig/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/SysConfig",
    method: "delete",
    data,
  });
}

// 根据配置项获取枚举列表
export function getSysConfigEnumList(data) {
  return request({
    url: "/v1/SysConfig/GetSysConfigEnumList",
    method: "post",
    data,
  });
}

// 获取配置类型
export function getSysConfigType(id) {
  return request({
    url: "/v1/SysConfig/GetSysConfigType/" + id,
    method: "get",
  });
}

// 保存基本配置
export function saveBasicSysData(data) {
  return request({
    url: "/v1/SysConfig/SaveBasicSysData",
    method: "post",
    data,
  });
}

// 树形
export function getTreeList(data) {
  return request({
    url: "/v1/SysConfig/GetTreeList",
    method: "post",
    data,
  });
}

// 获取类型树
export function treeselect() {
  return request({
    url: "/v1/SysConfig/GetSysConfigCategoryEnums",
    method: "post",
  });
}
