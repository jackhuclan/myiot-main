<template>
  <div :class="className" :style="{ height: height, width: width }" />
</template>

<script>
require("echarts/theme/macarons"); // echarts theme
import resize from "./mixins/resize";
export default {
  name: "LineChart",
  mixins: [resize],
  props: {
    className: {
      type: String,
      default: "chart",
    },
    width: {
      type: String,
      default: "100%",
    },
    height: {
      type: String,
      default: "350px",
    },
    autoResize: {
      type: Boolean,
      default: true,
    },
    chartData: {
      type: Object,
    },
  },
  data() {
    return {
      chart: null,
      timer: null,
      seriesType: ["bar", "bar", "bar", "line"],
    };
  },
  watch: {
    chartData(nv, ov) {
      this.setOptions(nv);
    },
  },
  mounted() {
    this.$nextTick(() => {
      this.initChart();
    });
  },
  beforeDestroy() {
    if (!this.chart) {
      return;
    }
    this.chart.dispose();
    this.chart = null;
  },
  methods: {
    initChart() {
      this.chart = this.$echarts.init(this.$el, "macarons");
    },
    setOptions(res) {
      let option = {
        title: {
          text: "近一周产量统计",
          left: "center",
          textStyle: {
            fontSize: 18, //字体大小
            color: "#000", //字体颜色
          },
        },
        tooltip: {
          trigger: "axis",
          axisPointer: {
            // 坐标轴指示器，坐标轴触发有效
            type: "shadow", // 默认为直线，可选为：'line' | 'shadow'
          },
        },
        legend: {
          data: res?.quantitysStats.map((v) => v.processName),
          top: 30,
          textStyle: {
            fontSize: 14, //字体大小
            color: "#000", //字体颜色
          },
        },
        grid: {
          left: "1%",
          right: "1%",
          bottom: "3%",
          containLabel: true,
        },
        xAxis: [
          {
            // x轴字体提示样式
            axisLabel: {
              margin: 20,
              textStyle: {
                show: true,
                fontFamily: "微软雅黑",
                color: "#000",
                fontSize: 14,
              },
            },
            type: "category",
            //x轴显示内容
            data: res?.times,
          },
        ],
        yAxis: [
          {
            name: "产量",
            nameTextStyle: {
              padding: [0, 10, 10, 0],
              fontSize: 14,
              fontFamily: "微软雅黑",
              color: "#000",
            },
            type: "value",
            // y轴字体提示样式
            axisLabel: {
              textStyle: {
                show: true,
                fontFamily: "微软雅黑",
                color: "#000",
                fontSize: 14,
              },
            },
            splitLine: {
              lineStyle: {
                height: 10,
                color: "rgba(255, 255, 255, 0.4)",
                type: "dotted", //折线图表格行边框样式
              },
            },
          },
          {
            name: "合格率",
            nameTextStyle: {
              padding: [0, 0, 10, 10],
              fontSize: 14,
              fontFamily: "微软雅黑",
              color: "#000",
            },
            type: "value",

            axisLabel: {
              textStyle: {
                show: true,
                fontFamily: "微软雅黑",
                color: "#000",
                fontSize: 14,
              },
              formatter: "{value}%", // 格式化标签
            },
            axisLine: {
              show: true,
            },
            axisTick: {
              show: true,
            },
            splitLine: {
              lineStyle: {
                height: 10,
                color: "rgba(0,0,0,.3)",
                type: "dotted", //折线图表格行边框样式
              },
            },
          },
        ],
        series: res?.quantitysStats.map((v, i) => {
          if (i == res?.quantitysStats.length - 1) {
            return {
              yAxisIndex: 1,
              name: v.processName,
              data: v.quantitys,
              type: this.seriesType[i],
            };
          }
          return {
            name: v.processName,
            data: v.quantitys,
            type: this.seriesType[i],
          };
        }),
      };

      this.chart.setOption(option);
    },
  },
};
</script>
