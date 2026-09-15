<template>
  <el-dialog
    :title="title"
    :visible.sync="open"
    width="400px"
    :disabled="isUploading"
    :auto-upload="false"
    :close-on-click-modal="false"
    append-to-body
    v-dialogDrag
    v-dialogClose
  >
    <el-upload
      :file-list="fileList"
      :on-change="onChange"
      drag
      action="#"
      :http-request="onUpload"
      ref="uploadXlsx"
      accept=".xlsx, .xls"
    >
      <i class="el-icon-upload"></i>
      <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
      <div class="el-upload__tip text-center" slot="tip">
        <span>仅允许导入xls、xlsx格式文件。</span>
        <el-link
          type="primary"
          :underline="false"
          style="font-size: 12px; vertical-align: baseline"
          @click="importTemplate"
          >下载模板</el-link
        >
      </div>
    </el-upload>
    <div slot="footer" class="dialog-footer">
      <el-button type="primary" @click="submitFileForm" 
        >确 定</el-button
      >
      <el-button @click="open = false" >取 消</el-button>
    </div>
  </el-dialog>
</template>

<script>
export default {
  data() {
    return {
      fileList: [],
      // 文件流
      formData: null,
      formDataFalg: false,
      // 用户导入参数
      // 是否显示弹出层（用户导入）
      open: false,
      // 弹出层标题（用户导入）
      title: "",
      // 是否禁用上传
      isUploading: false,
    };
  },
  props: ["uploadUrl", 'downloadUrl',"fileName"],
  methods: {
    /** 下载模板操作 */
    importTemplate() {
      this.download(this.downloadUrl, this.fileName);
    },
    onUpload(file) {
      this.formData = new FormData();
      this.formData.append("file", file.file);
      this.formDataFalg = true;
    },
    // 提交上传文件
    submitFileForm() {
      if (this.formDataFalg) {
        this.upload(this.uploadUrl, this.formData).then((res) => {
          this.$emit("getList");
          // 清空上传列表
          this.$refs.uploadXlsx.clearFiles();
        });
        this.open = false;
      } else {
        this.$modal.msgError("未导入任何文件");
        this.formDataFalg = false;
      }
      this.formData = new FormData();
    },
    onChange(file, fileList) {
      // 这是关键一句
      if (fileList.length > 0) {
        this.fileList = [fileList[fileList.length - 1]];
      }
    },
  },
};
</script>

<style scoped></style>
