<template>
  <div id="index" ref="appRef">
    <div class="bg">
      <dv-loading v-show="loading">Loading...</dv-loading>
      <div class="host-body">
        <!-- 顶部title部分 -->
        <ScreenHeader :route="'detail'" />
        <!-- 主体部分 -->
        <el-row style="height: calc(100% - 45px)">
          <!-- 第一列 -->
          <el-col :span="6" style="height: calc(100%)">
            <!-- 设备运行情况部分 -->
            <DetailLeftOne ref="detailLeftOne" />
            <!-- 告警信息部分 -->
            <DetailLeftTwo :config="detailLeftTwoConfig" />
            <!-- 加工时间统计部分 -->
            <DetailLeftThree :config="detailLeftThreeConfig" />
          </el-col>
          <!-- 第二列 -->
          <el-col :span="12" class="plr-10" style="height: calc(100%)">
            <!-- 中间部分 -->
            <img src="@/assets/img/Multi.png" class="device_detail_img" />
          </el-col>
          <!-- 第三列 -->
          <el-col :span="6" style="height: calc(100%)">
            <!-- 轮播板料追踪部分 -->
            <DetailRightOne ref="detailRightOne" />
            <!-- 设备稼动率 -->
            <DetailRightTwo ref="detailRightTwo" />
            <!-- 设备开机率 -->
            <DetailRightThree ref="detailRightThree" />
          </el-col>
        </el-row>
      </div>
    </div>
  </div>
</template>


<script>
import {
  getDeviceStatusStats,
  getDeviceAlarmStats,
  getPanelStats,
  getDeviceProcessingStats,
  getDeviceMovementStats,
  getDeviceUptimeStats,
} from "@/api";
import ScreenHeader from "@/components/header";
import DetailLeftOne from "@/components/detailLeftOne";
import DetailLeftTwo from "@/components/detailLeftTwo";
import DetailLeftThree from "@/components/detailLeftThree";
import DetailRightOne from "@/components/detailRightOne";
import DetailRightTwo from "@/components/detailRightTwo";
import DetailRightThree from "@/components/detailRightThree";
import { mapState } from "vuex";
export default {
  name: "DetailView",
  components: {
    ScreenHeader,
    DetailLeftOne,
    DetailLeftTwo,
    DetailLeftThree,
    DetailRightOne,
    DetailRightTwo,
    DetailRightThree,
  },
  computed: {
    ...mapState(["deviceId"]),
  },
  data() {
    return {
      // 进度条属性
      percentage: 0,
      customColor: "#409eff",
      loading: true,
      progressTimer: null,
      // 定时刷新
      timer: null,
      detailLeftTwoConfig: {
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
      detailLeftThreeConfig: {
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
    };
  },
  created() {
    this.$nextTick(() => {
      this.progressTimer = setInterval(() => {
        this.percentage += 1;
        if (this.percentage >= 100) {
          this.percentage = 100;
          clearInterval(this.progressTimer);
          this.progressTimer = null;
        }
      }, 1000);
      this.getDetailLeftOne();
      this.getDetailLeftTwo();
      this.getDetailLeftThree();
      this.getDetailRightOne();
      this.getDetailRightTwo();
      this.getDetailRightThree();
    });
  },
  mounted() {
    //定时刷新1分钟
    this.timer = setInterval(() => {
      this.getDetailLeftOne();
      this.getDetailLeftTwo();
      this.getDetailLeftThree();
      this.getDetailRightOne();
      this.getDetailRightTwo();
      this.getDetailRightThree();
    }, 60000);
  },
  // 切换页面销毁定时器
  destroyed() {
    clearInterval(this.timer);
    this.timer = null;
  },
  methods: {
    getDetailLeftOne() {
      console.log(this.deviceId, "111111111");
      getDeviceStatusStats(this.deviceId).then((res) => {
        this.$refs.detailLeftOne.semicircle(res);
      });
    },
    getDetailLeftTwo() {
      getDeviceAlarmStats(this.deviceId).then((res) => {
        // 使用es6...解决DataV改变数据视图不主动刷新
        this.detailLeftTwoConfig = {
          ...this.detailLeftTwoConfig,
          data: res.data,
        };
      });
    },
    getDetailLeftThree() {
      getPanelStats(this.deviceId).then((res) => {
        // 使用es6...解决DataV改变数据视图不主动刷新
        this.detailLeftThreeConfig = {
          ...this.detailLeftThreeConfig,
          data: res.data,
        };
      });
    },
    getDetailRightOne() {
      getDeviceProcessingStats(this.deviceId).then((res) => {
        this.$refs.detailRightOne.line_center_diagram(res);
      });
    },
    getDetailRightTwo() {
      getDeviceMovementStats(this.deviceId).then((res) => {
        this.$refs.detailRightTwo.line_center_diagram(res);
      });
    },
    getDetailRightThree() {
      getDeviceUptimeStats(this.deviceId).then((res) => {
        this.$refs.detailRightThree.line_center_diagram(res);
      });
    },
    increase() {
      this.percentage += 10;
      if (this.percentage > 100) {
        this.percentage = 100;
      }
    },
    decrease() {
      this.percentage -= 10;
      if (this.percentage < 0) {
        this.percentage = 0;
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.device_detail_img {
  width: 100%;
  height: 100%;
}
</style>