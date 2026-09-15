<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
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
          @change="(e) => handleSelectMultiple(e, 'requestDeviceKindList')"
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
          @change="(e) => handleSelectMultiple(e, 'interactionSequenceList')"
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
          @change="(e) => handleSelectMultiple(e, 'routeCodeList')"
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
          @change="(e) => handleSelectMultiple(e, 'scheduledTaskStatusList')"
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
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['device:scheduleHistory:export'])"
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

    <el-table border :key="tableKey" :ref="page" :data="schedulementList">
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
        min-width="150px"
        key="interactionSequence"
        prop="interactionSequence"
        show-overflow-tooltip
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
            :disabled="hasPermi(['device:scheduleHistory:detail'])"
            >详情</el-button
          >
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

    <my-drawer
      @changeSetInterval="changecomponentsSetInterval"
      ref="MyDrawer"
      :detailId="detailId"
      isPage="scheduleHistory"
    />
  </div>
</template>

<script>
import drawer from "../components/drawer.vue";
import { getDropSelectDatas } from "@/api/produce/route";
import { listScheduleHistory } from "@/api/device/scheduleHistory";
export default {
  name: "ScheduleHistory",
  components: { MyDrawer: drawer },
  data() {
    return {
      tableKey: Math.random(),
      page: "scheduleHistory",
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
      queryParams: this.$cache.local.get("scheduleHistory_queryParams")
        ? JSON.parse(this.$cache.local.get("scheduleHistory_queryParams"))
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
              new Date().setMonth(new Date().getMonth() - 1)
            ).toLocaleString("sv-SE"),
            endTime: undefined,
            runningStartTime: undefined,
            runningEndTime: undefined,
            refresh: 10,
            checked: false,
            isBarcodeOk: undefined,
            queryOrderBy: 6,
            agvPayloadPanels: undefined,
          },
      // 工艺路线
      routeQueryList: [],
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
          "scheduleHistory_queryParams",
          JSON.stringify({
            ...val,
          })
        );
      },
      deepL: true,
      immediate: true,
    },
  },
  activated() {
    this.queryParams.startTime = this.queryParams.startTime
      ? this.queryParams.startTime
      : new Date(new Date().setMonth(new Date().getMonth() - 1)).toLocaleString(
          "sv-SE"
        );
    this.getList();
    this.getRouteList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  methods: {
    /** 查询设备列表 */
    async getList(isSearch) {
      if (!this.queryParams.startTime && !this.queryParams.endTime)
        return this.$modal.msgWarning("请填写呼叫时间的起始或结束再进行搜索!");
      this.queryParams.queryOrderBy = this.queryParams.queryOrderBy
        ? this.queryParams.queryOrderBy
        : 6;
      this.loading = true;
      listScheduleHistory(this.queryParams).then((res) => {
        // 有分配时间
        this.schedulementList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
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
          return { ...v, label: `${v.code}(${v.name})` };
        });
      });
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
        //一个月前的今天
        startTime: new Date(
          new Date().setMonth(new Date().getMonth() - 1)
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

    changecomponentsSetInterval() {},
    // 点击详情
    handleDetail(row) {
      this.detailId = row.id || this.ids;
      this.$refs.MyDrawer.getDetail(this.detailId);
    },
    // 设备类别、交互方式、状态、工艺路线多选限制只能选择一个
    handleSelectMultiple(val, key) {
      if (val.length > 0) {
        // 取用户最后选择的值
        const lastSelected = val[val.length - 1];
        this.queryParams[key] = [lastSelected];
      }
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/scheduleHistory/DownLoadList",
        "调度历史记录.xlsx",
        this.queryParams
      );
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
