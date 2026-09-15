<!-- 表格 el-table-column 组件 -->
<template>
  <el-table-column
    v-if="
      (!localColumnData.children || localColumnData.children.length === 0) &&
      localColumnData.visible
    "
    v-bind="localColumnData"
  >
    <template slot-scope="scope">
      <!-- 渲染自定义函数 -->
      <renderComp
        v-if="localColumnData.render"
        :render="localColumnData.render"
        :row="scope.row"
        :index="scope.$index"
        :sc="scope"
        :column="scope.column"
      />

      <!-- 默认渲染 -->
      <span v-else>{{ scope.row[localColumnData.prop] }}</span>
    </template>
  </el-table-column>

  <el-table-column v-else-if="localColumnData.visible" v-bind="localColumnData">
    <recursive-column
      v-for="child in localColumnData.children"
      :key="child.prop + timeStamp"
      :columnData="child"
    ></recursive-column>
  </el-table-column>
</template>

<script>
import { renderComp } from "./renderComp.js";
export default {
  name: "recursive-column",
  components: { renderComp },
  props: {
    columnData: {
      type: Object,
      required: true,
      default: () => ({
        label: "表头未定义",
        prop: Math.random().toString(36).substr(2, 9),
        fixed: false,
        align: "left",
        showOverflowTooltip: true,
        resizable: true, // 是否可拖动用来改变列宽度 
        children: null,
      }),
    },
  },
  computed: {
    localColumnData() {
      return {
        ...this.$options.props.columnData.default(),
        ...this.columnData,
      };
    },
  },
};
</script>
