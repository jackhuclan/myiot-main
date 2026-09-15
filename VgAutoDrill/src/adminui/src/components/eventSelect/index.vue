<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="事件选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="事件名称" prop="eventName">
        <el-input
          v-trim
          v-model="queryParams.eventName"
          placeholder="请输入事件名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="事件级别" prop="eventLevel">
        <el-select
          @clear="clearQueryParams('eventLevel')"
          v-model="queryParams.eventLevel"
          placeholder="事件级别"
          clearable
        >
          <el-option
            v-for="dict in $status.alarmLevelOptions"
            :key="dict.value"
            :label="dict.label"
            :value="dict.value"
          />
        </el-select>
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
      :data="eventList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center" fixed="left">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedEventId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="Id"
        align="center"
        width="55"
        prop="id"
        fixed="left"
      />
      <el-table-column
        label="事件编码"
        fixed="left"
        min-width="300px"
        prop="eventId"
        show-overflow-tooltip
      />
      <el-table-column
        label="事件名称"
        min-width="200px"
        prop="eventName"
        show-overflow-tooltip
      />
      <el-table-column label="事件级别" align="center">
        <template slot-scope="scope">
          <el-tag type="danger" v-if="scope.row.eventLevel == 3">紧急</el-tag>
          <el-tag type="warning" v-else-if="scope.row.eventLevel == 2"
            >严重</el-tag
          >
          <el-tag v-else>普通</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="参数配置"
        align="center"
        prop="parameterJson"
        show-overflow-tooltip
      />

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
import { listAlarmEvent } from "@/api/alarm/alarmEvent";

export default {
  name: "EventSelect", 
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      selectedEventId: undefined,
      selectedRows: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工作站表格数据
      eventList: [],
      // 弹出层标题
      title: "",
      // 查询参数
      queryParams: {
        pageNum: 0,
        pageSize: 10,
        eventName: undefined,
        eventLevel: undefined,
        deviceTypeId: undefined,
        status: undefined,
      },
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
    async getList(isSearch) {
      this.loading = true;
      const res = await listAlarmEvent(this.queryParams);
      this.eventList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 切换事件等级
    handleEventLevel(val) {
      this.eventLevel = this.$status.alarmLevelOptions.filter(
        (v) => val == v.value
      )[0].label;
      this.form.eventLevel = val * 1;
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
        this.selectedEventId = undefined;
        this.selectedRow = undefined;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectedEventId == null || this.selectedEventId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRows);
      this.showFlag = false;
      this.selectedEventId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
