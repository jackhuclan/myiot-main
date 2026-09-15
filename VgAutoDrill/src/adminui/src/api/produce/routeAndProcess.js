import request  from "@/utils/request";
 
// 查询工艺关联工序大类列表
export function listRouteAndProcess(data) {
  return request({
    url: '/v1/RouteAndProcess/GetList',
    method: 'post',
    data,
  })
}

// 查询工艺关联工序大类详细
export function getRouteAndProcess(notificationRecordId) {
  return request({
    url: '/v1/RouteAndProcess/' + notificationRecordId,
    method: 'get'
  })
}

// 新增工艺关联工序大类
export function addRouteAndProcess(data) {
  return request({
    url: '/v1/RouteAndProcess',
    method: 'post',
    data,
  })
}

// 修改工艺关联工序大类
export function updateRouteAndProcess(data) {
  return request({
    url: '/v1/RouteAndProcess',
    method: 'put',
    data,
  })
}

// 删除工艺关联工序大类
export function delRouteAndProcess(id) {
  return request({
    url: '/v1/RouteAndProcess/' + id,
    method: 'delete'
  })
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/RouteAndProcess",
    method: "delete", 
    data
  });
}

// 查询工序信息
export function listAllProcess(data){
  return request({
    url: '/v1/Process/GetList',
    method: 'post',
    data
  })
}


// 获取树形结构
export function treeSelect(){
  return request({
    url: '/v1/RouteAndProcess/GetTreeSelect/',
    method: 'get'
  })
}


