<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>
import axios from "axios";
import { getTitle } from "./api/layout";
import settings from "./settings";
import { mapState } from "vuex";
export default {
  name: "App",
  data() {
    return {
      title: this.$store.state.settings.mes_title,
      timer: null,
    };
  },
  computed: {
    ...mapState({
      version_updates_interval: (state) =>
        state.personalized.version_updates_interval * 1000,
    }),
  },
  watch: {
    version_updates_interval: {
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
    // 表单数据操作保存
    changeIsSaved(lastVersion) {
      setInterval(() => {
        const isSaved = localStorage.getItem("isSaved");
        if (isSaved == "true") {
          localStorage.removeItem("isSaved");
          localStorage.setItem("version", lastVersion);
          setTimeout(() => {
            window.location.reload();
          }, 1000);
        } else if (isSaved == "false") {
          localStorage.removeItem("isSaved");
          localStorage.setItem("version", lastVersion);
          setTimeout(() => {
            window.location.reload();
          }, 1000);
        }
      }, 2000);
    },
    changeTitle() {
      getTitle().then((res) => {
        if (res.length > 0) {
          this.title = res[0] + res[1];
          this.$store.dispatch("settings/changeSetting", {
            key: "htmlTitle",
            value: res[0],
          });
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
      }, this.version_updates_interval);
    },
    getVersion() {
      const isLoginPage = this.$route.path == "/login";
      if (process.env.NODE_ENV !== "development") {
        axios
          .get("/version.json", { params: { data: new Date() } })
          .then((res) => {
            let lastVersion = res.data.version;
            localStorage.setItem("lastVersion", lastVersion);
            if (localStorage.version == undefined)
              localStorage.setItem("version", lastVersion);
            if (localStorage.version != lastVersion) {
              if (!isLoginPage && !localStorage.getItem("is_login_page")) {
                clearInterval(this.timer);
                this.timer = null;
                this.$modal
                  .confirm("检测到版本更新，是否刷新页面？")
                  .then(() => {
                    // 表单元素是否保存
                    const isFormModified =
                      localStorage.getItem("isFormModified");
                    if (isFormModified == "true") {
                      this.$notify({
                        title: "提示",
                        type: "warning",
                        message: "有数据未保存,请先保存数据！",
                      });
                    } else {
                      window.location.reload();
                      localStorage.setItem("version", lastVersion);
                    }
                    this.changeIsSaved(lastVersion);
                    // 清空个性化配置本地存储
                    localStorage.removeItem("layout-personalized");
                  })
                  .catch(() => {
                    this.changeInterval();
                  });
              } else {
                localStorage.setItem("is_login_page", true);
                // 清空个性化配置本地存储
                localStorage.removeItem("layout-personalized");
              }
            } else {
              localStorage.removeItem("is_login_page");
            }
          });
      }
    },
    beforeunload() {
      if (process.env.NODE_ENV !== "development") {
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
              localStorage.removeItem("layout-personalized");
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
    this.changeTitle();
    this.changeInterval();
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
<style lang="scss"></style>
