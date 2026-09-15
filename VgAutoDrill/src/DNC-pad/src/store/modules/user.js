import { login, logout, getUserProfile } from "@/api/user";
import { getToken, setToken, removeToken } from "@/utils/auth";

const user = {
  state: {
    token: getToken(),
    name: uni.getStorageSync("userName"),
    userInfo: null || uni.getStorageSync("userInfo"),
  },

  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token;
    },

    SET_NAME: (state, name) => {
      state.name = name;
      uni.setStorageSync("userName", name);
    },
    SET_USERINFO: (state, userInfo) => { 
      state.userInfo = userInfo;
      uni.setStorageSync("userInfo", userInfo);
    },
  },

  actions: {
    // 登录
    Login({ commit }, userInfo) {
      return new Promise((resolve, reject) => {
        login(userInfo)
          .then((res) => {
            if (res.code == 1) return reject(res);
            setToken(res.data);
            commit("SET_TOKEN", res.data);
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      });
    },

    // 获取用户信息
    GetInfo({ commit, state }) {
      return new Promise((resolve, reject) => {
        getUserProfile()
          .then((res) => {
            const user = res.data.user; 
            commit("SET_USERINFO", user);
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      });
    },

    // 退出系统
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        logout(state.token)
          .then(() => {
            commit("SET_TOKEN", "");
            removeToken();
            uni.removeStorageSync("userName");
            resolve();
          })
          .catch((error) => {
            reject(error);
          });
      });
    },
  },
};

export default user;
