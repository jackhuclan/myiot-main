import request from "@/utils/request";
// 查询供应商列表
export function listVendor(data) {
  return request({
    url: "/v1/Vendor/GetList",
    method: "post",
    data,
  });
}

// 查询供应商详细
export function getVendor(Id) {
  return request({
    url: "/v1/Vendor/" + Id,
    method: "get",
  });
}

// 新增供应商
export function addVendor(data) {
  return request({
    url: "/v1/Vendor",
    method: "post",
    data,
  });
}

// 修改供应商
export function updateVendor(data) {
  return request({
    url: "/v1/Vendor",
    method: "put",
    data,
  });
}

// 删除供应商
export function delVendor(id) {
  return request({
    url: "/v1/Vendor/" + id,
    method: "delete",
  });
}


// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Vendor",
    method: "delete",
    data
  });
}