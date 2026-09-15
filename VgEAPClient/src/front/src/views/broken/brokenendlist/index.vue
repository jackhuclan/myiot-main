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
        <el-form-item prop="sActProgram">
          <border-title>
            <el-input
              style="border: none; width: 200px"
              v-model="queryParams.sActProgram"
              placeholder="文件名"
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
            ><el-button icon="el-icon-download" @click="resetQuery"
              >导出</el-button
            >
          </border-title>
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
        <el-table-column label="断刀时间" align="center" prop="sTime">
        </el-table-column>
        <el-table-column
          label="钻带文件"
          align="center"
          prop="sActProgram"
          show-overflow-tooltip
          min-width="120px"
        >
        </el-table-column>
        <el-table-column label="断刀次数" align="center" prop="sBrokens">
        </el-table-column>
        <el-table-column label="断刀轴号" align="center" prop="sSpindle">
        </el-table-column>
        <el-table-column label="刀具号" align="center" prop="sToolID">
        </el-table-column>
        <el-table-column label="刀径" align="center" prop="sToolDIA">
        </el-table-column>
        <el-table-column label="孔号" align="center" prop="sHoleID">
        </el-table-column>
        <el-table-column label="X坐标" align="center" prop="sX">
        </el-table-column>
        <el-table-column label="Y坐标" align="center" prop="sY">
        </el-table-column>
        <el-table-column label="程序块" align="center" prop="sProgramBlock">
        </el-table-column>
        <el-table-column label="程序阶级" align="center" prop="sProgramStep">
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
import { queryBrokenEndList } from "@/api/broken"; 
export default { 
  data() {
    return {
      page: "brokenendlist",
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
      queryBrokenEndList(this.queryParams).then((res) => {
        this.list = res.List;
        this.total = res.Total;
        this.loading = false;
      });
    },
  },
};
</script>
 