import request from "@/utils/request";

// 获取设备列表
export function listDrill(data) {
  let isAuto = undefined;
  if (data.isAuto == "自动") {
    isAuto = true;
  } else if (data.isAuto == "非自动") {
    isAuto = false;
  }
  return request({
    url: "/v1/Device/GetList",
    method: "post",
    data: {
      pageNum: 1,
      pageSize: 1000,
      isAuto,
    },
  });
}

// agv操作
export function agvOperate(data) {
  return request({
    url: "/v1/ManualCallAgv/AgvOperate",
    method: "post",
    data,
  });
}

// 获取生产任务
export function listTask(data) {
  return request({
    url: "/v1/ManualCallAgv/TaskList",
    method: "post",
    data,
  });
}

// 修改任务状态(开始和完成)
export function udateTaskStatus(data) {
  return request({
    url: "/v1/ManualCallAgv/UpdateTaskStatus",
    method: "post",
    data,
  });
}

// 解绑
export function getAgvTaskByLocationCode(locationCode) {
  return request({
    url: "/v1/ManualCallAgv/GetAgvTaskByLocationCode",
    method: "get",
    params: { locationCode },
  });
}

// 获取AGV运行状态

export function getAgvRunStatus(locationCode) {
  return request({
    url: "/v1/ManualCallAgv/GetAgvRunStatus",
    method: "get",
    params: { locationCode },
  });
}

// 加载钻带参数
export function loadingDrillFile(data) {
  return request({
    url: "/v1/ManualCallAgv/LoadingDrillFile",
    method: "post",
    data,
  });
}

// 加载ATP
export function loadingATPFile(data) {
  return request({
    url: "/v1/ManualCallAgv/LoadingATPFile",
    method: "post",
    data,
  });
}

//获取刀盒系统二维码
export function getAptBoxBarcode(params) {
  return request({
    url: "/v1/ManualCallAgv/GetAptBoxBarcode",
    method: "get",
    params,
  });
}

// 比对刀盒并更新状态 
export function confirmAptBoxBarcode(data) {
  return request({
    url: "/v1/ManualCallAgv/ConfirmAptBoxBarcode",
    method: "post",
    data,
  });
}
