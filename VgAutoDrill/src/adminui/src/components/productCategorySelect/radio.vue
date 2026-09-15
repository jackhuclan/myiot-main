<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="产品大类选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
      ><el-form-item label="产品大类编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入产品大类编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="产品大类名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入产品大类名称"
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
      :data="productCategoryList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedProductCategoryId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column label="产品大类编码" min-width="150" prop="code" />
      <el-table-column label="产品大类名称" min-width="150" prop="name" />
      <el-table-column label="审批状态" align="center" prop="vettingStatus">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.vettingStatus == 1">已审批</el-tag>
          <el-tag type="danger" v-else>未审批</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center" prop="status">
        <template slot-scope="scope">
          <dict-tag
            :options="dict.type.sys_normal_disable"
            :value="scope.row.status"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        min-width="150"
        show-overflow-tooltip
        prop="remark"
      />

      <el-table-column
        label="创建时间"
        align="center"
        prop="createTime"
        width="180"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
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
import { listProductCategory } from "@/api/masterData/productCategory";
export default {
  name: "ProductCategorySelectRadio",
  dicts: ["sys_normal_disable"],
  data() {
    return {
      showFlag: false,
      selectedProductCategoryId: undefined,
      selectedRow: undefined,
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 产品大类表格数据
      productCategoryList: [],
      // 查询参数
      queryParams: {
        pageNum: 0,
        pageSize: 10,
        name: undefined,
        code: undefined,
        remark: undefined,
        status: 1,
        vettingStatus: 1,
      },
    };
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedProductCategoryId = undefined;
        this.resetForm("queryForm");
      }
    },
  },
  methods: {
    /** 查询产品大类列表 */
    getList(isSearch) {
      this.loading = true;
      listProductCategory(this.queryParams).then((res) => {
        this.productCategoryList = res.data.list;
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
        this.selectedProductCategoryId = undefined;
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
      if (
        this.selectedProductCategoryId == null ||
        this.selectedProductCategoryId == 0
      ) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedProductCategoryId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
