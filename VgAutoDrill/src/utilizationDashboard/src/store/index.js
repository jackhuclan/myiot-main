import Vue from "vue";
import Vuex from "vuex";
import axios from "axios"; // 引入axios
// 引入插件
import createPersistedState from "vuex-persistedstate";
Vue.use(Vuex);

export default new Vuex.Store({
  state: {},
  getters: {},
  mutations: {},
  actions: {},
  modules: {},
  /* vuex数据持久化配置 */
  plugins: [],
});
