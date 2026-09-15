<template>
  <div class="app-container">
    <el-card>
      <el-row :gutter="10" class="mb8" v-if="taskOptType != 'view'">
        <el-col :span="1.5">
          <el-button
            v-debounce
            type="primary"
            plain
            icon="el-icon-plus"
            @click="handleAdd"
            :disabled="hasPermi(['produce:schedule:addTask'])"
            >新增</el-button
          >
        </el-col>
        <el-col :span="1.5">
          <el-button
            v-debounce
            plain
            type="primary"
            icon="el-icon-check"
            @click="handleCommit"
            :disabled="hasPermi(['produce:schedule:commitTask'])"
            >批量提交</el-button
          >
        </el-col>
        <el-col :span="1.5">
          <el-button
            v-debounce
            plain
            type="warning"
            icon="el-icon-refresh-left"
            @click="handleRevoke"
            :disabled="hasPermi(['produce:schedule:revokeTask'])"
          >
            批量撤销
          </el-button>
        </el-col>
        <el-col :span="1.5">
          <el-button
            v-debounce
            type="danger"
            plain
            icon="el-icon-delete"
            @click="handleDelete"
            :disabled="hasPermi(['produce:schedule:removeTask'])"
            >批量删除</el-button
          >
        </el-col>
      </el-row>

      <el-table
        border
        :ref="page"
        v-loading="loading"
        :data="taskList"
        @selection-change="handleSelectionChange"
        @row-dblclick="rowDblclick"
        :row-style="rowStyle"
      >
        <el-table-column
          type="selection"
          width="55"
          align="center"
          v-if="taskOptType != 'view'"
        />
        <el-table-column
          label="任务编码"
          fixed="left"
          min-width="180px"
          prop="code"
          show-overflow-tooltip
        >
          <template slot-scope="scope">
            <span
              class="click_code"
              :data-id="scope.row.id"
              v-isGetSelection="['produce:schedule:viewTask']"
              >{{ scope.row.code }}</span
            >
          </template>
        </el-table-column>
        <el-table-column
          label="任务名称"
          min-width="180px"
          prop="name"
          show-overflow-tooltip
        />
        <el-table-column
          label="工作站编码"
          min-width="150px"
          prop="workStationCode"
          show-overflow-tooltip
        />
        <el-table-column
          label="工作站名称"
          min-width="150px"
          prop="workStationName"
          show-overflow-tooltip
        />
        <el-table-column label="任务状态" align="center" prop="taskStatus">
          <template slot-scope="scope">
            <status-tag
              :options="$status.taskOptions"
              :status="scope.row.taskStatus"
            />
          </template>
        </el-table-column>
        <el-table-column label="排产数量" align="center" prop="quantity" />
        <el-table-column
          label="开始生产时间"
          align="center"
          prop="startTime"
          width="180"
        >
          <template slot-scope="scope">
            <span>{{ scope.row.startTime }}</span>
          </template>
        </el-table-column>
        <el-table-column
          label="预计完成时间"
          align="center"
          prop="endTime"
          width="180"
        >
          <template slot-scope="scope">
            <span>{{ scope.row.endTime }}</span>
          </template>
        </el-table-column>
        <el-table-column label="显示颜色" align="center" prop="color">
          <template slot-scope="scope">
            <el-color-picker
              v-model="scope.row.color"
              disabled
            ></el-color-picker>
          </template>
        </el-table-column>

        <el-table-column
          label="操作"
          align="center"
          min-width="140px"
          fixed="right"
          class-name="small-padding fixed-width"
          v-if="taskOptType != 'view'"
        >
          <template slot-scope="scope">
            <el-button
              type="text"
              icon="el-icon-edit"
              @click="handleUpdate(scope.row)"
              :disabled="
                hasPermi(['produce:schedule:editTask']) ||
                scope.row.taskStatus == 1
              "
              >修改</el-button
            >

            <el-dropdown
              trigger="click"
              @command="(command) => handleCommand(command, scope.row)"
            >
              <span :class="'el-dropdown-link ' + $store.getters.size">
                <i class="el-icon-d-arrow-right el-icon--right"></i>更多
              </span>
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item
                  :disabled="
                    hasPermi(['produce:schedule:commitTask']) ||
                    scope.row.taskStatus == 10
                  "
                  command="handleCommit"
                  icon="el-icon-check"
                  >提交</el-dropdown-item
                >
                <el-dropdown-item
                  :disabled="
                    hasPermi(['produce:schedule:revokeTask']) ||
                    scope.row.taskStatus != 10
                  "
                  command="handleRevoke"
                  icon="el-icon-refresh-left"
                  >撤销</el-dropdown-item
                >
                <el-dropdown-item
                  command="handleDelete"
                  icon="el-icon-delete"
                  :disabled="hasPermi(['produce:schedule:removeTask'])"
                  >删除</el-dropdown-item
                >
              </el-dropdown-menu>
            </el-dropdown>
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

      <!-- 添加或修改生产任务对话框 -->
      <edit-form-dialog
        v-model="open"
        :title="title"
        :optType="optType"
        :isFormModified="isFormModified"
        @submitForm="submitForm"
      >
        <el-form
          ref="form"
          :model="form"
          label-width="100px"
          :rules="rules"
          :disabled="optType == 'view'"
        >
          <el-row>
            <el-col :span="10">
              <el-form-item label="任务编码" prop="code">
                <el-input
                  v-model="form.code"
                  placeholder="请输入任务编码"
                  :disabled="autoGenFlag || optType != 'add'"
                />
              </el-form-item>
            </el-col>
            <el-col :span="4">
              <el-form-item label-width="80">
                <el-switch
                  v-model="autoGenFlag"
                  active-color="#13ce66"
                  active-text="自动生成"
                  @change="handleAutoGenChange(autoGenFlag)"
                  :disabled="optType != 'add'"
                >
                </el-switch>
              </el-form-item>
            </el-col>
            <el-col :span="10">
              <el-form-item label="任务名称" prop="name">
                <el-input v-model="form.name" placeholder="请输入任务名称" />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item label="甘特图颜色" prop="color">
                <el-input
                  placeholder="请输入颜色编码"
                  v-model="form.color"
                  maxlength="7"
                  class="color-input"
                >
                  <template slot="prepend"
                    ><el-color-picker v-model="form.color"></el-color-picker>
                  </template>
                </el-input>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="工作站" prop="workStationName">
                <el-input
                  v-model="form.workStationName"
                  placeholder="请选择工作站"
                >
                  <el-button
                    v-debounce
                    slot="append"
                    icon="el-icon-search"
                    @click="handleSelectWorkStation"
                  ></el-button>
                </el-input>
              </el-form-item>

              <WorkStationSelect
                ref="wsSelect"
                @onSelected="onWorkStationSelected"
              />
            </el-col>
            <el-col :span="8">
              <el-form-item label="排产数量" prop="quantity">
                <!-- 引入自定义计数器组件 -->
                <input-number
                  :dis="optType == 'view'"
                  :myNum="form.quantity"
                  @changeNum="changeNum"
                  :numName="'quantity'"
                  :min="1"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item label="开始时间" prop="startTime">
                <el-date-picker
                  clearable
                  v-model="form.startTime"
                  @change="calculateEndTime"
                  type="datetime"
                  placeholder="开始时间"
                >
                </el-date-picker>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="耗时(分钟)" prop="duration">
                <el-input
                  v-model="form.duration"
                  placeholder="请输入任务编码"
                  disabled
                />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="完成时间" prop="endTime">
                <el-date-picker
                  clearable
                  disabled
                  v-model="form.endTime"
                  placeholder="完成时间"
                  type="datetime"
                >
                </el-date-picker>
              </el-form-item>
            </el-col>
          </el-row>
        </el-form>
      </edit-form-dialog>
    </el-card>
  </div>
</template>

<script>
import {
  listTaskByItemTypeId,
  getTask,
  delTask,
  addTask,
  updateTask,
  addCheckRecords,
  commitTask,
  revokeCommit,
  delList,
} from "@/api/produce/task";
import WorkStationSelect from "@/components/workStationSelect";
export default {
  name: "ProTask",
  components: { WorkStationSelect },
  data() {
    var quantityBtn = (rule, value, callback) => {
      if (value < 1) {
        callback(new Error("数量不得为0"));
      }
      callback();
    };
    return {
      page: "proTask",
      autoGenFlag: false,
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
      // 生产任务表格数据
      taskList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 步数器最大值
      max: 50,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        itemCode: undefined,
        itemTypeId: undefined,
        batchCode: undefined,
        taskStatusList: [],
        workOrderId: undefined,
        processId: undefined,
        processCode: undefined,
        requestDate: undefined,
        status: undefined,
        taskNumber: undefined,
        startDate: undefined,
        workStationCode: undefined,
        queryStartTime: undefined,
        queryEndTime: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        workStationName: [
          { required: true, message: "工作站不能为空", trigger: "change" },
        ],
        quantity: [{ validator: quantityBtn, trigger: "change" }],
        startTime: [
          { required: true, message: "请选择开始生产日期", trigger: "change" },
        ],
        duration: [
          { required: true, message: "请输入估算的生产用时", trigger: "blur" },
        ],
        name: [
          { required: true, message: "任务名称不能为空", trigger: "change" },
        ],
        code: [
          { required: true, message: "任务编码不能为空", trigger: "change" },
        ],
      },
    };
  },
  props: ["workOrderForm", "processForm", "taskOptType"],
  created() {
    this.getList();
  },
  watch: {
    "form.workStationName": {
      handler(val) {
        if (!val) {
          this.form.workStationId = undefined;
          this.form.workStationName = undefined;
          this.form.workStationCode = undefined;
        }
      },
      deep: true,
    },
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询生产任务列表 */
    getList() {
      this.loading = true;
      listTaskByItemTypeId({
        ...this.queryParams,
        workOrderId: this.workOrderForm.id,
        processId: this.processForm.processId,
        itemTypeId: this.workOrderForm.itemTypeId,
      }).then((res) => {
        this.taskList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
      });
    },
    //计算结束时间
    calculateEndTime() {
      if (this.form.startTime && this.form.quantity) {
        const date = this.form.startTime;
        const endTime = new Date(
          new Date(date).getTime() +
            // this.processForm.requiredTime * 60000 * this.form.quantity
            this.processForm.requiredTime * 60000
        );
        this.form.endTime = new Date(endTime).toLocaleString("sv-SE");
      } else {
        this.form.endTime = "";
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
        name: "",
        code: "",
        parentId: undefined,
        ancestors: undefined,
        workOrderId: this.workOrderForm.id,
        workOrderName: this.workOrderForm.name,
        workOrderCode: this.workOrderForm.code,
        workStationId: undefined,
        workStationName: undefined,
        workStationCode: undefined,
        processId: this.processForm.processId,
        processName: this.processForm.processName,
        processCode: this.processForm.processCode,
        itemId: this.workOrderForm.itemId,
        itemName: this.workOrderForm.itemName,
        itemCode: this.workOrderForm.itemCode,
        itemTypeId: this.workOrderForm.itemTypeId,
        batchCode: this.workOrderForm.batchCode,
        specification: this.workOrderForm.specification,
        unitOfMeasure: this.workOrderForm.unitOfMeasure,
        panelCount: this.workOrderForm.panelCount,
        quantity: 0,
        quantityChanged: undefined,
        quantityProduced: undefined,
        quantityQuanlify: 0,
        quantityUnquanlify: 0,
        clientId: this.workOrderForm.clientId,
        clientName: this.workOrderForm.clientCode,
        clientCode: this.workOrderForm.clientCode,
        startTime: new Date(),
        duration: 0,
        endTime: undefined,
        requestDate: this.workOrderForm.requestDate,
        color: this.processForm.color,
        keyFlag: this.processForm.keyFlag,
      };
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList();
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.resetForm("queryForm");
      this.handleQuery();
    },

    // 点击查看
    handleView(id) {
      this.reset();
      getTask(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.duration = this.processForm.requiredTime;
          this.open = true;
          this.title = "查看生产任务";
          this.optType = "view";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.title = "添加生产任务";
      this.optType = "add";
      this.autoGenFlag = false;
      this.form.duration = this.processForm.requiredTime;
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const taskId = row.id || this.ids;
      getTask(taskId).then((res) => {
        if (res.code == 0) {
          const startTime = res.data.startTime;
          const endTime = res.data.endTime;
          this.form = {
            ...res.data,
            startTime,
            endTime,
          };
          this.enCode = res.data.code;
          this.enName = res.data.name;
          this.open = true;
          this.optType = "edit";
          this.title = "修改生产任务";
          this.initialForm = Object.assign({}, res.data);
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleCommit":
          this.handleCommit(row);
          break;
        case "handleRevoke":
          this.handleRevoke(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateTask(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            // this.$emit("father_getGanttTasks");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addTask(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            // this.$emit("father_getGanttTasks");
            this.open = false;
            this.queryParams.pageNum = 1;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 点击提交按钮
    async handleCommit(row) {
      if (!row.id && this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行提交？")
        .catch(() => {});
      const commitIds = row.id ? [row.id] : this.ids;
      if (results == "confirm") {
        commitTask({
          ids: commitIds,
          taskStatus: 10,
        }).then((res) => {
          if (res.code == 0) {
            this.getList();
            this.$modal.msgSuccess("提交成功");
            this.open = false;
            this.$emit("father_getList");
            this.$emit("handleUpdate");
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 点击撤销提交
    async handleRevoke(row) {
      if (!row.id && this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行撤销？")
        .catch(() => {});
      const commitIds = row.id ? [row.id] : this.ids;
      if (results == "confirm") {
        revokeCommit({
          ids: commitIds,
          taskStatus: 0,
        }).then((res) => {
          if (res.code == 0) {
            this.getList();
            this.$modal.msgSuccess("撤销成功");
            this.open = false;
            // this.$emit("father_getList");
            this.$emit("handleUpdate");
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 点击计数器
    changeNum(params) {
      this.form[params.str] = params.value;
      this.calculateEndTime();
    },
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delTask, this.getList, "任务编码为" + row.code);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },

    // 生成检验记录
    handleAddCheckRecords() {
      const form = {
        taskId: this.form.id,
        taskCode: this.form.code,
        taskName: this.form.name,
        workOrderId: this.form.workOrderId,
        workOrderCode: this.form.workOrderCode,
        workOrderName: this.form.workOrderName,
        isCheckOk: -1,
      };
      addCheckRecords(form).then((res) => {
        this.$modal.msgSuccess("操作成功");
        this.open = false;
      });
    },
    // 选择工作站
    handleSelectWorkStation() {
      this.$refs.wsSelect.showFlag = true;
      this.$refs.wsSelect.selectedWorkStationId = this.form.workStationId
        ? this.form.workStationId
        : undefined;
      this.$refs.wsSelect.queryTaskParams = {
        ...this.$refs.wsSelect.queryTaskParams,
        routeAndProcessId: this.processForm.id,
      };

      this.$refs.wsSelect.taskFlag = true;
      this.$refs.wsSelect.getList();
    },
    onWorkStationSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "workStationId", row.workStationId);
        this.$set(this.form, "workStationCode", row.workStationCode);
        this.$set(this.form, "workStationName", row.workStationName);
      }
    },
    //自动生成码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "TASK_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
          this.form.name = code;
        });
      } else {
        if (this.optType == "edit") {
          this.form.code = this.enCode;
          this.form.name = this.enName;
          return;
        }
        this.form.code = "";
        this.form.name = "";
      }
    },
  },
};
</script>
