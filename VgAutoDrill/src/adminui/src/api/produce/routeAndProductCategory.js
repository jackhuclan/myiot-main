import request  from "@/utils/request";
 
// 查询工艺关联产品大类列表
export function listRouteAndProductCategory(data) {
  return request({
    url: '/v1/RouteAndProductCategory/GetList',
    method: 'post',
    data,
  })
}

// 查询工艺关联产品大类详细
export function getRouteAndProductCategory(notificationRecordId) {
  return request({
    url: '/v1/RouteAndProductCategory/' + notificationRecordId,
    method: 'get'
  })
}

// 新增工艺关联产品大类
export function addRouteAndProductCategory(data) {
  return request({
    url: '/v1/RouteAndProductCategory',
    method: 'post',
    data,
  })
}

// 修改工艺关联产品大类
export function updateRouteAndProductCategory(data) {
  return request({
    url: '/v1/RouteAndProductCategory',
    method: 'put',
    data,
  })
}

// 删除工艺关联产品大类
export function delRouteAndProductCategory(id) {
  return request({
    url: '/v1/RouteAndProductCategory/' + id,
    method: 'delete'
  })
}

// 多选删除
export function delList(data) {
  return request({
    url: "/v1/RouteAndProductCategory",
    method: "delete",
    data
  });
}

// 查询工序信息
export function listAllProcess() {

}


// 获取树形结构
export function treeSelect() {
  return request({
    url: '/v1/RouteAndProductCategory/GetTreeSelect/',
    method: 'get'
  })
}


// 产品大类页面 （根据产品大类获取工艺路线）

export function getRouteInfoList(data) {
  return request({
    url: '/v1/RouteAndProductCategory/GetRouteInfoList',
    method: 'post',
    data,
  })
}



// 产品大类页面（上移、下移)

export function updateDataOrderNum(data) {
  return request({
    url: '/v1/RouteAndProductCategory/UpdateDataOrderNum',
    method: 'put',
    data,
  })
}

