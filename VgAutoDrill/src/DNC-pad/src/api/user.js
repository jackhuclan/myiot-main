import request from "@/utils/request";
// 登录方法
export function login(data) {
  return request({
    url: "/v1/User/Login",
    header: {
      "Content-Type": "application/json",
    },
    headers: {
      isToken: false,
    },
    method: "post",
    data: data,
  });
}

// 获取用户详细信息
export function getInfo() {
  return request({
    url: "/v1/User/GetInfo",
    method: "get",
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
    url: "/v1/User/PutSimpleInfo",
    method: "put",
    data,
  });
}

// 退出方法
export function logout() {
  return request({
    url: "/v1/User/Logout",
    method: "post",
  });
}
