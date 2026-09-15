<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="报工类型" prop="feedBackType">
        <el-select
          v-model="queryParams.feedBackType"
          placeholder="请选择报工类型"
          clearable
          @lear="clearQueryParams('feedBackType')"
          style="width: 150px"
        >
          <el-option :value="'自行报工'">自行报工</el-option>
          <el-option :value="'统一报工'">统一报工</el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="工作站" prop="workStationName">
        <el-input
          v-trim
          v-model="queryParams.workStationName"
          placeholder="请输入工作站"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工单编码" prop="workOrderCode">
        <el-input
          v-trim
          v-model="queryParams.workOrderCode"
          placeholder="请输入生产工单编码"
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
    </search-form>
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['produce:reportRecords:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          plain
          type="warning"
          icon="el-icon-check"
          @click="handleApproval"
          :disabled="hasPermi(['produce:reportRecords:commit'])"
          >批量审批</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          plain
          type="warning"
          icon="el-icon-refresh-left"
          @click="handleRevoke"
          :disabled="hasPermi(['produce:reportRecords:revoke'])"
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
          :disabled="hasPermi(['produce:reportRecords:remove'])"
          >批量删除</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['produce:reportRecords:export'])"
          >导出</el-button
        >
      </el-col>
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
      :data="feedbackList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="记录Id"
        fixed="left"
        align="center"
        key="id"
        prop="id"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['produce:reportRecords:view']"
            >{{ scope.row.id }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="任务编码"
        min-width="200"
        fixed="left"
        key="taskCode"
        prop="taskCode"
        show-overflow-tooltip
        v-if="columns[1].visible"
      >
      </el-table-column>
      <el-table-column
        label="工单编码"
        min-width="200"
        key="workOrderCode"
        prop="workOrderCode"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />

      <el-table-column
        label="报工类型"
        align="center"
        key="feedBackType"
        prop="feedBackType"
        v-if="columns[3].visible"
      />
      <el-table-column
        label="工作站"
        min-width="120px"
        key="workStationCode"
        prop="workStationCode"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />

      <el-table-column
        label="产品编码"
        min-width="200px"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        label="规格型号"
        align="center"
        key="specification"
        prop="specification"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[6].visible"
      />
      <el-table-column
        label="本次报工数量"
        min-width="120px"
        align="center"
        key="quantityFeedBack"
        prop="quantityFeedBack"
        v-if="columns[7].visible"
      />
      <el-table-column
        label="报工人"
        align="center"
        key="userName"
        prop="userName"
        show-overflow-tooltip
        v-if="columns[8].visible"
      />
      <el-table-column
        label="报工时间"
        align="center"
        key="feedBackTime"
        prop="feedBackTime"
        min-width="180"
        v-if="columns[9].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.feedBackTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="审核人"
        align="center"
        key="nickName"
        prop="nickName"
        show-overflow-tooltip
        v-if="columns[10].visible"
      />
      <el-table-column
        label="审批状态"
        align="center"
        key="feedBackStatus"
        prop="feedBackStatus"
        v-if="columns[11].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.feedBackStatus == 'COMMITED'">已完成</el-tag>
          <el-tag v-else type="danger">草稿</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        align="center"
        key="remark"
        prop="remark"
        min-width="150px"
        v-if="columns[12].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="
              hasPermi(['produce:reportRecords:edit']) ||
              scope.row.feedBackStatus == 'COMMITED'
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
                command="handleRevoke"
                :disabled="
                  hasPermi(['produce:reportRecords:revoke']) ||
                  scope.row.feedBackStatus != 'COMMITED'
                "
                icon="el-icon-refresh-left"
                >撤销</el-dropdown-item
              >
              <el-dropdown-item
                command="handleApproval"
                icon="el-icon-check"
                :disabled="
                  hasPermi(['produce:reportRecords:commit']) ||
                  scope.row.feedBackStatus == 'COMMITED'
                "
                >审批</el-dropdown-item
              >

              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="
                  hasPermi(['produce:reportRecords:remove']) ||
                  scope.row.feedBackStatus == 'COMMITED'
                "
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
    />

    <!-- 添加或修改生产报工记录对话框 -->
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
        :rules="rules"
        label-width="100px"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="8">
            <el-form-item label="报工类型">
              <el-select
                v-model="form.feedBackType"
                placeholder="请选择报工类型"
                clearable
              >
                <el-option :value="'自行报工'">自行报工</el-option>
                <el-option :value="'统一报工'">统一报工</el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="生产工单" prop="workOrderCode">
              <el-input
                disabled
                v-model="form.workOrderCode"
                placeholder="请选择生产工单"
              >
              </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="生产任务" prop="taskCode">
              <el-input
                v-model="form.taskCode"
                disabled
                placeholder="请选择生产任务"
              >
              </el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="产品编码" prop="itemCode">
              <el-input v-model="form.itemCode" disabled />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="产品名称" prop="itemName">
              <el-input v-model="form.itemName" disabled />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="单位" prop="unitOfMeasure">
              <el-input
                v-model="form.unitOfMeasure"
                disabled
                placeholder="请选择工单"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="规格型号" prop="specification">
              <el-input
                v-model="form.specification"
                disabled
                placeholder="请选择工单"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="排产数量" prop="quantity">
              <el-input
                v-model="form.quantity"
                disabled
                placeholder="请输入排产数量"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="报工数量" prop="quantityFeedBack">
              <el-input
                v-model="form.quantityFeedBack"
                disabled
                placeholder="请输入报工数量"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="良品数量" prop="quantityQualified">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.quantityQualified"
                @changeNum="changeNum"
                :numName="'quantityQualified'"
                :dis="optType == 'view'"
                :min="0"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="不良品数量" prop="quantityUnQuanlified">
              <input-number
                :myNum="form.quantityUnQuanlified"
                @changeNum="changeNum"
                :numName="'quantityUnQuanlified'"
                :dis="optType == 'view'"
                :min="0"
              ></input-number>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="报工人" prop="userName">
              <el-input v-model="form.userName" placeholder="请选择报工人">
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleUserSelect"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
            </el-form-item>
            <UserSingleSelect
              ref="userSelect"
              @onSelected="onUserSelected"
            ></UserSingleSelect>
          </el-col>
          <el-col :span="8">
            <el-form-item label="报工时间" prop="feedBackTime">
              <el-date-picker
                clearable
                v-model="form.feedBackTime"
                type="datetime"
                placeholder="选择日期时间"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="审核人" prop="nickName">
              <el-input v-model="form.nickName" placeholder="请选择审核人">
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleUser2Select"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
            </el-form-item>
            <UserSingleSelect
              ref="user2Select"
              @onSelected="onUser2Selected"
            ></UserSingleSelect>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="备注" prop="remark">
              <el-input
                v-model="form.remark"
                type="textarea"
                placeholder="请输入内容"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listFeedback,
  getFeedback,
  delFeedback,
  addFeedback,
  updateFeedback,
  commitFeedback,
  revokeCommit,
  delList,
} from "@/api/produce/reportWork";
// 工单选择
import WorkOrderSelect from "@/components/workOrderSelect";
// 报工人及审核人选择
import UserSingleSelect from "@/components/userSelect";
// 生产任务选择
import ProTaskSelect from "@/components/taskSelect";
export default {
  name: "ReportRecords",
  components: {
    WorkOrderSelect,
    UserSingleSelect,
    ProTaskSelect,
  },
  data() {
    // 自定义校验规则
    const validateRule = (rule, value, callback) => {
      if (value <= 0) {
        callback("报工数量不能为0");
      } else {
        callback();
      }
    };
    return {
      page: "reportRecords",
      optType: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 生产报工记录表格数据
      feedbackList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        feedBackType: undefined,
        workStationCode: undefined,
        workStationName: undefined,
        workOrderCode: undefined,
        workOrderName: undefined,
        processCode: undefined,
        processName: undefined,
        taskCode: undefined,
        itemCode: undefined,
        itemName: undefined,
        itemTypeId: undefined,
        feedBackStatus: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        feedBackType: [
          { required: true, message: "报工类型不能为空", trigger: "change" },
        ],
        taskCode: [
          { required: true, message: "请选择生产任务", trigger: "blur" },
        ],
        workOrderCode: [
          { required: true, message: "生产工单不能为空", trigger: "blur" },
        ],
        quantityQualified: [
          { required: true, message: "请输入良品数量", trigger: "change" },
        ],
        quantityUnQuanlified: [
          { required: true, message: "请输入不良品数量", trigger: "change" },
        ],
        quantityFeedBack: [
          {
            required: true,
            validator: validateRule,
            trigger: "change",
          },
        ],
        userName: [
          { required: true, message: "请选择报工人", trigger: "change" },
        ],
        feedBackTime: [
          { required: true, message: "请选择报工时间", trigger: "change" },
        ],
      },
      columns: [
        { key: 0, label: "记录Id", visible: true },
        { key: 1, label: "任务编码", visible: true },
        { key: 2, label: "工单编码", visible: true },
        { key: 3, label: "报工类型", visible: true },
        { key: 4, label: "工作站", visible: true },
        { key: 5, label: "产品编码", visible: true },
        { key: 6, label: "规格型号", visible: true },
        { key: 7, label: "本次报工数量", visible: true },
        { key: 8, label: "报工人", visible: true },
        { key: 9, label: "报工时间", visible: true },
        { key: 10, label: "审核人", visible: true },
        { key: 11, label: "审批状态", visible: true },
        { key: 12, label: "备注", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    "form.userName": {
      handler(val) {
        if (val == "") {
          this.form.userName = undefined;
        }
      },
      deep: true,
    },
    "form.nickName": {
      handler(val) {
        if (val == "") {
          this.form.nickName = undefined;
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
    /** 查询生产报工记录列表 */
    async getList(isSearch) {
      this.loading = true;
      // 生产报工
      const res = await listFeedback(this.queryParams);
      this.feedbackList = res.data.list;
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
        id: undefined,
        status: undefined,
        feedBackType: undefined,
        workStationId: undefined,
        workStationCode: undefined,
        workStationName: undefined,
        workOrderId: undefined,
        workOrderCode: undefined,
        workOrderName: undefined,
        processId: undefined,
        processCode: undefined,
        processName: undefined,
        taskId: undefined,
        taskCode: undefined,
        itemTypeId: undefined,
        itemId: undefined,
        itemCode: undefined,
        itemName: undefined,
        unitOfMeasure: undefined,
        specification: undefined,
        quantity: undefined,
        quantityFeedBack: undefined,
        quantityQualified: 0,
        quantityUnQuanlified: 0,
        userName: this.name,
        nickName: undefined,
        feedBackChannel: undefined,
        feedBackTime: new Date(),
        remark: undefined,
      };
      this.optType = undefined;
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.title = "添加生产报工记录";
      this.optType = "add";
      this.initialForm = Object.assign(
        {},
        {
          quantityQualified: 0,
          quantityUnQuanlified: 0,
          userName: this.name,
          feedBackTime: new Date(),
        }
      );
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const recordId = row.id || this.ids;
      getFeedback(recordId).then((res) => {
        if (res.code == 0) {
          this.form = {
            ...res.data,
            remark: res.data.remark ? res.data.remark : "",
          };
          this.open = true;
          this.title = "修改生产报工记录";
          this.optType = "edit";
          this.initialForm = Object.assign(
            {},
            { ...res.data, remark: res.data.remark ? res.data.remark : "" }
          );
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getFeedback(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看生产报工单信息";
          this.optType = "view";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    handleCommand(command, row) {
      switch (command) {
        case "handleApproval":
          this.handleApproval(row);
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
        if (this.form.quantityFeedBack > this.form.quantity) {
          this.$confirm("报工数量大于排产数量,是否继续?", "提示", {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning",
          })
            .then(() => {
              updateFeedback(this.form).then((res) => {
                if (res.code == 0) {
                  this.$modal.msgSuccess("修改成功");
                  this.open = false;
                  this.getList();
                } else {
                  this.$modal.notifyError(res.message);
                }
              });
            })
            .catch(() => {});
        } else {
          updateFeedback(this.form).then((res) => {
            if (res.code == 0) {
              this.$modal.msgSuccess("修改成功");
              this.open = false;
              this.getList();
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }
      } else {
        addFeedback(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delFeedback, this.getList, "记录Id为" + row.id);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },

    // 批量审批
    async handleApproval(row) {
      if (!row.id && this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行审批？")
        .catch(() => {});
      const commitIds = row.id ? [row.id] : this.ids;
      if (results == "confirm") {
        commitFeedback({
          ids: commitIds,
          feedBackStatus: "COMMITED",
        })
          .then((res) => {
            if (res.code == 0) {
              this.$modal.msgSuccess("审批成功");
              this.open = false;
              this.getList();
            } else {
              this.$modal.notifyError(res.message);
            }
          })
          .catch(() => {});
      }
    },
    // 点击撤销审批
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
          feedBackStatus: "DRAFT",
        })
          .then((res) => {
            if (res.code == 0) {
              this.$modal.msgSuccess("撤销成功");
              this.open = false;
              this.getList();
            } else {
              this.$modal.notifyError(res.message);
            }
          })
          .catch(() => {});
      }
    },
    //选择生产工单
    handleWorkOrderSelect() {
      this.$refs.woSelect.showFlag = true;
      this.$refs.woSelect.getList();
    },
    // 选择生产工单
    onWorkOrderSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "workOrderId", row.id);
        this.$set(this.form, "workOrderCode", row.code);
        this.$set(this.form, "workOrderName", row.name);
        this.$set(this.form, "itemId", row.itemId);
        this.$set(this.form, "itemCode", row.itemCode);
        this.$set(this.form, "itemName", row.itemName);
        this.$set(this.form, "itemTypeId", row.itemTypeId);
        this.$set(this.form, "specification", row.specification);
        this.$set(this.form, "unitOfMeasure", row.unitOfMeasure);
      }
    },
    // 选择生产任务
    handleTaskSelect() {
      this.$refs.taskSelect.showFlag = true;
      this.$refs.taskSelect.getList();
      this.$refs.taskSelect.getProcess();
    },
    // 选择生产任务
    onTaskSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "taskId", row.id);
        this.$set(this.form, "taskCode", row.code);
        this.$set(this.form, "taskName", row.name);
        this.$set(this.form, "workStationId", row.workStationId);
        this.$set(this.form, "workStationName", row.workStationName);
        this.$set(this.form, "workStationCode", row.workStationCode);
        this.$set(this.form, "processId", row.processId);
        this.$set(this.form, "processCode", row.processCode);
        this.$set(this.form, "processName", row.processName);
      }
    },
    //点击报工人选择按钮
    handleUserSelect() {
      this.$refs.userSelect.showFlag = true;
      this.$refs.userSelect.selectedId = this.form.userName
        ? this.form.userName
        : undefined;
      this.$refs.userSelect.getTreeselect();
      this.$refs.userSelect.getList();
    },
    //报工人返回
    onUserSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "userName", row.realName);
      }
    },
    //点击审核人选择按钮
    handleUser2Select() {
      this.$refs.user2Select.showFlag = true;
      this.$refs.user2Select.selectedId = this.form.nickName
        ? this.form.nickName
        : undefined;
      this.$refs.user2Select.getTreeselect();
      this.$refs.user2Select.getList();
    },
    //审核人选择返回
    onUser2Selected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "nickName", row.realName);
      }
    },
    // 计算报工数量
    calculateQuantityFeedBack() {
      this.form.quantityFeedBack =
        this.form.quantityQualified + this.form.quantityUnQuanlified;
    },

    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel("/v1/FeedBack/DownLoadList", "生产报工.xlsx");
    },
    // 点击计数器
    changeNum(params) {
      this.form[params.str] = params.value;
      this.calculateQuantityFeedBack();
    },
  },
};
</script>
