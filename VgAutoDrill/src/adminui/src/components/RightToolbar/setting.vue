<template>
  <div class="top-right-btn">
    <div class="button_box">
      <el-tooltip
        class="item"
        effect="dark"
        :content="showSearch ? '隐藏搜索' : '显示搜索'"
        placement="top"
      >
        <el-button
          size="mini"
          circle
          icon="el-icon-search"
          @click="toggleSearch()"
          class="search"
        />
      </el-tooltip>
      <el-tooltip class="item" effect="dark" content="刷新" placement="top">
        <el-button
          v-debounce
          size="mini"
          circle
          icon="el-icon-refresh"
          @click="refresh()"
          class="refresh"
        />
      </el-tooltip>
      <el-tooltip
        class="item"
        effect="dark"
        content="列设置"
        placement="top"
        v-if="columns"
      >
        <el-button
          class="isShow"
          size="mini"
          circle
          icon="el-icon-menu"
          @click="showColumn()"
        />
      </el-tooltip>
    </div>
    <el-dialog
      :title="title"
      :visible.sync="open"
      append-to-body
      :close-on-click-modal="false"
      v-dialogClose
      v-dialogDrag
      class="transfer_dialog"
      width="750px"
    >
      <el-row>
        <el-col :span="16">
          <li
            style="
              text-align: center;
              color: #333;
              font-weight: bold;
              margin-bottom: 10px;
            "
          >
            显示/隐藏列
          </li>
          <div style="max-height: 400px; overflow: auto">
            <recursion-checkbox
              v-for="(item, index) in columnAll"
              :item="item"
              :key="index"
              :parent="item"
              @getCheck="getCheck"
              @changeMinWidth="changeMinWidth"
            ></recursion-checkbox>
          </div>
          <div style="color: #999; margin-top: 10px">
            <li>使用方法:</li>
            <li>1:左侧勾选需要的列;</li>
            <li>2:右侧可拖动到想要的位置和顺序;</li>
            <li>
              注：<span style="color: red; font-size: 14px; font-weight: bold"
                >只能同级拖拽</span
              >
            </li>
          </div>
        </el-col>
        <el-col :span="8" style="border-left: #ccc 1px solid">
          <li
            style="
              text-align: center;
              color: #333;
              font-weight: bold;
              margin-bottom: 10px;
            "
          >
            拖拽排列
          </li>
          <div style="max-height: 300px; overflow: auto">
            <el-tree
              :data="columnAll"
              node-key="id"
              :expand-on-click-node="false"
              draggable
              :allow-drop="allowDrop"
              @node-drop="nodeDrop"
            >
              <span slot-scope="{ node, data }">
                <div :class="data.visible ? '' : 'visible'">
                  <i
                    class="el-icon-rank"
                    :style="{
                      color: data.visible ? '#1890ff' : '',
                      marginRight: '5px',
                    }"
                  ></i
                  >{{ node.label }}
                  <span style="color: red; margin-left: 5px">{{
                    data.fixed == "left" ? "(左侧固定)" : ""
                  }}</span>
                </div>
              </span>
            </el-tree>
          </div>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>
<script>
import RecursionCheckbox from "./RecursionCheckbox.vue";
export default {
  name: "RightToolbar",
  components: { RecursionCheckbox },
  data() {
    return {
      // 显隐数据
      // value: [],
      // 弹出层标题
      title: "列设置",
      // 是否显示弹出层
      open: false,
      columnAll: [], //表头总列
      dialogVisible: false,
    };
  },
  props: {
    showSearch: {
      type: Boolean,
      default: true,
    },
    columns: {
      type: Array,
    },
    value: {
      type: Array,
    },
    page: {
      type: String,
    },
  },
  created() {
    if (this.$cache.session.get(this.page + "-columns")) {
      // 显隐列初始默认隐藏列
      this.columnAll = this.getColumns(this.columns, this.page);
    } else {
      // 显隐列初始默认隐藏列
      this.columnAll = this.columns;
    }
  },
  methods: {
    // 搜索
    toggleSearch() {
      this.$emit("update:showSearch", !this.showSearch);
    },
    // 刷新
    refresh() {
      this.$emit("queryTable", "search");
    },

    // 拖拽时判定目标节点能否被放置
    // 'prev'、'inner' 和 'next'，前、插入、后
    allowDrop(draggingNode, dropNode, type) {
      //注掉的是同级拖拽
      if (draggingNode.level === dropNode.level) {
        return type === "prev" || type === "next";
      } else {
        // 不同级进行处理
        return false;
      }
    },

    getCheck(e, parent) {
      if (parent.children && parent.children.length > 0) {
        const flag = parent.children.some((v) => v.visible);
        parent.visible = flag;
      }

      this.dataChange();
    },
    changeMinWidth() {
      this.dataChange();
    },
    nodeDrop() {
      this.dataChange();
    },
    dataChange() {
      for (let i = 0; i < this.columnAll.length; i++) {
        this.columns[i] = this.columnAll[i];
      }
      this.$cache.session.setJSON(this.page + "-columns", this.columnAll);
      this.$nextTick(() => {
        this.$parent.$parent
          ? (this.$parent.$parent.timeStamp = new Date().getTime() + "")
          : undefined;
        //带有树形结构的页面与其他页面多出几个父级
        this.$parent.$parent.$refs[this.page]
          ? this.$parent.$parent.$refs[this.page]?.doLayout()
          : this.$parent.$parent.$parent.$parent.$refs[this.page]?.doLayout();
      });
    },
    // 打开显隐列dialog
    showColumn() {
      this.open = true;
    },
  },
};
</script>
<style scoped>
.visible {
  text-decoration: line-through;
}
::v-deep .el-dialog__body {
  padding: 0 20px 20px 20px !important;
}
</style>
