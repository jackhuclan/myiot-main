import request  from "@/utils/request";
 
// 查询编码规则列表
export function listEncodeBuildRules(data) {
  return request({
    url: "/v1/EncodeBuildRules/GetList",
    method: "Post",
    data,
  });
}

// 查询编码规则详细
export function getEncodeBuildRules(id) {
  return request({
    url: "/v1/EncodeBuildRules/" + id,
    method: "get",
  });
}

// 新增编码规则
export function addEncodeBuildRules(data) {
  return request({
    url: "/v1/EncodeBuildRules",
    method: "post",
    data,
  });
}

// 修改编码规则
export function updateEncodeBuildRules(data) {
  return request({
    url: "/v1/EncodeBuildRules",
    method: "put",
    data,
  });
}

// 删除编码规则
export function delEncodeBuildRules(id) {
  return request({
    url: "/v1/EncodeBuildRules/" + id,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/EncodeBuildRules",
    method: "delete",
    data,
  });
}