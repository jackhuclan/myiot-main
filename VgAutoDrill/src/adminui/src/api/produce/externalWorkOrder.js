import request from "@/utils/request";

// 查询外部工单列表
export function listExternalWorkOrder(data) {
  return request({
    url: "/v1/ExternalWorkOrder/GetExterWorkOrderList",
    method: "post",
    data,
  });
}

// 查询外部工单详细
export function getExternalWorkOrder(id) {
  return request({
    url: "/v1/ExternalWorkOrder/" + id,
    method: "get",
  });
}

// 新增外部工单
export function addExternalWorkOrder(data) {
  return request({
    url: "/v1/ExternalWorkOrder",
    method: "post",
    data,
  });
}

// 修改外部工单
export function updateExternalWorkOrder(data) {
  return request({
    url: "/v1/ExternalWorkOrder/Update",
    method: "post",
    data,
  });
}

// 删除外部工单
export function delExternalWorkOrder(id) {
  return request({
    url: "/v1/ExternalWorkOrder/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/ExternalWorkOrder",
    method: "delete",
    data,
  });
}

//v1/ExternalWorkOrder/GetStockInfoQuery
export function getStockInfoQuery(data) {
  return request({
    url: "/v1/ExternalWorkOrder/GetStockInfoQuery",
    method: "post",
    data,
  });
}

// 发起mes转仓
export function kwAGVStockIn(code) {
  return request({
    url: "/v1/ExternalWorkOrder/KwAGVStockIn?externalWorkOrderCode=" + code,
    method: "get",
  });
}

// 发起mes暂停lot
export function kwHoldLot(code) {
  return request({
    url: "/v1/ExternalWorkOrder/KwHoldLot?externalWorkOrderCode=" + code,
    method: "get",
  });
}
