<template>
  <span>
    <slot>{{ content }}</slot>
  </span>
</template>

<script>
export default {
  data() {
    return {
      content: "",
      timer: null,
    };
  },
  props: {
    //接收父组件的数据
    endTime: { type: String, default: "" },
    endText: { type: String, default: "0" },
  },
  watch: {
    //监听时间的变化
    endTime() {
      this.countdowm(this.endTime);
    },
  },
  mounted() {
    this.countdowm(this.endTime);
    this.dateCount(this.endTime);
  },
  methods: {
    /**
     * 根据指定的t，获取t距离现在过去了多少天
     * @param t     指定的时间
     * @return {any} elapsed 过去的时间
     */

    dateCount(t) {
      let BirthDay = new Date(t);
      //获取当前时时间
      let today = new Date();
      let timeold = today.getTime() - BirthDay.getTime(); //总豪秒数
      if (timeold > 0) {
        let e_daysold = timeold / (24 * 60 * 60 * 1000);
        let daysold = Math.floor(e_daysold); //相差天数
        let e_hrsold = (e_daysold - daysold) * 24;
        let hrsold = Math.floor(e_hrsold); //相差小时数
        let e_minsold = (e_hrsold - hrsold) * 60;
        let minsold = Math.floor(e_minsold); //相差分钟数
        let seconds = Math.floor((e_minsold - minsold) * 60); //相差秒数
        //将所获取的时间拼接到一起，再把值显示到页面
        this.content =
          "已完成:" +
          daysold +
          "天" +
          hrsold +
          "小时" +
          minsold +
          "分" +
          seconds +
          "秒";
      } else {
        clearInterval(this.timer);
        this.content = this.endText;
      }
    },
    countdowm(timestamp) {
      let self = this;
      this.timer = setInterval(() => {
        setTimeout(() => {
          self.dateCount(timestamp); //调用接口的方法
        }, 0);
      }, 1000);
    },
  },
};
</script>
