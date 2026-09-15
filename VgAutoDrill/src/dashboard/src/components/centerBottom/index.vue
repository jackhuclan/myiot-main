<template>
  <div class="line_center">
    <dv-border-box-8 :dur="5" class="ptb-20">
      <p class="center_title">最近一周产量统计</p>
      <div id="line_center_diagram" ref="myChart"></div>
    </dv-border-box-8>
  </div>
</template>

<script>
export default {
  name: "CenterBottom",
  data() {
    return {
      seriesType: ["bar", "bar", "bar", "line"],
    };
  },
  methods: {
    //折线图
    line_center_diagram(data) {
      // 获取已有echarts实例的DOM节点
      let myChart = this.$echarts.getInstanceByDom(this.$refs.myChart);
      // 如果不存在，就进行初始化
      if (myChart == null) {
        myChart = this.$echarts.init(this.$refs.myChart);
      }
      // 指定图表的配置项和数据
      let option = {
        tooltip: {
          trigger: "axis",
          axisPointer: {
            // 坐标轴指示器，坐标轴触发有效
            type: "shadow", // 默认为直线，可选为：'line' | 'shadow'
          },
        },
        legend: {
          data: data?.quantitysStats.map((v) => v.processName),
          top: 20,
          fontSize: 20, //字体大小
          color: "rgba(255,255,255,0.65)", //字体颜色
        },
        grid: {
          left: "3%",
          right: "4%",
          bottom: "3%",
          containLabel: true,
        },
        xAxis: [
          {
            // x轴字体提示样式
            axisLabel: {
              margin: 20,
              show: true,
              fontFamily: "微软雅黑",
              color: "#fff",
              fontSize: 18,
            },
            type: "category",
            //x轴显示内容
            data: data?.times,
          },
        ],
        yAxis: [
          {
            name: "产量",
            nameTextStyle: {
              padding: [0, 10, 10, 0],
              fontSize: 18,
            },
            type: "value",
            // y轴字体提示样式
            axisLabel: {
              show: true,
              fontFamily: "微软雅黑",
              color: "#fff",
              fontSize: 18,
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
              fontSize: 18,
            },
            type: "value",

            axisLabel: {
              show: true,
              fontFamily: "微软雅黑",
              color: "#fff",
              fontSize: 18,
              formatter: "{value}%", // 格式化标签
            },
            axisLine: {
              show: false,
            },
            axisTick: {
              show: false,
            },
            splitLine: {
              lineStyle: {
                height: 10,
                color: "rgba(255, 255, 255, 0.4)",
                type: "dotted", //折线图表格行边框样式
              },
            },
          },
        ],
        series: data?.quantitysStats.map((v, i) => {
          if (i == data?.quantitysStats.length - 1) {
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
      myChart.setOption(option);
      //建议加上以下这一行代码，不加，当浏览器窗口缩小时，echarts显示不全。
      this.echartsResize(myChart);
    },
  },
};
</script>

<style lang="scss" scoped>
.line_center {
  height: 30%;
}
 
</style>
