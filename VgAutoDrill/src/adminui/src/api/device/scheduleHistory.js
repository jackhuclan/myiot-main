import request from "@/utils/request";

// 查询调度记录列表
export function listScheduleHistory(data) {
  return request({
    url: "/v1/ScheduleHistory/GetList",
    method: "post",
    data,
    // 一小时
    timeout: 60 * 60 * 1000,
  });
}

// 查询调度详细
export function getScheduleHistory(ScheduleHistoryId) {
  return request({
    url: "/v1/ScheduleHistory/" + ScheduleHistoryId,
    method: "get",
  });
}

// 获取调度记录明细
export function getDetailData(ScheduleHistoryId) {
  return request({
    url: "/v1/ScheduleHistory/GetScheduleLogs/" + ScheduleHistoryId,
    method: "get",
  });
}
