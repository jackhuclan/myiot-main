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
        <el-button icon="el-icon-refresh" @click="resetQuery" v-debounce
          >重置</el-button
        >
      </el-form-item>
    </el-form>

    <el-table
      border
      v-loading="loading"
      :data="notifyList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedNotifyWaysId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
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
      selectedNotifyWaysId: undefined,
      selectedRow: undefined,
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工作站表格数据
      notifyList: [],
      ids: [],
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
    // 单选选中数据
    handleRowChange(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRow = row;
        this.$emit("onSelected", this.selectedRow);
        this.showFlag = false;
        this.selectedNotifyWaysId = undefined;
        this.selectedRow = undefined;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectedNotifyWaysId == null || this.selectedNotifyWaysId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedNotifyWaysId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
