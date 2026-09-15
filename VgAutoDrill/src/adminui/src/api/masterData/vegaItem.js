import request from "@/utils/request";
// 查询AGV或中转位列表
export function listVegaRawMaterial(data) {
  return request({
    url: "/v1/Item/GetVegaRawMaterial",
    method: "post",
    data,
  });
}
