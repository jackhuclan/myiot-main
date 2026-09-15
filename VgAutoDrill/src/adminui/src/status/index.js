// 排序方式
const queryOrderByOptions = [
  {
    label: "创建时间正序",
    value: 1,
  },
  {
    label: "创建时间倒序",
    value: 2,
  },
  {
    label: "编码正序",
    value: 3,
  },
  {
    label: "编码倒序",
    value: 4,
  },
];
const deviceRecords_QueryOrderByOptions = [
  {
    label: "数据日期正序",
    value: 1,
  },
  {
    label: "数据日期倒序",
    value: 2,
  },
  {
    label: "设备编码正序",
    value: 3,
  },
  {
    label: "设备编码倒序",
    value: 4,
  },
];
const alarmLevelOptions = [
  {
    value: 1,
    label: "普通",
  },
  {
    value: 2,
    label: "严重",
  },
  {
    value: 3,
    label: "紧急",
  },
];
const alarmKindList = [
  {
    label: "钻机缺少生产任务排产",
    value: 1,
  },
  {
    label: "库位没有发起调度",
    value: 2,
  },
  {
    label: "代理异常",
    value: 3,
  },
  {
    label: "mqtt异常",
    value: 4,
  },
  {
    label: "http异常",
    value: 5,
  },
  {
    label: "agv底盘车故障",
    value: 6,
  },
  {
    label: "钻机故障",
    value: 7,
  },
  {
    label: "钻机buffer故障",
    value: 8,
  },
];
// 设备状态
const deviceStatusOptions = [
  {
    label: "在线",
    value: 0,
    background: "#eaf4ff",
    color: "#4992ff",
    icon: "onlined",
    num: "onlineCount",
  },

  {
    label: "运行中",
    value: 3,
    background: "#eafaf0",
    color: "#56ce66",
    icon: "running",
    num: "runningCount",
  },
  {
    label: "待机",
    value: 2,
    background: "#AFEEEE",
    color: "#00008B",
    icon: "waitting",
    num: "standbyCount",
  },
  {
    label: "故障",
    value: 4,
    background: "#fceded",
    color: "#f05b59",
    icon: "fault",
    num: "warnningCount",
  },
  {
    label: "低电量",
    value: 5,
    background: "#e0e4ed",
    color: "#919399",
    icon: "didianliang",
    num: "lowBatteryCount",
  },
  {
    label: "充电中",
    value: 6,
    background: "#e6e2f6",
    color: "#967ad7",
    icon: "chongdian",
    num: "chargingCount",
  },
  {
    label: "维护中",
    value: 7,
    background: "#FFC2C2",
    color: "#800000",
    icon: "weixiuzhong",
    num: "maintenanceCount",
  },
  {
    label: "离线",
    value: 1,
    background: "#f4f4f5",
    color: "#9a9ca1",
    icon: "offline",
    num: "offlineCount",
  },
];
// 调度状态
const schedulementOptions = [
  {
    value: -1,
    label: "已终止",
    background: "#fceded",
    color: "#f05b59",
  },
  {
    value: -2,
    label: "已取消",
    background: "#e6e2f6",
    color: "#967ad7",
  },
  // {
  //   value: 0,
  //   label: "默认",
  //   background: "#eaf4ff",
  //   color: "#4992ff",
  // },
  {
    value: 1,
    label: "已上报",
    background: "#f4f4f5",
    color: "#9a9ca1",
  },
  {
    value: 2,
    label: "已分配",
    background: "#eaf4ff",
    color: "#4992ff",
  },
  {
    value: 3,
    label: "已开始",
    background: "#fef8e6",
    color: "#f6c222",
  },
  {
    value: 4,
    label: "已完成",
    background: "#eafaf0",
    color: "#56ce66",
  },
  {
    value: 5,
    label: "部分完成",
    background: "rgba(0,255,255,0.1)",
    color: "#008080",
  },
];
// 申请物料种类
// 1-板料，2-刀具，3-板料料仓，4-刀具料仓，0-未指定
const requestMaterialKindOptions = [
  {
    lable: "板料",
    value: 1,
  },
  {
    lable: "未指定",
    value: 0,
  },
  {
    lable: "刀具",
    value: 2,
  },
  {
    lable: "板料料仓",
    value: 3,
  },
  {
    lable: "刀具料仓",
    value: 4,
  },
];

// 设备类别
// 所有
const deviceKinds = [
  {
    value: 2,
    label: "CNC84钻机",
  },
  {
    value: 3,
    label: "CNC95钻机",
  },
  {
    value: 4,
    label: "叠板机",
  },
  {
    value: 5,
    label: "拆板机",
  },
  {
    value: 6,
    label: "后上料AGV",
  },
  {
    value: 7,
    label: "前上料AGV",
  },
  {
    value: 8,
    label: "换刀AGV",
  },
  // 短期用不到的：滚轮AGV、运料AGV屏蔽
  // {
  //   value: 9,
  //   label: "滚轮AGV",
  // },
  // {
  //   value: 10,
  //   label: "运料AGV",
  // },

  {
    value: 13,
    // 板料料架==>公共缓存区
    label: "公共缓存区",
  },
  {
    value: 14,
    label: "刀具料架",
  },
  {
    value: 15,
    // 板料插齿==>中转区
    label: "中转区",
  },
];
//AGV
const agvKindList = [
  {
    value: 6,
    label: "后上料AGV",
  },
  {
    value: 7,
    label: "前上料AGV",
  },
  {
    value: 8,
    label: "换刀AGV",
  },
  {
    value: 9,
    label: "滚轮AGV",
  },
  {
    value: 10,
    label: "运料AGV",
  },
];

// DRILL
const drillKindList = [
  {
    value: 2,
    label: "CNC84钻机",
  },
  {
    value: 3,
    label: "CNC95钻机",
  },
];

// 交互方式
const interactionSequenceOptions = [
  {
    label: "只上",
    value: 0,
  },
  {
    label: "只下",
    value: 1,
  },
  {
    label: "先上再下",
    value: 2,
  },
  {
    label: "先下再上",
    value: 3,
  },
];

// 工单状态
const workOrderOptions = [
  {
    value: 0,
    label: "草稿",
    background: "#fceded",
    color: "#f05b59",
  },
  {
    value: 1,
    label: "已审批",
    background: "#eaf4ff",
    color: "#4992ff",
  },
  {
    value: 2,
    label: "已排产",
    background: "#fef8e6",
    color: "#f6c222",
  },
  {
    value: 3,
    label: "已投产",
    background: "#e6e2f6",
    color: "#967ad7",
  },
  {
    value: 4,
    label: "已完工",
    background: "#eafaf0",
    color: "#56ce66",
  },
];
// 任务状态
const taskOptions = [
  {
    value: 0,
    label: "草稿",
    background: "#fceded",
    color: "#f05b59",
  },
  {
    value: 10,
    label: "已提交",
    background: "#eaf4ff",
    color: "#4992ff",
  },
  {
    value: 20,
    label: "派送中",
    background: "#E6E6FA",
    color: "#8A2BE2",
  },
  {
    value: 30,
    label: "已就位",
    background: "#FFF8DC",
    color: "#800000",
  },
  {
    value: 40,
    label: "已开始",
    background: "#fef8e6",
    color: "#f6c222",
  },
  {
    value: 50,
    label: "已完成",
    background: "#eafaf0",
    color: "#56ce66",
  },
];
// 板料类型
const productStatusOptions = [
  {
    value: 1,
    label: "空层",
    background: "#DCDCDC",
    color: "#696969",
  },
  {
    value: 20000,
    label: "已上pin",
    background: "#FAFAD2",
    color: "#BDB76B",
  },
  {
    value: 20100,
    label: "已上pin转运",
    background: "#eafaf0",
    color: "#56ce66",
  },
  {
    value: 30000,
    label: "生料在库",
    background: "rgba(176,224,230,.2)",
    color: "#00008B",
  },
  {
    value: 30100,
    label: "生料待上机",
    background: "rgba(230,230,250,.2",
    color: "#FF1493",
  },
  {
    value: 40100,
    label: "熟料待入库",
    background: "#F0FFF0",
    color: "#919399",
  },
  {
    value: 50000,
    label: "熟料已入库",
    background: "#e6e2f6",
    color: "#967ad7",
  },
  {
    value: 50100,
    label: "退pin转运",
    background: "#FFC2C2",
    color: "#800000",
  },
  {
    value: 60000,
    label: "退pin就绪",
    background: "#f4f4f5",
    color: "#8B4513",
  },
];

// 分区类别
const partitionKinds = [
  {
    value: 0,
    label: "未指定",
  },
  {
    value: 1,
    label: "私有分区",
  },
  {
    value: 2,
    label: "公共分区",
  },
];

export default {
  queryOrderByOptions,
  deviceRecords_QueryOrderByOptions,
  alarmLevelOptions,
  deviceStatusOptions,
  schedulementOptions,
  requestMaterialKindOptions,
  deviceKinds,
  agvKindList,
  drillKindList,
  interactionSequenceOptions,
  workOrderOptions,
  taskOptions,
  productStatusOptions,
  partitionKinds,
  alarmKindList,
};
