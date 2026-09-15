<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="物料分类编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入物料分类编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料分类名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入物料分类名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料Or产品" prop="itemOrProduct">
        <el-select
          @clear="clearQueryParams('itemOrProduct')" 
          v-model="queryParams.itemOrProduct"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option :label="'产品'" :value="2" />
          <el-option :label="'物料'" :value="1" />
        </el-select>
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
          :disabled="hasPermi(['masterData:itemType:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-upload2"
          @click="handleImport"
          :disabled="hasPermi(['masterData:itemType:import'])"
          >导入</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          type="info"
          plain
          icon="el-icon-sort"
          @click="toggleExpandAll"
          >展开/折叠</el-button
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
      v-if="refreshTable"
      v-loading="loading"
      :data="itemTypeList"
      row-key="id"
      :default-expand-all="isExpandAll"
      :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
    >
      <el-table-column
        key="code"
        prop="code"
        min-width="150"
        label="物料分类编码"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.parentId == 0">{{ scope.row.code }}</span>
          <span
            v-else
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['masterData:itemType:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        key="name"
        prop="name"
        label="物料分类名称"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[1].visible"
      ></el-table-column>
      <el-table-column
        label="物料/产品"
        align="center"
        key="itemOrProduct"
        prop="itemOrProduct"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.itemOrProduct == 1">物料</span>
          <span v-else-if="scope.row.itemOrProduct == 2">产品</span>
          <span v-else></span>
        </template>
      </el-table-column>
      <el-table-column
        key="status"
        prop="status"
        label="状态"
        align="center"
        v-if="columns[3].visible"
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
        min-width="180"
        v-if="columns[4].visible"
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
            icon="el-icon-plus"
            @click.native.stop="handleAdd(scope.row)"
            :disabled="hasPermi(['masterData:itemType:add'])"
            >新增</el-button
          >
          <el-dropdown
            trigger="click"
            v-if="scope.row.isSystem != 'Y'"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                command="handleUpdate"
                icon="el-icon-edit"
                :disabled="hasPermi(['masterData:itemType:edit'])"
                >修改</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                :disabled="hasPermi(['masterData:itemType:remove'])"
                icon="el-icon-delete"
                >删除</el-dropdown-item
              >
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>
    <!-- <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    /> -->
    <!-- 添加或修改物料分类对话框 -->
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
          <el-col :span="10">
            <el-form-item label="分类编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入物料分类编码"
                :disabled="autoGenFlag || optType != 'add'"
              /> </el-form-item
          ></el-col>
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
            <el-form-item label="分类名称" prop="name">
              <el-input
                v-model="form.name"
                placeholder="请输入物料分类名称"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="上级分类" prop="parentId">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                v-model="form.parentId"
                :options="itemTypeOptions"
                :normalizer="normalizer"
                :show-count="true"
                placeholder="请选择所属分类"
              />
            </el-form-item>
          </el-col>
          <el-col :span="7">
            <el-form-item label="物料/产品" prop="itemOrProduct">
              <el-radio-group v-removeAriaHidden v-model="form.itemOrProduct">
                <el-radio :label="1"> 物料 </el-radio>
                <el-radio :label="2"> 产品 </el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="9">
            <el-form-item
              label="状态"
              v-if="title == '修改物料分类'"
              prop="status"
            >
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio :label="1"> 正常 </el-radio>
                <el-radio :label="0"> 停用 </el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
    <!-- 物料产品分类导入 -->
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
  listItemType,
  getItemType,
  delItemType,
  addItemType,
  updateItemType,
  delList,
  treeSelect,
} from "@/api/masterData/itemType";

export default {
  name: "ItemType",
  dicts: ["sys_normal_disable"],
  data() {
    // 自定义校验规则
    const parentIdRule = (rule, value, callback) => {
      if (this.form.parentId == 1) {
        callback(new Error("根分类不可选，请重新选择"));
      } else if (!this.form.parentId) {
        callback(new Error("请选择所属分类"));
      } else {
        callback();
      }
    };
    return {
      page: "itemType",
      autoGenFlag: false,
      enCode: "",
      optType: "",
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 表格树数据
      itemTypeList: [],
      // 物料分类树选项
      itemTypeOptions: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      total: 0,
      // 是否展开，默认全部展开
      isExpandAll: true,
      // 重新渲染表格状态
      refreshTable: true,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10000,
        name: undefined,
        code: undefined,
        itemOrProduct: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        parentId: [
          { required: true, validator: parentIdRule, trigger: "change" },
        ],
        name: [
          { required: true, message: "物料分类名称不能为空", trigger: "blur" },
        ],
        code: [
          {
            required: true,
            message: "物料分类编码不能为空",
            trigger: "change",
          },
        ],
        itemOrProduct: [
          { required: true, message: "物料或产品不能为空", trigger: "blur" },
        ],
      },
      itemId: undefined,
      // 物料产品分类导入参数
      // 导入的url
      uploadUrl: "/v1/ItemType/UploadList",
      // 下载的url
      downloadUrl: "/v1/ItemType/DownLoad",
      // 导入的模板下载名
      fileName: "物料类型模板.xlsx", 
      columns: [
        { key: 0, label: "物料分类编码", visible: true },
        { key: 1, label: "物料分类名称", visible: true },
        { key: 2, label: "物料/产品", visible: true },
        { key: 3, label: "状态", visible: true },
        { key: 4, label: "创建时间", visible: true },
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
    /** 查询物料分类列表 */
    getList(isSearch) {
      this.loading = true;
      listItemType(this.queryParams).then((res) => {
        this.total = res.data.total;
        this.itemTypeList = this.handleTree(res.data.list, "id");
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    /** 查询下拉树结构 */
    getTreeselect() {
      treeSelect().then((res) => {
        this.itemTypeOptions = res.data;
      });
    },
    /** 转换类型数据结构 */
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.label,
        children: node.children,
      };
    },
    // 表单重置
    reset() {
      this.form = {
        name: "",
        code: "",
        parentId: 1,
        itemOrProduct: "",
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
    handleAdd(row) {
      this.reset();
      this.getTreeselect();
      if (row.id != undefined) {
        getItemType(row.id).then((res) => {
          if (res.code == 0) {
            this.form.parentId = res.data.id;
            this.initialForm = Object.assign({}, this.form);
            this.open = true;
            this.title = "添加物料分类";
            this.optType = "add";
            this.autoGenFlag = false;
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        this.form.parentId = 1;
        this.initialForm = Object.assign({}, this.form);
        this.open = true;
        this.title = "添加物料分类";
        this.optType = "add";
        this.autoGenFlag = false;
      }
    },
    /** 展开/折叠操作 */
    toggleExpandAll() {
      this.refreshTable = false;
      this.isExpandAll = !this.isExpandAll;
      this.$nextTick(() => {
        this.refreshTable = true;
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      this.getTreeselect();
      getItemType(row.id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改物料分类";
          this.optType = "edit";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleUpdate":
          this.handleUpdate(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      this.getTreeselect();
      getItemType(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看物料分类";
          this.optType = "view";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateItemType(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addItemType(this.form).then((res) => {
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
    //删除
    handleDelete(row) {
      if (row.children && row.children.length > 0)
        return this.$modal.notifyError("当前分类存在子分类,无法删除!");
      this.deleteItem(
        row.id,
        delItemType,
        this.getList,
        "物料分类编码为" + row.code
      );
    },
 
    //自动生成编码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "ITEMTYPE_CODE",
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
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "物料产品分类导入";
      this.$refs.upload.open = true;
    },
  },
};
</script>
