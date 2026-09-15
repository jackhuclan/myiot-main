import request from "@/utils/request";
// 获取首页数据
export function getSysdrillInformation() {
  return request({
    url: "/v1/Home/GetSysdrillInformation",
    method: "POST",
  });
}
// 获取首页详情数据
export function getEquDrillInformation(data) {
  return request({
    url: "/v1/Home/GetEquDrillInformation",
    method: "POST",
    data: data,
  });
}
