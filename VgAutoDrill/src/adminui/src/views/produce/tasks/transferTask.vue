el-tab-pane
<template>
  <edit-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="转发任务"
    @submitForm="submitForm"
    @cancel="showFlag = false"
  >
    <el-descriptions title="关联工艺路线" :column="2" border>
      <el-descriptions-item label="工艺路线编码">{{
        tasks[0].routeCode
      }}</el-descriptions-item>
      <el-descriptions-item label="工艺路线名称">{{
        tasks[0].routeName
      }}</el-descriptions-item>
    </el-descriptions>
    <el-tabs
      type="border-card"
      style="margin-top: 10px"
      @tab-click="handleTabClick"
    >
      <el-tab-pane label="操作任务">
        <el-table v-loading="loading" :data="tasks" border>
          <el-table-column
            label="任务编码"
            min-width="150px"
            align="center"
            prop="code"
            show-overflow-tooltip
          >
          </el-table-column>
          <el-table-column
            label="工单编码"
            min-width="150px"
            align="center"
            prop="workOrderCode"
            show-overflow-tooltip
          />
          <el-table-column
            label="工序编码"
            min-width="150px"
            align="center"
            prop="processCode"
            show-overflow-tooltip
          />
          <el-table-column
            label="工艺路线"
            min-width="150px"
            align="center"
            prop="routeCode"
            show-overflow-tooltip
          />
          <el-table-column
            label="工作站编码"
            min-width="150px"
            align="center"
            prop="workStationCode"
            show-overflow-tooltip
          />
        </el-table>
      </el-tab-pane>
      <el-tab-pane label="工作站">
        <el-form
          :model="queryParams"
          ref="queryForm"
          :inline="true"
          v-show="showSearch"
        >
          <el-form-item label="工作站状态" prop="deviceStatusList">
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
          :data="workStationList"
          @current-change="handleCurrent"
          @row-dblclick="handleRowDbClick"
        >
          <el-table-column width="55" align="center">
            <template v-slot="scope">
              <el-radio
                v-model="selectedWorkStationId"
                :label="scope.row.id"
                :disabled="
                  tasks.map((v) => v.workStationId).includes(scope.row.id)
                "
                @change="handleRowChange(scope.row)"
                >{{ "" }}</el-radio
              >
            </template>
          </el-table-column>
          <el-table-column
            label="工作站编码"
            min-width="150px"
            align="center"
            prop="code"
            show-overflow-tooltip
          >
          </el-table-column>
          <el-table-column
            label="工作站名称"
            min-width="150px"
            align="center"
            prop="name"
            show-overflow-tooltip
          />
          <el-table-column
            label="待做任务"
            min-width="100px"
            align="center"
            prop="toDoTaskCount"
            show-overflow-tooltip
          />
          <el-table-column label="工作站状态" align="center">
            <template slot-scope="scope">
              <status-tag
                :options="$status.deviceStatusOptions"
                :status="scope.row.deviceStatus"
              />
            </template>
          </el-table-column>
          <el-table-column
            label="开始空闲时间"
            min-width="150px"
            align="center"
            prop="beginFreeTime"
            show-overflow-tooltip
          />
        </el-table>
        <pagination
          v-show="total > 0"
          :total="total"
          :page.sync="queryParams.pageNum"
          :limit.sync="queryParams.pageSize"
          @pagination="getList"
        />
      </el-tab-pane>
    </el-tabs>
  </edit-form-dialog>
</template>

<script>
import { getFitWorkStationListByRoute } from "@/api/produce/routeProcessAndWorkStation";
export default {
  name: "TransferTask", 
  props: ["tasks"],
  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedWorkStationId = 0;
        this.resetForm("queryForm");
      }
    },
  },
  data() {
    return {
      showFlag: false,
      selectedWorkStationId: undefined,
      selectedRow: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      //表格数据
      workStationList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        routeCode: undefined,
        processCode: undefined,
        deviceStatusList: [],
      },
      // 表单参数
      form: {},
      // 表单校验
      rules: {
        workstationId: [
          { required: true, message: "工作站不能为空", trigger: "blur" },
        ],
        quantity: [
          { required: true, message: "排产数量不能为空", trigger: "blur" },
        ],
        startTime: [
          { required: true, message: "请选择开始生产日期", trigger: "blur" },
        ],
        duration: [
          { required: true, message: "清输入估算的生产用时", trigger: "blur" },
        ],
      },
    };
  },
  methods: {
    // 之前工作站禁用
    disabledWorkStation(id) {
      let flag = false;
      this.tasks.forEach((v) => {
        if (v.workStationId == id) {
          flag = true;
        } else {
          flag = false;
        }
      });
      return flag;
    },
    getList(isSearch) {
      this.loading = true;
      getFitWorkStationListByRoute(this.queryParams).then((res) => {
        this.workStationList = res.data.list;
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
      this.queryParams.code = this.queryParams.code?.trim();
      this.queryParams.name = this.queryParams.name?.trim();
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
      if (this.tasks.map((v) => v.workStationId).includes(row.id))
        return this.$modal.msgWarning("该项不可操作，请选择其他数据!");

      if (row) {
        this.selectedRow = row;
        this.$emit("onSelected", this.selectedRow);
        this.showFlag = false;
        this.selectedWorkStationId = undefined;
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
    submitForm() {
      if (
        this.selectedWorkStationId == null ||
        this.selectedWorkStationId == 0
      ) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请选择一条工作站数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedWorkStationId = undefined;
      this.selectedRow = undefined;
    },
    // 点击tab切花
    handleTabClick(tab) {
      if (tab.label == "工作站") {
        this.getList();
      }
    },
  },
};
</script>
