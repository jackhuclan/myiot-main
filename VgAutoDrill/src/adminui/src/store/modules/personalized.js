import defaultPersonalized from "@/personalized";

const {
  form: {
    alarmInfoTime,
    onlineDevice_detailInterval,
    schedule_detailInterval,
    warehouse_dashboard_interval,
    version_updates_interval,
    maxShowFormItem,
    utilizationRate_dashboard_interval,
    drill_dashboard_Interval,
    schedule_dashboard_interval,
    agv_dashboard_interval,
  },
} = defaultPersonalized;
const storagePersonalized =
  JSON.parse(localStorage.getItem("layout-personalized")) || "";
const state = {
  alarmInfoTime:
    storagePersonalized.alarmInfoTime == undefined
      ? alarmInfoTime
      : storagePersonalized.alarmInfoTime,
  drill_dashboard_Interval:
    storagePersonalized.drill_dashboard_Interval == undefined
      ? drill_dashboard_Interval
      : storagePersonalized.drill_dashboard_Interval,

  utilizationRate_dashboard_interval:
    storagePersonalized.utilizationRate_dashboard_interval == undefined
      ? utilizationRate_dashboard_interval
      : storagePersonalized.utilizationRate_dashboard_interval,

  onlineDevice_detailInterval:
    storagePersonalized.onlineDevice_detailInterval == undefined
      ? onlineDevice_detailInterval
      : storagePersonalized.onlineDevice_detailInterval,
  schedule_detailInterval:
    storagePersonalized.schedule_detailInterval == undefined
      ? schedule_detailInterval
      : storagePersonalized.schedule_detailInterval,
  warehouse_dashboard_interval:
    storagePersonalized.warehouse_dashboard_interval == undefined
      ? warehouse_dashboard_interval
      : storagePersonalized.warehouse_dashboard_interval,
  version_updates_interval:
    storagePersonalized.version_updates_interval == undefined
      ? version_updates_interval
      : storagePersonalized.version_updates_interval,
  maxShowFormItem:
    storagePersonalized.maxShowFormItem == undefined
      ? maxShowFormItem
      : storagePersonalized.maxShowFormItem,
  schedule_dashboard_interval:
    storagePersonalized.schedule_dashboard_interval == undefined
      ? schedule_dashboard_interval
      : storagePersonalized.schedule_dashboard_interval,
  agv_dashboard_interval:
    storagePersonalized.agv_dashboard_interval == undefined
      ? agv_dashboard_interval
      : storagePersonalized.agv_dashboard_interval,
};
const mutations = {
  CHANGE_SETTING: (state, { key, value }) => {
    if (state.hasOwnProperty(key)) {
      state[key] = value;
    }
  },
};

const actions = {
  // 修改布局设置
  changePersonalized({ commit }, data) {
    commit("CHANGE_SETTING", data);
  },
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
};
