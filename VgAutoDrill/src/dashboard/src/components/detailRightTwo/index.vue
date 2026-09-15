<template>
  <div class="right_two">
    <dv-border-box-12 class="p-20 ">
      <p class="title">设备稼动率</p>
      <div id="line_center_diagram" ref="myChart"></div>
    </dv-border-box-12>
  </div>
</template>

<script>
export default {
  name: "DetailLeftTwo",
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
        title: [
          {
            text: "平均移动率",
            right: 70,
            color: "#ccc", //'red'，字体颜色
            fontWeight: "normal", //'bold'(粗体) | 'bolder'(粗体) | 'lighter'(正常粗细) ，字体粗细
            fontSize: 16, //字体大小
            lineHeight: 16, //字体行高
          },
          {
            text: "80%",
            right: 20,
            color: "#ccc", //'red'，字体颜色
            fontStyle: "italic", //'italic'(倾斜) | 'oblique'(倾斜体) ，字体风格
            fontWeight: "bold", //'bold'(粗体) | 'bolder'(粗体) | 'lighter'(正常粗细) ，字体粗细
            fontSize: 20, //字体大小
            lineHeight: 18, //字体行高
          },
        ],
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
                color: "rgba(0, 246, 255, 1)", //x轴边框颜色
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
              show: true,
              fontFamily: "微软雅黑",
              color: "#fff",
              fontSize: 14,
              interval: "auto",
              // y轴设置成%
              formatter: "{value} %",
            },
            min: 0,
            max: 100,
            splitLine: {
              lineStyle: {
                height: 10,
                color: "rgba(255, 255, 255, 0.4)",
                type: "dotted", //折线图表格行边框样式
              },
            },
          },
        ],
        // 悬浮提示
        tooltip: {
          trigger: "axis",
          formatter: "{b}\n\n{c}%",
        },
        series: [
          {
            type: "line",
            lineStyle: {
              width: 1, //波形图波浪的边框
              color: "rgba(0, 246, 255, 1)",
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
                    color: "rgba(0, 246, 255, 0.42)",
                  },
                  {
                    offset: 1,
                    color: "rgba(0, 246, 255, 0.1)",
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
.right_two {
  height: 30%;
}
</style>
