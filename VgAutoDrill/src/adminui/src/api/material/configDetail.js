import request  from "@/utils/request";
 
// 查询刀具参数明细列表
export function listCutterConfigDetail(data) {
  return request({
    url: "/v1/CutterConfigDetail/GetList",
    method: "Post",
    data,
  });
}

// 查询刀具参数明细列表详细
export function getCutterConfigDetail(CutterId) {
  return request({
    url: "/v1/CutterConfigDetail/" + CutterId,
    method: "get",
  });
}

// 新增刀具参数明细
export function addCutterConfigDetail(data) {
  return request({
    url: "/v1/CutterConfigDetail",
    method: "post",
    data,
  });
}

// 修改刀具参数明细
export function updateCutterConfigDetail(data) {
  delete data.show;
  return request({
    url: "/v1/CutterConfigDetail",
    method: "put",
    data,
  });
}

// 删除刀具参数明细列表
export function delCutterConfigDetail(id) {
  return request({
    url: "/v1/CutterConfigDetail/" + id,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/CutterConfigDetail",
    method: "delete",
    data,
  });
}
