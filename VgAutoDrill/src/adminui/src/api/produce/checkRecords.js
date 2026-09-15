import request from "@/utils/request";
 
// 查询检验记录列表
export function listCheckRecords(data) {
  return request({
    url: "/v1/CheckRecords/GetList",
    method: "post",
    data,
  });
}

// 查询检验记录详细
export function getCheckRecords(id) {
  return request({
    url: "/v1/CheckRecords/" + id,
    method: "get",
  });
}

// 新增检验记录
export function addCheckRecords(data) {
  return request({
    url: "/v1/CheckRecords",
    method: "post",
    data,
  });
}

// 修改检验记录
export function updateCheckRecords(data) {
  return request({
    url: "/v1/CheckRecords",
    method: "put",
    data,
  });
}

// 删除检验记录
export function delCheckRecords(id) {
  return request({
    url: "/v1/CheckRecords/" + id,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/CheckRecords",
    method: "delete",
    data,
  });
}

// 设置合格与不合格
export function putCheck(data) {
  return request({
    url: "v1/CheckRecords/PutCheck",
    method: "put",
    data,
  });
}
