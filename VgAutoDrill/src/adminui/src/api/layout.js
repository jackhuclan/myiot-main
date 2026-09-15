import request from "@/utils/request";
// 获取告警信息等及时消息
export function getSysTimelyInformation(id) {
  return request({
    //headers: {
    //  isToken: false,
    //},
    url: "/v1/DeviceSchedule/GetSysTimelyInformation",
    method: "get",
  });
}


// 获取系统标题
export function getTitle() {
  return request({
    url: "/v1/BigScreen/GetTitle",
    method: "get",
  });
}
