import request from "@/utils/request";

// 获取系统标题
export function getTitle() {
  return request({
    url: "/v1/BigScreen/GetTitle",
    method: "get",
  });
}
