import Vue from "vue";
import Router from "vue-router";

Vue.use(Router);

/* Layout */
import Layout from "@/layout";

/**
 * Note: 路由配置项
 *
 * hidden: true                     // 当设置 true 的时候该路由不会再侧边栏出现 如401，login等页面，或者如一些编辑页面/edit/1
 * alwaysShow: true                 // 当你一个路由下面的 children 声明的路由大于1个时，自动会变成嵌套的模式--如组件页面
 *                                  // 只有一个时，会将那个子路由当做根路由显示在侧边栏--如引导页面
 *                                  // 若你想不管路由下面的 children 声明的个数都显示你的根路由
 *                                  // 你可以设置 alwaysShow: true，这样它就会忽略之前定义的规则，一直显示根路由
 * redirect: noRedirect             // 当设置 noRedirect 的时候该路由在面包屑导航中不可被点击
 * name:'router-name'               // 设定路由的名字，一定要填写不然使用<keep-alive>时会出现各种问题
 * query: '{"id": 1, "name": "ry"}' // 访问路由的默认传递参数
 * roles: ['admin', 'common']       // 访问路由的角色权限
 * permissions: ['a:a:a', 'b:b:b']  // 访问路由的菜单权限
 * meta : {
    noCache: true                   // 如果设置为true，则不会被 <keep-alive> 缓存(默认 false)
    title: 'title'                  // 设置该路由在侧边栏和面包屑中展示的名字
    icon: 'svg-name'                // 设置该路由的图标，对应路径src/assets/icons/svg
    breadcrumb: false               // 如果设置为false，则不会在breadcrumb面包屑中显示
    activeMenu: '/system/user'      // 当路由设置了该属性，则会高亮相对应的侧边栏。
  }
 */

// 公共路由
export const constantRoutes = [
  {
    path: "/redirect",
    component: Layout,
    hidden: true,
    children: [
      {
        path: "/redirect/:path(.*)",
        component: () => import("@/views/redirect"),
      },
    ],
  },
  {
    path: "/404",
    component: () => import("@/views/error/404"),
    hidden: true,
  },
  {
    path: "/401",
    component: () => import("@/views/error/401"),
    hidden: true,
  },
  {
    path: "/500",
    component: require(`@/views/error/500`).default,
    hidden: true,
  },
  {
    path: "/",
    component: Layout,
    redirect: "index",
    children: [
      {
        path: "index",
        component: () => import("@/views/index"),
        name: "Index",
        meta: { title: "首页", icon: "dashboard", affix: true },
      },
    ],
  },

  { path: "*", redirect: "/404", hidden: true },
  {
    path: "/detail",
    component: Layout,
    hidden: true,
    children: [
      {
        path: ":code",
        component: () => import("@/components/deviceDetail"),
        name: "Detail",
        meta: { title: "详情", activeMenu: "/index" },
      },
    ],
  },
];

// 动态路由，基于用户权限动态去加载
export const dynamicRoutes = [
  {
    name: "duty",
    path: "/duty",
    hidden: false,
    redirect: "noRedirect",
    component: Layout,
    alwaysShow: true,
    meta: {
      title: "稼动率",
      icon: "utilizationRate",
      noCache: false,
    },
    children: [
      {
        name: "dutyList",
        path: "dutyList",
        hidden: false,
        component: () => import("@/views/duty/dutyList"),
        alwaysShow: false,
        meta: {
          title: "标准稼动率",
          icon: "#",
          noCache: false,
        },
      },
      {
        name: "realTimeDutylist",
        path: "realTimeDutylist",
        hidden: false,
        component: () => import("@/views/duty/realTimeDutylist"),
        alwaysShow: false,
        meta: {
          title: "实时稼动率",
          icon: "#",
          noCache: false,
        },
      },
      {
        name: "shiftDutyList",
        path: "shiftDutyList",
        hidden: false,
        component: () => import("@/views/duty/shiftDutyList"),
        alwaysShow: false,
        meta: {
          title: "班次稼动率",
          icon: "#",
          noCache: false,
        },
      },
      {
        name: "analysisDutyList",
        path: "analysisDutyList",
        hidden: false,
        component: () => import("@/views/duty/analysisDutyList"),
        alwaysShow: false,
        meta: {
          title: "稼动率分析",
          icon: "#",
          noCache: false,
        },
      },
    ],
  },
  {
    alwaysShow: false,
    path: "",
    component: Layout,
    redirect: "workCondition",
    meta: { icon: "chart" },
    children: [
      {
        path: "workCondition",
        component: () => import("@/views/workCondition"),
        name: "workCondition",
        meta: { title: "工况", icon: "chart" },
      },
    ],
  },

  {
    alwaysShow: false,
    path: "",
    component: Layout,
    redirect: "userDirective",
    meta: { icon: "table" },
    children: [
      {
        path: "userDirective",
        component: () => import("@/views/userDirective"),
        name: "userDirective",
        meta: { title: "用户指令", icon: "dict" },
      },
    ],
  },
  {
    name: "broken",
    path: "/broken",
    hidden: false,
    redirect: "noRedirect",
    component: Layout,
    alwaysShow: true,
    meta: {
      title: "断刀",
      icon: "tab",
      noCache: false,
    },
    children: [ 
      {
        name: "brokenlist",
        path: "brokenlist",
        hidden: false,
        component: () => import("@/views/broken/brokenlist"),
        alwaysShow: false,
        meta: {
          title: "实时断刀",
          icon: "#",
          noCache: false,
        },
      },
      {
        name: "brokenendlist",
        path: "brokenendlist",
        hidden: false,
        component: () => import("@/views/broken/brokenendlist"),
        alwaysShow: false,
        meta: {
          title: "断刀信息",
          icon: "#",
          noCache: false,
        },
      },
    ],
  },
  {
    alwaysShow: false,
    path: "",
    component: Layout,
    redirect: "alarm",
    meta: { icon: "alarm" },
    children: [
      {
        path: "alarm",
        component: () => import("@/views/alarm"),
        name: "alarm",
        meta: { title: "报警信息", icon: "alarm" },
      },
    ],
  },
  {
    name: "utilizationRate",
    path: "/event",
    hidden: false,
    redirect: "noRedirect",
    component: Layout,
    alwaysShow: true,
    meta: {
      title: "日志",
      icon: "utilizationRate",
      noCache: false,
    },
    children: [
      {
        name: "eventList",
        path: "eventList",
        hidden: false,
        component: () => import("@/views/event/eventList"),
        alwaysShow: false,
        meta: {
          title: "事件日志",
          icon: "#",
          noCache: false,
        },
      },
      {
        name: "m54tList",
        path: "m54tList",
        hidden: false,
        component: () => import("@/views/event/m54List"),
        alwaysShow: false,
        meta: {
          title: "m54日志",
          icon: "#",
          noCache: false,
        },
      },
    ],
  },
];
// 防止连续点击多次路由报错
let routerPush = Router.prototype.push;
Router.prototype.push = function push(location) {
  return routerPush.call(this, location).catch((err) => err);
};

export default new Router({
  mode: process.env.NODE_ENV == "production" ? "hash" : "history", // 去掉url中的#
  // base: "/dist",
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes.concat(dynamicRoutes),
});
