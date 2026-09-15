<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="任务编码" prop="taskCode">
        <el-input
          v-trim
          v-model="queryParams.taskCode"
          placeholder="请输入任务编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工单编码" prop="workOrderCode">
        <el-input
          v-trim
          v-model="queryParams.workOrderCode"
          placeholder="请输入工单编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="产品编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入产品编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="设备编码" prop="workStationCode">
        <el-input
          v-trim
          v-model="queryParams.workStationCode"
          placeholder="请输入设备编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="工艺路线" prop="routeCode">
        <el-select
          v-model="queryParams.routeCode"
          placeholder="请选择"
          @clear="clearQueryParams('routeCode')"
          clearable
        >
          <el-option
            v-for="item in routeQueryList"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          ></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="所属工序" prop="processCode">
        <el-select
          v-model="queryParams.processCode"
          placeholder="请选择"
          @clear="clearQueryParams('processCode')"
          clearable
        >
          <el-option
            v-for="item in processOptions"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          ></el-option>
        </el-select>
      </el-form-item>

      <el-form-item label="设备类别" prop="requestDeviceKindList">
        <el-select
          v-model="queryParams.requestDeviceKindList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.requestDeviceKindList &&
            queryParams.requestDeviceKindList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.drillKindList"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="设备状态" prop="deviceStatusList">
        <el-select
          v-model="queryParams.deviceStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.deviceStatusList &&
            queryParams.deviceStatusList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.deviceStatusOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="任务开始时间" prop="queryStartTime">
        <el-date-picker
          style="width: 185px"
          v-model="queryParams.queryStartTime"
          type="datetime"
          placeholder="选择日期时间"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="任务结束时间" prop="queryEndTime">
        <el-date-picker
          style="width: 185px"
          v-model="queryParams.queryEndTime"
          type="datetime"
          placeholder="选择日期时间"
        >
        </el-date-picker>
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
        :columns="columns"
        :page="page"
      ></right-toolbar>
    </el-row>

    <el-table
      border
      :ref="page"
      v-loading="loading"
      class="tableRowClassName"
      :data="workStationLoadTaskList"
    >
      <el-table-column
        label="设备编码"
        min-width="120"
        key="code"
        prop="code"
        fixed
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['device:deviceLoad:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="设备名称"
        key="name"
        prop="name"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="设备类别"
        align="center"
        key="deviceKind"
        min-width="120"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          {{
            scope.row.deviceKind &&
            $status.drillKindList.find(
              (v) => v.value == scope.row.deviceKind
            ) &&
            $status.drillKindList.find((v) => v.value == scope.row.deviceKind)
              .label
          }}
        </template>
      </el-table-column>
      <el-table-column
        label="设备状态"
        align="center"
        key="deviceStatus"
        prop="deviceStatus"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.deviceStatusOptions"
            :status="scope.row.deviceStatus"
          />
        </template>
      </el-table-column>

      <el-table-column
        label="任务数量"
        align="center"
        key="taskCount"
        prop="taskCount"
        v-if="columns[4].visible"
      />
      <el-table-column
        label="所属工序"
        key="processCode"
        prop="processCode"
        min-width="100"
        show-overflow-tooltip
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          {{
            scope.row.processCode && scope.row.processName
              ? scope.row.processCode + "---" + scope.row.processName
              : ""
          }}
        </template></el-table-column
      >
      <el-table-column
        label="工艺路线"
        min-width="120"
        key="routeCode"
        prop="routeCode"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />
    </el-table>

    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />
    <edit-form-dialog v-model="open" :title="title" optType="view" width="90%">
      <TaskList ref="taskList" />
    </edit-form-dialog>
  </div>
</template>

<script>
import { getWorkStationLoadTask } from "@/api/device/deviceLoad";
import { getDropSelectDatas as getProcessSelectDatas } from "@/api/produce/process";

import { getDropSelectDatas } from "@/api/produce/route";
import TaskList from "./taskList.vue";
export default {
  name: "DeviceLoad",
  components: { TaskList },
  data() {
    return {
      page: "DeviceLoad",
      optType: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 设备负载表格数据
      workStationLoadTaskList: [],
      processOptions: [],
      routeQueryList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        taskCode: undefined,
        workOrderCode: undefined,
        itemCode: undefined,
        workStationCode: undefined,
        processCode: undefined,
        routeCode: undefined,
        taskStatusList: [10],
        deviceStatusList: [],
        queryStartTime: undefined,
        queryEndTime: undefined,
      },
      // 表单参数
      form: {},
      // 创建时间
      dateRange: [],
      // 列信息，
      columns: [
        { key: 0, label: "设备编码", visible: true },
        { key: 1, label: "设备名称", visible: true },
        { key: 2, label: "设备类别", visible: true },
        { key: 3, label: "设备状态", visible: true },
        { key: 4, label: "任务数量", visible: true },
        { key: 5, label: "工序编码", visible: true },
        { key: 6, label: "工艺路线", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    this.getProcessList();
    this.getRouteList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  methods: {
    //查询工序信息
    getProcessList() {
      // 工序状态为正常的
      getProcessSelectDatas({ pageNum: 1, pageSize: 1000, status: 1 }).then(
        (res) => {
          this.processOptions = res.data;
        }
      );
    },
    // 查询工艺路线
    getRouteList() {
      getDropSelectDatas({
        vettingStatus: 1,
        pageNum: 1,
        pageSize: 100,
      }).then((res) => {
        this.routeQueryList = res.data?.map((v) => {
          return { ...v, label: `${v.code}(${v.name})` };
        });
      });
    },
    /** 查询设备负载列表 */
    async getList(isSearch) {
      this.loading = true;
      // this.queryParams.taskStatusList =
      //   this.$cache.local.getJSON("deviceLoad_taskStatusList");
      const res = await getWorkStationLoadTask(this.queryParams);
      this.workStationLoadTaskList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
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
      this.form = {
        code: undefined,
        name: undefined,
        remark: undefined,
      };
      this.autoGenFlag = false;
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      // 重置时时间范围也重置
      this.queryParams.taskStatusList = [];
      this.$cache.local.remove("deviceLoad_taskStatusList");
      this.queryParams.queryEndTime = undefined;
      this.queryParams.queryStartTime = undefined;
      this.dateRange = [];
      this.handleQuery();
    },
    // 选择任务状态
    handleSelectStatus(val) {
      this.$cache.local.setJSON("deviceLoad_taskStatusList", val);
    },
    // 选择创建时间
    changeDate(val) {
      if (val == null) {
        this.queryParams.queryStartTime = undefined;
        this.queryParams.queryEndTime = undefined;
      } else {
        this.queryParams.queryStartTime = val[0] && val[0];
        this.queryParams.queryEndTime = val[1] && val[1];
      }
    },
    // 查询明细按钮操作
    handleView(id) {
      const item = this.workStationLoadTaskList.find((v) => v.id == id);
      this.open = true;
      this.title = item.code + " " + "任务列表";
      this.$nextTick(() => {
        this.$refs.taskList.queryParams.workStationCode = item.code;
        this.$refs.taskList.queryParams.code = this.queryParams.taskCode;
        this.$refs.taskList.queryParams.workOrderCode =
          this.queryParams.workOrderCode;
        this.$refs.taskList.queryParams.itemCode = this.queryParams.itemCode;
        this.$refs.taskList.queryParams.taskStatusList =
          this.queryParams.taskStatusList;
        this.$refs.taskList.queryParams.queryStartTime =
          this.queryParams.queryStartTime;
        this.$refs.taskList.queryParams.queryEndTime =
          this.queryParams.queryEndTime;
        this.$refs.taskList.queryParams.routeCode = this.queryParams.routeCode;
        this.$refs.taskList.queryParams.processCode =
          this.queryParams.processCode;
        this.$refs.taskList.queryParams.pageNum = 1;
        this.$refs.taskList.getList();
      });
    },
  },
};
</script>
<style lang="scss" scoped>
::v-deep.el-table .warning-row {
  background: #d3d3d3 !important;
}
</style>
