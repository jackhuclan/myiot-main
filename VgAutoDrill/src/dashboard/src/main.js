import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
// 将自动注册所有组件为全局组件
import dataV from "@jiaminghi/data-view";
import {
  Badge,
  Button,
  Row,
  Col,
  Progress,
  Tooltip,
  Carousel,
  CarouselItem,
} from "element-ui";
import "element-ui/lib/theme-chalk/index.css";
import * as echarts from "echarts";
import drawMixin from "@/mixins/drawMixin";
import lazy from "@/directives";
// 定义懒加载图片，自定义指令
Vue.directive("lazy", lazy);
// 将echarts挂在Vue原型上
Vue.prototype.$echarts = echarts;
// mixin混入自适应分辨率缩放
Vue.mixin(drawMixin);
Vue.use(Button);
Vue.use(Row);
Vue.use(Col);
Vue.use(Progress);
Vue.use(Tooltip);
Vue.use(Carousel);
Vue.use(CarouselItem);
Vue.use(Badge);
Vue.use(dataV);
Vue.config.productionTip = false;
new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");
