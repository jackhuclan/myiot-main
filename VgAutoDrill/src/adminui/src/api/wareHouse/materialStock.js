import request from "@/utils/request";
// 查询库存现有量数据列表
export function listMaterialStock(data) {
  return request({
    // url: '/v1/MaterialStock/GetList',
    // 树形表格
    url: "/v1/MaterialStock/GetEquipmentTreeList",
    method: "post",
    data,
  });
}

// 查询库存现有量数据详细
export function getMaterialStock(id) {
  return request({
    url: "/v1/MaterialStock/" + id,
    method: "get",
  });
}

// 新增库存现有量数据
export function addMaterialStock(data) {
  return request({
    url: "/v1/MaterialStock",
    method: "post",
    data,
  });
}

// 修改库存现有量数据
export function updateMaterialStock(data) {
  return request({
    url: "/v1/MaterialStock",
    method: "put",
    data,
  });
}

// 删除库存现有量数据
export function delMaterialStock(id) {
  return request({
    url: "/v1/MaterialStock/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/MaterialStock",
    method: "delete",
    data,
  });
}
