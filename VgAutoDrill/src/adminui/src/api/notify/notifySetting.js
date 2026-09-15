import request from "@/utils/request";
// 查询通知设置列表
export function listNotifySetting(data) {
  return request({
    url: "/v1/NotifySetting/GetList",
    method: "post",
    data,
  });
}

// 查询通知设置详细
export function getNotifySetting(NotifySettingId) {
  return request({
    url: "/v1/NotifySetting/" + NotifySettingId,
    method: "get",
  });
}

// 新增通知设置
export function addNotifySetting(data) {
  return request({
    url: "/v1/NotifySetting",
    method: "post",
    data,
  });
}

// 修改通知设置
export function updateNotifySetting(data) {
  return request({
    url: "/v1/NotifySetting",
    method: "put",
    data,
  });
}

// 删除通知设置
export function delNotifySetting(notifySettingId) {
  return request({
    url: "/v1/NotifySetting/" + notifySettingId,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/NotifySetting",
    method: "delete",
    data,
  });
}
