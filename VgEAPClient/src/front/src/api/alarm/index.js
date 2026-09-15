import request from "@/utils/request";
// 获取报警信息数据
export function queryAlarmList(data) {
  return request({
    url: "/v1/Alarm/QueryAlarmList",
    method: "POST",
    data: data,
  });
}
