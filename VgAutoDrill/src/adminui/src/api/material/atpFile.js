import request  from "@/utils/request";
 
// 查询atp文件列表
export function listItemAtpFile(data) {
  return request({
    url: "/v1/ItemAtpFile/GetList",
    method: "Post",
    data,
  });
}

// 查询atp文件详细
export function getItemAtpFile(ItemAtpFileId) {
  return request({
    url: "/v1/ItemAtpFile/" + ItemAtpFileId,
    method: "get",
  });
}

// 新增atp文件
export function addItemAtpFile(data) {
  return request({
    url: "/v1/ItemAtpFile",
    method: "post",
    data,
  });
}

// 修改atp文件
export function updateItemAtpFile(data) {
  return request({
    url: "/v1/ItemAtpFile",
    method: "put",
    data,
  });
}

// 删除atp文件
export function delItemAtpFile(id) {
  return request({
    url: "/v1/ItemAtpFile/" + id,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/ItemAtpFile",
    method: "delete",
    data,
  });
}

// 上传文件/v1/CutterConfigMaster/UpLoad
export function upLoad(data) {

  return request({
    url: "/v1/CutterConfigMaster/UpLoad",
    method: "post",
    headers: {
      "Content-Type": "multipart/form-data",
    },
    data,
  });
}

// 下载ATP文件/v1/ItemAtpFile/DownLoad
export function downLoadATPFile(query) {
  return request({
    responseType: 'blob',
    url: "/v1/ItemAtpFile/DownLoad?fileName=" + query,
    method: "get",
    headers: {
      "Content-Type": "text/html; charset=utf-8",
    },
  });
}


// 生产ATP文件
export function generateATP(query) {
  return request({
    url: "/v1/ItemAtpFile/GenerateATP?id=" + query,
    method: "get",
    // headers: {
    //   "Content-Type": "text/html; charset=utf-8",
    // },
  });
}