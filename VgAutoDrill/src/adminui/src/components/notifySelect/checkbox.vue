<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="通知方式选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="通知类型编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入通知类型编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="通知类型名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入通知类型名称"
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
        <el-button
          icon="el-icon-refresh"
          @click="resetQuery"
          v-debounce
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
      :data="notifyList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="echoRowStyle"
      ref="multipleTable"
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
        label="通知类型编码"
        min-width="150"
        prop="code"
        show-overflow-tooltip
      />
      <el-table-column
        label="通知类型名称"
        min-width="150"
        prop="name"
        show-overflow-tooltip
      />

      <el-table-column
        label="通知类型描述"
        min-width="150"
        prop="notifyDesc"
        show-overflow-tooltip
      />
      <!-- <el-table-column label="通知方式" align="center" prop="notifyWays" /> -->
      <!-- <el-table-column label="通知参数" align="center" prop="notifyParams" /> -->

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
import { listNotifySetting } from "@/api/notify/notifySetting";

export default {
  name: "EventSelect", 
  data() {
    return {
      // 判断告警页面调用还是通知记录页面调用单选、多选
      type: "",
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      selectedRows: [],
      showData: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工作站表格数据
      notifyList: [],
      // 弹出层标题
      title: "",
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        notifyDesc: undefined,
        notifyWays: undefined,
        notifyParams: undefined,
        status: undefined,
      },
      // 事件级别
      eventLevelOptions: [
        {
          value: "1",
          label: "普通",
        },
        {
          value: "2",
          label: "严重",
        },
        {
          value: "3",
          label: "紧急",
        },
      ],
      eventLevel: "",
    };
  },
  watch: {
    // 监听回显数据
    ids(nv, ov) {
      this.getList(nv);
    },
    showFlag(val) {
      if (!val) {
        this.resetForm("queryForm");
      }
    },
  },
  methods: {
    /** 查询事件列表 */
    getList(isSearch) {
      this.loading = true;
      listNotifySetting(this.queryParams).then((res) => {
        this.notifyList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
        this.$nextTick((res) => {
          const showDataId = this.showData.map((v) => v * 1);
          this.notifyList.forEach((val) => {
            if (showDataId?.length && showDataId.includes(val.id)) {
              this.$refs.multipleTable?.toggleRowSelection(val, true);
            }
          });
        });
      });
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      // 获取表格对象
      let refsElTable = this.$refs.multipleTable;
      let findRow = this.selectedRows.find((c) => c.id == row.rowId);
      //找到选中的行
      if (findRow) {
        refsElTable.toggleRowSelection(row, false); //如过重复选中，则取消选中
        return;
      }
      refsElTable.toggleRowSelection(row, true); // 实现选中行中选中事件
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.selectedRows = selection;
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
      if (this.selectedRows.length <= 0)
        return this.$modal.msgWarning("未选中任何数据!");
      this.selectedRows = [];
      this.$refs.multipleTable.clearSelection();
    },
    //确定选中
    confirmSelect() {
      if (this.selectedRows.length == 0 || this.selectedRows == []) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRows);
      this.showFlag = false;
      this.selectedRows = [];
    },
  },
};
</script>
