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
    dateCount(timestamp) {
      let nowTime = new Date();
      let endTime = new Date(timestamp);
      let t = endTime.getTime() - nowTime.getTime();
      if (t > 0) {
        let day = Math.floor(t / 86400000);
        let hour = Math.floor((t / 3600000) % 24);
        let min = Math.floor((t / 60000) % 60);
        let sec = Math.floor((t / 1000) % 60);
        hour = hour < 10 && hour > 0 ? "0" + hour : hour;
        min = min < 10 && min > 0 ? "0" + min : min;
        sec = sec < 10 && sec > 0 ? "0" + sec : sec;
        let format = "";
        if (day > 0) {
          format = `${day}天${hour}时${min}分${sec}秒`;
        }
        if (day <= 0 && hour > 0) {
          format = `${hour}时${min}分${sec}秒`;
        }
        if (day <= 0 && hour <= 0) {
          format = `${min}分${sec}秒`;
        }
        this.content = format;
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
