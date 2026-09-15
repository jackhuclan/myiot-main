<template>
  <div>
    <el-table :data="list" style="width: 100%" v-loading="loading" border>
      <el-table-column
        prop="materialStockCode"
        label="入库单号"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          <span class="click_code" :data-id="scope.row.id"   v-isGetSelection  >{{
            scope.row.materialStockCode
          }}</span>
        </template>
      </el-table-column>
      <el-table-column prop="batchCode" label="批号" show-overflow-tooltip>
      </el-table-column>
      <el-table-column prop="quantityOnhand" align="center" label="数量">
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
    <edit-form-dialog v-model="open" title="查看物料库存信息" optType="view">
      <el-form ref="form" :model="form"> </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import { getMaterialData } from "@/api/produce/drillWorkOrder";

export default {
  name: "Storages",
  data() {
    return {
      loading: true,
      list: [],
      total: 0,
      open: false,
      form: {},
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
        this.list = res.data.storages.list;
        this.total = res.data.storages.total;
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
