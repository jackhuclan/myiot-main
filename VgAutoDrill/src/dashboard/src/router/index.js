import Vue from "vue";
import VueRouter from "vue-router";
import HomeView from "../views/home.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/device",
    name: "device",
    component: () => import("@/views/device.vue"),
  },
  {
    path: "/detail",
    name: "detail",
    component: () => import("@/views/detail.vue"),
  },
];

const router = new VueRouter({
  // mode: 'history', // 去掉url中的#
  routes,
});

export default router;
