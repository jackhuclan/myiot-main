import request from "@/utils/request";

// 查询物料产品列表
export function listItem(data) {
  return request({
    url: "/v1/Item/GetList",
    method: "Post",
    data,
  });
}

// 查询物料产品详细
export function getItem(id) {
  return request({
    url: "/v1/Item/" + id,
    method: "get",
  });
}

// 新增物料产品
export function addItem(data) {
  return request({
    url: "/v1/Item",
    method: "post",
    data,
  });
}

// 修改物料产品
export function updateItem(data) {
  return request({
    url: "/v1/Item",
    method: "put",
    data,
  });
}

// 删除物料产品
export function delItem(id) {
  return request({
    url: "/v1/Item/" + id,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Item",
    method: "delete",
    data,
  });
}
// 获取物料Or产品树形列表
export function treeselect() {
  return request({
    url: "/v1/ItemType/GetTreeSelect",
    method: "get",
  });
}
// 获取产品大类树形列表ProductCategory
export function pCTreeselect() {
  return request({
    url: "/v1/ProductCategory/GetTreeSelect",
    method: "get",
  });
}
// 查询分区列表
export function wHTreeselect() {
  return request({
    url: "/v1/Partition/GetList",
    method: "post",
    data: {
      name: undefined,
    },
  });
}
