import request from "@/utils/request";

export function formatDateToYMD(cellValue) {
  if (cellValue == null || cellValue == "") return "";
  var date = new Date(cellValue);
  var year = date.getFullYear();
  var month =
    date.getMonth() + 1 < 10
      ? "0" + (date.getMonth() + 1)
      : date.getMonth() + 1;
  var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

  return year + "-" + month + "-" + day;
}

// 获取设备列表
export function listDeviceRecords(data) {
  return request({
    url: "/v1/DeviceRecords/GetList",
    method: "post",
    data,
  });
}
// 获取设备记录汇总
export function getSummaryList(data) {
  return request({
    url: "/v1/DeviceRecords/GetSummaryList",
    method: "post",
    data,
  });
}

// 获取设备详情
export function getDeviceRecords(DeviceRecordsId) {
  return request({
    url: "/v1/DeviceRecords/" + DeviceRecordsId,
    method: "get",
  });
}

// 新增设备
export function addDeviceRecords(data) {
  return request({
    url: "/v1/DeviceRecords",
    method: "post",
    data,
  });
}

// 修改设备
export function updateDeviceRecords(data) {
  return request({
    url: "/v1/DeviceRecords",
    method: "put",
    data,
  });
}

// 删除设备
export function delDeviceRecords(deviceTypeId) {
  return request({
    url: "/v1/DeviceRecords/" + deviceTypeId,
    method: "delete",
  });
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/DeviceRecords",
    method: "delete",
    data,
  });
}

//  导出
export function downLoadList(data) {
  return request({
    url: "/v1/DeviceRecords/DownLoadList",
    method: "post",
    data,
    responseType: "blob",
  });
}

// 获取设备异常明细
export function getRateReasonDetails(recordSummaryId) {
  return request({
    url: "/v1/DeviceRecords/GetRateReasonDetails",
    params: {
      recordSummaryId,
    },
    method: "get",
  });
}
