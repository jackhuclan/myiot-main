<!-- 拖动提示框 -->
<template>
  <el-dialog
    title="提示"
    :visible.sync="dialogVisible"
    width="400px"
    @close="closeDrag"
  >
    <el-alert
      class="warn_title"
      title="该操作会修改任务顺序, 是否继续?"
      type="error"
      show-icon
      :closable="false"
    >
    </el-alert>
    <span slot="footer" class="dialog-footer">
      <span>
        <el-checkbox v-model="checked">不再提示</el-checkbox>
      </span>
      <el-button :style="{ marginLeft: '10px' }" @click="handleCancelDrag"
        >取 消</el-button
      >
      <el-button type="primary" @click="handleOkDrag">确 定</el-button>
    </span>
  </el-dialog>
</template>

<script>
import Cookies from "js-cookie";
export default {
  props: ["value"],
  data() {
    return {
      checked: false,
      dialogVisible: false,
    };
  },
  methods: {
    closeDrag() {
      this.dialogVisible = false;
      if (this.value) return this.$emit("getList");
    },
    handleOkDrag() {
      Cookies.set("checked", this.checked);
      this.dialogVisible = false;
      this.$emit("moveTask");
    },
    handleCancelDrag() {
      Cookies.set("checked", this.checked);
      this.dialogVisible = false;
      if (this.value) return this.$emit("getList");
    },
  },
};
</script>
