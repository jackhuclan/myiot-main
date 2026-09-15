<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="入库记录选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料名称" prop="itemName">
        <el-input
          v-trim
          v-model="queryParams.itemName"
          placeholder="请输入物料名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="分区" prop="warehouseName">
        <el-input
          v-trim
          v-model="queryParams.warehouseName"
          placeholder="请输入分区"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item>
        <el-button
          type="primary"
          icon="el-icon-search"
          @click="handleQuery"
          v-debounce
          >搜索</el-button
        >
        <el-button icon="el-icon-refresh" @click="resetQuery" v-debounce
          >重置</el-button
        >
      </el-form-item>
    </el-form>

    <el-table
      border
      v-loading="loading"
      :data="materialStockList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center" fixed="left">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedMaterialStockId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      =<el-table-column
        label="出/入库单号"
        fixed="left"
        prop="code"
        show-overflow-tooltip
        min-width="200px"
      />
      <el-table-column
        label="物料编码"
        min-width="200px"
        prop="itemCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="物料名称"
        min-width="200px"
        prop="itemName"
        show-overflow-tooltip
      />
      <el-table-column
        min-width="200px"
        label="工单编码"
        prop="workOrderCode"
        show-overflow-tooltip
      />

      <el-table-column
        label="分区"
        min-width="200px"
        prop="warehouseName"
        show-overflow-tooltip
      />
      <el-table-column
        label="分区编码"
        min-width="200px"
        prop="warehouseCode"
        show-overflow-tooltip
      />

      <el-table-column
        label="料仓编码"
        min-width="200px"
        prop="siloCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="料仓最大层"
        align="center"
        prop="maxLayers"
        width="90px"
      />
      <el-table-column
        label="料仓实际层"
        align="center"
        prop="currentLayers"
        width="90px"
      />
      <!-- <el-table-column
        label="物料类型"
        align="center"
        prop="itemType"
             show-overflow-tooltip
      /> -->
      <el-table-column
        label="规格型号"
        align="center"
        prop="specification"
        show-overflow-tooltip
      />
      <el-table-column label="数量" align="center" prop="quantityTransaction" />
      <el-table-column label="在库数量" align="center" prop="quantityOnhand" />
      <el-table-column
        label="单位"
        align="center"
        prop="unitOfMeasure"
        show-overflow-tooltip
      />
      <el-table-column
        label="批次号"
        align="center"
        prop="batchCode"
        show-overflow-tooltip
      />
    </el-table>
    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />
  </select-form-dialog>
</template>

<script>
import { listMaterialStock } from "@/api/wareHouse/materialStock";
export default {
  name: "MaterialStockSelect", 
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      selectedMaterialStockId: undefined,
      selectedRows: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工作站表格数据
      materialStockList: [],
      // 弹出层标题
      title: "",
      // 查询参数
      queryParams: {
        // 查询参数
        queryParams: {
          pageNum: 1,
          pageSize: 1000,
          siloCode: undefined,
          inOrOut: "in",
          itemTypeId: 0,
        },
      },
    };
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedMaterialStockId = undefined;
        this.resetForm("queryForm");
      }
    },
  },
  methods: {
    /** 查询工作站列表 */
    getList(isSearch) {
      this.loading = true;
      listMaterialStock(this.queryParams).then((res) => {
        this.materialStockList = res.data.list.filter(
          (v) => v.quantityOnhand > 0
        );
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
        this.$emit("onSelected", this.selectedRows);
        this.showFlag = false;
        this.selectedMaterialStockId = undefined;
        this.selectedRow = undefined;
      }
    },
    //确定选中
    confirmSelect() {
      if (
        this.selectedMaterialStockId == null ||
        this.selectedMaterialStockId == 0
      ) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRows);
      this.showFlag = false;
      this.selectedMaterialStockId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
