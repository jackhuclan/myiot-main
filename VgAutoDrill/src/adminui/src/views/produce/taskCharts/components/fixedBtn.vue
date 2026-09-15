<template>
  <div class="fixed-btn">
    <el-tooltip effect="dark" content="批量左滑" placement="top">
      <!-- 左边批量按钮 -->
      <el-button
        :disabled="btnLeft"
        class="batchBtn"
        circle
        @click="scrollLeft(batchNum)"
        icon="el-icon-d-arrow-left"
      ></el-button>
    </el-tooltip>
    <el-tooltip effect="dark" content="单个左滑" placement="top">
      <!-- 左边按钮单个 -->
      <el-button
        circle
        :disabled="btnLeft"
        @click="scrollLeft"
        icon="el-icon-caret-left"
      ></el-button>
    </el-tooltip>
    <el-tooltip effect="dark" content="右滑单个" placement="top">
      <!-- 右边按钮单个 -->
      <el-button
        circle
        :disabled="btnRight"
        @click="scrollRight"
        icon="el-icon-caret-right"
      ></el-button>
    </el-tooltip>
    <el-tooltip effect="dark" content="批量右滑" placement="top">
      <!-- 右边批量按钮 -->
      <el-button
        :disabled="btnRight"
        class="batchBtn right"
        circle
        @click="scrollRight(batchNum)"
        icon="el-icon-d-arrow-right"
      ></el-button>
    </el-tooltip>
    <el-tooltip effect="dark" content="回到顶部" placement="top">
      <!-- 回顶部按钮 -->
      <el-button circle @click="goTop" icon="el-icon-caret-top"></el-button>
    </el-tooltip>
  </div>
</template>

<script>
import { scrollTo } from "@/utils/scroll-to";

export default {
  props: {
    btnLeft: {
      type: Boolean,
    },
    btnRight: {
      type: Boolean,
    },
    batchNum: {
      type: Number,
    },
  },
  methods: {
    // 点击回到顶部
    goTop() {
      scrollTo(0, 800);
    },
    scrollLeft(num) {
      this.$emit("scrollLeft", num);
    },
    scrollRight(num) {
      this.$emit("scrollRight", num);
    },
  },
};
</script>

<style lang="scss" scoped>
// 右下角按钮
.fixed-btn {
  position: fixed;
  display: flex;
  bottom: 40px;
  z-index: 28;
  width: 60px;
  > button {
    &:first-child {
      margin-right: 2px;
    }
    margin: 0 2px;
    background-color: #fff;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    // color: #409eff;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 20px;
    box-shadow: 0 0 8px rgba(0, 0, 0, 0.2);
    cursor: pointer;
    z-index: 5;
  }
}
</style>
