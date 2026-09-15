<template>
  <div
    :class="classObj"
    class="app-wrapper"
    :style="{ '--current-color': theme }"
  >
    <div
      v-if="device === 'mobile' && sidebar.opened"
      class="drawer-bg"
      @click="handleClickOutside"
    />
    <sidebar v-if="!sidebar.hide" class="sidebar-container" />
    <div
      :class="{ hasTagsView: needTagsView, sidebarHide: sidebar.hide }"
      class="main-container"
    >
      <div :class="{ 'fixed-header': fixedHeader }">
        <navbar />
        <tags-view v-if="needTagsView" />
      </div>
      <app-main />
      <right-panel>
        <settings />
      </right-panel>
    </div>
    <alarm-dialog
      ref="AlarmDialog"
      v-model="isShowMsg"
      @mouseEnter="mouseEnter"
      @mouseLeave="mouseLeave"
    ></alarm-dialog>
  </div>
</template>
<script>
import RightPanel from "@/components/RightPanel";
import {
  AppMain,
  Navbar,
  Settings,
  Sidebar,
  TagsView,
  AlarmDialog,
} from "./components";
import ResizeMixin from "./mixin/ResizeHandler";
import { mapState } from "vuex";
import variables from "@/assets/styles/variables.scss";
import LeftAlter from "./components/LeftAlter";
import { getSysTimelyInformation } from "@/api/layout";
export default {
  data() {
    return {
      isShowMsg: false,
      // 循环定时
      timer: null,
      // 自动关闭
      timeOut: null,
      // 滑出关闭
      closeTimeOut: null,
    };
  },
  name: "Layout",
  components: {
    AppMain,
    Navbar,
    RightPanel,
    Settings,
    Sidebar,
    TagsView,
    LeftAlter,
    AlarmDialog,
  },
  mixins: [ResizeMixin],
  computed: {
    ...mapState({
      theme: (state) => state.settings.theme,
      sideTheme: (state) => state.settings.sideTheme,
      sidebar: (state) => state.app.sidebar,
      device: (state) => state.app.device,
      needTagsView: (state) => state.settings.tagsView,
      fixedHeader: (state) => state.settings.fixedHeader,
      alarmInfoTime: (state) => state.personalized.alarmInfoTime*1000,
    }),

    classObj() {
      return {
        hideSidebar: !this.sidebar.opened,
        openSidebar: this.sidebar.opened,
        withoutAnimation: this.sidebar.withoutAnimation,
        mobile: this.device === "mobile",
      };
    },
    variables() {
      return variables;
    },
  },
  watch: {
    alarmInfoTime: {
      handler(val) {
        if (this.timer) {
          clearInterval(this.timer);
          this.timer = null;
        }
        this.timer = setInterval(() => {
          setTimeout(() => {
            this.getList(); //调用接口的方法
          }, 0);
        }, val);
      },
      deepL: true,
      immediate: true,
    },
  },
  mounted() {
    this.getList();
  },
  methods: {
    handleClickOutside() {
      this.$store.dispatch("app/closeSideBar", { withoutAnimation: false });
    },
    // 鼠标进入LeftAlter
    mouseEnter() {
      // console.log("滑入");
      clearInterval(this.timer);
      clearTimeout(this.timeOut);
      clearTimeout(this.closeTimeOut);
      this.closeTimeOut = null;
      this.timer = null;
      this.timeOut = null;
    },
    // 鼠标离开LeftAlter
    mouseLeave() {
      // console.log("滑出");
      clearInterval(this.timer);
      clearTimeout(this.timeOut);
      this.timer = null;
      this.timeOut = null;
      this.closeTimeOut = setTimeout(() => {
        this.showNotify();
        this.isShowMsg = false;
      }, 2000);
    },
    // 获取LeftAlter相关信息
    showNotify() {
      if (this.timer) {
        clearInterval(this.timer);
        this.timer = null;
      }
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.getList(); //调用接口的方法
        }, 0);
      }, this.alarmInfoTime);
    },
    getList() {
      getSysTimelyInformation()
        .then((res) => {
          // 判断是否出现LeftAlter
          if (
            res.data.alarmTimelyInfo &&
            res.data.alarmTimelyInfo.alarmInfoList.length > 0
          ) {
            this.isShowMsg = true;
            this.$refs.AlarmDialog.getList();
            this.timeOut = setTimeout(() => {
              this.isShowMsg = false;
            }, 5000);
          } else {
            this.isShowMsg = false;
          }
        })
        .catch(() => {
          this.mouseEnter();
        });
    },
  },
};
</script>
<style lang="scss">
.notifyStyle {
  padding: 0 !important;
  width: 400px !important;
  .el-notification__group {
    .el-notification__content {
      margin: 0 !important;
    }
    width: 100% !important;
    margin: 0 !important;
    padding: 0 !important;
  }
}
</style>
<style lang="scss" scoped>
@import "~@/assets/styles/mixin.scss";
@import "~@/assets/styles/variables.scss";
.app-wrapper {
  @include clearfix;
  position: relative;
  height: 100%;
  width: 100%;
  &.mobile.openSidebar {
    position: fixed;
    top: 0;
  }
}
.drawer-bg {
  background: #000;
  opacity: 0.3;
  width: 100%;
  top: 0;
  height: 100%;
  position: absolute;
  z-index: 999;
}
.fixed-header {
  position: fixed;
  top: 0;
  right: 0;
  z-index: 9;
  width: calc(100% - #{$base-sidebar-width});
  transition: width 0.28s;
}
.hideSidebar .fixed-header {
  width: calc(100% - 54px);
}
.sidebarHide .fixed-header {
  width: 100%;
}
.mobile .fixed-header {
  width: 100%;
}
</style>

