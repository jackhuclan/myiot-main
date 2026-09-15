import request  from "@/utils/request";
 
// 查询刀具参数列表
export function listCutterPlan(data) {
  return request({
    url: "/v1/CutterPlan/GetList",
    method: "Post",
    data,
  });
}

// 查询刀具参数详细
export function getCutterPlan(CutterId) {
  return request({
    url: "/v1/CutterPlan/" + CutterId,
    method: "get",
  });
}

// 新增刀具参数
export function addCutterPlan(data) {
  return request({
    url: "/v1/CutterPlan",
    method: "post",
    data,
  });
}

// 修改刀具参数
export function updateCutterPlan(data) {
  return request({
    url: "/v1/CutterPlan",
    method: "put",
    data,
  });
}

// 删除刀具参数
export function delCutterPlan(id) {
  return request({
    url: "/v1/CutterPlan/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/CutterPlan",
    method: "delete",
    data,
  });
}
