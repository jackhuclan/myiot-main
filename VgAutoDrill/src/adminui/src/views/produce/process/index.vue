<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="工序编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入工序编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工序名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入工序名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="是否启用" prop="status">
        <el-select
          @clear="clearQueryParams('status')"
          clearable
          v-model="queryParams.status"
          placeholder="请选择"
          style="width: 150px"
        >
          <el-option :label="'是'" :value="1"> </el-option>
          <el-option :label="'否'" :value="0"> </el-option>
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
          :disabled="hasPermi(['produce:process:add'])"
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
          :disabled="hasPermi(['produce:process:remove'])"
          >批量删除</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-upload2"
          @click="handleImport"
          :disabled="hasPermi(['produce:process:import'])"
          >导入</el-button
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
      :data="processList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="工序编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['produce:process:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="工序名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[1].visible"
      />
      <el-table-column
        label="是否启用"
        key="status"
        align="center"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="工艺要求"
        min-width="180px"
        key="attention"
        prop="attention"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.attention" />
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
            :disabled="hasPermi(['produce:process:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['produce:process:remove'])"
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

    <!-- 添加或修改生产工序对话框 -->
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
            <el-form-item label="工序编码" prop="code">
              <el-input v-model="form.code" placeholder="请输入工序编码" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="工序名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入工序名称" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12" v-if="title == '修改生产工序'">
            <el-form-item label="是否启用" prop="status">
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio :label="1"> 是 </el-radio>
                <el-radio :label="0"> 否 </el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="工序要求" prop="attention">
              <el-input
                v-model="form.attention"
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
    <!-- 工序导入 -->
    <ImportXlsx
      ref="upload"
      @getList="getList"
      :uploadUrl="uploadUrl"
      :downloadUrl="downloadUrl"
      :fileName="fileName"
    ></ImportXlsx>
  </div>
</template>

<script>
import {
  listProcess,
  getProcess,
  delProcess,
  addProcess,
  updateProcess,
  delList,
} from "@/api/produce/process";

export default {
  name: "Process", 
  data() {
    return {
      page: "process",
      //自动生成编码
      autoGenFlag: false,
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
      // 生产工序表格数据
      processList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: null,
        name: null,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "工序编码不能为空", trigger: "blur" },
        ],
        name: [
          { required: true, message: "工序名称不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "是否启用不能为空", trigger: "blur" },
        ],
      },
      // 工序导入参数
      // 导入的url
      uploadUrl: "/v1/Process/UploadList",
      downloadUrl: "/v1/Process/DownLoad",
      // 导入的模板下载名
      fileName: "工序模板.xlsx",
      // 列信息
      columns: [
        { key: 0, label: "工序编码", visible: true },
        { key: 1, label: "工序名称", visible: true },
        { key: 2, label: "是否启用", visible: true },
        { key: 3, label: "工艺要求", visible: true },
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
    /** 查询生产工序列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listProcess(this.queryParams);
      this.processList = res.data.list;
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
        attention: "",
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
      this.open = true;
      this.title = "添加生产工序";
      this.optType = "add";
      this.initialForm = Object.assign({}, this.form);
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getProcess(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看工序信息";
          this.optType = "view";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const id = row.id || this.ids;
      getProcess(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "修改生产工序";
          this.optType = "edit";
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateProcess(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addProcess(this.form).then((res) => {
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
          delProcess,
          this.getList,
          "工序编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "工序导入";
      this.$refs.upload.open = true;
    },
    //自动生成编码
    async handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        // const res=await genCode('PROCESS_CODE')
        //   this.form.code = response;
      } else {
        this.form.code = null;
      }
    },
  },
};
</script>
