<template>
  <el-checkbox
    v-if="!item.children"
    style="margin-right: 25px; margin-bottom: 10px"
    v-model="item.visible"
    @change="(e) => handleChange(e, parent)"
    >{{ item.label }}
  </el-checkbox>
  <span v-else>
    <recursion-checkbox
      v-for="(v, i) in item.children"
      :key="v.prop + i"
      :item="v"
      :parent="item"
      @getCheck="handleChange"
    ></recursion-checkbox>
  </span>
</template>

<script>
export default {
  name: "RecursionCheckbox",
  props: {
    item: {
      type: Object,
    },
    parent: {
      type: Object,
    },
  },
  methods: {
    handleChange(e, parent) {
      this.$emit("getCheck", e, parent);
    },
  },
};
</script>

<style lang="scss" scoped></style>
