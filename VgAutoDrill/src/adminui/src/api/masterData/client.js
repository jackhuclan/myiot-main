import request from "@/utils/request";
 
// 查询客户列表
export function listClient(data) {
  return request({
    url: "/v1/Client/GetList",
    method: "Post",
    data,
  });
}

// 查询客户详细
export function getClient(CutterId) {
  return request({
    url: "/v1/Client/" + CutterId,
    method: "get",
  });
}

// 新增客户
export function addClient(data) {
  return request({
    url: "/v1/Client",
    method: "post",
    data,
  });
}

// 修改客户
export function updateClient(data) {
  return request({
    url: "/v1/Client",
    method: "put",
    data,
  });
}

// 删除客户
export function delClient(id) {
  return request({
    url: "/v1/Client/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Client",
    method: "delete",
    data,
  });
}

 