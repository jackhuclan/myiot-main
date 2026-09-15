import Vue from "vue";
import Vuex from "vuex";
import axios from "axios"; // 引入axios
import { list } from "../mock";
// 引入插件
import createPersistedState from "vuex-persistedstate";
Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    deviceType: "全部",
    deviceId: 11, //暂时默认11
    list: [],
    btnIndex: 0,
    headerList: [],
  },
  getters: {},
  mutations: {
    SETSTATE(state, payload) {
      state[payload.key] = payload.value;
    },
  },
  actions: {
    // 保存当前点击设备类型
    changeDeviceType({ commit }, payload) {
      commit("SETSTATE", { key: "deviceType", value: payload });
    },
    // 保存当前点击设备类型按钮
    changeBtnIndex({ commit }, payload) {
      commit("SETSTATE", { key: "btnIndex", value: payload });
    },
    // 保存当前点击设备id
    changeDeviceId({ commit }, payload) {
      commit("SETSTATE", { key: "deviceId", value: payload });
    },
    getDeviceList({ state, commit }, payload) {
      console.log(state.deviceType, payload);
      const list1 = list
        .filter((v) => {
          if (state.deviceType == "全部") return true;
          return v.title == state.deviceType;
        })
        .filter((v) => {
          if (payload == "总机台" || !payload) return true;
          return v.status == payload;
        });
      commit("SETSTATE", { key: "list", value: list1 });
      commit("SETSTATE", {
        key: "headerList",
        value: [
          {
            title: "在线",
            value: list
              .filter((v) => {
                if (state.deviceType == "全部") return true;
                return v.title == state.deviceType;
              })
              .filter((v) => {
                return v.status == "在线";
              }).length,
          },
          {
            title: "加工中",
            value: list
              .filter((v) => {
                if (state.deviceType == "全部") return true;
                return v.title == state.deviceType;
              })
              .filter((v) => {
                return v.status == "加工中";
              }).length,
          },
          {
            title: "报警中",
            value: list
              .filter((v) => {
                if (state.deviceType == "全部") return true;
                return v.title == state.deviceType;
              })
              .filter((v) => {
                return v.status == "报警中";
              }).length,
          },
          {
            title: "停止中",
            value: list
              .filter((v) => {
                if (state.deviceType == "全部") return true;
                return v.title == state.deviceType;
              })
              .filter((v) => {
                return v.status == "停止中";
              }).length,
          },
          {
            title: "待料中",
            value: list
              .filter((v) => {
                if (state.deviceType == "全部") return true;
                return v.title == state.deviceType;
              })
              .filter((v) => {
                return v.status == "待料中";
              }).length,
          },
          {
            title: "总机台",
            value: list.filter((v) => {
              if (state.deviceType == "全部") return true;
              return v.title == state.deviceType;
            }).length,
          },
        ],
      });
    },
  },
  modules: {},
  /* vuex数据持久化配置 */
  plugins: [
    createPersistedState({
      // 存储方式：localStorage、sessionStorage、cookies
      storage: window.sessionStorage,
      // 存储的 key 的key值
      key: "store",
      render(state) {
        // 要存储的数据：本项目采用es6扩展运算符的方式存储了state中所有的数据
        return {
          deviceType: state.deviceType,
          deviceId: state.deviceId,
          btnIndex: state.btnIndex,
        };
      },
    }),
  ],
});
