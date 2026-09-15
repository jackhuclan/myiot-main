<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="设备" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入设备"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工艺路线" prop="routeCodes">
        <el-select
          v-model="queryParams.routeCodes"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.routeCodes && queryParams.routeCodes.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in routeQueryList"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          >
          </el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="打板进度" prop="progress">
        <el-select
          @clear="clearQueryParams('progress')"
          v-model="queryParams.progress"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option :label="'即将停止'" :value="1" />
          <el-option :label="'已停止'" :value="0" />
        </el-select>
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-select
          @clear="clearQueryParams('status')"
          v-model="queryParams.status"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option :label="'启用'" :value="1" />
          <el-option :label="'禁用'" :value="0" />
        </el-select>
      </el-form-item>
    </search-form>
    <div class="status-color-container">
      <div class="status-color">
        <span style="border-color: #ff4949"></span>
        <span>已停止</span>
      </div>
      <div class="status-color">
        <span style="border-color: #f3dd10"></span>
        <span>即将停止</span>
      </div>
      <div class="status-color">
        <span style="border-color: #4992ff"></span>
        <span>已启用</span>
      </div>
    </div>
    <div class="drill_box">
      <div class="item" v-for="item in drillDeviceTask" :key="item.deviceId">
        <div
          class="agv"
          :style="{
            borderColor: getBorderColor(item),
          }"
        >
          <div class="header_label">
            <li>
              <!-- <span
                class="light"
                :style="{
                  background: item.isLoadingOrUnLoading ? '#56ce66' : '#4992ff',
                }"
              ></span> -->
              <svg-icon
                icon-class="drill"
                class="icon-agv"
                :data-id="item.deviceConsoleAddress"
                v-isGetSelection
              />
              <!-- :style="{
                  color: getBorderColor(item),
                  fontSize: '16px',
                }" -->
              <span
                class="tmp"
                :data-id="item.deviceConsoleAddress"
                v-isGetSelection
              >
                {{ item.code }}
              </span>
            </li>
            <div class="fixed_right_top">
              <el-tooltip
                class="item"
                effect="dark"
                content="详情"
                placement="top"
              >
                <i class="el-icon-document" @click="handleDrillDetail(item)">
                </i
              ></el-tooltip>

              <el-switch
                class="status_switch"
                :disabled="hasPermi(['dashboard:drill:disabled'])"
                :value="Boolean(item.status)"
                active-text="启用"
                inactive-text="禁用"
                inactive-color="#999"
                @change="(event) => handleEnabledOrDisabled(event, item)"
              >
              </el-switch>
            </div>
          </div>
          <div class="times">
            <li class="msg" v-if="item.withoutTaskTime">
              {{ item.withoutTaskTime }}
            </li>
            <li class="msg" v-if="item.waitTime">
              {{ item.waitTime }}
            </li>
            <li class="msg" v-if="item.loadOrUnLoadTime">
              {{ item.loadOrUnLoadTime }}
            </li>
            <li class="msg" v-if="item.alarmTime">
              {{ item.alarmTime }}
            </li>
            <!-- 未排产时长，等待时长，上下料时长，异常时长 -->
          </div>
          <li class="rack_li">
            <el-button
              type="primary"
              icon="el-icon-paperclip"
              plain
              @click="handleDrillDetail(item)"
              >{{
                hasPermi(["dashboard:drill:change"]) ? "查看" : "操作"
              }}板料</el-button
            >
          </li>
          <div class="msg">工艺路线:{{ item.routeCode }}</div>
          <div class="msg" v-if="item.overView">{{ item.overView }}</div>
          <!-- 由于该字段目前后端返回数据不正确 故先注释-->
          <!-- <div class="msg" v-if="item.taskDuration">
            {{ item.taskDuration }}
          </div> -->

          <div
            v-if="item.callAgvMessage"
            class="msg"
            :style="{
              color: item.callStatus ? 'green' : 'red',
            }"
          >
            {{ item.callAgvMessage }}
          </div>
          <div v-else class="msg">暂无实时信息</div>
          <el-progress
            :percentage="item.percentage"
            color="#67c23a"
          ></el-progress>

          <ul class="siloInfos">
            <li v-for="(v, vI) in item.siloInfos" :key="vI">
              <div>
                <span> {{ v.layer }}</span
                >， <span> 数量：{{ v.siloCount }}</span>
              </div>
              <div>
                <span> 物料：{{ v.itemCode }}</span>
              </div>
              <div>
                <span> 位置：{{ v.position }}</span>
              </div>
            </li>
          </ul>
        </div>
      </div>
      <change-panel
        v-if="!hasPermi(['dashboard:drill:change'])"
        :code="drillCode"
        ref="panel-dialog"
      />
      <view-panel v-else :code="drillCode" ref="panel-dialog" />
    </div>
    <el-backtop></el-backtop>
  </div>
</template>

<script>
import { getDrillPanelFullData } from "@/api/device/dashboard";
import { disableDevice, enableDevice } from "@/api/device/device";
import { getDropSelectDatas } from "@/api/produce/route";
import { mapState } from "vuex";
import ChangePanel from "./changePanel.vue";
import ViewPanel from "./viewPanel.vue";

export default {
  components: { ChangePanel, ViewPanel },
  data() {
    return {
      timer: null,
    };
  },
  computed: {
    getBorderColor() {
      return (item) => {
        // let str = "";
        if (item.percentage >= 90 && item.percentage < 100) {
          return "#F3DD10";
        }
        if (item.percentage == 100) {
          return "#ff4949";
        }

        if (item.status != 0) {
          return "#4992ff";
        }
        // if (item.isLoadingOrUnLoading) {
        //   str = "#56ce66";
        // } else if (item.isOnline && !item.isLoadingOrUnLoading) {
        //   str = "#4992ff";
        // } else if (!item.isOnline) {
        //   str = "#999";
        // }

        return "#4992ff";
      };
    },
    getSVG() {
      return (item) => {
        if (item.percentage >= 90 && item.percentage < 100) {
          return "#F3DD10";
        }
        if (item.percentage == 100) {
          return "#ff4949";
        }

        if (item.status != 0) {
          return "#drill";
        }
        return "#drill";
        // let str = "";
        // if (item.isLoadingOrUnLoading) {
        //   str = "drill_green";
        // } else if (item.isOnline && !item.isLoadingOrUnLoading) {
        //   str = "drill";
        // } else if (!item.isOnline) {
        //   str = "drill_#ccc";
        // }
        // return str;
      };
    },
    ...mapState({
      drill_dashboard_Interval: (state) =>
        state.personalized.drill_dashboard_Interval * 1000,
    }),
  },
  data() {
    return {
      showSearch: true,
      routeQueryList: [],
      drillDeviceTask: [],
      drillCode: null,
      queryParams: {
        pageNum: 1,
        pageSize: 1000,
        itemCode: undefined,
        name: undefined,
        code: undefined,
        deviceTypeId: undefined,
        deviceTypeCode: undefined,
        status: undefined,
        progress: undefined,
        deviceStatusList: [],
        routeCodes: [],
      },
    };
  },
  watch: {
    queryParams: {
      handler(val) {
        localStorage.setItem("drill_db_searchForm", JSON.stringify(val));
      },
      deep: true,
    },
  },

  activated() {
    this.queryParams = localStorage.getItem("drill_db_searchForm")
      ? JSON.parse(localStorage.getItem("drill_db_searchForm"))
      : {
          pageNum: 1,
          pageSize: 1000,
          itemCode: undefined,
          name: undefined,
          code: undefined,
          deviceTypeId: undefined,
          deviceTypeCode: undefined,
          status: undefined,
          progress: undefined,
          deviceStatusList: [],
          routeCodes: [],
        };
    this.getList();
    this.getRouteList();
    this.openTimer();
  },
  deactivated() {
    this.closeTimer();
  },

  methods: {
    // 开启定时器
    openTimer() {
      // 每隔5秒自动刷新
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.getList(); //调用接口的方法
        }, 0);
      }, this.drill_dashboard_Interval);
    },
    // 关闭定时器
    closeTimer() {
      clearInterval(this.timer);
      this.timer = null;
    },
    getList() {
      //获取钻机视图数据
      getDrillPanelFullData(this.queryParams).then((res) => {
        if (res?.code == 0) {
          this.drillDeviceTask = res.data?.list.map((v, i) => {
            return {
              ...v,
              // percentage: Math.floor(Math.random() * (100 - 85 + 1)) + 85,
              endTime: `2024/05/10 11:${i < 10 ? "0" + i : i}:00`,
            };
          });
          // this.drillDeviceTask = Array.from({ length: 20 }, (v, i) => {
          //   return {
          //     callAgvMessage: "测试数据" + `${i % 2 === 0 ? "成功" : "失败"}`,
          //     callStatus: i % 2 === 0 ? true : false,
          //   };
          // });
        }
      });
    },
    // 查询工艺路线
    getRouteList() {
      getDropSelectDatas({ vettingStatus: 1, pageNum: 1, pageSize: 100 }).then(
        (res) => {
          this.routeQueryList = res.data?.map((v) => {
            return { ...v, label: `${v.code}(${v.name})` };
          });
        }
      );
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.handleQuery();
    },
    // 点击标题跳转
    handleView(val) {
      if (!val) return;
      window.open(val, "_blank");
    },

    //点击详情
    handleDrillDetail(row) {
      this.drillCode = row.code;
      this.$refs["panel-dialog"].getList(row.code);
    },
    // 启用禁用
    handleEnabledOrDisabled(value, item) {
      let text = value ? "启用" : "禁用";
      let api = value ? enableDevice : disableDevice;
      const that = this;
      this.$modal
        .confirm(`确定${text}<span style="color:red"> ${item.code} </span>?`, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
        })
        .then(function () {
          api({ deviceCode: item.code }).then((result) => {
            if (result.code == 0) {
              that.$modal.msgSuccess(text + "成功");
              that.getList();
              item.status = item.status === 0 ? 1 : 0;
            } else {
              item.status = item.status === 0 ? 0 : 1;
              this.$modal.notifyError(result.message);
            }
          });
        })
        .catch(function () {
          item.status = item.status === 0 ? 0 : 1;
        });
    },
  },
};
</script>
<style lang="scss" scoped>
.app-container {
  background-color: rgb(240, 242, 245);
  .status-color-container {
    padding: 0 15px 10px 15px;
    display: flex;
    align-items: center;
    margin-top: 10px;
    .status-color {
      display: flex;
      align-items: center;
      margin-right: 10px;
      font-size: 14px;
      span:nth-child(1) {
        width: 16px;
        height: 16px;
        border: solid 2px transparent;
        border-radius: 3px;
        margin-right: 5px;
      }
    }
  }
}

.drill_box {
  padding: 0 10px;
  display: flex;
  flex-wrap: wrap;
  .item {
    border-radius: 10px;
    position: relative;
    width: calc(100% / 4);
    @media screen and (min-width: 1270px) and (max-width: 1920px) {
      width: calc(100% / 4);
    }

    @media screen and (min-width: 768px) and (max-width: 1270px) {
      width: calc(100% / 3);
    }
    @media screen and (min-width: 600px) and (max-width: 768px) {
      width: calc(100% / 2);
    }
    @media screen and (max-width: 600px) {
      width: 100%;
    }
    padding: 2px 5px;
    .agv {
      font-size: 14px;
      margin-bottom: 5px;
      background: #fff;
      border: transparent 3px solid;
      border-radius: 3px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
      > div {
        // padding-bottom: 5px;
        &:not(:last-child) {
          border-bottom: 1px solid #ccc !important;
        }
      }
    }
    .header_label {
      width: 100%;
      padding: 5px 0;
      padding-left: 5px;
      margin-bottom: 5px;
      display: flex;
      justify-content: space-between;
      .fixed_right_top {
        display: flex;
        align-items: center;
        justify-content: space-around;
        .status_switch {
          margin-right: 5px;
          ::v-deep .el-switch__label * {
            line-height: 1;
            font-size: 12px;
            display: inline-block;
          }
          ::v-deep .el-switch__label {
            position: absolute;
            display: none;
            color: #fff !important;
            font-size: 12px !important;
            width: 48px;
          }
          /*打开时文字位置设置*/
          ::v-deep .el-switch__label--right {
            z-index: 1;
            right: -3px;
          }
          /*关闭时文字位置设置*/
          ::v-deep .el-switch__label--left {
            z-index: 1;
            left: 20px;
          }
          /*显示文字*/
          ::v-deep .el-switch__label.is-active {
            display: block;
          }
          /*开关宽度*/
          ::v-deep .el-switch__core,
          .el-switch__label {
            width: 48px !important;
          }
        }

        .status_switch:hover {
          cursor: default;
        }
        > i {
          color: #1989fa;
          margin: 0 3px;
        }
        > i:hover {
          cursor: pointer;
        }
      }
      li {
        width: calc(100% - 80px);
        white-space: normal !important;
        word-break: break-all !important;
      }
      span {
        flex-shrink: 0;
      }
      .tmp {
        color: #1989fa;
        font-size: 16px;
      }
      .tmp:hover,
      .icon-agv:hover {
        cursor: pointer;
      }
      .icon-agv {
        margin: 0 2px;
        flex-shrink: 0;
        display: inline-block;
        width: 28px;
        height: 18px;
        fill: orange !important;
      }
      // 指示灯
      .light {
        display: inline-block;
        width: 16px;
        height: 16px;
        margin-right: 2px;
        border-radius: 50%;
        background: transparent;
      }
    }
    .siloInfos {
      margin: 0;
      padding: 0;
      list-style: none;
      font-size: 13px;
      li {
        padding: 2px 0;
        &:not(:last-child) {
          border-bottom: 1px solid #ccc;
        }
        &:last-child {
          margin-bottom: 2px;
        }

        > div {
          padding: 0px 5px;
          white-space: normal !important;
          word-break: break-all !important;
        }
      }
    }
    .times {
      .msg {
        color: red;
      }
    }
    // 状态行
    .msg {
      padding: 5px;
      // margin-bottom: 5px;
      display: flex;
      word-break: break-all;
    }
    .msg.lastTask {
      color: rgb(222, 175, 74);
    }
    .rack_li {
      .status {
        font-size: 15px;
        padding: 0px 6px;
        letter-spacing: 1px;
      }
      &.silocode {
        justify-content: flex-start;
      }
      > span {
        padding: 0 2px;
      }
      .el-button {
        padding: 3px 6px !important;
      }
      padding: 5px;
      border-bottom: solid 1px #ccc;
      display: flex;
      align-items: center;
      justify-content: space-between;
    }
  }
}

::v-deep .el-progress {
  margin-bottom: 5px;
  padding-left: 5px;
}
</style>
