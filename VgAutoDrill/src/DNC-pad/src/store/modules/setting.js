const setting = {
  state: {
    deviceIsAuto: uni.getStorageSync("uni-deviceIsAuto")
      ? uni.getStorageSync("uni-deviceIsAuto")
      : false,
    deviceCode: uni.getStorageSync("uni-deviceCode")
      ? uni.getStorageSync("uni-deviceCode")
      : "",
    rawLocationCodes: uni.getStorageSync("uni-rawLocationCodes")
      ? JSON.parse(uni.getStorageSync("uni-rawLocationCodes"))
      : [],
    clinkerLocationCodes: uni.getStorageSync("uni-clinkerLocationCodes")
      ? JSON.parse(uni.getStorageSync("uni-clinkerLocationCodes"))
      : [],
  },

  mutations: {
    SET_DEVICEISAUTO: (state, item) => {
      state.deviceIsAuto = item;
    },
    SET_DEVICECODE: (state, item) => {
      state.deviceCode = item;
    },
    SET_RAWLOCATIONCODES: (state, list) => {
      state.rawLocationCodes = list;
    },
    SET_CLINKERLOCATIONCODES: (state, list) => {
      state.clinkerLocationCodes = list;
    },
  },

  actions: {
    // 当前钻机是否自动
    changeIsAuto({ commit }, IsAuto) {
      uni.setStorageSync("uni-deviceIsAuto", IsAuto);
      commit("SET_DEVICEISAUTO", IsAuto);
    },
    // 当前钻机
    changeCode({ commit }, code) {
      uni.setStorageSync("uni-deviceCode", code);
      commit("SET_DEVICECODE", code);
    },
    // 生料库位
    changeRawLocationCodes({ commit }, list) {
      uni.setStorageSync("uni-rawLocationCodes", JSON.stringify(list));
      commit("SET_RAWLOCATIONCODES", list);
    },
    // 熟料库位
    changeClinkerLocationCodes({ commit }, list) {
      uni.setStorageSync("uni-clinkerLocationCodes", JSON.stringify(list));
      commit("SET_CLINKERLOCATIONCODES", list);
    },
  },
};

export default setting;
