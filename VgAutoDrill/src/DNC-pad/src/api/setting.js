import request from "@/utils/request";
//  根据钻机code查询手动呼叫agv任务信息
export function queryByDeviceCode(deviceCode) {
  return request({
    url:
      "/v1/ManualCallAgv/ManualCallAgvTask/QueryByDeviceCode?deviceCode=" +
      deviceCode,
    method: "get",
  });
}

export function update(data) {
  return request({
    url: "/v1/ManualCallAgv/ManualCallAgvTask/Update",
    method: "post",
    data,
  });
}
