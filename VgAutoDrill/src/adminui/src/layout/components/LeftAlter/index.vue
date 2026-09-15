<template>
  <div
    class="notifyDiv"
    @mouseenter="mouseEnter()"
    @mouseleave="isClick && mouseLeave()"
  >
    <!-- 提示类型标题 -->
    <div class="header_title">
      <p>
        <i :class="'el-icon-' + notifyType"></i>
        <span class="title">{{ title }}</span>
      </p>
      <i class="el-icon-close" @click="close()"></i>
    </div>
    <!-- 自定义标题 -->
    <!-- <p class="text">字段以111111</p>
    <p class="text">字段2222222</p> -->
    <slot />
    <div class="warp">
      <div class="item" v-for="item in alarmInfoList" :key="item.name">
        <li>
          <span class="alarm_name">{{ item.alarmName }}</span>
          <span class="alarm_code">
            {{ item.alarmCode }}
          </span>
          <span class="alarm_level">
            {{ item.eventData }}
          </span>
          <p class="alarm_time">
            <span :style="levelStyle(item.alarmLevel)">
              {{ levelLabel(item.alarmLevel) }} </span
            >{{ parseTime(item.alarmTime) }}
          </p>
        </li>
        <div>
          <el-button type="danger" size="mini" plain @click="handleDelete(item)"
            >处理</el-button
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { delAlarm } from "@/api/alarm/alarm";
import { getSysTimelyInformation } from "@/api/layout";

export default {
  name: "LeftAlter",
  props: {
    mouseEnter: {
      type: Function,
    },
    mouseLeave: {
      type: Function,
    },
    close: {
      type: Function,
    },
    notifyType: {
      type: String,
      default: "info",
    },
    tableData: {
      type: Array,
      default: [],
    },
    title: {
      type: String,
      default: "",
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
      alarmInfoList: [],
      isClick: false,
    };
  },
  watch: {
    tableData: {
      handler(val) {
        this.alarmInfoList = val;
      },
      deep: true,
      immediate: true,
    },
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
    getList() {
      this.isClick = false;

      getSysTimelyInformation()
        .then((res) => {
          // 判断是否出现LeftAlter
          if (
            res.data.alarmTimelyInfo &&
            res.data.alarmTimelyInfo.alarmInfoList.length > 0
          ) {
            this.alarmInfoList = res.data.alarmTimelyInfo.alarmInfoList;
          } else {
            this.alarmInfoList = [];
          }
        })
        .catch(() => {
          this.alarmInfoList = [];
        });
    },
    //处理
    handleDelete(row) {
      this.isClick = true;
      this.$modal
        .confirm(
          `确定处理<span style="color:red">告警记录编码为 ${row.alarmCode}</span> 的数据项？`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .then((result) => {
          if (result == "confirm") {
            delAlarm(row.id)
              .then((res) => {
                if (res.code == 0) {
                  this.$modal.msgSuccess("处理成功");
                  this.getList();
                } else {
                  this.$modal.notifyError(res.message);
                }
              })
              .catch(() => {});
          }
        })

        .catch(() => {});
    },
  },
};
</script>

<style lang="scss" scoped>
.notifyDiv {
  height: 350px;
  display: flex;
  flex-direction: column;
  padding: 10px 0;
  .el-table {
    width: 480px;
    margin: 10px;
  }
  .text {
    padding: 0 10px 5px;
    box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.1);
  }
  .header_title {
    padding: 0 10px;
    margin-bottom: 10px;
    font-size: 20px;
    font-weight: bold;
    display: flex;
    align-items: center !important;
    justify-content: space-between;
    P {
      display: flex;
    }
    .title {
      margin-left: 5px;
    }
    .el-icon-close:hover {
      cursor: pointer;
    }
  }
}
.warp {
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
    span {
      display: block;
      word-break: break-all;
      line-height: 18px;
      vertical-align: middle;
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
      font-size: 13px;
      color: #625858;
      span {
        width: 40px;
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
