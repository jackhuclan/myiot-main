import request  from "@/utils/request";
 
// 查询配方列表
export function listProcessRecipe(data) {
  return request({
    url: "/v1/ProcessRecipe/GetList",
    method: "post",
    data,
  });
}

// 查询配方详细
export function getProcessRecipe(ProcessRecipeId) {
  return request({
    url: "/v1/ProcessRecipe/" + ProcessRecipeId,
    method: "get",
  });
}

// 新增配方
export function addProcessRecipe(data) {
  return request({
    url: "/v1/ProcessRecipe",
    method: "post",
    data,
  });
}

// 修改配方
export function updateProcessRecipe(data) {
  return request({
    url: "/v1/ProcessRecipe",
    method: "put",
    data,
  });
}

// 删除配方
export function delProcessRecipe(deviceRecipeId) {
  return request({
    url: "/v1/ProcessRecipe/" + deviceRecipeId,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/ProcessRecipe",
    method: "delete",
    data,
  });
}

// 获取设备树形列表
export function treeselect() {
  return request({
    url: "/v1/DeviceType/GetTreeSelect",
    method: "get",
  });
}
