const deviceStatus = [
  {
    label: "工作",
    color: "green",
    value: "WORK",
  },
  { label: "离线", color: "#ccc", value: "OFFLINE" },
  {
    label: "停止",
    color: "#9a9ca1",
    value: "STOP",
  },

  {
    label: "等待",
    color: "#ff7301",
    value: "WAIT",
  },

  {
    label: "报警",
    color: "red",
    value: "ALAM",
  },
  {
    label: "空闲",
    color: "yellow",
    value: "IDLE",
  },
  {
    label: "服务",
    color: "pink",
    value: "SERV",
  },
];
const deviceOption = {
  // 加工
  runningCount: 10,
  // 报警
  warnningCount: 2,
  // 停止
  stopingCount: 3,
  // 等待
  standbyCount: 0,
  // 闲置
  idleCount: 10,
  // 离线
  offlineCount: 10,
};

const list = Array.from({ length: 30 }, (v, k) => {
  const ind = Math.floor(Math.random() * deviceStatus.length);
  return {
    code: "00" + (k + 1),
    status: deviceStatus[ind].value,
    duty: Math.floor(Math.random() * (100 - 1 + 1)) + 1,
  };
});

export default list;
