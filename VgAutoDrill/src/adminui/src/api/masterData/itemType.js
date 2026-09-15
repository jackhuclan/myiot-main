import request  from "@/utils/request";
 
// 查询物料产品分类列表
export function listItemType(data) {
  return request({
    url: "/v1/ItemType/GetList",
    // 产品分类树形
    // url: "/v1/ItemType/GetFullTreeList",
    method: "Post",
    data,
  });
}

// 查询物料产品分类详细
export function getItemType(CutterId) {
  return request({
    url: "/v1/ItemType/" + CutterId,
    method: "get",
  });
}

// 新增物料产品分类
export function addItemType(data) {
  return request({
    url: "/v1/ItemType",
    method: "post",
    data,
  });
}

// 修改物料产品分类
export function updateItemType(data) {
  return request({
    url: "/v1/ItemType",
    method: "put",
    data,
  });
}

// 删除物料产品分类
export function delItemType(id) {
  return request({
    url: "/v1/ItemType/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/ItemType",
    method: "delete",
    data,
  });
}

// 获取父类树形结构
export function treeSelect() {
  return request({
    url: "/v1/ItemType/GetTreeSelect",
    method: "get",
  });
}
