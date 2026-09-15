import Cookies from "js-cookie";
// #ifdef APP
const TokenKey = "APP-Token";
// #endif
// #ifdef H5
const TokenKey = "H5-Token";
// #endif
export function getToken() {
  // #ifdef APP
  return uni.getStorageSync(TokenKey);
  // #endif
  // #ifdef H5
  return Cookies.get(TokenKey);
  // #endif
}

export function setToken(token) {
  // #ifdef APP
  return uni.setStorageSync(TokenKey, token);
  // #endif
  // #ifdef H5
  return Cookies.set(TokenKey, token);
  // #endif
}

export function removeToken() {
  // #ifdef APP
  return uni.removeStorageSync(TokenKey);
  // #endif
  // #ifdef H5
  return Cookies.remove(TokenKey);
  // #endif
}
