<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      :isFixed="true"
      @search="handleQuery"
      @reset="resetQuery"
      ref="queryForm"
    >
      <el-form-item label="是否刷新">
        <!-- <el-checkbox v-model="queryParams.checked"></el-checkbox> -->
        <el-switch v-model="queryParams.checked" />
      </el-form-item>
      <el-form-item label="频率" prop="refresh">
        <el-select
          v-model="queryParams.refresh"
          placeholder="请选择"
          @change="handleRefreshSelect"
          style="width: 150px"
        >
          <el-option :label="'5秒'" :value="5" />
          <el-option :label="'10秒'" :value="10" />
          <el-option :label="'30秒'" :value="30" />
          <el-option :label="'1分钟'" :value="60" />
        </el-select> </el-form-item
      ><el-form-item label="是否显示历史任务">
        <el-switch v-model="queryParams.isHistory" />
      </el-form-item>

      <el-form-item label="工单编码" prop="workOrderCode">
        <el-input
          v-trim
          v-model="queryParams.workOrderCode"
          placeholder="请输入工单编码"
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
      <el-form-item label="工艺路线" prop="routeCodeList">
        <el-select
          @change="handleChangeRoute"
          v-model="queryParams.routeCodeList"
          placeholder="请选择"
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
          />
        </el-select>
      </el-form-item>
      <el-form-item label="是否自动" prop="isAuto">
        <el-select
          @clear="clearQueryParams('isAuto')"
          v-model="queryParams.isAuto"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option label="自动" :value="true" />
          <el-option label="非自动" :value="false" />
        </el-select>
      </el-form-item>
      <el-form-item label="开始日期" prop="startDate">
        <el-date-picker
          v-model="queryParams.startDate"
          type="date"
          placeholder="选择日期"
          style="width: 180px"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="查看范围" prop="taskNumber">
        <el-select v-model="queryParams.taskNumber" style="width: 150px">
          <el-option :label="'三天'" :value="3" />
          <el-option :label="'五天'" :value="5" />
          <el-option :label="'七天'" :value="7" />
        </el-select>
      </el-form-item>
    </search-form>

    <el-row
      :class="whether ? 'isRowFixed' : ''"
      :style="
        whether
          ? {
              width: '100%',
              top: $refs.queryForm.$el.offsetHeight + 'px',
            }
          : ''
      "
      :gutter="10"
      ref="topRow"
      class="topRow"
    >
      <el-col :span="1.5" v-if="visibleAndHidden">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-sort"
          :disabled="hasPermi(['produce:taskCharts:batchRationalizeTask'])"
          @click="handleBatchRationalizeTask"
          >重排计划</el-button
        >
      </el-col>
      <el-col :span="1.5" v-if="visibleAndHidden">
        <el-button
          v-debounce
          plain
          type="primary"
          icon="el-icon-check"
          @click="handleCommitAll"
          :disabled="hasPermi(['produce:taskCharts:commit'])"
          >批量提交</el-button
        >
      </el-col>
      <el-col :span="1.5" v-if="visibleAndHidden">
        <el-button
          v-debounce
          plain
          type="warning"
          icon="el-icon-refresh-left"
          @click="handleRevokeAll"
          :disabled="hasPermi(['produce:taskCharts:revoke'])"
        >
          批量撤销
        </el-button>
      </el-col>
      <!-- <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-share"
          @click="handleTransferTask"
          :disabled="hasPermi(['produce:tasks:transferTask'])"
          >批量转发</el-button
        ></el-col
      > -->

      <el-col :span="1.5">
        <el-dropdown trigger="click" :hide-on-click="false">
          <el-button type="primary" plain>
            选择展示字段<i class="el-icon-arrow-down el-icon--right"></i>
          </el-button>
          <el-dropdown-menu
            slot="dropdown"
            style="width: 130px; box-shadow: 0 2px 10px 2px #92b6dd"
          >
            <el-dropdown-item style="padding-left: 8px">
              <el-checkbox-group v-model="infoShowList">
                <el-checkbox
                  class="dropdown-checkbox"
                  label="任务编码"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="工单编码"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="物料编码"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="工艺路线"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="钻机编码"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="开始时间"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="生产叠数"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="状态"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="预计完成时长"
                ></el-checkbox>
                <el-checkbox
                  class="dropdown-checkbox"
                  label="组计划编号"
                ></el-checkbox>
              </el-checkbox-group>
            </el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </el-col>
      <el-col :span="1.5">
        <el-button
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleClose"
          v-if="$route.path == '/produce/drillTask/taskedit'"
          >关闭</el-button
        >
      </el-col>
      <el-col :span="24">
        <div class="status-color-container">
          <div
            class="status-color"
            v-for="item in $status.taskOptions"
            :key="item.value"
          >
            <span
              :style="{
                background: item.background,
                border: 'solid 1px' + item.color,
              }"
            ></span>
            <span>{{ item.label }}</span>
          </div>
        </div>
      </el-col>
    </el-row>
    <div
      v-if="loading"
      class="loading-mask"
      v-loading="loading"
      :style="{
        height: '100vh',
        ...isLeftBtnFixed,
        top: task_top_height + 'px',
      }"
    ></div>
    <div class="wrapper" id="menuFlag">
      <div
        class="left-btn-box"
        :class="whether ? 'isBtnFixed' : ''"
        :style="
          whether
            ? {
                ...isLeftBtnFixed,
                top: task_top_height + 'px',
              }
            : ''
        "
      >
        <el-tooltip effect="dark" content="左滑单个" placement="top">
          <!-- 左边按钮单个 -->
          <el-button
            :disabled="btnLeft"
            class="btn"
            @click="scrollLeft"
            icon="el-icon-caret-left"
          ></el-button>
        </el-tooltip>
        <el-tooltip effect="dark" content="批量左滑" placement="bottom">
          <!-- 左边批量按钮 -->
          <el-button
            :disabled="btnLeft"
            class="btn"
            @click="scrollLeft(batchNum)"
            icon="el-icon-d-arrow-left"
          ></el-button>
        </el-tooltip>
      </div>

      <div class="task-hidden-container" @contextmenu.prevent>
        <div class="fixed-date-container">
          <div
            ref="isSelectAll"
            :class="whether ? 'isDateFixed' : 'isSelectAll'"
            :style="
              whether ? { ...isDateFixed, top: task_top_height + 'px' } : ''
            "
          >
            <el-checkbox
              @change="handleSelectAll"
              v-model="isSelectAll"
              style="margin: 0 3px"
              v-if="visibleAndHidden"
            ></el-checkbox
            >钻孔任务
          </div>
          <li v-for="(date, dateI) in dateList" :key="date">
            <span class="date" :class="date == 'Invalid' ? 'date-after' : ''">
              {{ date == "Invalid" ? "历史任务" : date }}
            </span>
            <div class="times" v-if="date != 'Invalid'">
              <div
                v-for="task in times[dateI] && times[dateI].times"
                :key="task"
                class="time"
                ref="time"
                :data-date="date"
                :data-time="task"
              >
                {{ task }}
              </div>
            </div>
          </li>
        </div>
        <div class="all-container">
          <div
            class="drill-container"
            :class="whether ? 'isFixed' : ''"
            :style="
              whether
                ? {
                    ...isFixed,
                    top: task_top_height + 'px',
                  }
                : ''
            "
            ref="drillContainer"
          >
            <div class="drill-item"></div>
            <div
              v-for="item in workStationList"
              :key="item.id"
              class="drill-item"
            >
              <el-checkbox
                @change="handleSelect(item)"
                v-model="item.checked"
                v-if="visibleAndHidden"
                style="margin: 0 3px"
              ></el-checkbox>
              <el-tooltip
                v-if="item.routeCode"
                :content="'所属工艺路线:' + item.routeCode"
                placement="top-end"
              >
                <span @click="goToLink(item)">{{ item.code }}</span>
              </el-tooltip>
              <span @click="goToLink(item)" v-else>{{ item.code }}</span
              ><i
                v-if="item.deviceConsoleAddress"
                @click="goToLink(item)"
                class="el-icon-paperclip"
              ></i>
            </div>
          </div>

          <div
            v-for="(date, dateI) in dateList"
            :key="date"
            class="task-container"
          >
            <div class="date-container"></div>
            <div
              class="drag-container"
              :ref="date == 'Invalid' ? 'drag-container' : ''"
            >
              <div
                :style="{
                  minHeight: date == 'Invalid' ? '40px' : '',
                  padding: date == 'Invalid' ? '10px 4px' : 0,
                }"
                :class="date == 'Invalid' ? 'drag-item-after' : ''"
                class="drag-item"
                v-for="(works, worksI) in times[dateI].children"
                :key="works.id"
                :ref="works.id + getDate(date)"
                @contextmenu.prevent="
                  openMenu(list[dateI].children[worksI].arr, date, $event)
                "
              >
                <div v-for="(v, i) in works.arr" :key="v.time">
                  <draggable
                    :scroll="true"
                    v-model="v.child"
                    filter=".undraggable"
                    group="name"
                    animation="150"
                    dragClass="dragClass"
                    ghostClass="ghostClass"
                    :move="onMove"
                    @start="onStart"
                    @end="onEnd"
                    :class="
                      works.arr.length - 1 == i || date == 'Invalid'
                        ? ''
                        : 'draggable'
                    "
                  >
                    <transition-group
                      ref="item"
                      class="transition-box"
                      :data-workStationInfo="
                        JSON.stringify({
                          code: workStationList[worksI].code,
                          id: workStationList[worksI].id,
                          name: workStationList[worksI].name,
                        })
                      "
                      :data-time="times[dateI].times[i]"
                      :data-date="date"
                      :style="{
                        padding:
                          v.child.length <= 0 || date == 'Invalid' ? 0 : '4px',
                      }"
                    >
                      <div
                        class="item"
                        :class="{
                          'undraggable unitem':
                            !statusList.includes(drag.taskStatus) ||
                            !visibleAndHidden,
                        }"
                        :style="{
                          background: $status.taskOptions.find(
                            (v) => v.value == drag.taskStatus
                          ).background,
                          color: $status.taskOptions.find(
                            (v) => v.value == drag.taskStatus
                          ).color,
                          border:
                            'solid 1px' +
                            $status.taskOptions.find(
                              (v) => v.value == drag.taskStatus
                            ).color,
                        }"
                        v-for="drag in v.child"
                        :data-id="drag.id"
                        :data-time="v.time"
                        :data-date="date"
                        :key="drag.code"
                      >
                        <i
                          class="el-icon-star-on"
                          style="color: #f05b59"
                          v-if="drag.isUrgent == 1"
                        />

                        <div class="toolpit_li_box">
                          <li v-if="infoShowList.includes('任务编码')">
                            <span>任务编码: </span>
                            <span>{{ drag.code }}</span>
                          </li>
                          <li v-if="infoShowList.includes('工单编码')">
                            <span>工单编码: </span>
                            <span>{{ drag.workOrderCode }}</span>
                          </li>

                          <li v-if="infoShowList.includes('物料编码')">
                            <span>物料编码: </span>
                            <span>{{ drag.itemCode }}</span>
                          </li>
                          <li v-if="infoShowList.includes('工艺路线')">
                            <span>工艺路线: </span>
                            <span>{{ drag.routeCode }}</span>
                          </li>
                          <li v-if="infoShowList.includes('钻机编码')">
                            <span>钻机编码: </span>
                            <span>{{ drag.workStationCode }}</span>
                          </li>
                          <li v-if="infoShowList.includes('组计划编号')">
                            <span>组计划编号: </span>
                            <span>{{ drag.cutterGroupNo }}</span>
                          </li>
                          <li
                            class="start_date"
                            v-if="infoShowList.includes('开始时间')"
                          >
                            <span>开始时间: </span>
                            <span>
                              {{
                                getStartDateOrEndDate(date, drag.startTime)
                              }}</span
                            >
                          </li>
                          <li
                            class="start_date"
                            v-if="infoShowList.includes('生产叠数')"
                          >
                            <span>本次生产叠数: </span>
                            <span>{{ drag.nowWadCount }}</span>
                          </li>
                          <li
                            class="start_date"
                            v-if="infoShowList.includes('状态')"
                          >
                            <span>状态: </span>
                            <span>{{
                              $status.taskOptions.find(
                                (v) => v.value == drag.taskStatus
                              ).label
                            }}</span>
                          </li>
                          <li
                            class="start_date"
                            v-if="infoShowList.includes('预计完成时长')"
                          >
                            <span>预计完成时长: </span>
                            <span>{{
                              drag.duration ? `${drag.duration} (分钟)` : ""
                            }}</span>
                          </li>
                        </div>
                        <div
                          v-if="visibleAndHidden"
                          :style="{ marginLeft: '5px', zIndex: 0 }"
                        >
                          <el-checkbox
                            v-if="statusList.includes(drag.taskStatus)"
                            v-model="drag.checked"
                            @change="onDragChecked(drag)"
                          ></el-checkbox>
                        </div>
                        <i
                          v-show="!statusList.includes(drag.taskStatus)"
                          style="margin-left: 5px"
                          class="el-icon-lock"
                        ></i>
                      </div>
                    </transition-group>
                  </draggable>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div
        class="right-btn-box"
        :class="whether ? 'isBtnFixed' : ''"
        :style="
          whether ? { ...isRightBtnFixed, top: task_top_height + 'px' } : ''
        "
      >
        <el-tooltip effect="dark" content="右滑单个" placement="top">
          <!-- 右边按钮 -->
          <el-button
            :disabled="btnRight"
            class="btn"
            @click="scrollRight"
            icon="el-icon-caret-right"
          ></el-button>
        </el-tooltip>
        <el-tooltip effect="dark" content="批量右滑" placement="bottom">
          <!-- 右边批量按钮 -->
          <el-button
            :disabled="btnRight"
            class="btn"
            @click="scrollRight(batchNum)"
            icon="el-icon-d-arrow-right"
          ></el-button>
        </el-tooltip>
      </div>
    </div>
    <!-- 滚动后底部左滑右滑按钮 -->
    <fixedBtn
      v-if="isShowBtn"
      @scrollLeft="scrollLeft"
      @scrollRight="scrollRight"
      :btnLeft="btnLeft"
      :btnRight="btnRight"
      :batchNum="batchNum"
    />
    <!-- 右键菜单 -->
    <template v-if="visibleAndHidden">
      <contextmenu
        v-model="infoShowList"
        :left="left"
        :top="top"
        v-if="visible"
        @handleMenuSelect="handleMenuSelect"
        @handleUnCheckAll="handleUnCheckAll"
        @handleCommitAll="handleCommitAll"
        @handleRevokeAll="handleRevokeAll"
        @handleTransferTask="handleTransferTask"
    /></template>
    <!-- 方向按钮 -->
    <controlBtn
      :show="selectList.length > 0 && isControl"
      @handleControl="handleControl"
      @handleControlOk="handleControlOk"
      :targetCode="targetCode"
      :targetDate="targetDate"
      :targetRouteCode="targetRouteCode"
      :batchNum="batchNum"
      v-if="visibleAndHidden"
    />
    <!-- 拖拽完成的提示框 -->
    <dragDialog
      v-model="to"
      ref="dragDialog"
      @moveTask="taskLoadingFinish"
      @getList="getList"
    />
    <!-- 转发任务 -->
    <TransferTask
      ref="TransferTask"
      :tasks="selectList"
      @onSelected="onTransferTaskSelected"
    />
  </div>
</template>

<script>
import Cookies from "js-cookie";
import { mapGetters } from "vuex";
import {
  batchRationalizeTask,
  listDrill,
  moveTask,
} from "@/api/produce/drillWorkOrder.js";
import draggable from "vuedraggable";
import { mergeList, isAllEqual, removeDuplicate, copyText } from "./function";
import { getDropSelectDatas } from "@/api/produce/route";
import {
  commitTask,
  revokeCommit,
  verifyTaskBelongOneRouteAndProcess,
} from "@/api/produce/task";
import contextmenu from "./components/contextmenu.vue";
import ControlBtn from "./components/control.vue";
import fixedBtn from "./components/fixedBtn.vue";
import dragDialog from "./components/dragDialog.vue";
import { scrollTo } from "@/utils/scroll-to";
import TransferTask from "@/views/produce/tasks/transferTask.vue";
export default {
  name: "TaskCharts",
  components: {
    draggable,
    contextmenu,
    ControlBtn,
    fixedBtn,
    dragDialog,
    TransferTask,
  },
  data() {
    return {
      // 显示搜索条件
      showSearch: true,
      loading: false,
      list: [],
      workStationList: [],
      dateList: [],
      //  选择展示字段
      infoShowList: JSON.parse(localStorage.getItem("infoShowList"))
        ? JSON.parse(localStorage.getItem("infoShowList"))
        : ["任务编码", "工单编码", "物料编码", "预计完成时长", "组计划编号"],
      // 可以进行拖拽的任务状态
      statusList: [0, 10],
      queryParams: {
        // 默认查询 草稿 已提交 派送中 的任务
        taskStatusList: ["0", "10", "20"],
        isHistory: true,
        startDate: new Date().toLocaleDateString("sv-SE"),
        processCode: "drill",
        taskNumber: 7,
        workOrderCode: undefined,
        itemCode: undefined,
        routeCodeList: [],
        isAuto: undefined,
        refresh: this.$cache.local.get("taskCharts_queryParams")
          ? JSON.parse(this.$cache.local.get("taskCharts_queryParams")).refresh
          : 30,
        checked: this.$cache.local.get("taskCharts_queryParams")
          ? JSON.parse(this.$cache.local.get("taskCharts_queryParams")).checked
          : false,
      },
      routeQueryList: [],
      // 拖拽字段
      dragItem: null,
      to: {},
      from: {},
      // 头部钻机是否全选
      isSelectAll: false,
      // 选择的头部钻机
      selectWorkStationList: [],
      // 选中拖动的数组
      selectList: [],
      // 是否允许向历史任务拖入
      isDragIn: true,
      checkHistory: false,
      // 右击菜单
      visible: false,
      top: 0,
      left: 0,
      menuSelectArr: [],
      // 方向控制器操作任务字段
      leftCount: 0,
      topCount: 0,
      rightCount: 0,
      bottomCount: 0,
      targetCode: undefined,
      targetDate: undefined,
      targetRouteCode: undefined,
      isControl: false,
      dateList: [],
      // 批量滑动数量
      batchNum: 5,
      // 是否显示底部按钮
      isShowBtn: false,
      btnLeft: false,
      btnRight: false,
      allLength: 0,
      boxLength: 0,
      taskListEl: null,
      // 处理好的数据
      times: [],
      dataId: 0,
      isFixed: {},
      isDateFixed: {},
      isLeftBtnFixed: {},
      isRightBtnFixed: {},
      whether: false,
      task_top_height: 0,
      timer: null,
    };
  },
  computed: {
    ...mapGetters(["sidebar", "device"]),
    // 拿出要监听的属性
    listenChange() {
      const { refresh, checked } = this.queryParams;
      return { refresh, checked };
    },
    queryParamsChange() {
      return { ...this.queryParams };
    },
    getDate() {
      return (date) => {
        return new Date(date).toLocaleDateString("sv-SE").split(" ")[0];
      };
    },
    getStartDateOrEndDate() {
      //校验是否同年
      function checkIsSameYear(date) {
        const year = new Date(date).getFullYear();
        const currentYear = new Date().getFullYear();
        if (year == currentYear) {
          return true;
        }
        return false;
      }
      return (date, time) => {
        let str = "";
        if (date == "Invalid" && !checkIsSameYear(time)) {
          str = this.parseTime(time, "{y}-{m}-{d} {h}:{i}:{s}");
        } else if (date == "Invalid" && checkIsSameYear(time)) {
          str = this.parseTime(time, "{m}-{d} {h}:{i}:{s}");
        } else {
          str = this.parseTime(time, "{h}:{i}:{s}");
        }
        return str;
      };
    },
    // 显示隐藏
    visibleAndHidden() {
      // 是否超级管理员角色(为true)
      const hasRole = this.hasRole(["admin"]);
      // 是否仅查看
      const hasPermi = this.hasPermi(["produce:taskCharts:view"]);
      // 超级管理员默认可显示及操作所有
      if (hasRole) return true;
      return hasPermi;
    },
  },
  watch: {
    queryParamsChange: {
      handler(val) {
        this.$cache.local.set(
          "taskCharts_queryParams",
          JSON.stringify({ refresh: val.refresh, checked: val.checked })
        );
      },
      deepL: true,
      immediate: true,
    },
    listenChange: {
      // 开启深度监听
      handler(val, old) {
        const { refresh, checked } = val;
        if (checked) {
          if (this.timer != null) {
            clearInterval(this.timer);
            this.timer = null;
          } else {
            this.changeSetInterval(refresh, "监听页面");
          }
        } else {
          clearInterval(this.timer);
          this.timer = null;
        }
      },
      immediate: true,
      deep: true,
    },
    visible(value) {
      if (value) {
        document.body.addEventListener("click", this.closeMenu);
      } else {
        document.body.removeEventListener("click", this.closeMenu);
      }
    },
    // 本地存储信息展示
    infoShowList(val) {
      localStorage.setItem("infoShowList", JSON.stringify(val));
      this.changeStyle();
    },
    // 监听菜单收缩
    device: {
      handler(val) {
        const control = document.querySelector(".control-wrapper");
        if (val == "desktop") {
          if (this.sidebar.opened) {
            this.isDateFixed = {
              left: "250px",
            };
            this.isLeftBtnFixed = {
              left: "209px",
            };
          } else {
            this.isDateFixed = {
              left: "104px",
            };
            this.isLeftBtnFixed = {
              left: "62px",
            };
          }

          if (control?.getBoundingClientRect().left <= 0) {
            control.style.left = "54px";
          }
        } else {
          this.isDateFixed = {
            left: "50px",
          };
          this.isLeftBtnFixed = {
            left: "9px",
          };
          if (control?.getBoundingClientRect().left <= 54) {
            control.style.left = "0px";
          }
        }
      },
      immediate: true,
      deep: true,
    },
    "sidebar.opened": {
      handler(val) {
        if (this.device == "mobile") return;
        this.changeControlLeft(val);
        this.isFixed = {
          border: "1px solid #888",
          borderBottom: "none",
          borderLeft: "none",
        };
        this.isRightBtnFixed = {
          marginRight: "10px",
        };
        if (val) {
          this.isDateFixed = {
            left: "250px",
          };
          this.isLeftBtnFixed = {
            left: "209px",
          };
        } else {
          this.isDateFixed = {
            left: "104px",
          };
          this.isLeftBtnFixed = {
            left: "62px",
          };
        }
      },
      immediate: true,
      deep: true,
    },
    selectList(val) {
      if (val.length <= 0) {
        // 方向操作任务字段重置
        this.targetCode = undefined;
        this.targetDate = undefined;
        this.targetRouteCode = undefined;
        this.isControl = false;
        this.to = undefined;
        this.topCount = 0;
        this.bottomCount = 0;
        this.leftCount = 0;
        this.rightCount = 0;
        this.$nextTick(() => {
          const elements = document.querySelectorAll(".drag-item");
          elements.forEach((v) => {
            v.classList.contains("target-item")
              ? v.classList.remove("target-item")
              : "";
          });
        });
        this.queryParams.checked = true;
      } else {
        if (this.timer) {
          this.$modal.msgWarning("警告,定时刷新已关闭,稍后请手动开启!");
        }
        // 点击选中时先取消定时刷新
        this.queryParams.checked = false;
        clearInterval(this.timer);
        this.timer = null;
      }
    },
    selectWorkStationList(val) {
      if (val.length > 0) {
        if (this.timer) {
          this.$modal.msgWarning("警告,定时刷新已关闭,稍后请手动开启!");
        }
        // 点击选中时先取消定时刷新
        this.queryParams.checked = false;
        clearInterval(this.timer);
        this.timer = null;
      } else {
        this.queryParams.checked = true;
      }
    },
  },
  activated() {
    const { refresh, checked } = this.queryParams;
    if (checked) {
      if (this.timer != null) {
        clearInterval(this.timer);
        this.timer = null;
        this.changeSetInterval(refresh, "进入页面时1");
      } else {
        this.changeSetInterval(refresh, "进入页面时2");
      }
    } else {
      clearInterval(this.timer);
      this.timer = null;
    }
    this.getRouteList();
    this.changeControlLeft(this.sidebar.opened);
    window.addEventListener("scroll", this.handleScroll, true);
    window.addEventListener("resize", this.handleScroll);

    this.$el.addEventListener("dblclick", copyText);
  },
  deactivated() {
    window.removeEventListener("scroll", this.handleScroll, true);
    window.removeEventListener("resize", this.handleScroll, true);

    this.$el.removeEventListener("dblclick", copyText);
    this.isShowBtn = false;
    //  关闭定时器
    clearInterval(this.timer);
    this.timer = null;
  },
  methods: {
    // 定时器
    changeSetInterval(refresh, label) {
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.getList();
          console.log(label);
          this.selectWorkStationList = [];
        }, 0);
      }, refresh * 1000);
    },
    // 切换刷新频率
    handleRefreshSelect(val) {
      clearInterval(this.timer);
      this.timer = null;
    },
    // 改变时间样式
    changeStyle() {
      const that = this;
      setTimeout(() => {
        const date_historys = that.$refs["drag-container"];
        const date_history = document.querySelector(".date-after");
        if (date_history) {
          date_history.style.height =
            date_historys[0]?.getBoundingClientRect().height + "px";
        }
        const items = that.$refs["item"];
        const times = that.$refs.time;
        if (!items || !times) return;
        this.$refs.isSelectAll.style.height =
          this.$refs.drillContainer?.getBoundingClientRect().height + 1 + "px";
        times.forEach((v, i) => {
          let items_filter = [];
          let items_doms = [];
          let items_children = [];
          items &&
            items.forEach((item) => {
              if (
                v.dataset.date == item.$vnode.data.attrs["data-date"] &&
                v.dataset.time == item.$vnode.data.attrs["data-time"]
              ) {
                items_filter.push(item.$el?.getBoundingClientRect().height);
                items_doms.push(item.$el);
                items_children.push(item.$el.querySelectorAll(".item"));
              }
            });
          const maxChildren = [...items_children].reduce((p, v) =>
            p.length < v.length ? v : p
          );
          let num = 0;
          if (maxChildren.length == 0) {
            // 默认40高度防止无法拖拽
            num = 40;
          } else {
            const n =
              // +margin
              (maxChildren[0].getBoundingClientRect().height + 6) *
              maxChildren.length;
            // +padding
            num = n + 14;
          }
          v.style.height = num + "px";
          items_doms.forEach((v, i) => {
            v.parentNode.style.height = num + "px";
            v.style.minHeight = num + "px";
          });
        });
      }, 150);
    },
    // 改变方向控制器偏移
    changeControlLeft(val) {
      const control = document.querySelector(".control-wrapper");
      // 方向控制器left
      if (control?.getBoundingClientRect().left == 200 && !val) {
        control.style.left = "54px";
      } else if (control?.getBoundingClientRect().left < 200 && val) {
        control.style.left = 200 + "px";
      }
    },
    getRouteList() {
      getDropSelectDatas({ pageNum: 1, pageSize: 1000, vettingStatus: 1 }).then(
        (res) => {
          this.routeQueryList = res.data?.map((v) => {
            return { ...v, label: `${v.code}(${v.name})` };
          });
          this.queryParams.routeCodeList = JSON.parse(
            localStorage.getItem("task_routeCodeList")
          )
            ? JSON.parse(localStorage.getItem("task_routeCodeList"))
            : [this.routeQueryList[0]?.code];
        }
      );
    },
    // 获取钻孔任务
    getList() {
      this.queryParams.routeCodeList = JSON.parse(
        localStorage.getItem("task_routeCodeList")
      )
        ? JSON.parse(localStorage.getItem("task_routeCodeList"))
        : [this.routeQueryList[0]?.code];
      this.loading = true;
      this.queryParams.startDate = new Date(
        this.queryParams.startDate
      ).toLocaleDateString("sv-SE");
      this.isHistory = this.queryParams.isHistory;
      listDrill(this.queryParams)
        .then((res) => {
          const list =
            res.data.list &&
            res.data.list.map((v) => {
              if (v.date == "history") {
                return {
                  ...v,
                  children: v.children.map((v1) => {
                    return {
                      ...v1,
                      arr: v1.arr.map((v2) => {
                        return { ...v2, checked: false, history: true };
                      }),
                    };
                  }),
                };
              }
              return {
                ...v,
                children: v.children.map((v1) => {
                  return {
                    ...v1,
                    arr: v1.arr.map((v2) => {
                      return { ...v2, checked: false };
                    }),
                  };
                }),
              };
            });
          this.list = list;
          this.dateList = this.list.map(
            (v) => new Date(v.date).toLocaleDateString("sv-SE").split(" ")[0]
          );
          this.workStationList =
            res.data.workStationList &&
            res.data.workStationList.map((v) => {
              return {
                ...v,
                checked: false,
              };
            });
          this.times = mergeList(list, this.workStationList);
          this.changeStyle();
          setTimeout(() => {
            this.loading = false;
          }, 300);
          this.init();
          // 方向操作任务字段重置
          this.targetCode = undefined;
          this.targetDate = undefined;
          this.targetRouteCode = undefined;
          this.isControl = false;
          this.to = undefined;
          this.topCount = 0;
          this.bottomCount = 0;
          this.leftCount = 0;
          this.rightCount = 0;
          this.selectList = [];
        })
        .catch(() => {
          // 请求失败
          this.loading = false;
        });
    },
    /** 搜索按钮操作 */
    handleQuery() {
      // 到最开始的时候
      if (this.taskListEl) {
        this.taskListEl.style.left = "0px";
      }
      this.getList();
      this.selectWorkStationList = [];
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.queryParams.isHistory = true;
      this.queryParams.routeCodeList = [];
      localStorage.removeItem("task_routeCodeList");
      this.isSelectAll = false;
      this.handleQuery();
    },
    handleChangeRoute(val) {
      localStorage.setItem("task_routeCodeList", JSON.stringify(val));
    },
    // 点击关闭跳转到钻孔任务页面
    handleClose() {
      const obj = { path: "/produce/drillWorkOrder" };
      this.$tab.closeOpenPage(obj);
    },
    // 左滑动逻辑
    scrollLeft(num) {
      num = typeof num == "number" ? num : 1;
      this.btnRight = false;
      if (this.allLength <= this.boxLength) {
        this.btnRight = true;
      }
      let leftMove = 0;
      if (this.taskListEl.style.left) {
        leftMove = Math.abs(parseInt(this.taskListEl.style.left));
      }
      if (leftMove + this.boxLength - 180 * num <= this.boxLength) {
        // 到最开始的时候
        this.taskListEl.style.left = "0px";
        this.btnLeft = true;
      } else {
        this.taskListEl.style.left = "-" + (leftMove - 180 * num) + "px";
        this.btnLeft = false;
      }
    },
    // 右滑动逻辑
    scrollRight(num) {
      num = typeof num == "number" ? num : 1;
      this.btnLeft = false;
      if (this.allLength <= this.boxLength) {
        this.btnLeft = true;
      }
      let leftMove = 0;
      if (this.taskListEl.style.left) {
        leftMove = Math.abs(parseInt(this.taskListEl.style.left));
      }
      if (leftMove + this.boxLength + 180 * num >= this.allLength) {
        this.taskListEl.style.left =
          "-" + (this.allLength - this.boxLength) + "px";
        this.btnRight = true;
      } else {
        this.taskListEl.style.left = "-" + (leftMove + 180 * num) + "px";
        this.btnRight = false;
      }
    },
    // 点击重排
    handleBatchRationalizeTask() {
      if (this.selectWorkStationList.length <= 0)
        return this.$modal.msgWarning("请选择要操作的工作站!");

      batchRationalizeTask({
        startDate: this.queryParams.startDate,
        processCode: "drill",
        workStationList: this.selectWorkStationList,
      }).then((res) => {
        if (res.code == 0) {
          this.isSelectAll = false;
          this.selectWorkStationList = [];
          this.$modal.msgSuccess("操作成功");
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // confirm确认框
    async handleConfirm(label, taskStatus, api, apiParams) {
      if (this.selectList.length <= 0)
        return this.$modal.msgWarning("请选择要操作的任务!");
      let falg;
      if (Array.isArray(taskStatus)) {
        falg = this.selectList.some((item) => {
          return (
            item.taskStatus != taskStatus[0] &&
            item.taskStatus != taskStatus[1] &&
            item.taskStatus != taskStatus[2]
          );
        });
      } else {
        falg = this.selectList.some((item) => item.taskStatus != taskStatus);
      }
      if (falg)
        return this.$modal.notifyError("当前的数据不能执行" + label + "操作!");
      const results = await this.$modal
        .confirm(
          "确认" +
            label +
            " " +
            this.selectList[0].workStationCode +
            " 下的任务吗？"
        )
        .catch(() => {});
      if (results == "confirm") {
        api(apiParams).then((res) => {
          if (res.code == 0) {
            this.getList();
            this.$modal.msgSuccess("操作成功");
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 点击提交按钮
    handleCommitAll() {
      const ids = this.selectList.map((v) => v.id);
      const params = {
        ids,
        taskStatus: 10,
      };
      this.handleConfirm("提交", 0, commitTask, params);
    },
    // 点击撤销提交
    handleRevokeAll() {
      const ids = this.selectList.map((v) => v.id);
      const params = {
        ids,
        taskStatus: 0,
      };
      this.handleConfirm("撤销", 10, revokeCommit, params);
    },
    // 跳转链接
    goToLink(item) {
      if (item.deviceConsoleAddress)
        return window.open(item.deviceConsoleAddress, "_blank");
    },
    // 全选
    handleSelectAll(val) {
      if (val) {
        this.workStationList = this.workStationList.map((v) => {
          this.selectWorkStationList.push(v.code);
          return { ...v, checked: true };
        });
      } else {
        this.workStationList = this.workStationList.map((v) => {
          return { ...v, checked: false };
        });
        this.selectWorkStationList = [];
      }
    },
    // 反选
    handleSelect(val) {
      if (val.checked) {
        this.selectWorkStationList.push(val.code);
        this.selectWorkStationList = [...new Set(this.selectWorkStationList)];
      } else {
        this.selectWorkStationList = this.selectWorkStationList.filter(
          (v) => v !== val.code
        );
      }
      if (this.selectWorkStationList.length == this.workStationList.length) {
        this.isSelectAll = true;
      } else {
        this.isSelectAll = false;
      }
    },
    // 拖动中事件(禁止向历史任务中拖入)
    onMove(event) {
      this.changeStyle();
      this.isDragIn = true;
      if (event.to.dataset.date == "Invalid") {
        this.isDragIn = false;
        return false;
      }
    },
    //开始拖拽事件
    onStart(event) {
      this.dragItem = {
        ...event.item._underlying_vm_,
      };
      let fromItems =
        event.from.parentNode.parentNode.parentNode.querySelectorAll(".item");
      // 用来判断是否移动
      const oldIndex = [...fromItems].findIndex(
        (v) => v.getAttribute("data-id") == this.dragItem.id
      );
      this.from = { ...event.from.dataset, oldIndex };

      const lists = document.querySelectorAll(".el-tooltip__popper");
      if (lists.length > 0) {
        lists.forEach((item) => {
          item.style.display = "none";
        });
      }
    },
    //拖拽结束事件
    onEnd(event) {
      this.changeStyle();

      // 工作站id code name 日期 拖拽数据id
      this.to = event.to.dataset;
      // 当前格子内所有item
      const date = this.to.date
        ? new Date(this.to.date).toLocaleDateString("sv-SE")
        : new Date(
            new Date().getTime() - 24 * 60 * 60 * 1000
          ).toLocaleDateString("sv-SE");
      const workStationCode = JSON.parse(this.to.workstationinfo).code;

      if (!this.isDragIn || date == "Invalid Date") {
        this.$modal.notifyError("禁止向历史任务中拖入");
        return this.getList();
      }

      let items =
        this.$refs[workStationCode + date][0]?.querySelectorAll(".item");
      const realityItems = [...items].filter(
        (v) => !v.classList.contains("v-leave")
      );
      // 获取盒子移动后的位置
      let newIndex = realityItems.findIndex(
        (item) => item.getAttribute("data-id") == this.dragItem.id
      );
      this.dataId = realityItems[newIndex - 1]
        ? realityItems[newIndex - 1].getAttribute("data-id")
        : 0;
      // 判断是否移动位置
      if (
        JSON.parse(this.to.workstationinfo).code ==
          JSON.parse(this.from.workstationinfo).code &&
        this.from.date == this.to.date &&
        this.from.oldIndex == newIndex
      ) {
        this.getList();
        this.$modal.msg("未发生实质性变化");
        return;
      }
      if (Cookies.get("checked") != "true") {
        this.$refs.dragDialog.dialogVisible = true;
      } else {
        this.$refs.dragDialog.dialogVisible = false;
        this.taskLoadingFinish();
      }
    },
    // loading走完后执行
    taskLoadingFinish() {
      if (this.selectList.length > 0) {
        this.changeList(this.selectList);
      } else {
        this.changeList([this.dragItem]);
      }
    },
    // 拖动改变数据库
    changeList(items) {
      let startTime, workStationId, workStationName, workStationCode;
      if (this.to) {
        const date = this.to.date
          ? new Date(this.to.date).toLocaleDateString("sv-SE")
          : new Date(
              new Date().getTime() - 24 * 60 * 60 * 1000
            ).toLocaleDateString("sv-SE");
        startTime = date + " " + "23:59";
        workStationId = JSON.parse(this.to.workstationinfo).id;
        workStationName = JSON.parse(this.to.workstationinfo).name;
        workStationCode = JSON.parse(this.to.workstationinfo).code;
      } else {
        startTime = this.targetDate + " " + "23:59";
        workStationId = this.workStationList.filter((v) => {
          return v.code == this.targetCode;
        })[0]?.id;
        workStationName = this.workStationList.filter((v) => {
          return v.code == this.targetCode;
        })[0]?.name;
        workStationCode = this.targetCode;
      }
      const paramsList = items.map((v) => {
        return {
          ...v,
          startTime,
          workStationId,
          workStationName,
          workStationCode,
          dataId: this.dataId,
        };
      });

      moveTask(paramsList).then((res1) => {
        if (res1.code == 0) {
          this.$nextTick(() => {
            const elements = document.querySelectorAll(".drag-item");
            elements.forEach((v) => {
              v.classList.contains("target-item")
                ? v.classList.remove("target-item")
                : "";
            });
            let message = res1.data == "" ? "操作成功,任务已顺排。" : res1.data;
            this.$modal.notifySuccess(message);
          });
        } else {
          this.$modal.notifyError(res1.message);
        }

        //关闭提示loading
        setTimeout(() => {
          this.getList();
          this.dataId = 0;
        }, 333);
      });
    },
    // 点击选择事件
    onDragChecked(val) {
      // 多选拖动
      if (val.checked) {
        this.selectList.push(val);
        // 数组套对象去重
        this.selectList = removeDuplicate(this.selectList);
      } else {
        //取消选中
        this.selectList = this.selectList.filter((v) => v.id != val.id);
      }
      if (!isAllEqual(this.selectList)) {
        this.$modal.notifyError("仅操作同机器同日期的任务,请重新选择!");

        const lastId = this.selectList[this.selectList.length - 1]?.id;
        this.selectList = this.selectList.filter((v) => v.id != lastId);
        setTimeout(() => {
          this.times = this.times.map((v) => {
            return {
              ...v,
              children: v.children.map((child) => {
                return {
                  ...child,
                  arr: child.arr.map((taskChild) => {
                    return {
                      ...taskChild,
                      child: taskChild.child.map((task) => {
                        if (task.id == lastId) {
                          return { ...task, checked: false };
                        }
                        return { ...task };
                      }),
                    };
                  }),
                };
              }),
            };
          });
        }, 333);
      }
    },
    // 鼠标右键事件
    // 关闭、
    closeMenu() {
      this.visible = false;
    },
    //鼠标右击item_box
    openMenu(arr, date, event) {
      const menuMinWidth = 120,
        menuMinHeight = 180;
      this.checkHistory = date == "history" ? true : false;
      // 宽度放不下生成新的位置
      if (event.clientX > window.innerWidth - menuMinWidth) {
        this.left = window.innerWidth - menuMinWidth;
      } else {
        this.left = event.clientX;
      }

      // 高度放不下生成新的位置
      if (event.clientY > window.innerHeight - menuMinHeight) {
        this.top = window.innerHeight - menuMinHeight;
      } else {
        this.top = event.clientY;
      }

      const arrList = [];
      if (arr.length <= 0) return (this.visible = false);
      if (arr.some((v) => this.statusList.includes(v.taskStatus))) {
        this.visible = true;
      } else {
        this.visible = false;
      }

      arr.forEach((v) => {
        if (this.statusList.includes(v.taskStatus)) {
          arrList.push(v);
        }
      });
      this.menuSelectArr = arrList;
    },
    // 右击菜单全部选中
    handleMenuSelect() {
      this.menuSelectArr.forEach((v) => {
        v.checked = true;
        const flag = this.selectList.some((s) => s.id == v.id);
        if (flag) return;
        this.selectList.push(v);
      });
      if (!isAllEqual(this.selectList, this.checkHistory)) {
        this.$modal.notifyError("仅操作同机器同日期的任务,请重新选择!");
        const lastStartTime = new Date(
          this.selectList[this.selectList.length - 1]?.startTime
        ).toLocaleDateString();
        const lastCode =
          this.selectList[this.selectList.length - 1]?.workStationCode;
        // 是否是同一天
        const isSameDate = this.selectList.every(
          (v) => new Date(v.startTime).toLocaleDateString() == lastStartTime
        );
        // 是否是同一机台
        const isSameCode = this.selectList.every(
          (v) => v.workStationCode == lastCode
        );
        if (!isSameCode) {
          this.selectList = this.selectList.filter(
            (v) => v.workStationCode != lastCode
          );
        }
        if (!isSameDate) {
          this.selectList = this.selectList.filter(
            (v) => new Date(v.startTime).toLocaleDateString() != lastStartTime
          );
        }
      }
      this.times = this.times.map((v) => {
        return {
          ...v,
          children: v.children.map((child) => {
            return {
              ...child,
              arr: child.arr.map((taskChild) => {
                return {
                  ...taskChild,
                  child: taskChild.child.map((task) => {
                    if (this.selectList.find((v) => v.id == task.id)) {
                      return { ...task, checked: true };
                    } else {
                      return { ...task, checked: false };
                    }
                  }),
                };
              }),
            };
          }),
        };
      });
    },
    // 右键取消全部
    handleUnCheckAll() {
      this.times = this.times.map((v) => {
        return {
          ...v,
          children: v.children.map((child) => {
            return {
              ...child,
              arr: child.arr.map((taskChild) => {
                return {
                  ...taskChild,
                  child: taskChild.child.map((task) => {
                    return { ...task, checked: false };
                  }),
                };
              }),
            };
          }),
        };
      });

      this.selectList = [];
    },
    // 点击批量转发
    handleTransferTask() {
      const ids = this.selectList.map((v) => v.id);
      if (ids.length <= 0) return this.$modal.msgWarning("请选择要操作的数据!");
      // 是否是草稿数据
      const falg = this.selectList.some((item) => item.taskStatus != 0);
      if (falg) return this.$modal.notifyError("当前操作仅草稿数据可执行!");
      verifyTaskBelongOneRouteAndProcess(
        this.selectList.map((v) => {
          return { ...v, checked: undefined, history: undefined };
        })
      ).then((res) => {
        if (res.code == 0) {
          this.$refs.TransferTask.showFlag = true;
          this.$refs.TransferTask.queryParams.routeCode = res.data.routeCode;
          this.$refs.TransferTask.queryParams.processCode =
            res.data.processCode;
          this.$refs.TransferTask.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 执行转发
    onTransferTaskSelected(row) {
      const form = {
        taskIds: this.ids,
        workStationId: row.id,
        workStationName: row.name,
        workStationCode: row.code,
      };
      transferTask(form).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("操作成功");
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 方向控制功能
    handleControl(val, num) {
      num = typeof num == "number" ? num : 1;
      const dateList = this.isHistory
        ? this.dateList.slice(1, this.dateList.length)
        : this.dateList;
      // 用于区分有无目标点 没有目标点默认向后排序
      this.dataId = 1;
      // 点击事件触发时，判断当前状态是拖拽还是点击，若是拖拽，直接返回不继续执行
      const isDrag = document
        .querySelector(".control-wrapper")
        .getAttribute("drag-flag");
      if (isDrag === "true") {
        return;
      }
      // 未选择提示
      if (this.selectList.length <= 0)
        return this.$modal.msgError("请选择要移动的任务");
      // 显示目标信息
      this.isControl = true;
      const workStationCode = this.selectList.map((v) => v.workStationCode)[0];
      const startDate = this.selectList[0].history
        ? new Date(this.queryParams.startDate)
        : new Date(this.selectList.map((v) => v.startTime)[0]);
      if (val == "top") {
        this.topCount++;
        this.targetCode = this.targetCode ? this.targetCode : workStationCode;
        this.targetRouteCode = this.targetRouteCode
          ? this.targetRouteCode
          : this.workStationList.find((v) => v.code == this.targetCode)
              ?.routeCode;
        if (this.targetDate) {
          this.targetDate = new Date(
            new Date(this.targetDate).getTime() - 24 * 60 * 60 * 1000
          ).toLocaleDateString("sv-SE");
        } else {
          this.targetDate = new Date(
            (startDate.getTime() - 24 * 60 * 60 * 1000) * this.topCount
          ).toLocaleDateString("sv-SE");
        }
        // 日期比较
        const tarDate = new Date(this.targetDate).getTime();
        const currDate = new Date(dateList[0]).getTime();
        if (tarDate < currDate) {
          this.targetDate = dateList[0];
          this.$modal.msgError("已达上限");
        }
      } else if (val == "bottom") {
        this.bottomCount++;
        this.targetCode = this.targetCode ? this.targetCode : workStationCode;
        this.targetRouteCode = this.targetRouteCode
          ? this.targetRouteCode
          : this.workStationList.find((v) => v.code == this.targetCode)
              ?.routeCode;
        if (this.targetDate) {
          this.targetDate = new Date(
            new Date(this.targetDate).getTime() + 24 * 60 * 60 * 1000
          ).toLocaleDateString("sv-SE");
        } else {
          this.targetDate = this.selectList[0].history
            ? new Date(
                startDate.getTime() * this.bottomCount
              ).toLocaleDateString("sv-SE")
            : new Date(
                (startDate.getTime() + 24 * 60 * 60 * 1000) * this.bottomCount
              ).toLocaleDateString("sv-SE");
        }

        // 日期比较
        const tarDate = new Date(this.targetDate).getTime();
        const lastDate = new Date(dateList[dateList.length - 1]).getTime();
        if (tarDate > lastDate) {
          this.targetDate = dateList[dateList.length - 1];
          this.$modal.msgError("已达上限");
        }
      }

      if (val == "left") {
        this.leftCount += num;
        this.targetDate = this.targetDate
          ? this.targetDate
          : new Date(startDate).toLocaleDateString("sv-SE");

        let index;
        if (this.targetCode) {
          index =
            this.workStationList.findIndex(
              (item) => item.code === this.targetCode
            ) - num;
        } else {
          index =
            this.workStationList.findIndex(
              (item) => item.code === workStationCode
            ) - this.leftCount;
        }
        if (this.workStationList[index]) {
          this.targetCode = this.workStationList[index].code;
          this.targetRouteCode = this.workStationList[index].routeCode;
        } else {
          this.targetCode = this.workStationList[0].code;
          this.targetRouteCode = this.workStationList[0].routeCode;
          this.$modal.msgError("已达上限");
        }
      } else if (val == "right") {
        this.rightCount += num;
        this.targetDate = this.targetDate
          ? this.targetDate
          : new Date(startDate).toLocaleDateString("sv-SE");
        let index;
        if (this.targetCode) {
          index =
            this.workStationList.findIndex(
              (item) => item.code === this.targetCode
            ) + num;
        } else {
          index =
            this.workStationList.findIndex(
              (item) => item.code === workStationCode
            ) + this.rightCount;
        }

        if (this.workStationList[index]) {
          this.targetCode = this.workStationList[index].code;
          this.targetRouteCode = this.workStationList[index].routeCode;
        } else {
          this.targetCode =
            this.workStationList[this.workStationList.length - 1].code;
          this.targetRouteCode =
            this.workStationList[this.workStationList.length - 1].routeCode;
          this.$modal.msgError("已达上限");
        }
      }
      // 方向操作时做出响应的高亮
      this.$nextTick(() => {
        const elements = document.querySelectorAll(".drag-item");
        const dom = this.$refs[this.targetCode + this.targetDate];

        elements.forEach((v) => {
          v.classList.contains("target-item")
            ? v.classList.remove("target-item")
            : "";
        });
        dom[0].classList.add("target-item");
        // 滚动
        scrollTo(dom[0].offsetTop, 800);
        // 判断元素有没有被遮盖
        let isVisible;
        new IntersectionObserver(
          ([change]) => {
            // 被覆盖就是false，反之true
            isVisible = change.isVisible;
          },
          {
            threshold: [1.0],
            delay: 1000,
            trackVisibility: true,
          }
        ).observe(dom[0]);
        // 移动
        if (val == "right" && !isVisible) {
          this.scrollRight(num);
        }
        if (val == "left" && !isVisible) {
          this.scrollLeft(num);
        }
      });
    },
    // 点击确定
    handleControlOk() {
      // 点击事件触发时，判断当前状态是拖拽还是点击，若是拖拽，直接返回不继续执行
      const isDrag = document
        .querySelector(".control-wrapper")
        .getAttribute("drag-flag");
      if (isDrag === "true") return;
      // 未选择提示
      if (this.selectList.length <= 0)
        return this.$modal.msgError("请选择要移动的任务");
      if (!this.targetCode || !this.targetDate)
        return this.$modal.msgError("请选择要移动的位置");
      this.isControl = false;
      // 是否弹出确认框
      if (Cookies.get("checked") != "true") {
        this.$refs.dragDialog.dialogVisible = true;
      } else {
        this.$refs.dragDialog.dialogVisible = false;
        this.taskLoadingFinish();
      }
    },
    // 初始化是否可以点击右侧
    init() {
      if (
        this.workStationList.length <= 0 ||
        !document.querySelector(".all-container")
      )
        return (this.btnRight = true), (this.btnLeft = true);
      this.allLength = (this.workStationList.length + 1) * 180;
      this.boxLength = document.querySelector(
        ".task-hidden-container"
      )?.offsetWidth;
      this.taskListEl = document.querySelector(".all-container");
      if (
        this.taskListEl &&
        Math.abs(parseInt(this.taskListEl.style.left)) > 0
      ) {
        if (
          Math.abs(parseInt(this.taskListEl.style.left)) +
            this.boxLength +
            180 >=
          this.allLength
        ) {
          this.taskListEl.style.left =
            "-" + (this.allLength - this.boxLength) + "px";
          this.btnRight = true;
          this.btnLeft = false;
        } else {
          this.btnRight = false;
        }
      } else {
        if (this.allLength < this.boxLength) {
          this.btnRight = true;
          this.btnLeft = true;
        } else {
          this.btnRight = false;
        }
      }
    },

    handleScroll() {
      //计算滚动条位置
      var scrollTop =
        window.pageYOffset ||
        document.documentElement.scrollTop ||
        document.body.scrollTop;
      //计算绑定div位置
      var offsetTop = document.querySelector("#menuFlag")?.offsetTop;
      //进行比较设置位置fixed
      this.whether = scrollTop > offsetTop;
      if (this.whether) {
        this.isShowBtn = true;
      } else {
        this.isShowBtn = false;
      }
    },
  },
  mounted() {
    window.addEventListener("scroll", this.handleScroll);
    window.addEventListener("resize", this.handleScroll);

    // 实时监听宽度变化 非窗口window变化
    const resizeObserver = new ResizeObserver((entries) => {
      this.init();
    });
    resizeObserver.observe(document.querySelector(".task-hidden-container"));
    const resizeObserver1 = new ResizeObserver((entries) => {
      const topRowHeight = this.$refs.topRow?.$el?.getBoundingClientRect()
        .height
        ? this.$refs.topRow?.$el?.getBoundingClientRect().height
        : 0;
      const queryFormHeight = this.$refs.queryForm?.$el?.getBoundingClientRect()
        .height
        ? this.$refs.queryForm?.$el?.getBoundingClientRect().height
        : 0;
      this.task_top_height = queryFormHeight + topRowHeight;
    });
    resizeObserver1.observe(this.$refs.queryForm.$el);
  },
};
</script>

<style lang="scss" scoped>
@import "./task.scss";
</style>
