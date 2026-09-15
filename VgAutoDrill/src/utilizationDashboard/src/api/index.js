import request from "@/utils/request";
//  获取设备列表
export function getDeviceList(data) {
  return request({
    url: "/v1/DeviceSchedule/GetDeviceList",
    method: "post",
    data: {
      pageNum: 1,
      pageSize: 1000,
      ...data,
    },
  });
}

// 获取设备看板
export function getDashbordList() {
  return request({
    url: "/v1/BigScreen/GetSpecificDeviceDatas",
    method: "get",
  });
}
