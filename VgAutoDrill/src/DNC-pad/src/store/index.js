import Vue from "vue";
import Vuex from "vuex";
import app from "./modules/app.js";
import error from "./modules/error.js";
import user from "./modules/user.js";
import getters from "./getters";
import setting from "./modules/setting.js";
Vue.use(Vuex);

const store = new Vuex.Store({
  modules: {
    app,
    error,
    user,
    setting,
  },
  getters,
});

export default store;
