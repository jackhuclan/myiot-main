<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="角色名称" prop="roleName">
        <el-input
          v-trim
          v-model="queryParams.roleName"
          placeholder="请输入角色名称"
          clearable
          style="width: 240px"
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
      <el-form-item label="创建时间">
        <el-date-picker
          v-model="dateRange"
          style="width: 240px"
          value-format="yyyy-MM-dd"
          type="daterange"
          range-separator="-"
          start-placeholder="开始日期"
          end-placeholder="结束日期"
        ></el-date-picker>
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['system:role:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['system:role:remove'])"
          >批量删除</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
      ></right-toolbar>
    </el-row>

    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="roleList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="角色编码"
        key="id"
        prop="id"
        show-overflow-tooltip
        align="center"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['system:role:view']"
            >{{ scope.row.id }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="角色名称"
        key="roleName"
        prop="roleName"
        show-overflow-tooltip
        align="center"
      />
      <el-table-column label="显示顺序" key="sort" prop="sort" align="center" />
      <el-table-column label="状态" align="center" key="status" prop="status">
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
        width="180"
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
        <template slot-scope="scope" v-if="scope.row.id !== 1">
          <el-button
            type="text"
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="hasPermi(['system:role:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['system:role:remove'])"
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

    <!-- 添加或修改角色配置对话框 -->
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
        v-loading="loading"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item label="角色名称" prop="roleName">
              <el-input v-model="form.roleName" placeholder="请输入角色名称" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="角色顺序" prop="sort">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.sort"
                @changeNum="changeNum"
                :numName="'sort'"
                :min="1"
                :dis="optType == 'view'"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="应用系统" prop="appCode">
              <el-select
                v-model="form.appCode"
                placeholder="请选择系统"
                @change="getMenuTreeselect()"
              >
                <el-option
                  v-for="dict in appOptions"
                  :key="dict.appCode"
                  :label="dict.appName"
                  :value="dict.appCode"
                ></el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="状态">
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

        <el-form-item label="菜单权限">
          <el-checkbox
            v-model="form.menuExpand"
            @change="handleCheckedTreeExpand($event, 'menu')"
            >展开/折叠</el-checkbox
          >
          <el-checkbox
            v-model="form.menuNodeAll"
            @change="handleCheckedTreeNodeAll($event, 'menu')"
            >全选/全不选</el-checkbox
          >
          <el-checkbox
            v-model="form.menuCheckStrictly"
            @change="handleCheckedTreeConnect($event, 'menu')"
            >父子联动</el-checkbox
          >
          <el-tree
            @check-change="handleCheckChange"
            :disable="optType == 'view'"
            class="tree-border"
            :data="menuOptions"
            show-checkbox
            ref="menu"
            node-key="id"
            :check-strictly="!form.menuCheckStrictly"
            empty-text="加载中，请稍候"
            :props="defaultProps"
          ></el-tree>
        </el-form-item>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listRole,
  getRole,
  delRole,
  addRole,
  updateRole,
  delList,
} from "@/api/system/role";
import {
  treeselect as menuTreeselect,
  roleMenuTreeselect,
} from "@/api/system/menu";
import { listApp } from "@/api/system/appsecret";

export default {
  name: "Role",
  dicts: ["sys_normal_disable"],
  data() {
    return {
      page: "role",
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
      // 角色表格数据
      roleList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 日期范围
      dateRange: [],
      // 菜单列表
      menuOptions: [],
      // 部门列表
      deptOptions: [],
      //应用列表
      appOptions: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        roleName: undefined,
        status: undefined,
      },
      // 表单参数
      form: {
        appCode: "",
      },
      initialForm: {},
      defaultProps: {
        children: "children",
        label: "label",
      },
      // 表单校验
      rules: {
        roleName: [
          { required: true, message: "角色名称不能为空", trigger: "blur" },
        ],
        sort: [
          { required: true, message: "角色顺序不能为空", trigger: "change" },
        ],
      },
    };
  },
  activated() {
    this.getList();
    this.getAppList();
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询应用列表 */
    getAppList() {
      this.loading = true;
      listApp({ pageNum: 1, pageSize: 1000 }).then((res) => {
        this.appOptions = res.data.list;
        if (this.appOptions.length > 0) {
          this.form.appCode = this.appOptions[0].appCode;
          this.getMenuTreeselect();
        }
        this.loading = false;
      });
    },
    /** 查询角色列表 */
    getList(isSearch) {
      this.loading = true;
      listRole(this.addDateRange(this.queryParams, this.dateRange)).then(
        (res) => {
          this.roleList = res.data.list;
          this.total = res.data.total;
          this.loading = false; // 只有搜索状态下进行提示
          if (isSearch == "search") {
            this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
          }
        }
      );
    },
    /** 根据角色ID查询菜单树结构 */
    getRoleMenuTreeselect(roleId, appCode) {
      return roleMenuTreeselect(roleId, appCode).then((response) => {
        // this.initialForm.menuIds = response.data;
        // this.form.menuIds = response.data;
        return response;
      });
    },
    /**设置菜单选中 */
    setRoleMenuTreeselect(roleId, appCode) {
      this.getRoleMenuTreeselect(roleId, this.form.appCode).then((res) => {
        var roleMenu = res;
        getRole(roleId).then((response) => {
          this.form.roleName = response.data.roleName;
          this.form.sort = response.data.sort;
          this.form.status = response.data.status + "";
          this.$nextTick(() => {
            let checkedKeys = roleMenu.data;
            checkedKeys.forEach((v) => {
              this.$nextTick(() => {
                this.$refs.menu.setChecked(v, true, false);
              });
            });
          });
        });
      });
    },
    /** 查询菜单树结构 */
    getMenuTreeselect() {
      menuTreeselect(this.form.appCode).then((response) => {
        this.menuOptions = response.data;
        if (this.form.id != undefined) {
          var roleId = this.form.id;
          this.setRoleMenuTreeselect(roleId, this.form.appCode);
        }
      });
    },
    // 所有菜单节点数据
    getMenuAllCheckedKeys() {
      // 目前被选中的菜单节点
      let checkedKeys = this.$refs.menu.getCheckedKeys();
      // 半选中的菜单节点
      let halfCheckedKeys = this.$refs.menu.getHalfCheckedKeys();
      checkedKeys.unshift.apply(checkedKeys, halfCheckedKeys);
      return checkedKeys.sort((a, b) => a - b);
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
      if (this.$refs.menu != undefined) {
        this.$refs.menu.setCheckedKeys([]);
      }
      (this.form.menuExpand = false),
        (this.form.menuNodeAll = false),
        (this.form = {
          roleId: undefined,
          roleName: "",
          roleKey: undefined,
          appCode: this.appOptions[0].appCode,
          sort: 1,
          status: "1",
          menuIds: [],
          menuCheckStrictly: true,
          menuNodeAll: false, //全选/全不选
          menuExpand: false, //展开/折叠
          remark: "",
        });
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.dateRange = [];
      this.handleQuery();
    },
    // 树权限（展开/折叠）
    handleCheckedTreeExpand(value, type) {
      if (type == "menu") {
        let treeList = this.menuOptions;
        for (let i = 0; i < treeList.length; i++) {
          this.$refs.menu.store.nodesMap[treeList[i].id].expanded = value;
        }
      } else if (type == "dept") {
        let treeList = this.deptOptions;
        for (let i = 0; i < treeList.length; i++) {
          this.$refs.dept.store.nodesMap[treeList[i].id].expanded = value;
        }
      }
    },
    // 树权限（全选/全不选）
    handleCheckedTreeNodeAll(value, type) {
      if (type == "menu") {
        this.$refs.menu.setCheckedNodes(value ? this.menuOptions : []);
      } else if (type == "dept") {
        this.$refs.dept.setCheckedNodes(value ? this.deptOptions : []);
      }
    },
    // 树权限（父子联动）
    handleCheckedTreeConnect(value, type) {
      if (type == "menu") {
        this.form.menuCheckStrictly = value ? true : false;
      } else if (type == "dept") {
        this.form.deptCheckStrictly = value ? true : false;
      }
    },
    handleCheckChange() {
      this.form.menuIds = this.getMenuAllCheckedKeys();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.getAppList();
      this.getMenuTreeselect();
      this.optType = "add";
      this.open = true;
      this.title = "添加角色";
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const roleId = row.id || this.ids[0];
      if (roleId == 1) {
        return this.$modal.msgError("当前角色不能修改!");
      }
      getRole(roleId).then(async (res) => {
        if (res.code == 0) {
          let { data } = await this.getRoleMenuTreeselect(
            roleId,
            this.appOptions[0].appCode
          );
          this.optType = "edit";
          this.open = true;
          this.title = "修改角色";
          this.getAppList();
          this.form = {
            ...res.data,
            status: res.data.status + "",
            appCode: this.appOptions[0].appCode,
            menuCheckStrictly: true,
            menuNodeAll: false, //全选/全不选
            menuExpand: false, //展开/折叠
            remark: res.data.remark ? res.data.remark : "",
            menuIds: data.sort((a, b) => a - b),
          };

          this.initialForm = Object.assign(
            {},
            {
              ...res.data,
              status: res.data.status + "",
              appCode: this.appOptions[0].appCode,
              menuCheckStrictly: true,
              menuNodeAll: false, //全选/全不选
              menuExpand: false, //展开/折叠
              remark: res.data.remark ? res.data.remark : "",
              menuIds: data.sort((a, b) => a - b),
            }
          );
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 查看点击编码操作 */
    handleView(id) {
      this.reset();
      getRole(id).then((res) => {
        if (res.code == 0) {
          this.optType = "view";
          this.getAppList();
          this.form.appCode = this.appOptions[0].appCode;
          this.form.id = res.data.id;
          this.open = true;
          this.title = "查看角色";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 选择角色权限范围触发 */
    dataScopeSelectChange(value) {
      if (value !== "2") {
        this.$refs.dept.setCheckedKeys([]);
      }
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        // this.form.menuIds = this.getMenuAllCheckedKeys();
        this.loading = true;
        updateRole(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.loading = false;
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        // this.form.menuIds = this.getMenuAllCheckedKeys();
        this.loading = true;
        addRole(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.loading = false;
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
        this.deleteItem(row.id, delRole, this.getList, "角色编码为" + row.id);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },

    /** 导出按钮操作 */
    handleExport() {
      this.download(
        "system/role/export",
        {
          ...this.queryParams,
        },
        `role_${new Date().getTime()}.xlsx`
      );
    },
  },
};
</script>
