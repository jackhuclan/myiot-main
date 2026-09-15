import request from "@/utils/request";

// 查询板料列表
export function listBoardTrace(data) {
  return request({
    // url: '/v1/BoardTrace/GetList',
    url: "/v1/BoardTrace/GetFullTreeList", //树形结构查找
    method: "post",
    data,
    timeout: 1000 * 60,
  });
}

// 查询板料详细
export function getBoardTrace(id) {
  return request({
    url: "/v1/BoardTrace/" + id,
    method: "get",
  });
}

// 新增板料
export function addBoardTrace(data) {
  return request({
    url: "/v1/BoardTrace",
    method: "post",
    data,
  });
}

// 修改板料
export function updateBoardTrace(data) {
  return request({
    url: "/v1/BoardTrace",
    method: "put",
    data,
  });
}

// 删除板料
export function delBoardTrace(id) {
  return request({
    url: "/v1/BoardTrace/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/BoardTrace",
    method: "delete",
    data,
  });
}
