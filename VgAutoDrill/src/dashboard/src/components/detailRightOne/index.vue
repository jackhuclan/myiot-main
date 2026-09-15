<template>
  <div class="right_one">
    <dv-border-box-12 class="p-20">
      <p class="title">加工时间统计</p>
      <div id="line_center_diagram" ref="myChart"></div>
    </dv-border-box-12>
  </div>
</template>

<script>
export default {
  name: "DetailLeftOne",
  methods: {
    //折线图
    line_center_diagram(res) {
      // 获取已有echarts实例的DOM节点
      let myChart = this.$echarts.getInstanceByDom(this.$refs.myChart);
      // 如果不存在，就进行初始化
      if (myChart == null) {
        myChart = this.$echarts.init(this.$refs.myChart);
      }
      const times = res.data[0];
      const data = res.data[1];
      // 指定图表的配置项和数据
      let option = {
        // grid 绘图网格
        grid: {
          top: 40,
          left: 0,
          right: 15,
          bottom: 45,
          containLabel: true,
        },
        xAxis: [
          {
            type: "category",
            boundaryGap: false,
            axisLine: {
              lineStyle: {
                color: "rgba(0,191,255, 1)", //x轴边框颜色
                width: 1,
              },
            },
            // x轴字体提示样式
            axisLabel: {
              padding: [10, 0, 0, 0],
              color: "#fff",
            },
            // x轴字段
            data: times,
          },
        ],
        yAxis: [
          {
            //Y轴线的样式和单位
            type: "value",
            axisTick: {
              show: false,
            },
            // y轴字体提示样式
            axisLabel: {
              formatter: function (value) {
                let str = "";
                let number = ((value * 1) / 60).toFixed(0);
                str = number + "小时";
                return str;
              },
              show: true,
              fontFamily: "微软雅黑",
              color: "#fff",
              fontSize: 14,
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
        tooltip: {
          trigger: "axis",
          formatter: function (value) {
            let str = "";
            let number = ((value[0].data * 1) / 60).toFixed(1);
            str = value[0].axisValue + " " + number + "小时";
            return str;
          },
        },
        series: [
          {
            type: "line",
            lineStyle: {
              width: 1, //波形图波浪的边框
              color: "rgba(0,191,255, 1)",
            },
            areaStyle: {
              //渐变色
              color: {
                x: 0,
                y: 0,
                x2: 0,
                y2: 1,
                type: "linear",
                global: false,
                colorStops: [
                  {
                    //波形图渐变色样式
                    offset: 0,
                    color: "rgba(0,191,255, 0.42)",
                  },
                  {
                    offset: 1,
                    color: "rgba(0,191,255, 0.1)",
                  },
                ],
              },
            },
            itemStyle: { color: "rgba(0, 246, 255, 1)" },
            data,
          },
        ],
      };

      myChart.setOption(option);
      //建议加上以下这一行代码，不加，当浏览器窗口缩小时，echarts显示不全。
      this.echartsResize(myChart);
    },
  },
};
</script>
<style scoped lang="scss">
.right_one {
  height: 35%;
}
</style>
