import request from "@/utils/request";
// 查询通知列表
export function listNotify(data) {
  return request({
    url: '/v1/Notify/GetList',
    method: 'post',
    data
  })
}

// 查询通知详细
export function getNotify(NotifyId) {
  return request({
    url: '/v1/Notify/' + NotifyId,
    method: 'get'
  })
}

// 新增通知
export function addNotify(data) {
  return request({
    url: '/v1/Notify',
    method: 'post',
    data: data
  })
}

// 修改通知
export function updateNotify(data) {
  return request({
    url: '/v1/Notify',
    method: 'put',
    data: data
  })
}

// 删除通知
export function delNotify(NotifyId) {
  return request({
    url: '/v1/Notify/' + NotifyId,
    method: 'delete'
  })
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Notify",
    method: "delete",
    data
  });
}