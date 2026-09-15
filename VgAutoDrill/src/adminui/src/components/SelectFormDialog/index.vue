<template>
  <el-dialog
    :visible.sync="value"
    :width="width"
    append-to-body
    :close-on-click-modal="false"
    v-dialogClose
    v-dialogDrag
    :center="center"
    :before-close="cancel"
    :show-close="!isShowBtn"
  >
    <!-- 自定义标题 -->
    <div slot="title" class="dialog-title">
      <div>
        <span>{{ title }}</span>
      </div>
      <div v-if="isShowBtn">
        <el-button :type="submitType" @click="submitForm">{{
          submitText
        }}</el-button>
        <el-button @click="cancel">取 消</el-button>
      </div>
    </div>
    <!-- 插槽 -->
    <div v-loading="loading">
      <slot />
    </div>
    <div slot="footer" class="dialog-footer" ref="dialog_footer">
      <div v-if="!isShowBtn">
        <el-button :type="submitType" @click="submitForm">{{
          submitText
        }}</el-button>
        <el-button @click="cancel">取 消</el-button>
      </div>
    </div>
  </el-dialog>
</template>

<script>
export default {
  data() {
    return {
      isShowBtn: false,
    };
  },
  props: {
    open: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
      default: "",
    },
    submitText: {
      type: String,
      default: "确 定",
    },
    submitType: {
      type: String,
      default: "primary",
    },
    width: {
      type: String,
      default: "960px",
    },
    center: {
      type: Boolean,
      default: false,
    },
    loading: {
      type: Boolean,
      default: false,
    },

    value: {
      type: Boolean,
      default: false,
    },
  },

  mounted() {
    if ("IntersectionObserver" in window) {
      const observer = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting) {
          // 元素进入可视区域内
          // 执行相应操作
          this.$nextTick(() => {
            this.isShowBtn = false;
          });
        } else {
          // 元素离开可视区域
          // 执行相应操作
          this.$nextTick(() => {
            this.isShowBtn = true;
          });
        }
      });

      observer.observe(this.$refs.dialog_footer);
    }
  },
  methods: {
    submitForm() {
      this.$emit("submitForm");
    },
    cancel() {
      this.$emit("input", false);
    },
  },
};
</script>

<style lang="scss" scoped>
.dialog-title {
  display: flex;
  justify-content: space-between;
}
</style>
