<template>
  <el-row class="header_bg">
    <el-col :span="6" class="left_top">
      <div class="left_btn_box" v-if="route == 'device'">
        <span
          :style="{ opacity: btnIndex == i ? 1 : 0.6 }"
          v-for="(v, i) in list"
          :key="v"
          @click="tabClick(v, i)"
          >{{ v }}</span
        >
      </div>
      <dv-decoration-8 class="title_left" :color="['#008CFF', '#00ADDD']" />
    </el-col>
    <el-col :span="12"
      ><div class="title_text">
        <img src="../../assets/logo.png" alt="" />维 嘉 智 能 钻 孔 车 间
      </div>
      <dv-decoration-5
        :dur="10"
        class="title_center"
        :color="['#008CFF', '#00ADDD']"
    /></el-col>
    <el-col :span="6" class="time_box">
      <div class="title_time">
        <p>{{ dateYear }} {{ dateDay }} {{ dateWeek }}</p>
        <el-button
          v-if="route != 'index'"
          @click="jump"
          icon="el-icon-s-home"
          class="title_right_button"
          circle
        ></el-button>
        <el-button
          v-if="route == 'detail'"
          @click="jump1"
          icon="el-icon-back"
          class="title_right_button1"
          circle
        ></el-button>
      </div>
      <dv-decoration-8
        :reverse="true"
        class="title_right"
        :color="['#008CFF', '#00ADDD']"
    /></el-col>
  </el-row>
</template>

<script>
import { formatTime } from "@/utils";
import { mapActions, mapState } from "vuex";
export default {
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Header",
  props: ["route"],
  // 获取vuex数据
  computed: {
    ...mapState(["btnIndex"]),
  },
  data() {
    return {
      list: ["全部", "钻机", "锣机", "AOI"],
      //定时器
      timing: null,
      //时分秒
      dateDay: "",
      //年月日
      dateYear: "",
      //周几
      dateWeek: "",
      //周几
      weekday: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
    };
  },
  mounted() {
    this.timing = setInterval(() => {
      this.timeFn();
    }, 1000);
  },
  created() {
    this.timeFn();
  },
  beforeDestroy() {
    //离开时删除计时器
    clearInterval(this.timing);
    this.timing = null;
  },
  methods: {
    // 获取vueX方法
    ...mapActions(["changeDeviceType", "changeBtnIndex", "getDeviceList"]),
    //右上角当前日期时间显示：每一秒更新一次最新时间
    timeFn() {
      //获取当前时分秒
      this.dateDay = formatTime(new Date(), "HH: mm: ss");
      // //获取当前年月日
      this.dateYear = formatTime(new Date(), "yyyy-MM-dd");
      //获取当前周几
      this.dateWeek = this.weekday[new Date().getDay()];
    },
    // 返回首页
    jump() {
      this.$router.push({ path: "/" });
    },
    // 返回上一页
    jump1() {
      this.$router.go(-1);
    },
    tabClick(v, i) {
      // MOCK
      this.changeDeviceType(v);
      this.changeBtnIndex(i);
      this.getDeviceList();
    },
  },
};
</script>

<style lang="scss" scoped>
// 头部背景图
.header_bg {
  background-image: url("@/assets/img/bg2.png");
  background-size: cover; //背景尺寸
  background-position: center center; //背景位置
}
//顶部右边装饰效果
.title_right {
  width: 100%;
  height: 50px;
  position: absolute;
  left: 0;
  top: 18px;
}
// 顶部左边装饰效果
.title_left {
  width: 100%;
  height: 50px;
  margin-top: 18px;
}
//顶部中间装饰效果
.title_center {
  width: 100%;
  height: 50px;
}
//顶部中间文字维嘉智能钻孔车间
.title_text {
  img {
    height: 22px;
    margin-right: 5px;
    vertical-align: middle;
  }
  text-align: center;
  font-size: 24px;
  font-weight: bold;
  margin-top: 12px;
  color: #008cff;
}
.time_box {
  position: relative;
}
//时间日期
.title_time {
  height: 40px;
  width: 100%;
  position: absolute;
  left: -20px;
  top: -5px;
  display: flex;
  align-items: center;
  justify-content: center;
  p {
    font-family: YouSheBiaoTiHei !important;
    font-size: 17px;
    font-weight: 400;
    color: #05e8fe;
    white-space: nowrap;
    width: 200px;
  }
}
// 返回首页按钮
.title_right_button {
  position: absolute !important;
  right: 80px !important;
  font-size: 20px !important;
  color: #fff !important;
  border: none !important;
  background: transparent !important;
  z-index: 999 !important;
}
.title_right_button1 {
  position: absolute !important;
  right: 50px !important;
  font-size: 22px !important;
  color: #fff !important;
  border: none !important;
  background: transparent !important;
  z-index: 999 !important;
}
// 左上角
.left_top {
  position: relative;
}
// 左上角设备按钮
.left_btn_box {
  height: 40px;
  position: absolute;
  left: 30px;
  top: -10px;
  display: flex;
  align-items: center;
  z-index: 999;
  span {
    // 鼠标样式
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100px;
    height: 30px;
    font-size: 18px;
    margin: 0 5px;
    background-image: url("@/assets/img/btn.png");
    background-size: 100% 100%; //背景尺寸
    background-position: center center; //背景位置
  }
}
</style>
