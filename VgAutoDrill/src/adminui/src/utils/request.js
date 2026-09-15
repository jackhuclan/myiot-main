import axios from "axios";
import { Notification, MessageBox, Message, Loading } from "element-ui";
import store from "@/store";
import { getToken } from "@/utils/auth";
import { tansParams } from "@/utils/ruoyi";
import router from "@/router";
let downloadLoadingInstance;
// 是否显示重新登录
export let isRelogin = { show: false };
// 是否显示响应时提示
let isMessageShow = true;
// 不进行错误提示的接口
const urlList = [
  "/v1/BigScreen/GetTitle",
  "/v1/DeviceSchedule/GetDrillDeviceTask",
  "/v1/DeviceSchedule/GetAGVDeviceSiloInfo",
  "/v1/DeviceSchedule/GetTaskByDevice",
  "/v1/DeviceSchedule/GetScheduleList",
  "/v1/DeviceSchedule/GetScheduleLogs/",
  "/v1/DeviceSchedule/GetRunningScheduleList",
  "/v1/DeviceSchedule/GetCentralControlSystemIsMaintaining",
  "/v1/DeviceSchedule/GetWorkOrderList",
  "/v1/DeviceSchedule/GetDeviceList",
  "/v1/DeviceSchedule/GetHomeDeviceData",
  "/v1/BigScreen/GetFeedBackStats",
  "/v1/DeviceSchedule/GetScheduleDeviceStats",
  "/v1/DeviceSchedule/GetScheduleDeviceStatusStats",
];
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
    // 是否需要设置 token
    const isToken = (config.headers || {}).isToken === false;
    // 是否需要防止数据重复提交
    const isRepeatSubmit = (config.headers || {}).repeatSubmit === false;
    if (getToken() && !isToken) {
      config.headers["Authorization"] = "Bearer " + getToken(); // 让每个请求携带自定义token 请根据实际情况自行修改
    }
    if (urlList.includes(config.url)) {
      isMessageShow = false;
    } else {
      isMessageShow = true;
    }
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
    // 登录过期
    if (message.includes("Request failed with status code")) {
      const num = message.substr(message.length - 3);
      if (num == 401) {
        msg = undefined;
        if (!isRelogin.show) {
          isRelogin.show = true;
          MessageBox.confirm(
            "登录状态已过期，您可以继续留在该页面，或者重新登录",
            "系统提示",
            {
              confirmButtonText: "重新登录",
              cancelButtonText: "取消",
              type: "warning",
            }
          )
            .then(() => {
              isRelogin.show = false;
              store.dispatch("LogOut").then(() => {
                //过期情况退出前保存当前路由
                sessionStorage.setItem(
                  "preRoute",
                  router.currentRoute.fullPath
                );
                location.href = "/";
              });
            })
            .catch(() => {
              isRelogin.show = false;
            });
          return Promise.reject("无效的会话，或者会话已过期，请重新登录。");
        }
      } else {
        msg = "系统接口" + num + "异常";
      }
    }
    // 服务器结果都没有返回(可能服务器错误可能客户端断网)，断网处理:可以跳转到断网页面
    if (!window.navigator.onLine) router.push({ path: "/500", replace: true });
    // 部分接口异常不展示
    if (!isMessageShow) return;
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
    msg && console.log(msg);
    return Promise.reject(error);
  }
);

// 通用下载方法
export function download(url, filename) {
  downloadLoadingInstance = Loading.service({
    text: "正在下载数据，请稍候",
    spinner: "el-icon-loading",
    background: "rgba(0, 0, 0, 0.7)",
  });
  return service({
    responseType: "blob",
    url,
    method: "get",
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
      console.error(r);
      Message.error("下载文件出现错误，请联系管理员！");
      downloadLoadingInstance.close();
    });
}
// 通用上传
export function upload(url, data) {
  downloadLoadingInstance = Loading.service({
    text: "正在导入数据，请稍候",
    spinner: "el-icon-loading",
    background: "rgba(0, 0, 0, 0.7)",
  });
  return service
    .post(url, data, {
      headers: { "Content-Type": "multipart/form-data" },
    })
    .then((res) => {
      if (res.code == 0) {
        Message.success(res.data);
      } else {
        Message.error(res.message ? res.message : "请参考模板准备导入数据!");
      }
      downloadLoadingInstance.close();
    })
    .catch((r) => {
      downloadLoadingInstance.close();
    });
}

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
    // 一小时
    timeout: 60 * 60 * 1000,
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
//通用自动生成编码
export function getEncode(data) {
  return service({
    url: "/v1/EncodeBuildRules/GetEncodeList",
    method: "post",
    data,
  });
}
export default service;
