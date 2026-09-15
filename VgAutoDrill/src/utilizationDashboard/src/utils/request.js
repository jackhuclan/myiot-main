import axios from "axios";
import { Message } from "element-ui";
axios.defaults.headers["Content-Type"] = "application/json;charset=utf-8";
// 创建axios实例
const service = axios.create({
  // axios中请求配置有baseURL选项，表示请求URL公共部分
  baseURL: process.env.VUE_APP_BASE_API,
  // 超时
  timeout: 5000,
});

// 添加请求拦截器
service.interceptors.request.use(
  function (config) {
    // 在发送请求之前做些什么
    return config;
  },
  function (error) {
    // 对请求错误做些什么
    return Promise.reject(error);
  }
);

// 响应拦截器
service.interceptors.response.use(
  (response) => {
    // 对响应数据做点什么
    const res = response.data;
    // 二进制数据则直接返回
    if (
      response.request.responseType === "blob" ||
      response.request.responseType === "arraybuffer"
    ) {
      return res;
    }
    return res;
  },
  (error) => {
    // 对响应错误做点什么
    // console.log(error, isMessage); // for debug
    const { message } = error;
    let msg = "";
    // 登录过期
    if (message.includes("Request failed with status code")) {
      const num = message.substr(message.length - 3);
      if (num == 401) {
        msg = undefined;
      } else {
        msg = "系统接口" + num + "异常";
      }
    }
    if (message == "Network Error") {
      msg = "后端接口连接异常";
    } else if (message.includes("timeout")) {
      msg = "系统接口请求超时,请稍后手动刷新浏览器";
    }
    // msg &&
    //   Message({
    //     message: msg,
    //     type: "error",
    //     duration: 2000,
    //   });
    msg&&console.log(msg)
    return Promise.reject(error);
  }
);
export default service;
