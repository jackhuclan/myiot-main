<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="部门名称" prop="departmentName">
        <el-input
          v-trim
          v-model="queryParams.departmentName"
          placeholder="请输入部门名称"
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
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['system:dept:add'])"
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
        :page="page"
      ></right-toolbar>
    </el-row>

    <el-table
      border
      :ref="page"
      v-if="refreshTable"
      v-loading="loading"
      :data="deptList"
      row-key="id"
      :default-expand-all="isExpandAll"
      :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
    >
      <el-table-column
        key="departmentName"
        prop="departmentName"
        label="部门名称"
        min-width="150px"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['system:dept:view']"
            >{{ scope.row.departmentName }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        key="leader"
        prop="leader"
        label="部门负责人"
        v-if="columns[1].visible"
        align="center"
        min-width="150px"
        show-overflow-tooltip
      ></el-table-column>
      <el-table-column
        key="telephone"
        prop="telephone"
        label="负责人电话"
        v-if="columns[2].visible"
        align="center"
        min-width="150px"
        show-overflow-tooltip
      ></el-table-column>
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
        min-width="180px"
        key="createTime"
        prop="createTime"
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
        <template slot-scope="scope"
          ><el-button
            type="text"
            icon="el-icon-plus"
            @click.native.stop="handleAdd(scope.row)"
            :disabled="hasPermi(['system:dept:add'])"
            >新增</el-button
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
                command="handleUpdate"
                icon="el-icon-edit"
                :disabled="hasPermi(['system:dept:edit'])"
                >修改</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['system:dept:remove'])"
                >删除</el-dropdown-item
              >
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>
    <!-- 添加或修改部门对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      width="650px"
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
            <el-form-item label="上级部门">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                v-model="form.parentId"
                :options="deptOptions"
                :normalizer="normalizer"
                :show-count="true"
                placeholder="选择上级部门"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="部门名称" prop="departmentName">
              <el-input
                v-model="form.departmentName"
                placeholder="请输入部门名称"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="负责人" prop="leader">
              <el-input
                v-model="form.leader"
                placeholder="请输入负责人"
                maxlength="20"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="联系电话" prop="telephone">
              <el-input
                v-model="form.telephone"
                placeholder="请输入联系电话"
                maxlength="11"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="邮箱" prop="email">
              <el-input
                v-model="form.email"
                placeholder="请输入邮箱"
                maxlength="50"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="部门状态">
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
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listDept,
  getDept,
  delDept,
  addDept,
  updateDept,
  treeselect,
} from "@/api/system/dept";
export default {
  name: "Dept",
  dicts: ["sys_normal_disable"],
  data() {
    return {
      page: "dept",
      optType: "",
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 表格树数据
      deptList: [],
      // 部门树选项
      deptOptions: [],
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
        departmentName: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        parentId: [
          { required: true, message: "上级部门不能为空", trigger: "blur" },
        ],
        departmentName: [
          { required: true, message: "部门名称不能为空", trigger: "blur" },
        ],
        email: [
          {
            type: "email",
            message: "请输入正确的邮箱地址",
            trigger: ["blur", "change"],
          },
        ],
        telephone: [
          {
            pattern: /^1[3|4|5|6|7|8|9][0-9]\d{8}$/,
            message: "请输入正确的手机号码",
            trigger: "blur",
          },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: `部门名称`, visible: true },
        { key: 1, label: `部门负责人`, visible: true },
        { key: 2, label: `负责人电话`, visible: true },
        { key: 3, label: `状态`, visible: true },
        { key: 4, label: `创建时间`, visible: true },
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
    /** 查询部门列表 */
    getList(isSearch) {
      this.loading = true;
      listDept(this.queryParams).then((res) => {
        this.deptList = this.handleTree(res.data.list, "id");
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    getTreeselect() {
      treeselect().then((res) => {
        this.deptOptions = res.data;
      });
    },
    /** 转换菜单数据结构 */
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
        parentId: undefined,
        departmentName: "",
        telephone: "",
        email: "",
        status: "1",
        leader: "",
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
      if (row != null && row.id) {
        this.form.parentId = row.id;
        getDept(row.id).then((res) => {
          if (res.code == 0) {
            this.optType = "add";
            this.open = true;
            this.title = "添加部门";
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        this.form.parentId = undefined;
        this.optType = "add";
        this.open = true;
        this.title = "添加部门";
      }
      this.initialForm = Object.assign({}, this.form);
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
      getDept(row.id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          if (res.data.parentId == 0) {
            this.form.parentId = null;
          }
          this.open = true;
          this.optType = "edit";
          this.title = "修改部门";
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 点击编码查看操作 */
    handleView(id) {
      this.reset();
      this.getTreeselect();
      getDept(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          if (res.data.parentId == 0) {
            this.form.parentId = null;
          }
          this.optType = "view";
          this.open = true;
          this.title = "查看部门";
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
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateDept(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
            this.getTreeselect();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addDept(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.open = false;
            this.getList();
            this.getTreeselect();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    //删除单个
    handleDelete(row) {
      if (row.children && row.children.length > 0)
        return this.$modal.notifyError("当前部门存在子部门,无法删除!");
      this.deleteItem(
        row.id,
        delDept,
        this.getList,
        "部门名称为" + row.departmentName
      );
    },
  },
};
</script>
