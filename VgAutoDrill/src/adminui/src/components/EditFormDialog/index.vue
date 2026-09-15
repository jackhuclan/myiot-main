<template>
  <el-dialog
    :visible.sync="value"
    :width="editWidth"
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
        <el-button
          :type="submitType"
          @click="submitForm"
          v-if="optType != 'view'"
          >{{ submitText }}</el-button
        >
        <el-button @click="cancel">{{
          optType != "view" ? "取 消" : "返 回"
        }}</el-button>
      </div>
    </div>
    <!-- 插槽 -->
    <div v-loading="loading">
      <slot />
    </div>
    <div slot="footer" class="dialog-footer" ref="dialog_footer">
      <div v-if="!isShowBtn">
        <el-button
          :type="submitType"
          @click="submitForm"
          v-if="optType != 'view'"
          >{{ submitText }}</el-button
        >
        <el-button @click="cancel">{{
          optType != "view" ? "取 消" : "返 回"
        }}</el-button>
      </div>
    </div>
  </el-dialog>
</template>

<script>
export default {
  data() {
    return {
      isShowBtn: false,
      editWidth: "",
    };
  },
  props: {
    open: {
      type: Boolean,
      default: false,
    },
    optType: {
      type: String,
      default: "",
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
    isFormModified: {
      type: Boolean,
      default: false,
    },
    isHasCancel: {
      type: Boolean,
      default: false,
    },
    value: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    isFormModified: {
      handler(val) {
        if (
          !this.value ||
          !val ||
          localStorage.version == localStorage.lastVersion
        ) {
          localStorage.removeItem("isFormModified");
        } else {
          localStorage.setItem("isFormModified", val);
        }
      },
      deep: true,
    },
    value: {
      handler(val) {
        if (!val) {
          localStorage.removeItem("isFormModified");
        } else {
          this.changeWidth();
          localStorage.removeItem("isSaved");
        }
      },
      deep: true,
    },
  },
  mounted() {
    this.editWidth = this.width;
    window.addEventListener("resize", this.changeWidth);
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
    changeWidth() {
      if (this.width.includes("%") && window.innerWidth <= 800) {
        this.editWidth = "780px";
      } else {
        this.editWidth = this.width;
      }
    },
    changeLocal(val) {
      if (localStorage.version != localStorage.lastVersion) {
        localStorage.setItem("isSaved", val);
      } else {
        localStorage.removeItem("isSaved");
      }
    },
    submitForm() {
      const form = this.$slots.default[0]?.child;
      if (this.$slots.default[0]?.data?.ref == "form") {
        form?.validate((valid) => {
          if (valid) {
            if (!this.isFormModified && this.optType == "edit") {
              this.$emit("input", false);
            } else {
              this.$emit("submitForm");
              this.changeLocal(true);
            }
          } else {
            this.$parent.page == "partition" &&
              this.$modal.notifyError("基本信息及转运配置部分信息不正确！");
          }
        });
      } else {
        this.$emit("submitForm");
        this.changeLocal(true);
      }
    },
    cancel() {
      if (this.isHasCancel) return this.$emit("cancel");
      if (this.isFormModified)
        return this.$confirm("表单内容已发生变化, 确定要取消?", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
        })
          .then(() => {
            this.changeLocal(false);
            this.$emit("input", false);
          })
          .catch(() => {});
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
