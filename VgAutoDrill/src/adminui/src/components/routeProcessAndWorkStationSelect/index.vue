<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="工作站选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <!-- 工艺流程页面 -->
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="工作站编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入工作站编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工作站名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入工作站名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="所在车间" prop="workshopName">
        <el-select
          v-model="queryParams.workshopName"
          placeholder="请选择车间"
          @clear="clearQueryParams('workshopName')"
          clearable
        >
          <el-option
            v-for="item in workShopOptions"
            :key="item.id"
            :value="item.name"
            :label="item.label"
            v-optionTitle
          ></el-option>
        </el-select>
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
        <el-button
          icon="el-icon-delete"
          type="danger"
          plain
          @click="removeShowData"
          >清除选中项</el-button
        >
      </el-form-item>
    </el-form>

    <el-table
      border
      v-loading="loading"
      width="100%"
      :data="workStationList"
      ref="multipleTable"
      @select-all="handelSelectAll"
      @select="handelSelect"
      @row-dblclick="rowDblclick"
      :row-style="echoRowStyle"
      :row-key="
        (row) => {
          return row.id;
        }
      "
    >
      <el-table-column
        type="selection"
        width="55"
        align="center"
        :reserve-selection="true"
      />
      <el-table-column
        min-width="150"
        label="工作站编码"
        prop="code"
        show-overflow-tooltip
      >
      </el-table-column>
      <el-table-column
        min-width="150"
        label="工作站名称"
        prop="name"
        show-overflow-tooltip
      />
      <el-table-column
        min-width="150"
        label="所在车间名称"
        prop="workshopName"
        show-overflow-tooltip
      />
      <el-table-column label="是否启用" align="center" prop="status">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        min-width="150"
        label="备注"
        prop="remark"
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
  </select-form-dialog>
</template>

<script>
import { listWorkstation } from "@/api/masterData/workStation";
import { getDropSelectDatas } from "@/api/masterData/workShop";
export default {
  name: "WorkStationSelect",
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      showData: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工作站表格数据
      workStationList: [],
      //车间选项
      workShopOptions: [],
      //工序选项
      processOptions: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        workshopId: undefined,
        workshopCode: undefined,
        workshopName: undefined,
        status: 1,
      },
    };
  },
  created() {
    this.getWorkShopList();
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.resetForm("queryForm");
      }
    },
  },
  methods: {
    /** 查询工作站列表 */
    getList(isSearch) {
      this.loading = true;
      listWorkstation(this.queryParams).then((res) => {
        this.workStationList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
        // 多选框回显
        this.$nextTick((res) => {
          this.workStationList.forEach((val) => {
            if (this.showData?.length && this.showData.includes(val.id)) {
              this.$refs.multipleTable?.toggleRowSelection(val, true);
            }
          });
        });
      });
    },
    // 获取车间列表
    getWorkShopList() {
      getDropSelectDatas({ pageNum: 1, pageSize: 1000 }).then((res) => {
        this.workShopOptions = res.data;
      });
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      // 获取表格对象
      let refsElTable = this.$refs.multipleTable;
      let findRow = this.showData.find((c) => c == row.rowId);
      //找到选中的行
      if (findRow) {
        this.showData = this.showData.filter((v) => v != row.id);
        refsElTable.toggleRowSelection(row, false); //如过重复选中，则取消选中
        return;
      }
      this.showData.push(row.id);
      refsElTable.toggleRowSelection(row, true); // 实现选中行中选中事件
    },

    //获取table组件中的全选checkbox的勾选状态
    getIsAllChecked() {
      return this.$refs.multipleTable.store.states.isAllSelected;
    },
    // 全选、反选
    handelSelectAll(selection) {
      if (this.getIsAllChecked()) {
        this.workStationList.forEach((v) => {
          if (!this.showData.includes(v.id)) {
            this.showData.push(v.id);
          }
        });
      } else {
        const workStationId = this.workStationList.map((v) => v.id);
        this.showData = this.showData.filter((v) => !workStationId.includes(v));
      }
    },
    // 单选
    handelSelect(selection, row) {
      if (this.showData.includes(row.id)) {
        this.showData = this.showData.filter((v) => v != row.id);
      } else {
        this.showData.push(row.id);
      }
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
    // 清除选中项
    removeShowData() {
      if (this.showData.length <= 0)
        return this.$modal.msgWarning("未选中任何数据!");
      this.showData = [];
      this.$refs.multipleTable.clearSelection();
    },
    //确定选中
    confirmSelect() {
      if (this.showData == [] || this.showData.length == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.showData);
    },
  },
};
</script>
