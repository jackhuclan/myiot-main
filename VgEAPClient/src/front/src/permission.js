import router from "./router";
import store from "./store";
import NProgress from "nprogress";
import "nprogress/nprogress.css";
NProgress.configure({ showSpinner: false });

router.beforeEach((to, from, next) => {
  NProgress.start();
  to.meta.title && store.dispatch("settings/setTitle", to.meta.title);
  if (to.params.code) {
    // 点击设备跳转详情页修改title,tagsView页面
    to.meta.title = to.params.code + " 的详情";
  }
  next();
});

router.afterEach(() => {
  NProgress.done();
});
