import request from "@/utils/request";
 
// 查询刀具列表
export function listCutter(data) {
  return request({
    url: "/v1/DeviceCutter/GetList",
    method: "Post",
    data,
  });
}

// 查询刀具详细
export function getCutter(CutterId) {
  return request({
    url: "/v1/DeviceCutter/" + CutterId,
    method: "get",
  });
}

// 新增刀具
export function addCutter(data) {
  return request({
    url: "/v1/DeviceCutter",
    method: "post",
    data,
  });
}

// 修改刀具
export function updateCutter(data) {
  return request({
    url: "/v1/DeviceCutter",
    method: "put",
    data,
  });
}

// 删除刀具
export function delCutter(id) {
  return request({
    url: "/v1/DeviceCutter/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/DeviceCutter",
    method: "delete",
    data,
  });
}
