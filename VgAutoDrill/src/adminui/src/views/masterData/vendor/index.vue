<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="供应商编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入供应商编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="供应商名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入供应商名称"
          clearable
          @keyup.enter.native="handleQuery"
        /> </el-form-item
    ></search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['masterData:vendor:add'])"
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
          :disabled="hasPermi(['masterData:vendor:remove'])"
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
          :disabled="hasPermi(['masterData:vendor:import'])"
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
      :data="vendorList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="供应商编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        v-if="columns[0].visible"
        min-width="150"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['masterData:vendor:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="供应商名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="150"
      />
      <el-table-column
        label="状态"
        align="center"
        key="status"
        prop="status"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <dict-tag
            :options="dict.type.sys_normal_disable"
            :value="scope.row.status"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        min-width="180"
        key="remark"
        prop="remark"
        v-if="columns[3].visible"
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
            :disabled="hasPermi(['masterData:vendor:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['masterData:vendor:remove'])"
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

    <!-- 添加或修改供应商对话框 -->
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
          <el-col :span="10">
            <el-form-item label="供应商编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入供应商编码"
                :disabled="autoGenFlag || optType != 'add'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="4">
            <el-form-item label-width="80">
              <el-switch
                v-model="autoGenFlag"
                active-color="#13ce66"
                active-text="自动生成"
                @change="handleAutoGenChange(autoGenFlag)"
                :disabled="optType != 'add'"
              >
              </el-switch>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="供应商名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入供应商名称" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8" v-if="title == '修改供应商'">
            <el-form-item label="状态" prop="status">
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio :label="1">正常</el-radio>
                <el-radio :label="0">停用</el-radio>
              </el-radio-group>
            </el-form-item>
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
    <!-- 供应商导入 -->
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
  listVendor,
  getVendor,
  delVendor,
  addVendor,
  updateVendor,
  delList,
} from "@/api/masterData/vendor";
export default {
  name: "Vendor",
  dicts: ["sys_normal_disable"],
  data() {
    return {
      page: "vendor",
      //自动生成编码
      autoGenFlag: false,
      enCode: "",
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
      // 供应商表格数据
      vendorList: [],
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
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "供应商编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "供应商名称不能为空", trigger: "blur" },
        ],
        status: [{ required: true, message: "状态不能为空", trigger: "blur" }],
      },
      // 供应商导入参数
      // 导入的url
      uploadUrl: "/v1/Vendor/UploadList",
      // 下载的url
      downloadUrl: "/v1/Vendor/DownLoad",
      // 导入的模板下载名
      fileName: "供应商模板.xlsx",
      // 列信息
      columns: [
        { key: 0, label: "供应商编码", visible: true },
        { key: 1, label: "供应商名称", visible: true },
        { key: 2, label: "状态", visible: true },
        { key: 3, label: "备注", visible: true },
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
    /** 查询供应商列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listVendor(this.queryParams);
      this.vendorList = res.data.list;
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
        code: "",
        name: "",
        remark: "",
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
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加供应商";
      this.optType = "add";
      this.autoGenFlag = false;
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getVendor(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看供应商";
          this.optType = "view";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const vendorId = row.id || this.ids;
      getVendor(vendorId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.enCode = res.data.code;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改供应商";
          this.optType = "edit";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateVendor(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addVendor(this.form).then((res) => {
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
          delVendor,
          this.getList,
          "供应商编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "供应商导入";
      this.$refs.upload.open = true;
    },
    //自动生成编码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "VENDOR_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
        });
      } else {
        if (this.optType == "edit") return (this.form.code = this.enCode);
        this.form.code = "";
      }
    },
  },
};
</script>
