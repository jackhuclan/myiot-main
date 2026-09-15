// 日志页面
import request from "@/utils/request";

// 获取手动呼叫AGV上下料操作日志
export function listLog(data) {
  return request({
    url: "/v1/ManualCallAgv/Log/List",
    method: "post",
    data,
  });
}
