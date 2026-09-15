import request from "@/utils/request";
import { parseStrEmpty } from "@/utils/ruoyi";

// 获取路由
export const getRouters = () => {
  return request({
    url: '/v1/User/GetRouters',
    method: 'get',
  })
}
// 用户管理页面
// 查询用户列表
export function listUser(data) {
  return request({
    url: "/v1/User/GetPageList",
    method: "post",
    data,
  });
}

// 查询用户详细
export function getUser(userId) {
  return request({
    url: "/v1/User/" + parseStrEmpty(userId),
    method: "get",
  });
}

// 新增用户
export function addUser(data) {
  return request({
    url: "/v1/User/",
    method: "post",
    data,
  });
}

// 修改用户
export function updateUser(data) {
  return request({
    url: "/v1/User",
    method: "put",
    data,
  });
}

// 删除用户
export function delUser(userId) {
  return request({
    url: "/v1/User/" + userId,
    method: "delete",
  });
}
// 多选删除
export function delList(data) {
  return request({
    url: "/v1/User",
    method: "delete",
    data,
  });
}
// 管理员用户密码重置
export function resetUserPwd(data) {
  return request({
    url: "/v1/User/ResetPwdByAdmin",
    method: "put",
    data,
  });
}

// 用户状态修改
export function changeUserStatus(userId, status) {
  const data = {
    userId,
    status,
  };
  return request({
    url: "/v1/User/ChangeStatus",
    method: "put",
    data,
  });
}

// 查询用户个人信息
export function getUserProfile() {
  return request({
    url: "/v1/User/GetInfo",
    method: "get",
  });
}

// 修改用户个人信息
export function updateUserProfile(data) {
  return request({
    url: "v1/User/PutSimpleInfo",
    method: "put",
    data,
  });
}
// 个人中心密码重置
export function updateUserPwd(data) {
  return request({
    url: "/v1/User/ResetPwd",
    method: "put",
    data,
  });
}

// 用户头像上传
export function uploadAvatar(data) {
  return request({
    url: "/system/user/profile/avatar",
    method: "post",
    data,
  });
}

// 查询授权角色
export function getAuthRole(userId) {
  return request({
    url: "/system/user/authRole/" + userId,
    method: "get",
  });
}

// 保存授权角色
export function updateAuthRole(data) {
  return request({
    url: "/system/user/authRole",
    method: "put",
    params: data,
  });
}

// 修改岗位
export function updatePost(data) {
  return request({
    url: "/v1/Position",
    method: "put",
    data,
  });
}
