<template>
  <div class="header">
    <div class="status_box">
      <li><span></span><span>在线</span></li>
      <li><span></span><span>离线</span></li>
      <li><span></span><span>待机</span></li>
      <li><span></span><span>运行中</span></li>
      <li><span></span><span>报警中</span></li>
    </div>
    <div class="header_title">PCB钻孔车间</div>
    <div class="times">{{ currentTime }}</div>
  </div>
</template>

<script>
import dayjs from "dayjs";
export default {
  data() {
    return {
      currentTime: "",
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
  height: 60px;
  background-image: url("@/assets/img/header_bg.png");
  background-size: 100% 100%; //背景尺寸
  background-position: center center; //背景位置
  position: relative;
  display: flex;
  .status_box {
    padding-left: 10px;
    height: 40px;
    display: flex;
    font-size: 18px;
    li {
      display: flex;
      align-items: center;
      span:first-child {
        padding: 8px 16px;
        background: green;
        margin-right: 10px;
        border-radius: 5px;
      }
      margin-right: 20px;
    }
    li:nth-child(1) {
      span:first-child {
        background: #36a3f7;
      }
    }
    li:nth-child(2) {
      span:first-child {
        background: #666;
      }
    }
    li:nth-child(3) {
      span:first-child {
        background: #ff7301;
      }
    }
    li:nth-child(4) {
      span:first-child {
        background: green;
      }
    }
    li:nth-child(5) {
      span:first-child {
        background: red;
      }
    }
  }
  .header_title {
    line-height: 50px;
    position: absolute;
    left: 50%;
    font-size: 24px;
    transform: translateX(-50%);
    text-shadow: 2px 1px 1px #fff;

    position: absolute;
    font-size: 24px;
    color: transparent;
    font-style: italic;
    font-weight: 900;
    letter-spacing: 5px;
  }
  .times {
    line-height: 50px;
    position: absolute;
    right: 40px;
    font-size: 16px;
  }
}
</style>