<template>
  <div class="control-wrapper" v-draggable>
    <div
      v-if="show"
      unselectable="on"
      onselectstart="return false"
      class="tooltip"
    >
      <li>钻机：{{ targetCode }}</li>
      <li>日期：{{ targetDate }}</li>
      <li>工艺路线：{{ targetRouteCode }}</li>
    </div>

    <div
      class="control-btn out-right"
      @click="handleControl('right', batchNum)"
    >
      <span class="el-icon-d-arrow-left"></span>
    </div>
    <div class="control-btn out-left" @click="handleControl('left', batchNum)">
      <span class="el-icon-d-arrow-left"></span>
    </div>
    <div class="control-inner-wrapper">
      <div class="control-btn control-top" @click="handleControl('top')"></div>
      <div
        class="control-btn control-right"
        @click="handleControl('right')"
      ></div>
      <div
        class="control-btn control-bottom"
        @click="handleControl('bottom')"
      ></div>
      <div
        class="control-btn control-left"
        @click="handleControl('left')"
      ></div>
    </div>
    <div class="control-round">
      <div class="control-round-inner" @click="handleControlOk">ok</div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    targetCode: {
      type: String,
      default: "",
    },
    targetRouteCode: {
      type: String,
      default: "",
    },
    targetDate: {
      type: String,
      default: "",
    },
    show: {
      type: Boolean,
      default: true,
    },
    batchNum: {
      type: Number,
      default: 1,
    },
  },
  methods: {
    handleControl(control, num) {
      this.$emit("handleControl", control, num);
    },
    handleControlOk() {
      this.$emit("handleControlOk");
    },
  },
};
</script>

<style lang="scss" scoped>
// 方向按钮
.control-wrapper {
  position: fixed;
  right: 20px;
  bottom: 100px;
  width: 180px;
  height: 180px;
  z-index: 25;
  border-radius: 50%;
  // transform: rotate(-45deg);
}
/* 	第二层按钮 */
.control-inner-wrapper {
  position: absolute;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
  width: 120px;
  height: 120px;
  border-radius: 50%;
  background: #73cadc;
  .control-btn:before {
    content: "";
    position: relative;
    display: block;
    width: 10px;
    height: 10px;
    border-top: 2px solid #78aee4;
    border-right: 2px solid #78aee4;
    border-radius: 0 4px 0 0;
    box-sizing: border-box;
    z-index: 3;
  }
  .control-btn:after {
    content: "";
    position: absolute;
    width: 60%;
    height: 60%;
    background: #fff;
    z-index: 2;
  }
}

.control-btn {
  position: absolute;
  width: 40%;
  height: 40%;
  border: 1px solid #78aee4;
  box-sizing: border-box;
  transition: all 0.3s linear;
  background: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
}
.control-btn:hover {
  background-color: #abd0f4;
  cursor: pointer;
}
.control-top {
  top: 0;
  left: 50%;
  transform: translateX(-50%) rotate(-45deg);
  border-radius: 4px 100% 4px 4px;
}
.control-bottom {
  left: 50%;
  bottom: 0;
  transform: translateX(-50%) rotate(45deg);
  border-radius: 4px 4px 100% 4px;
}
.control-left {
  top: 50%;
  left: 0;
  transform: translateY(-50%) rotate(45deg);
  border-radius: 4px 4px 4px 100%;
}

.control-right {
  top: 50%;
  right: 0;
  transform: translateY(-50%) rotate(45deg);
  border-radius: 4px 100% 4px 4px;
}

.control-btn span {
  position: absolute;
  color: #78aee4;
  font-size: 16px;
  font-weight: bold;
}
.out-left {
  top: 50%;
  left: 0;
  border-radius: 4px 4px 4px 100%;
  transform: translateY(-50%) rotate(45deg);
  span {
    left: 18px;
    bottom: 18px;

    transform: rotate(-45deg);
  }
}
.out-right {
  top: 50%;
  right: 0;
  border-radius: 4px 100% 4px 4px;
  transform: translateY(-50%) rotate(45deg);
  span {
    right: 18px;
    top: 18px;
    transform: rotate(135deg);
  }
}
.control-bottom:before {
  transform: rotate(90deg);
}

.control-left:before {
  transform: rotate(180deg);
}

.control-top:after {
  left: 0;
  bottom: 0;
  border-top: 1px solid #78aee4;
  border-right: 1px solid #78aee4;
  border-radius: 0 100% 0 0;
}

.control-bottom:after {
  top: 0;
  left: 0;
  border-bottom: 1px solid #78aee4;
  border-right: 1px solid #78aee4;
  border-radius: 0 0 100% 0;
}

.control-left:after {
  right: 0;
  top: 0;
  border-bottom: 1px solid #78aee4;
  border-left: 1px solid #78aee4;
  border-radius: 0 0 0 100%;
}

.control-right:after {
  left: 0;
  bottom: 0;
  border-top: 1px solid #78aee4;
  border-right: 1px solid #78aee4;
  border-radius: 0 100% 0 0;
}

.control-round {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 30%;
  height: 30%;
  background: #fff;
  border-radius: 50%;
  z-index: 21;
}

.control-round-inner {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  display: flex;
  justify-content: center;
  align-items: center;
  width: 65%;
  height: 65%;
  font-size: 14px;
  border-radius: 50%;
  background: #78aee4;
  color: #fff;
  z-index: 22;
}

.control-round-inner:hover {
  background-color: #abd0f4;
  cursor: pointer;
}
// 目标信息提示
.tooltip {
  width: 100%;
  padding: 5px;
  border-radius: 5px;
  background: rgba(164, 160, 160, 0.4);
  position: absolute;
  left: 0px;
  top: -50px;
  user-select: none;
  user-select: none;
  user-select: none;
  li {
    display: flex;
    font-size: 14px;
  }
}
</style>
