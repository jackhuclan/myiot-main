<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="设备编码" prop="deviceCode">
        <el-input
          v-trim
          v-model="queryParams.deviceCode"
          placeholder="请输入设备编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['alarm:event:export'])"
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
      :ref="page"
      v-loading="loading"
      :data="analysisList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
      border
    >
      <el-table-column type="selection" width="55" align="center" />

      <el-table-column
        show-overflow-tooltip
        label="设备编码"
        prop="deviceCode"
        fixed="left"
        key="deviceCode"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['alarm:event:view']"
            >{{ scope.row.deviceCode }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        show-overflow-tooltip
        label="方式"
        prop="manner"
        v-if="columns[1].visible"
        key="manner"
      />
      <el-table-column
        label="日期"
        prop="date"
        align="center"
        v-if="columns[2].visible"
        key="date"
      >
      </el-table-column>
      <el-table-column
        label="采集时间"
        prop="time"
        key="time"
        v-if="columns[3].visible"
      >
      </el-table-column>

      <el-table-column
        label="换板次数"
        align="center"
        prop="changeBoardNum"
        key="changeBoardNum"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.changeBoardNum) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="标准换板时间"
        align="center"
        prop="changeBoardTime"
        min-width="180"
        key="changeBoardTime"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="换料次数"
        align="center"
        prop="refuelingNum"
        key="refuelingNum"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="标准换料时间"
        align="center"
        prop="refuelingTime"
        min-width="180"
        key="refuelingTime"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.refuelingTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="异常处理次数"
        align="center"
        prop="errorNum"
        min-width="180"
        key="errorNum"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="异常处理时间"
        align="center"
        prop="errorTime"
        min-width="180"
        key="errorTime"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.errorTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="保养时间"
        align="center"
        prop="errorTime"
        key="errorTime"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.errorTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="理论稼动率"
        align="center"
        prop="errorTime"
        min-width="120"
        key="errorTime"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="实际稼动率"
        align="center"
        prop="errorTime"
        min-width="120"
        key="errorTime"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="差异"
        align="center"
        prop="errorTime"
        key="errorTime"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="所属班次"
        align="center"
        prop="errorTime"
        key="errorTime"
        v-if="columns[4].visible"
      >
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['alarm:event:edit'])"
            >修改</el-button
          >
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

    <!-- 添加或修改事件对话框 -->
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
          <el-col :span="12">
            <el-form-item label="事件Id" prop="eventId">
              <el-input
                v-model="form.eventId"
                :min="0"
                :disabled="title == '修改事件'"
                placeholder="请输入事件Id"
              /> </el-form-item
          ></el-col>
          <el-col :span="12"
            ><el-form-item label="事件名称" prop="eventName">
              <el-input
                v-model="form.eventName"
                placeholder="请输入事件名称"
                :disabled="title == '修改事件'"
              /> </el-form-item
          ></el-col>
        </el-row>

        <el-form-item label="事件级别" prop="eventLevel">
          <el-select
            v-model="eventLevel"
            placeholder="事件级别"
            @change="handleEventLevel"
          >
            <el-option
              v-for="dict in $status.alarmLevelOptions"
              :key="dict.value"
              :label="dict.label"
              :value="dict.value"
            />
          </el-select>
        </el-form-item>

        <el-form-item label="参数配置" prop="parameterJson">
          <el-input
            v-model="form.parameterJson"
            type="textarea"
            placeholder="请输入参数配置"
            :autosize="{ minRows: 2, maxRows: 10 }"
            :resize="optType == 'view' ? 'none' : ''"
          ></el-input>
        </el-form-item>
      </el-form>
    </edit-form-dialog>
  </div>
</template>
  
  <script>
import {
  listAlarmEvent,
  getAlarmEvent,
  addAlarmEvent,
  updateAlarmEvent,
} from "@/api/alarm/alarmEvent";

export default {
  // 实时稼动率
  name: "RealTime",
  data() {
    return {
      page: "event",
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
      // 事件表格数据
      alarmEventList: [],
      analysisList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        eventName: undefined,
        eventId: undefined,
        eventLevel: undefined,
        deviceTypeId: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        name: [
          { required: true, message: "事件名称不能为空", trigger: "blur" },
        ],
        code: [{ required: true, message: "事件Id不能为空", trigger: "blur" }],
        eventLevel: [
          { required: true, message: "事件等级不能为空", trigger: "change" },
        ],
        eventName: [
          { required: true, message: "事件名称不能为空", trigger: "blur" },
        ],
        eventId: [
          { required: true, message: "事件Id不能为空", trigger: "blur" },
        ],
      },
      eventLevel: "",
      columns: [
        { key: 0, label: "事件Id", visible: true },
        { key: 1, label: "事件名称", visible: true },
        { key: 2, label: "事件级别", visible: true },
        { key: 3, label: "参数配置", visible: true },
        { key: 4, label: "创建时间", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询事件列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listAlarmEvent(this.queryParams);
      this.alarmEventList = res.data.list;
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
        eventId: undefined,
        eventName: undefined,
        eventLevel: undefined,
        deviceTypeId: undefined,
      };
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
      this.optType = "add";
      this.open = true;
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加事件";
      this.eventLevel = "";
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const alarmTypeId = row.id || this.ids;
      getAlarmEvent(alarmTypeId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.eventLevel = this.$status.alarmLevelOptions.filter(
            (v) => res.data.eventLevel == v.value
          )[0]?.label;
          this.initialForm = Object.assign({}, res.data);
          this.optType = "edit";
          this.open = true;
          this.title = "修改事件";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击查看
    handleView(id) {
      this.reset();
      getAlarmEvent(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.eventLevel = this.$status.alarmLevelOptions.filter(
            (v) => res.data.eventLevel == v.value
          )[0]?.label;
          this.title = "查看事件";
          this.optType = "view";
          this.open = true;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      this.form.parentId = 0;
      if (this.form.id != undefined) {
        updateAlarmEvent(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addAlarmEvent(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.open = false;
            this.queryParams.pageNum = 1;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 切换事件等级
    handleEventLevel(val) {
      this.eventLevel = this.$status.alarmLevelOptions.filter(
        (v) => val == v.value
      )[0]?.label;
      this.form.eventLevel = val;
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/AlarmEvent/DownLoadList",
        "事件数据.xlsx",
        this.queryParams
      );
    },
  },
};
</script>
  