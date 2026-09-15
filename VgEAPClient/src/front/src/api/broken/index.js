import request from "@/utils/request";
// 获取实时断刀数据
export function queryBrokenList(data) {
  return request({
    url: "/v1/Broken/QueryBrokenList",
    method: "POST",
    data: data,
  });
}
// 获取断刀信息数据
export function queryBrokenEndList(data) {
  return request({
    url: "/v1/Broken/QueryBrokenEndList",
    method: "POST",
    data: data,
  });
}
