<template>
  <el-scrollbar
    ref="scrollContainer"
    :vertical="false"
    class="scroll-container"
    @wheel.native.prevent="handleScroll"
  >
    <slot />
  </el-scrollbar>
</template>

<script>
const tagAndTagSpacing = 4; // tagAndTagSpacing

export default {
  name: "ScrollPane",
  data() {
    return {
      left: 0,
    };
  },
  computed: {
    scrollWrapper() {
      return this.$refs.scrollContainer.$refs.wrap;
    },
  },
  mounted() {
    this.scrollWrapper.addEventListener("scrollWrapper", this.emitScroll, true);
  },
  updated() {
    this.scrollWrapper.scrollLeft =
      this.$cache.session.get("scrollWrapper") * 1;
  },
  beforeDestroy() {
    this.scrollWrapper.removeEventListener("scrollWrapper", this.emitScroll);
  },
  methods: {
    handleScroll(e) {
      const eventDelta = e.wheelDelta || -e.deltaY * 40;
      const $scrollWrapper = this.scrollWrapper;
      $scrollWrapper.scrollLeft = $scrollWrapper.scrollLeft + eventDelta / 4;
    },
    emitScroll() {
      this.$emit("scroll");
    },
    moveToTarget(currentTag) {
      const $container = this.$refs.scrollContainer.$el;
      const $containerWidth = $container.offsetWidth;
      const $scrollWrapper = this.scrollWrapper;
      const tagList = this.$parent.$refs.tag;

      let firstTag = null;
      let lastTag = null;

      // find first tag and last tag
      if (tagList.length > 0) {
        firstTag = tagList[0];
        lastTag = tagList[tagList.length - 1];
      }

      if (firstTag === currentTag) {
        $scrollWrapper.scrollLeft = 0;
        this.$cache.session.set("scrollWrapper", 0);
      } else if (lastTag === currentTag) {
        $scrollWrapper.scrollLeft =
          $scrollWrapper.scrollWidth - $containerWidth;
        this.$cache.session.set(
          "scrollWrapper",
          $scrollWrapper.scrollWidth - $containerWidth
        );
      } else {
        // find preTag and nextTag
        const currentIndex = tagList.findIndex((item) => item === currentTag);
        const prevTag = tagList[currentIndex - 1];
        const nextTag = tagList[currentIndex + 1];

        // the tag's offsetLeft after of nextTag
        const afterNextTagOffsetLeft =
          nextTag.$el.offsetLeft + nextTag.$el.offsetWidth + tagAndTagSpacing;

        // the tag's offsetLeft before of prevTag
        const beforePrevTagOffsetLeft =
          prevTag.$el.offsetLeft - tagAndTagSpacing;

        if (
          afterNextTagOffsetLeft >
          $scrollWrapper.scrollLeft + $containerWidth
        ) {
          $scrollWrapper.scrollLeft = afterNextTagOffsetLeft - $containerWidth;
          this.$cache.session.set(
            "scrollWrapper",
            afterNextTagOffsetLeft - $containerWidth
          );
        } else if (beforePrevTagOffsetLeft < $scrollWrapper.scrollLeft) {
          $scrollWrapper.scrollLeft = beforePrevTagOffsetLeft;
          this.$cache.session.set(
            "scrollWrapper",
            afterNextTagOffsetLeft - $containerWidth
          );
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.scroll-container {
  white-space: nowrap;
  position: relative;
  // overflow: hidden;
  width: 100%;
}
// ::v-deep .el-scrollbar__wrap {
//   margin-bottom: -10px !important;
// }
::v-deep .el-scrollbar__view {
  height: 48px;
}
::v-deep .el-scrollbar__bar {
  bottom: 2px !important;
  height: 10px !important;
}
::v-deep .el-scrollbar__bar.is-vertical {
  display: none !important;
}
</style>
