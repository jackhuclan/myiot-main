<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="参数名称" prop="drillFileName">
        <el-input
          v-trim
          v-model="queryParams.drillFileName"
          placeholder="请输入参数名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料名称" prop="itemName">
        <el-input
          v-trim
          v-model="queryParams.itemName"
          placeholder="请输入物料名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
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
          :disabled="hasPermi(['material:drillFile:add'])"
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
          :disabled="hasPermi(['material:drillFile:remove'])"
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
          :disabled="hasPermi(['material:drillFile:import'])"
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
      :data="itemDrillFileList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="参数名称"
        key="drillFileName"
        prop="drillFileName"
        min-width="160"
        show-overflow-tooltip
        v-if="columns[0].visible"
      />
      <el-table-column
        label="参数路径"
        min-width="160"
        key="drillFilePath"
        prop="drillFilePath"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="物料编码"
        min-width="160"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="物料名称"
        min-width="160"
        key="itemName"
        prop="itemName"
        show-overflow-tooltip
        v-if="columns[3].visible"
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
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['material:drillFile:edit'])"
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
                icon="el-icon-upload"
                command="handleParse"
                :disabled="hasPermi(['material:drillFile:parse'])"
                >解析</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['material:drillFile:remove'])"
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

    <!-- 添加或修改钻带参数对话框 -->
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
            <el-form-item label="物料编码" prop="itemCode">
              <el-input v-model="form.itemCode" placeholder="请输入物料编码">
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleSelectProduct"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <ItemSelect ref="ItemSelect" @onSelected="onItemSelected">
              </ItemSelect>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="物料名称" prop="itemName">
              <el-input
                v-model="form.itemName"
                readonly
                placeholder="请输入物料名称"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="钻带文件" prop="drillFilePath">
              <UpLoadFile
                ref="drillFileUpload"
                :onChange="fileChangeDrillFilePath"
                :fileList="drillFileList"
                :btnText="'选择钻带文件'"
                :onRemove="onRemoveDrillFileList"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <el-divider v-if="form.id != null && open" content-position="center"
        >钻带参数明细</el-divider
      >
      <DrillFileDetail
        v-if="form.id != null && open"
        ref="drillFileDetail"
        :itemDrillFileId="itemDrillFileId"
        :parentOptType="optType"
      ></DrillFileDetail>
    </edit-form-dialog>
    <!-- 钻带参数导入 -->
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
  listItemDrillFile,
  getItemDrillFile,
  delItemDrillFile,
  addItemDrillFile,
  updateItemDrillFile,
  upLoad,
  delList,
} from "@/api/material/drillFile";
// 物料选择
import ItemSelect from "@/components/itemSelect";
// 查看弹框
import DrillFileDetail from "./drillFileDetail.vue";
export default {
  name: "DrillFile", 
  components: { ItemSelect, DrillFileDetail },
  data() {
    // 自定义校验规则
    const drillFileRole = (rule, value, callback) => {
      if (this.drillFileList.length <= 0) {
        callback(new Error("未选择文件"));
      } else {
        callback();
      }
    };
    return {
      page: "drillFile",
      // 遮罩层
      loading: true,
      optType: "",
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // ATP表格数据
      itemDrillFileList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        itemId: undefined,
        itemCode: undefined,
        itemName: undefined,
        itemTypeId: undefined,
        drillFileName: undefined,
        drillFilePath: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        itemName: [
          { required: true, message: "物料名称不能为空", trigger: "change" },
        ],
        itemCode: [
          { required: true, message: "物料编码不能为空", trigger: "change" },
        ],
        drillFilePath: [
          { required: true, validator: drillFileRole, trigger: "change" },
        ],
        drillFileName: [
          { required: true, message: "钻带文件名称不能为空", trigger: "blur" },
        ],
      },
      // 用户导入参数
      // 导入的url
      uploadUrl: "/v1/UnitMeasure/UploadUnitList",
      // 导入的模板下载名
      fileName: "计量单位模板.xlsx",
      // 下载模板的url
      downloadUrl: "/v1/UnitMeasure/DownLoad",
      itemDrillFileId: undefined,
      // 钻带文件
      drillFileList: [],
      // 列信息
      columns: [
        { key: 0, label: "参数名称", visible: true },
        { key: 1, label: "参数路径", visible: true },
        { key: 2, label: "物料编码", visible: true },
        { key: 3, label: "物料名称", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    drillFileList: {
      handler(val) {
        if (val.length > 0) {
          if (val[0].raw) {
            this.upLoadFile(val[0].raw, upLoad).then((res) => {
              this.form.drillFilePath = res;
              this.form.drillFileName = val[0].name;
            });
          }
        } else {
          this.form.drillFilePath = undefined;
          this.form.drillFileName = undefined;
        }
      },
      deep: true,
    },
    "form.itemCode": {
      handler(val) {
        if (val == "") {
          this.form.itemId = undefined;
          this.form.itemName = undefined;
          this.form.itemCode = undefined;
          this.form.itemTypeId = undefined;
        }
      },
      deep: true,
    },
    open(val) {
      if (!val) {
        this.onRemoveDrillFileList();
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
    /** 查询ATP列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listItemDrillFile(this.queryParams);
      this.itemDrillFileList = res.data.list;
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
        itemId: undefined,
        itemCode: undefined,
        itemName: undefined,
        drillFilePath: undefined,
        drillFileName: undefined,
        itemTypeId: undefined,
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
      this.open = true;
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加钻带参数";
      this.optType = "add";
      // 清空文件列表
      this.$nextTick(() => {
        this.$refs.drillFileUpload.$refs.upload.clearFiles();
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const ItemDrillFileId = row.id || this.ids;
      getItemDrillFile(ItemDrillFileId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.itemDrillFileId = row.id;
          this.optType = "edit";
          this.open = true;
          this.title = "修改钻带参数";
          // 钻带文件回显
          this.drillFileList = [
            {
              name: res.data.drillFileName,
              url: res.data.drillFilePath,
            },
          ];
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击解析
    handleParse(row) {
      const parseLoading = this.$loading({
        lock: true,
        text: "正在解析文件，请稍后",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.7)",
      });
      new Promise((resolve, reject) => {
        setTimeout(() => {
          parseLoading.close();
          resolve("解析成功");
        }, 1000);
      }).then((res) => {
        this.$modal.msgSuccess(res);
      });
    },
    // 查看编码
    handleSee(row) {
      this.reset();
      const ItemDrillFileId = row.id || this.ids;
      getItemDrillFile(ItemDrillFileId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.itemDrillFileId = row.id;
          this.optType = "view";
          this.open = true;
          this.title = "修改钻带参数";
          // 钻带文件回显
          this.drillFileList = [
            {
              name: res.data.drillFileName,
              url: res.data.drillFilePath,
            },
          ];
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
        case "handleParse":
          this.handleParse(row);
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
        updateItemDrillFile(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addItemDrillFile(this.form).then((res) => {
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
          delItemDrillFile,
          this.getList,
          "参数名称为" + row.drillFileName
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    //物料选择弹出框
    handleSelectProduct() {
      this.$refs.ItemSelect.showFlag = true;
      this.$refs.ItemSelect.selectedItemCode = this.form.itemCode
        ? this.form.itemCode
        : undefined;
      this.$refs.ItemSelect.title = "物料选择";
      this.$refs.ItemSelect.getList();
    },
    onItemSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "itemCode", obj.code);
        this.$set(this.form, "itemId", obj.id);
        this.$set(this.form, "itemName", obj.name);
        this.$set(this.form, "itemTypeId", obj.itemTypeId);
      }
    },
    // 上传钻带文件
    fileChangeDrillFilePath(file, fileList) {
      // 这是关键一句
      if (fileList.length > 0) {
        this.drillFileList = [fileList[fileList.length - 1]];
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "钻带参数导入";
      this.$refs.upload.open = true;
    },
    // 清空文件
    onRemoveDrillFileList() {
      this.drillFileList = [];
      this.form.drillFilePath = "";
      this.form.drillFileName = "";
    },
  },
};
</script>
