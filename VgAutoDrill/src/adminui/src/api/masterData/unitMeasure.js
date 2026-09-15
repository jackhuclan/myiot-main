import request from "@/utils/request";
// 查询计量单位列表
export function listUnitMeasure(data) {
  return request({
    url: "/v1/UnitMeasure/GetList",
    method: "post",
    data,
  });
}

// 查询主计量单位列表
export function listPrimaryUnitmeasure(id) {
  return request({
    url: "/v1/UnitMeasure/GetPrimaryUnitList?id=" + id,
    method: "get",
  });
}

// 查询计量单位详细
export function getUnitMeasure(Id) {
  return request({
    url: "/v1/UnitMeasure/" + Id,
    method: "get",
  });
}

// 新增计量单位
export function addUnitMeasure(data) {
  return request({
    url: "/v1/UnitMeasure",
    method: "post",
    data,
  });
}

// 修改计量单位
export function updateUnitMeasure(data) {
  return request({
    url: "/v1/UnitMeasure",
    method: "put",
    data,
  });
}

// 删除计量单位
export function delUnitMeasure(id) {
  return request({
    url: "/v1/UnitMeasure/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/UnitMeasure",
    method: "delete",
    data,
  });
}

// 上传文件
// export function upLoad(data) {
//   return request({
//     url: "/v1/UnitMeasure/UploadUnitList",
//     method: "post",
//     headers: {
//       "Content-Type": "multipart/form-data",
//     },
//     data,
//   });
// }

// 获取单位下拉框数据
export function getDropSelectDatas(data) {
  return request({
    url: "/v1/UnitMeasure/getDropSelectDatas",
    method: "post",
    data,
  });
}
