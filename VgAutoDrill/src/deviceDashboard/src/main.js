import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
// 将自动注册所有组件为全局组件
import dataV from "@jiaminghi/data-view";
import VScaleScreen from "v-scale-screen";
import dayjs from "dayjs";
import "dayjs/locale/zh-cn"; // 导入中文语言包
import weekOfYear from "dayjs/plugin/weekOfYear";
import weekday from "dayjs/plugin/weekday";

import { Progress, Button } from "element-ui";
import "element-ui/lib/theme-chalk/index.css";
import * as echarts from "echarts";
import lazy from "@/directives";
Vue.use(VScaleScreen);
// 定义懒加载图片，自定义指令
Vue.directive("lazy", lazy);
// 将echarts挂在Vue原型上
Vue.prototype.$echarts = echarts;
Vue.use(Progress);

Vue.use(Button);
Vue.use(dataV);
Vue.config.productionTip = false;

dayjs.extend(weekOfYear);
dayjs.extend(weekday);
dayjs.locale("zh-cn"); // 设置语言为中文
new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");
