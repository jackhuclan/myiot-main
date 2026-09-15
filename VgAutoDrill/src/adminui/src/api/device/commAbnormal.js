import request from "@/utils/request";

// 获取钻机服务调用记录
export function listDeviceServiceInvocations(data) {
  return request({
    url: "/v1/DeviceServiceInvocation/GetDeviceServiceInvocations",
    method: "post",
    data,
  });
}
