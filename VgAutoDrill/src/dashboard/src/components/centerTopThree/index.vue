<template>
  <div>
    <dv-border-box-7 class="p-20">
      <p class="center_title">生产量情况</p>
      <div class="gauge_box">
        <li class="gauge" ref="gauge"></li>
        <li>
          <p>
            <span>生产目标</span> <span>{{ config.productionTargets }}</span>
          </p>
          <p>
            <span>实际产量</span><span>{{ config.productionActual }}</span>
          </p>
        </li>
      </div>

      <el-progress :percentage="config.progressDaily"></el-progress>
    </dv-border-box-7>
  </div>
</template>

<script>
export default {
  name: "CenterTopThree",
  props: ["config"],
  methods: {
    // 生产产量图标
    gaugeEcharts() {
      // 日进度
      const progressDaily = this.config.progressDaily;
      // 获取已有echarts实例的DOM节点
      let myChart = this.$echarts.getInstanceByDom(this.$refs.gauge);
      // 如果不存在，就进行初始化
      if (myChart == null) {
        myChart = this.$echarts.init(this.$refs.gauge);
      }
      let option = {
        tooltip: {
          formatter: "{a} <br/>{b} : {c}%",
        },
        series: [
          {
            startAngle: 220, // 开始角度 左侧角度
            endAngle: -40, // 结束角度 右侧
            name: "Pressure",
            type: "gauge",
            radius: "100%",
            progress: {
              show: true,
              width: 16, //进度宽度
            },
            axisLine: {
              lineStyle: {
                width: 16, //圆弧宽度
              },
            },
            center: ["50%", "53%"],
            pointer: {
              // 指针样式
              width: 3,
              length: "50%",
              shadowBlur: 10,
            },
            detail: {
              // 中间数据
              valueAnimation: true,
              formatter: "{value}%", // 数据值的样式
              fontSize: 16,
              offsetCenter: [0, "70%"], // 中间值的位置
              color: "#fff", // 中间值的颜色
            },
            title: {
              // 标题位置
              offsetCenter: [0, "-35%"],
              color: "#00CCCC",
            },
            axisTick: {
              // 短刻度样式
              show: false, // false表示不显示
            },
            splitLine: {
              // 长刻度设置
              show: false,
            },
            axisLabel: {
              show: false,
            },
            color: ["#00CCCC"], //进度及指针颜色
            data: [
              {
                // 数据库数据
                value: progressDaily,
                name: "日进度",
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

<style lang="scss" scoped>
// 生产量情况
.gauge_box {
  margin: 20px 0;
  width: 100%;
  height: 180px;
  display: flex;
  .gauge {
    width: 50%;
  }
  li {
    height: 100%;
    width: 50%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    font-size: 20px;
    p {
      width: 100%;
      padding: 0 20px;
      height: 60px;
      display: flex;
      align-items: center;
      justify-content: space-between;
    }
  }
}
</style>
