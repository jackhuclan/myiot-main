import request from "@/utils/request";

// 获取设备列表
export function getAgvList(data) {
  return request({
    url: "/v1/DeviceScheduleSummary/GetList",
    method: "post",
    data,
  });
}
 