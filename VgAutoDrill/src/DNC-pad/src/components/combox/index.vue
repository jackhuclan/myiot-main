<template>
  <view class="uni-combox" :class="disabled ? 'disabled' : ''">
    <view class="uni-combox__input-box">
      <input
        :disabled="disabled"
        class="uni-combox__input"
        type="text"
        :placeholder="placeholder"
        placeholder-class="uni-combox__input-plac"
        v-model="inputVal"
        @input="onInput"
        @focus="onFocus"
        @blur="onBlur"
      />
      <uni-icons
        v-if="inputVal"
        type="close"
        size="14"
        color="#999"
        @click="clearValue"
        style="margin-right: 10px"
      >
      </uni-icons>
      <uni-icons
        :type="showSelector ? 'top' : 'bottom'"
        size="14"
        color="#999"
        @click="toggleSelector"
      >
      </uni-icons>
    </view>
    <view class="uni-combox__selector" v-if="showSelector">
      <view class="uni-popper__arrow"></view>
      <scroll-view scroll-y="true" class="uni-combox__selector-scroll">
        <view
          class="uni-combox__selector-empty"
          v-if="filterCandidatesLength === 0"
        >
          <text>{{ emptyTips }}</text>
        </view>
        <view
          class="uni-combox__selector-item"
          v-for="(item, index) in filterCandidates"
          :key="index"
          :class="inputVal == item ? 'select' : ''"
          @click="onSelectorClick(index)"
          v-title
          >{{ item }}
        </view>
      </scroll-view>
    </view>
  </view>
</template>

<script>
/**
 * Combox 组合输入框
 * @description 组合输入框一般用于既可以输入也可以选择的场景
 * @tutorial https://ext.dcloud.net.cn/plugin?id=1261
 * @property {String} placeholder 输入框占位符
 * @property {Array} candidates 候选项列表
 * @property {String} emptyTips 筛选结果为空时显示的文字
 * @property {String} value 组合框的值
 */
export default {
  name: "uniCombox",
  emits: ["input", "update:modelValue", "blur"],
  props: {
    border: {
      type: Boolean,
      default: true,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    placeholder: {
      type: String,
      default: "",
    },
    candidates: {
      type: Array,
      default() {
        return [];
      },
    },
    emptyTips: {
      type: String,
      default: "无匹配项",
    },
    // #ifndef VUE3
    value: {
      type: [String, Number],
      default: "",
    },
    // #endif
    // #ifdef VUE3
    modelValue: {
      type: [String, Number],
      default: "",
    },
    // #endif
  },
  directives: {
    title: {
      inserted: function (el) {
        // #ifdef H5
        function getActualWidthOfChars(text, options = {}) {
          const { size = 14, family = "Microsoft YaHei" } = options;
          const canvas = document.createElement("canvas");
          const ctx = canvas.getContext("2d");
          ctx.font = `${size}px ${family}`;
          const metrics = ctx.measureText(text);
          const actual =
            Math.abs(metrics.actualBoundingBoxLeft) +
            Math.abs(metrics.actualBoundingBoxRight);
          return Math.floor(Math.max(metrics.width, actual));
        }
        if (getActualWidthOfChars(el.innerHTML) > 200) {
          el.setAttribute("title", el.innerHTML);
        } else {
          el.removeAttribute("title");
        }
        // #endif
      },
    },
  },
  data() {
    return {
      showSelector: false,
      inputVal: "",
      compareVal: "",
    };
  },
  computed: {
    filterCandidates() {
      return this.candidates.filter((item) => {
        return item.toString().indexOf(this.compareVal) > -1;
      });
    },
    filterCandidatesLength() {
      return this.filterCandidates.length;
    },
  },
  watch: {
    // #ifndef VUE3
    value: {
      handler(newVal) {
        this.inputVal = newVal;
        this.compareVal = newVal;
      },
      immediate: true,
    },
    // #endif
    // #ifdef VUE3
    modelValue: {
      handler(newVal) {
        this.inputVal = newVal;
        this.compareVal = newVal;
      },
      immediate: true,
    },
    // #endif
  },
  methods: {
    toggleSelector() {
      if (this.disabled) return;
      this.compareVal = "";
      this.showSelector = !this.showSelector;
    },
    clearValue() {
      if (this.disabled) return;
      setTimeout(() => {
        this.$emit("clear");
        this.$emit("input", "");
        this.$emit("update:modelValue", "");
      });
    },
    onFocus() {
      this.compareVal = "";
      this.showSelector = true;
    },
    onBlur() {
      setTimeout(() => {
        this.showSelector = false;
        this.$emit("blur", this.inputVal);
        this.$emit("update:modelValue", this.inputVal);
      }, 150);
    },
    onSelectorClick(index) {
      this.inputVal = this.filterCandidates[index];
      this.$emit("input", this.inputVal);
      this.$emit("update:modelValue", this.inputVal);
    },
    onInput() {
      setTimeout(() => {
        this.$emit("input", this.inputVal);
        this.$emit("update:modelValue", this.inputVal);
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.uni-combox {
  font-size: 14px;
  width: 180px;
  border: 1px solid #dcdfe6;
  border-radius: 4px;
  padding: 6px 10px;
  position: relative;
  /* #ifndef APP-NVUE */
  display: flex;
  /* #endif */
  // height: 40px;
  flex-direction: row;
  align-items: center;
  // border-bottom: solid 1px #DDDDDD;
}
.uni-combox.disabled {
  background: #f7f6f6;
  color: #d5d5d5;
}
.uni-combox__input-box {
  position: relative;
  /* #ifndef APP-NVUE */
  display: flex;
  /* #endif */
  flex: 1;
  flex-direction: row;
  align-items: center;
}

.uni-combox__input {
  flex: 1;
  font-size: 14px;
  height: 22px;
  line-height: 22px;
}

.uni-combox__input-plac {
  font-size: 14px;
  color: #999;
}

.uni-combox__selector {
  /* #ifndef APP-NVUE */
  box-sizing: border-box;
  /* #endif */
  position: absolute;
  top: calc(100% + 12px);
  left: 0;
  width: 100%;
  background-color: #ffffff;
  border: 1px solid #ebeef5;
  border-radius: 6px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  z-index: 22;
  padding: 4px 0;
}

.uni-combox__selector-scroll {
  /* #ifndef APP-NVUE */
  max-height: 200px;
  box-sizing: border-box;
  /* #endif */
}

.uni-combox__selector-empty,
.uni-combox__selector-item {
  /* #ifndef APP-NVUE */
  display: flex;
  cursor: pointer;
  /* #endif */
  line-height: 36px;
  font-size: 14px;
  padding: 0px 10px;
  width: 180px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  display: block;
}

.uni-combox__selector-item:hover {
  background-color: #f9f9f9;
}

.uni-combox__selector-empty:last-child,
.uni-combox__selector-item:last-child {
  /* #ifndef APP-NVUE */
  border-bottom: none;
  /* #endif */
}

// picker 弹出层通用的指示小三角
.uni-popper__arrow,
.uni-popper__arrow::after {
  position: absolute;
  display: block;
  width: 0;
  height: 0;
  border-color: transparent;
  border-style: solid;
  border-width: 6px;
}

.uni-popper__arrow {
  filter: drop-shadow(0 2px 12px rgba(0, 0, 0, 0.03));
  top: -6px;
  left: 10%;
  margin-right: 3px;
  border-top-width: 0;
  border-bottom-color: #ebeef5;
}

.uni-popper__arrow::after {
  content: " ";
  top: 1px;
  margin-left: -6px;
  border-top-width: 0;
  border-bottom-color: #fff;
}

.uni-combox__no-border {
  border: none;
}
.select {
  color: #409eff;
  font-weight: 700;
  background-color: #f5f7fa;
}
</style>
