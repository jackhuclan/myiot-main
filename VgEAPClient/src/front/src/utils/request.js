import axios from "axios";
import { Loading } from "element-ui";
import { tansParams } from "@/utils/common";
import router from "@/router";

axios.defaults.headers["Content-Type"] = "application/json;charset=utf-8";
// 创建axios实例
const service = axios.create({
  // axios中请求配置有baseURL选项，表示请求URL公共部分
  baseURL: process.env.VUE_APP_BASE_API,
  //请求超时时间
  timeout: 5000,
});

// request拦截器
service.interceptors.request.use(
  (config) => {
    // get请求映射params参数
    if (config.method === "get" && config.params) {
      let url = config.url + "?" + tansParams(config.params);
      url = url.slice(0, -1);
      config.params = {};
      config.url = url;
    }
    return config;
  },
  (error) => {
    // 对请求错误做些什么
    console.log(error);
    Promise.reject(error);
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
    if (message.includes("Request failed with status code")) {
      const num = message.substr(message.length - 3);
      if (num == 401) {
        msg = undefined;
      } else {
        msg = "系统接口" + num + "异常";
      }
    }
    // 服务器结果都没有返回(可能服务器错误可能客户端断网)，断网处理:可以跳转到断网页面
    if (!window.navigator.onLine) router.push({ path: "/500", replace: true });
    if (message == "Network Error") {
      msg = "后端接口连接异常";
    } else if (message.includes("timeout")) {
      msg = "系统接口请求超时,请稍后手动刷新浏览器";
    }
    msg && console.log(msg);
    return Promise.reject(error);
  }
);
let downloadLoadingInstance;
// 通用的导出方法
export function exportExcel(url, filename, data) {
  downloadLoadingInstance = Loading.service({
    text: "正在导出数据，请稍候",
    spinner: "el-icon-loading",
    background: "rgba(0, 0, 0, 0.7)",
  });
  return service({
    responseType: "blob",
    url,
    method: data ? "post" : "get",
    data,
  })
    .then((res) => {
      const blob = new Blob([res]);
      let href = window.URL.createObjectURL(blob); //创建下载的链接

      if (window.navigator.msSaveBlob) {
        // ie 浏览器
        try {
          window.navigator.msSaveBlob(blob, filename);
        } catch (e) {
          console.log(e);
        }
      } else {
        // 谷歌浏览器 创建a标签 添加download属性下载
        let downloadElement = document.createElement("a");
        if (typeof blob == "string") {
          downloadElement.target = "_blank";
        }

        downloadElement.href = href;
        downloadElement.download = filename;
        document.body.appendChild(downloadElement);
        downloadElement.click(); //点击下载
        document.body.removeChild(downloadElement); //下载完成移除元素
        if (typeof blob != "string") {
          window.URL.revokeObjectURL(href); //释放掉blob对象
        }
      }
      downloadLoadingInstance.close();
    })
    .catch((r) => {
      console.log("导出失败");
      downloadLoadingInstance.close();
      return false;
    });
}

export default service;
