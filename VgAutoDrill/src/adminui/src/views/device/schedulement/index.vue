<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="是否刷新">
        <el-switch v-model="queryParams.checked" />
      </el-form-item>
      <el-form-item label="频率" prop="refresh">
        <el-select
          v-model="queryParams.refresh"
          placeholder="请选择"
          @change="handleRefreshSelect"
          style="width: 120px"
        >
          <el-option :label="'3秒'" :value="3" />
          <el-option :label="'5秒'" :value="5" />
          <el-option :label="'10秒'" :value="10" />
        </el-select>
      </el-form-item>
      <el-form-item label="排序方式" prop="queryOrderBy">
        <el-select
          v-model="queryParams.queryOrderBy"
          placeholder="请选择"
          style="width: 150px"
        >
          <el-option label="记录Id正序" :value="5" />
          <el-option label="记录Id倒序" :value="6" />
          <el-option label="开始时间正序" :value="7" />
          <el-option label="开始时间倒序" :value="8" />
        </el-select>
      </el-form-item>
      <el-form-item label="记录Id" prop="id">
        <el-input
          v-trim
          v-model="queryParams.id"
          oninput="value=value.replace(/\D/g,'')"
          @clear="clearQueryParams('id')"
          placeholder="请输入内容纯数字"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="发起设备" prop="sourceDeviceId">
        <el-input
          v-trim
          v-model="queryParams.sourceDeviceId"
          placeholder="请输入发起设备"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="调度设备" prop="requireDeviceId">
        <el-input
          v-trim
          v-model="queryParams.requireDeviceId"
          placeholder="请输入调度设备"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="任务编码" prop="taskId">
        <el-input
          v-trim
          v-model="queryParams.taskId"
          placeholder="请输入任务编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="产品编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入产品编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="AGV料仓信息" prop="agvPayloadPanels">
        <el-input
          v-trim
          v-model="queryParams.agvPayloadPanels"
          placeholder="请输入AGV料仓信息"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="设备类别" prop="requestDeviceKindList">
        <el-select
          v-model="queryParams.requestDeviceKindList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.requestDeviceKindList &&
            queryParams.requestDeviceKindList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.deviceKinds.filter(
              (v) => !v.label.includes('AGV')
            )"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="交互方式" prop="interactionSequenceList">
        <el-select
          v-model="queryParams.interactionSequenceList"
          placeholder="请选择"
          multiple
          collapse-tags
        >
          <el-option
            v-for="item in $status.interactionSequenceOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="工艺路线" prop="routeCodeList">
        <el-select
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
          >
          </el-option>
        </el-select>
      </el-form-item>

      <el-form-item label="板料检验是否OK" prop="isBarcodeOk">
        <el-select
          v-model="queryParams.isBarcodeOk"
          placeholder="请选择"
          style="width: 150px"
          @clear="clearQueryParams('isBarcodeOk')"
          clearable
        >
          <el-option label="是" :value="true"> </el-option>
          <el-option label="否" :value="false"> </el-option>
        </el-select>
      </el-form-item>
      <!-- 多选状态 -->
      <el-form-item label="状态" prop="scheduledTaskStatusList">
        <el-select
          v-model="queryParams.scheduledTaskStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.scheduledTaskStatusList &&
            queryParams.scheduledTaskStatusList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.schedulementOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <!-- 单选状态 -->
      <!-- <el-form-item label="状态" prop="scheduledTaskStatus">
        <el-select
          v-model="queryParams.scheduledTaskStatus"
          placeholder="请选择"
          @change="handleSelectStatus"
          style="width: 150px"
        >
          <el-option
            v-for="item in $status.schedulementOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item> -->

      <el-form-item label="开始时间">
        <el-date-picker
          v-model="queryParams.runningStartTime"
          type="datetime"
          placeholder="起始时间"
        >
        </el-date-picker>
        ↔
        <el-date-picker
          v-model="queryParams.runningEndTime"
          type="datetime"
          placeholder="结束时间"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="呼叫时间">
        <el-date-picker
          v-model="queryParams.startTime"
          type="datetime"
          placeholder="起始时间"
          :picker-options="pickerCreateStart"
        >
        </el-date-picker>
        ↔
        <el-date-picker
          :picker-options="pickerCreateEnd"
          v-model="queryParams.endTime"
          type="datetime"
          placeholder="结束时间"
        >
        </el-date-picker>
      </el-form-item>
    </search-form>
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-close"
          @click="handleBatchCancel"
          :disabled="hasPermi(['device:schedulement:cancel'])"
          >批量取消</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['device:schedulement:export'])"
          >导出</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
        :columns="columns"
        :page="page"
      ></right-toolbar>
    </el-row>

    <el-table
      border
      :key="tableKey"
      :ref="page"
      :data="schedulementList"
      @select-all="handleSelectAll"
      @selection-change="handleSelectionChange"
      @select="handleSelect"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
      @sort-change="sortChange"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="记录Id"
        align="center"
        key="id"
        prop="id"
        show-overflow-tooltip
        v-if="columns[0].visible"
        fixed="left"
      />
      <el-table-column
        min-width="180px"
        label="发起设备"
        key="sourceDeviceId"
        prop="sourceDeviceId"
        show-overflow-tooltip
        fixed="left"
        v-if="columns[1].visible"
      >
        <template slot-scope="scope">
          <span
            >{{ scope.row.subDeviceCode
            }}{{
              scope.row.positionCodes
                ? "（" + scope.row.positionCodes + "）"
                : ""
            }}</span
          >
        </template>
      </el-table-column>

      <el-table-column
        min-width="120px"
        label="调度设备"
        key="requireDeviceId"
        prop="requireDeviceId"
        fixed="left"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="交互方式"
        min-width="200px"
        key="interactionSequence"
        prop="interactionSequence"
        v-if="columns[3].visible"
        fixed="left"
        align="center"
      >
        <template slot-scope="scope">
          <span :style="{ color: scope.row.isUrgent > 0 ? 'red' : '' }">
            {{ scope.row.isUrgent > 0 ? "紧急 ---" : "" }}
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
          </span>
        </template></el-table-column
      >

      <el-table-column
        label="调度状态"
        align="center"
        key="scheduledTaskStatus"
        prop="scheduledTaskStatus"
        v-if="columns[4].visible"
        fixed="left"
        min-width="120px"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.schedulementOptions"
            :status="scope.row.scheduledTaskStatus"
          />
        </template>
      </el-table-column>

      <el-table-column
        label="任务编码"
        key="taskId"
        prop="taskId"
        min-width="150px"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />

      <el-table-column
        label="产品编码"
        key="itemCode"
        prop="itemCode"
        min-width="180px"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <tooltip
            v-if="
              scope.row.requestDeviceKind != 2 &&
              scope.row.requestDeviceKind != 3
            "
            :value="scope.row.requestSummaryInfo"
          />
          <tooltip v-else :value="scope.row.itemCode" />
        </template>
      </el-table-column>
      <el-table-column
        label="AGV料仓信息"
        key="agvPayloadPanels"
        prop="agvPayloadPanels"
        min-width="180px"
        show-overflow-tooltip
        v-if="columns[7].visible"
      >
        <template slot-scope="scope">
          {{ scope.row.agvPayloadPanels }}
          <!-- <tooltip :value="scope.row.requestSummaryInfo" /> -->
        </template>
      </el-table-column>
      <el-table-column
        label="工艺路线"
        key="routeCode"
        prop="routeCode"
        min-width="100px"
        show-overflow-tooltip
        v-if="columns[8].visible"
      />
      <!-- 接口暂无提供 -->
      <!-- <el-table-column
        label="班次"
        min-width="150"
        align="center"
        key="sailings"
        prop="sailings"
        show-overflow-tooltip
        v-if="columns[8].visible"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.sailings == 1"
            >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-中班</span
          >
          <span v-if="scope.row.sailings == 2"
            >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-晚班</span
          >
          <span v-else-if="scope.row.sailings === 0"
            >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-白班</span
          >
        </template>
      </el-table-column> -->

      <el-table-column
        label="需求备注"
        key="remark"
        prop="remark"
        min-width="180px"
        v-if="columns[9].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
        </template>
      </el-table-column>
      <el-table-column
        label="呼叫时间"
        align="center"
        key="createTime"
        prop="createTime"
        min-width="180"
        v-if="columns[10].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.createTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="分配时间"
        align="center"
        key="allocateTime"
        prop="allocateTime"
        min-width="180"
        v-if="columns[11].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.allocateTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="等待时长(分钟)"
        key="waitTime"
        prop="waitTime"
        show-overflow-tooltip
        v-if="columns[12].visible"
        align="center"
        min-width="120"
      >
        <template slot-scope="scope">
          <!--  等待时长 逻辑变更 -->
          <!-- 1.---存在任意时间时（分配、开始、异常、取消、完成），取最小值，最小值 -- 呼叫时间 -->
          <!-- 2.---都不存在任意时间时（分配、开始、异常、取消、完成），系统时间 --呼叫时间  -->
          <span style="color: red" v-if="getWaitTime(scope.row) > 0">{{
            getWaitTime(scope.row)
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="开始时间"
        align="center"
        key="runningTime"
        prop="runningTime"
        min-width="180"
        v-if="columns[13].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.runningTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="完成时间"
        align="center"
        key="completedTime"
        prop="completedTime"
        min-width="180"
        v-if="columns[14].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.completedTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="上下料时长(分钟)"
        key="materialTime"
        prop="materialTime"
        show-overflow-tooltip
        v-if="columns[15].visible"
        align="center"
        min-width="150"
      >
        <template slot-scope="scope">
          <!-- 2. 上下料时长 逻辑变更；
开始时间不存在时，不要显示值
开始时间存在，并且 不存在（取消、异常、完成）时间， 用系统时间 -- 开始时间；
开始时间存在，并且 存在任意一个（取消、异常、完成）时间， 用最大的那个值的时间 -- 开始时间； -->
          <span style="color: red" v-if="getMaterialTime(scope.row) > 0">{{
            getMaterialTime(scope.row)
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="失败时间"
        align="center"
        key="failedTime"
        prop="failedTime"
        min-width="180"
        v-if="columns[16].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.failedTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="取消时间"
        align="center"
        key="canceledTime"
        prop="canceledTime"
        min-width="180"
        v-if="columns[17].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.canceledTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="取消原因"
        key="cancelReason"
        prop="cancelReason"
        min-width="180px"
        v-if="columns[18].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.cancelReason" />
        </template>
      </el-table-column>

      <el-table-column
        label="板料检验是否OK"
        key="isBarcodeOk"
        align="center"
        min-width="150px"
        prop="isBarcodeOk"
        v-if="columns[19].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isBarcodeOk">是</el-tag>
          <el-tag type="danger" v-else-if="scope.row.isBarcodeOk == 0"
            >否</el-tag
          >
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-document"
            @click="handleDetail(scope.row)"
            :disabled="hasPermi(['device:schedulement:detail'])"
            >详情</el-button
          >

          <el-dropdown
            v-if="scope.row.scheduledTaskStatus != -2"
            trigger="click"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                v-if="
                  scope.row.scheduledTaskStatus == 1 ||
                  scope.row.scheduledTaskStatus == 5
                "
                command="handleSetUrgent"
                icon="el-icon-setting"
                :disabled="
                  hasPermi(['device:schedulement:setUrgent']) ||
                  scope.row.isUrgent == 1
                "
                >设置紧急
              </el-dropdown-item>
              <el-dropdown-item
                v-if="
                  scope.row.scheduledTaskStatus == 4 &&
                  scope.row.taskId != undefined &&
                  scope.row.taskId != ''
                "
                command="handleStart"
                icon="el-icon-video-play"
                v-hasRole="['admin']"
                >开始
              </el-dropdown-item>
              <el-dropdown-item
                icon="el-icon-close"
                v-if="
                  scope.row.scheduledTaskStatus == 1 ||
                  scope.row.scheduledTaskStatus == 5 ||
                  scope.row.scheduledTaskStatus == 3 ||
                  scope.row.scheduledTaskStatus == 2
                "
                command="handleCancel"
                :disabled="hasPermi(['device:schedulement:cancel'])"
                >取消
              </el-dropdown-item>
              <el-dropdown-item
                icon="el-icon-warning-outline"
                v-if="scope.row.scheduledTaskStatus == -1"
                command="handleAbnormal"
                :disabled="hasPermi(['device:schedulement:abnormal'])"
                >异常处理
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>
    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />
    <!-- 异常处理 -->
    <abnormal
      ref="abnormal"
      @changeSetInterval="changecomponentsSetInterval"
      :id="abnormalId"
      :sourceDeviceId="sourceDeviceId"
      :requireDeviceId="requireDeviceId"
    />
    <my-drawer
      @changeSetInterval="changecomponentsSetInterval"
      ref="MyDrawer"
      :detailId="detailId"
      isPage="schedulement"
    />
  </div>
</template>

<script>
import {
  listSchedulement,
  getSchedulement,
  delSchedulement,
  addSchedulement,
  updateSchedulement,
  updateCentralTask,
  bulkCanceled,
  relaunch,
  setUrgent,
  downLoadList,
} from "@/api/device/schedulement";
import { updateByOutSide } from "@/api/produce/task";
import abnormal from "./abnormal.vue";
import drawer from "../components/drawer.vue";
import { getDropSelectDatas } from "@/api/produce/route";
export default {
  name: "Schedulement",
  components: { MyDrawer: drawer, abnormal },
  data() {
    return {
      tableKey: Math.random(),
      page: "schedulement",
      initWidth: 0,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 设备表格数据
      schedulementList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: this.$cache.local.get("schedule_queryParams")
        ? JSON.parse(this.$cache.local.get("schedule_queryParams"))
        : {
            pageNum: 1,
            pageSize: 10,
            sourceDeviceId: undefined,
            requireDeviceId: undefined,
            id: undefined,
            taskId: undefined,
            itemCode: undefined,
            scheduledTaskStatusList: [],
            requestDeviceKindList: [],
            routeCodeList: [],
            interactionSequenceList: [],
            startTime: new Date(
              new Date().setDate(new Date().getDate() - 1)
            ).toLocaleString("sv-SE"),
            endTime: undefined,
            runningStartTime: undefined,
            runningEndTime: undefined,
            refresh: 10,
            checked: false,
            isBarcodeOk: undefined,
            agvPayloadPanels: undefined,
            queryOrderBy: 6,
          },
      // 工艺路线
      routeQueryList: [],
      rememberChecked: null,
      // 表单参数
      form: {},
      // 表单校验
      rules: {
        name: [
          { required: true, message: "设备名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "设备顺序不能为空", trigger: "blur" },
        ],
      },
      // 定时刷新数据
      timer: null,
      detailId: null,
      // 列信息
      columns: [
        { key: 0, label: "记录Id", visible: true },
        { key: 1, label: "发起设备", visible: true },
        { key: 2, label: "调度设备", visible: true },
        { key: 3, label: "交互方式", visible: true },
        { key: 4, label: "调度状态", visible: true },
        { key: 5, label: "任务编码", visible: true },
        { key: 6, label: "产品编码", visible: true },
        { key: 7, label: "AGV料仓信息", visible: true },
        { key: 8, label: "工艺路线", visible: true },
        { key: 9, label: "需求备注", visible: true },
        { key: 10, label: "呼叫时间", visible: true },
        { key: 11, label: "分配时间", visible: true },
        { key: 12, label: "等待时长(分钟)", visible: true },
        { key: 13, label: "开始时间", visible: true },
        { key: 14, label: "完成时间", visible: true },
        { key: 15, label: "上下料时长(分钟)", visible: true },
        { key: 16, label: "失败时间", visible: true },
        { key: 17, label: "取消时间", visible: true },
        { key: 18, label: "取消原因", visible: true },
        { key: 19, label: "板料检验是否OK", visible: true },
      ],
      // 异常处理
      abnormalId: null,
      requireDeviceId: null,
      sourceDeviceId: null,

      //.限制结束时间必须大于等于开始时间
      pickerCreateEnd: {
        disabledDate: (time) => {
          if (this.queryParams.startTime) {
            return (
              time.getTime() <=
              new Date(this.queryParams.startTime).getTime() - 86400000
            );
          }
        },
      },
      // 限制开始日期必须小于结束时间或当前日期
      pickerCreateStart: {
        disabledDate: (time) => {
          if (this.queryParams.endTime) {
            return (
              time.getTime() > Date.now() ||
              time.getTime() >
                new Date(this.queryParams.endTime).getTime() -
                  8.64e6 /*开始日期要在选择的结束日期之前 若结束日期大于当前日期 则开始日期为小于当前日期*/
            );
          }
        },
      },
    };
  },
  computed: {
    // 拿出要监听的属性
    listenChange() {
      const { refresh, checked } = this.queryParams;
      return { refresh, checked };
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
    queryParamsChange() {
      return { ...this.queryParams };
    },
  },
  watch: {
    queryParamsChange: {
      handler(val) {
        this.$cache.local.set(
          "schedule_queryParams",
          JSON.stringify({ ...val })
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
          if (this.timer) {
            clearInterval(this.timer);
            this.timer = null;
          } else {
            this.changeSetInterval(refresh);
          }
        } else {
          clearInterval(this.timer);
          this.timer = null;
        }
      },
      immediate: true,
      deep: true,
    },
  },
  activated() {
    this.getList();
    this.getRouteList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  // keep-alive 特有钩子函数 关闭定时器
  deactivated() {
    clearInterval(this.timer);
    this.timer = null;
    this.queryParams.checked = false;
  },
  methods: {
    // 定时器
    changeSetInterval(refresh) {
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.input_trim();
          this.handleQuery(); //调用接口的方法
        }, 0);
      }, refresh * 1000);
    },
    // 去空格
    input_trim() {
      this.queryParams.sourceDeviceId = this.queryParams.sourceDeviceId?.trim();
      this.queryParams.requireDeviceId =
        this.queryParams.requireDeviceId?.trim();
      this.queryParams.taskId = this.queryParams.taskId?.trim();
      this.queryParams.itemCode = this.queryParams.itemCode?.trim();
    },

    /** 查询设备列表 */
    async getList(isSearch) {
      this.loading = true;
      this.queryParams.queryOrderBy = this.queryParams.queryOrderBy
        ? this.queryParams.queryOrderBy
        : 6;
      listSchedulement(this.queryParams)
        .then((res) => {
          // 有分配时间
          this.schedulementList = res.data.list;
          this.total = res.data.total;
          this.loading = false;
          // 只有搜索状态下进行提示
          if (isSearch == "search") {
            this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
          }
        })
        .catch((error) => {
          // this.queryParams.checked = false;
          // clearInterval(this.timer);
          // this.timer = null;
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
          return { ...v, label: `${v.code}(${v.name})` };
        });
      });
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      if (this.timer) {
        this.$modal.msgWarning("警告,定时刷新已关闭,稍后请手动开启!");
      }
      // 点击选中时先取消定时刷新
      this.queryParams.checked = false;
      clearInterval(this.timer);
      this.timer = null;

      this.handleDoubleClick(row, this, column);
    },
    // 勾选表格checkbox清除定时器
    handleSelect(selection, row) {
      if (this.timer) {
        this.$modal.msgWarning("警告,定时刷新已关闭,稍后请手动开启!");
      }
      // 点击选中时先取消定时刷新
      this.queryParams.checked = false;
      clearInterval(this.timer);
      this.timer = null;
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.multipleSelection = selection;
      this.ids = selection.map((item) => item.id);
    },
    // 等待时长排序
    sortChange(info) {
      // ascending升序
      // descending降序
      // this.queryParams.order = info.order;
      // this.queryParams.pageNum++;
      // this.getList("search");

      console.log("接口暂时未支持等待时长排序");
    },
    // 监听全选表格事件
    handleSelectAll() {
      if (this.timer) {
        this.$modal.msgWarning("警告,定时刷新已关闭,稍后请手动开启!");
      }
      // 点击选中时先取消定时刷新
      this.queryParams.checked = false;
      clearInterval(this.timer);
      this.timer = null;
    },

    // 表单重置
    reset() {
      this.form = {
        id: undefined,
        name: undefined,
        code: undefined,
      };
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.queryParams = {
        pageNum: 1,
        pageSize: 10,
        sourceDeviceId: undefined,
        requireDeviceId: undefined,
        id: undefined,
        taskId: undefined,
        itemCode: undefined,
        scheduledTaskStatusList: [],
        requestDeviceKindList: [],
        routeCodeList: [],
        interactionSequenceList: [],
        startTime: new Date(
          new Date().setDate(new Date().getDate() - 1)
        ).toLocaleString("sv-SE"),
        endTime: undefined,
        runningStartTime: undefined,
        runningEndTime: undefined,
        refresh: 10,
        checked: false,
        isBarcodeOk: undefined,
        agvPayloadPanels: undefined,
        queryOrderBy: 6,
      };
      this.handleQuery();
    },
    changecomponentsSetInterval() {
      this.queryParams.checked = this.rememberChecked;
    },
    /** 修改按钮操作 */
    async handleUpdate(row) {
      this.reset();
      const deviceId = row.id || this.ids;
      const res = await getSchedulement(deviceId);
      this.form = res.data;
      this.open = true;
      this.title = "修改调度记录";
    },
    // 点击详情
    handleDetail(row) {
      this.rememberChecked = this.queryParams.checked;
      this.detailId = row.id || this.ids;
      this.$refs.MyDrawer.getDetail(this.detailId);
      if (this.queryParams.checked) {
        this.queryParams.checked = false;
      }
      clearInterval(this.timer);
      this.timer = null;
    },
    // 设置紧急
    handleSetUrgent(row) {
      const id = row.id || this.ids;
      this.$modal
        .confirm('是否确认将记录Id为"' + id + '"的数据设置紧急？')
        .then((result) => {
          if (result == "confirm") {
            setUrgent(id).then((res1) => {
              if (res1.code == 0) {
                this.$modal.msgSuccess("设置成功");
                this.getList();
              } else {
                this.$modal.notifyError(res1.message);
              }
            });
          }
        })
        .catch(() => {});
    },
    // 点击取消
    async handleCancel(row) {
      const id = row.id || this.ids;
      getSchedulement(id).then((res) => {
        if (res.code == 0) {
          this.form = { ...res.data, scheduledTaskStatus: -2 };
          this.$modal
            .confirm(
              `确定取消ID为<span style="color:red"> ${id}</span> 的数据项？`,
              {
                dangerouslyUseHTMLString: true, // 使用HTML片段
              }
            )
            .then((result) => {
              if (result == "confirm") {
                updateCentralTask(this.form).then((res1) => {
                  if (res1.code == 0) {
                    this.$modal.msgSuccess("取消成功");
                    this.getList();
                  } else {
                    this.$modal.notifyError(res1.message);
                  }
                });
              }
            })
            .catch(() => {});
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击开始
    async handleStart(row) {
      this.queryParams.checked = false;
      const results = await this.$modal.confirm("确认执行开始？").catch(() => {
        this.queryParams.checked = true;
      });
      if (results == "confirm") {
        updateByOutSide({
          code: row.taskId,
          taskStatus: 40,
        }).then((res) => {
          if (res.code == 0) {
            this.getList();
            this.$modal.msgSuccess("操作成功");
          } else {
            this.$modal.notifyError(res.message);
          }
          this.queryParams.checked = true;
        });
      }
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/Schedule/DownLoadList",
        "调度记录.xlsx",
        this.queryParams
      );
    },
    // 批量取消
    async handleBatchCancel() {
      if (this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行取消？")
        .catch(() => {});
      if (results == "confirm") {
        bulkCanceled(this.ids).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("取消成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleSetUrgent":
          this.handleSetUrgent(row);
          break;
        case "handleAbnormal":
          this.handleAbnormal(row);
          break;
        case "handleStart":
          this.handleStart(row);
          break;
        case "handleCancel":
          this.handleCancel(row);
          break;
        default:
          break;
      }
    },
    //异常处理
    handleAbnormal(row) {
      this.abnormalId = row.id;
      this.sourceDeviceId = row.sourceDeviceId;
      this.requireDeviceId = row.requireDeviceId;
      this.rememberChecked = this.queryParams.checked;
      if (this.queryParams.checked) {
        this.queryParams.checked = false;
      }
      clearInterval(this.timer);
      this.timer = null;
      this.$refs.abnormal.sourceDeviceKind = row.requestDeviceKind;
      this.$refs.abnormal.changeSynchronousData(
        row.sourceDeviceId,
        row.requireDeviceId
      );
      this.$refs.abnormal.getList(row.sourceDeviceId, row.requireDeviceId);
      this.$refs.abnormal.open = true;
    },
    // 点击重新发起
    handleRelaunch(row) {
      // 先获取设备
      getSchedulement(row.id).then((res) => {
        if (res.code == 0) {
          relaunch({
            code: res.data.code,
            needRepublish: "1",
          })
            .then((res1) => {
              const { message } = JSON.parse(res1.message);
              const msg = message ? message : "找不到设备" + row.code;
              if (res1.code == 0) {
                this.$modal.msgSuccess("操作成功");
                this.getList();
              } else {
                this.$modal.notifyError(msg);
              }
            })
            .catch((err) => {
              this.$modal.notifyError("服务器未启动");
            });
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },

    //删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delSchedulement, this.getList);
      } else {
        // this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 切换刷新频率
    handleRefreshSelect(val) {
      clearInterval(this.timer);
      this.timer = null;
    },
  },
};
</script>

<style lang="scss" scoped>
// 时间日期组件
::v-deep .el-date-editor {
  width: 180px !important;
  .el-input__inner {
    width: 180px !important;
    padding: 0 25px 0 25px !important;
  }
}
</style>
