const state = {
  device: "",
};
const mutations = {
  // 保存device
  SET_DEVICE: (state, { key, value }) => {
    state[key] = value;
  },
};

const actions = {
  // 设置device
  setDevice({ commit }, value) {
    commit("SET_DEVICE", { key: "device", value });
  },
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
};
