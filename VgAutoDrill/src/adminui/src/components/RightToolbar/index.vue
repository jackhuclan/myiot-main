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
        content="显隐列"
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
    <!-- <FormDialog
      v-model="open"
      :title="title"
      width="650px"
      optType="view"
    >
      <el-transfer
        :titles="['显示', '隐藏']"
        v-model="value"
        :data="columns"
        @change="dataChange"
      ></el-transfer>
    </FormDialog> -->
    <el-dialog
      :title="title"
      :visible.sync="open"
      append-to-body
      :close-on-click-modal="false"
      v-dialogClose
      v-dialogDrag
      class="transfer_dialog"
      width="650px"
    >
      <el-transfer
        :titles="['显示', '隐藏']"
        v-model="value"
        :data="columns"
        @change="dataChange"
      ></el-transfer>
    </el-dialog>
  </div>
</template>
<script>
import cos from "highlight.js/lib/languages/cos";

export default {
  name: "RightToolbar",
  data() {
    return {
      // 显隐数据
      value: [],
      // 弹出层标题
      title: "显示/隐藏",
      // 是否显示弹出层
      open: false,
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
    page: {
      type: String,
    },
  },
  created() {
    if (this.$cache.session.get(this.page + "-columns")) {
      // 显隐列初始默认隐藏列
      for (let item in JSON.parse(
        this.$cache.session.get(this.page + "-columns")
      )) {
        if (
          JSON.parse(this.$cache.session.get(this.page + "-columns"))[item]
            .visible === false
        ) {
          this.value.push(parseInt(item));
        }
      }
    } else {
      // 显隐列初始默认隐藏列
      for (let item in this.columns) {
        if (this.columns[item].visible === false) {
          this.value.push(parseInt(item));
        }
      }
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
    // 右侧列表元素变化
    dataChange(data) {
      for (let item in this.columns) {
        const key = this.columns[item].key;
        this.columns[item].visible = !data.includes(key);
        this.$cache.session.setJSON(
          this.page + "-columns",
          this.columns.map((v) => {
            return { label: v.label, visible: v.visible };
          })
        );
      }
      // 解决表格列动态显示隐藏后错位问题
      this.$nextTick(() => {
        this.$parent.$parent.$refs[this.page]
          ? this.$parent.$parent.$refs[this.page]?.doLayout()
          : this.$parent.$parent.$parent.$parent.$refs[this.page]?.doLayout(); //带有树形结构的页面与其他页面多出几个父级
      });
    },
    // 打开显隐列dialog
    showColumn() {
      this.open = true;
    },
  },
};
</script>
<style>
.transfer_dialog .el-dialog__body {
  display: flex;
  align-items: center;
  justify-content: center;
}
.transfer_dialog .el-transfer__buttons {
  margin: 0;
  padding: 0;
}
</style>
