import request from "@/utils/request";
// 获取事件日志数据
export function queryEventList(data) {
  return request({
    url: "/v1/Event/QueryEventList",
    method: "POST",
    data: data,
  });
}
// 获取m54日志数据
export function queryM54List(data) {
  return request({
    url: "/v1/Event/QueryM54List",
    method: "POST",
    data: data,
  });
}
