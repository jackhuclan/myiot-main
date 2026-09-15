<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="配置名称" prop="configName">
        <el-input
          v-trim
          v-model="queryParams.configName"
          placeholder="请输入配置名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="状态" prop="status">
        <el-select
          v-model="queryParams.status"
          placeholder="请选择"
          clearable
          @clear="clearQueryParams('status')"
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
          :disabled="hasPermi(['material:config:add'])"
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
          :disabled="hasPermi(['material:config:remove'])"
          >批量删除</el-button
        >
      </el-col>
      <!-- <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-upload2"
          @click="handleImport"
          :disabled="hasPermi([' material:config:import'])"
          >导入</el-button
        >
      </el-col> -->
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
      :data="cutterMasterList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="配置名称"
        min-width="160"
        key="configName"
        prop="configName"
        show-overflow-tooltip
        fixed="left"
        v-if="columns[0].visible"
      />

      <el-table-column
        label="文件路径"
        min-width="160"
        key="diaFilePath"
        prop="diaFilePath"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="文件名称"
        min-width="160"
        key="diaFileName"
        prop="diaFileName"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />

      <el-table-column
        label="大类编码"
        min-width="160"
        key="productCategoryCode"
        prop="productCategoryCode"
        show-overflow-tooltip
        v-if="columns[3].visible"
      />
      <el-table-column
        label="大类名称"
        min-width="160"
        key="productCategoryName"
        prop="productCategoryName"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />

      <el-table-column
        label="描述"
        min-width="160"
        key="configDesc"
        prop="configDesc"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.configDesc"
        /></template>
      </el-table-column>
      <el-table-column
        label="状态"
        align="center"
        key="status"
        prop="status"
        width="60"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <dict-tag
            :options="dict.type.sys_normal_disable"
            :value="scope.row.status"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="创建时间"
        align="center"
        key="createTime"
        prop="createTime"
        show-overflow-tooltip
        min-width="180px"
        v-if="columns[7].visible"
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
            :disabled="hasPermi(['material:config:edit'])"
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
                command="handleSee"
                icon="el-icon-view"
                :disabled="hasPermi(['material:config:view'])"
                >查看</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['material:config:remove'])"
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

    <!-- 添加或修改刀具参数对话框 -->
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
            <el-form-item label="配置名称" prop="configName">
              <el-input v-model="form.configName" placeholder="请输入名称" />
            </el-form-item>
          </el-col>

          <el-col :span="12">
            <el-form-item label="文件路径" prop="diaFilePath">
              <UpLoadFile
                ref="diaFileUpload"
                :onChange="fileChangeDiaFilePath"
                :onRemove="onRemoveDiaFileList"
                :fileList="diaFileList"
                :btnText="'选择刀具文件'"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="大类编码" prop="productCategoryCode">
              <el-input
                v-model="form.productCategoryCode"
                placeholder="请输入大类编码"
              >
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleSelectProductCategory"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <ProductCategorySelect
                ref="productCategorySelect"
                @onSelected="onProductCategorySelected"
              ></ProductCategorySelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="大类名称" prop="productCategoryName">
              <el-input
                v-model="form.productCategoryName"
                placeholder="请输入大类名称"
                readonly="readonly"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              label="状态"
              v-if="title == '修改刀具参数'"
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
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col>
            <el-form-item label="描述">
              <el-input
                v-model="form.configDesc"
                placeholder="请输入描述"
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <el-divider v-if="form.id != null" content-position="center"
        >刀具参数明细</el-divider
      >
      <ConfigDetail
        v-if="form.id != null && open"
        :optType="optType"
        ref="ConfigDetail"
      />
    </edit-form-dialog>
    <!-- 刀具参数导入 -->
    <ImportXlsx
      ref="upload"
      @getList="getList"
      :uploadUrl="uploadUrl"
      :fileName="fileName"
    ></ImportXlsx>
  </div>
</template>

<script>
import {
  listCutterConfigMaster,
  getCutterConfigMaster,
  delCutterConfigMaster,
  addCutterConfigMaster,
  updateCutterConfigMaster,
  upLoad,
  delList,
} from "@/api/material/config";
// 刀具参数
import ConfigDetail from "./diaFileDetail.vue";
// 产品大类选择
import ProductCategorySelect from "@/components/productCategorySelect/radio.vue";
export default {
  name: "DiaFile",
  dicts: ["sys_normal_disable"],
  components: { ConfigDetail, ProductCategorySelect },
  data() {
    // 自定义校验规则
    const diaFileRule = (rule, value, callback) => {
      if (this.diaFileList.length <= 0) {
        callback(new Error("未选择文件"));
      } else {
        callback();
      }
    };
    return {
      page: "diaFile",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      optType: "",
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 刀具参数表格数据
      cutterMasterList: [],

      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        configName: undefined,
        configDesc: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        configName: [
          { required: true, message: "配置名称不能为空", trigger: "blur" },
        ],
        configDesc: [
          { required: true, message: "描述不能为空", trigger: "blur" },
        ],
        diaFilePath: [
          { required: true, validator: diaFileRule, trigger: "change" },
        ],
        diaFileName: [
          { required: true, message: "dia文件名称不能为空", trigger: "change" },
        ],
        productCategoryCode: [
          { required: true, message: "大类编码不能为空", trigger: "change" },
        ],
        productCategoryName: [
          { required: true, message: "大类名称不能为空", trigger: "change" },
        ],
      },
      masterId: undefined,
      // 用户导入参数
      // 导入的url
      uploadUrl: "/v1/UnitMeasure/UploadUnitList",
      // 导入的模板下载名
      fileName: "计量单位模板.xlsx",
      // 下载模板的url
      downloadUrl: "/v1/UnitMeasure/DownLoad",
      // 刀具文件列表
      diaFileList: [],
      // 列信息
      columns: [
        { key: 0, label: "配置名称", visible: true },
        { key: 1, label: "文件路径", visible: true },
        { key: 2, label: "文件名称", visible: true },
        { key: 3, label: "大类编码", visible: true },
        { key: 4, label: "大类名称", visible: true },
        { key: 5, label: "描述", visible: true },
        { key: 6, label: "状态", visible: true },
        { key: 7, label: "创建时间", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    diaFileList: {
      handler(val) {
        if (val.length > 0) {
          if (val[0].raw) {
            this.upLoadFile(val[0].raw, upLoad).then((res) => {
              this.form.diaFilePath = res;
              this.form.diaFileName = val[0].name;
            });
          }
        } else {
          this.form.diaFilePath = undefined;
          this.form.diaFileName = undefined;
        }
      },
      deep: true,
    },
    "form.productCategoryCode": {
      handler(val) {
        if (val == "") {
          this.form.productCategoryId = undefined;
          this.form.productCategoryName = undefined;
          this.form.productCategoryCode = undefined;
        }
      },
      deep: true,
    },
    open(val) {
      if (!val) {
        this.onRemoveDiaFileList();
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
    /** 查询刀具参数列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listCutterConfigMaster(this.queryParams);
      this.cutterMasterList = res.data.list;
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
        configName: "",
        configDesc: "",
        diaFilePath: undefined,
        diaFileName: undefined,
        productCategoryId: undefined,
        productCategoryCode: undefined,
        productCategoryName: undefined,
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
      this.title = "添加刀具参数";
      this.optType = "add";
      // 清空文件列表
      this.$nextTick(() => {
        this.$refs.diaFileUpload.$refs.upload.clearFiles();
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.title = "修改刀具参数";
      this.reset();
      const cutterId = row.id || this.ids;
      this.optType = "edit";
      getCutterConfigMaster(cutterId).then((res) => {
        if (res.code == 0) {
          this.open = true;
          this.form = res.data;
          this.form.status = res.data.status + "";
          this.initialForm = Object.assign({}, res.data);
          this.$nextTick(() => {
            this.$refs.ConfigDetail.queryParams.masterId = res.data.id;
            this.$refs.ConfigDetail.getList();
          });
          // 钻带文件回显
          this.diaFileList = [
            {
              name: res.data.diaFileName,
              url: res.data.diaFilePath,
            },
          ];
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击查看
    handleSee(row) {
      this.title = "查看刀具参数";
      this.reset();
      const cutterId = row.id || this.ids;
      this.optType = "view";
      getCutterConfigMaster(cutterId).then((res) => {
        if (res.code == 0) {
          this.open = true;
          this.form = res.data;
          this.form.status = res.data.status + "";
          this.$nextTick(() => {
            this.$refs.ConfigDetail.queryParams.masterId = res.data.id;
            this.$refs.ConfigDetail.getList();
          });
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleDelete":
          this.handleDelete(row);
          break;
        case "handleSee":
          this.handleSee(row);
          break;
        default:
          break;
      }
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateCutterConfigMaster(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addCutterConfigMaster(this.form).then((res) => {
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
          delCutterConfigMaster,
          this.getList,
          "配置名称为" + row.configName
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 产品大类选择弹出框
    handleSelectProductCategory() {
      this.$refs.productCategorySelect.showFlag = true;
      this.$refs.productCategorySelect.selectedProductCategoryId = this.form
        .productCategoryId
        ? this.form.productCategoryId
        : undefined;
      this.$refs.productCategorySelect.getList();
    },
    // 产品大类选择框
    onProductCategorySelected(obj) {
      if (obj != null && obj != undefined) {
        this.$set(this.form, "productCategoryName", obj.name);
        this.$set(this.form, "productCategoryCode", obj.code);
        this.$set(this.form, "productCategoryId", obj.id);
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "刀具参数导入";
      this.$refs.upload.open = true;
    },
    // 刀具文件变化
    fileChangeDiaFilePath(file, fileList) {
      // 这是关键一句;
      if (fileList.length > 0) {
        this.diaFileList = [fileList[fileList.length - 1]];
      }
    },
    // 清空文件
    onRemoveDiaFileList() {
      this.diaFileList = [];
      this.form.diaFileName = "";
      this.form.diaFilePath = "";
    },
  },
};
</script>
