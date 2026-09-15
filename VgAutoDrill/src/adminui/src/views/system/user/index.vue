<template>
  <div class="app-container">
    <div class="index">
      <div class="left" :class="{ fold: openType }">
        <!--部门数据-->
        <leftTreeSelect
          v-model="departmentName"
          :placeholder="'请输入部门名称'"
          :filterNode="filterNode"
          :options="leftDeptOptions"
          :defaultProps="defaultProps"
          :handleNodeClick="handleNodeClick"
          :highlightCurrent="highlightCurrent"
        ></leftTreeSelect>
      </div>
      <div class="right" :class="{ fold: openType }">
        <search-form
          :openType="openType"
          v-show="showSearch"
          :form="queryParams"
          @search="handleQuery"
          @reset="resetQuery"
        >
          <el-form-item label="用户账号" prop="userName">
            <el-input
              v-trim
              v-model="queryParams.userName"
              placeholder="请输入用户账号"
              clearable
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item label="手机号码" prop="mobile">
            <el-input
              v-trim
              v-model="queryParams.mobile"
              placeholder="请输入手机号码"
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
          <el-form-item label="创建时间">
            <el-date-picker
              v-model="dateRange"
              style="width: 220px"
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
              type="info"
              plain
              icon="el-icon-s-operation"
              @click="handleOpenType"
              >显/隐分类</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              type="primary"
              plain
              icon="el-icon-plus"
              @click="handleAdd"
              :disabled="hasPermi(['system:user:add'])"
              >新增</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              type="danger"
              plain
              icon="el-icon-delete"
              @click="handleDelete"
              :disabled="hasPermi(['system:user:remove'])"
              >批量删除</el-button
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
          :data="userList"
          @selection-change="handleSelectionChange"
          @row-dblclick="rowDblclick"
          :row-style="rowStyle"
        >
          <el-table-column type="selection" width="50" align="center" />
          <el-table-column
            label="用户账号"
            min-width="100px"
            key="userName"
            prop="userName"
            v-if="columns[0].visible"
            show-overflow-tooltip
          />
          <el-table-column
            label="用户昵称"
            min-width="100px"
            key="realName"
            prop="realName"
            v-if="columns[1].visible"
            show-overflow-tooltip
          />
          <el-table-column
            label="部门"
            class-name="small-padding fixed-width"
            key="departmentName"
            prop="departmentName"
            v-if="columns[2].visible"
            show-overflow-tooltip
          />
          <el-table-column
            label="手机号码"
            align="center"
            key="mobile"
            prop="mobile"
            v-if="columns[3].visible"
            width="120"
          />
          <el-table-column
            label="状态"
            align="center"
            key="status"
            v-if="columns[4].visible"
          >
            <template slot-scope="scope">
              <!-- <el-switch
                v-model="scope.row.status"
                :active-value="1"
                :inactive-value="0"
                @change="handleStatusChange(scope.row)"
              ></el-switch> -->
              <el-tag v-if="scope.row.status == 1">正常</el-tag>
              <el-tag type="danger" v-else>停用</el-tag>
            </template>
          </el-table-column>
          <el-table-column
            label="创建时间"
            align="center"
            key="createTime"
            prop="createTime"
            v-if="columns[5].visible"
            min-width="160"
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
                :disabled="hasPermi(['system:user:edit'])"
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
                    command="handleResetPwd"
                    icon="el-icon-key"
                    :disabled="hasPermi(['system:user:resetPwd'])"
                    >重置密码</el-dropdown-item
                  >
                  <el-dropdown-item
                    command="handleDelete"
                    icon="el-icon-delete"
                    :disabled="hasPermi(['system:user:remove'])"
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
      </div>
    </div>

    <!-- 添加或修改用户配置对话框 -->
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
            <el-form-item v-if="!form.id" label="用户账号" prop="userName">
              <el-input
                v-model="form.userName"
                placeholder="请输入用户账号"
                maxlength="30"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item v-if="!form.id" label="用户密码" prop="password">
              <span slot="label">
                <el-tooltip
                  content="需由大小写字母、数字及字符（!@#$%^&*.）组成"
                  placement="top"
                >
                  <i class="el-icon-question"></i>
                </el-tooltip>
                用户密码
              </span>
              <el-input
                v-model="form.password"
                placeholder="请输入用户密码"
                type="password"
                maxlength="20"
                show-password
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="用户昵称" prop="realName">
              <el-input
                v-model="form.realName"
                placeholder="请输入用户昵称"
                maxlength="30"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="归属部门" prop="departmentId">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                v-model="form.departmentId"
                :options="deptOptions"
                :normalizer="normalizer"
                @open="changeTreeselectOpen"
                :show-count="true"
                placeholder="请选择归属部门"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="手机号码" prop="mobile">
              <el-input
                v-model="form.mobile"
                placeholder="请输入手机号码"
                maxlength="11"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="邮箱" prop="email">
              <el-input
                v-model="form.email"
                placeholder="请输入邮箱"
                maxlength="50"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="12">
            <el-form-item label="用户性别">
              <el-select
                ref="select1"
                v-model="form.sex"
                placeholder="请选择性别"
              >
                <el-option
                  v-for="dict in sexOptions"
                  :key="dict.value"
                  :label="dict.name"
                  :value="dict.value"
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
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="岗位" prop="postIds">
              <el-select
                ref="select2"
                v-model="form.postIds"
                placeholder="请选择岗位"
              >
                <el-option
                  v-for="item in postOptions"
                  :key="item.id"
                  :label="item.positionName"
                  :value="item.id"
                  :disabled="item.status == 0"
                ></el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="角色" prop="roleIds">
              <el-select
                ref="select3"
                v-model="form.roleIds"
                placeholder="请选择角色"
              >
                <el-option
                  v-for="item in roleOptions"
                  :key="item.id"
                  :label="item.roleName"
                  :value="item.id"
                  :disabled="item.status == 0"
                ></el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listUser,
  getUser,
  delUser,
  addUser,
  updateUser,
  delList,
  resetUserPwd,
  changeUserStatus,
} from "@/api/system/user";
import md5 from "js-md5";
import { getToken } from "@/utils/auth";
import { treeselect } from "@/api/system/dept";
import { listRole } from "@/api/system/role";
import { listPost } from "@/api/system/post";
export default {
  name: "User",
  dicts: ["sys_normal_disable"],
  data() {
    // ^[0-9a-zA-Z_] {1,}$.
    const checkUserName = (rule, value, callback) => {
      if (value.length < 3 || value.length > 16) {
        callback(new Error("用户账号长度需要在3-16个字符之间"));
      } else if (!/^[0-9a-zA-Z_]{1,}$/.test(value)) {
        callback(new Error("字符限制，只允许字母、数字、下划线"));
      } else {
        callback();
      }
    };
    const checkUserPassword = (rule, value, callback) => {
      const reg =
        /^(?=.*[0-9])(?=.*[!@#$%^&*.])(?=.*[a-zA-Z])(?=.*[A-Z]).{10,}$/;
      if (value.length < 10 || value.length > 16) {
        callback(new Error("密码长度需要在10-16个字符之间"));
      } else if (!reg.test(value)) {
        callback(new Error("需由大小写字母、数字及字符组成"));
      } else {
        callback();
      }
    };
    return {
      page: "user",
      optType: "",
      // 分类属性结构显隐
      openType: true,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 用户表格数据
      userList: null,
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 部门树选项
      deptOptions: undefined,
      leftDeptOptions: undefined,
      // 部门名称
      // 树形结构
      highlightCurrent: true,
      departmentStatus: undefined,
      departmentName: "",
      defaultProps: {
        children: "children",
        label: "label",
      },
      // 默认密码
      initPassword: undefined,
      // 日期范围
      dateRange: [],
      // 岗位选项
      postOptions: [],
      // 角色选项
      roleOptions: [],
      // 表单参数
      form: {},
      initialForm: {},
      sexOptions: [
        {
          name: "男",
          value: 1,
        },
        {
          name: "女",
          value: 0,
        },
      ],
      // 用户导入参数
      upload: {
        // 是否显示弹出层（用户导入）
        open: false,
        // 弹出层标题（用户导入）
        title: "",
        // 是否禁用上传
        isUploading: false,
        // 是否更新已经存在的用户数据
        updateSupport: 0,
        // 设置上传的请求头部
        headers: { Authorization: "Bearer " + getToken() },
        // 上传的地址
        url: process.env.VUE_APP_BASE_API + "/system/user/importData",
      },
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        userName: undefined,
        mobile: undefined,
        status: undefined,
        departmentId: undefined,
      },
      // 表单校验
      rules: {
        userName: [
          {
            validator: checkUserName,
            required: true,
            trigger: "blur",
          },
        ],
        realName: [
          {
            message: "用户昵称不能为空",
            required: true,
            trigger: "blur",
          },
        ],
        password: [
          {
            validator: checkUserPassword,
            required: true,
            trigger: "blur",
          },
        ],
        roleIds: [{ required: true, message: "请选择角色", trigger: "change" }],
        departmentId: [
          { required: true, message: "请选择部门", trigger: "change" },
        ],
        postIds: [{ required: true, message: "请选择岗位", trigger: "change" }],
        email: [
          {
            type: "email",
            message: "请输入正确的邮箱地址",
            trigger: ["blur", "change"],
          },
        ],
        mobile: [
          {
            pattern: /^1[3|4|5|6|7|8|9][0-9]\d{8}$/,
            message: "请输入正确的手机号码",
            trigger: "blur",
          },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: `用户账号`, visible: true },
        { key: 1, label: `用户昵称`, visible: true },
        { key: 2, label: `部门`, visible: true },
        { key: 3, label: `手机号码`, visible: true },
        { key: 4, label: `状态`, visible: true },
        { key: 5, label: `创建时间`, visible: true },
      ],
    };
  },

  activated() {
    this.queryParams.departmentId = undefined;
    this.getList();
    this.getTreeselect();
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
    /** 查询用户列表 */
    getList(isSearch) {
      this.loading = true;
      listUser(this.addDateRange(this.queryParams, this.dateRange)).then(
        (res) => {
          this.userList = res.data.list;
          this.total = res.data.total;
          this.loading = false;
          // 只有搜索状态下进行提示
          if (isSearch == "search") {
            this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
          }
        }
      );
    },
    /** 查询角色列表 */
    getRoleList() {
      listRole({ pageNum: 1, pageSize: 1000, params: {} }).then((res) => {
        this.roleOptions = res.data.list;
      });
    },
    /** 查询岗位列表 */
    getPostList() {
      listPost({ pageNum: 1, pageSize: 1000, params: {} }).then((res) => {
        this.postOptions = res.data.list;
      });
    },
    /** 查询部门下拉树结构 */
    getTreeselect() {
      treeselect().then((res) => {
        this.deptOptions = res.data;
        this.leftDeptOptions = [];
        const menu = { id: 0, status: 1, label: "全部", children: [] };
        menu.children = res.data;
        this.leftDeptOptions.push(menu);
        if (res.data[0].id == 1) {
          this.openType = true;
        } else {
          this.openType = false;
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
        label: node.label,
        children: node.children,
        isDisabled: node.status == 0,
      };
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
    // 筛选节点
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },

    // 节点单击事件
    handleNodeClick(data) {
      this.highlightCurrent = true;
      this.departmentStatus = data.status;
      if (data.label != "全部") {
        this.queryParams.departmentId = data.id;
      } else {
        this.queryParams.departmentId = undefined;
      }
      this.handleQuery();
    },
    // 用户状态修改
    handleStatusChange(row) {
      let text = row.status == "1" ? "启用" : "停用";
      this.$modal
        .confirm('确认要"' + text + '""' + row.userName + '"用户吗？')
        .then((result) => {
          if (result == "confirm") {
            // 调接口
            changeUserStatus(row.id, row.status).then((res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess(text + "成功");
              } else {
                this.$modal.notifyError(res.message);
              }
            });
          }
        })
        .catch(() => {
          // 点击取消状态按钮还原初始状态
          row.status = row.status == "1" ? 0 : 1;
        });
    },
    // 显隐分类
    handleOpenType() {
      this.openType = !this.openType;
    },

    // 表单重置
    reset() {
      this.form = {
        departmentId: undefined,
        userName: "",
        realName: "",
        password: "",
        mobile: "",
        email: "",
        sex: 1,
        status: "1",
        remark: "",
        postIds: undefined,
        roleIds: undefined,
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
      this.dateRange = [];
      this.departmentName = "";
      this.queryParams.departmentId = undefined;
      this.highlightCurrent = false;
      this.handleQuery();
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleResetPwd":
          this.handleResetPwd(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    /** 新增按钮操作 */
    handleAdd() {
      if (this.departmentStatus != null && this.departmentStatus == 0)
        return this.$modal.msgError("当前分类已停用");
      this.reset();
      // this.getTreeselect();
      this.getRoleList();
      this.getPostList();
      this.optType = "add";
      this.open = true;
      this.title = "添加用户";
      if (this.queryParams.departmentId) {
        this.form.departmentId = this.queryParams.departmentId;
      } else {
        this.form.departmentId = 1;
      }
      // this.form.password = this.initPassword;
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      // this.getTreeselect();
      this.getRoleList();
      this.getPostList();
      const userId = row.id || this.ids;
      getUser(userId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.optType = "edit";
          this.open = true;
          this.title = "修改用户";
          this.form.password = "";
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 重置密码按钮操作 */
    handleResetPwd(row) {
      this.$prompt('请输入"' + row.userName + '"的新密码', "重置密码", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        closeOnClickModal: true,
        inputPattern:
          /^(?=.*[0-9])(?=.*[!@#$%^&*.])(?=.*[a-zA-Z])(?=.*[A-Z]).{10,16}$/,
        inputErrorMessage:
          "由大小写字母、数字及字符（!@#$%^&*.）组成长度为10-16的字符",
      })
        .then(({ value }) => {
          resetUserPwd({ userId: row.id, newPassword: md5(value) }).then(
            (res) => {
              if (res.code == 0) {
                this.$modal.msgSuccess("修改成功");
                this.open = false;
                this.getList();
              } else {
                this.$modal.notifyError(res.message);
              }
              // this.$modal.msgSuccess("修改成功，新密码是：" + value);
            }
          );
        })
        .catch(() => {});

      this.optType = "resetPwd";
    },
    // 点击编码查看
    handleView(row) {
      this.reset();
      this.getTreeselect();
      this.getRoleList();
      this.getPostList();
      const userId = row.id || this.ids;
      getUser(userId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.optType = "view";
          this.open = true;
          this.title = "修改用户";
          this.form.password = "";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined && this.form.id > 0) {
        updateUser(this.form).then((res) => {
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
        addUser({ ...this.form, password: md5(this.form.password) }).then(
          (res) => {
            if (res.code == 0) {
              this.$modal.msgSuccess("新增成功");
              this.open = false;
              this.getList();
            } else {
              this.$modal.notifyError(res.message);
            }
          }
        );
      }
    },
    //删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delUser,
          this.getList,
          "用户账号为" + row.userName
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    changeTreeselectOpen() {
      const doms = document.querySelectorAll(".el-dialog .el-select");
      doms.forEach((v, i) => {
        this.$refs[`select` + (i + 1)]?.blur();
      });
    },
  },
};
</script>
