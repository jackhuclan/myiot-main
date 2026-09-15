import request from "@/utils/request";

//获取设备看板数据
export function getDeviceDatas( ) {
 
  return request({
    url: "/v1/BigScreen/GetDeviceDatas", 
    method: "get", 
  });
}