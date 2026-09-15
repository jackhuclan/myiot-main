<template>
  <div class="app-container">
    <el-table border :ref="page" v-loading="loading" :data="routeList">
      <el-table-column
        label="编码"
        min-width="150"
        fixed="left"
        prop="code"
        show-overflow-tooltip
      ></el-table-column>
      <el-table-column
        label="名称"
        prop="name"
        min-width="150"
        show-overflow-tooltip
      />
      <el-table-column
        label="说明"
        prop="routeDesc"
        min-width="150"
        show-overflow-tooltip
      />
      <el-table-column label="审批状态" align="center" prop="vettingStatus">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.vettingStatus == 1">已审批</el-tag>
          <el-tag type="danger" v-else>未审批</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="是否启用" align="center" prop="status">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="备注" width="150px" prop="routeRemark" />
    </el-table>

    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />
    <workStationAndRouteSelect ref="workStationAndRouteSelect" />
  </div>
</template>

<script>
import { getRoutesByWorkStation } from "@/api/masterData/workStation";
import workStationAndRouteSelect from "@/components/workStationAndRouteSelect";
export default {
  name: "ProrouteAndRoute", 
  components: { workStationAndRouteSelect },
  data() {
    return {
      page: "prorouteAndRoute",
      // 遮罩层
      loading: true,
      // 总条数
      total: 0,
      // 表格数据
      routeList: [],
      // 查询参数
      queryParams: {
        pageNum: 0,
        pageSize: 0,
        workStationId: 0,
        routeAndProcessId: 0,
      },
      processCode: undefined,
    };
  },
  methods: {
    /** 根据工序编码获取工艺路线结果 */
    async getList() {
      this.loading = true;
      const res = await getRoutesByWorkStation(this.queryParams);
      this.routeList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
    },
  },
};
</script>
