<template>
  <v-scale-screen ref="scale-screen" width="1920" height="1080">
    <div class="bg">
      <myHeader @handleOpenDialog="handleOpenDialog" />
      <div class="average_box">
        <li>当前机台数: {{ total }}</li>
        <li>平均实际稼动率: {{ averageDuty }}%</li>
        <li>平均理论稼动率: {{ averageTheoryDuty }}%</li>
        <li>平均实际达成率: {{ averageDutyRate }}%</li>
      </div>
      <div
        class="main"
        :class="gridStyle"
        :style="{
          overflow:
            dashbordList && dashbordList.length > 20 ? 'auto' : 'hidden',
        }"
      >
        <div
          v-for="item in dashbordList"
          :key="item.code"
          class="bg_border"
          :style="{
            border: item.not == 'nothing' ? '4px solid red' : '',
            background: getStatus(item),
            color:
              item.drillState == 'IDLE' || item.drillState == 'SERV'
                ? '#000'
                : '#fff',
          }"
          :class="item.drillState == 'ALAM' ? ' breathe-div' : ''"
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
    <el-dialog
      title="选择钻机"
      :visible.sync="open"
      :before-close="handleClose"
      :modal-append-to-body="false"
    >
      <el-table
        size="medium"
        border
        ref="multipleTable"
        :data="deviceList"
        @select-all="handelSelectAll"
        @select="handelSelect"
        @row-dblclick="rowDblclick"
        :row-style="echoRowStyle"
      >
        <el-table-column type="selection"> </el-table-column>
        <el-table-column label="设备编码">
          <template slot-scope="scope">{{ scope.row.code }}</template>
        </el-table-column>
        <el-table-column prop="name" label="设备名称"> </el-table-column>
      </el-table>
      <span slot="footer" class="dialog-footer">
        <el-button @click="open = false">取 消</el-button>
        <el-button type="primary" @click="handleClose">确 定</el-button>
      </span>
    </el-dialog>
  </v-scale-screen>
</template>
    
    <script>
import { getDeviceList, getDashbordList } from "@/api";
import header from "@/components/header";
import { Notification } from "element-ui";
import statusList from "@/utils/status.js";
import VScaleScreen from "v-scale-screen";
export default {
  components: { myHeader: header, VScaleScreen },
  data() {
    return {
      open: false,
      // 展示数据
      dashbordList: [],
      //弹框数据
      deviceList: [],
      timer: null,
      devicequeryParams: {
        deviceKindList: [2, 3],
      },
      // 状态
      statusList,
      // 选中数组
      showData: localStorage.getItem("selectDevice")
        ? JSON.parse(localStorage.getItem("selectDevice"))
        : [],
      // 当前机台数
      total: 0,
      // 平均实际达成率
      averageTheoryDuty: 0,
      // 平均实际稼动率
      averageDuty: 0,
      // 平均理论稼动率
      averageDutyRate: 0,
    };
  },
  watch: {
    showData: {
      handler(val) {
        if (!val || val.length <= 0) {
          this.open = true;
        } else {
          if (this.open == false) {
            this.getSpecificDeviceDatas();
          }
        }
      },
      deep: true,
      immediate: true,
    },
    open: {
      handler(val) {
        if (val) return this.getList();
      },
      deep: true,
      immediate: true,
    },
  },
  created() {
    this.getList();
    this.startTimer();
  },
  beforeDestroy() {
    clearInterval(this.timer);
    this.timer = null;
  },
  computed: {
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

    gridStyle() {
      let str = "";
      if (this.dashbordList.length <= 15) {
        str = "main_list_15";
      } else if (this.dashbordList.length >= 30) {
        str = "main_list_30";
      }
      return str;
    },
  },
  methods: {
    echoRowStyle({ row, rowIndex }) {
      Object.defineProperty(row, "rowId", {
        //给每一行添加不可枚举属性rowIndex来标识当前行
        value: row.id,
        writable: true,
        enumerable: false,
      });
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
    getFilter(arr1, arr2) {
      return arr1.filter((item) => {
        return arr2.find((v) => v == item.deviceCode);
      });
    },

    // 获取看板数据
    getSpecificDeviceDatas() {
      getDashbordList().then((res) => {
        this.dashbordList = this.getFilter(
          res,
          this.showData.map((v) => v.code)
        );

        this.total = this.dashbordList?.length;
        // 平均理论稼动率
        this.averageTheoryDuty = this.calculateAverage(
          this.dashbordList?.map((v) => v.theoryDuty)
        );
        // 平均实际稼动率
        this.averageDuty = this.calculateAverage(
          this.dashbordList?.map((v) => v.duty)
        );
        // 平均实际达成率
        this.averageDutyRate = this.calculateAverage(
          this.dashbordList?.map((v) => v.dutyRate)
        );
      });
    },
    // 获取设备弹框数据
    getList() {
      getDeviceList(this.devicequeryParams).then((res) => {
        this.deviceList = res.data.list;
        this.$nextTick(() => {
          const showDataId = this.showData.map((v) => v.code);
          this.deviceList.forEach((val) => {
            if (showDataId.length && showDataId.includes(val.code)) {
              this.$refs.multipleTable?.toggleRowSelection(val, true);
            }
          });
        });
      });
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      // 获取表格对象
      let refsElTable = this.$refs.multipleTable;
      let findRow = this.showData.find((c) => c.id == row.rowId);
      //找到选中的行
      if (findRow) {
        this.showData = this.showData.filter((v) => v.code != row.code);
        refsElTable.toggleRowSelection(row, false); //如过重复选中，则取消选中
      } else {
        refsElTable.toggleRowSelection(row, true); // 实现选中行中选中事件
        this.showData.push(row);
      }
    },
    //获取table组件中的全选checkbox的勾选状态
    getIsAllChecked() {
      return this.$refs.multipleTable.store.states.isAllSelected;
    },
    // 全选、反选
    handelSelectAll(selection) {
      const showDataId = this.showData.map((v) => v.code);
      if (this.getIsAllChecked()) {
        this.deviceList.forEach((v) => {
          if (!showDataId.includes(v.code)) {
            this.showData.push(v);
          }
        });
      } else {
        const routeId = this.deviceList.map((v) => v.code);
        this.showData = this.showData.filter((v) => !routeId.includes(v.code));
      }
    },
    // 单选
    handelSelect(selection, row) {
      const showDataId = this.showData.map((v) => v.code);
      if (showDataId.includes(row.code)) {
        this.showData = this.showData.filter((v) => v.code != row.code);
      } else {
        this.showData.push(row);
      }
    },

    handleOpenDialog() {
      this.open = true;
      this.getList();
    },

    handleClose() {
      if (this.showData == [] || this.showData.length == 0) {
        localStorage.removeItem("selectDevice");
        Notification({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      } else {
        localStorage.setItem(
          "selectDevice",
          JSON.stringify(
            this.showData.map((v) => {
              return { code: v.code };
            })
          )
        );
        this.getSpecificDeviceDatas();
      }

      this.open = false;
    },
    startTimer() {
      this.timer = setInterval(() => {
        this.getSpecificDeviceDatas();
      }, 5000); // 1000毫秒 = 1秒
    },
  },
};
</script>
    <style lang="scss" scoped>
// 滚动条样式
/* 设置垂直滚动条 */
::-webkit-scrollbar {
  width: 12px;
}

/* 设置滚动条轨道为透明 */
::-webkit-scrollbar-track {
  background-color: rgba(0, 0, 0, 0);
}

/* 设置滚动条滑块为透明 */
::-webkit-scrollbar-thumb {
  background-color: rgba(0, 0, 0, 0);
}

/* 设置水平滚动条 */
::-webkit-scrollbar-horizontal {
  height: 12px;
}

/* 滚动条滑块悬停时的样式 */
::-webkit-scrollbar-thumb:hover {
  background-color: rgba(0, 0, 0, 0.3);
}
// 数据数量使用不同样式
.main_list_15 {
  grid-template-rows: repeat(3, 1fr) !important;
  .device_code {
    padding: 15px 10px !important;
    font-size: 24px !important;
  }
  .device_content {
    li {
      font-size: 20px !important;
      span {
        margin-right: 2px !important;
      }
    }
  }
}
.main_list_30 {
  grid-template-columns: repeat(5, 1fr) !important;
  grid-template-rows: repeat(6, 1fr) !important;
  .device_code {
    padding: 10px !important;
  }
}
.average_box {
  width: 100%;
  height: 30px;
  padding-left: 20px;
  display: flex;
  font-size: 20px;
  align-items: center;
  li {
    margin-right: 30px;
  }
}
.main {
  width: 100%;
  height: calc(100% - 60px);
  padding: 0 20px 15px 20px;
  padding-top: 10px;
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 15px;

  .bg_border {
    border-radius: 10px;
    border: solid 3px transparent;
    display: flex;
    flex-direction: column;
    .device_code {
      display: flex;
      align-items: center;
      padding: 10px 10px;
      font-size: 22px;
    }

    .device_content {
      flex: 1;
      padding: 0px 10px;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      align-content: center;
      li {
        font-size: 18px;
        padding: 5px 0;
        span {
          margin-right: 10px;
        }
      }
      .item_code {
        flex: 1;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        justify-content: start;
      }
    }
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
  height: 18px !important;
}

::v-deep .normal .el-progress__text {
  color: #fff !important;
}
</style> 