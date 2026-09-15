<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    :title="title"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="休息点编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入休息点编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="休息点名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入休息点名称"
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
      :data="itemList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center" fixed="left">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedAgvRestId"
            :label="scope.row.code"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="休息点编码"
        prop="code"
        show-overflow-tooltip
        min-width="120"
      >
      </el-table-column>
      <el-table-column
        label="休息点名称"
        prop="name"
        show-overflow-tooltip
        min-width="120"
      />

      <el-table-column label="物理点位" prop="point" show-overflow-tooltip />

      <el-table-column
        label="预定分配(AGV)"
        prop="preBookAgv"
        min-width="120"
        show-overflow-tooltip
      />

      <el-table-column
        label="正占用(AGV)"
        min-width="120"
        prop="currentAgv"
        show-overflow-tooltip
      />
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
import { listAgvRest } from "@/api/wareHouse/agvRest";
export default {
  name: "AgvRestSelect",
  data() {
    return {
      showFlag: false,
      selectedAgvRestId: undefined,
      title: undefined,
      selectedRow: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 生产任务表格数据
      itemList: [],
      // 弹出层标题
      title: "",

      // 查询参数
      queryParams: {
        pageNum: 0,
        pageSize: 10,
        name: undefined,
        code: undefined,
      },
    };
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedAgvRestId = 0;
        this.resetForm("queryForm");
      }
    },
  },
  methods: {
    /** 查询生产任务列表 */
    getList(isSearch) {
      this.loading = true;
      listAgvRest(this.queryParams).then((res) => {
        this.itemList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    // 是否有产品大类
    isHasproductCategory(row) {
      if (row.productCategoryId) {
        return false;
      }
      return true;
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
        this.selectedAgvRestId = undefined;
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
      if (this.selectedAgvRestId == null || this.selectedAgvRestId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedAgvRestId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
