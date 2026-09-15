import request  from "@/utils/request";
 

// 查询钻带参数列表
export function listItemDrillFile(data) {
  return request({
    url: "/v1/ItemDrillFile/GetList",
    method: "Post",
    data,
  });
}

// 查询钻带参数详细
export function getItemDrillFile(ItemDrillFileId) {
  return request({
    url: "/v1/ItemDrillFile/" + ItemDrillFileId,
    method: "get",
  });
}

// 新增钻带参数
export function addItemDrillFile(data) {
  return request({
    url: "/v1/ItemDrillFile",
    method: "post",
    data,
  });
}

// 修改钻带参数
export function updateItemDrillFile(data) {
  return request({
    url: "/v1/ItemDrillFile",
    method: "put",
    data,
  });
}

// 删除钻带参数
export function delItemDrillFile(id) {
  return request({
    url: "/v1/ItemDrillFile/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/ItemDrillFile",
    method: "delete",
    data,
  });
}

// 上传钻带参数/v1/ItemDrillFile/UpLoad
export function upLoad(data) {
  return request({
    url: "/v1/ItemDrillFile/UpLoad",
    method: "post",
    headers: {
      "Content-Type": "multipart/form-data",
    },
    data,
  });
}

