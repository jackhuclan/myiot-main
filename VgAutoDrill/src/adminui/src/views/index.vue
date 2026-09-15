<template>
  <div class="dashboard-editor-container">
    <!-- DEV分支隐藏 -->
    <!-- <panel-group
      :deviceData="deviceData"
      @openTimer="openTimer"
      @closeTimer="closeTimer"
    /> -->
    <!-- 近一周AGV调度统计(全部) -->
    <el-row
      v-if="isWeekAgvData"
      style="background: #fff; padding: 16px 16px 0; margin-bottom: 32px"
    >
      <agvBar-chart :chartData="agvBarChartList" />
    </el-row>
    <!-- 近一周AGV调度统计(完成/异常) -->
    <el-row
      v-if="isWeekAgvTypeData"
      style="background: #fff; padding: 16px 16px 0; margin-bottom: 32px"
    >
      <agvTypeBar-chart :chartData="agvTypeBarChartList" />
    </el-row>
    <!-- 近一周钻机叫料统计 -->
    <el-row
      v-if="!1"
      style="background: #fff; padding: 16px 16px 0; margin-bottom: 32px"
    >
      <DrillBarChart :chartData="drillBarChartList" />
    </el-row>
    <!-- 近一周产量 -->
    <el-row
      v-if="isYield"
      style="background: #fff; padding: 16px 16px 0; margin-bottom: 32px"
    >
      <line-chart :chartData="lineChartList" />
    </el-row>
    <el-row :gutter="32" style="padding: 16px 16px 0; margin-bottom: 32px">
      <WorkOrderTable
        ref="workTable"
        :workorderList="workorderList"
        :loading="loading"
      />
    </el-row>
    <!-- DEV分支隐藏 -->
    <!-- <el-row>
      <el-col>
        <el-popover
          placement="left"
          style="float: right; min-height: 100px"
          trigger="hover"
          v-if="delList.length > 0"
        >
          <li
            class="back_li"
            v-for="item in delList"
            :key="item.view"
            @click="back(item)"
          >
            {{ item.name }}
          </li>
          <el-button slot="reference" icon="el-icon-delete"> </el-button>
        </el-popover>
        <el-button style="float: right" v-else icon="el-icon-delete">
        </el-button>
      </el-col>
    </el-row> -->
    <!-- DEV分支隐藏 -->
    <!-- <el-row :gutter="32">
      <transition-group name="col_transition">
        <el-col :xs="24" :sm="12" :lg="8" v-for="(item, i) in list" :key="i">
          <el-button @click="tableShow(i)">x</el-button>
          <div class="chart-wrapper"> 
            <component :is="item.view">动态渲染图表组件</component>
          </div>
        </el-col>
      </transition-group>
    </el-row> -->
  </div>
</template>

<script>
import PanelGroup from "./dashboard/PanelGroup";
import LineChart from "./dashboard/LineChart";
import RaddarChart from "./dashboard/RaddarChart";
import PieChart from "./dashboard/PieChart";
import BarChart from "./dashboard/BarChart";
import AgvBarChart from "./dashboard/AgvBarChart.vue";

import AgvTypeBarChart from "./dashboard/AgvTypeBarChart.vue";
import StackedHorizontalBarChart from "./dashboard/StackedHorizontalBarChart";
import DoughnutChart from "./dashboard/DoughnutChart";
import WorkOrderTable from "./dashboard/WorkOrderTable";
import {
  getFeedBackStats,
  getSchedulementDeviceStats,
  getSchedulementDeviceStatusStats,
  getHomeDeviceData,
  getWorkOrderList,
} from "@/api/dashboard";
import { Draggable } from "vue-smooth-dnd";
import DrillBarChart from "./dashboard/DrillBarChart.vue";
export default {
  name: "Index",
  components: {
    PanelGroup,
    LineChart,
    RaddarChart,
    PieChart,
    BarChart,
    StackedHorizontalBarChart,
    DoughnutChart,
    WorkOrderTable,
    AgvBarChart,
    AgvTypeBarChart,
    DrillBarChart,
    Draggable,
    DrillBarChart,
  },
  data() {
    return {
      loading: false,
      // 设备数据
      deviceData: {},
      // 近一周产量
      isYield: true,
      lineChartList: {},
      // 近一周Agv调度统计
      isWeekAgvData: true,
      agvBarChartList: {},
      // 近一周Agv调度统计(完成/异常)
      isWeekAgvTypeData: true,
      agvTypeBarChartList: {},
      // 近一周钻机叫料统计
      drillBarChartList: {},
      // 生产进度
      workorderList: [],
      timer: null,
      // 底部图表组
      list: [],
      // 删除的项
      delList: sessionStorage.getItem("delList")
        ? JSON.parse(sessionStorage.getItem("delList"))
        : [],
    };
  },
  created() {
    this.getList();
    this.openTimer();
  },
  beforeDestroy() {
    this.closeTimer();
  },
  methods: {
    // 开启定时器
    openTimer() {
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.getList(); //调用接口的方法
        }, 0);
      }, 5000);
    },
    // 关闭定时器
    closeTimer() {
      clearInterval(this.timer);
      this.timer = null;
    },
    getList() {
      // 获取设备数据
      // getHomeDeviceData().then((res) => {
      //   this.deviceData = res.data;
      // });
      // 获取产量数据
      getFeedBackStats().then((res) => {
        this.lineChartList = res;
        this.isYield = res.quantitysStats.some((v) =>
          v.quantitys.some((i) => i > 0)
        );
      });
      // 获取近一周agv数据
      getSchedulementDeviceStats().then((res) => {
        this.agvBarChartList = res;
        this.isWeekAgvData = res.quantitysStats.some((v) =>
          v.quantitys.some((i) => i > 0)
        );
      });
      // 获取近一周agv数据(完成/异常)
      getSchedulementDeviceStatusStats().then((res) => {
        this.agvTypeBarChartList = res;
        this.drillBarChartList = res;
        this.isWeekAgvTypeData = res.quantitysStats.some((v) =>
          v.quantitys.some((i) => i > 0)
        );
      });
      // 获取首页表格数据
      this.$nextTick(() => {
        this.loading = true;
        getWorkOrderList({}).then((response) => {
          this.workorderList = response?.data.list;
          this.loading = false;
        });
      });
      this.list = sessionStorage.getItem("viewList")
        ? JSON.parse(sessionStorage.getItem("viewList"))
        : [
            {
              view: "raddar-chart",
              name: "雷达图",
            },
            { view: "pie-chart", name: "南丁格尔玫瑰图" },
            { view: "bar-chart", name: "堆叠柱状图" },
            { view: "stackedHorizontalBar-chart", name: "堆叠条形图" },
            { view: "doughnut-chart", name: "环形图" },
          ];
    },
    //点击图表上方x号
    tableShow(i) {
      const item = this.list[i];
      this.delList.push(item);
      this.list.splice(i, 1);
      sessionStorage.setItem("viewList", JSON.stringify(this.list));
      sessionStorage.setItem("delList", JSON.stringify(this.delList));
    },
    back(item) {
      this.delList = this.delList.filter((v) => v.view != item.view);
      this.list.push(item);
      sessionStorage.setItem("viewList", JSON.stringify(this.list));
      sessionStorage.setItem("delList", JSON.stringify(this.delList));
    },
  },
};
</script>

<style lang="scss" scoped>
.dashboard-editor-container {
  padding: 20px;
  background-color: rgb(240, 242, 245);
  position: relative;

  .chart-wrapper {
    background: #fff;
    padding: 16px 16px 0;
    margin-bottom: 32px;
  }
}
.back_li:hover {
  cursor: pointer;
}

@media (max-width: 1024px) {
  .chart-wrapper {
    padding: 8px;
  }
}
.col_transition-enter,
.col_transition-leave-to {
  opacity: 0;
  // transform: translateX(400px);
}
.col_transition-enter-active,
.col_transition-leave-active {
  transition: all 0.5s ease;
}
.col_transition-move {
  /*让元素被改变定位的时候，有一个缓动效果*/
  transition: all 0.5s ease;
}
.col_transition-leave-active {
  /*表示要被删除的元素 ，让即将被移除的元素脱离标准流，这样后面的元素就能渐渐的浮动上来*/
  position: absolute;
}
</style>
