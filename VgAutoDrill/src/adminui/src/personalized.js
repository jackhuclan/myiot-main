const list = [
  {
    name: "alarmInfoTime",
    label: "告警即时信息定时刷新(秒) :",
  },
  {
    name: "onlineDevice_detailInterval",
    label: "在线设备详情定时刷新(秒) :",
  },
  {
    name: "schedule_detailInterval",
    label: "调度记录详情定时刷新(秒) :",
  },
  {
    name: "schedule_dashboard_interval",
    label: "调度大屏定时刷新(秒) :",
  },
  {
    name: "warehouse_dashboard_interval",
    label: "库位相关看板定时刷新(秒) :",
  },
  {
    name: "utilizationRate_dashboard_interval",
    label: "稼动率大屏定时刷新(秒) :",
  },
  {
    name: "agv_dashboard_interval",
    label: "AGV看板定时刷新(秒) :",
  },
  {
    name: "drill_dashboard_Interval",
    label: "钻机看板定时刷新(秒) :",
  },

  {
    name: "version_updates_interval",
    label: "版本更新弹框定时刷新(秒) :",
  },
  {
    name: "maxShowFormItem",
    label: "查询表单默认展示条件数量(个):",
  },
];
const form = {
  /**
   * 告警即时信息定时刷新(默认30s)
   */
  alarmInfoTime: 30,
  /**
   * 在线设备详情定时刷新(默认5s)
   */
  onlineDevice_detailInterval: 5,
  /**
   * 调度大屏定时刷新(默认15s)
   */
  schedule_dashboard_interval:15,
  /**
   * 调度记录详情定时刷新(默认5s)
   */
  schedule_detailInterval: 5,

  /**
   * 库位相关看板定时刷新(默认15s)
   * 库位相关看板定时刷新(默认15s)
   */
  warehouse_dashboard_interval:15,

  // 版本更新弹框定时刷新(30分钟)
  version_updates_interval:1800,
  // 查询表单默认展示条件数量
  maxShowFormItem: 2,
  // 稼动率大屏(默认15s)
  utilizationRate_dashboard_interval: 15,
  // 钻机看板(默认15s)
  drill_dashboard_Interval: 15,
  /**
   * AGV看板(默认15s)
   */
  agv_dashboard_interval: 15,
};
module.exports = {
  list,
  form,
};
