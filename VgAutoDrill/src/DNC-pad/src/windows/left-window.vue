<template>
  <scroll-view class="sidebar" scroll-y="true">
    <uni-data-menu
      :localdata="localMenus"
      :unique-opened="true"
      :value="currentMenu"
      active-text-color="#409eff"
      @select="select"
    ></uni-data-menu>
  </scroll-view>
</template>

<script>
export default {
  data() {
    return {
      currentMenu: "/",
      localMenus: [
        {
          icon: "home",
          menu_id: "index",
          value: "/",
          text: "首页",
        },
        {
          icon: "map",
          menu_id: "system_user",
          value: "/pages/logs/logs",
          text: "日志",
        },
        {
          icon: "gear",
          menu_id: "settings",
          value: "/pages/setting/setting",
          text: "配置",
        },
      ],
    };
  },
  watch: {
    // #ifdef H5
    $route: {
      immediate: true,
      deep: true,
      handler(newRoute, oldRoute) {
        const path = newRoute.fullPath;
        if (path) {
          this.currentMenu = this.splitFullPath(path);
        }
      },
    },
    // #endif
  },
  methods: {
    select(e) {
      const value = e.value;
      uni.switchTab({
        url: value,
      });
    },
    splitFullPath(path) {
      if (!path) {
        path = "/";
      }
      return path.split("?")[0];
    },
  },
};
</script>

<style lang="scss">
::v-deep .uni-nav-menu {
  width: 100px !important;
}

.sidebar {
  position: fixed;
  // top: var(--top-window-height); // useless
  width: 100px;
  height: calc(100vh - (var(--top-window-height)));
  box-sizing: border-box;
  border-right: 1px solid darken($left-window-bg-color, 8%);
  background-color: $left-window-bg-color;
  padding-bottom: 10px;
}

/* #ifdef H5 */
.sidebar ::-webkit-scrollbar {
  display: none;
  // scrollbar-width: thin;
}

/* #endif */

.title {
  margin-left: 5px;
}
</style>
