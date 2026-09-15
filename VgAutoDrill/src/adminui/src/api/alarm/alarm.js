import request from "@/utils/request";

// 查询告警记录列表
export function listAlarm(data) {
  return request({
    url: "/v1/Alarm/GetList",
    method: "post",
    data,
  });
}

// 查询告警记录详细
export function getAlarm(AlarmId) {
  return request({
    url: "/v1/Alarm/" + AlarmId,
    method: "get",
  });
}

// 新增告警记录
export function addAlarm(data) {
  return request({
    url: "/v1/Alarm",
    method: "post",
    data,
  });
}

// 修改告警记录
export function updateAlarm(data) {
  return request({
    url: "/v1/Alarm",
    method: "put",
    data,
  });
}

// 删除告警记录
export function delAlarm(AlarmId) {
  return request({
    url: "/v1/Alarm/" + AlarmId,
    method: "delete",
  });
}

// 获取告警类别列表
export function treeselect() {
  return request({
    url: "/v1/AlarmType/GetTreeSelect",
    method: "get",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/Alarm",
    method: "delete",
    data,
  });
}
