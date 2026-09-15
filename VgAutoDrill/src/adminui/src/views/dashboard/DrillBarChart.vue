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
      default: "450px",
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
      let data = [
        {
          date: "10-01",
          data: ["钻1", "钻2", "钻3", "钻是公司无关4", "钻5", "钻6", "钻7", "钻8"],
        },
        {
          date: "10-02",
          data: ["钻1", "钻2", "钻3", "钻4", "钻5", "钻6", "钻7", "钻8"],
        },
        {
          date: "10-03",
          data: ["钻1", "钻2", "钻3", "钻4", "钻5", "钻6", "钻7", "钻8"],
        },
        {
          date: "10-04",
          data: ["钻1", "钻2", "钻3", "钻4", "钻5", "钻6", "钻7", "钻8"],
        },
        {
          date: "10-05",
          data: ["钻1", "钻2", "钻3", "钻4", "钻5", "钻6", "钻7", "钻8"],
        },
        {
          date: "10-06",
          data: ["钻1", "钻2", "钻3", "钻4", "钻5", "钻6", "钻7", "钻8"],
        },
        {
          date: "10-07",
          data: ["钻1", "钻2", "钻3", "钻4", "钻5", "钻6", "钻7", "钻8"],
        },
      ];
      let option;
     
      option = {
        title: {
          text: "近一周钻机叫料统计",
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
          formatter: (params) => {
            let dataStr = `<div><p style="font-weight:bold;margin:0 8px 15px;">${params[0].name}</p></div>`;
            params.forEach((item) => {
              dataStr += `<div>
            <div style="margin: 0 8px;">
              <span style="display:inline-block;margin-right:5px;width:10px;height:10px;background-color:${item.color};"></span>
              <span>${item.seriesName}</span>
              <span style="float:right;color:#fff;margin-left:20px;">${item.data}</span>
            </div>
          </div>`;
            });
            return dataStr;
          },
        },
        legend: {
          data: ["成功", "失败"],
          top: 30,
          textStyle: {
            fontSize: 12, //字体大小
            color: "#000", //字体颜色
          },
        },
        grid: {
          left: "0%",
          right: "3%",
        //   bottom: "10%",
          containLabel: true,
        },
        // x轴的数据
        xAxis: [
          {
            data: data
              .map((v) => {
                return v.data;
              })
              .flat(Infinity),
            axisLabel: {
              interval: 0,
              rotate: -60, // 倾斜角度
            },
            axisTick: {
              show: true, // 显示坐标轴刻度线
              //   length: 20, // 刻度线的长度
            },
          },
          {
            data: data.map((v) => {
              return v.date;
            }),
            axisLabel: {
              interval: 0,
            },
            position: "top", // 很重要，如果没有这个设置，默认第二个x轴就会在图表的顶部
            // offset:0, // X 轴相对于默认位置的偏移，在相同的 position 上有多个 X 轴的时候有用。
            axisTick: {
              show: true,
              length: 320,
              inside: true, // 坐标轴刻度是否朝内，默认朝外
              lineStyle: {
                type: "dotted",
              },
            },
            axisLabel: {
              inside: true, // 坐标轴刻度是否朝内，默认朝外
            },
          },
        ],
        yAxis: {},
        // 可用于指定统计图类型
        // dataZoom: [
        //   // 有滚动条 平移
        //   {
        //     type: "slider",
        //     realtime: true,
        //     start: 0,
        //     end: 20, // 初始展示30%
        //     // height: 0,
        //     fillerColor: "rgba(17, 100, 210, 0.42)", // 滚动条颜色
        //     borderColor: "rgba(17, 100, 210, 0.12)",
        //     handleSize: 0, // 两边手柄尺寸
        //     showDetail: false, // 拖拽时是否展示滚动条两侧的文字
        //     top: "97.5%",
        //     zoomLock: true, // 是否只平移不缩放
        //     // moveOnMouseMove:true, //鼠标移动能触发数据窗口平移
        //     // zoomOnMouseWheel :true, //鼠标移动能触发数据窗口缩放
        //   },
        //   {
        //     type: "inside", // 支持内部鼠标滚动平移
        //     start: 0,
        //     end: 20,
        //     zoomOnMouseWheel: false, // 关闭滚轮缩放
        //     moveOnMouseWheel: true, // 开启滚轮平移
        //     moveOnMouseMove: true, // 鼠标移动能触发数据窗口平移
        //   },
        // ],
        series: [
          {
            name: "成功",
            type: "bar",
            stack: "Ad",
            // smooth: true,
            data: [
              0, 10, 6, 9, 12, 11, 1, 13,

              11, 12, 13, 14, 15, 16, 17, 18,

              2, 3, 9, 24, 25, 26, 27, 28,

              21, 10, 8, 7, 4, 1, 7, 8,

              1, 2, 3, 4, 5, 6, 7, 8,

              7, 9, 11, 13, 14, 15, 6, 7,

              10, 12, 14, 8, 7, 19, 0, 1,
            ],
          },
          {
            name: "失败",
            type: "bar",
            stack: "Ad",
            // smooth: true,
            data: [
              1, 2, 3, 4, 5, 6, 7, 8,

              2, 3, 4, 6, 5, 6, 7, 8,

              1, 2, 3, 4, 5, 6, 7, 8,

              1, 2, 3, 4, 5, 6, 7, 8,

              2, 3, 4, 6, 5, 6, 7, 8,

              1, 2, 3, 4, 5, 6, 7, 8,

              2, 3, 4, 6, 5, 6, 7, 8,
            ],
          },
        ],
        // yAxis: {
        //   nameTextStyle: {
        //     padding: [0, 10, 10, 0],
        //     fontSize: 14,
        //     fontFamily: "微软雅黑",
        //     color: "#000",
        //   },
        //   // y轴字体提示样式
        //   axisLabel: {
        //     textStyle: {
        //       show: true,
        //       fontFamily: "微软雅黑",
        //       color: "#000",
        //       fontSize: 14,
        //     },
        //   },
        //   type: "category",
        //   data: res?.statsTimes,
        // },
        // series: res?.quantitysStats.map((item) => {
        //   return {
        //     name: item.deviceCode,
        //     type: "bar",
        //     stack: "total",
        //     label: {
        //       show: true,
        //       formatter: function (params) {
        //         if (params.data == 0) {
        //           return "";
        //         }

        //         return params.data;
        //       },
        //     },
        //     emphasis: {
        //       focus: "series",
        //       blurScope: "coordinateSystem",
        //     },
        //     data: item.quantitys.every((v) => v == 0)
        //       ? [120, 132, 101, 134, 90, 230, 210]
        //       : item.quantitys,
        //   };
        // }),
      };
      //   }

      this.chart.setOption(option);
    },
  },
};
</script>
