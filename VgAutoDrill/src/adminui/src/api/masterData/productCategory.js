import request from "@/utils/request";
 
// 查询产品大类列表
export function listProductCategory(data) {
  return request({
    url: "/v1/ProductCategory/GetList",
    method: "Post",
    data,
  });
}

// 查询产品大类详细
export function getProductCategory(id) {
  return request({
    url: "/v1/ProductCategory/" + id,
    method: "get",
  });
}

// 新增产品大类
export function addProductCategory(data) {
  return request({
    url: "/v1/ProductCategory",
    method: "post",
    data,
  });
}

// 修改产品大类
export function updateProductCategory(data) {
  return request({
    url: "/v1/ProductCategory",
    method: "put",
    data,
  });
}

// 删除产品大类
export function delProductCategory(id) {
  return request({
    url: "/v1/ProductCategory/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/ProductCategory",
    method: "delete",
    data,
  });
}
// 审批产品大类
export function vettingProductCategory(data) {
  return request({
    url: "/v1/ProductCategory/VettingProductCategory",
    method: "post",
    data,
  });
}


// 撤销审批产品大类
export function cncelVettingProductCategory(data){
  return request({
    url: "/v1/ProductCategory/CancelVettingProductCategory",
    method: "post",
    data,
  });
}