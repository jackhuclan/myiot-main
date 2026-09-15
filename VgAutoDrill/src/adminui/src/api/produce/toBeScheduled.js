import request from "@/utils/request";

// 查询板料列表
export function getMaterialList(data) {
  return request({
    url: "/v1/ExternalWorkOrder/GetMaterialList",
    method: "post",
    data,
  });
}
