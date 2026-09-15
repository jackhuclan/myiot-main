<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
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

      <el-form-item label="所属工序" prop="processCode">
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
      <el-form-item label="任务状态" prop="taskStatusList">
        <el-select
          v-model="queryParams.taskStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.taskStatusList && queryParams.taskStatusList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.taskOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
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
    <el-table v-loading="loading" :data="taskList" :ref="page" border>
      <el-table-column
        label="任务编码"
        fixed="left"
        min-width="200px"
        key="code"
        prop="code"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
      </el-table-column>
      <el-table-column
        label="工单编码"
        min-width="200px"
        key="workOrderCode"
        prop="workOrderCode"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />

      <el-table-column
        label="工作站"
        min-width="150px"
        key="workStationCode"
        prop="workStationCode"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="任务状态"
        align="center"
        key="taskStatus"
        prop="taskStatus"
        v-if="columns[3].visible"
      >
        <!-- 草稿，已提交，已开始，已完成 -->
        <template slot-scope="scope">
          <status-tag
            :options="$status.taskOptions"
            :status="scope.row.taskStatus"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="所属工序"
        min-width="150px"
        key="processName"
        prop="processName"
        show-overflow-tooltip
        v-if="columns[4].visible"
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
        label="产品编码"
        min-width="200px"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        label="规格型号"
        key="specification"
        prop="specification"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[6].visible"
      />
      <el-table-column
        label="排产数量"
        align="center"
        key="quantity"
        prop="quantity"
        v-if="columns[7].visible"
      />
      <el-table-column
        label="领取数量"
        align="center"
        key="nowWadCount"
        prop="nowWadCount"
        v-if="columns[8].visible"
      />
      <el-table-column
        label="已生产"
        align="center"
        key="quantityProduced"
        prop="quantityProduced"
        v-if="columns[9].visible"
      />
      <el-table-column
        label="良品"
        align="center"
        key="quantityQuanlify"
        prop="quantityQuanlify"
        v-if="columns[10].visible"
      />
      <el-table-column
        label="不良品"
        align="center"
        key="quantityUnQuanlify"
        prop="quantityUnQuanlify"
        v-if="columns[11].visible"
      />
      <el-table-column
        label="开始时间"
        min-width="150px"
        align="center"
        key="startTime"
        prop="startTime"
        v-if="columns[12].visible"
      >
        <template slot-scope="scope">
          <span
            >{{ (parseTime(scope.row.startTime,"{m}-{d} {h}:{i}")),  }}</span
          >
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
            :disabled="
              hasPermi(['produce:reportWork:report']) ||
              scope.row.taskStatus == 3
            "
            @click="handleAddFeedback(scope.row)"
            >报工</el-button
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
                v-if="scope.row.taskStatus == 10"
                command="handleStart"
                icon="el-icon-video-play"
                :disabled="hasPermi(['produce:reportWork:start'])"
                >开始</el-dropdown-item
              >
              <el-dropdown-item
                v-else-if="scope.row.taskStatus == 40"
                command="handleFinish"
                icon="el-icon-circle-check"
                :disabled="hasPermi(['produce:reportWork:finish'])"
                >完成</el-dropdown-item
              >
              <el-dropdown-item
                command="handleAddCheckRecords"
                icon="el-icon-upload2"
                :disabled="
                  hasPermi(['produce:reportWork:checkRecord']) ||
                  scope.row.taskStatus == 50
                "
                >生成检验记录</el-dropdown-item
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
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="100px">
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
              <!-- <el-date-picker
                clearable
                v-model="form.feedBackTime"
                type="date"
                value-format="yyyy-MM-dd"
                placeholder="请选择报工时间"
              >
              </el-date-picker> -->
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
    <!-- 生产检验记录弹框 -->
    <CheckDialog ref="checkDialog" @parent_getList="getList" />
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { addFeedback, listTaskList } from "@/api/produce/reportWork";
// 报工人及审核人选择
import UserSingleSelect from "@/components/userSelect";
// 生产检验记录dialog
import CheckDialog from "./checkRecordsDialog";
import { getTask, updateByOutSide, delTask, delList } from "@/api/produce/task";
import { getDropSelectDatas as getProcessSelectDatas } from "@/api/produce/process";

export default {
  name: "ReportWork",
  components: {
    UserSingleSelect,
    CheckDialog,
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
      page: "reportWork",
      optType: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 生产报工记录表格数据
      taskList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
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
          {
            required: true,
            message: "良品不能为空",
            trigger: "change",
          },
        ],
        quantityUnQuanlified: [
          {
            required: true,
            message: "不良品不能为空",
            trigger: "change",
          },
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
        status: undefined,
      },
      // 工序
      processOptions: [],
      // 列信息
      columns: [
        { key: 0, label: "任务编码", visible: true },
        { key: 1, label: "工单编码", visible: true },
        { key: 2, label: "工作站", visible: true },
        { key: 3, label: "任务状态", visible: true },
        { key: 4, label: "所属工序", visible: true },
        { key: 5, label: "产品编码", visible: true },
        { key: 6, label: "规格型号", visible: true },
        { key: 7, label: "排产数量", visible: true },
        { key: 8, label: "领取数量", visible: true },
        { key: 9, label: "已生产", visible: true },
        { key: 10, label: "良品", visible: true },
        { key: 11, label: "不良品", visible: true },
        { key: 12, label: "开始时间", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    this.getProcessList();
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
    ...mapGetters(["name"]),
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
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
    /** 查询生产报工记录列表 */
    async getList(isSearch) {
      this.loading = true;
      // 生产报工
      const res = await listTaskList(this.queryParams);
      this.taskList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 表单重置
    reset() {
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
    /** 报工按钮操作 */
    handleAddFeedback(row) {
      this.reset();
      const recordId = row.id;
      getTask(recordId).then((res) => {
        if (res.code == 0) {
          this.form = {
            ...res.data,
            taskId: res.data.id,
            taskCode: res.data.code,
            feedBackType: "自行报工",
            quantity: res.data.quantity,
            quantityFeedBack: 0,
            quantityQualified: 0,
            quantityUnQuanlified: 0,
            userName: this.name,
            nickName: undefined,
            remark: "",
            feedBackTime: new Date(),
          };
          this.initialForm = Object.assign({}, this.form);
          this.open = true;
          this.title = "执行生产报工";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },

    // 生成检验记录
    handleAddCheckRecords(row) {
      const recordId = row.id;
      getTask(recordId).then((res) => {
        if (res.code == 0) {
          this.$refs.checkDialog.form = {
            id: undefined,
            status: undefined,
            taskId: res.data.id,
            taskName: res.data.name,
            taskCode: res.data.code,
            workOrderId: res.data.workOrderId,
            workOrderName: res.data.workOrderName,
            workOrderCode: res.data.workOrderCode,
            userId: undefined,
            userName: undefined,
            isCheckOk: undefined,
            checkTime: undefined,
            remark: "",
          };
          this.$refs.checkDialog.open = true;
          this.$refs.checkDialog.initialForm = Object.assign(
            {},
            this.$refs.checkDialog.form
          );
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击完成
    async handleFinish(row) {
      const results = await this.$modal
        .confirm("确认执行完成？")
        .catch(() => {});
      if (results == "confirm") {
        updateByOutSide({
          code: row.code,
          taskStatus: 50,
        }).then((res) => {
          if (res.code == 0) {
            this.getList();
            this.$modal.msgSuccess("操作成功");
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 点击开始
    async handleStart(row) {
      const results = await this.$modal
        .confirm("确认执行开始？")
        .catch(() => {});
      if (results == "confirm") {
        updateByOutSide({
          code: row.code,
          taskStatus: 40,
        }).then((res) => {
          if (res.code == 0) {
            this.getList();
            this.$modal.msgSuccess("操作成功");
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleAddCheckRecords":
          this.handleAddCheckRecords(row);
          break;
        case "handleStart":
          this.handleStart(row);
          break;
        case "handleFinish":
          this.handleFinish(row);
          break;
        default:
          break;
      }
    },
    /** 提交报工按钮 */
    submitForm() {
      if (!this.form.quantity)
        return this.$modal.msgError("排产数量为0，无法报工");
      if (this.form.quantityFeedBack > this.form.quantity) {
        this.$confirm("报工数量大于排产数量,是否继续?", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
        })
          .then(() => {
            addFeedback(this.form).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("操作成功");
                this.open = false;
                this.getList();
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          })
          .catch(() => {});
      } else {
        addFeedback(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("操作成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
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
    onUserSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "userName", obj.realName);
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
    onUser2Selected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "nickName", obj.realName);
      }
    },

    // 计算报工数量
    calculateQuantityFeedBack() {
      this.form.quantityFeedBack =
        this.form.quantityQualified + this.form.quantityUnQuanlified;
    },
    // 点击计数器
    changeNum(params) {
      this.form[params.str] = params.value;
      this.calculateQuantityFeedBack();
    },
  },
};
</script>
