<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="通知编码" prop="notifyCode">
        <el-input
          v-trim
          v-model="queryParams.notifyCode"
          placeholder="请输入通知编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="通知名称" prop="notifyName">
        <el-input
          v-trim
          v-model="queryParams.notifyName"
          placeholder="请输入通知名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-select
          @clear="clearQueryParams('status')"
          v-model="queryParams.status"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option
            v-for="dict in dict.type.sys_normal_disable"
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
          :disabled="hasPermi(['notify:record:add'])"
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
          :disabled="hasPermi(['notify:record:remove'])"
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
          :disabled="hasPermi(['notify:record:export'])"
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
      :data="notifyList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="通知编码"
        key="notifyCode"
        prop="notifyCode"
        show-overflow-tooltip
        v-if="columns[0].visible"
        min-width="150px"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['notify:record:view']"
            >{{ scope.row.notifyCode }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="通知名称"
        key="notifyName"
        prop="notifyName"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="150px"
      />
      <el-table-column
        label="通知内容"
        min-width="150px"
        key="notifyMsg"
        prop="notifyMsg"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.notifyMsg" /></template
      ></el-table-column>
      <el-table-column
        label="通知方式"
        align="center"
        key="notifyWaysName"
        prop="notifyWaysName"
        show-overflow-tooltip
        v-if="columns[3].visible"
      />
      <el-table-column
        label="状态"
        align="center"
        key="status"
        prop="status"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <dict-tag
            :options="dict.type.sys_normal_disable"
            :value="scope.row.status"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="通知时间"
        align="center"
        key="notifyTime"
        prop="notifyTime"
        min-width="180"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.notifyTime) }}</span>
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
            :disabled="hasPermi(['notify:record:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['notify:record:remove'])"
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

    <!-- 添加或修改通知对话框 -->
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
            <el-form-item label="通知编码" prop="notifyCode">
              <el-input
                v-model="form.notifyCode"
                placeholder="请输入通知编码"
              /> </el-form-item
          ></el-col>
          <el-col :span="12">
            <el-form-item label="通知名称" prop="notifyName">
              <el-input
                v-model="form.notifyName"
                placeholder="请输入通知名称"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="通知方式" prop="notifyWaysName">
              <el-input
                v-model="form.notifyWaysName"
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
        <el-row>
          <el-col :span="24">
            <el-form-item label="通知内容" prop="notifyMsg">
              <el-input
                type="textarea"
                v-model="form.notifyMsg"
                placeholder="请输入内容"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              /> </el-form-item
          ></el-col>
        </el-row>

        <el-row>
          <el-col :span="12">
            <el-form-item
              label="通知状态"
              v-if="title == '修改通知'"
              prop="status"
            >
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio
                  v-for="dict in dict.type.sys_normal_disable"
                  :key="dict.value"
                  :label="dict.value"
                  >{{ dict.label }}</el-radio
                >
              </el-radio-group>
            </el-form-item></el-col
          >
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listNotify,
  getNotify,
  delNotify,
  addNotify,
  updateNotify,
  delList,
} from "@/api/notify/notify";
import NotifySelect from "@/components/notifySelect/radio.vue";

export default {
  name: "Notify",
  components: {
    NotifySelect,
  },
  dicts: ["sys_normal_disable"],
  data() {
    return {
      page: "notify",
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
      // 通知表格数据
      notifyList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        notifyCode: undefined,
        notifyName: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        notifyName: [
          { required: true, message: "通知名称不能为空", trigger: "blur" },
        ],
        notifyCode: [
          { required: true, message: "通知编码不能为空", trigger: "blur" },
        ],
        notifyMsg: [
          { required: true, message: "通知内容不能为空", trigger: "blur" },
        ],
        notifyWays: [
          { required: true, message: "通知方式不能为空", trigger: "blur" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "通知编码", visible: true },
        { key: 1, label: "通知名称", visible: true },
        { key: 2, label: "通知内容", visible: true },
        { key: 3, label: "通知方式", visible: true },
        { key: 4, label: "状态", visible: true },
        { key: 5, label: "通知时间", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    "form.notifyWaysName": {
      handler(val) {
        if (val == "") {
          this.form.notifyWays = undefined;
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
    /** 查询通知列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listNotify(this.queryParams);
      this.notifyList = res.data.list;
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
        notifyCode: "",
        notifyName: "",
        notifyWaysName: "",
        notifyMsg: "",
        notifyWays: undefined,
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
      this.title = "添加通知";
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const notificationId = row.id || this.ids;
      getNotify(notificationId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.status = this.form.status + "";
          this.initialForm = Object.assign({}, res.data);
          this.optType = "edit";
          this.open = true;
          this.title = "修改通知";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 点击编码查看操作 */
    handleView(id) {
      this.reset();
      getNotify(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.status = this.form.status + "";
          this.optType = "view";
          this.open = true;
          this.title = "查看通知";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateNotify(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addNotify({
          ...this.form,
          notifyTime: new Date().toLocaleString(),
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
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delNotify,
          this.getList,
          "类型编码为" + row.notifyCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 选择通知方式
    handleNotifySelect() {
      this.$refs.notifySelect.showFlag = true;
      this.$refs.notifySelect.selectedNotifyWaysId = this.form.notifyWays
        ? this.form.notifyWays
        : undefined;
      // 获取数据
      this.$refs.notifySelect.getList();
    },
    onNotifySelected(row) {
      if (row != null && row != undefined) {
        this.$set(this.form, "notifyWaysName", row.name);
        this.$set(this.form, "notifyWays", row.id);
      }
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel("/v1/Notify/DownLoadList", "通知.xlsx");
    },
  },
};
</script>
