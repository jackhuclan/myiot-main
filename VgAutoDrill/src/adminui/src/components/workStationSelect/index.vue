<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="工作站选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-table
      border
      v-loading="loading"
      :data="workStationList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedWorkStationId"
            :label="scope.row.workStationId"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="工作站编码"
        align="center"
        prop="workStationCode"
      />
      <el-table-column
        label="工作站名称"
        align="center"
        prop="workStationName"
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
import { listRouteProcessAndWorkStation } from "@/api/masterData/workStation";
import { listWorkshop } from "@/api/masterData/workShop";
export default {
  name: "WorkStationSelect", 
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      selectedWorkStationId: undefined,
      selectedRows: [],
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工作站表格数据
      workStationList: [],
      //车间选项
      workshopOptions: [],
      //工序选项
      processOptions: [],
      // 弹出层标题
      title: "",

      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        workshopId: undefined,
        workshopCode: undefined,
        workshopName: undefined,
      },
      // 排产页面展示查询参数
      // 查询参数
      queryTaskParams: {
        pageNum: 1,
        pageSize: 10,
        workStationId: undefined,
        routeAndProcessId: undefined,
      },
    };
  },
  created() {
    this.getWorkshops();
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.resetForm("queryForm");
        this.selectedWorkStationId = undefined;
      }
    },
  },
  methods: {
    /** 查询工作站列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listRouteProcessAndWorkStation(this.queryTaskParams);
      this.workStationList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    //查询车间信息
    getWorkshops() {
      listWorkshop({
        pageNum: 1,
        pageSize: 100,
      }).then((res) => {
        this.workshopOptions = res.data.list;
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
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
      this.selectedRow = selection;
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
        this.selectedWorkStationId = undefined;
        this.selectedRows = undefined;
      }
    },

    //确定选中
    confirmSelect() {
      if (
        this.selectedWorkStationId == null ||
        this.selectedWorkStationId == 0
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
      this.selectedWorkStationId = undefined;
      this.selectedRows = undefined;
    },
  },
};
</script>
