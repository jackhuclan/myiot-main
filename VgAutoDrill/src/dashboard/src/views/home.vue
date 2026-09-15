<template>
  <div id="index" ref="appRef">
    <div class="bg">
      <dv-loading v-show="loading">Loading...</dv-loading>
      <div class="host-body">
        <!-- 顶部title部分 -->
        <ScreenHeader :route="'index'" />
        <!-- 主体部分 -->
        <el-row style="height: calc(100% - 45px)">
          <!-- 第一列 -->
          <el-col :span="6" style="height: calc(100%)">
            <!-- 公司简介部分 -->
            <LeftOne />
            <!-- 设备统计部分 -->
            <LeftTwo :list="leftTwoList" />
            <!-- 设备告警部分 -->
            <LeftThree :config="leftThreeConfig" />
          </el-col>
          <!-- 第二列 -->
          <el-col :span="12" class="plr-10" style="height: calc(100%)">
            <!-- 中间部分 -->
            <div class="center_box">
              <!-- 任务统计 -->
              <CenterTopOne :config="{ feedBackConfig, workOrderConfig }" />
              <!-- 操作车间展示 -->
              <CenterTopTwo :config="centerTopTwoConfig" />
              <!--  生产产量 -->
              <CenterTopThree
                ref="gaugeEcharts"
                :config="{
                  productionActual,
                  productionTargets,
                  progressDaily,
                }"
              />
            </div>
            <!-- 折线图 -->
            <CenterBottom ref="line" />
          </el-col>
          <!-- 第三列 -->
          <el-col :span="6" style="height: calc(100%)">
            <!-- 轮播板料追踪部分 -->
            <RightOne :config="rightOneConfig" />
            <!-- 工单进度部分 -->
            <RightTwo :config="rightTwoConfig" />
          </el-col>
        </el-row>
      </div>
    </div>
  </div>
</template>

<script>
import ScreenHeader from "@/components/header";
import LeftOne from "@/components/leftOne";
import LeftTwo from "@/components/leftTwo";
import LeftThree from "@/components/leftThree";
import RightOne from "@/components/rightOne";
import RightTwo from "@/components/rightTwo";
import CenterBottom from "@/components/centerBottom";
import CenterTopOne from "@/components/centerTopOne";
import CenterTopTwo from "@/components/centerTopTwo";
import CenterTopThree from "@/components/centerTopThree";
import {
  getDeviceStats,
  getDeviceAlarmStats,
  getPanelStats,
  getWorkOrderStats,
  getWorkOrderRateStats,
  getDeviceServiceStatsList,
  getTaskStats,
  getFeedBackStats,
} from "@/api";
export default {
  name: "HomeView",
  components: {
    ScreenHeader,
    LeftOne,
    LeftTwo,
    LeftThree,
    RightOne,
    RightTwo,
    CenterBottom,
    CenterTopOne,
    CenterTopTwo,
    CenterTopThree,
  },
  data() {
    return {
      // timer定时器
      timer: null,
      loading: true,
      leftTwoList: [],
      leftThreeConfig: {
        header: ["设备", "日期", "告警描述", "级别"],
        data: [],
        evenRowBGC: "#020308",
        oddRowBGC: "#020308",
        headerBGC: "rgba(255,245,238,.1)",
        // 列宽
        columnWidth: [100, 220, 180, 80],
        // 对齐方式
        align: ["center", "center", "center", "center"],
      },
      rightOneConfig: {
        header: ["板料料号", "板料类型", "对应设备"],
        data: [],
        evenRowBGC: "#020308",
        oddRowBGC: "#020308",
        headerBGC: "rgba(255,245,238,.1)",
        // 列宽
        columnWidth: [150, 150, 150],
        // 对齐方式
        align: ["center", "center", "center"],
      },
      rightTwoConfig: {
        header: ["工单号", "产品名", "工单进度"],
        data: [],
        evenRowBGC: "#020308",
        oddRowBGC: "#020308",
        headerBGC: "rgba(255,245,238,.1)",
        // 列宽
        columnWidth: [150, 150, 150],
        // 对齐方式
        align: ["center", "center", "center"],
      },
      feedBackConfig: {
        data: [],
        shape: "round",
      },
      workOrderConfig: {
        data: [],
        shape: "round",
      },
      centerTopTwoConfig: {
        header: ["呼叫设备", "呼叫时间", "响应设备", "响应时间"],
        data: [],
        evenRowBGC: "#020308",
        oddRowBGC: "rgba(176,224,230,.1)",
        headerBGC: "rgba(255,245,238,.1)",
        // 列宽
        columnWidth: [150, 220, 150, 220],
        // 对齐方式
        align: ["center", "center", "center", "center"],
      },
      // 生产产量模块数据
      productionActual: 0,
      productionTargets: 0,
      progressDaily: 0,
      // 生产产量折线图数据
      lineData: {},
    };
  },
  created() {
    this.$nextTick(() => {
      this.getLeftTwo();
      this.getLeftThree();
      this.getRightOne();
      this.getRightTwo();
      this.getCenterTopOne();
      this.getCenterTopTwo();
      this.getCenterTopThree();
      this.getCenterBottom();
    });
  },
  mounted() {
    // 定时刷新每隔5分钟刷新
    this.timer = setInterval(() => {
      this.getLeftTwo();
      this.getLeftThree();
      this.getRightOne();
      this.getRightTwo();
      this.getCenterTopOne();
      this.getCenterTopTwo();
      this.getCenterTopThree();
      this.getCenterBottom();
    }, 30000);
  },
  destroyed() {
    clearInterval(this.timer);
    this.timer = null;
  },
  methods: {
    getLeftTwo() {
      // 默认只显示3条
      getDeviceStats().then((res) => {
        this.leftTwoList = res.data.slice(0, 3);
      });
    },
    getLeftThree() {
      getDeviceAlarmStats().then((res) => {
        // 使用es6...解决DataV改变数据视图不主动刷新
        this.leftThreeConfig = { ...this.leftThreeConfig, data: res.data };
      });
    },
    getRightOne() {
      getPanelStats().then((res) => {
        // 使用es6...解决DataV改变数据视图不主动刷新
        this.rightOneConfig = { ...this.rightOneConfig, data: res.data };
      });
    },
    getRightTwo() {
      getWorkOrderStats().then((res) => {
        // 使用es6...解决DataV改变数据视图不主动刷新
        this.rightTwoConfig = { ...this.rightTwoConfig, data: res.data };
      });
    },
    getCenterTopOne() {
      getWorkOrderRateStats().then((res) => {
        // 使用es6...解决DataV改变数据视图不主动刷新
        this.workOrderConfig = {
          ...this.workOrderConfig,
          data: [res.data.workOrderAddRate],
        };
        this.feedBackConfig = {
          ...this.feedBackConfig,
          data: [res.data.feedBackAddRate],
        };
      });
    },
    getCenterTopTwo() {
      getDeviceServiceStatsList().then((res) => {
        // 使用es6...解决DataV改变数据视图不主动刷新
        this.centerTopTwoConfig = {
          ...this.centerTopTwoConfig,
          data:
            res.data.lengtn > 0
              ? res.data
              : [
                  ["钻机1", "2023-05-05 15:00", "AGV001", "2023-05-05 18:00"],
                  ["钻机2", "2023-05-02 08:00", "AGV002", "2023-05-02 10:00"],
                  ["钻机3", "2023-05-05 13:00", "AGV003", "2023-05-05 15:00"],
                  ["钻机4", "2023-05-01 11:00", "AGV004", "2023-05-01 13:00"],
                  ["钻机5", "2023-04-28 10:43", "AGV005", "2023-04-28 20:00"],
                  ["钻机6", "2023-04-30 09:00", "AGV006", "2023-04-30 17:00"],
                ],
        };
      });
    },
    getCenterTopThree() {
      getTaskStats().then((res) => {
        this.productionTargets = res.data.productionTargets;
        this.productionActual = res.data.productionActual;
        this.progressDaily = res.data.progressDaily;
      });
      this.$nextTick(() => {
        this.$refs.gaugeEcharts.gaugeEcharts();
      });
    },
    getCenterBottom() {
      getFeedBackStats().then((res) => {
        this.$nextTick(() => {
          this.$refs.line.line_center_diagram(res.data);
        });
      });
    },
  },
};
</script>
<style lang="scss" scoped>
// 中间部分
.center_box {
  height: 65%;
  width: 100%;
  padding-top: 5px;
  /* 声明一个容器 */
  display: grid;
  /*  声明列的宽度  */
  grid-template-columns: 40% 58%;
  /*  声明行间距和列间距  */
  grid-gap: 11px;
  /*  声明行的高度  */
  grid-template-rows: 320px 315px;
}
</style>