<template>
  <uni-popup
    type="dialog"
    @change="handleChangePopup"
    ref="popup"
    :is-mask-click="false"
    background-color="#fff"
  >
    <view class="popup-content">
      <uni-icons
        type="closeempty"
        class="close-fixed"
        @click="$refs.popup.close()"
      ></uni-icons>
      <view class="header">
        <view class="title">刀盒校验</view>
        <uni-row style="margin: 5px 0">
          <uni-col :span="16"
            >配刀组编号：<text class="iscopy">{{ groupNo }}</text></uni-col
          >
          <uni-col :span="1.5" v-if="comparisonRes == '待验证'">
            <button
              class="res-button"
              :style="{
                background: 'yellow',
                color: '#000',
              }"
            >
              待验证
            </button>
          </uni-col>
          <uni-col :span="1.5" v-else-if="comparisonRes == 'ok'">
            <button
              class="res-button"
              :style="{
                background: 'green',
                color: '#000',
              }"
            >
              OK
            </button>
          </uni-col>
          <uni-col :span="1.5" v-if="comparisonRes == 'ng'">
            <button
              class="res-button"
              :style="{
                background: 'red',
                color: '#000',
              }"
            >
              NG
            </button>
          </uni-col>
        </uni-row>
        <uni-row :gutter="20">
          <uni-col :span="1.5">
            <button @click="handleClear" class="uni-button" size="mini">
              一键清空
            </button>
          </uni-col>
          <uni-col :span="1.5">
            <button @click="getCodeList" class="uni-button" size="mini">
              获取系统刀盒码
            </button>
          </uni-col>
          <uni-col :span="1.5">
            <button @click="handleComparison" class="uni-button" size="mini">
              比对上传
            </button>
          </uni-col>
        </uni-row>
      </view>
      <uni-row :gutter="20">
        <uni-col :span="8"
          ><view class="col-container">
            <view class="col-header">
              <text> 扫码:</text>
              <uni-easyinput
                :focus="focusShow"
                style="width: 268px; margin-right: 10px"
                v-model="inputValue"
                @confirm="handleEnter"
                @blur="handleBlur"
              />
            </view>
            <view class="col-main">
              <view
                v-for="(item, index) in currList"
                :key="index"
                :style="{ color: item.flag ? 'red' : '' }"
              >
                {{ index + 1 }}.
                <text class="iscopy">{{ item.val }}</text></view
              >
            </view>
          </view></uni-col
        >
        <uni-col :span="8"
          ><view class="col-container">
            <view class="col-header">校验结果:</view>
            <view class="col-main">
              <text
                class="iscopy"
                v-for="(item, index) in resList"
                :key="index"
                :style="{ color: item.flag ? 'green' : 'red' }"
              >
                {{
                  !item.resMessage
                    ? `第${item.num} 个码${item.val} `
                    : item.resMessage
                }}
              </text>
            </view>
          </view></uni-col
        >
        <uni-col :span="8"
          ><view class="col-container">
            <view class="col-header">系统上的二维码:</view>
            <view class="col-main">
              <text
                class="iscopy"
                v-for="(item, index) in sysList"
                :key="index"
              >
                {{ item }}</text
              >
            </view>
          </view></uni-col
        >
      </uni-row></view
    >
  </uni-popup>
</template>

<script>
import { getAptBoxBarcode, confirmAptBoxBarcode } from "@/api/home";
export default {
  data() {
    return {
      showMask: false,
      // 扫码
      inputValue: "",
      // 配刀组编号
      groupNo: "",
      // 当前扫码
      currList: [],
      // 系统刀盒码
      sysList: [],
      // 校验结果
      resList: [],
      // 比对结果
      comparisonRes: "待验证",
      // 文本框自动聚焦
      focusShow: true,
    };
  },
  mounted() {
    document.addEventListener("dblclick", this.copyText);
  },
  destroyed() {
    document.removeEventListener("dblclick", this.copyText);
  },
  computed: {
    deviceCode() {
      return this.$store.state.setting.deviceCode;
    },
  },
  methods: {
    // 双击
    copyText(node) {
      const elem = node.target;
      if (!elem.parentNode.classList.contains("iscopy")) return;
      // 当前点击元素获取的值
      const value = elem.innerHTML;
      const color = getComputedStyle(elem, null)["color"];
      elem.style.background = "rgb(24,144,255)";
      elem.style.color = "#fff";
      let oInput = document.createElement("input");
      oInput.setAttribute("value", value);
      document.body.appendChild(oInput);
      oInput.select();
      // 该API已被弃用
      document.execCommand("copy");
      navigator.clipboard?.writeText(value);
      document.body.removeChild(oInput);
      setTimeout(() => {
        this.$modal.showToast("复制成功 " + value);
        elem.style.background = "none";
        elem.style.color = color;
      }, 800);
    },
    openPopup() {
      this.$refs.popup.open();
    },
    // 监听组件变化
    handleChangePopup(item) {
      if (!item.show) {
        this.handleClear();
      }
    },
    // 失去焦点
    handleBlur() {
      let that = this;
      that.focusShow = false; // 这个代码是关键!!!!
      that.$nextTick(() => {
        that.focusShow = true;
      });
      that.$forceUpdate();
    },
    // 回车
    handleEnter() {
      if (this.inputValue == "") return this.$modal.showToast("请扫码");
      if (this.sysList.length == 0)
        return this.$modal.showToast("请先获取系统刀盒码");

      if (!this.currList.find((v) => v.val == this.inputValue)) {
        if (!this.sysList.includes(this.inputValue)) {
          this.currList.push({ val: this.inputValue, flag: true });
          this.resList.push({
            val: "校验失败",
            flag: false,
            num: this.currList.length,
          });
        } else {
          this.currList.push({ val: this.inputValue, flag: false });
          this.resList.push({
            val: "校验成功",
            flag: true,
            num: this.currList.length,
          });
        }
      } else {
        this.currList.push({ val: this.inputValue, flag: true });
        this.resList.push({
          val: "出现重复",
          flag: false,
          num: this.currList.length,
        });
      }
      this.inputValue = "";
    },
    // 一键清空
    handleClear() {
      this.inputValue = "";
      this.comparisonRes = "待验证";
      this.currList = [];
      this.sysList = [];
      this.resList = [];
    },
    // 获取刀盒二维码数据
    getCodeList() {
      getAptBoxBarcode({
        groupNo: this.groupNo,
        deviceCode: this.deviceCode,
      }).then((res) => {
        if (res.code != 0) return this.$modal.showToast(res.message);
        this.sysList = res.data;
        this.$modal.showToast("获取成功");
      });
    },
    // 对比上传
    handleComparison() {
      // 去重后的数据
      const inpList = [...new Set(this.currList)];
      // 是否存在异常
      const hasError = this.resList.some((v) => !v.resMessage && !v.flag);
      if (this.sysList.length == 0)
        return this.$modal.showToast("请先获取系统刀盒码");
      if (inpList.length == 0) return this.$modal.showToast("扫码区无数据");
      if (inpList.length !== this.sysList.length) {
        this.resList.push({ resMessage: "扫码数量与系统不符", flag: false });
        this.comparisonRes = "ng";
      } else {
        if (hasError) {
          this.comparisonRes = "ng";
        } else {
          confirmAptBoxBarcode({
            cutterGroupNo: this.groupNo,
            deviceCode: this.deviceCode,
            inputBoxs: inpList.map((v) => v.val),
          }).then((res) => {
            if (res.code != 1) {
              this.resList.push({
                resMessage: res.message,
                flag: res.data == "ok" ? true : false,
              });
              this.comparisonRes = res.data;
            }
            this.$modal.showToast(res.message);
          });
        }
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.popup-content {
  overflow: auto;
  padding: 20px 20px;
  width: 800px;
  position: relative;
  .close-fixed {
    position: absolute;
    right: 10px;
    top: 10px;
  }
  .header {
    height: 100px;
    .title {
      height: 20px;
      font-size: 18px;
      font-weight: bold;
      display: flex;
      align-items: center;
    }

    .res-button {
      margin-top: -10px;
      margin-left: 30px;
    }
  }
}
.col-container {
  .col-header {
    display: flex;
    align-items: center;
    height: 40px;
    uni-text {
      flex-shrink: 0;
    }
    .uni-input {
      border: 1px solid #ccc;
      border-radius: 5px;
      padding: 5px;
    }
  }
  .col-main {
    border: 1px solid #ccc;
    margin-top: 10px;
    display: flex;
    flex-direction: column;
    height: 400px;
    padding: 10px;
    overflow-y: auto;
  }
}
</style>
