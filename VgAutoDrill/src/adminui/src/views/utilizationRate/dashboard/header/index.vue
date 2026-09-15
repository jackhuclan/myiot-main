<template>
  <div class="header">
    <div class="status_box">
      <li v-for="item in statusList" :key="item.value">
        <span :style="{ background: item.color }"></span><span>{{item.label}}</span>
      </li>
    </div>
    <div class="header_title">JSPCB三厂钻机看板</div>
    <div class="times">{{ currentTime }}</div>
  </div>
</template>

<script>
import statusList from "../status";
import dayjs from "dayjs";
export default {
  data() {
    return {
      currentTime: "",
      statusList,
    };
  },
  mounted() {
    this.updateTime();
    // 每秒更新一次时间
    this.interval = setInterval(this.updateTime, 1000);
  },
  beforeDestroy() {
    // 清除定时器，防止内存泄漏
    clearInterval(this.interval);
  },
  methods: {
    updateTime() {
      // 按照“2023年 01月 03日 22:08:56 星期二”的格式更新时间
      this.currentTime = dayjs().format("YYYY年 MM月 DD日 HH:mm:ss dddd");
    },
  },
};
</script>
 
<style lang="scss" scoped >
.header {
  height: 30px;
  background-image: url("../../../../assets/images/header_bg.png");
  background-size: 100% 100%; //背景尺寸
  background-position: center center; //背景位置
  position: relative;
  display: flex;
  .status_box {
    padding-left: 10px;
    height: 20px;
    display: flex;
    font-size: 14px;
    li {
      display: flex;
      align-items: center;
      span:first-child {
        padding: 4px 7px;
        margin-right: 2px;
        border-radius: 2px;
      }
      margin-right: 6px;
    }
  }
  .header_title {
    line-height: 20px;
    position: absolute;
    left: 50%;
    font-size: 18px;
    transform: translateX(-50%);
    text-shadow: 2px 1px 1px #fff;

    position: absolute;
    color: transparent;
    font-style: italic;
    font-weight: 900;
    letter-spacing: 5px;
  }
  .times {
    line-height: 20px;
    position: absolute;
    right: 40px;
    font-size: 14px;
  }
}
</style>