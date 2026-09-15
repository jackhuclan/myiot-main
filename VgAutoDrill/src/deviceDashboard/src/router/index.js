import Vue from "vue";
import VueRouter from "vue-router";
import HomeView from "../views/home.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "home3",
    component: HomeView,
  }
];

const router = new VueRouter({
  // mode: 'history', // 去掉url中的#
  routes,
});

export default router;
