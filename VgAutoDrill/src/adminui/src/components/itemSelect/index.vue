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
      <el-form-item label="物料编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入物料编码"
          clearable
          style="width: 240px"
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入物料名称"
          clearable
          style="width: 240px"
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
      :data="itemList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center" fixed="left">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedItemCode"
            :label="scope.row.code"
            @change="handleRowChange(scope.row)"
            :disabled="isHasproductCategory(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="物料编码"
        fixed="left"
        min-width="200px"
        prop="code"
        show-overflow-tooltip
      >
      </el-table-column>
      <el-table-column
        label="物料名称"
        min-width="200px"
        prop="name"
        show-overflow-tooltip
      />
      <el-table-column
        label="规格型号"
        prop="specification"
        show-overflow-tooltip
      />
      <el-table-column label="单位" prop="unitOfMeasure" show-overflow-tooltip>
      </el-table-column>
      <el-table-column label="物料/产品" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.itemOrProduct == 1 ? "物料" : "产品" }}</span>
        </template>
      </el-table-column>

      <el-table-column
        label="产品大类"
        prop="productCategoryName"
        min-width="200px"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          <span v-if="scope.row.productCategoryId">{{
            scope.row.productCategoryName
          }}</span>
          <el-tag type="danger" v-else> 未配置 </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="叠数"
        align="center"
        key="panelCount"
        prop="panelCount"
      />
      <el-table-column
        label="板长"
        align="center"
        key="panelLength"
        prop="panelLength"
      />
      <el-table-column
        label="板宽"
        align="center"
        key="panelWidth"
        prop="panelWidth"
      />
      <el-table-column
        label="库房名称"
        min-width="200px"
        prop="warehouseName"
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
    <div style="font-weight: bold">
      提示：如未查询到需要的产品，请至此页面添加 主数据->产品物料管理！
    </div>
  </select-form-dialog>
</template>

<script>
import { listItem } from "@/api/masterData/item";
export default {
  name: "ItemSelect",
  data() {
    return {
      showFlag: false,
      selectedItemCode: undefined,
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
        // 只允许选择产品物料
        itemOrProduct: "2",
        // 只允许选择成品物料
        //itemTypeId: "22",
        status: 1,
      },
    };
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedItemCode = 0;
        this.resetForm("queryForm");
      }
    },
  },
  methods: {
    /** 查询生产任务列表 */
    getList(isSearch) {
      this.loading = true;
      listItem(this.queryParams).then((res) => {
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
        if (row.productCategoryId == undefined) return;
        this.$emit("onSelected", this.selectedRow);
        this.showFlag = false;
        this.selectedItemCode = undefined;
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
      if (this.selectedItemCode == null || this.selectedItemCode == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedItemCode = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
