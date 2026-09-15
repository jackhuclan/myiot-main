import request from "@/utils/request";
//  根据查询条件获取料仓板料追溯列表 (条件查询筛选)
export function listSiloPanelTrace(data) {
  return request({
    url: "/v1/SiloPanelTrace/GetList",
    method: "post",
    data,
  });
}
//  根据查询条件获取料仓板料追溯详细记录列表 (条件查询筛选)
export function listSiloPanelTraceDetail(data) {
  return request({
    url: "/v1/SiloPanelTraceDetail/GetList",
    method: "post",
    data,
  });
}
 
// 查询料仓板料追溯详细信息
export function getSiloPanelTrace(id) {
  return request({
    url: "/v1/SiloPanelTrace/" + id,
    method: "get",
  });
}