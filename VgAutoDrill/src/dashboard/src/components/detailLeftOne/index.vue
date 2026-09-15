<template>
  <div class="left_one">
    <dv-border-box-12 class="p-20">
      <p class="title">设备运行情况</p>
      <div class="pie_box">
        <!-- 外层 -->
        <li class="gauge" ref="gauge"></li>
      </div>
    </dv-border-box-12>
  </div>
</template>

<script>
export default {
  name: "DetailLeftOne",
  data() {
    return {
      deviceId: 11,
    };
  },
  methods: {
    // 第一圈重要 数据 第二圈第三圈只是样式展示
    semicircle(res) {
      // 获取已有echarts实例的DOM节点
      let myChart = this.$echarts.getInstanceByDom(this.$refs.gauge);
      // 如果不存在，就进行初始化
      if (myChart == null) {
        myChart = this.$echarts.init(this.$refs.gauge);
      }
      const workingDevicePr = parseFloat(res.data?.workingDevicePr);
      const warnningDevicePr = parseFloat(res.data?.warnningDevicePr);
      const stopDevicePr = parseFloat(res.data?.stopDevicePr);
      const standbyDevicePr = parseFloat(res.data?.standbyDevicePr);
      let option = {
        title: {
          show: true,
          text: "735",
          subtext: "·运行设备·",
          itemGap: 8,
          left: "48%",
          top: "63%",
          color: "#B4E4FF",
          fontSize: 20,
          subtextStyle: {
            color: "#B4E4FF",
            fontSize: 14,
          },
          x: "center",
          y: "center",
          textAlign: "center",
        },

        legend: {
          // selectedMode: false, //禁用点击
          bottom: 0,
          left: "center",
          textStyle: {
            fontSize: 16,
            color: "#fff",
          },
        },

        series: [
          // 第一圈
          {
            itemStyle: {
              borderRadius: 0,
              borderColor: "#000",
              borderWidth: 3,
              //每个柱子的颜色即为colorList数组里的每一项，如果柱子数目多于colorList的长度，则柱子颜色循环使用该数组
              color: function (params) {
                var colorList = [
                  ["rgba(21, 118, 210, 0)", "rgba(21, 118, 210, 1)"],
                  ["rgba(255,69,0,0)", "rgba(255,69,0,1)"],
                  ["rgba(255,218,185,0)", "rgba(255,218,185,1)"],
                  ["rgba(176,224,230,0)", "rgba(176,224,230,1)"],
                ];
                var index = params.dataIndex;
                if (params.dataIndex >= colorList.length) {
                  index = params.dataIndex - colorList.length;
                }
                return {
                  x: 1,
                  y: 0,
                  x2: 1,
                  y2: 1,
                  type: "linear",
                  global: false,
                  colorStops: [
                    {
                      offset: 0,
                      color: colorList[index][0],
                    },
                    {
                      offset: 1,
                      color: colorList[index][1],
                    },
                  ],
                };
              },
            },
            type: "pie",
            radius: ["100%", "120%"],
            center: ["50%", "80%"],
            startAngle: 180,
            label: {
              // 设置标签位置，默认在饼状图外 可选值：'outer' ¦ 'inner（饼状图上）
              position: "outer",
              color: "#fff",
              fontSize: 16,
              show: true,
              padding: [0, -40],
              // \n\n可让文字居于牵引线上方，很关键
              formatter: "{c}%\n\n",
            },
            labelLine: { length: 20, length2: 30 },
            data: [
              {
                value: workingDevicePr,
                name: "运行中",
              },
              { value: standbyDevicePr, name: "故障中" },
              { value: stopDevicePr, name: "停用" },
              { value: warnningDevicePr, name: "待机" },
              {
                value:
                  warnningDevicePr +
                  workingDevicePr +
                  stopDevicePr +
                  standbyDevicePr,
                labelLine: { show: false },
                label: {
                  show: false,
                },
                itemStyle: {
                  color: "none",
                  decal: {
                    symbol: "none",
                  },
                  borderWidth: 0,
                },
              },
            ],
          },
          // 第二圈
          {
            type: "pie",
            radius: ["80%", "83%"],
            center: ["50%", "80%"],
            startAngle: 180,
            label: {
              show: false, //不显示指针和字段
            },
            color: ["#ccc"],
            itemStyle: {
              borderRadius: 0,
              borderColor: "#000",
              borderWidth: 3,
            },
            data: [
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              {
                value: 1200,
                itemStyle: {
                  // stop the chart from rendering this piece
                  color: "none",
                  decal: {
                    symbol: "none",
                  },
                  borderWidth: 0,
                },
              },
            ],
          },
          // 第三圈
          {
            itemStyle: {
              borderRadius: 0,
              borderColor: "#000",
              borderWidth: 3,
              //每个柱子的颜色即为colorList数组里的每一项，如果柱子数目多于colorList的长度，则柱子颜色循环使用该数组
              color: {
                x: 0,
                y: 0,
                x2: 1,
                y2: 0,
                type: "linear",
                global: false,
                colorStops: [
                  {
                    offset: 0,
                    color: "rgba(128,128,128,0)",
                  },
                  {
                    offset: 0.39,
                    color: "rgba(128,128,128,0.39)",
                  },
                  {
                    offset: 0.79,
                    color: "rgba(128,128,128,0.79)",
                  },
                  {
                    offset: 1,
                    color: "rgba(128,128,128,1)",
                  },
                ],
              },
            },
            type: "pie",
            radius: ["50%", "70%"],
            center: ["50%", "80%"],
            startAngle: 180,
            label: {
              show: false,
            },
            data: [
              {
                value: 100,
              },
              { value: 100 },
              { value: 100 },
              { value: 100 },
              {
                value: 400,
                labelLine: { show: false },
                label: {
                  show: false,
                },
                itemStyle: {
                  color: "none",
                  decal: {
                    symbol: "none",
                  },
                  borderWidth: 0,
                },
              },
            ],
          },
        ],
      };

      option && myChart.setOption(option);
      //建议加上以下这一行代码，不加，当浏览器窗口缩小时，echarts显示不全。
      this.echartsResize(myChart);
    },
  },
};
</script>

<style scoped lang="scss">
.left_one {
  height: 35%;
}
// 工单统计模块
.pie_box {
  width: 100%;
  height: 280px;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  .gauge {
    position: absolute;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
  }
}
canvas {
  background: pink;
}
</style>
