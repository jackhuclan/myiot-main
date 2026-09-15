<template>
  <div class="left_two">
    <dv-border-box-12 class="p-20">
      <p class="title">
        设备统计
        <el-button size="mini" class="all_btn" @click="jumpDevice">
          更多
        </el-button>
      </p>

      <div class="device_item" v-for="item in list" :key="item.id">
        <li>
          <span>{{ item.deviceTypeName }}</span
          ><span>{{ item.allDeviceCount }}台</span>
        </li>
        <el-progress :percentage="item.onlineDevicePr"></el-progress>
      </div>
    </dv-border-box-12>
  </div>
</template>

<script>
export default {
  name: "LeftTwo",
  props: ["list"],
  methods: {
    // 跳转设备详情
    jumpDevice() {
      this.$router.push("/device");
      this.$store.dispatch("changeBtnIndex", 0);
      this.$store.dispatch("changeDeviceType", "全部");
    },
  },
};
</script>

<style lang="scss" scoped>
.left_two {
  height: 25%;
}
.all_btn {
  font-size: 20px !important;
  border: none !important;
  background: transparent !important;
  font-weight: bold !important;
  letter-spacing: 0.2rem !important;
  color: #fff;
}
.device_item {
  width: 100%;
  display: flex;
  flex-direction: column;
  margin-bottom: 10px;
  li {
    height: 30px;
    align-items: center;
    width: 100%;
    display: flex;
    justify-content: space-between;
    font-size: 18px;
  }
}
</style>

