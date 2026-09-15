<template>
  <div class="app-container">
    <search-form :form="queryParams" @search="handleQuery" @reset="resetQuery">
      <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工艺路线" prop="routeCodeList">
        <el-select
          v-model="queryParams.routeCodeList"
          placeholder="工艺路线"
          multiple
          collapse-tags
          :class="
            queryParams.routeCodeList && queryParams.routeCodeList.length >= 2
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
      <el-form-item label="运行状态" prop="deviceStatuseLists">
        <el-select
          v-model="queryParams.deviceStatuseLists"
          placeholder="运行状态"
          multiple
          collapse-tags
          :class="
            queryParams.deviceStatuseLists &&
            queryParams.deviceStatuseLists.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.deviceStatusOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
    </search-form>
    <div class="masonry" v-loading="loading" v-if="agvList.length > 0">
      <div class="item" v-for="(item, i) in agvList" :key="item.deviceId">
        <div
          class="agv"
          :style="{
            borderColor: getStatus(item.deviceStatus, 'color'),
          }"
        >
          <div class="header_label">
            <li class="agv_title">
              <!-- <span
              class="light"
              :style="{
                background: getStatus(item.deviceStatus, 'color'),
              }"
            ></span> -->
              <svg-icon
                icon-class="AGV"
                class="icon-agv"
                @click="goDeviceConsoleAddress(item.deviceConsoleAddress)"
              />

              <span
                class="tmp"
                @click="goDeviceConsoleAddress(item.deviceConsoleAddress)"
              >
                {{ item.deviceId }}
                {{ item.routeCode ? "(" + item.routeCode + ")" : "" }}
              </span>
            </li>
          </div>
          <li class="agv_li">
            <el-button
              type="primary"
              icon="el-icon-paperclip"
              plain
              @click="handleChangePanel(item)"
              >{{
                hasPermi([`dashboard:agv:change`]) ? "查看" : "操作"
              }}板料</el-button
            >
            <el-button
              v-debounce
              type="danger"
              plain
              icon="el-icon-upload2"
              @click="hanldeCommand('ResetStatusCommand', item)"
              :disabled="hasPermi([`dashboard:agv:report`])"
              >重新上报</el-button
            >
          </li>
          <!-- <div class="agv-status">
            调度记录
            <el-switch
              @change="showSchedules(i, item)"
              style="margin-left: 5px"
              :class="'schedules_switch_' + i"
              active-color="#13ce66"
              inactive-color="#dcdfe6"
            >
            </el-switch>
          </div> -->
          <el-descriptions
            direction="vertical"
            size="medium"
            border
            :column="3"
            style="margin-bottom: 10px"
            :contentStyle="content_style"
          >
            <el-descriptions-item :span="1" label="运行状态">
              <el-tag
                size="mini"
                type="info"
                :style="{
                  color: getStatus(item.deviceStatus, 'color'),
                  background: getStatus(item.deviceStatus, 'bg'),
                  borderColor: getStatus(item.deviceStatus, 'color'),
                }"
                >{{ getStatus(item.deviceStatus, "label") }}</el-tag
              >
            </el-descriptions-item>
            <el-descriptions-item :span="1" label="总层数">{{
              item.layerCount
            }}</el-descriptions-item>
            <el-descriptions-item label="设备类别" :span="1">{{
              $status.deviceKinds.find((v) => v.value == item.deviceKind) &&
              $status.deviceKinds.find((v) => v.value == item.deviceKind).label
            }}</el-descriptions-item>
            <el-descriptions-item label="对接设备" :span="1">{{
              item.targetDevice
            }}</el-descriptions-item>
            <el-descriptions-item label="对接库位" :span="2">
              {{ item.properties.TargetLocation }}
            </el-descriptions-item>
            <el-descriptions-item label="任务编号" :span="1">{{
              item.taskCode
            }}</el-descriptions-item>
            <el-descriptions-item label="料仓" :span="2">
              {{ item.siloCode }}
            </el-descriptions-item>

            <el-descriptions-item label="产品编码" :span="1">
              <el-tooltip
                effect="dark"
                placement="bottom"
                content="点击查看库存"
              >
                <span
                  class="one-line-red"
                  @click="openItemSearchDialog(item.taskItemCode)"
                >
                  {{ item.taskItemCode }}</span
                >
              </el-tooltip></el-descriptions-item
            >

            <el-descriptions-item label="当前点位" :span="2">{{
              item.properties ? item.properties.CarCurrentPos : ""
            }}</el-descriptions-item>
            <el-descriptions-item :span="4" label="预约信息">{{
              item.properties ? item.properties.PreBookedInfo : ""
            }}</el-descriptions-item>
          </el-descriptions>
          <el-table
            :data="item.agVSiloInfo"
            style="width: 100%; margin-bottom: 10px"
            size="small"
          >
            <el-table-column
              prop="itemCode"
              label="产品编码"
              min-width="180"
              show-overflow-tooltip
              ><template slot-scope="scope">
                <el-tooltip
                  effect="dark"
                  placement="bottom"
                  content="点击查看库存"
                >
                  <span
                    class="one-line-red"
                    @click="openItemSearchDialog(scope.row.itemCode)"
                    >{{ scope.row.itemCode }}</span
                  >
                </el-tooltip>
              </template>
            </el-table-column>

            <el-table-column prop="siloCount" label="数量" align="center">
            </el-table-column>
            <el-table-column prop="productStatus" label="状态" align="center">
            </el-table-column>
          </el-table>
          <el-table
            :class="'schedules_' + i"
            size="mini"
            style="margin-bottom: 15px; display: none"
            :data="item.schedules"
          >
            <el-table-column
              min-width="60px"
              label="调度Id"
              align="center"
              prop="id"
              show-overflow-tooltip
            >
              <template slot-scope="scope">
                <span
                  class="click_code"
                  :data-id="scope.row.id"
                  v-isGetSelection
                  >{{ scope.row.id }}</span
                >
              </template>
            </el-table-column>
            <el-table-column
              min-width="100px"
              align="center"
              label="发起设备"
              prop="sourceDeviceId"
              show-overflow-tooltip
            >
              <template slot-scope="scope">
                <span v-if="scope.row.subDeviceCode">{{
                  scope.row.subDeviceCode
                }}</span>
                <span v-else>{{ scope.row.sourceDeviceId }}</span></template
              >
            </el-table-column>
            <el-table-column
              min-width="100px"
              align="center"
              label="交互方式"
              prop="interactionSequence"
              show-overflow-tooltip
            >
              <template slot-scope="scope">
                {{
                  $status.interactionSequenceOptions.find(
                    (v) => v.value == scope.row.interactionSequence
                  )
                    ? $status.interactionSequenceOptions.find(
                        (v) => v.value == scope.row.interactionSequence
                      ).label
                    : ""
                }}
                {{
                  scope.row.requestInteractionBehaviorName
                    ? "--- " + scope.row.requestInteractionBehaviorName
                    : ""
                }}
              </template>
            </el-table-column>
            <el-table-column
              min-width="80px"
              label="状态"
              align="center"
              prop="scheduledTaskStatus"
            >
              <template slot-scope="scope">
                <status-tag
                  :size="'mini'"
                  :options="$status.schedulementOptions"
                  :status="scope.row.scheduledTaskStatus"
                />
              </template>
            </el-table-column>
            <el-table-column
              label="分配时间"
              align="center"
              prop="allocateTime"
              min-width="110px"
            >
              <template slot-scope="scope">
                <span>{{
                  parseTime(scope.row.allocateTime, "{m}-{d} {h}:{i}:{s}")
                }}</span>
              </template>
            </el-table-column>
          </el-table>
        </div>
        <li class="switch">
          调度记录
          <el-switch
            @change="showSchedules(i, item)"
            style="margin-left: 5px"
            :class="'schedules_switch_' + i"
            active-color="#13ce66"
            inactive-color="#dcdfe6"
          >
          </el-switch>
        </li>
      </div>

      <change-panel
        v-if="!hasPermi([`dashboard:agv:change`])"
        ref="panel-dialog"
        setFormPage="AGV"
        @getRackList="getList"
        @openTimer="openTimer"
        @closeTimer="closeTimer"
      >
      </change-panel>
      <view-panel
        v-else
        ref="panel-dialog"
        setFormPage="AGV"
        @getRackList="getList"
        @openTimer="openTimer"
        @closeTimer="closeTimer"
      ></view-panel>
      <my-drawer ref="MyDrawer" :detailId="detailId" />
    </div>
    <el-empty v-else description="暂无数据"></el-empty>
    <ItemSearchDialog ref="ItemSearchDialog" />
    <el-backtop></el-backtop>
  </div>
</template>

<script>
import drawer from "@/views/device/components/drawer.vue";
import { getDropSelectDatas } from "@/api/produce/route";
import ChangePanel from "@/views/wareHouse/components/changePanel.vue";
import ViewPanel from "@/views/wareHouse/components/viewPanel.vue";

import { getAGVDeviceSiloInfo } from "@/api/device/dashboard";
import { allotsDeviceCommand } from "@/api/device/device";
import { mapState } from "vuex";

export default {
  components: { MyDrawer: drawer, ChangePanel, ViewPanel },
  name: "AGVDashboard",
  data() {
    return {
      loading: false,
      // AGV数据
      agvList: [],
      routeQueryList: [],
      content_style: {
        // 居中
        height: "40px",
        // 排列第二行
        "word-break": "break-all",
      },
      detailId: null,

      queryParams: {
        itemCode: undefined,
        routeCodeList: [],
        deviceStatuseLists: [],
      },
    };
  },
  activated() {
    this.getList();
    this.getRouteList();
  },
  deactivated() {
    this.closeTimer();
  },
  computed: {
    ...mapState({
      agv_dashboard_interval: (state) =>
        state.personalized.agv_dashboard_interval * 1000,
    }),
  },
  watch: {
    agv_dashboard_interval: {
      handler(val) {
        if (this.timer) {
          clearInterval(this.timer);
          this.timer = null;
        }
        this.timer = setInterval(() => {
          setTimeout(() => {
            //获取AGV视图数据
            getAGVDeviceSiloInfo(this.queryParams).then((res) => {
              if (res?.code == 0) {
                this.agvList = res.data?.list;
                this.loading = false;
              }
            });
          }, 0);
        }, val);
      },
      deepL: true,
      immediate: true,
    },
  },
  methods: {
    //获取AGV
    getList() {
      this.loading = true;
      //获取AGV视图数据
      getAGVDeviceSiloInfo(this.queryParams).then((res) => {
        if (res?.code == 0) {
          this.agvList = res.data?.list;
          this.loading = false;
        }
      });
    },
    // 查询工艺路线
    getRouteList() {
      getDropSelectDatas({
        vettingStatus: 1,
        pageNum: 1,
        pageSize: 100,
      }).then((res) => {
        this.routeQueryList = res.data?.map((v) => {
          return { ...v, label: `${v.partitionCode}(${v.code})` };
        });
      });
    },
    handleQuery() {
      this.getList(); //调用接口的方法
    },
    resetQuery() {
      this.handleQuery();
    },

    // 开启定时器
    openTimer() {
      getAGVDeviceSiloInfo(this.queryParams).then((res) => {
        if (res?.code == 0) {
          this.agvList = res.data?.list;
          this.loading = false;
        }
      });
      if (this.timer) {
        clearInterval(this.timer);
        this.timer = null;
      }
      // 每隔5秒自动刷新
      this.timer = setInterval(() => {
        setTimeout(() => {
          getAGVDeviceSiloInfo(this.queryParams).then((res) => {
            if (res?.code == 0) {
              this.agvList = res.data?.list;
              this.loading = false;
            }
          });
          this.getRouteList();
        }, 0);
      }, this.agv_dashboard_interval);
    },
    // 关闭定时器
    closeTimer() {
      clearInterval(this.timer);
      this.timer = null;
    },

    getStatus(status, type) {
      let str = "";
      const item = this.$status.deviceStatusOptions.find(
        (v) => v.value == status
      );
      if (status != undefined) {
        if (type == "color") {
          str = item?.color;
        } else if (type == "bg") {
          str = item?.background;
        } else {
          str = item?.label;
        }
      } else if (type == "label") {
        str = "未知";
      } else {
        str = "";
      }
      return str;
    },
    // 点击跳转
    goDeviceConsoleAddress(val) {
      if (!val) return;
      window.open(val, "_blank");
    },
    // 点击显示调度记录
    showSchedules(i, item) {
      // 当前要操作的元素
      const el = document.querySelector(`.schedules_` + i);
      // 样式
      const display = el.style.display;
      // 开关
      const switchEl = document.querySelector(`.schedules_switch_` + i);
      const core = switchEl.querySelector(".el-switch__core");

      if (display == "none") {
        el.style.display = "block";
        switchEl.classList.add("is-checked");
        core.style =
          "width: 40px; border-color: rgb(19, 206, 102); background-color: rgb(19, 206, 102";
      } else {
        el.style.display = "none";
        switchEl.classList.remove("is-checked");
        core.style =
          "width: 40px; border-color:#dcdfe6; background-color:#dcdfe6;";
      }
    },
    // 点击详情
    handleView(id) {
      this.detailId = id;
      this.$refs.MyDrawer.getDetail(this.detailId);
    },
    // 点击产品编码打开弹框
    openItemSearchDialog(code) {
      this.$refs.ItemSearchDialog.open = true;
      this.$refs.ItemSearchDialog.title = `查看库存--(${code})`;
      this.$refs.ItemSearchDialog.queryParams.itemCode = code;
      this.$refs.ItemSearchDialog.getList();
    },
    // 绑定料仓
    handleChangePanel(row) {
      this.$refs["panel-dialog"].showFlag = true;
      this.$refs["panel-dialog"].topForm = {
        ...row,
        code: row.deviceId,
        relateDeviceCode: row.deviceId,
      };
      this.$refs["panel-dialog"].oldSiloCode = row.siloCode;
      this.$refs["panel-dialog"].dialogTitle = `操作板料`;
      this.$refs["panel-dialog"].queryParams.siloCode = row.siloCode;
      this.$refs["panel-dialog"].getList();
    },
    async hanldeCommand(val, item) {
      let command = "";
      let label = "";
      if (typeof val != "string") {
        command = val ? "ResetStatusCommand" : "SetOnlineCommand";
        label = `<span style="color:red"> ${item.deviceId}</span> 当前状态为${
          item.readyOrOnline ? "REDAY" : "ONLINE"
        },确定修改状态？`;
      } else {
        command = val;
        label = `确定对<span style="color:red"> ${item.deviceId}</span> 执行重新上报操作？`;
      }
      const res = await this.$modal
        .confirm(label, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
        })
        .catch(() => {});
      if (!res) return;

      allotsDeviceCommand({
        locationCode: item.deviceId,
        command,
      }).then((res) => {
        if (res.code != 0) {
          this.$modal.notifyError(res.message);
        } else {
          this.$modal.msgSuccess("设置成功");
          item.readyOrOnline = val ? 1 : 0;
          this.getList();
        }
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.masonry {
  width: 100%;
  display: flex;
  padding: 0 5px;
  flex-wrap: wrap;
  .item {
    position: relative;
    width: calc(100% / 3);

    @media screen and (min-width: 1280px) and (max-width: 1920px) {
      width: calc(100% / 3);
    }

    @media screen and (min-width: 768px) and (max-width: 1280px) {
      width: calc(100% / 2);
    }

    @media screen and (min-width: 0px) and (max-width: 768px) {
      width: 100%;
    }
    padding: 2px 5px 0px 0px;
    font-size: 16px;
    // 状态行
    .agv-status {
      display: flex;
      word-break: break-all;
      border-bottom: #ccc 1px solid;
      padding: 6px 5px;
      line-height: 20px;
    }
    .agv {
      margin-bottom: 12px;

      background: #fff;
      border: transparent 2px solid;
    }

    .switch {
      display: flex;
      align-items: center;
      position: absolute;
      right: 15px;
      top: 5px;
    }
    .switch:hover {
      cursor: default;
    }
    .agv_li {
      padding: 5px;
      border-bottom: solid 1px #ccc;
      display: flex;
      align-items: center;
      justify-content: space-between;
      .el-button {
        padding: 3px 6px !important;
      }
    }
    .header_label {
      width: 100%;
      padding: 4px 5px;
      border-bottom: 1px solid #ccc;
      line-height: 20px;
      .agv_title {
        min-width: 30px;
        font-size: 18px;
        display: block;
        word-break: break-all;
        color: #1989fa;
        width: calc(100% - 120px);
      }
      .tmp:hover,
      .icon-agv:hover {
        cursor: pointer;
      }
      .icon-agv {
        width: 30px;
        height: 22px;
        margin-bottom: -2px;
      }
      // 指示灯
      .light {
        display: inline-block;
        width: 16px;
        height: 16px;
        border-radius: 50%;
        background: transparent;
      }
    }
  }
}
</style>
