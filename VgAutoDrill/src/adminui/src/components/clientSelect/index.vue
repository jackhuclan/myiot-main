<template>
  <select-form-dialog
    v-model="showFlag"
    v-if="showFlag"
    title="客户选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="客户编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入客户编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="客户名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入客户名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
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

    <el-table
      border
      v-loading="loading"
      :data="clientList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedClientId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column label="客户编码" align="center" prop="code">
      </el-table-column>
      <el-table-column
        label="客户名称"
        width="150px"
        align="center"
        prop="name"
        show-overflow-tooltip
      />
      <el-table-column label="是否启用" align="center" prop="status">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template>
      </el-table-column>
    </el-table>
    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
      :autoScroll="false"
    />
  </select-form-dialog>
</template>

<script>
import { listClient } from "@/api/masterData/client";
export default {
  name: "ClientSelect",

  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedClientId = 0;
        this.resetForm("queryForm");
      }
    },
  },
  data() {
    return {
      showFlag: false,
      selectedClientId: undefined,
      selectedRow: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 客户表格数据
      clientList: [],
      // 弹出层标题
      title: "",

      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        status: 1,
      },
    };
  },
  methods: {
    /** 查询客户列表 */
    getList(isSearch) {
      this.loading = true;
      listClient(this.queryParams).then((res) => {
        this.clientList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
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
      this.resetForm("queryForm");
      this.handleQuery();
    },

    handleCurrent(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //行双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRow = row;
        this.$emit("onSelected", this.selectedRow);
        this.showFlag = false;
        this.selectedClientId = undefined;
        this.selectedRow = undefined;
      }
    },
    // 单选选中数据
    handleRowChange(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectedClientId == null || this.selectedClientId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedClientId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
