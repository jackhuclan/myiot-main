import request from "@/utils/request";

// 查询钻带参数列表
export function listItemDrillFileDetail(data) {
  return request({
    url: "/v1/ItemDrillFileDetail/GetList",
    method: "Post",
    data,
  });
}

// 查询钻带参数详细
export function getItemDrillFileDetail(ItemDrillFileDetailId) {
  return request({
    url: "/v1/ItemDrillFileDetail/" + ItemDrillFileDetailId,
    method: "get",
  });
}

// 新增钻带参数
export function addItemDrillFileDetail(data) {
  return request({
    url: "/v1/ItemDrillFileDetail",
    method: "post",
    data,
  });
}

// 修改钻带参数
export function updateItemDrillFileDetail(data) {
  return request({
    url: "/v1/ItemDrillFileDetail",
    method: "put",
    data,
  });
}

// 删除钻带参数
export function delItemDrillFileDetail(id) {
  return request({
    url: "/v1/ItemDrillFileDetail/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/ItemDrillFileDetail",
    method: "delete",
    data,
  });
}
