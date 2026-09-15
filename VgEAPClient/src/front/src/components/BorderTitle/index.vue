<template>
  <div class="borderTitle" :style="{ border: border ? undefined : 'none' }">
    <span v-if="title">{{ title }}</span>
    <slot></slot>
  </div>
</template>
<script>
export default {
  props: {
    title: {
      type: String,
      default: "",
    },
    border: {
      type: Boolean,
      default: true,
    },
  },
};
</script>
<style lang="scss" scoped>
.borderTitle {
  position: relative;
  border: 1px solid #dcdfe6;
  border-radius: 5px;
  > span {
    z-index: 1;
    position: absolute;
    left: 20px;
    top: -16px; 
    height: 20px;
    padding: 0 5px;
    text-align: center;
    font-size: 12px;
    color: #606266;
    background: #fff; 
  }
  ::v-deep .el-input__inner {
    border: 0;
  }
  ::v-deep .el-textarea__inner {
    border: 0;
    resize: none;
  }
  ::v-deep.el-input__inner::placeholder {
    color: #7a7c7d;
  }

  ::v-deep.el-input__inner::-webkit-input-placeholder {
    color: #7a7c7d;
  }

  ::v-deep.el-input__inner:-moz-placeholder {
    color: #7a7c7d;
  }

  ::v-deep.el-input__inner:-ms-input-placeholder {
    color: #7a7c7d;
  }
}
</style>
