import Vue from "vue";
import Vuex from "vuex";
import app from "./modules/app";
import dict from "./modules/dict";
import user from "./modules/user";
import tagsView from "./modules/tagsView";
import permission from "./modules/permission";
import settings from "./modules/settings";
import personalized from "./modules/personalized";
import ganttChar from "./modules/ganttChar";
import device from "./modules/device";
import getters from "./getters";

Vue.use(Vuex);

const store = new Vuex.Store({
  state: {
    loading: true,
  },
  mutations: {
    SETSTATE(state, payload) {
      state[payload.key] = payload.value;
    },
  },
  actions: {
    changeLoading({ commit }, payload) {
      commit("SETSTATE", { key: "loading", value: payload });
    },
  },
  modules: {
    app,
    dict,
    user,
    tagsView,
    permission,
    settings,
    ganttChar,
    device,
    personalized,
  },
  getters,
});

export default store;
