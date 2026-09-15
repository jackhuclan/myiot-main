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
        <el-form-item prop="sEventID">
          <border-title>
            <el-input
              style="border: none; width: 200px"
              v-model="queryParams.sEventID"
              placeholder="事件代码"
              clearable
              @keyup.enter.native="handleQuery"
          /></border-title>
        </el-form-item>
        <el-form-item prop="sEventDesc">
          <border-title>
            <el-input
              style="border: none; width: 200px"
              v-model="queryParams.sEventDesc"
              placeholder="日志描述"
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
        <el-table-column label="日期" align="center" min-width="100px"
          ><template slot-scope="scope">{{
            parseTime(scope.row.dtDate, "{y}-{m}-{d}")
          }}</template>
        </el-table-column>
        <el-table-column label="事件时间" align="center" prop="sTime">
        </el-table-column>
        <el-table-column
          label="日志描述"
          prop="sEventDesc"
          align="center"
          show-overflow-tooltip
          min-width="120px"
        >
        </el-table-column>
        <el-table-column
          label="钻带文件"
          prop="sActProgram"
          align="center"
          show-overflow-tooltip
          min-width="120px"
        >
        </el-table-column>
        <el-table-column
          label="事件代码"
          prop="sEventID"
          align="center"
          show-overflow-tooltip
          min-width="120px"
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
    </el-card>
  </div>
</template>

<script>
import { queryM54List } from "@/api/event"; 
export default { 
  data() {
    return {
      page: "M54List",
      // 遮罩层
      loading: false,
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 事件表格数据
      list: [],
      //   是否显示搜索
      showSearch: true,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        sStartDate: undefined,
        sEndDate: undefined,
        sEquipmentID: undefined,
        sEventID: undefined,
        sEventDesc: undefined,
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
      queryM54List(this.queryParams).then((res) => {
        this.list = res.List;
        this.total = res.Total;
        this.loading = false;
      });
    },
  },
};
</script>
 