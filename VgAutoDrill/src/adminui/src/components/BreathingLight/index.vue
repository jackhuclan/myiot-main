<template>
  <div>
    <span>{{ label }}</span>
    <el-badge
      class="item"
      ref="badgePage"
      :class="status ? 'flash' : ''"
      :type="status ? undefined : 'info'"
      is-dot
    >
    </el-badge>
  </div>
</template>

<script>
export default {
  props: {
    label: {
      type: String,
      default: "",
    },
    status: {
      type: Boolean,
      default: true,
    },
    color: {
      type: String,
      default: "red",
    },
  },
  mounted() {
    this.$nextTick(function () {
      this.$refs.badgePage.$el.style.setProperty("--color", this.color);
    });
  },
};
</script>

<style lang="scss" scoped>
/*闪烁动画*/
@keyframes twinkle {
  0% {
    box-shadow: 0 0 0 0 var(--color);
  }
  70% {
    box-shadow: 0 0 0 5px rgba(255, 0, 0, 0);
  }
  100% {
    box-shadow: 0 0 0 0 rgba(255, 0, 0, 0);
  }
}
.item {
  margin: -4px 15px 0px 10px;
  ::v-deep .el-badge__content {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 14px; /* 设置宽度 */
    height: 14px; /* 设置高度 */
    border-radius: 50%; /* 设置圆角成圆形 */
  }
}
.flash {
  ::v-deep .el-badge__content {
    background: var(--color);
    animation: twinkle 1s infinite !important;
  }
}
</style>
