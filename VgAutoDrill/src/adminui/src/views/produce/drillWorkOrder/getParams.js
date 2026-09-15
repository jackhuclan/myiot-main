// 会话存储workOrderId
export function setWorkOrderId(val) {
  return sessionStorage.setItem("drillWorkOrder--workOrderId", val);
}
// 获取会话存储workOrderId
export function getWorkOrderId() {
  return sessionStorage.getItem("drillWorkOrder--workOrderId");
}
export function setWorkOrderCode(val) {
  return sessionStorage.setItem("drillWorkOrder--workOrderCode", val);
}
// 获取会话存储workOrderCode
export function getWorkOrderCode() {
  return sessionStorage.getItem("drillWorkOrder--workOrderCode");
}

export function setRouteCode(val) {
  return sessionStorage.setItem("drillWorkOrder--routeCode", val);
}
// 获取会话存储routeCode
export function getRouteCode() {
  return sessionStorage.getItem("drillWorkOrder--routeCode");
}

// 会话存储ItemCode
export function setItemCode(val) {
  return sessionStorage.setItem("drillWorkOrder--itemCode", val);
}
// 获取会话存储ItemCode
export function getItemCode() {
  return sessionStorage.getItem("drillWorkOrder--itemCode");
}

// 会话存储isCollapse
export function setIsCollapse(val) {
  return sessionStorage.setItem("drillWorkOrder--isCollapse", val);
}
// 获取会话存储isCollapse
export function getIsCollapse() {
  return sessionStorage.getItem("drillWorkOrder--isCollapse");
}
