import { getToken } from "@/utils/auth";
import errorCode from "@/utils/errorCode";
import { toast, showConfirm, tansParams } from "@/utils/common";
import store from "@/store";

let timeout = 10000;
const baseUrl = process.env.VUE_APP_BASE_API;
// 是否显示重新登录
export let isRelogin = { show: false };
const request = (config) => {
  // 是否需要设置 token
  const isToken = (config.headers || {}).isToken === false;
  config.header = config.header || {};
  if (getToken() && !isToken) {
    config.header["Authorization"] = "Bearer " + getToken();
  }
  // get请求映射params参数
  if (config.params) {
    let url = config.url + "?" + tansParams(config.params);
    url = url.slice(0, -1);
    config.url = url;
  }
  return new Promise((resolve, reject) => {
    uni
      .request({
        method: config.method || "get",
        timeout: config.timeout || timeout,
        url: config.baseUrl || baseUrl + config.url,
        data: config.data,
        header: config.header,
        dataType: "json",
      })
      .then((response) => {
        let { data, statusCode } = response;
        const code = statusCode || 200;
        const msg = errorCode[code] || data.msg || errorCode["default"];
        if (code === 401) {
          if (!isRelogin.show) {
            isRelogin.show = true;
            showConfirm(
              "登录状态已过期，您可以继续留在该页面，或者重新登录?"
            ).then((res) => {
              if (res.confirm) {
                console.log("用户点击确定");
                isRelogin.show = false;

                store.dispatch("LogOut").then(() => {
                  uni.reLaunch({
                    url: "/pages/login/login",
                  });
                });
              } else if (res.cancel) {
                isRelogin.show = false;
                console.log("用户点击取消");
              }
            });
          }

          reject("无效的会话，或者会话已过期，请重新登录。");
        } else if (code === 500) {
          toast(msg);
          reject("500");
        } else if (code !== 200) {
          toast(msg);
          reject(code);
        }
        resolve(data);
      })
      .catch((error) => {
        console.log(error);
        // // // 对请求错误做些什么
        // // console.log(error, 'error');
        // let { message } = error;
        // if (message === "Network Error") {
        //   message = "后端接口连接异常";
        // } else if (message.includes("timeout")) {
        //   message = "系统接口请求超时";
        // } else if (message.includes("Request failed with status code")) {
        //   message = "系统接口" + message.substr(message.length - 3) + "异常";
        // }
        // toast(message);
        reject(error);
      });
  });
};

export default request;
