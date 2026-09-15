const state = {
  time: 8, //默认8小时
};
const mutations = {
  // 改变查看时间段
  CHANGE_TIME: (state, value) => {
    state.time = value;
  },
};

export default {
  // 开启命名空间
  namespaced: true,
  state,
  mutations,
};
