import request from "@/utils/request";

// 查询车间列表
export function listWorkshop(data) {
  return request({
    url: "/v1/Workshop/GetList",
    method: "post",
    data,
  });
}

// 查询车间详细
export function getWorkshop(Id) {
  return request({
    url: "/v1/Workshop/" + Id,
    method: "get",
  });
}

// 新增车间
export function addWorkshop(data) {
  return request({
    url: "/v1/Workshop",
    method: "post",
    data,
  });
}

// 修改车间
export function updateWorkshop(data) {
  return request({
    url: "/v1/Workshop",
    method: "put",
    data,
  });
}

// 删除车间
export function delWorkshop(id) {
  return request({
    url: "/v1/Workshop/" + id,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Workshop",
    method: "delete",
    data,
  });
}
// 获取车间下拉框数据
export function getDropSelectDatas(data) {
  return request({
    url: "v1/Workshop/GetDropSelectDatas",
    method: "post",
    data,
  });
}
