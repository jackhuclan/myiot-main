<template>
  <div class="app-container">
    <el-card class="page-header">
      <el-form
        :model="queryParams"
        ref="queryForm"
        size="small"
        :inline="true"
        v-show="showSearch"
        label-width="68px"
      >
        <el-form-item prop="sEquipmentID">
          <border-title>
            <el-input
              style="border: none; width: 200px"
              v-model="queryParams.sEquipmentID"
              placeholder="机台号"
              clearable
              @keyup.enter.native="handleQuery"
          /></border-title>
        </el-form-item>

        <el-form-item prop="sStartDate">
          <border-title title="开始日期">
            <el-date-picker
              v-model="queryParams.sStartDate"
              style="border: none; width: 200px"
              value-format="yyyy-MM-dd"
              type="date"
            ></el-date-picker>
          </border-title>
        </el-form-item>
        <el-form-item prop="sEndDate">
          <border-title title="截止日期">
            <el-date-picker
              v-model="queryParams.sEndDate"
              style="border: none; width: 200px"
              value-format="yyyy-MM-dd"
              type="date"
            ></el-date-picker>
          </border-title>
        </el-form-item>
        <el-form-item>
          <border-title :border="false">
            <el-button type="primary" icon="el-icon-search" @click="handleQuery"
              >搜索</el-button
            >
            <el-button icon="el-icon-refresh" @click="resetQuery"
              >重置</el-button
            ></border-title
          >
        </el-form-item>
      </el-form>
    </el-card>
    <el-card class="page-body">
      <el-table :ref="page" v-loading="loading" :data="list" border>
        <el-table-column
          show-overflow-tooltip
          label="机台编号"
          prop="sEquipmentID"
          fixed="left"
          min-width="120px"
          align="center"
        >
        </el-table-column>
        <el-table-column
          show-overflow-tooltip
          align="center"
          label="方式"
          prop="sZ"
        />
        <el-table-column label="日期" align="center" min-width="100px"
          ><template slot-scope="scope">{{
            parseTime(scope.row.dtDate, "{y}-{m}-{d}")
          }}</template>
        </el-table-column>
        <el-table-column label="采集时间" prop="sTime"> </el-table-column>
        <el-table-column label="稼动率" align="center" prop="sDuty">
        </el-table-column>
        <el-table-column label="加工时间" align="center" prop="sWorkTime">
        </el-table-column>
        <el-table-column label="等待时间" align="center" prop="sWaitTime">
        </el-table-column>
        <el-table-column label="停机时间" align="center" prop="sStopTime">
        </el-table-column>
        <el-table-column label="总时间" align="center" prop="sTotalTime">
        </el-table-column>
        <el-table-column label="孔数" align="center" prop="sHits">
        </el-table-column>
        <el-table-column label="锣程" align="center" prop="sRoutPath">
        </el-table-column>
        <el-table-column label="换板次数" align="center" prop="sChangePanels">
        </el-table-column>
        <el-table-column label="换料次数" align="center" prop="sChangeDrills">
        </el-table-column>

        <el-table-column label="注册日期" align="center" min-width="100px"
          ><template slot-scope="scope">{{
            parseTime(scope.row.sRegistrationDate, "{y}-{m}-{d}")
          }}</template>
        </el-table-column>
        <el-table-column label="所属班次" align="center" prop="sShift">
        </el-table-column>
      </el-table>
      <pagination
        v-show="total > 0"
        :total="total"
        :page.sync="queryParams.pageNum"
        :limit.sync="queryParams.pageSize"
        @pagination="getList"
      />
    </el-card>
  </div>
</template>

<script>
import { queryRealTimeDutyList } from "@/api/duty";
export default {
  data() {
    return {
      page: "realTimeDutylist",
      optType: "",
      // 遮罩层
      loading: false,
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 10,
      // 事件表格数据
      list: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        sStartDate: undefined,
        sEndDate: undefined,
        sEquipmentID: undefined,
      },
    };
  },
  activated() {
    this.getList();
  },
  methods: {
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList();
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.resetForm("queryForm");
      this.handleQuery();
    },
    // 获取数据
    getList() {
      this.queryParams.sStartDate =
        this.queryParams.sStartDate == null
          ? undefined
          : this.queryParams.sStartDate;
      this.queryParams.sEndDate =
        this.queryParams.sEndDate == null
          ? undefined
          : this.queryParams.sEndDate;
      this.loading = true;
      this.loading = true;
      queryRealTimeDutyList(this.queryParams).then((res) => {
        this.list = res.List;
        this.total = res.Total;
        this.loading = false;
      });
    },
  },
};
</script>

 