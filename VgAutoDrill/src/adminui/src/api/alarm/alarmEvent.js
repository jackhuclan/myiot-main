import request from "@/utils/request";
// 查询事件列表
export function listAlarmEvent(data) {
  return request({
    url: "/v1/AlarmEvent/GetList",
    method: "post",
    data,
  });
}

// 查询事件详细
export function getAlarmEvent(AlarmEventId) {
  return request({
    url: "/v1/AlarmEvent/" + AlarmEventId,
    method: "get",
  });
}

// 新增事件
export function addAlarmEvent(data) {
  return request({
    url: "/v1/AlarmEvent",
    method: "post",
    data,
  });
}

// 修改事件
export function updateAlarmEvent(data) {
  return request({
    url: "/v1/AlarmEvent",
    method: "put",
    data,
  });
}

// 删除事件
export function delAlarmEvent(id) {
  return request({
    url: "/v1/AlarmEvent/" + id,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/AlarmEvent",
    method: "delete",
    data,
  });
}
