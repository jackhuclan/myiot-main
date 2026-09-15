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
          @change="handleQueryOrderBy"
          v-model="queryParams.queryOrderBy"
          placeholder="请选择"
          style="width: 150px"
        >
          <el-option
            v-for="item in $status.deviceRecords_QueryOrderByOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="设备编码" prop="deviceCode">
        <el-input
          v-trim
          v-model="queryParams.deviceCode"
          placeholder="请输入设备编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工艺路线" prop="routeCodeList">
        <el-select
          v-model="queryParams.routeCodeList"
          placeholder="请选择"
          multiple
          @change="handleSelectRouteCodeList"
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
      <el-form-item label="开始日期">
        <el-date-picker
          v-model="queryParams.queryStartTime"
          type="date"
          placeholder="开始日期"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="结束日期">
        <el-date-picker
          v-model="queryParams.queryEndTime"
          type="date"
          placeholder="结束日期"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="班次" prop="sailings">
        <el-select
          v-model="queryParams.sailings"
          placeholder="请选择"
          style="width: 150px"
          @clear="clearQueryParams('sailings')"
          clearable
        >
          <el-option label="白班" :value="0" />
          <!-- <el-option label="中班" :value="1" /> -->
          <el-option label="晚班" :value="2" />
        </el-select>
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
          :disabled="hasPermi(['device:deviceRecords:export'])"
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
    <div style="margin-bottom: 10px; color: #515a6e">
      <i class="el-icon-info" style="margin-right: 5px"></i
      >表格所有时长列单位均为：分钟
    </div>
    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="deviceList"
      @selection-change="handleSelectionChange"
      @sort-change="sortChange"
      class="tableHei"
    >
      <el-table-column
        label="设备编码"
        key="deviceCode"
        prop="deviceCode"
        fixed="left"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.deviceCode"
            v-isGetSelection="['device:deviceRecords:view']"
            >{{ scope.row.deviceCode }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="工艺路线"
        min-width="100"
        align="center"
        fixed="left"
        key="routeCode"
        prop="routeCode"
        show-overflow-tooltip
        v-if="columns[1].visible"
      >
      </el-table-column>
      <el-table-column
        label="班次"
        min-width="150"
        align="center"
        fixed="left"
        key="sailings"
        prop="sailings"
        show-overflow-tooltip
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <!-- <span v-if="scope.row.sailings == 1"
            >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-中班</span
          > -->
          <span v-if="scope.row.sailings == 2"
            >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-晚班</span
          >
          <span v-else-if="scope.row.sailings === 0"
            >{{ parseTime(scope.row.createTime, "{y}-{m}-{d} ") }}-白班</span
          >
        </template>
      </el-table-column>

      <el-table-column
        label="实际稼动率(%)"
        min-width="120"
        align="center"
        fixed="left"
        key="duty"
        prop="duty"
        show-overflow-tooltip
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <span :style="{ color: getDutyColor(scope.row.duty) }">{{
            scope.row.duty
          }}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column
        label="理论稼动率(%)"
        min-width="120"
        align="center"
        key="theoryDuty"
        prop="theoryDuty"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="稼动率达成率(%)"
        min-width="120"
        align="center"
        key="dutyRate"
        prop="dutyRate"
        v-if="columns[5].visible"
      >
      </el-table-column> -->
      <el-table-column
        label="开机时长"
        align="center"
        key="openTime"
        prop="openTime"
        v-if="columns[4].visible"
      >
      </el-table-column>

      <el-table-column
        label="工作时长"
        align="center"
        key="workTime"
        prop="workTime"
        v-if="columns[5].visible"
      >
      </el-table-column>
      <el-table-column
        label="等待时长"
        align="center"
        sortable="custom"
        min-width="120"
        key="waitTime"
        prop="waitTime"
        v-if="columns[6].visible"
      >
      </el-table-column>
      <el-table-column
        label="设备异常时长"
        min-width="150"
        align="center"
        key="errorTime"
        prop="errorTime"
        v-if="columns[7].visible"
      >
        <template slot-scope="scope">
          <span style="color: red">{{ scope.row.errorTime }}</span>
        </template></el-table-column
      >

      <el-table-column
        label="未排产时长"
        align="center"
        min-width="100"
        key="withoutTaskTime"
        prop="withoutTaskTime"
        v-if="columns[8].visible"
      >
      </el-table-column>
      <el-table-column
        label="未设置自动化时长"
        min-width="150"
        align="center"
        key="deviceDisableTime"
        prop="deviceDisableTime"
        v-if="columns[9].visible"
      >
      </el-table-column>
      <el-table-column
        label="手动时长"
        align="center"
        key="bufferManualTime"
        prop="bufferManualTime"
        v-if="columns[10].visible"
      >
        <template slot-scope="scope">
          <span style="color: red">{{ scope.row.bufferManualTime }}</span>
        </template>
      </el-table-column>
      <!-- 刀具异常时长 -->
      <el-table-column
        label="刀检时长"
        align="center"
        min-width="120"
        key="drillToolEvaluationTime"
        prop="drillToolEvaluationTime"
        v-if="columns[11].visible"
      >
        <template slot-scope="scope">
          <span style="color: red">{{
            scope.row.drillToolEvaluationTime
          }}</span>
        </template>
      </el-table-column>

      <el-table-column
        label="刀具寿命时长"
        align="center"
        min-width="150"
        key="toolLifeExporedTime"
        prop="toolLifeExporedTime"
        v-if="columns[12].visible"
      >
        <template slot-scope="scope">
          <span style="color: red">{{ scope.row.toolLifeExporedTime }}</span>
        </template>
      </el-table-column>

      <el-table-column
        label="刀具寿命报警次数"
        min-width="150"
        align="center"
        key="toolLifeExporedCount"
        prop="toolLifeExporedCount"
        v-if="columns[13].visible"
      >
      </el-table-column>
      <el-table-column
        label="清洗夹头总时长"
        min-width="140"
        align="center"
        key="collectClearTime"
        prop="collectClearTime"
        v-if="columns[14].visible"
      >
      </el-table-column>
      <el-table-column
        label="无钻带参数时长"
        align="center"
        min-width="140"
        key="withoutDrillFileTime"
        prop="withoutDrillFileTime"
        v-if="columns[15].visible"
      >
        <template slot-scope="scope">
          <span style="color: red">{{ scope.row.withoutDrillFileTime }}</span>
        </template>
      </el-table-column>

      <!-- 原label:上趟打板结束到下趟打板开始间隔时长(结束到开始总时间);因不是实时数据,暂时先隐藏该字段-->
      <el-table-column
        label="结束到开始总时长"
        align="center"
        min-width="150"
        key="endToStartTime"
        prop="endToStartTime"
        v-if="columns[16].visible"
      >
      </el-table-column>
      <el-table-column
        label="无生产板料时长"
        align="center"
        min-width="140"
        key="withoutPanelTime"
        prop="withoutPanelTime"
        v-if="columns[17].visible"
      >
      </el-table-column>

      <el-table-column
        label="设备异常告警时长"
        align="center"
        min-width="150"
        key="alarmTime"
        prop="alarmTime"
        v-if="columns[18].visible"
      >
      </el-table-column>
      <el-table-column
        label="设备运行时长"
        min-width="120"
        align="center"
        key="runTime"
        prop="runTime"
        v-if="columns[19].visible"
      >
      </el-table-column>
      <el-table-column
        label="无生料时长"
        min-width="120"
        align="center"
        key="bufferNoBoardTime"
        prop="bufferNoBoardTime"
        v-if="columns[20].visible"
      >
      </el-table-column>
      <el-table-column
        label="有熟料时长"
        min-width="120"
        align="center"
        key="bufferClinkerExistTime"
        prop="bufferClinkerExistTime"
        v-if="columns[21].visible"
      >
      </el-table-column>

      <el-table-column
        label="有板到开始打板时长"
        min-width="150"
        align="center"
        key="drillRawExistToRunTime"
        prop="drillRawExistToRunTime"
        v-if="columns[22].visible"
      >
      </el-table-column>

      <el-table-column
        label="板方向检测时长"
        min-width="120"
        align="center"
        key="drillBoardDirectionTime"
        prop="drillBoardDirectionTime"
        v-if="columns[23].visible"
      >
      </el-table-column>
      <el-table-column
        label="pin检测时长"
        min-width="120"
        align="center"
        key="drillTestPinTime"
        prop="drillTestPinTime"
        v-if="columns[24].visible"
      >
      </el-table-column>

      <el-table-column
        label="数据日期"
        align="center"
        key="dateString"
        min-width="180"
        v-if="columns[25].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.dateString, "{y}-{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="中控记录时间"
        align="center"
        key="modifyTime"
        min-width="180"
        v-if="columns[26].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.modifyTime, "{y}-{m}-{d} {h}:{i}:{s}")
          }}</span>
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
    <summaryForm ref="summaryForm" />
  </div>
</template>

<script>
import { getSummaryList, formatDateToYMD } from "@/api/device/deviceRecords";
import { getDropSelectDatas } from "@/api/produce/route";
import summaryForm from "./summary.vue";

export default {
  name: "Standard",
  components: { summaryForm },
  data() {
    return {
      page: "standard",
      optType: "",
      // 遮罩层
      loading: false,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 产品表格数据
      deviceList: [],
      routeQueryList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceCode: undefined,
        // 默认当前日期前一天
        queryStartTime: this.getNextDate(new Date(), -1),
        queryEndTime: undefined,
        // 数据日期倒序
        queryOrderBy: 2,
        routeCodeList: [],
        sailings: undefined,
        // 等待时长排序
        order: undefined,
      },
      // 表单参数
      form: {},
      // 表单校验
      initialForm: {},
      rules: {
        name: [
          { required: true, message: "产品名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "产品顺序不能为空", trigger: "blur" },
        ],
        isCheckOk: [{ required: true }],
      },
      // 列信息
      columns: [
        { key: 0, label: "设备编码", visible: true },
        { key: 1, label: "工艺路线", visible: true },
        { key: 2, label: "班次", visible: true },
        { key: 3, label: "实际稼动率", visible: true },
        // { key: 4, label: "理论稼动率", visible: true },
        // { key: 5, label: "稼动率达成率", visible: true },
        { key: 4, label: "开机时长", visible: true },
        { key: 5, label: "工作时长", visible: true },
        { key: 6, label: "等待时长", visible: true },
        { key: 7, label: "设备异常时长", visible: true },
        { key: 8, label: "未排产时长", visible: true },
        { key: 9, label: "未设置自动化时长", visible: true },
        { key: 10, label: "手动时长", visible: true },
        { key: 11, label: "刀检时长", visible: true },
        { key: 12, label: "刀具寿命时长", visible: true },
        { key: 13, label: "刀具寿命报警次数", visible: true },
        { key: 14, label: "清洗夹头总时长", visible: true },
        { key: 15, label: "无钻带参数时长", visible: true },
        { key: 16, label: "结束到开始总时长", visible: true },
        { key: 17, label: "无生产板料时长", visible: true },
        { key: 18, label: "设备异常告警时长", visible: true },
        { key: 19, label: "设备运行时长", visible: true },
        { key: 20, label: "无生料时长", visible: true },
        { key: 21, label: "有熟料时长", visible: true },
        { key: 22, label: "有板到开始打板时长", visible: true },
        { key: 23, label: "板方向检测时长", visible: true },
        { key: 24, label: "pin检测时长", visible: true },
        { key: 25, label: "数据日期", visible: true },
        { key: 26, label: "中控记录时间", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    this.getRouteList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
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
    /** 查询产品列表 */
    async getList(isSearch) {
      this.loading = true;
      this.queryParams.queryOrderBy =
        this.$cache.local.get("standard_QueryOrderBy") != undefined &&
        this.$cache.local.get("standard_QueryOrderBy") != "0"
          ? this.$cache.local.get("standard_QueryOrderBy") * 1
          : 2;
      const local_arr = this.$cache.local.getJSON("standard_routeCodeList")
        ? JSON.parse(this.$cache.local.getJSON("standard_routeCodeList"))
        : [];
      this.queryParams.routeCodeList = Array.isArray(local_arr)
        ? local_arr
        : [local_arr];
      const res = await getSummaryList(this.queryParams);

      this.deviceList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 表单重置
    reset() {
      this.form = {
        id: undefined,
        status: undefined,
        isCheckOk: undefined,
        remark: undefined,
      };
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      // 重置时时间范围也重置
      this.$cache.local.remove("standard_routeCodeList");
      this.queryParams.queryEndTime = undefined;
      this.queryParams.queryStartTime = this.getNextDate(new Date(), -1);
      this.handleQuery();
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
    },
    // 等待时长排序
    sortChange(info) {
      // ascending升序==0
      // descending降序==1
      let orderByWaitTime = undefined;
      if (info.order == "ascending") {
        orderByWaitTime = 0;
      } else if (info.order == "descending") {
        orderByWaitTime = 1;
      } else {
        orderByWaitTime = undefined;
      }

      this.queryParams.orderByWaitTime = orderByWaitTime;
      this.handleQuery();
    },
    // 监听排序方式选择
    handleQueryOrderBy(val) {
      this.$cache.local.set("standard_QueryOrderBy", val);
    },
    handleView(val) {
      this.$refs.summaryForm.queryParams = {
        ...this.queryParams,
        deviceCode: val,
      };
      this.$refs.summaryForm.title = val;
      this.$refs.summaryForm.getList();
      this.$refs.summaryForm.open = true;
    },
    // 多选工艺路线
    handleSelectRouteCodeList(val) {
      this.$cache.local.setJSON(
        "standard_routeCodeList",
        JSON.stringify(Array.isArray(val) ? val : [val])
      );
    },
    /** 导出按钮操作 */
    handleExport() {
      let name = "";
      if (!this.queryParams.queryStartTime)
        return this.$modal.msgWarning("请选择开始日期或一个时间段后再导出！");

      let pre = formatDateToYMD(this.queryParams.queryStartTime);

      if (this.queryParams.sailings == undefined) {
        name = pre + "-白班和晚班稼动率报表.xlsx";
      } else {
        name =
          pre +
          (this.queryParams.sailings == 0 ? "-白班" : "-晚班") +
          "稼动率报表.xlsx";
      }
      this.exportExcel(
        "/v1/DeviceRecords/DownLoadInnerSummaryListAsync",
        name,
        this.queryParams
      );
    },
  },
};
</script>

<style scoped>
.tableHei /deep/ th {
  padding: 0 !important;
  height: 30px;
  line-height: 30px;
  text-align: center;
}
.tableHei /deep/ td {
  padding: 0 !important;
  height: 35px;
  line-height: 30px;
  text-align: center;
}
</style>
