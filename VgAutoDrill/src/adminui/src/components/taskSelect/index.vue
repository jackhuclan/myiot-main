<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="生产任务选择"
    :center="true"
    @submitForm="confirmSelect"
    @cancel="showFlag = false"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="任务编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入任务编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工序" prop="processCode">
        <el-select
          v-model="queryParams.processCode"
          placeholder="请选择工序"
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
      :data="protaskList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedTaskId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="任务编码"
        align="center"
        width="100px"
        prop="code"
        show-overflow-tooltip
      />
      <el-table-column
        label="任务名称"
        align="center"
        width="120px"
        prop="name"
        show-overflow-tooltip
      />
      <el-table-column
        label="工作站编码"
        align="center"
        width="150px"
        prop="workStationCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="工作站名称"
        align="center"
        width="150px"
        prop="workStationName"
        show-overflow-tooltip
      />
      <el-table-column label="排产数量" align="center" prop="quantity" />
      <el-table-column
        label="已生产数量"
        align="center"
        width="100px"
        prop="quantityProduced"
      />
      <el-table-column
        label="开始生产时间"
        align="center"
        prop="startTime"
        width="180"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.startTime, "{y}-{m}-{d} {h}") }}</span>
        </template>
      </el-table-column>
      <el-table-column label="生产时长" align="center" prop="duration" />
      <el-table-column
        label="预计完成时间"
        align="center"
        prop="endTime"
        width="180"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.endTime, "{y}-{m}-{d} {h}") }}</span>
        </template>
      </el-table-column>
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
import { listTask } from "@/api/produce/task";
import { listAllProcess } from "@/api/produce/routeAndProcess";
export default {
  name: "ProtaskSelect",
  props: {
    workOrderId: null,
    workOrderCode: null,
    processId: null,
    processCode: null,
    workstationId: null,
    workstationCode: null,
  },
  watch: {
    workOrderId(v) {
      this.queryParams.workOrderId = v;
    },
    showFlag(val) {
      if (!val) {
        this.resetForm("queryForm");
        this.selectedTaskId = undefined;
      }
    },
  },
  data() {
    return {
      processOptions: [],
      showFlag: false,
      selectedTaskId: undefined,
      selectedRow: undefined,
      // 遮罩层
      loading: true,

      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 生产任务表格数据
      protaskList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        itemCode: undefined,
        itemTypeId: undefined,
        batchCode: undefined,
        taskStatus: undefined,
        workOrderId: this.workOrderId,
        processId: this.processId,
        status: undefined,
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
  created() {
    this.getProcess();
  },
  methods: {
    /** 查询生产任务列表 */
    getList(isSearch) {
      this.loading = true;
      listTask(this.queryParams).then((res) => {
        this.protaskList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    //查询工序信息
    getProcess() {
      listAllProcess({}).then((res) => {
        this.processOptions = res.data.list?.map((v) => {
          return { ...v, label: v.code + "---" + v.name };
        });
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
        this.selectedTaskId = undefined;
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
      if (this.selectedTaskId == null || this.selectedTaskId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedTaskId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
