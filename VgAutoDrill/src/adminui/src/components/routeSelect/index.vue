<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="工艺路线选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-table
      border
      v-loading="loading"
      :data="itemList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedRouteId"
            :label="scope.row.id"
            :disabled="isApproval(scope.row)"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="路线编码"
        min-width="150"
        prop="code"
        show-overflow-tooltip
      >
      </el-table-column>
      <el-table-column
        label="路线名称"
        min-width="150"
        prop="name"
        show-overflow-tooltip
      />
      <el-table-column
        label="路线描述"
        min-width="150"
        prop="routeDesc"
        show-overflow-tooltip
      />
      <el-table-column label="审批状态" align="center" prop="vettingStatus">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.vettingStatus == 1">已审批</el-tag>
          <el-tag type="danger" v-else>未审批</el-tag>
        </template>
      </el-table-column>
    </el-table>
  </select-form-dialog>
</template>

<script>
import { listAllProcess } from "@/api/produce/task";

export default {
  name: "RouteSelect",
  data() {
    return {
      showFlag: false,
      selectedRouteId: undefined,
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
  methods: {
    isApproval(row) {
      if (row.vettingStatus == 0) {
        return true;
      }
      return false;
    },
    /** 查询生产任务列表 */
    getList(id) {
      this.loading = true;
      listAllProcess({ itemId: id }).then((response) => {
        this.itemList = response.data.routeInfos;
        //   this.total = response.data.total;
        this.loading = false;
      });
    },

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

    handleCurrent(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //行双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRow = row;
        if (row.vettingStatus == 0) return;
        this.$emit("onSelected", this.selectedRow);
        this.showFlag = false;
        this.selectedRouteId = undefined;
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
      this.selectedRow = this.itemList.find(
        (v) => v.id == this.selectedRouteId
      );
      if (this.selectedRouteId == null || this.selectedRouteId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedRouteId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
