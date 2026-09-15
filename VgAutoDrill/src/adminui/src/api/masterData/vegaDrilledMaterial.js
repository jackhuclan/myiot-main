import request from "@/utils/request";
// 查询AGV或中转位列表
export function listVegaDrilledMaterial(data) {
  return request({
    url: "/v1/Item/GetVegaDrilledMaterial",
    method: "post",
    data,
  });
}
