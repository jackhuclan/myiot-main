<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="AGV编码" prop="deviceCode">
        <el-input
          v-trim
          v-model="queryParams.deviceCode"
          placeholder="请输入AGV编码"
          clearable
          style="width: 240px"
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="AGV名称" prop="deviceName">
        <el-input
          v-trim
          v-model="queryParams.deviceName"
          placeholder="请输入AGV名称"
          clearable
          style="width: 240px"
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="开始日期">
        <el-date-picker
          v-model="dateRange"
          style="width: 240px"
          value-format="yyyy-MM-dd"
          type="daterange"
          range-separator="-"
          start-placeholder="开始日期"
          end-placeholder="结束日期"
        ></el-date-picker>
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
          :disabled="hasPermi(['device:agvList:export'])"
          >导出</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
      ></right-toolbar>
    </el-row>
    <el-table border ref="multipleTable" v-loading="loading" :data="roleList">
      <el-table-column label="AGV编码" min-width="180" prop="deviceCode">
      </el-table-column>
      <el-table-column label="AGV名称" min-width="180" prop="deviceName">
      </el-table-column>
      <el-table-column
        label="日期"
        align="center"
        key="statsDate"
        prop="statsDate"
        min-width="120"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.statsDate, "{y}-{m}-{d}") }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="总数量"
        min-width="100"
        prop="totalCount"
        align="center"
      >
      </el-table-column>
      <el-table-column
        label="完成数量"
        min-width="120"
        prop="completedCount"
        align="center"
      >
      </el-table-column>
      <el-table-column
        label="异常数量"
        min-width="120"
        prop="failedCount"
        align="center"
      >
      </el-table-column>
      <el-table-column
        label="取消数量"
        min-width="120"
        prop="canceledCount"
        align="center"
      >
      </el-table-column>
    </el-table>

    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />
  </div>
</template>

<script>
import { getAgvList } from "@/api/device/agvList";

export default {
  name: "AgvList",
  data() {
    return {
      page: "agvList",
      optType: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 角色表格数据
      roleList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 日期范围
      dateRange: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        deviceCode: undefined,
        deviceName: undefined,
        deviceTypeCode: undefined,
        startDate: undefined,
        endDate: undefined,
      },
    };
  },
  activated() {
    this.getList();
  },

  methods: {
    /** 查询角色列表 */
    getList(isSearch) {
      this.loading = true;
      this.queryParams.startDate =
        this.dateRange != undefined ? this.dateRange[0] : undefined;
      this.queryParams.endDate =
        this.dateRange != undefined ? this.dateRange[1] : undefined;
      getAgvList(this.queryParams).then((res) => {
        this.roleList = res.data.list;
        this.total = res.data.total;
        this.loading = false; // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },

    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.dateRange = [];
      this.handleQuery();
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/DeviceScheduleSummary/DownLoadList",
        "AGV汇总.xlsx",
        this.queryParams
      );
    },
  },
};
</script>
