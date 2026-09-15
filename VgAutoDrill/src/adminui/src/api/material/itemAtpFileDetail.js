import request from "@/utils/request";
 

// 查询atp文件明细表列表
export function listMultiList(data) {
  return request({
    url: "/v1/ItemAtpFileDetail/GetMultiList",
    method: "Post",
    data,
  });
}
// 修改atp文件明细表详细

export function updateMultiList(data) {
  return request({
    url: "/v1/ItemAtpFileDetail/UpdateList",
    method: "put",
    data,
  });
}

// 查询atp文件明细表详细
export function getItemDrillFileDetail(ItemDrillFileDetailId) {
  return request({
    url: "/v1/ItemDrillFileDetail/" + ItemDrillFileDetailId,
    method: "get",
  });
}

// 新增atp文件明细表
export function addItemDrillFileDetail(data) {
  return request({
    url: "/v1/ItemDrillFileDetail",
    method: "post",
    data,
  });
}

// 修改atp文件明细表
export function updateItemDrillFileDetail(data) {
  return request({
    url: "/v1/ItemDrillFileDetail",
    method: "put",
    data,
  });
}

// 删除atp文件明细表
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
