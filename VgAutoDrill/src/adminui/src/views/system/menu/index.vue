<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="应用系统" prop="appCode">
        <el-select
          v-model="queryParams.appCode"
          placeholder="请选择系统"
          @change="getList()"
        >
          <el-option
            v-for="dict in appOptions"
            :key="dict.appCode"
            :label="dict.appName"
            :value="dict.appCode"
          ></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="菜单名称" prop="menuName">
        <el-input
          v-trim
          v-model="queryParams.menuName"
          placeholder="请输入菜单名称"
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
          :disabled="hasPermi(['system:menu:add'])"
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
      :data="menuList"
      row-key="id"
      :default-expand-all="isExpandAll"
      :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
    >
      <el-table-column
        fixed="left"
        key="menuName"
        prop="menuName"
        label="菜单名称"
        show-overflow-tooltip
        min-width="150"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['system:menu:view']"
            >{{ scope.row.menuName }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        key="menuIcon"
        prop="menuIcon"
        label="图标"
        align="center"
        v-if="columns[1].visible"
      >
        <template slot-scope="scope">
          <svg-icon :icon-class="scope.row.menuIcon" />
        </template>
      </el-table-column>

      <el-table-column
        key="menuType"
        prop="menuType"
        label="菜单类型"
        align="center"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.menuType == 2">菜单</el-tag>
          <el-tag v-else-if="scope.row.menuType == 1" type="success"
            >目录</el-tag
          >
          <el-tag v-else type="warning">按钮</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        key="sort"
        prop="sort"
        label="排序"
        align="center"
        v-if="columns[3].visible"
      />
      <el-table-column
        key="authorize"
        prop="authorize"
        label="权限标识"
        show-overflow-tooltip
        v-if="columns[4].visible"
        min-width="150"
      />
      <el-table-column
        key="menuUrl"
        prop="menuUrl"
        label="组件路径"
        align="center"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        key="status"
        prop="status"
        label="状态"
        width="80"
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
        min-width="180"
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
            v-if="scope.row.menuType != 3"
            type="text"
            icon="el-icon-plus"
            @click.native.stop="handleAdd(scope.row)"
            :disabled="hasPermi(['system:menu:add'])"
            >新增</el-button
          >
          <el-button
            v-if="scope.row.menuType == 3"
            type="text"
            icon="el-icon-edit"
            @click.native.stop="handleUpdate(scope.row)"
            :disabled="hasPermi(['system:menu:edit'])"
            >修改</el-button
          >

          <el-button
            v-if="scope.row.menuType == 3"
            type="text"
            icon="el-icon-delete"
            @click.native.stop="handleDelete(scope.row)"
            :disabled="hasPermi(['system:menu:remove'])"
            >删除</el-button
          >
          <el-dropdown
            v-if="scope.row.menuType != 3"
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
                :disabled="hasPermi(['system:menu:edit'])"
                >修改</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['system:menu:remove'])"
                >删除</el-dropdown-item
              >
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>

    <!-- 添加或修改菜单对话框 -->
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
        label-width="100px"
        :rules="rules"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item label="上级菜单">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                v-model="form.parentId"
                :options="menuOptions"
                :normalizer="normalizer"
                :show-count="true"
                placeholder="选择上级菜单"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="显示排序" prop="sort">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :dis="optType == 'view'"
                :myNum="form.sort"
                @changeNum="changeNum"
                :numName="'sort'"
                :min="1"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24" v-if="form.menuType != '3'">
            <el-form-item label="菜单图标" prop="icon">
              <el-input
                placeholder="请点击右侧按钮选择"
                v-model="form.menuIcon"
              >
                <svg-icon
                  v-if="form.menuIcon"
                  slot="prefix"
                  :icon-class="form.menuIcon"
                  class="el-input__icon"
                  style="height: 32px; width: 16px"
                />
                <!-- <i v-else slot="prefix" class="el-icon-search el-input__icon" /> -->
                <el-button
                  slot="append"
                  icon="el-icon-search"
                  @click="showFlag = true"
                ></el-button>
              </el-input>
              <IconSelect
                ref="iconSelect"
                v-if="showFlag"
                @selected="selected"
              />
              <!-- <el-popover
                :disabled="optType == 'view'"
                placement="bottom-start"
                width="460"
                trigger="click"
                @show="$refs['iconSelect'].reset()"
              >
                <IconSelect ref="iconSelect" @selected="selected" />
                <el-input
                  slot="reference"
                  v-model="form.menuIcon"
                  placeholder="点击选择图标"
                  readonly
                >
                  <svg-icon
                    v-if="form.menuIcon"
                    slot="prefix"
                    :icon-class="form.menuIcon"
                    class="el-input__icon"
                    style="height: 32px; width: 16px"
                  />
                  <i
                    v-else
                    slot="prefix"
                    class="el-icon-search el-input__icon"
                  />
                </el-input>
              </el-popover> -->
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="14">
            <el-form-item label="菜单类型" prop="menuType">
              <el-radio-group v-removeAriaHidden v-model="form.menuType">
                <el-radio label="2">菜单</el-radio>
                <el-radio label="1">目录</el-radio>
                <el-radio label="3">按钮</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item v-if="form.menuType != '3'">
              <span slot="label">
                <el-tooltip
                  content="选择停用则路由将不会出现在侧边栏，也不能被访问"
                  placement="top"
                >
                  <i class="el-icon-question"></i>
                </el-tooltip>
                菜单状态
              </span>
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
          <el-col :span="12">
            <el-form-item
              :label="form.menuType == 3 ? '按钮名称' : '菜单名称'"
              prop="menuName"
            >
              <el-input
                v-model="form.menuName"
                :placeholder="
                  form.menuType == 3 ? '请输入按钮名称' : '请输入菜单名称'
                "
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item v-if="form.menuType != 1">
              <el-input
                v-model="form.authorize"
                placeholder="请输入权限标识"
                maxlength="100"
              />
              <span slot="label">
                <el-tooltip
                  content="控制器中定义的权限字符，如：@PreAuthorize(`@ss.hasPermi('system:user:list')`)"
                  placement="top"
                >
                  <i class="el-icon-question"></i>
                </el-tooltip>
                权限字符
              </span>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12" v-if="form.menuType == '2'">
            <el-form-item prop="path">
              <span slot="label">
                <el-tooltip
                  content="访问的路由地址，如：`user`，如外网地址需内链访问则以`http(s)://`开头"
                  placement="top"
                >
                  <i class="el-icon-question"></i>
                </el-tooltip>
                路由地址
              </span>
              <el-input v-model="form.path" placeholder="请输入路由地址" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item prop="menuUrl" v-if="form.menuType == 2">
              <span slot="label">
                <el-tooltip
                  content="访问的组件路径，如：`system/user/index`，默认在`views`目录下"
                  placement="top"
                >
                  <i class="el-icon-question"></i>
                </el-tooltip>
                组件路径
              </span>
              <el-input v-model="form.menuUrl" placeholder="请输入组件路径" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listMenu,
  getMenu,
  delMenu,
  addMenu,
  updateMenu,
} from "@/api/system/menu";
import IconSelect from "@/components/IconSelect";
import { listApp } from "@/api/system/appsecret";

export default {
  name: "Menu",
  dicts: ["sys_normal_disable"],
  components: { IconSelect },
  data() {
    return {
      page: "menu",
      optType: "",
      // 遮罩层
      showFlag: false,
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 菜单表格树数据
      menuList: [],
      // 菜单树选项
      menuOptions: [],
      //应用列表
      appOptions: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 是否展开，默认全部折叠
      isExpandAll: false,
      // 重新渲染表格状态
      refreshTable: true,
      // 查询参数
      queryParams: {
        menuName: undefined,
        appCode: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        menuName: [
          { required: true, message: "菜单名称不能为空", trigger: "blur" },
        ],
        sort: [
          { required: true, message: "菜单顺序不能为空", trigger: "change" },
        ],
        menuUrl: [
          { required: true, message: "组件路径不能为空", trigger: "blur" },
        ],
        path: [
          { required: true, message: "路由地址不能为空", trigger: "blur" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: `菜单名称`, visible: true },
        { key: 1, label: `图标`, visible: true },
        { key: 2, label: `菜单类型`, visible: true },
        { key: 3, label: `排序`, visible: true },
        { key: 4, label: `权限标识`, visible: true },
        { key: 5, label: `组件路径`, visible: true },
        { key: 6, label: `状态`, visible: true },
        { key: 7, label: `创建时间`, visible: true },
      ],
    };
  },
  created() {
    this.getAppList();
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
    // 选择图标
    selected(name) {
      this.form.menuIcon = name;
      this.showFlag = false;
    },
    /** 查询应用列表 */
    getAppList() {
      this.loading = true;
      listApp({ pageNum: 1, pageSize: 1000 }).then((res) => {
        this.appOptions = res.data.list;
        this.queryParams.appCode = this.appOptions[0].appCode;
        this.loading = false;
        this.getList();
      });
    },
    /** 查询菜单列表 */
    getList(isSearch) {
      this.loading = true;
      listMenu(this.queryParams).then((res) => {
        this.menuList = this.handleTree(res.data, "id");
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch(
            "搜索成功，共" + this.menuList.length + "条数据！"
          );
        }
      });
    },
    /** 转换菜单数据结构 */
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.menuName,
        children: node.children,
      };
    },
    /** 查询菜单下拉树结构 */
    getTreeselect() {
      this.form.appCode = this.queryParams.appCode;
      listMenu(this.queryParams).then((res) => {
        this.menuOptions = [];
        const menu = { id: 0, menuName: "主类目", children: [] };
        menu.children = this.handleTree(res.data, "id");
        this.menuOptions.push(menu);
      });
    },

    // 表单重置
    reset() {
      this.form = {
        menuName: "",
        parentId: 0,
        menuIcon: "",
        menuUrl: "",
        path: "",
        menuType: "1",
        authorize: "",
        sort: 1,
        status: "1",
      };
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.appCode = this.appOptions[0].appCode;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd(row) {
      this.showFlag = false;
      this.reset();
      this.getTreeselect();
      if (row.id && row != null) {
        if (row.menuType == 1) {
          this.form.menuType = "2";
        } else if (row.menuType == 2) {
          this.form.menuType = "3";
        }
        this.form.parentId = row.id;
        getMenu(row.id).then((res) => {
          if (res.code == 0) {
            this.optType = "add";
            this.open = true;
            this.title = "添加菜单";
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        this.form.parentId = 0;
        this.optType = "add";
        this.open = true;
        this.title = "添加菜单";
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
      this.showFlag = false;
      this.reset();
      this.getTreeselect();
      getMenu(row.id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.optType = "edit";
          this.open = true;
          this.initialForm = Object.assign({}, res.data);
          this.title = "修改菜单";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击查看
    handleView(id) {
      this.reset();
      this.getTreeselect();
      getMenu(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.optType = "view";
          this.open = true;
          this.title = "查看菜单";
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
      if (this.form.menuType != "2") {
        this.form.menuUrl = "#";
      }
      if (this.form.id != undefined) {
        updateMenu(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addMenu(this.form).then((res) => {
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
    //删除单个
    handleDelete(row) {
      let label = null;
      switch (row.menuType) {
        case "1":
          label = "目录名称为";
          break;
        case "2":
          label = "菜单名称为";
          break;
        case "3":
          label = "按钮名称为";
          break;
      }
      this.deleteItem(row.id, delMenu, this.getList, label + row.menuName);
    },
  },
};
</script>
