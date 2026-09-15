<template>
  <edit-form-dialog
    v-model="open"
    :title="title"
    :optType="'view'"
    @submitForm="submitForm"
  >
    <el-form
      v-affix
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
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
            v-for="item in routeList"
            :key="item.id"
            :label="item.code"
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

      <el-form-item>
        <el-button
          v-debounce
          type="primary"
          icon="el-icon-search"
          @click="handleQuery"
          >搜索</el-button
        >
        <el-button v-debounce icon="el-icon-refresh" @click="resetQuery"
          >重置</el-button
        >
      </el-form-item>
    </el-form>
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
        :page="page"
      ></right-toolbar>
    </el-row>
    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="deviceList"
      @selection-change="handleSelectionChange"
    >
      <el-table-column
        label="设备编码"
        prop="deviceCode"
        fixed="left"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
      </el-table-column>
      <el-table-column
        label="工艺路线"
        min-width="100"
        align="center"
        prop="routeCode"
        show-overflow-tooltip
        v-if="columns[1].visible"
      >
      </el-table-column>
      <el-table-column
        label="实际稼动率(%)"
        min-width="120"
        align="center"
        prop="duty"
        show-overflow-tooltip
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <span :style="{ color: getDutyColor(scope.row.duty) }">{{
            scope.row.duty
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="开机时长(分钟)"
        min-width="120"
        align="center"
        prop="openTime"
        v-if="columns[3].visible"
      >
      </el-table-column>
      <el-table-column
        label="设备异常时长(分钟)"
        align="center"
        min-width="160"
        prop="errorTime"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span style="color: red">{{ scope.row.errorTime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="工作时长(分钟)"
        min-width="120"
        align="center"
        prop="workTime"
        v-if="columns[5].visible"
      >
      </el-table-column>
      <el-table-column
        label="等待时长(分钟)"
        align="center"
        min-width="120"
        prop="waitTime"
        v-if="columns[6].visible"
      >
      </el-table-column>
      <el-table-column
        label="结束-开始总时长(分钟)"
        align="center"
        min-width="160"
        prop="endToStartTime"
        v-if="columns[7].visible"
      >
      </el-table-column>
      <el-table-column
        label="清洗夹头总时长(分钟)"
        min-width="150"
        align="center"
        prop="collectClearTime"
        v-if="columns[8].visible"
      >
      </el-table-column>

      <el-table-column
        label="数据日期"
        align="center"
        min-width="180"
        v-if="columns[9].visible"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.dateString, "{y}-{m}-{d} {h}:{i}:{s}")
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
  </edit-form-dialog>
</template>

<script>
import { listDeviceRecords } from "@/api/device/deviceRecords";
import { listRoute } from "@/api/produce/route";
export default {
  name: "Summary",
  data() {
    return {
      page: "summary",
      optType: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 产品表格数据
      deviceList: [],
      routeList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceCode: undefined,
        queryStartTime: undefined,
        queryEndTime: undefined,
        queryOrderBy: 4,
        routeCodeList: [],
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
        { key: 2, label: "实际稼动率", visible: true },
        { key: 3, label: "开始时长", visible: true },
        { key: 4, label: "异常时长", visible: true },
        { key: 5, label: "工作时长", visible: true },
        { key: 6, label: "等待时长", visible: true },
        { key: 7, label: "结束-开始总时长", visible: true },
        { key: 8, label: "清洗夹头时长", visible: true },
        { key: 9, label: "数据日期", visible: true },
      ],
    };
  },
  activated() {
    // this.getList();
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
    // 查询所有已审批工艺路线
    getRouteList() {
      listRoute({ pageNum: 1, pageSize: 1000, vettingStatus: 1 }).then(
        (res) => {
          this.routeList = res.data.list;
        }
      );
    },
    /** 查询产品列表 */
    async getList(isSearch) {
      this.loading = true;
      this.queryParams.queryOrderBy =
        this.$cache.local.get("deviceRecords_QueryOrderBy") != undefined &&
        this.$cache.local.get("deviceRecords_QueryOrderBy") != "0"
          ? this.$cache.local.get("deviceRecords_QueryOrderBy") * 1
          : 4;
      const res = await listDeviceRecords(this.queryParams);
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
      this.queryParams.queryEndTime = undefined;
      this.queryParams.queryStartTime = undefined;
      this.queryParams.routeCodeList = [];

      this.resetForm("queryForm");
      this.handleQuery();
    },

    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
    },
    // 监听排序方式选择
    handleQueryOrderBy(val) {
      this.$cache.local.set("deviceRecords_QueryOrderBy", val);
    },

    submitForm() {},
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/DeviceRecords/DownLoadList",
        "设备" + this.queryParams.deviceCode + "明细报表.xlsx",
        this.queryParams
      );
    },
  },
};
</script>

<style lang="scss" scoped>
</style>