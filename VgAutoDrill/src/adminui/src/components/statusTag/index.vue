<template>
  <el-tag
    :size="tag_size"
    v-if="options.find((v) => v.value == status)"
    :style="{
      background: options.find((v) => v.value == status).background,
      color: options.find((v) => v.value == status).color,
      border: 'solid 1px' + options.find((v) => v.value == status).color,
    }"
  >
    {{ options.find((v) => v.value == status).label
    }}{{ extend != "" ? "---" + extend : "" }}
  </el-tag>
</template>

<script>
export default {
  props: {
    options: {
      required: true,
      type: Array,
      default: [],
    },
    status: {
      required: true,
      default: 0,
      type: Number,
    },
    //扩展
    extend: {
      default: "",
    },
    size: {
      default: "",
      type: String,
    },
  },
  computed: {
    tag_size() {
      return this.size ? this.size : this.$store.getters.size;
    },
  },
};
</script>

<style></style>
