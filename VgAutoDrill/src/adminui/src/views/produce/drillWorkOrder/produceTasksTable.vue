<template>
  <div>
    <el-table :data="list" style="width: 100%" v-loading="loading" border>
      <el-table-column
        prop="taskCode"
        label="任务编码"
        show-overflow-tooltip
        min-width="160"
      >
        <template slot-scope="scope">
          <span class="click_code" :data-id="scope.row.id"   v-isGetSelection  >{{
            scope.row.taskCode
          }}</span>
        </template>
      </el-table-column>
      <el-table-column prop="nowWadCount" label="数量" align="center" />
      <el-table-column
        label="开始时间"
        prop="startTime"
        show-overflow-tooltip
        align="center"
        min-width="150"
      >
        <template slot-scope="scope">
          <span>{{
            parseTime(scope.row.startTime, "{m}-{d} {h}:{i}:{s}")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="结束时间"
        prop="endTime"
        show-overflow-tooltip
        align="center"
        min-width="150"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.endTime, "{m}-{d} {h}:{i}:{s}") }}</span>
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
    />
    <edit-form-dialog v-model="open" title="查看待制任务信息" optType="view">
      <el-form ref="form" :model="form"> </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import { getMaterialData } from "@/api/produce/drillWorkOrder";

export default {
  name: "ProduceTasks",
  data() {
    return {
      loading: true,
      list: [],
      total: 0,
      form: {},
      open: false,
      queryParams: {
        pageSize: 10,
        pageNum: 1,
        workOrderCode: undefined,
        itemCode: undefined,
      },
    };
  },
  methods: {
    getList() {
      getMaterialData(this.queryParams).then((res) => {
        this.loading = true;
        this.list = res.data.produceTasks.list;
        this.total = res.data.produceTasks.total;
        this.loading = false;
      });
    },
    // 查询明细按钮操作
    handleView(row) {
      this.open = true;
    },
  },
};
</script>

<style lang="scss" scoped>
.table-pagination {
  margin-bottom: 20px;
}
</style>
