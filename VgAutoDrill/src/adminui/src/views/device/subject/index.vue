<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="项目编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入项目编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="项目名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入项目名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="项目类型" prop="subjectType">
        <el-select
          @clear="clearQueryParams('subjectType')"
          v-model="queryParams.subjectType"
          placeholder="请选择"
          clearable
          style="width: 120px"
        >
          <el-option :value="'点检'">点检</el-option>
          <el-option :value="'保养'">保养</el-option>
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
          :disabled="hasPermi(['device:subject:add'])"
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
          :disabled="hasPermi(['device:subject:remove'])"
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
      :data="dvsubjectList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="项目编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        v-if="columns[0].visible"
        min-width="150px"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['device:subject:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="项目名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="150px"
      />
      <el-table-column
        label="项目类型"
        min-width="150px"
        align="center"
        key="subjectType"
        prop="subjectType"
        v-if="columns[2].visible"
      />

      <el-table-column
        label="标准"
        min-width="150"
        key="standard"
        prop="standard"
        show-overflow-tooltip
        v-if="columns[3].visible"
      />

      <el-table-column
        label="备注"
        min-width="150px"
        key="subjectContent"
        prop="subjectContent"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.subjectContent" />
        </template>
      </el-table-column>
      <el-table-column
        label="是否启用"
        align="center"
        key="status"
        prop="status"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
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
            :disabled="hasPermi(['device:subject:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['device:subject:remove'])"
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

    <!-- 添加或修改点检项目对话框 -->
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
            <el-form-item label="项目编码" prop="code">
              <el-input v-model="form.code" placeholder="请输入项目编码" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="项目名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入项目名称" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="项目类型" prop="subjectType">
              <el-select
                v-model="form.subjectType"
                multiple
                collapse-tags
                filterable
                allow-create
                default-first-option
                placeholder="请选择"
              >
                <el-option :value="'点检'">点检</el-option>
                <el-option :value="'保养'">保养</el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12" v-if="form.id != null">
            <el-form-item label="是否启用" prop="status">
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio :label="1">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="标准" prop="standard">
          <el-input
            type="textarea"
            v-model="form.standard"
            placeholder="请输入标准"
            :autosize="{ minRows: 2, maxRows: 10 }"
            :resize="optType == 'view' ? 'none' : ''"
          />
        </el-form-item>
        <el-form-item label="备注">
          <el-input
            type="textarea"
            v-model="form.subjectContent"
            placeholder="请输入备注"
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
  listSubject,
  getSubject,
  delSubject,
  delList,
  addSubject,
  updateSubject,
} from "@/api/device/subject";
export default {
  name: "Subject",
  data() {
    return {
      page: "subject",
      optType: "",
      autoGenFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组s
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 点检项目表格数据
      dvsubjectList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        subjectType: undefined,
        subjectContent: undefined,
        standard: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "项目编码不能为空", trigger: "blur" },
        ],
        name: [
          { required: true, message: "项目名称不能为空", trigger: "blur" },
        ],
        subjectType: [
          { required: true, message: "请选择项目类型", trigger: "change" },
        ],
        subjectContent: [
          { required: true, message: "备注不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "是否启用不能为空", trigger: "blur" },
        ],
        standard: [
          { required: true, message: "标准不能为空", trigger: "blur" },
        ],
      },
      recallArrId: [],
      // 列信息
      columns: [
        { key: 0, label: "项目编码", visible: true },
        { key: 1, label: "项目名称", visible: true },
        { key: 2, label: "项目类型", visible: true },
        { key: 3, label: "标准", visible: true },
        { key: 4, label: "备注", visible: true },
        { key: 5, label: "是否启用", visible: true },
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
    /** 查询点检项目列表 */
    getList(isSearch) {
      this.loading = true;
      listSubject(this.queryParams).then((res) => {
        this.dvsubjectList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
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
        name: "",
        code: "",
        subjectType: [],
        subjectContent: "",
        standard: "",
      };
      this.autoGenFlag = false;
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
      this.title = "添加点检项目";
      this.recallArrId = null;
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const subjectId = row.id || this.ids;
      getSubject(subjectId).then((res) => {
        if (res.code == 0) {
          this.optType = "edit";
          this.form = res.data;
          this.form.subjectType = res.data.subjectType?.split(",");
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改点检项目";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 点击编码操作 */
    handleView(id) {
      this.reset();
      getSubject(id).then((res) => {
        if (res.code == 0) {
          this.optType = "view";
          this.form = res.data;
          this.form.subjectType = res.data.subjectType?.split(",");
          this.open = true;
          this.title = "查看点检项目";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      const subjectType = this.form.subjectType.join(",");
      if (this.form.id != null) {
        updateSubject({ ...this.form, subjectType }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addSubject({ ...this.form, subjectType }).then((res) => {
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
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delSubject,
          this.getList,
          "项目编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
  },
};
</script>
