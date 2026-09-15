export default {
  computed: {
    windowHeight() {
      return uni.getSystemInfoSync().windowHeight - 50;
    },
  },
  data() {
    return {
      // 任务状态
      taskOptions: [
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
          color: "#56ce66",
        },
        {
          value: 50,
          label: "已完成",
          background: "#eafaf0",
          color: "#f6c222",
        },
      ],
      // 按钮list
      btnInfo: {
        leftList: [
          {
            label: "叫料",
            type: 1,
          },
          {
            label: "退空盘",
            type: 2,
          },
          {
            label: "上机解绑",
            type: 3,
          },
        ],
        rightList: [
          {
            label: "叫空盘",
            type: 4,
          },
          {
            label: "退料",
            type: 5,
          },
          {
            label: "下机绑定",
            type: 6,
          },
        ],
      },
      // 校验规则
      rulesInfo: {
        leftRules: {
          lot: {
            rules: [
              {
                required: true,
                errorMessage: "请选择Lot号",
              },
            ],
          },
          podCode: {
            rules: [
              {
                required: true,
                errorMessage: "托盘号不能为空",
              },
            ],
          },
        },
        // 校验规则
        rightRules: {
          lot: {
            rules: [
              {
                required: true,
                errorMessage: "请选择Lot号",
              },
            ],
          },
          podCode: {
            rules: [
              {
                required: true,
                errorMessage: "托盘号不能为空",
              },
            ],
          },
          clinkerMaterialNum: {
            rules: [
              {
                required: true,
                errorMessage: "熟料数量不能为0",
              },
              {
                validateFunction: function (rule, value, data, callback) {
                  if (value <= 0) {
                    callback("熟料数量不能为0");
                  }
                  return true;
                },
              },
            ],
          },
        },
      },
    };
  },
  methods: {
    getLabel(btnList, type) {
      return (
        this.btnInfo[btnList].find((v) => v.type == type) &&
        this.btnInfo[btnList].find((v) => v.type == type).label
      );
    },
  },
};
