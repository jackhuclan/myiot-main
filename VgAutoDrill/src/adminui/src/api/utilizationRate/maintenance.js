import request from "@/utils/request";

//获取临时保养记录
export function listMaintenanceRecords(data) {
  return request({
    url: "/v1/DeviceTemporaryMaintenanceRecords/GetList",
    method: "post",
    data,
  });
}

//新增临时保养记录
export function addMaintenanceRecords(data) {
  return request({
    url: "/v1/DeviceTemporaryMaintenanceRecords/Add",
    method: "post",
    data,
  });
}

// 获取临时保养记录详情
export function getMaintenanceRecords(id) {
    return request({
      url: "/v1/DeviceTemporaryMaintenanceRecords/QueryByID/" ,
      params:{id},
      method: "get",
    });
  }
//修改临时保养记录
export function updateMaintenanceRecords(data) {
  return request({
    url: "/v1/DeviceTemporaryMaintenanceRecords/Update",
    method: "post",
    data,
  });
}

// 删除临时保养记录
export function delMaintenanceRecords(id) {
  return request({
    url: "/v1/DeviceTemporaryMaintenanceRecords/Delete?id=" + id,
    method: "delete",
  });
}
