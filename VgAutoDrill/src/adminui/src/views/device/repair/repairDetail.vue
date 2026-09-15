<template>
  <div class="app-container">
    <el-row :gutter="10" class="mb8" v-if="parentOptType != 'view'">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['device:repairDetails:add'])"
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
          :disabled="hasPermi(['device:repairDetails:remove'])"
          >批量删除</el-button
        >
      </el-col>
    </el-row>

    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="repairDetailList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column
        type="selection"
        width="55"
        align="center"
        v-if="parentOptType != 'view'"
      />
      <el-table-column
        label="项目编码"
        min-width="150"
        prop="subjectCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="项目名称"
        min-width="150"
        prop="subjectName"
        show-overflow-tooltip
      />
      <el-table-column
        label="检验结果"
        min-width="150"
        prop="maintainResult"
        show-overflow-tooltip
      />
      <el-table-column
        label="改善设施"
        min-width="150"
        prop="betterSteps"
        show-overflow-tooltip
      />
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        class-name="small-padding fixed-width"
        v-if="parentOptType != 'view'"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['device:repairDetails:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['device:repairDetails:remove'])"
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
      :autoScroll="false"
    />

    <!-- 添加或修改设备维护明细对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="100px">
        <el-row>
          <el-col :span="12">
            <el-form-item label="项目编码" prop="subjectCode">
              <el-input
                v-model="form.subjectCode"
                placeholder="请选择项目"
                disabled
              >
                <el-button
                  v-debounce
                  v-if="form.id == null"
                  @click="handleSubjectSelect"
                  slot="append"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
            </el-form-item>

            <SubjectSelect
              ref="SubjectSelect"
              type="radio"
              @onSelected="onSubjectSelected"
            ></SubjectSelect>
          </el-col>
          <el-col :span="12">
            <el-form-item label="项目名称" prop="subjectName">
              <el-input
                v-model="form.subjectName"
                placeholder="请选择项目"
                disabled
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="检验结果" prop="maintainResult">
              <el-input v-model="form.maintainResult" placeholder="请输入" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="改善设施" prop="betterSteps">
          <el-input
            type="textarea"
            v-model="form.betterSteps"
            placeholder="请输入"
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
  getDeviceMaintainDetail,
  listDeviceMaintainDetail,
  updateDeviceMaintainDetail,
  delDeviceMaintainDetail,
  delList,
  addDeviceMaintainDetail,
} from "@/api/device/deviceMaintainDetail";
import SubjectSelect from "@/components/subjectSelect/radio.vue";
export default {
  name: "RepairDetail",
  components: {
    SubjectSelect,
  },
  props: ["masterId", "deviceId", "parentOptType"],
  data() {
    return {
      page: "repairdetail",
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
      // 设备维护明细表格数据
      repairDetailList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        masterId: undefined,
        deviceId: undefined,
        subjectId: undefined,
        subjectCode: undefined,
        maintainResult: undefined,
        betterSteps: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        subjectName: [
          { required: true, message: "项目名称不能为空", trigger: "change" },
        ],
        subjectCode: [
          { required: true, message: "项目编码不能为空", trigger: "change" },
        ],
      },
    };
  },
  watch: {
    masterId(newVal) {
      this.queryParams.masterId = newVal;
      this.getList();
    },
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  created() {
    this.getList();
  },
  methods: {
    /** 查询设备维护明细列表 */
    getList() {
      this.loading = true;
      this.queryParams.masterId = this.masterId;
      listDeviceMaintainDetail(this.queryParams).then((response) => {
        this.repairDetailList = response.data.list;
        this.total = response.data.total;
        this.loading = false;
      });
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
        masterId: this.masterId,
        deviceId: this.deviceId,
        maintainResult: "",
        betterSteps: "",
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

    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.optType = "add";
      this.title = "添加设备维护明细";
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const lineId = row.id || this.ids;
      getDeviceMaintainDetail(lineId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.optType = "edit";
          this.initialForm = Object.assign({}, res.data);
          this.title = "修改设备维护明细";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateDeviceMaintainDetail(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addDeviceMaintainDetail(this.form).then((res) => {
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
          delDeviceMaintainDetail,
          this.getList,
          "项目编码为" + row.subjectCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导出按钮操作 */
    handleExport() {
      this.download(
        "dv/repairline/export",
        {
          ...this.queryParams,
        },
        `repairline_${new Date().getTime()}.xlsx`
      );
    },
    // 选择项目
    handleSubjectSelect() {
      this.$refs.SubjectSelect.showFlag = true;
      this.$refs.SubjectSelect.selectSubjectId = this.form.subjectId
        ? this.form.subjectId
        : undefined;
      this.$refs.SubjectSelect.getList();
    },
    onSubjectSelected(row) {
      if (row != null) {
        this.$set(this.form, "subjectName", row.name);
        this.$set(this.form, "subjectCode", row.code);
        this.$set(this.form, "subjectId", row.id);
      }
    },
  },
};
</script>
