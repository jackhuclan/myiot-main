<template>
  <div class="app-container">
    <myHeader />
    <div class="average_box">
      <li>当前机台数: {{ total }}</li>
      <li>平均实际稼动率: {{ averageDuty }}%</li>
      <li>平均理论稼动率: {{ averageTheoryDuty }}%</li>
      <li>平均实际达成率: {{ averageDutyRate }}%</li>
    </div>
    <div class="main">
      <div class="box" v-for="item in list" :key="item.id">
        <div
          class="bg_border"
          :style="{
            border: item.not == 'nothing' ? '2px solid red' : '',
            background: getStatus(item),
            color:
              item.drillState == 'IDLE' || item.drillState == 'SERV'
                ? '#000'
                : '#fff',
          }"
          :class="item.drillState == 'ALAM' ? 'breathe-div' : ''"
        >
          <div class="device_code">{{ item.deviceCode }}</div>
          <div class="device_content">
            <li>状态: {{ getStatusText(item) }}</li>
            <li>
              <span>
                实际稼动率:
                {{ item.duty != undefined ? item.duty + "%" : "" }} </span
              ><span>
                理论稼动率:{{
                  item.theoryDuty != undefined ? item.theoryDuty + "%" : ""
                }}
              </span>
            </li>
            <li>
              <span>
                实际达成率:
                {{ item.dutyRate != undefined ? item.dutyRate + "%" : "" }}
              </span>
            </li>

            <li>
              当前料号:
              <span class="item_code" :title="item.nowItemCode">
                {{ item.nowItemCode }}</span
              >
            </li>
            <li>
              下一料号:
              <span class="item_code" :title="item.nextItemCode">
                {{ item.nextItemCode }}</span
              >
            </li>
            <li>
              <el-progress
                :class="
                  item.drillState == 'IDLE' || item.drillState == 'SERV'
                    ? ''
                    : 'normal'
                "
                :percentage="item.percentage"
                style="width: 100%; padding-bottom: 6px"
              ></el-progress>
            </li>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { getDeviceDatas } from "@/api/utilizationRate";
import myHeader from "./header";
import { mapState } from "vuex";
import statusList from "./status";
export default {
  components: {
    myHeader,
  },
  data() {
    return {
      list: [],
      timer: null,
      statusList,
      // 当前机台数
      total: 0,
      //  平均理论稼动率
      averageTheoryDuty: 0,
      // 平均实际稼动率
      averageDuty: 0,
      //平均实际达成率
      averageDutyRate: 0,
    };
  },

  computed: {
    ...mapState({
      utilizationRateDashboardTime: (state) =>
        state.personalized.utilizationRate_dashboard_interval * 1000,
    }),
    getStatus() {
      return (val) => {
        const item = this.statusList.find((v) => v.value == val.drillState);
        let str = "";
        if (item) {
          str = item.color;
        } else {
          str = "green";
        }
        return str;
      };
    },
    getStatusText() {
      return (val) => {
        const item = this.statusList.find((v) => v.value == val.drillState);
        let str = "";
        if (item) {
          str = item.label;
        } else {
          str = "工作";
        }
        return str;
      };
    },
  },
  activated() {
    this.getList();
    this.getDetatilSetInterval();
  },
  // keep-alive 特有钩子函数 关闭定时器
  deactivated() {
    clearInterval(this.timer);
    this.timer = null;
  },
  methods: {
    getDetatilSetInterval(val) {
      if (this.timer) {
        clearInterval(this.timer);
        this.timer = null;
      }
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.getList(); //调用接口的方法
        }, 0);
      }, this.utilizationRateDashboardTime);
    },
    // 处理平均值
    calculateAverage(arr) {
      if (arr.length === 0) {
        return;
      }

      const sum = arr.reduce(
        (accumulator, currentValue) => accumulator + currentValue,
        0
      );

      return (sum / arr.length).toFixed(1);
    },
    getList() {
      getDeviceDatas().then((res) => {
        // 模拟数据
        // this.list = Array.from({ length: 40 }, (v, k) => {
        //   return {
        //     id: k,
        //     deviceCode: [
        //       "D10-2849-076",
        //       "D10-2849-0XX",
        //       "D10-2849-241",
        //       "D10-2849-242",
        //       "D10-2849-243",
        //       "D3-2849-001",
        //       "D5-2849-142",
        //       "D5-2849-143",
        //       "D5-2849-241",
        //       "D5-2849-242",
        //     ][Math.floor(Math.random() * 10)],
        //     drillState: statusList[Math.floor(Math.random() * 4)].value,
        //     percentage: Math.floor(Math.random() * 100) + 1,
        //     dutyRate: Math.floor(Math.random() * 100) + 1,
        //     duty: Math.floor(Math.random() * 100) + 1,
        //     theoryDuty: Math.floor(Math.random() * 100) + 1,
        //     nowItemCode: "nowItemCode" + k,
        //     nextItemCode: "nextItemCode" + k,
        //   };
        // });
        this.list = res;
        this.total = this.list?.length;
        // 平均理论稼动率
        this.averageTheoryDuty = this.calculateAverage(
          this.list?.map((v) => v.theoryDuty)
        );
        // 平均实际稼动率
        this.averageDuty = this.calculateAverage(this.list?.map((v) => v.duty));
        // 平均实际达成率
        this.averageDutyRate = this.calculateAverage(
          this.list?.map((v) => v.dutyRate)
        );
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.app-container {
  //整体页面背景
  background-image: url("../../../assets/images/bg.jpeg"); //背景图
  background-size: 100% 100%; //背景尺寸
  background-position: center center; //背景位置
  display: flex;
  flex-direction: column;
  color: #fff;
}
.average_box {
  width: 100%;
  height: 40px;
  padding-left: 10px;
  display: flex;
  align-items: center;
  li {
    margin-right: 30px;
  }
}
.main {
  width: 100%;
  height: calc(100% - 80px);
  padding-top: 0px;
  position: relative;

  .bg_border {
    overflow: hidden;
    width: 100%;
    height: 100%;
    border-radius: 10px;
    position: relative;
    border: solid 2px transparent;

    .device_code {
      padding: 3px 10px;
      font-size: 18px;
    }

    .device_content {
      padding: 0 5px;
      li {
        padding: 2px 0;
        font-size: 13px;
        display: flex;
        justify-content: space-between;
      }
      .item_code {
        flex: 1;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }
    }
  }

  .box {
    padding: 6px;
    width: calc(100% / 5);
    display: flex;
    display: inline-block;
  }
}

.breathe-div {
  animation: mymove1 1s infinite;
}

@keyframes mymove1 {
  from {
    box-shadow: 0 0 20px 15px yellow;
  }
  to {
    box-shadow: 0 0 10px 7px yellow;
  }
}

::v-deep .el-progress-bar__outer {
  height: 12px !important;
}

::v-deep .normal .el-progress__text {
  color: #fff !important;
}
</style>