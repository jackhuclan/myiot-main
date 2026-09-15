<template>
  <div class="app-container">
    <el-card>
      <div class="device-container">
        <!-- 图片 -->
        <el-image
          class="img"
          :src="require('@/assets/images/device.png')"
        ></el-image>
        <ul>
          <h1 class="font-bold">设备状态：{{ info.sWorkMode }}</h1>
          <h3 class="font-bold">设备编号：{{ info.sEquipmentID }}#</h3>
          <h5>钻孔进度：{{ info.sPersent }}</h5>
          <h5 style="display: flex; align-items: center">
            推荐指数：
            <el-rate
              allow-half
              :value="info.sDuty / 20"
              disabled
              show-score
              disabled-void-color="#ccc"
            >
            </el-rate>
          </h5>
          <h3 class="font-bold">稼动率：{{ info.sDuty }}%</h3>
          <p>钻带程序：{{ info.sActProgram }}</p>
          <p>参数程序：{{ info.sDiaFileName }}</p>
        </ul>
      </div>
    </el-card>
  </div>
</template>

<script>
import { getEquDrillInformation } from "@/api/home";
export default {
  data() {
    return {
      info: {},
      code: undefined,
    };
  },
  activated() {
    this.getDeviceInfo();
  },
  methods: {
    getDeviceInfo() {
      this.code = this.$route.params && this.$route.params.code;
      getEquDrillInformation({ sEquipmentID: this.code }).then((res) => {
        this.info = res;
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.device-container {
  display: flex;
  align-items: center;
  .img {
    margin-left: 50px;
    width: 300px;
  }
  .font-bold {
    font-weight: bold;
  }
}
::v-deep .el-rate__text {
  display: none;
}
</style>
