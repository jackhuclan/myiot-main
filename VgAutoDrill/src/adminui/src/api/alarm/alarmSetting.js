import request from "@/utils/request";
// 查询告警设置列表
export function listAlarmSetting(data) {
  return request({
    url: "/v1/AlarmSetting/GetList",
    method: "post",
    data,
  });
}

// 查询告警设置详细
export function getAlarmSetting(AlarmSettingId) {
  return request({
    url: "/v1/AlarmSetting/" + AlarmSettingId,
    method: "get",
  });
}

// 新增告警设置
export function addAlarmSetting(data) {
  return request({
    url: "/v1/AlarmSetting",
    method: "post",
    data,
  });
}

// 修改告警设置
export function updateAlarmSetting(data) {
  return request({
    url: "/v1/AlarmSetting",
    method: "put",
    data,
  });
}

// 删除告警设置
export function delAlarmSetting(id) {
  return request({
    url: "/v1/AlarmSetting/" + id,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/AlarmSetting",
    method: "delete",
    data,
  });
}
