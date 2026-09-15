<template>
  <div :class="className" :style="{ height: height, width: width }" />
</template>

<script>
require("echarts/theme/macarons"); // echarts theme
import resize from "./mixins/resize";

export default {
  name: "StackedHorizontalBarChart",
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
      default: "300px",
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
    };
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
  watch: {
    chartData(nv, ov) {
      this.setOptions(nv);
    },
  },
  methods: {
    initChart() {
      this.chart = this.$echarts.init(this.$el, "macarons");
    },
    setOptions(res) {
      let option;

      if (res?.quantitysStats.length <= 0) {
        option = {
          title: [
            {
              text: "近一周AGV调度统计(完成/异常)",
              left: "center",
              textStyle: {
                fontSize: 18, //字体大小
                color: "#000", //字体颜色
              },
            },
            {
              text: "暂无数据",
              x: "center",
              y: "center",
              textStyle: {
                fontSize: 16,
                fontWeight: "normal",
              },
            },
          ],
          grid: {
            left: "0%",
            right: "3%",
            bottom: "3%",
            containLabel: true,
          },
          xAxis: {
            type: "value",
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
          },
          yAxis: {
            nameTextStyle: {
              padding: [0, 10, 10, 0],
              fontSize: 14,
              fontFamily: "微软雅黑",
              color: "#000",
            },
            // y轴字体提示样式
            axisLabel: {
              textStyle: {
                show: true,
                fontFamily: "微软雅黑",
                color: "#000",
                fontSize: 14,
              },
            },
            type: "category",
            data: res?.statsTimes,
          },
          series: [],
        };
      } else {
        option = {
          title: {
            text: "近一周AGV调度统计(完成/异常)",
            left: "center",
            textStyle: {
              fontSize: 18, //字体大小
              color: "#000", //字体颜色
            },
          },
          tooltip: {
            trigger: "axis",
            axisPointer: {
              // Use axis to trigger tooltip
              type: "shadow", // 'shadow' as default; can also be 'line' or 'shadow'
            },
          },
          legend: {
            type: "scroll",
            data: res?.quantitysStats.map((v) => v.lable),
            top: 30,
            textStyle: {
              fontSize: 12, //字体大小
              color: "#000", //字体颜色
            },
          },
          grid: {
            left: "0%",
            right: "3%",
            bottom: "3%",
            containLabel: true,
          },
          xAxis: {
            type: "value",
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
          },
          yAxis: {
            nameTextStyle: {
              padding: [0, 10, 10, 0],
              fontSize: 14,
              fontFamily: "微软雅黑",
              color: "#000",
            },
            // y轴字体提示样式
            axisLabel: {
              textStyle: {
                show: true,
                fontFamily: "微软雅黑",
                color: "#000",
                fontSize: 14,
              },
            },
            type: "category",
            data: res?.statsTimes,
          },
          series: res?.quantitysStats.map((item) => {
            return {
              name: item.lable,
              type: "bar",
              stack: "total",
              label: {
                show: true,
                formatter: function (params) {
                  if (params.data == 0) {
                    return "";
                  }

                  return params.data;
                },
              },
              emphasis: {
                focus: "series",
                blurScope: "coordinateSystem",
              },
              data: item?.quantitys,
            };
          }),
        };
      }

      this.chart.setOption(option);
    },
  },
};
</script>
