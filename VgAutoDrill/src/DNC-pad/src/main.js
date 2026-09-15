import App from "./App";
import plugins from "./plugins"; // plugins
import store from "./store"; // store
import directive from "./directive"; // directive
import combox from "@components/combox";
// #ifndef VUE3
import Vue from "vue";
import "./uni.promisify.adaptor";
Vue.config.productionTip = false;
Vue.prototype.$store = store;
// 全局组件挂载
Vue.component("combox", combox);
App.mpType = "app";
Vue.use(plugins);
Vue.use(directive);

const app = new Vue({
  ...App,
});
app.$mount();
// #endif

// #ifdef VUE3
import { createSSRApp } from "vue";
export function createApp() {
  const app = createSSRApp(App);
  return {
    app,
  };
}
// #endif
