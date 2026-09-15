<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="告警设置编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入告警设置编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="告警设置名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入告警设置名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="告警级别" prop="alarmLevel">
        <el-select
          @clear="clearQueryParams('alarmLevel')"
          v-model="queryParams.alarmLevel"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option
            v-for="dict in $status.alarmLevelOptions"
            :key="dict.value"
            :label="dict.label"
            :value="dict.value"
          />
        </el-select>
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
          :disabled="hasPermi(['alarm:setting:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['alarm:setting:remove'])"
          >批量删除</el-button
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
      :data="alarmSettingList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="告警设置编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        fixed="left"
        min-width="150"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['alarm:setting:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="告警设置名称"
        key="name"
        prop="name"
        min-width="200"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        min-width="300"
        label="事件"
        key="eventName"
        prop="eventName"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="级别"
        align="center"
        key="alarmLevel"
        prop="alarmLevel"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <el-tag type="danger" v-if="scope.row.alarmLevel == 3">紧急</el-tag>
          <el-tag type="warning" v-else-if="scope.row.alarmLevel == 2"
            >严重</el-tag
          >
          <el-tag v-else-if="scope.row.alarmLevel == 1">普通</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        min-width="150"
        label="描述"
        key="alarmDesc"
        prop="alarmDesc"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />

      <el-table-column
        label="通知方式"
        align="center"
        key="notifyWayNames"
        prop="notifyWayNames"
        show-overflow-tooltip
        v-if="columns[5].visible"
        min-width="150px"
      >
        <template slot-scope="scope">
          {{ scope.row.notifyWayNames }}
        </template>
      </el-table-column>
      <el-table-column
        label="触发规则"
        min-width="150"
        key="eventRules"
        prop="eventRules"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            v-debounce
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['alarm:setting:edit'])"
            >修改</el-button
          >
          <el-button
            v-debounce
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['alarm:setting:remove'])"
            >删除</el-button
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

    <!-- 添加或修改告警设置对话框 -->
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
          <el-col :span="12"
            ><el-form-item label="设置编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入告警设置编码"
              /> </el-form-item
          ></el-col>
          <el-col :span="12"
            ><el-form-item label="设置名称" prop="name">
              <el-input
                v-model="form.name"
                placeholder="请输入告警设置名称"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="事件" prop="eventName">
              <el-input v-model="form.eventName" placeholder="请选择事件">
                <el-button
                  v-debounce
                  slot="append"
                  icon="el-icon-search"
                  @click="handleEventSelect"
                ></el-button>
              </el-input>
            </el-form-item>
            <EventSelect ref="eventSelect" @onSelected="onEventSelected" />
          </el-col>
          <el-col :span="12">
            <el-form-item label="通知方式" prop="notifyWayNames">
              <el-input
                v-model="form.notifyWayNames"
                placeholder="请选择通知方式"
              >
                <el-button
                  v-debounce
                  slot="append"
                  icon="el-icon-search"
                  @click="handleNotifySelect"
                ></el-button>
              </el-input>
            </el-form-item>
            <NotifySelect ref="notifySelect" @onSelected="onNotifySelected" />
          </el-col>
        </el-row>

        <el-form-item label="触发规则">
          <el-input
            v-model="form.eventRules"
            type="textarea"
            placeholder="请输入触发规则"
            :autosize="{ minRows: 2, maxRows: 10 }"
            :resize="optType == 'view' ? 'none' : ''"
          />
        </el-form-item>
        <el-form-item label="设置描述">
          <el-input
            type="textarea"
            v-model="form.alarmDesc"
            placeholder="请输入告警设置描述"
            :autosize="{ minRows: 2, maxRows: 10 }"
            :resize="optType == 'view' ? 'none' : ''"
          />
        </el-form-item>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listAlarmSetting,
  getAlarmSetting,
  delAlarmSetting,
  addAlarmSetting,
  updateAlarmSetting,
  delList,
} from "@/api/alarm/alarmSetting";
import EventSelect from "@/components/eventSelect";
import NotifySelect from "@/components/notifySelect/checkbox.vue";
export default {
  name: "AlarmSetting",
  components: { EventSelect, NotifySelect },
  data() {
    return {
      page: "alarmSetting",
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
      alarmSettingList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 告警设置查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        status: undefined,
        alarmLevel: undefined,
        eventId: 0,
        eventRules: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        name: [
          { required: true, message: "告警设置名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "告警设置编码不能为空", trigger: "blur" },
        ],
        alarmDesc: [
          { required: true, message: "告警设置描述不能为空", trigger: "blur" },
        ],
        alarmLevel: [
          { required: true, message: "告警设置等级不能为空", trigger: "blur" },
        ],
        eventRules: [
          { required: true, message: "触发规则不能为空", trigger: "blur" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "告警设置编码", visible: true },
        { key: 1, label: "告警设置名称", visible: true },
        { key: 2, label: "事件", visible: true },
        { key: 3, label: "级别", visible: true },
        { key: 4, label: "描述", visible: true },
        { key: 5, label: "通知方式", visible: true },
        { key: 6, label: "触发规则", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    "form.eventName": {
      handler(val) {
        if (val == "") {
          this.form.eventId = undefined;
          this.form.alarmLevel = undefined;
        }
      },
    },
    "form.notifyWayNames": {
      handler(val) {
        if (val == "") {
          this.form.notifyWayIds = undefined;
        }
      },
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
    /** 查询事件列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listAlarmSetting(this.queryParams);
      this.alarmSettingList = res.data.list;
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
        name: "",
        code: "",
        alarmDesc: "",
        eventId: undefined,
        eventName: "",
        alarmLevel: undefined,
        notifyWayIds: undefined,
        notifyWayNames: "",
        eventRules: "",
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
      this.initialForm = Object.assign({}, this.form);
      this.open = true;
      this.title = "添加告警设置";
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const AlarmSettingId = row.id || this.ids;
      getAlarmSetting(AlarmSettingId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.optType = "edit";
          this.open = true;
          this.title = "修改告警设置";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 点击编码操作 */
    handleView(id) {
      this.reset();
      getAlarmSetting(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.optType = "view";
          this.open = true;
          this.title = "查看告警设置";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      this.form.parentId = 0;
      this.form.alarmLevel = this.form.alarmLevel * 1;
      if (this.form.id != undefined) {
        updateAlarmSetting({
          ...this.form,
          isDeleted: 0,
        }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addAlarmSetting(this.form).then((res) => {
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
    //删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delAlarmSetting,
          this.getList,
          "告警设置编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },

    // 选择事件
    handleEventSelect() {
      this.$refs.eventSelect.showFlag = true;
      this.$refs.eventSelect.selectedEventId = this.form.eventId
        ? this.form.eventId
        : undefined;

      this.$refs.eventSelect.getList();
    },
    onEventSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "eventId", row.id);
        this.$set(this.form, "eventName", row.eventName);
        this.$set(this.form, "alarmLevel", row.eventLevel);
      }
    },
    // 选择通知方式
    handleNotifySelect() {
      this.$refs.notifySelect.showFlag = true;
      this.$refs.notifySelect.showData = this.form.notifyWayIds
        ? this.form.notifyWayIds.split(",")
        : [];
      this.$refs.notifySelect.getList();
    },
    onNotifySelected(row) {
      if (row.length > 0) {
        this.$set(this.form, "notifyWayIds", row.map((v) => v.id).join(","));
        this.$set(
          this.form,
          "notifyWayNames",
          row.map((v) => v.name).join(",")
        );
      }
    },
  },
};
</script>
