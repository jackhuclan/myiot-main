<template>
  <el-upload
    class="upload-demo"
    ref="upload"
    action=""
    :show-file-list="true"
    :on-change="onChange"
    :file-list="fileList"
    :auto-upload="false"
    :on-remove="onRemove"
  >
    <el-button v-debounce type="primary">{{ btnText }}</el-button>
  </el-upload>
</template>

<script>
export default {
  props: {
    onChange: {
      type: Function,
      required: true,
    },
    onRemove: {
      type: Function,
      required: true,
    },
    fileList: {
      type: Array,
      required: true,
    },
    btnText: {
      type: String,
      default: "选择文件",
    },
  },
};
</script>

<style lang="scss" scoped></style>
