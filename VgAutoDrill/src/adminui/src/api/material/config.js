import request  from "@/utils/request";
 
// 查询刀具参数主表列表
export function listCutterConfigMaster(data) {
  return request({
    url: "/v1/CutterConfigMaster/GetList",
    method: "Post",
    data
  });
}

// 查询刀具参数主表详细
export function getCutterConfigMaster(CutterId) {
  return request({
    url: "/v1/CutterConfigMaster/" + CutterId,
    method: "get",
  });
}

// 新增刀具参数主表
export function addCutterConfigMaster(data) {
  return request({
    url: "/v1/CutterConfigMaster",
    method: "post",
    data,
  });
}

// 修改刀具参数主表
export function updateCutterConfigMaster(data) {
  return request({
    url: "/v1/CutterConfigMaster",
    method: "put",
    data: data,
  });
}

// 删除刀具参数主表
export function delCutterConfigMaster(id) {
  return request({
    url: "/v1/CutterConfigMaster/" + id,
    method: "delete",
  });
}
// 多选删除刀具参数主表
export function delList(data) {
  return request({
    url: "/v1/CutterConfigMaster",
    method: "delete",
    data
  });
}


// 刀具文件上传
export function upLoad(data) {
  return request({
    url: "/v1/CutterConfigMaster/UpLoad",
    method: "post",
    headers: {
      "Content-Type": "multipart/form-data",
    },
    data,
  });
}
