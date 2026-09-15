<template>
  <section class="app-main">
    <transition name="fade-transform" mode="out-in">
    <keep-alive :exclude="excludeList">
      <router-view :key="$route.fullPath" />
    </keep-alive>
    </transition>
  </section>
</template>

<script>
export default {
  name: "AppMain",
  data() {
    return { 
      excludeList: [],
    };
  },
  computed: {
    cachedViews() {
      return this.$store.state.tagsView.cachedViews;
    },
    key() {
      return this.$route.path;
    },
  },
};
</script>

<style lang="scss" scoped>
.app-main,
.app-container {
  background: #f0f3fa;
  /* 50= navbar  50  */
  min-height: calc(100vh - 50px);
  width: 100%;
  position: relative;
  overflow: hidden;
}

.fixed-header + .app-main {
  padding-top: 50px;
}

.hasTagsView {
  .app-main,
  .app-container {
    /* 84 = navbar + tags-view = 50 + 34 */
    min-height: calc(100vh - 94px);
  }

  .fixed-header + .app-main {
    padding-top: 94px;
  }
}
</style>

<style lang="scss">
// fix css style bug in open el-dialog
.el-popup-parent--hidden {
  .fixed-header {
    padding-right: 17px;
  }
}
</style>
