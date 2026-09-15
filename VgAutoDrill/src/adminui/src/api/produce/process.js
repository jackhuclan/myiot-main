import request from "@/utils/request";

// 查询生产记录列表
export function listProcess(data) {
  return request({
    url: "/v1/Process/GetList",
    method: "post",
    data,
  });
}

// 查询生产记录详细
export function getProcess(id) {
  return request({
    url: "/v1/Process/" + id,
    method: "get",
  });
}

// 新增生产记录
export function addProcess(data) {
  return request({
    url: "/v1/Process",
    method: "post",
    data,
  });
}

// 修改生产记录
export function updateProcess(data) {
  return request({
    url: "/v1/Process",
    method: "put",
    data,
  });
}

// 删除生产记录
export function delProcess(id) {
  return request({
    url: "/v1/Process/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Process",
    method: "delete",
    data,
  });
}

// 获取工序下拉框数据
export function getDropSelectDatas(data) {
  return request({
    url: "v1/Process/GetDropSelectDatas",
    method: "post",
    data,
  });
}
