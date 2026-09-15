import Vue from "vue";
import Cookies from "js-cookie";
import Element from "element-ui";

import "./assets/styles/element-variables.scss";
import "@/assets/styles/index.scss"; // global css
import "@/assets/styles/ruoyi.scss"; // ruoyi css
import App from "./App";
import store from "./store";
import router from "./router";
import mixins from "./mixins";
import directive from "./directive"; // directive
import plugins from "./plugins"; // plugins
import { exportExcel } from "@/utils/request";
import "./permission";
import "./assets/icons"; // icon
import { parseTime, resetForm, handleTree } from "@/utils/common";
// 分页组件
import Pagination from "@/components/Pagination";
// 自定义表格工具组件
import RightToolbar from "@/components/RightToolbar";
// 头部标签组件
import VueMeta from "vue-meta";
import borderTitle from "@/components/BorderTitle";

Vue.mixin(mixins);
// 全局方法挂载
// 时间日期格式化
Vue.prototype.parseTime = parseTime;
// 重置表单
Vue.prototype.resetForm = resetForm;
// 构造树型结构数据
Vue.prototype.handleTree = handleTree;
// 导出文件
Vue.prototype.exportExcel = exportExcel;
// 全局组件挂载
Vue.component("Pagination", Pagination);
Vue.component("RightToolbar", RightToolbar);
Vue.component("borderTitle", borderTitle);

Vue.use(directive);
Vue.use(plugins);
Vue.use(VueMeta); 
Vue.use(Element, {
  size: Cookies.get("size") || "medium",
});

Vue.config.productionTip = false;

new Vue({
  el: "#app",
  router,
  store,
  render: (h) => h(App),
});
