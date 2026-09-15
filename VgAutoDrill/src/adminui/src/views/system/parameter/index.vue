<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="配置项编码" prop="configCode">
        <el-input
          v-trim
          v-model="queryParams.configCode"
          placeholder="请输入配置项编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="配置项描述" prop="configDescript">
        <el-input
          v-trim
          v-model="queryParams.configDescript"
          placeholder="请输入配置项描述"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="快速配置" prop="isQuickConfig">
        <el-select
          @clear="clearQueryParams('isQuickConfig')"
          v-model="queryParams.isQuickConfig"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option :label="'是'" :value="true" />
          <el-option :label="'否'" :value="false" /></el-select
      ></el-form-item>
      <el-form-item label="分类" prop="category">
        <!-- <el-select
          @clear="clearQueryParams('category')"
          v-model="queryParams.category"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option
            :key="v.id"
            :label="v.label"
            :value="v.id"
            v-for="v in sysConfigTypeOptions"
          />
        </el-select> -->
        <treeselect
          style="width: 200px"
          :class="$store.getters.size + '-treeselect'"
          noOptionsText="暂无数据"
          noChildrenText="暂无数据"
          noResultsText="暂无数据"
          v-model="queryParams.category"
          :options="sysConfigTypeOptions"
          :normalizer="normalizer"
          :show-count="true"
          placeholder="请选择所属分类"
        />
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['system:parameter:add'])"
          >新增</el-button
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
      ></right-toolbar>
    </el-row>
    <el-table
      border
      :ref="page"
      v-if="refreshTable"
      v-loading="loading"
      :data="sysConfigList"
      row-key="id"
      :default-expand-all="isExpandAll"
      :tree-props="{ children: 'children' }"
    >
      <el-table-column
        label="分类"
        key="category"
        prop="category"
        show-overflow-tooltip
        min-width="180px"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span v-if="!scope.row.children && scope.row.id < 20000">---</span>
          <span v-else>{{ scope.row.configDescript }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="配置项编码"
        key="configCode"
        prop="configCode"
        min-width="150px"
        show-overflow-tooltip
        v-if="columns[1].visible"
      >
        <template slot-scope="scope">{{
          scope.row.configCode ? scope.row.configCode : "---"
        }}</template></el-table-column
      >

      <el-table-column
        min-width="100px"
        label="配置项类型"
        align="center"
        key="configType"
        prop="configType"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">{{
          scope.row.configType ? scope.row.configType : "---"
        }}</template>
      </el-table-column>
      <el-table-column
        label="配置项值"
        min-width="150px"
        show-overflow-tooltip
        key="configValue"
        prop="configValue"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">{{
          scope.row.configValue ? scope.row.configValue : "---"
        }}</template></el-table-column
      >
      <el-table-column
        label="快速配置"
        key="isQuickConfig"
        prop="isQuickConfig"
        align="center"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <el-tag type="danger" v-if="scope.row.isQuickConfig == true"
            >是</el-tag
          >
          <el-tag v-else-if="scope.row.isQuickConfig == false">否</el-tag>
          <span v-else>---</span>
        </template>
      </el-table-column>
      <el-table-column
        label="配置项描述"
        key="configDescript"
        prop="configDescript"
        min-width="150px"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.configDescript" />
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        key="remark"
        prop="remark"
        min-width="150px"
        v-if="columns[6].visible"
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
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.configCode == '000'">---</span>
          <el-button
            v-else-if="scope.row.id < 5000"
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['system:parameter:edit'])"
            >修改</el-button
          >
          <el-button
            v-else
            type="text"
            icon="el-icon-plus"
            @click="handleAdd(scope.row)"
            :disabled="hasPermi(['system:parameter:add'])"
            >新增</el-button
          >
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

    <!-- 添加或修改配置项对话框 -->
    <edit-form-dialog
      v-model="open"
      :optType="optType"
      :title="title"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="100px">
        <el-row>
          <el-col :span="10">
            <el-form-item label="上级分类" prop="category">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                v-model="form.category"
                :options="sysConfigTypeOptions"
                :normalizer="normalizer"
                :show-count="true"
                placeholder="请选择所属分类"
              />
            </el-form-item>
          </el-col>
          <el-col :span="14">
            <el-form-item label="配置项编码" prop="configCode">
              <el-input
                :disabled="title == '修改配置项'"
                v-model="form.configCode"
                placeholder="请输入配置项编码"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row
          ><el-col :span="8">
            <el-form-item label="快速配置" prop="isQuickConfig">
              <el-radio-group v-removeAriaHidden v-model="form.isQuickConfig">
                <el-radio :label="true"> 是 </el-radio>
                <el-radio :label="false">否 </el-radio>
              </el-radio-group>
            </el-form-item></el-col
          >
          <el-col :span="8">
            <el-form-item label="配置项类型" prop="configType">
              <el-select
                :disabled="title == '修改配置项'"
                v-model="form.configType"
                placeholder="请选择"
                @change="handleSelectConfigType"
              >
                <el-option
                  v-for="item in options"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col
            :span="8"
            v-if="form.configType == 'enum' && title == '修改配置项'"
          >
            <el-form-item label="配置项值" prop="configValue">
              <el-select
                v-model="form.configValue"
                @change="handleSelectConfigValue"
                placeholder="请选择"
              >
                <el-option
                  v-for="item in sysConfigEnumList"
                  :key="item.configEnumValue"
                  :label="
                    item.configEnumDescript + '(' + item.configEnumValue + ')'
                  "
                  :value="item.configEnumValue"
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8" v-else-if="form.configType == 'int'">
            <el-form-item label="配置项值" prop="configValue">
              <input-number
                :myNum="form.configValue ? form.configValue * 1 : 0"
                @changeNum="changeNum"
                :numName="'configValue'"
                :min="1"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8" v-else-if="form.configType == 'bool'">
            <el-form-item label="配置项值" prop="configValue">
              <el-radio-group v-removeAriaHidden v-model="form.configValue">
                <el-radio label="true"> true </el-radio>
                <el-radio label="false">false </el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="24" v-else>
            <el-form-item label="配置项值" prop="configValue">
              <el-input
                v-model="form.configValue"
                placeholder="请输入配置项值"
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
          <el-col
            :span="24"
            v-if="form.configType == 'enum' && title == '添加配置项'"
          >
            <el-form-item label="Enum值" prop="configEnumValue">
              <el-input
                v-model="form.configEnumValue"
                placeholder="请输入Enum值"
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="备注" prop="remark">
          <el-input
            v-model="form.remark"
            type="textarea"
            placeholder="请输入内容"
            :autosize="{ minRows: 2, maxRows: 10 }"
            :resize="optType == 'view' ? 'none' : ''"
          />
        </el-form-item>
        <el-form-item label="配置项描述" prop="configDescript">
          <el-input
            v-model="form.configDescript"
            type="textarea"
            placeholder="请输入内容"
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
  addSysConfig,
  getSysConfig,
  updateSysConfig,
  getSysConfigEnumList,
  getTreeList,
  treeselect,
} from "@/api/system/parameter";
export default {
  name: "SysConfig",
  data() {
    // 自定义校验规则
    const categoryRule = (rule, value, callback) => {
      if (!this.form.category && this.form.category != 0) {
        callback(new Error("请选择所属分类"));
      } else if (this.form.category == 0) {
        callback(new Error("全部项不可选"));
      } else {
        callback();
      }
    };
    return {
      page: "sysConfig",
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
      // 配置项表格数据
      sysConfigList: [],
      // 枚举列表
      sysConfigEnumList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 是否展开，默认全部展开
      isExpandAll: true,
      // 重新渲染表格状态
      refreshTable: true,
      // 查询配置项
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        configCode: undefined,
        isQuickConfig: undefined,
        category: undefined,
        configDescript: undefined,
      },
      // 表单配置项
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        configCode: [
          { required: true, message: "配置项编码不能为空", trigger: "blur" },
        ],
        configType: [
          { required: true, message: "配置项类型不能为空", trigger: "change" },
        ],
        configValue: [
          { required: true, message: "配置项值不能为空", trigger: "change" },
        ],
        remark: [{ required: true, message: "备注不能为空", trigger: "blur" }],
        category: [
          {
            required: true,
            validator: categoryRule,
            trigger: "change",
          },
        ],
      },
      options: [
        {
          value: "int",
          label: "int",
        },
        {
          value: "string",
          label: "string",
        },
        {
          value: "enum",
          label: "enum",
        },
        {
          value: "bool",
          label: "bool",
        },
      ],
      sysConfigTypeOptions: [],
      // 列信息
      columns: [
        { key: 0, label: `分类`, visible: true },
        { key: 1, label: `配置项编码`, visible: true },
        { key: 2, label: `配置项类型`, visible: true },
        { key: 3, label: `配置项值`, visible: true },
        { key: 4, label: `快速配置`, visible: true },

        { key: 5, label: `配置项描述`, visible: true },
        { key: 6, label: `备注`, visible: true },

        { key: 7, label: `创建时间`, visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    this.getTypeList();
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
    /** 查询配置项列表 */
    getList(isSearch) {
      this.loading = true;
      getTreeList(this.queryParams).then((res) => {
        this.sysConfigList = res.data;
        this.loading = false;
      });
    },
    getTypeList() {
      treeselect().then((res) => {
        this.sysConfigTypeOptions = res.data;
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
    /** 展开/折叠操作 */
    toggleExpandAll() {
      this.refreshTable = false;
      this.isExpandAll = !this.isExpandAll;
      this.$nextTick(() => {
        this.refreshTable = true;
      });
    },
    /** 转换类型数据结构 */
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.label == "None" ? "全部" : node.label,
        children: node.children,
      };
    },
    // 表单重置
    reset() {
      this.form = {
        configCode: "",
        configValue: "",
        configType: "",
        configDescript: "",
        category: undefined,
        remark: "",
        configEnumValue: "",
        isQuickConfig: false,
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
    // 监听类型选择
    handleSelectConfigType(val) {
      this.form.configValue = undefined;
    },
    // 监听值选择
    handleSelectConfigValue(val) {
      this.form.configDescript = this.sysConfigEnumList.find(
        (v) => v.configEnumValue == val
      ).configEnumDescript;
    },
    /** 新增按钮操作 */
    handleAdd(row) {
      this.reset();
      if (row.id && row != null) {
        this.form.category = row.category;
        this.optType = "add";
        this.open = true;
        this.title = "添加配置项";
      } else {
        this.open = true;
        this.optType = "add";
        this.title = "添加配置项";
      }

      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const postId = row.id || this.ids;
      getSysConfig(postId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.configValue =
            res.data.configType == "int"
              ? res.data.configValue * 1
              : res.data.configValue;
          this.open = true;
          this.optType = "edit";
          this.title = "修改配置项";
          getSysConfigEnumList({ configCode: res.data.configCode }).then(
            (res1) => {
              this.sysConfigEnumList = res1.data?.sysConfigEnums;
            }
          );
          this.initialForm = Object.assign(
            {},
            {
              ...res.data,
              configValue:
                res.data.configType == "int"
                  ? res.data.configValue * 1
                  : res.data.configValue,
            }
          );
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      this.form.configValue = this.form.configValue + "";
      if (this.form.id != undefined) {
        updateSysConfig(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addSysConfig(this.form).then((res) => {
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
  },
};
</script>
