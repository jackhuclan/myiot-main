<template>
  <div>
    <el-input
      placeholder="输入关键字进行过滤"
      v-model="inpValue"
      clearable
      prefix-icon="el-icon-search"
      style="margin-bottom: 20px"
    >
    </el-input>

    <el-tree
      :expand-on-click-node="false"
      :data="options"
      :props="defaultProps"
      :filter-node-method="filterNode"
      ref="tree"
      default-expand-all
      :highlight-current="highlightCurrent"
      @node-click="handleNodeClick"
      node-key="id"
      ><template #default="{ data }">
        <div class="tree-div">
          <span>{{ data.label }}</span
          ><span v-if="data.status != 1">(停用)</span>
        </div>
      </template>
    </el-tree>
  </div>
</template>

<script>
export default {
  props: {
    placeholder: {
      type: String,
      default: "",
    },
    filterNode: {
      type: Function,
    },
    options: {
      type: Array,
    },
    defaultProps: {
      type: Object,
    },
    handleNodeClick: {
      type: Function,
    },
    highlightCurrent: {
      type: Boolean,
      default: true,
    },
    value: {
      type: String,
      default: "",
    },
  },
  watch: {
    // 根据名称筛选树
    inpValue(val) {
      this.$refs.tree.filter(val);
    },
  },
  computed: {
    inpValue: {
      get() {
        return this.value;
      },
      set(val) {
        this.$emit("input", val);
      },
    },
  },
};
</script>

<style>
.el-tree-node__content {
  height: 25px;
}
.tree-div {
  font-size: 14px;
  width: 100%;
  overflow-x: auto;
  display: flex;
  align-items: center;
  height: 25px;
}
.tree-div::-webkit-scrollbar {
  height: 4px;
}
.tree-div span:nth-child(2) {
  font-size: 12px;
  color: #f45050;
}
</style>
