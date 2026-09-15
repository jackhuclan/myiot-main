import request from "@/utils/request";
// 查询分区设置数据列表
export function listPartition(data) {
  return request({
    url: "/v1/Partition/GetList",
    method: "post",
    data,
  });
}

// 查询分区设置数据详细
export function getPartition(id) {
  return request({
    url: "/v1/Partition/" + id,
    method: "get",
  });
}

// 新增分区设置数据
export function addPartition(data) {
  return request({
    url: "/v1/Partition",
    method: "post",
    data,
  });
}

// 修改分区设置数据
export function updatePartition(data) {
  return request({
    url: "/v1/Partition",
    method: "put",
    data,
  });
}

// 删除分区设置数据
export function delPartition(id) {
  return request({
    url: "/v1/Partition/" + id,
    method: "delete",
  });
}

// 获取工作站列表wSTreeselect
export function wSTreeselect(data) {
  return request({
    url: "/v1/Workstation/GetList",
    method: "post",
    data: {
      name: undefined,
    },
  });
}
// 获取分区设置树形列表
export function treeselect() {
  return request({
    url: "/v1/Partition/GetTreeSelect",
    method: "get",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Partition",
    method: "delete",
    data,
  });
}

// 启用分区设置
export function enablePartition(params) {
  return request({
    url: "/v1/Partition/Enable",
    method: "post",
    params,
  });
}

// 启用分区设置
export function disablePartition(params) {
  return request({
    url: "/v1/Partition/Disable",
    method: "post",
    params,
  });
}
