import request from "@/utils/request";
import { status } from "nprogress";
// 获取在线钻机待做任务
export function getDrillDeviceTask(data) {
  return request({
    headers: {
      isToken: false,
    },
    url: "/v1/DeviceSchedule/GetDrillDeviceTask",
    method: "post",
    data,
  });
}

// 获取在线AGV板料信息
export function getAGVDeviceSiloInfo(data) {
  return request({
    headers: {
      isToken: false,
    },
    url: "/v1/DeviceSchedule/GetAGVDeviceSiloInfo",
    method: "post",
    data,
  });
}

// 根据设备编码查询COMMITED的任务
export function getTaskByDevice(data) {
  return request({
    headers: {
      isToken: false,
    },
    url: "/v1/DeviceSchedule/GetTaskByDevice",
    method: "post",
    data,
  });
}

// 获取未分配调度记录列表
export function getScheduleList(data) {
  return request({
    headers: {
      isToken: false,
    },
    url: "/v1/DeviceSchedule/GetScheduleList",
    method: "post",
    data,
  });
}
// 获取已开始调度记录表

export function getRunningScheduleList(data) {
  return request({
    headers: {
      isToken: false,
    },
    url: "/v1/DeviceSchedule/GetRunningScheduleList",
    method: "post",
    data,
  });
}
// 获取调度记录明细
export function getDetailData(schedulementId) {
  return request({
    headers: {
      isToken: false,
    },
    url: "/v1/DeviceSchedule/GetScheduleLogs/" + schedulementId,
    method: "get",
  });
}

// 获取中控是否维护
export function getCentralControlSystemIsMaintaining() {
  return request({
    headers: {
      isToken: false,
    },
    url: "/v1/DeviceSchedule/GetCentralControlSystemIsMaintaining",
    method: "get",
  });
}

// 获取钻机看板数据
export function getDrillPanelFullData(data) {
  return request({
    url: "/v1/DrillPanelDetail/GetDrillPanelFullData",
    method: "post",
    data: {
      ...data,
      status: data.status === "" ? undefined : data.status,
    },
    // 超时限制15s
    timeout: 15 * 1000,
  });
}

// 获取库位
export function getRackFullDatas(data) {
  return request({
    url: "/v1/DeviceSchedule/GetRackFullDatas",
    method: "post",
    data,
  });
}
