<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="生产工单选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="工单编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入工单编码"
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
      :data="workOrderList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center" fixed="left">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedWorkOrderId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="工单编码"
        fixed="left"
        min-width="200px"
        prop="code"
        show-overflow-tooltip
      />
      <el-table-column
        label="工单名称"
        min-width="200px"
        prop="name"
        show-overflow-tooltip
      />
      <el-table-column
        label="来源类型"
        align="center"
        prop="orderSource"
        show-overflow-tooltip
      />
      <el-table-column
        label="来源单据"
        align="center"
        prop="sourceCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="产品名称"
        align="center"
        prop="itemName"
        min-width="200px"
        show-overflow-tooltip
      />
      <el-table-column
        label="产品编码"
        align="center"
        prop="itemCode"
        min-width="200px"
        show-overflow-tooltip
      />
      <el-table-column
        label="批次号"
        align="center"
        prop="batchCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="规格型号"
        align="center"
        prop="specification"
        show-overflow-tooltip
      />
      <el-table-column
        label="单位"
        align="center"
        prop="unitOfMeasure"
        show-overflow-tooltip
      />
      <el-table-column label="生产数量" align="center" prop="quantity" />
      <el-table-column label="调整数量" align="center" prop="quantityChanged" />
      <el-table-column
        label="已生产数量"
        align="center"
        prop="quantityProduced"
        width="100"
      />
      <el-table-column
        label="已排产数量"
        align="center"
        prop="quantityScheduled"
        width="100"
      />
      <el-table-column
        label="客户名称"
        align="center"
        prop="clientName"
        show-overflow-tooltip
      />
      <el-table-column
        label="客户编码"
        align="center"
        prop="clientCode"
        show-overflow-tooltip
      />
      <el-table-column label="需求日期" align="center" width="180">
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.requestDate, "{y}-{m}-{d}") }}</span>
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
import { listWorkOrder } from "@/api/produce/workOrder";
export default {
  name: "WorkOrderSelectSingle",
  components: {},
  props: ["isAdd"], 
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      selectedWorkOrderId: undefined,
      selectedRows: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 生产工单表格数据
      workOrderList: [],
      // 弹出层标题
      title: "",

      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        sourceCode: undefined,
        itemCode: undefined,
        batchCode: undefined,
        status: undefined,
      },
    };
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.resetForm("queryForm");
        this.selectedWorkOrderId = undefined;
      }
    },
  },
  methods: {
    /** 查询生产工单列表 */
    getList(isSearch) {
      this.loading = true;
      if (this.isAdd) {
        listWorkOrder({ ...this.queryParams, isAddWorkOrder: 0 }).then(
          (res) => {
            this.workOrderList = res.data.list;
            this.total = res.data.total;
            this.loading = false;
            // 只有搜索状态下进行提示
            if (isSearch == "search") {
              this.$modal.msgSearch(
                "搜索成功，共" + res.data.total + "条数据！"
              );
            }
          }
        );
      } else {
        listWorkOrder(this.queryParams).then((res) => {
          this.workOrderList = res.data.list;
          this.total = res.data.total;
          this.loading = false;
          // 只有搜索状态下进行提示
          if (isSearch == "search") {
            this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
          }
        });
      }
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
        this.selectedRows = row;
      }
    },
    // 单选选中数据
    handleRowChange(row) {
      if (row) {
        this.selectedRows = row;
      }
    },
    //双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRows = row;
        this.selectedWorkOrderId = row.id;
        this.$emit("onSelected", this.selectedRows);
        this.showFlag = false;
        this.selectedWorkOrderId = undefined;
        this.selectedRow = undefined;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectedWorkOrderId == null || this.selectedWorkOrderId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRows);
      this.showFlag = false;
      this.selectedWorkOrderId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
