<template>
  <div class="app-container dashboard-editor-container">
    <el-collapse v-model="activeNames" style="margin-bottom: 5px">
      <el-collapse-item name="1" title="调度记录">
        <div class="collapse-title" slot="title">调度记录</div>
        <div :class="addFlex == 0 ? 'top_flex' : 'flex'">
          <el-tooltip
            class="item"
            effect="dark"
            content="切换布局"
            placement="bottom"
          >
            <el-button
              icon="el-icon-s-grid"
              circle
              size="mini"
              class="top_flex_btn"
              @click="handleAddFlex"
            ></el-button>
          </el-tooltip>

          <el-card class="top">
            <el-form slot="header" size="small" ref="queryForm" :inline="true">
              <el-form-item label="未分配" class="form-title"> </el-form-item>
              <el-form-item label="仅显示钻机">
                <el-switch
                  v-model="searchForm.isOnlyDrill"
                  @change="changeIsOnlyDrill"
                />
              </el-form-item>
              <el-form-item label="" prop="routeCodeList">
                <el-select
                  @remove-tag="handleClear"
                  @visible-change="handleFocus"
                  v-model="searchForm.not_assigned_routeList"
                  placeholder="工艺路线"
                  multiple
                  collapse-tags
                  :class="
                    searchForm.not_assigned_routeList &&
                    searchForm.not_assigned_routeList.length >= 2
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
              <el-form-item>
                <el-button type="primary" @click="onSubmit('table')"
                  >重置</el-button
                >
              </el-form-item>
            </el-form>

            <el-table border :data="schedulementList">
              <el-table-column label="记录Id" min-width="80px" prop="id">
                <template slot-scope="scope">
                  <el-tooltip
                    effect="dark"
                    placement="bottom"
                    content="点击显示详情"
                  >
                    <span
                      class="click_code"
                      :data-id="
                        JSON.stringify({ id: scope.row.id, type: 'detail' })
                      "
                      v-isGetSelection
                      >{{ scope.row.id }}</span
                    ></el-tooltip
                  ></template
                ></el-table-column
              >
              <el-table-column
                label="发起设备"
                :min-width="
                  flexColumnWidth(
                    '发起设备',
                    'initiateDevice',
                    schedulementList
                  )
                "
                prop="sourceDeviceId"
              >
                <template slot-scope="scope">
                  <el-tooltip class="item" effect="dark" placement="bottom">
                    <div slot="content" style="width: 200px">
                      <li class="tooltip_li">
                        记录Id：
                        {{ scope.row.id }}
                      </li>
                      <li class="tooltip_li">
                        发起设备：
                        {{ scope.row.initiateDevice }}
                      </li>
                      <li class="tooltip_li">
                        traceCode：
                        {{ scope.row.code }}
                      </li>
                      <li class="tooltip_li">
                        任务编码：
                        {{ scope.row.taskId }}
                      </li>
                      <li class="tooltip_li">
                        产品编码：
                        {{ scope.row.itemCode }}
                      </li>
                      <li class="tooltip_li">
                        工艺路线：
                        {{ scope.row.routeCode }}
                      </li>
                      <li class="tooltip_li">
                        呼叫时间：
                        {{ parseTime(scope.row.createTime, "{m}-{d} {h}:{i}") }}
                      </li>
                      <li class="tooltip_li">
                        需求备注：
                        {{ scope.row.remark }}
                      </li>
                    </div>
                    <span
                      class="one-line"
                      :data-id="
                        JSON.stringify({ id: scope.row.id, type: 'panel' })
                      "
                      v-isGetSelection
                    >
                      {{ scope.row.initiateDevice }}
                    </span>
                  </el-tooltip>
                </template>
              </el-table-column>

              <el-table-column
                label="产品编码"
                prop="itemCode"
                min-width="150px"
              >
                <template slot-scope="scope">
                  <el-tooltip
                    v-if="
                      scope.row.requestDeviceKind == 2 ||
                      scope.row.requestDeviceKind == 3
                    "
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
                  <tooltip v-else :value="scope.row.requestSummaryInfo" />
                </template>
              </el-table-column>
              <!-- <el-table-column
                label="调度设备"
                prop="requireDeviceId"
                min-width="100px"
                show-overflow-tooltip
              >
                <template slot-scope="scope">
                  {{ scope.row.requireDeviceId }}
                  {{
                    $status.deviceStatusOptions.find(
                      (v) => v.value == scope.row.deviceStatus
                    )
                      ? ` (${
                          $status.deviceStatusOptions.find(
                            (v) => v.value == scope.row.deviceStatus
                          ).label
                        })`
                      : ""
                  }}</template
                >
              </el-table-column> -->
              <el-table-column
                label="等待时长"
                prop="waitTime"
                align="center"
                show-overflow-tooltip
              >
                <!--  等待时长 逻辑变更 -->
                <!-- 1.---存在任意时间时（分配、开始、异常、取消、完成），取最小值，最小值 -- 呼叫时间 -->
                <!-- 2.---都不存在任意时间时（分配、开始、异常、取消、完成），系统时间 --呼叫时间  -->
                <template slot-scope="scope">
                  <span style="color: red" v-if="getWaitTime(scope.row) > 0"
                    >{{ getWaitTime(scope.row) }}(分钟)</span
                  >
                </template>
              </el-table-column>
              <el-table-column
                label="呼叫时间"
                align="center"
                prop="createTime"
                min-width="100px"
              >
                <template slot-scope="scope">
                  {{ parseTime(scope.row.createTime, "{m}-{d} {h}:{i}") }}
                </template>
              </el-table-column>
            </el-table>

            <pagination
              v-show="total > 0"
              :total="total"
              :page.sync="queryParams.pageNum"
              :limit.sync="queryParams.pageSize"
              @pagination="getSchedule"
              layout="total,sizes, prev, pager, next"
            />
          </el-card>
          <el-card class="top">
            <div slot="header" class="clearfix">
              <el-form size="small">
                <el-form-item label="已开始" class="form-title"> </el-form-item>
              </el-form>
            </div>
            <el-table border :data="runingSchedulementList">
              <el-table-column label="记录Id" prop="id" min-width="80px">
                <template slot-scope="scope">
                  <el-tooltip
                    effect="dark"
                    placement="bottom"
                    content="点击显示详情"
                  >
                    <span
                      class="click_code"
                      :data-id="
                        JSON.stringify({ id: scope.row.id, type: 'detail' })
                      "
                      v-isGetSelection
                      >{{ scope.row.id }}</span
                    >
                  </el-tooltip>
                </template>
              </el-table-column>
              <el-table-column
                :min-width="
                  flexColumnWidth(
                    '发起设备',
                    'initiateDevice',
                    schedulementList
                  )
                "
                label="发起设备"
                prop="sourceDeviceId"
              >
                <template slot-scope="scope">
                  <el-tooltip class="item" effect="dark" placement="bottom">
                    <div slot="content" style="width: 200px">
                      <li class="tooltip_li">
                        记录Id：
                        {{ scope.row.id }}
                      </li>
                      <li class="tooltip_li">
                        发起设备：
                        {{ scope.row.initiateDevice }}
                      </li>
                      <li class="tooltip_li">
                        traceCode：
                        {{ scope.row.code }}
                      </li>
                      <li class="tooltip_li">
                        任务编码：
                        {{ scope.row.taskId }}
                      </li>
                      <li class="tooltip_li">
                        产品编码：
                        {{ scope.row.itemCode }}
                      </li>
                      <li class="tooltip_li">
                        交互方式：
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
                      </li>
                      <li class="tooltip_li">
                        呼叫时间：
                        {{ parseTime(scope.row.createTime, "{m}-{d} {h}:{i}") }}
                      </li>
                      <li class="tooltip_li">
                        需求备注：
                        {{ scope.row.remark }}
                      </li>
                    </div>
                    <span class="one-line">
                      {{ scope.row.initiateDevice }}
                    </span>
                  </el-tooltip></template
                ></el-table-column
              >
              <el-table-column
                label="产品编码"
                prop="itemCode"
                min-width="150px"
                show-overflow-tooltip
              >
                <template slot-scope="scope">
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
              <el-table-column
                label="调度设备"
                prop="requireDeviceId"
                min-width="100px"
                show-overflow-tooltip
              >
                <template slot-scope="scope">
                  {{ scope.row.requireDeviceId }}
                  <!--{{
                    $status.deviceStatusOptions.find(
                      (v) => v.value == scope.row.deviceStatus
                    )
                      ? ` (${
                          $status.deviceStatusOptions.find(
                            (v) => v.value == scope.row.deviceStatus
                          ).label
                        })`
                      : ""
                  }}-->
                </template>
              </el-table-column>
              <el-table-column
                label="上下料时长"
                key="materialTime"
                prop="materialTime"
                align="center"
                min-width="100"
              >
                <!-- 2. 上下料时长 逻辑变更；
开始时间不存在时，不要显示值
开始时间存在，并且 不存在（取消、异常、完成）时间， 用系统时间 -- 开始时间；
开始时间存在，并且 存在任意一个（取消、异常、完成）时间， 用最大的那个值的时间 -- 开始时间； -->
                <template slot-scope="scope">
                  <span style="color: red" v-if="getMaterialTime(scope.row) > 0"
                    >{{ getMaterialTime(scope.row) }}分钟</span
                  >
                </template>
              </el-table-column>

              <el-table-column
                label="分配时间"
                align="center"
                prop="allocateTime"
                min-width="100px"
              >
                <template slot-scope="scope">
                  {{ parseTime(scope.row.allocateTime, "{m}-{d} {h}:{i}") }}
                </template>
              </el-table-column>
              <el-table-column
                label="呼叫时间"
                align="center"
                prop="createTime"
                min-width="100px"
              >
                <template slot-scope="scope">
                  {{
                    parseTime(scope.row.createTime, "{m}-{d} {h}:{i}")
                  }}</template
                >
              </el-table-column></el-table
            >
            <pagination
              layout="total,sizes, prev, pager, next"
              v-show="runingTotal > 0"
              :total="runingTotal"
              :page.sync="runingQueryParams.pageNum"
              :limit.sync="runingQueryParams.pageSize"
              @pagination="getRunningSchedule"
            />
          </el-card>
        </div>
      </el-collapse-item>
      <el-collapse-item name="2">
        <div class="collapse-title" slot="title">
          <div @click.stop.prevent="showCollapse">
            <el-form size="small" ref="queryForm" style="display: flex">
              <el-form-item label="AGV" class="form-title"> </el-form-item>
              <el-form-item label="" prop="routeCodeList">
                <el-select
                  @remove-tag="handleClear"
                  @visible-change="handleFocus"
                  v-model="searchForm.agv_routeList"
                  placeholder="工艺路线"
                  multiple
                  collapse-tags
                  :class="
                    searchForm.agv_routeList &&
                    searchForm.agv_routeList.length >= 2
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
              <el-form-item>
                <el-select
                  v-model="searchForm.agv_deviceStatusList"
                  @remove-tag="handleClear"
                  @visible-change="handleFocus"
                  placeholder="运行状态"
                  multiple
                  collapse-tags
                  :class="
                    searchForm.agv_deviceStatusList &&
                    searchForm.agv_deviceStatusList.length >= 2
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
                </el-select> </el-form-item
              ><el-form-item>
                <el-button type="primary" @click="onSubmit('AGV')"
                  >重置</el-button
                >
              </el-form-item>
            </el-form>
          </div>
        </div>
        <agv-view
          :list="agvList"
          @openTimer="openTimer"
          @closeTimer="closeTimer"
        />
      </el-collapse-item>
    </el-collapse>

    <my-drawer ref="MyDrawer" :detailId="detailId" />
    <my-panel ref="MyPanel" />
    <ItemSearchDialog ref="ItemSearchDialog" />
    <!-- 回到顶部 -->
    <el-backtop></el-backtop>
    <!-- 折叠 -->
    <el-backtop :right="90"
      ><div
        @click.stop="toggleExpandAll"
        style="
          height: 100%;
          width: 100%;
          border-radius: 50%;
          text-align: center;
          line-height: 40px;
        "
      >
        <i class="el-icon-sort"></i></div
    ></el-backtop>
  </div>
</template>

<script>
import AgvView from "./AgvView.vue";
import drawer from "../components/drawer.vue";
import panel from "../components/panel.vue";

import {
  getAGVDeviceSiloInfo,
  getScheduleList,
  getRunningScheduleList,
  getCentralControlSystemIsMaintaining,
} from "@/api/device/dashboard";
import { getDropSelectDatas } from "@/api/produce/route";
import { mapState } from "vuex";

export default {
  name: "DeviceDashboard",
  components: {
    AgvView,
    MyDrawer: drawer,
    MyPanel: panel,
  },
  data() {
    return {
      addFlex: 0,
      loading: false,
      // AGV数据
      agvList: [],
      // 钻机数据
      drillList: [],
      // 库位数据
      rackList: [],
      // 未分配调度记录
      schedulementList: [],
      // 已开始调度记录
      runingSchedulementList: [],
      // 工艺路线
      routeQueryList: [],
      timer: null,
      total: 0,
      runingTotal: 0,
      // 未分配查询参数
      queryParams: {
        pageNum: sessionStorage.getItem("notassigned-pageNum")
          ? Number(sessionStorage.getItem("notassigned-pageNum"))
          : 1,
        pageSize: sessionStorage.getItem("notassigned-pageSize")
          ? Number(sessionStorage.getItem("notassigned-pageSize"))
          : 10,
      },
      // 已开始查询参数
      runingQueryParams: {
        pageNum: 1,
        pageSize: sessionStorage.getItem("runing-pageNum")
          ? Number(sessionStorage.getItem("runing-pageNum"))
          : 1,
        pageSize: sessionStorage.getItem("runing-pageSize")
          ? Number(sessionStorage.getItem("runing-pageSize"))
          : 10,
      },
      searchForm: {
        // 未分配仅显示钻机请求
        isOnlyDrill: true,
        // 未分配
        not_assigned_routeList: [],
        // AGV查询
        agv_routeList: [],
        agv_deviceStatusList: [],
      },
      //折叠面板字段
      activeNames: ["1"],
      // 设备Id
      detailId: null,
    };
  },
  created() {
    this.searchForm = localStorage.getItem("db_searchForm")
      ? JSON.parse(localStorage.getItem("db_searchForm"))
      : {
          // 未分配仅显示钻机请求
          isOnlyDrill: true,
          // 未分配
          not_assigned_routeList: [],
          // AGV查询
          agv_routeList: [],
          agv_deviceStatusList: [],
        };
    // 折叠面板当前展开
    this.activeNames = localStorage.getItem("activeNames")
      ? JSON.parse(localStorage.getItem("activeNames"))
      : ["1"];
    // 顶部布局
    this.addFlex = localStorage.getItem("addFlex") * 1;
    this.getList();
  },
  beforeDestroy() {
    this.closeTimer();
  },
  watch: {
    schedule_dashboard_interval: {
      handler(val) {
        if (this.timer) {
          clearInterval(this.timer);
          this.timer = null;
        }
        this.timer = setInterval(() => {
          setTimeout(() => {
            this.getList(); //调用接口的方法
          }, 0);
        }, val);
      },
      deepL: true,
      immediate: true,
    },
    activeNames(val) {
      localStorage.setItem("activeNames", JSON.stringify(val));
    },
    "queryParams.pageSize": {
      handler(val) {
        sessionStorage.setItem("notassigned-pageSize", val);
      },
      deep: true,
    },
    "queryParams.pageNum": {
      handler(val) {
        sessionStorage.setItem("notassigned-pageNum", val);
      },
      deep: true,
    },
    "runingQueryParams.pageSize": {
      handler(val) {
        sessionStorage.setItem("runing-pageSize", val);
      },
      deep: true,
    },
    "runingQueryParams.pageNum": {
      handler(val) {
        sessionStorage.setItem("runing-pageNum", val);
      },
      deep: true,
    },
    searchForm: {
      handler(val) {
        localStorage.setItem("db_searchForm", JSON.stringify(val));
      },
      // immediate: true,
      deep: true,
    },
  },
  computed: {
    ...mapState({
      schedule_dashboard_interval: (state) =>
        state.personalized.schedule_dashboard_interval * 1000,
    }),
    // 获取发起设备展示
    getDeviceLabel() {
      return (row) => {
        const subDeviceCode = row.subDeviceCode
          ? row.subDeviceCode
          : row.sourceDeviceId;
        const wareHouseCode = row.wareHouseCode ? row.wareHouseCode : "";
        const routeCode = row.routeCode ? row.routeCode : "";

        let str = "";
        if (!wareHouseCode && !routeCode) {
          str = subDeviceCode;
        } else {
          str = subDeviceCode + `(${wareHouseCode}-${routeCode})`;
        }
        return str;
      };
    },
    getMaterialTime() {
      return (row) => {
        // 异常时间failedTime
        // 取消时间canceledTime
        // 完成时间completedTime
        let form = {
          failedTime: row.failedTime,
          canceledTime: row.canceledTime,
          completedTime: row.completedTime,
        };

        // 1.开始时间不存在时，不要显示值
        if (!row.runningTime) return "";
        // 2.开始时间存在，并且 存在任意一个（取消、异常、完成）时间， 用最大的那个值的时间 -- 开始时间
        const oneOfThem = Object.values(form).some(
          (v) => v && !isNaN(Date.parse(v))
        );
        if (oneOfThem) {
          let max = null;
          const array = Object.values(form).filter((item) => {
            if (item) {
              return new Date(item);
            }
          });

          if (array.length == 1) {
            max = array[0];
          } else {
            max = Math.max(...array);
          }
          return this.timeDifference(row.runningTime, max);
        } else {
          // 3.开始时间存在，并且 不存在（取消、异常、完成）时间， 用系统时间 -- 开始时间
          return this.timeDifference(row.runningTime, new Date());
        }
      };
    },
    getWaitTime() {
      return (row) => {
        // 分配时间allocateTime
        // 开始时间runningTime
        // 异常时间failedTime
        // 取消时间canceledTime
        // 完成时间completedTime
        // 呼叫时间createTime
        let form = {
          allocateTime: row.allocateTime,
          runningTime: row.runningTime,
          failedTime: row.failedTime,
          canceledTime: row.canceledTime,
          completedTime: row.completedTime,
        };
        //  <!-- 1.---存在任意时间时（分配、开始、异常、取消、完成），取最小值，最小值 -- 呼叫时间 -->
        const oneOfThem = Object.values(form).some(
          (v) => v && !isNaN(Date.parse(v))
        );
        if (oneOfThem) {
          let min = null;
          const array = Object.values(form).filter((item) => {
            if (item) {
              return new Date(item);
            }
          });

          if (array.length == 1) {
            min = array[0];
          } else {
            min = Math.min(...array);
          }
          return this.timeDifference(row.createTime, min);
        } else {
          //  <!-- 2.---都不存在任意时间时（分配、开始、异常、取消、完成），系统时间 --呼叫时间  -->
          return this.timeDifference(row.createTime, new Date());
        }
      };
    },
  },
  methods: {
    // 切换顶部布局
    handleAddFlex() {
      this.addFlex = this.addFlex == 0 ? 1 : 0;
      localStorage.setItem("addFlex", this.addFlex);
    },
    // 展开折叠
    toggleExpandAll() {
      this.activeNames = [];
    },
    // 开启定时器
    openTimer() {
      this.getList();
      if (this.timer) {
        clearInterval(this.timer);
        this.timer = null;
      }
      // 每隔5秒自动刷新
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.getList(); //调用接口的方法
        }, 0);
      }, this.schedule_dashboard_interval);
    },
    // 关闭定时器
    closeTimer() {
      clearInterval(this.timer);
      this.timer = null;
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
    // 获取未分配调度记录
    getSchedule() {
      this.count++;
      if (this.count > 1) {
        this.loading = false;
      } else {
        this.loading = true;
      }
      getScheduleList({
        ...this.queryParams,
        routeCodeList: this.searchForm.not_assigned_routeList,
        isFromBigScreenWeb: true,
        requestDeviceKindList: this.searchForm.isOnlyDrill ? [2, 3] : undefined,
      }).then((res) => {
        if (!res) return;
        this.schedulementList = res.data?.list.map((v) => {
          return { ...v, initiateDevice: this.getDeviceLabel(v) };
        });
        this.total = res.data?.total;
        this.loading = false;
      });
    },
    // 获取已开始
    getRunningSchedule() {
      getRunningScheduleList(this.runingQueryParams).then((res) => {
        if (!res) return;
        this.runingSchedulementList = res.data?.list.map((v) => {
          return { ...v, initiateDevice: this.getDeviceLabel(v) };
        });
        this.runingTotal = res.data?.total;
      });
    },

    //获取AGV
    getAgvList() {
      //获取AGV视图数据
      getAGVDeviceSiloInfo({
        routeCodeList: this.searchForm.agv_routeList,
        deviceStatuseLists: this.searchForm.agv_deviceStatusList,
      }).then((res) => {
        if (res?.code == 0) {
          this.agvList = res.data?.list;
        }
      });
    },
    // 获取中控是否在维护
    getIsMaintaining() {
      getCentralControlSystemIsMaintaining().then((res) => {
        if (res) return this.$modal.msgWarning("系统正在维护");
      });
    },
    getList() {
      this.getRouteList();
      this.getSchedule();
      this.getRunningSchedule();
      this.getAgvList();
      this.getIsMaintaining();
    },
    //重置
    onSubmit(val) {
      if (val == "AGV") {
        this.searchForm.agv_routeList = [];
        this.searchForm.agv_deviceStatusList = [];
      } else {
        this.searchForm.not_assigned_routeList = [];
      }
      this.handleClear();
    },
    changeIsOnlyDrill() {
      this.closeTimer();
      this.handleClear();
    },
    handleClear(val) {
      setTimeout(() => {
        this.openTimer();
      }, 300);
    },
    handleFocus(val) {
      if (val) {
        // console.log("获取焦点停用定时");
        this.closeTimer();
      } else {
        // console.log("失去焦点启用定时");
        this.openTimer();
      }
    },
    // 点击详情
    handleView(val) {
      const data = JSON.parse(val);
      this.detailId = data.id;
      if (data.type == "detail") {
        this.$refs.MyDrawer.getDetail(this.detailId);
      } else {
        // this.$refs.MyPanel.open = true;
      }
    },
    // 点击产品编码打开弹框
    openItemSearchDialog(code) {
      this.$refs.ItemSearchDialog.open = true;
      this.$refs.ItemSearchDialog.title = `查看库存--(${code})`;
      this.$refs.ItemSearchDialog.queryParams.itemCode = code;
      this.$refs.ItemSearchDialog.getList();
    },
    // 阻止冒泡
    showCollapse(e) {
      e?.stopPropagation();
    },
  },
};
</script>
<style lang="scss" scoped>
.dashboard-editor-container {
  background-color: rgb(240, 242, 245);
}
.top_flex {
  display: flex;
  position: relative;
  .top {
    flex: 1;
  }
  .top:nth-child(2) {
    margin-right: 5px;
  }
}
.flex {
  position: relative;
}
.top_flex_btn {
  position: absolute;
  right: 5px;
  top: 5px;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.12);
  font-size: 16px;
  z-index: 100;
}
.top {
  margin-bottom: 10px;
  .pagination-container {
    padding: 0 !important;
    margin-top: 15px !important;
    margin-bottom: 0px !important;
  }
}

.collapse-title {
  flex: 1 0 90%;
  order: 1;
}
::v-deep .el-card__header {
  padding: 10px 15px 7px;
}
::v-deep .el-collapse-item__header {
  flex: 1 0 auto;
  order: -1;
  font-size: 17px !important;

  // line-height: 0;
  // height: auto;
  // padding-left: 5px;
}
.one-line {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  text-decoration: underline;
  color: #1890ff;
}
.one-line-red {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  text-decoration: underline;
  color: red;
}
.one-line:hover {
  cursor: pointer;
}
.tooltip_li {
  margin-top: 2px;
  line-height: 18px;
  word-break: break-all;
}

::v-deep .el-form-item {
  margin-bottom: 0px !important;
  margin-right: 10px !important;
}

::v-deep .el-form-item.form-title {
  .el-form-item__label {
    font-size: 17px !important;
    padding: 0;
    font-weight: normal;
  }
}
</style>
