import request from "@/utils/request";
// 查询休息点和分区关联关系列表
export function listAgvRestAndPart(data) {
  return request({
    url: "/v1/AgvRestAndPart/GetList",
    method: "post",
    data,
  });
}

// 查询休息点和分区关联关系数据详细
export function getAgvRestAndPart(id) {
  return request({
    url: "/v1/AgvRestAndPart/" + id,
    method: "get",
  });
}

// 新增休息点和分区关联关系
export function addAgvRestAndPart(data) {
  return request({
    url: "/v1/AgvRestAndPart",
    method: "post",
    data,
  });
}

// 修改休息点和分区关联关系
export function updateAgvRestAndPart(data) {
  return request({
    url: "/v1/AgvRestAndPart",
    method: "put",
    data,
  });
}

// 删除休息点和分区关联关系
export function delAgvRestAndPart(AgvRestAndPartId) {
  return request({
    url: "/v1/AgvRestAndPart/" + AgvRestAndPartId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/AgvRestAndPart",
    method: "delete",
    data,
  });
}
