import request from "@/utils/request";

// 获取工单关联机台数据
export function getWorkOrderAndWorkStation(data) {
  return request({
    url: "/v1/WorkOrderAndWorkStation/GetList",
    method: "post",
    data,
  });
}

// 新增工单关联机台数据
export function addWorkOrderAndWorkStation(data) {
  return request({
    url: "/v1/WorkOrderAndWorkStation",
    method: "post",
    data,
  });
}
export function delList(data) {
  return request({
    url: "/v1/WorkOrderAndWorkStation/Delete",
    method: "post",
    data,
  });
}
