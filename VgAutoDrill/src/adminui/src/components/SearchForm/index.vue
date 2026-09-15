<template>
  <el-form
    v-affix="openType"
    :model="form"
    :inline="true"
    ref="queryForm"
    @submit.native.prevent
  >
    <div id="searchFilter" :gutter="10" style="display: flex; flex-wrap: wrap">
      <slot></slot>
      <el-form-item>
        <el-button
          v-debounce
          type="primary"
          icon="el-icon-search"
          @click="handleQuery"
          >查询</el-button
        >
        <el-button v-debounce icon="el-icon-refresh" @click="handleReset"
          >重置</el-button
        >
        <el-button v-show="collapsiable" type="text" @click="shiftCollapsiable">
          <span>
            {{ fold ? "收起" : "展开" }}
            <i :class="fold ? 'el-icon-arrow-up' : 'el-icon-arrow-down'"></i>
          </span>
        </el-button>
      </el-form-item>
    </div>
  </el-form>
</template>
  <script>
import { mapState } from "vuex";

export default {
  name: "SearchForm",
  computed: {
    ...mapState({
      maxShowFormItem: (state) => state.personalized.maxShowFormItem,
    }),
  },
  props: {
    isFixed: {
      type: Boolean,
      default: false,
    },
    openType: {
      default: "默认无openType",
    },
    form: {
      type: Object,
      default: {},
    },
  },
  data() {
    return {
      collapsiable: false,
      fold: false,
    };
  },
  mounted() {
    // 通过最大显示个数控制展开/折叠
    if (this.maxShowFormItem > 0) {
      this.minShowCtrol();
    }
  },
  activated() {
    // 通过最大显示个数控制展开/折叠
    if (this.maxShowFormItem > 0) {
      this.minShowCtrol();
    }
  },
  methods: {
    shiftCollapsiable() {
      this.fold = !this.fold;
      this.minShowCtrol();
    },
    // 通过maxShowFormItem控制元素显示/折叠
    minShowCtrol() {
      const group = window.document.querySelectorAll(
        `#searchFilter .el-form-item`
      );
      const len = group?.length ? group?.length - 1 : 0; 
      if (this.maxShowFormItem < len) {
        group.forEach((item, index) => {
          if (index > this.maxShowFormItem - 1 && index < len) {
            item.style.display = !this.fold ? "none" : "";
          } else {
            item.style.display = "";
          }
        });
        this.collapsiable = true;
      } else {
        group.forEach((item, index) => {
          item.style.display = "";
        });
        this.collapsiable = false;
      }
    },
    handleQuery() {
      this.$emit("search");
    },
    handleReset() {
      this.resetForm("queryForm");
      this.$emit("reset");
    },
  },
};
</script>
  <style lang="scss" scoped></style>
  
  