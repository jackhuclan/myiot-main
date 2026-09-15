<template>
  <div class="count">
    <!-- 组件中的 <button>一定要加 type=“button”，否则每次点击这个按钮的时候，执行完button的click事件后，会自动的重新刷新一下当前的页面。 -->

    <el-input
      type="text"
      :value="currentInputValue"
      @input="handleInput"
      :class="!dis ? 'num_input' : ''"
      :disabled="dis"
    >
      <el-button
        :slot="!dis ? 'prepend' : ''"
        @click="down"
        :disabled="minDisabled"
        icon="el-icon-minus"
      />
      <el-button
        :slot="!dis ? 'append' : ''"
        @click="up"
        :disabled="maxDisabled"
        icon="el-icon-plus"
      />
    </el-input>
  </div>
</template>

<script>
export default {
  props: {
    myNum: {
      type: Number,
      default: () => {
        return 0;
      },
    },
    min: {
      type: Number,
    },
    max: {
      type: Number,
    },
    numName: {
      type: String,
    },
    dis: {
      type: Boolean,
    },
  },
  data() {
    return {
      currentInputValue: this.myNum,
      currentMin: this.min,
      currentMax: this.max,
      minDisabled: this.myNum <= this.min ? true : false,
      maxDisabled: false,
    };
  },
  watch: {
    myNum(newVal) {
      this.currentInputValue = newVal;
      this.handelDown();
      this.handelUp();
    },
    dis(val) {
      this.maxDisabled = val;
      this.minDisabled = val;
    },
  },
  methods: {
    // 减
    down: function () {
      this.handelDown();
      this.maxDisabled = false;
      if (this.currentMin != undefined) {
        if (parseInt(this.currentInputValue - 1) >= this.currentMin) {
          this.currentInputValue = this.currentInputValue - 1;
          this.$emit("changeNum", {
            value: parseInt(this.currentInputValue),
            str: this.numName,
          });
        }
      } else {
        this.currentInputValue = this.currentInputValue - 1;
        this.$emit("changeNum", {
          value: parseInt(this.currentInputValue),
          str: this.numName,
        });
      }
    },
    // 加
    up: function () {
      this.minDisabled = false;
      if (this.currentMax != undefined) {
        if (parseInt(this.currentInputValue + 1) <= this.currentMax) {
          this.currentInputValue = this.currentInputValue + 1;
          this.$emit("changeNum", {
            value: parseInt(this.currentInputValue),
            str: this.numName,
          });
        }
      } else {
        this.currentInputValue = this.currentInputValue + 1;
        this.$emit("changeNum", {
          value: parseInt(this.currentInputValue),
          str: this.numName,
        });
      }
      this.handelUp();
    },
    // 禁用按钮
    handelDown() {
      if (this.currentMin != undefined) {
        if (this.currentInputValue <= this.currentMin) {
          this.minDisabled = true;
        } else {
          this.maxDisabled = false;
        }
      } else {
        this.minDisabled = false;
        this.maxDisabled = false;
      }
    },
    handelUp() {
      if (this.currentMax != undefined) {
        if (this.currentInputValue >= this.currentMax) {
          this.maxDisabled = true;
        } else {
          this.minDisabled = false;
        }
      } else {
        this.maxDisabled = false;
        this.minDisabled = false;
      }
    },
    // 输入框改变
    handleInput: function (e) {
      let value = e;
      let emitValue = 1;

      if (isNaN(Number(value))) {
        return (emitValue = 0);
      }
      // 防止输入不合法
      emitValue = parseInt(value);
      if (emitValue >= this.currentMax) {
        emitValue = this.currentMax;
        this.maxDisabled = true;
        this.minDisabled = false;
      } else {
        this.maxDisabled = false;
        this.minDisabled = false;
      }

      if (emitValue <= this.currentMin) {
        emitValue = parseInt(this.currentMin);
        this.maxDisabled = false;
        this.minDisabled = true;
      } else {
        this.maxDisabled = false;
        this.minDisabled = false;
      }

      this.currentInputValue = isNaN(Number(emitValue))
        ? 0
        : parseInt(emitValue);
      this.$emit("changeNum", {
        value: this.currentInputValue,
        str: this.numName,
      });
    },
  },
};
</script>

<style lang="scss">
.count .el-input__inner:focus {
  border-color: #ccc;
}
.num_input {
  .el-input__inner {
    text-align: center;
  }
}
</style>
