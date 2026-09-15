import router, { constantRoutes, dynamicRoutes } from "@/router"; 

const permission = {
  state: {
    routes: [],
    addRoutes: [],
    defaultRoutes: [],
    topbarRouters: [],
    sidebarRouters: [],
  },
  mutations: {
    SET_ROUTES: (state, routes) => {
      state.addRoutes = routes;
      state.routes = constantRoutes.concat(routes);
    },
    SET_DEFAULT_ROUTES: (state, routes) => {
      state.defaultRoutes = constantRoutes.concat(routes);
    },
    SET_TOPBAR_ROUTES: (state, routes) => {
      state.topbarRouters = routes;
    },
    SET_SIDEBAR_ROUTERS: (state, routes) => {
      state.sidebarRouters = routes;
    },
  },
  actions: {
    // 生成路由
    GenerateRoutes({ commit }) {
      return new Promise((resolve) => {
        commit("SET_SIDEBAR_ROUTERS", constantRoutes.concat(dynamicRoutes));
        commit("SET_DEFAULT_ROUTES", dynamicRoutes);
        commit("SET_TOPBAR_ROUTES", dynamicRoutes);
        resolve(constantRoutes.concat(dynamicRoutes));
      });
    },
  },
};

 
export default permission;
