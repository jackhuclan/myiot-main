<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>
import axios from "axios";
import { mapState } from "vuex";
import { getTitle } from "./api/layout";
export default {
  name: "App",
  created() {
    this.$store.dispatch("GenerateRoutes");
  },
  data() {
    return {
      title: "维嘉数控",
      timer: null,
    };
  },
  computed: {
    ...mapState({
      versionUpdatesTime: (state) => state.settings.versionUpdatesTime * 1000,
    }),
  },
  watch: {
    versionUpdatesTime: {
      handler(val) {
        if (this.timer) {
          clearInterval(this.timer);
          this.timer = null;
        }
        this.timer = setInterval(() => {
          setTimeout(() => {
            this.getVersion(); //调用接口的方法
          }, 0);
        }, val);
      },
      deepL: true,
      immediate: true,
    },
  },
  methods: {
    changeTitle() {
      getTitle().then((res) => {
        if (res.length > 0) {
          this.title = res[0] + res[1];
        }
      });
    },
    changeInterval() {
      if (this.timer) {
        clearInterval(this.timer);
        this.timer = null;
      }
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.getVersion(); //调用接口的方法
        }, 0);
      }, this.versionUpdatesTime);
    },
    getVersion() {
      if (process.env.NODE_ENV == "development") {
        axios
          .get("/version.json", { params: { data: new Date() } })
          .then((res) => {
            let lastVersion = res.data.version;
            localStorage.setItem("lastVersion", lastVersion);
            if (localStorage.version == undefined)
              localStorage.setItem("version", lastVersion);
            if (localStorage.version != lastVersion) {
              clearInterval(this.timer);
              this.timer = null;
              this.$modal
                .confirm("检测到版本更新，是否刷新页面？")
                .then(() => {
                  window.location.reload();
                  localStorage.setItem("version", lastVersion);
                })
                .catch(() => {
                  this.changeInterval();
                });
            }
          });
      }
    },
    beforeunload() {
      if (process.env.NODE_ENV == "development") {
        clearInterval(this.timer);
        this.timer = null;
        axios
          .get("/version.json", { params: { data: new Date() } })
          .then((res) => {
            let lastVersion = res.data.version;
            if (localStorage.lastVersion == undefined)
              localStorage.setItem("lastVersion", lastVersion);
            if (localStorage.version == undefined)
              localStorage.setItem("version", lastVersion);
            if (localStorage.version != lastVersion) {
              // 清空个性化配置本地存储
              localStorage.setItem("version", res.data.version);
              localStorage.setItem("lastVersion", res.data.version);
            }
          });
      }
    },
  },
  beforeDestroy() {
    clearInterval(this.timer);
    this.timer = null;
    document.removeEventListener("keydown", this.beforeunload);
  },
  mounted() {
    this.changeInterval();
    // this.changeTitle();
    window.addEventListener("beforeunload", this.beforeunload);
  },
  metaInfo() {
    let str = "";
    if (
      this.$store.state.settings.dynamicTitle &&
      this.$store.state.settings.title
    ) {
      str = this.title + "-" + this.$store.state.settings.title;
    } else {
      str = this.title;
    }
    return {
      title: str,
    };
  },
};
</script> 