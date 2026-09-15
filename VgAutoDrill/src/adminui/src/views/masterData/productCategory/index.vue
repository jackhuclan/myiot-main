<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="产品大类编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入产品大类编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="产品大类名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入产品大类名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="审批状态" prop="vettingStatus">
        <el-select
          @clear="clearQueryParams('vettingStatus')"
          v-model="queryParams.vettingStatus"
          placeholder="请选择"
          clearable
          style="width: 120px"
        >
          <el-option label="已审批" :value="1" />
          <el-option label="未审批" :value="0" />
        </el-select>
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-select
          @clear="clearQueryParams('status')"
          v-model="queryParams.status"
          placeholder="请选择"
          clearable
          style="width: 120px"
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
          :disabled="hasPermi(['masterData:productCategory:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="warning"
          plain
          icon="el-icon-check"
          :disabled="hasPermi(['masterData:productCategory:commit'])"
          @click="handleApproval"
          >批量审批</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          plain
          type="warning"
          icon="el-icon-refresh-left"
          @click="handleRevoke"
          :disabled="hasPermi(['masterData:productCategory:revoke'])"
        >
          批量撤销
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['masterData:productCategory:remove'])"
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
          v-hasPermi="['masterData:productCategory:import']"
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
      :data="productCategoryList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="大类编码"
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
            v-isGetSelection="['masterData:productCategory:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="大类名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="150"
      />
      <el-table-column
        label="建议机台数"
        key="dispenseMachines"
        prop="dispenseMachines"
        show-overflow-tooltip
        align="center"
        v-if="columns[2].visible"
        min-width="150"
      />

      <el-table-column
        label="审批状态"
        align="center"
        key="vettingStatus"
        prop="vettingStatus"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.vettingStatus == 1">已审批</el-tag>
          <el-tag type="danger" v-else>未审批</el-tag>
        </template>
      </el-table-column>
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
        label="备注"
        key="remark"
        prop="remark"
        v-if="columns[5].visible"
        min-width="180"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
        </template>
      </el-table-column>
      <el-table-column
        label="创建时间"
        align="center"
        key="createTime"
        prop="createTime"
        width="180"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
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
            :disabled="hasPermi(['masterData:productCategory:edit'])"
            >修改</el-button
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
                command="handleApproval"
                :disabled="
                  hasPermi(['masterData:productCategory:commit']) ||
                  scope.row.vettingStatus != 0
                "
                icon="el-icon-check"
                >审批</el-dropdown-item
              >

              <el-dropdown-item
                command="handleRevoke"
                icon="el-icon-refresh-left"
                :disabled="
                  hasPermi(['masterData:productCategory:revoke']) ||
                  scope.row.vettingStatus != 1
                "
                >撤销</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="
                  hasPermi(['masterData:productCategory:remove']) ||
                  scope.row.vettingStatus == 1
                "
                >删除</el-dropdown-item
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

    <!-- 添加或修改产品大类对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isListChange="isListChange"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="100px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="大类编码" prop="code">
              <el-input
                :disabled="optType == 'view' || form.vettingStatus == 1"
                v-model="form.code"
                placeholder="请输入产品大类编码"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="大类名称" prop="name">
              <el-input
                :disabled="optType == 'view' || form.vettingStatus == 1"
                v-model="form.name"
                placeholder="请输入产品大类名称"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="建议机台数" prop="dispenseMachines">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :dis="optType == 'view' || form.vettingStatus == 1"
                :myNum="form.dispenseMachines"
                @changeNum="changeNum"
                :numName="'dispenseMachines'"
                :min="1"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item
              v-if="title == '修改产品大类'"
              label="状态"
              prop="status"
            >
              <el-radio-group
                v-removeAriaHidden
                v-model="form.status"
                :disabled="optType == 'view' || form.vettingStatus == 1"
              >
                <el-radio :label="1">正常</el-radio>
                <el-radio :label="0">停用</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col>
            <el-form-item label="备注" prop="remark">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.remark"
                placeholder="请输入备注"
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <el-tabs type="border-card" v-if="form.id != null">
        <!-- 关联组成工序 -->
        <el-tab-pane label="关联工艺路线">
          <ProductCategoryAndRoute
            @father_watch="fatherWatch"
            :optType="optType"
            :productCategoryId="form.id"
            ref="productCategoryAndRoute"
          />
        </el-tab-pane>
      </el-tabs>
    </edit-form-dialog>

    <!-- 产品大类导入 -->
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
  listProductCategory,
  getProductCategory,
  delProductCategory,
  addProductCategory,
  updateProductCategory,
  delList,
  vettingProductCategory,
  cncelVettingProductCategory,
} from "@/api/masterData/productCategory";
import ProductCategoryAndRoute from "./productCategoryAndRoute";
export default {
  name: "ProductCategory",
  dicts: ["sys_normal_disable"],
  components: { ProductCategoryAndRoute },
  data() {
    // 自定义校验
    const dispenseMachinesChage = (rule, value, callback) => {
      if (this.form.dispenseMachines == null) {
        callback(new Error("请输入机台数"));
      } else if (this.form.dispenseMachines <= 0) {
        callback(new Error("机台数不得小于等于0"));
      } else {
        callback();
      }
    };
    return {
      page: "productCategory",
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
      // 产品大类表格数据
      productCategoryList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        remark: undefined,
        status: undefined,
        vettingStatus: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        name: [
          { required: true, message: "产品大类名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "产品大类编码不能为空", trigger: "blur" },
        ],
        dispenseMachines: [
          {
            required: true,
            validator: dispenseMachinesChage,
            trigger: "change",
          },
        ],
      },
      // 产品大类导入的参数
      // 导入的url
      uploadUrl: "/v1/ProductCategory/UploadList",
      // 下载的url
      downloadUrl: "/v1/ProductCategory/DownLoad",
      // 导入的模板下载名
      fileName: "产品大类模板.xlsx",
      // 监听嵌入的表格是否有变化
      listChange: 0,
      isListChange: false,
      // 监听表单是否有变化(与原来的form做对比)
      prevform: {},
      // 列信息
      columns: [
        { key: 0, label: "大类编码", visible: true },
        { key: 1, label: "大类名称", visible: true },
        { key: 2, label: "建议及台数", visible: true },
        { key: 3, label: "审批状态", visible: true },
        { key: 4, label: "状态", visible: true },
        { key: 5, label: "备注", visible: true },
        { key: 6, label: "创建时间", visible: true },
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
    /** 查询产品大类列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listProductCategory(this.queryParams);
      this.productCategoryList = res.data.list;
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

    // 监听嵌入表格数据是否有变化
    fatherWatch() {
      if (this.optType == "edit") {
        // 等于编辑时才去监听
        this.listChange++; // 默认值有变更的话
        if (this.listChange >= 1) {
          // 说明监听值有变化
          this.isListChange = true; // 弹框提示
        }
      }
    },
    // 表单重置
    reset() {
      this.form = {
        code: "",
        name: "",
        dispenseMachines: 2,
        remark: "",
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
      this.initialForm = Object.assign({}, this.form);
      this.open = true;
      this.title = "添加产品大类";
      this.optType = "add";
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const id = row.id || this.ids;
      getProductCategory(id).then((res) => {
        if (res.code == 0) {
          this.listChange = 0;
          this.isListChange = false;
          this.form = res.data;
          this.form.dispenseMachines =
            res.data.dispenseMachines == null ? 0 : res.data.dispenseMachines;
          this.prevform = JSON.stringify(res.data);
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.optType = "edit";
          this.title = "修改产品大类";
          this.$nextTick(() => {
            this.$refs.productCategoryAndRoute.getList();
          });
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 查询明细按钮操作
    handleView(id) {
      getProductCategory(id).then((res) => {
        if (res.code == 0) {
          this.listChange = 0;
          this.isListChange = false;
          this.form = res.data;
          this.prevform = JSON.stringify(res.data);
          this.open = true;
          this.optType = "view";
          this.title = "查看产品大类";
          this.$nextTick(() => {
            this.$refs.productCategoryAndRoute.getList();
          });
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 审批操作
    async handleApproval(row) {
      if (!row.id && this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行审批？")
        .catch(() => {});
      if (results == "confirm") {
        const vettingProductCategorys = row.id
          ? [
              {
                productCategoryId: row.id,
              },
            ]
          : this.ids.map((v) => {
              return { productCategoryId: v };
            });
        vettingProductCategory({ vettingProductCategorys }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("审批成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 撤销操作
    async handleRevoke(row) {
      if (!row.id && this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行撤销？")
        .catch(() => {});
      if (results == "confirm") {
        const vettingProductCategorys = row.id
          ? [
              {
                productCategoryId: row.id,
              },
            ]
          : this.ids.map((v) => {
              return { productCategoryId: v };
            });
        cncelVettingProductCategory({ vettingProductCategorys }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("撤销成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleApproval":
          this.handleApproval(row);
          break;
        case "handleRevoke":
          this.handleRevoke(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateProductCategory(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addProductCategory(this.form).then((res) => {
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
          delProductCategory,
          this.getList,
          "产品大类编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "产品大类导入";
      this.$refs.upload.open = true;
    },
  },
};
</script>
