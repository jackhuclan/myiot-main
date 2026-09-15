import request from "@/utils/request";

// 查询配刀组计划列表
export function listCutterGroup(data) {
  return request({
    url: "/v1/CutterGroup/GetList",
    method: "post",
    data,
  });
}

// 查询配刀组计划详细
export function getCutterGroup(id) {
  return request({
    url: "/v1/CutterGroup/GetDetail?cutterGroupNo=" + id,
    method: "get",
  });
}
//  修改配刀组计划列表
export function updateCutterGroup(data) {
  return request({
    url: "/v1/CutterGroup/Update",
    method: "post",
    data,
  });
}
//  批量删除
export function delList(data) {
  return request({
    url: "/v1/CutterGroup/Delete",
    method: "post",
    data,
  });
}
