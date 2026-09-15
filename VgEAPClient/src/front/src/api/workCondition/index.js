import request from "@/utils/request";
// 获取工况数据
export function queryorkConditionList(data) {
  return request({
    url: "/v1/WorkCondition/QueryWorkConditionList",
    method: "POST",
    data: data,
  });
}
