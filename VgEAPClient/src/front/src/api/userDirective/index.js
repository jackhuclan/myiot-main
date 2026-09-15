import request from "@/utils/request";
// 获取用户指令
export function queryCOMMList(data) {
  return request({
    url: "/v1/Comm/QueryCOMMList",
    method: "POST",
    data: data,
  });
}
