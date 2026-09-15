<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="库位选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
      @submit.native.prevent
    >
      <el-form-item label="库位" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入库位编码"
          @keyup.enter.native="handleQuery"
        ></el-input>
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
      :data="rackManageList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedRackId"
            :label="scope.row.code"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="库位编号"
        prop="code"
        show-overflow-tooltip
        min-width="150"
      />
      <el-table-column
        label="分区"
        prop="wareHouseCode"
        min-width="150"
        show-overflow-tooltip
      />
      <el-table-column
        label="料仓"
        prop="siloCode"
        min-width="150"
        show-overflow-tooltip
      />
      <el-table-column label="内点" prop="innerPoint" />
      <el-table-column label="外点" prop="outPoint" />
      <el-table-column
        label="设备"
        prop="relateDeviceCode"
        min-width="100"
        show-overflow-tooltip
      />
      <el-table-column label="是否可用" prop="status" align="center">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status"> 是 </el-tag>
          <el-tag v-else type="danger">否</el-tag>
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
import { listRack } from "@/api/wareHouse/rack";

export default {
  name: "rackManageSelect", 
  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedSiloId = 0;
        this.resetForm("queryForm");
      }
    },
  },
  data() {
    return {
      showFlag: false,
      selectedRackId: undefined,
      selectedRow: undefined,
      // 遮罩层
      loading: true,

      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 料仓表格数据
      rackManageList: [],
      // 弹出层标题
      title: "",

      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        status: 1,
      },
    };
  },
  methods: {
    /** 查询客户列表 */
    getList(isSearch) {
      this.loading = true;
      listRack(this.queryParams).then((res) => {
        this.rackManageList = res.data.list;
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
        this.selectedRackId = undefined;
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
      if (this.selectedRackId == null || this.selectedRackId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedRackId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
