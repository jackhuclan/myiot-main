<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="告警分类" prop="alarmKinds">
        <el-select
          v-model="queryParams.alarmKinds"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.alarmKinds && queryParams.alarmKinds.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.alarmKindList"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>

      <el-form-item label="告警编码" prop="alarmCode">
        <el-input
          v-trim
          v-model="queryParams.alarmCode"
          placeholder="请输入告警编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="告警名称" prop="alarmName">
        <el-input
          v-trim
          v-model="queryParams.alarmName"
          placeholder="请输入告警名称"
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
      <el-form-item label="告警时间">
        <el-date-picker
          v-model="queryParams.queryStartTime"
          type="datetime"
          placeholder="起始时间"
        >
        </el-date-picker>
        ↔
        <el-date-picker
          v-model="queryParams.queryEndTime"
          type="datetime"
          placeholder="结束时间"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="库位编码" prop="locationCode">
        <el-input
          v-trim
          v-model="queryParams.locationCode"
          placeholder="请输入库位编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="区域" prop="partitionCode">
        <el-input
          v-trim
          v-model="queryParams.partitionCode"
          placeholder="请输入区域"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="是否处理" prop="isHandled">
        <el-select
          @clear="clearQueryParams('isHandled')"
          v-model="queryParams.isHandled"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option label="是" :value="true" />
          <el-option label="否" :value="false" />
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
          :disabled="hasPermi(['alarm:warn:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-set-up"
          @click="handleDelete"
          :disabled="hasPermi(['alarm:warn:remove'])"
          >批量处理</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['alarm:warn:export'])"
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
      :data="alarmRecordList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
      border
    >
      <el-table-column type="selection" width="55" align="center" />

      <el-table-column
        label="告警ID"
        align="center"
        key="id"
        prop="id"
        fixed="left"
        width="65"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['alarm:setting:view']"
            >{{ scope.row.id }}</span
          >
        </template></el-table-column
      >

      <el-table-column
        label="告警编码"
        key="alarmCode"
        prop="alarmCode"
        fixed="left"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[1].visible"
      >
      </el-table-column>
      <el-table-column
        label="告警名称"
        key="alarmName"
        prop="alarmName"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="告警分类"
        key="alarmKind"
        prop="alarmKind"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          {{
            $status.alarmKindList.find((v) => v.value == scope.row.alarmKind)
              ? $status.alarmKindList.find(
                  (v) => v.value == scope.row.alarmKind
                ).label
              : ""
          }}
        </template></el-table-column
      >
      <el-table-column
        label="库位"
        key="locationCode"
        prop="locationCode"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="区域"
        key="partitionCode"
        prop="partitionCode"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />locationCode
      <el-table-column
        label="是否处理"
        align="center"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isHandled"> 是 </el-tag>
          <el-tag v-else type="danger">否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="处理时间"
        align="center"
        key="handledTime"
        prop="handledTime"
        v-if="columns[5].visible"
        width="180"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.handledTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="级别"
        align="center"
        key="alarmLevel"
        prop="alarmLevel"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <el-tag type="danger" v-if="scope.row.alarmLevel == 3">紧急</el-tag>
          <el-tag type="warning" v-else-if="scope.row.alarmLevel == 2"
            >严重</el-tag
          >
          <el-tag v-else>普通</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="告警时间"
        align="center"
        key="alarmTime"
        prop="alarmTime"
        width="180"
        v-if="columns[7].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.alarmTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="160px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['alarm:warn:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-set-up"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['alarm:warn:remove']) || scope.row.isHandled"
            >{{ scope.row.isHandled ? "已处理" : "处理" }}</el-button
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

    <!-- 添加或修改告警对话框 -->
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
          <el-col :span="12">
            <el-form-item label="告警编码" prop="alarmCode">
              <el-input
                v-model="form.alarmCode"
                placeholder="请输入告警编码"
              /> </el-form-item
          ></el-col>
          <el-col :span="12">
            <el-form-item label="告警名称" prop="alarmName">
              <el-input
                v-model="form.alarmName"
                placeholder="请输入告警名称"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="告警级别" prop="alarmLevel">
              <el-select v-model="form.alarmLevel" placeholder="告警级别">
                <el-option
                  v-for="dict in $status.alarmLevelOptions"
                  :key="dict.value"
                  :label="dict.label"
                  :value="dict.value"
                />
              </el-select> </el-form-item
          ></el-col>
          <el-col :span="12"></el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listAlarm,
  getAlarm,
  delAlarm,
  addAlarm,
  updateAlarm,
  delList,
} from "@/api/alarm/alarm";
export default {
  name: "AlarmRecord",
  data() {
    return {
      page: "warn",
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
      // 告警表格数据
      alarmRecordList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        status: undefined,
        alarmLevel: undefined,
        alarmCode: undefined,
        alarmName: undefined,
        isHandled: undefined,
        alarmKinds: [],
        queryStartTime: undefined,
        queryEndTime: undefined,
        locationCode: undefined,
        partitionCode: undefined,
      },

      // 表单参数
      form: {},
      initialForm: {},
      timer: null,
      // 表单校验
      rules: {
        alarmName: [
          { required: true, message: "告警名称不能为空", trigger: "blur" },
        ],
        alarmLevel: [
          { required: true, message: "告警级别不能为空", trigger: "change" },
        ],
        alarmCode: [
          { required: true, message: "告警编码不能为空", trigger: "blur" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "告警ID", visible: true },
        { key: 1, label: "告警编码", visible: true },
        { key: 2, label: "告警名称", visible: true },
        { key: 3, label: "告警分类", visible: true },
        { key: 4, label: "是否处理", visible: true },
        { key: 5, label: "处理时间", visible: true },
        { key: 6, label: "级别", visible: true },
        { key: 7, label: "告警时间", visible: true },
      ],
    };
  },
  activated() {
    this.queryParams.isHandled = localStorage.getItem("isHandled")
      ? localStorage.getItem("isHandled") == "是"
        ? true
        : false
      : undefined;
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
    this.openTimer();
  },
  deactivated() {
    this.closeTimer();
  },
  watch: {
    "queryParams.isHandled": {
      handler(val) {
        localStorage.setItem("isHandled", val == true ? "是" : "否");
      },
    },

    open(val) {
      if (val) {
        this.closeTimer();
      } else {
        this.openTimer();
      }
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
    // 开启定时器
    openTimer() {
      this.getList();
      if (this.timer) {
        clearInterval(this.timer);
        this.timer = null;
      }
      // 每隔5秒自动刷新
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.getList(); //调用接口的方法
        }, 0);
      }, 5000);
    },
    // 关闭定时器
    closeTimer() {
      clearInterval(this.timer);
      this.timer = null;
    },
    /** 查询告警列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listAlarm(this.queryParams);
      this.alarmRecordList = res.data.list;
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
        alarmCode: "",
        alarmName: "",
        alarmLevel: "",
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
      this.queryParams.queryEndTime = undefined;
      this.queryParams.queryStartTime = undefined;
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.optType = "add";
      this.initialForm = Object.assign({}, this.form);
      this.open = true;
      this.title = "添加告警记录";
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const alarmRecordId = row.id || this.ids;
      getAlarm(alarmRecordId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.optType = "edit";
          this.open = true;
          this.title = "修改告警记录";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击编码查看
    handleView(id) {
      this.reset();
      getAlarm(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.optType = "view";
          this.open = true;
          this.title = "查看告警记录";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateAlarm(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addAlarm({
          ...this.form,
          status: 0,
          alarmTime: new Date().toLocaleString(),
          equimentId: 0,
          eventId: 0,
          eventData: "",
        }).then((res) => {
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

    //处理
    handleDelete(row) {
      const ids = row.id || this.ids;
      let delApi = null;
      let label = null;

      if (row.id) {
        delApi = delAlarm;
        label = `确定处理<span style="color:red">告警记录编码为 ${row.alarmCode}</span> 的数据项？`;
      } else {
        delApi = delList;
        label = "确定处理当前操作的数据项？";
      }
      if (Array.isArray(ids) && ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");

      this.$modal
        .confirm(label, {
          dangerouslyUseHTMLString: true, // 使用HTML片段
        })
        .then((result) => {
          if (result == "confirm") {
            delApi(ids)
              .then((res) => {
                if (res.code == 0) {
                  this.$modal.msgSuccess("处理成功");
                  this.getList();
                } else {
                  this.$modal.notifyError(res.message);
                }
              })
              .catch(() => {});
          }
        })

        .catch(() => {});
    },

    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/Alarm/DownLoadList",
        "告警记录.xlsx",
        this.queryParams
      );
    },
  },
};
</script>
