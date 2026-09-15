<template>
  <div>
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5" v-if="isApproval">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-s-operation"
          @click="handleAuto"
          >自动分配</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDeleteList"
          >批量删除</el-button
        >
      </el-col>
    </el-row>

    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="workOrderAndWorkStation"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="机台编码"
        prop="workStationCode"
        :min-width="
          flexColumnWidth(
            '机台编码',
            'workStationCode',
            workOrderAndWorkStation
          )
        "
        show-overflow-tooltip
      />
      <el-table-column
        label="机台名称"
        show-overflow-tooltip
        :min-width="
          flexColumnWidth(
            '机台名称',
            'workStationName',
            workOrderAndWorkStation
          )
        "
        prop="workStationName"
      />
      <el-table-column
        label="待做任务"
        width="80px"
        align="center"
        prop="waitTaskCount"
        show-overflow-tooltip
      />
      <el-table-column
        label="操作"
        align="center"
        width="80px"
        v-if="optType != 'view'"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            v-debounce
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            >删除</el-button
          >
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="total > 0"
      class="table-pagination"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
      layout="total, sizes, prev, pager, next"
      :autoScroll="false"
    />
    <DrillAndWorkStationSelect
      ref="drillAndWorkStationSelect"
      @onSelected="onDrillAndWorkStationSelected"
    >
    </DrillAndWorkStationSelect>
  </div>
</template>

<script>
import {
  getWorkOrderAndWorkStation,
  delList,
  addWorkOrderAndWorkStation,
} from "@/api/produce/workOrderAndWorkStation";
import { getFitWorkStationList } from "@/api/produce/drillWorkOrder";
import DrillAndWorkStationSelect from "@/components/drillAndWorkStationSelect";
export default {
  name: "FormMachine",
  components: { DrillAndWorkStationSelect },
  data() {
    return {
      page: "formMachine",
      // 遮罩层
      loading: false,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 需要回显的数据
      echoIds: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工艺关联设备表格数据
      workOrderAndWorkStation: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        workOrderId: undefined,
        workStationId: undefined,
        workStationCode: undefined,
        workStationName: undefined,
      },
      // 自动审批查询参数
      approvalQueryParams: {
        pageNum: 1,
        pageSize: 1000,
        workOrderCode: undefined,
        workOrderId: undefined,
        routeCode: undefined,
        fitCount: 0,
        processCode: "drill",
        workStationCode: undefined,
        workStationName: undefined,
      },
    };
  },
  props: ["optType", "workOrderId", "workOrderCode", "routeCode", "isApproval"],
  methods: {
    /** 查询工艺组成列表 */
    async getList(workOrderId) {
      this.queryParams.workOrderId =
        typeof workOrderId == "string" && workOrderId
          ? workOrderId
          : this.workOrderId;
      this.loading = true;
      const res = await getWorkOrderAndWorkStation(this.queryParams);
      this.workOrderAndWorkStation = res.data.list;
      this.total = res.data.total;
      this.loading = false;
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      this.handleDoubleClick(row, this, column);
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.multipleSelection = selection;
      this.ids = selection.map((item) => item.id);
    },
    // 表单重置
    reset() {
      this.resetForm("form");
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

    /** 新增按钮操作 */
    handleAdd() {
      if (this.workOrderId == null || this.workOrderId == "")
        return this.$modal.msgError("请先选择工单");

      this.reset();
      // 需要回显的数据
      this.echoIds = [];
      getWorkOrderAndWorkStation({
        pageNum: 1,
        pageSize: 10000,
        workOrderId: this.workOrderId,
      }).then((res) => {
        res.data.list?.forEach((v) => {
          this.echoIds.push({
            workStationId: v.workStationId,
            workStationCode: v.workStationCode,
            workStationName: v.workStationName,
          });
        });

        this.$refs.drillAndWorkStationSelect.queryParams.workOrderId =
          this.workOrderId;
        this.$refs.drillAndWorkStationSelect.queryParams.workOrderCode =
          this.workOrderCode;
        this.$refs.drillAndWorkStationSelect.queryParams.routeCode =
          this.routeCode;
        this.$refs.drillAndWorkStationSelect.queryParams.pageNum = 1;
        this.$refs.drillAndWorkStationSelect.showFlag = true;
        this.$refs.drillAndWorkStationSelect.showData = this.echoIds;
        this.$refs.drillAndWorkStationSelect.getList();
      });
    },
    // 自动分配
    handleAuto() {
      this.approvalQueryParams.workOrderId = this.workOrderId;
      this.approvalQueryParams.workOrderCode = this.workOrderCode;
      this.approvalQueryParams.routeCode = this.routeCode;
      getFitWorkStationList(this.approvalQueryParams).then((res) => {
        const workStationDatas = res.data.list.map((v) => {
          return {
            workStationId: v.id,
            workStationCode: v.code,
            workStationName: v.name,
          };
        });
        addWorkOrderAndWorkStation({
          workOrderId: this.workOrderId,
          workOrderCode: this.workOrderCode,
          workStationDatas,
        })
          .then((res) => {
            if (res.code == 0) {
              this.getList(this.workOrderId);
              this.$modal.msgSuccess("操作成功");
            } else {
              this.$modal.notifyError(res.message);
            }
          })
          .catch(() => {});
      });
    },
    // 单选删除
    handleDelete(row) {
      this.deleteItem(
        [row.id],
        delList,
        this.getList,
        "机台编码为" + row.workStationCode
      );
    },
    // 多选删除
    handleDeleteList() {
      this.deleteItem(this.ids, delList, this.getList);
    },

    //机台弹出框
    handleSelectWorkStation() {
      this.$refs.workStationSelect.showFlag = true;
    },
    onDrillAndWorkStationSelected(workStationDatas) {
      if (
        this.isArrEqual(
          workStationDatas.map((v) => v.workStationId),
          this.workOrderAndWorkStation,
          "workStationId"
        )
      ) {
        return (this.$refs.workStationSelect.showFlag = false);
      }
      addWorkOrderAndWorkStation({
        workOrderId: this.workOrderId,
        workOrderCode: this.workOrderCode,
        workStationDatas,
      })
        .then((res) => {
          if (res.code == 0) {
            this.queryParams.pageNum = 1;
            this.getList(this.workOrderId);
            this.$modal.msgSuccess("操作成功");
          } else {
            this.$modal.notifyError(res.message);
          }
        })
        .catch(() => {});
    },
  },
};
</script>
<style lang="scss" scoped>
.table-pagination {
  margin-bottom: 20px;
}
</style>
