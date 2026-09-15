import request from "@/utils/request";



// 获取设备与工艺路线关联列表
export function listDeviceAndRoute(data) {
    return request({
        url: "/v1/DeviceAndRoute/GetList",
        method: "post",
        data,
    });
}

// 获取设备与工艺路线关联
export function getDeviceAndRoute(DeviceAndRouteId) {
    return request({
        url: "/v1/DeviceAndRoute/" + DeviceAndRouteId,
        method: "get",
    });
}
// 添加设备关联路线
export function addDeviceAndRoute(data) {
    return request({
        url: "/v1/DeviceAndRoute",
        method: "post",
        data
    });
}

// 删除设备关联路线
export function delDeviceAndRoute(DeviceAndRouteId) {
    return request({
        url: "/v1/DeviceAndRoute/" + DeviceAndRouteId,
        method: "delete",
    });
}

// 多选删除
export function delList(data) {
    return request({
        url: "/v1/DeviceAndRoute",
        method: "delete",
        data,
    });
}
