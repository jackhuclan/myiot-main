<template>
  <transition name="moveR">
    <div
      v-if="value"
      class="alarm-dialog"
      @mouseenter="handleMouseEnter"
      @mouseleave="handleMouseLeave"
    >
      <header>
        <div class="header_title">
          <li>
            <i :class="'el-icon-warning'"></i>
            <span class="title">{{ alarmTimelyInfo.title }}</span>
          </li>
          <i class="el-icon-close" @click="close"></i>
        </div>
        <!-- 自定义标题 -->
        <div class="text">{{ alarmTimelyInfo.description }}</div>
      </header>
      <div class="warp"> 
        <div
          class="item"
          v-for="item in alarmTimelyInfo.alarmInfoList"
          :key="item.name"
        >
          <li>
            <span class="alarm-item alarm_name">{{ item.alarmName }}</span>
            <span class="alarm-item alarm_code">
              {{ item.alarmCode }}
            </span>
            <span class="alarm-item alarm_level">
              {{ item.eventData }}
            </span>
            <span class="alarm-item alarm_time">
              <span :style="levelStyle(item.alarmLevel)">
                {{ levelLabel(item.alarmLevel) }} </span
              >{{ parseTime(item.alarmTime) }}
            </span>
          </li>
          <div>
            <el-button
              type="danger"
              size="mini"
              plain
              @click="(e) => handleDelete(e, item)"
              >处理</el-button
            >
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>
  
  <script>
import { delAlarm } from "@/api/alarm/alarm";
import { getSysTimelyInformation } from "@/api/layout";
export default {
  props: {
    value: {
      type: Boolean,
    },
  },
  data() {
    return {
      styleOptions: [
        {
          value: 1,
          label: "普通",
          backgroundColor: "#ecf5ff",
          color: "#409eff",
          borderColor: "#d9ecff",
        },
        {
          value: 2,
          label: "严重",
          backgroundColor: "#fdf6ec",
          color: "#e6a23c",
          borderColor: "#faecd8",
        },
        {
          value: 3,
          label: "紧急",
          backgroundColor: "#fef0f0",
          color: "#f56c6c",
          borderColor: "#fde2e2",
        },
      ],
      alarmTimelyInfo: {},
      isClick: true,
    };
  },
  computed: {
    levelStyle() {
      return (item) => {
        const obj = this.styleOptions.find((v) => v.value == item);
        return obj;
      };
    },
    levelLabel() {
      return (item) => {
        const obj = this.styleOptions.find((v) => v.value == item);
        return obj?.label;
      };
    },
  },

  methods: {
    // 关闭
    close() {
      this.$emit("input", false);
    },
    getList() {
      this.isClick = false;
      getSysTimelyInformation()
        .then((res) => {
          // 判断是否出现LeftAlter
          if (
            res.data.alarmTimelyInfo &&
            res.data.alarmTimelyInfo.alarmInfoList.length > 0
          ) {
            this.alarmTimelyInfo = res.data.alarmTimelyInfo;
          } else {
            this.alarmTimelyInfo = {
              title: "告警及时信息",
              description: "显示告警及时信息，需尽快处理",
              alarmTimelyInfo: [],
            };
          }
        })
        .catch(() => {
          this.alarmTimelyInfo = {};
        });
    },
    handleMouseEnter() {
      this.isClick = true;
      this.$emit("mouseEnter");
    },
    handleMouseLeave() {
      this.isClick && this.$emit("mouseLeave");
    },
    //处理
    handleDelete(e, row) {
      this.isClick = false;
      this.$modal
        .confirm(
          `确定处理<span style="color:red">告警记录编码为 ${row.alarmCode}</span> 的数据项？`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .then((result) => {
          if (result == "confirm") {
            delAlarm(row.id).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("处理成功");
                this.getList();
                this.$emit("mouseLeave");
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {
          this.$emit("mouseLeave");
        });
      if (e.target.nodeName == "SPAN") {
        e.target.parentNode.blur();
      } else {
        e.target.blur();
      }
    },
  },
};
</script>
  <style lang="scss" scoped>
.moveR-enter-active,
.moveR-leave-active {
  transition: all 0.3s linear;
  transform: translateX(0);
}
.moveR-enter,
.moveR-leave {
  transform: translateX(100%);
}
.moveR-leave-to {
  transform: translateX(100%);
}
.alarm-dialog {
  border: 1px solid #ebeef5;
  border-radius: 8px;
  background-color: #fff;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  overflow: hidden;
  position: fixed;
  display: flex;
  flex-direction: column;
  right: 10px;
  top: 10px;
  z-index: 50;
  width: 400px;
  height: 300px;
}
header {
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  .header_title {
    padding: 5px 10px 0px 10px;
    font-size: 20px;
    font-weight: bold;
    display: flex;
    align-items: center !important;
    justify-content: space-between;
    li {
      display: flex;
      height: 40px;
      align-items: center;
      .el-icon-warning {
        margin-right: 5px;
        color: orange;
      }
    }
    .el-icon-close:hover {
      cursor: pointer;
    }
  }
  .text {
    width: 100%;
    padding: 0 10px 10px 10px;
  }
}
.warp {
  flex: 1;
  padding-top: 10px;
  overflow: hidden;
  overflow-y: auto;
  flex: 1;
}
.item {
  padding: 5px 0;
  border-bottom: 1px solid rgba(67, 63, 63, 0.3);
  display: flex;
  align-items: center;
  justify-content: space-between;
  > li {
    padding-left: 10px;
    width: calc(100% - 70px);
    .alarm-item {
      display: block;
      word-break: break-all;
      line-height: 20px;
    }
    .alarm_code {
      font-size: 15px;
      font-weight: bold;
    }
    .alarm_name {
      font-size: 14px;
      font-weight: bold;
    }
    .alarm_level {
      font-size: 13px;
      line-height: 17px;
    }
    .alarm_time {
      margin-top: 5px;
      font-size: 13px;
      color: #625858;
      span {
        width: 40px;
        font-size: 12px;
        text-align: center;
        border: solid 1px transparent;
        display: inline-block;
        margin-right: 10px;
        border-radius: 5px;
      }
    }
  }

  > div {
    width: 60px;
  }
}
</style>