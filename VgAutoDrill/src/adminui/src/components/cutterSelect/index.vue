<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="刀具选择"
    :center="true"
    @submitForm="confirmSelect"
    @cancel="showFlag = false"
  >
    <el-table
      border
      v-loading="loading"
      :data="dirllList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedCutterId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column label="钻带参数文件明细编码" align="center" prop="code">
      </el-table-column>
      <el-table-column label="直径" align="center" prop="diameter" />
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
import { listItemDrillFileDetail } from "@/api/material/itemDrillFileDetail";
export default {
  name: "CutterSelect", 
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      selectedCutterId: undefined,
      selectedRows: [],
      // 总条数
      total: 0,
      // 工作站表格数据
      dirllList: [],
      // 弹出层标题
      title: "",
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        itemDrillFileId: undefined,
        code: undefined,
        diameter: undefined,
      },
    };
  },
  created() {
    // this.getList();
  },
  methods: {
    /** 查询工作站列表 */
    getList() {
      this.loading = true;
      listItemDrillFileDetail(this.queryParams).then((response) => {
        this.dirllList = response.data.list;
        this.total = response.data.total;
        this.loading = false;
      });
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
        this.selectedCutterId = undefined;
        this.selectedRow = undefined;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectedCutterId == null || this.selectedCutterId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRows);
      this.showFlag = false;
      this.selectedCutterId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
