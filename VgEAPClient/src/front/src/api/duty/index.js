import request from "@/utils/request";
// 获取标准稼动率数据
export function queryDutyList(data) {
  return request({
    url: "/v1/Duty/QueryDutyList",
    method: "POST",
    data: data,
  });
}
// 获取实时稼动率数据
export function queryRealTimeDutyList(data) {
  return request({
    url: "/v1/Duty/QueryRealTimeDutyList",
    method: "POST",
    data: data,
  });
}
// 获取班次稼动率数据
export function queryShiftDutyList(data) {
  return request({
    url: "/v1/Duty/QueryShiftDutyList",
    method: "POST",
    data: data,
  });
}
// 获取稼动率分析数据
export function queryAnalysisDutyList(data) {
  return request({
    url: "/v1/Duty/QueryAnalysisDutyList",
    method: "POST",
    data: data,
  });
}
