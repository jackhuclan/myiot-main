<template>
  <div>
    <el-tooltip effect="dark" :disabled="isShowTooltip" :placement="placement">
      <div slot="content" ref="content" style="word-wrap: break-word">
        {{ value }}
      </div>
      <div class="remark" @mouseover="onMouseOver()">
        <span ref="remark">{{ value }}</span>
      </div>
    </el-tooltip>
  </div>
</template>

<script>
export default {
  name: "MyTooltip",
  props: {
    value: {
      required: true,
      default: "",
    },
    placement: {
      default: "top",
    },
  },
  data() {
    return {
      isShowTooltip: false,
    };
  },
  methods: {
    onMouseOver() {
      const parentWidth = this.$refs.remark.parentNode.offsetWidth; // 获取元素父级可视宽度
      const contentWidth = this.$refs.remark.offsetWidth; // 获取元素可视宽度
      this.$refs.content.style.width = parentWidth + "px";
      this.isShowTooltip = contentWidth <= parentWidth;
    },
  },
};
</script>

<style lang="scss" scoped>
// 自定义表格tooltip
.remark {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
