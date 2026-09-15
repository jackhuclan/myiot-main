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

      <el-form-item label="检验日期" prop="checkTime">
        <el-date-picker
          v-model="queryParams.checkTime"
          type="date"
          placeholder="选择日期"
          style="width: 180px"
        >
        </el-date-picker>
      </el-form-item>
      <el-form-item label="合格状态" prop="isCheckOk">
        <el-select
          @clear="clearQueryParams('isCheckOk')"
          v-model="queryParams.isCheckOk"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option :value="'1'" :label="'合格'" />
          <el-option :value="'0'" :label="'不合格'" />
          <el-option :value="'-1'" :label="'待检验'" />
        </el-select>
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
          :disabled="hasPermi(['produce:checkrecords:export'])"
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
      :data="deviceList"
      @selection-change="handleSelectionChange"
    >
      <el-table-column
        label="任务编码"
        key="taskCode"
        prop="taskCode"
        fixed="left"
        min-width="200"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
      </el-table-column>

      <el-table-column
        label="工单名称"
        min-width="200"
        key="workOrderName"
        prop="workOrderName"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="工单编码"
        min-width="200"
        key="workOrderCode"
        prop="workOrderCode"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="检验人"
        align="center"
        key="userName"
        prop="userName"
        show-overflow-tooltip
        min-width="150"
        v-if="columns[3].visible"
      />
      <el-table-column
        label="合格状态"
        align="center"
        key="isCheckOk"
        prop="isCheckOk"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isCheckOk == 1">合格</el-tag>
          <el-tag v-else-if="scope.row.isCheckOk == 0" type="danger"
            >不合格</el-tag
          >
          <el-tag v-else type="warning">待检验</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        key="remark"
        prop="remark"
        min-width="150"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <el-input
            v-model="scope.row.remark"
            placeholder="请输入备注"
            v-if="!scope.row.inpShow"
            @keyup.enter.native="clickOk(scope.row)"
          />
          <tooltip v-else :value="scope.row.remark" />
        </template>
      </el-table-column>
      <el-table-column
        label="检验日期"
        key="checkTime"
        align="center"
        min-width="180"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.checkTime">{{
            parseTime(scope.row.checkTime)
          }}</span>
          <el-tag v-else type="danger">未检验</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="220px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <div v-if="scope.row.inpShow">
            <el-button
              :style="{ paddingRight: '10px' }"
              type="text"
              icon="el-icon-edit"
              :disabled="hasPermi(['produce:checkrecords:edit'])"
              @click="handleUpdate(scope.row)"
              >修改</el-button
            >
            <el-popconfirm
              class="isCheckChange"
              title="只能操作一次，确定吗？"
              @confirm="handleIsCheckOkChange(scope.row, '合格')"
            >
              <el-button
                slot="reference"
                type="text"
                icon="el-icon-circle-check"
                :disabled="
                  hasPermi(['produce:checkrecords:changeCheck']) ||
                  scope.row.isCheckOk != '-1'
                "
                >合格</el-button
              >
            </el-popconfirm>

            <el-popconfirm
              title="只能操作一次，确定吗？"
              @confirm="handleIsCheckOkChange(scope.row, '不合格')"
            >
              <el-button
                slot="reference"
                type="text"
                icon="el-icon-warning-outline"
                :disabled="
                  hasPermi(['produce:checkrecords:changeCheck']) ||
                  scope.row.isCheckOk != '-1'
                "
                >不合格</el-button
              >
            </el-popconfirm>
          </div>
          <div v-else>
            <el-button
              type="text"
              icon="el-icon-edit"
              @click="clickOk(scope.row)"
              >确定</el-button
            >
            <el-button
              type="text"
              icon="el-icon-close"
              @click="cancelOk(scope.row)"
              >取消</el-button
            >
          </div>
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

    <!-- 添加或修改产品对话框 -->
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
        <el-form-item label="是否合格">
          <el-radio-group v-removeAriaHidden v-model="form.isCheckOk">
            <el-radio :label="'1'">合格</el-radio>
            <el-radio :label="'0'">不合格</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="备注" prop="remark">
          <el-input
            type="textarea"
            v-model="form.remark"
            placeholder="请输入备注"
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
  listCheckRecords,
  getCheckRecords,
  addCheckRecords,
  putCheck,
} from "@/api/produce/checkRecords";

export default {
  name: "CheckRecords", 
  data() {
    return {
      page: "checkRecords",
      optType: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 产品表格数据
      deviceList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        taskId: undefined,
        taskName: undefined,
        taskCode: undefined,
        workOrderId: undefined,
        workOrderName: undefined,
        workOrderCode: undefined,
        userId: undefined,
        userName: undefined,
        isCheckOk: undefined,
        checkTime: undefined,
        remark: undefined,
      },
      // 表单参数
      form: {},
      // 表单校验
      initialForm: {},
      rules: {
        name: [
          { required: true, message: "产品名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "产品顺序不能为空", trigger: "blur" },
        ],
        isCheckOk: [{ required: true }],
      },
      // 列信息
      columns: [
        { key: 0, label: "任务编码", visible: true },
        { key: 1, label: "工单名称", visible: true },
        { key: 2, label: "工单编码", visible: true },
        { key: 3, label: "检验人", visible: true },
        { key: 4, label: "合格状态", visible: true },
        { key: 5, label: "备注", visible: true },
        { key: 6, label: "检验日期", visible: true },
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
    /** 查询产品列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listCheckRecords(this.queryParams);
      this.deviceList = res.data.list.map((v) => {
        return { ...v, inpShow: true };
      });
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 表单重置
    reset() {
      this.form = {
        id: undefined,
        status: undefined,
        isCheckOk: undefined,
        remark: undefined,
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
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const deviceId = row.id || this.ids;
      getCheckRecords(deviceId).then((res) => {
        if (res.code == 0) {
          this.optType = "edit";
          this.form = {
            ...res.data,
            remark: res.data.remark ? res.data.remark : "",
          };
          this.initialForm = Object.assign(
            {},
            { ...res.data, remark: res.data.remark ? res.data.remark : "" }
          );
          this.open = true;
          this.title = "修改记录";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        putCheck(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addCheckRecords(this.form).then((res) => {
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
    // 合格与不合格
    handleIsCheckOkChange(row, type) {
      if (type == "合格") {
        getCheckRecords(row.id).then((res) => {
          if (res.code == 0) {
            row.inpShow = false;
            this.form.isCheckOk = "1";
            this.form.id = res.data.id;
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        getCheckRecords(row.id).then((res) => {
          if (res.code == 0) {
            row.inpShow = false;
            this.form.isCheckOk = "0";
            this.form.id = res.data.id;
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 点击确定
    clickOk(row) {
      this.form.remark = row.remark;
      putCheck(this.form)
        .then((res) => {
          if (res.code == 0) {
            row.inpShow = true;
            row.disabled = true;
            this.$modal.msgSuccess("操作成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        })
        .catch(() => {});
    },
    // 点击取消
    cancelOk(row) {
      row.inpShow = true;
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel("/v1/CheckRecords/DownLoadList", "检验记录.xlsx");
    },
  },
};
</script>
<style lang="scss">
.isCheckChange {
  margin-right: 10px;
}
</style>
