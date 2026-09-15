<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="人员选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-row :gutter="20">
      <!--部门数据-->
      <el-col :span="6" :xs="24">
        <leftTreeSelect
          v-model="deptName"
          :placeholder="'请输入部门名称'"
          :filterNode="filterNode"
          :options="deptOptions"
          :defaultProps="defaultProps"
          :highlightCurrent="highlightCurrent"
          :handleNodeClick="handleNodeClick"
        ></leftTreeSelect>
      </el-col>
      <!--用户数据-->
      <el-col :span="18" :xs="24">
        <el-form
          :model="queryParams"
          ref="queryForm"
          :inline="true"
          v-show="showSearch"
        >
          <el-form-item label="用户账号" prop="userName">
            <el-input
              v-trim
              v-model="queryParams.userName"
              placeholder="请输入用户账号"
              clearable
              style="width: 240px"
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item label="手机号码" prop="mobile">
            <el-input
              v-trim
              v-model="queryParams.mobile"
              placeholder="请输入手机号码"
              clearable
              style="width: 240px"
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item>
            <el-button
              type="primary"
              icon="el-icon-search"
              @click="handleQuery"
              v-debounce
              >搜索</el-button
            >
            <el-button icon="el-icon-refresh" @click="resetQuery" v-debounce
              >重置</el-button
            >
          </el-form-item>
        </el-form>
        <el-table
          border
          v-loading="loading"
          :data="userList"
          @current-change="handleCurrent"
          @row-dblclick="handleRowDbClick"
        >
          <el-table-column width="55" align="center">
            <template v-slot="scope">
              <el-radio
                v-removeAriaHidden
                v-model="selectedId"
                :label="scope.row.realName"
                @change="handleRowChange(scope.row)"
                >{{ "" }}</el-radio
              >
            </template>
          </el-table-column>
          <el-table-column
            label="用户账号"
            min-width="150"
            prop="userName"
            show-overflow-tooltip
          />
          <el-table-column
            label="用户昵称"
            min-width="150"
            prop="realName"
            show-overflow-tooltip
          />

          <el-table-column
            label="部门"
            min-width="150"
            prop="departmentName"
            show-overflow-tooltip
          />
          <el-table-column
            label="手机号码"
            min-width="150"
            prop="mobile"
            show-overflow-tooltip
          />
          <el-table-column
            label="创建时间"
            align="center"
            prop="createTime"
            show-overflow-tooltip
            width="180"
          >
            <template slot-scope="scope">
              <span>{{ parseTime(scope.row.createTime) }}</span>
            </template>
          </el-table-column>
        </el-table>

        <pagination
          v-show="total > 0"
          :total="total"
          :page.sync="queryParams.pageNum"
          :limit.sync="queryParams.pageSize"
          @pagination="getList"
          :autoScroll="false"
        />
      </el-col>
    </el-row>
  </select-form-dialog>
</template>

<script>
import { listUser } from "@/api/system/user";
import { treeselect } from "@/api/system/dept";

export default {
  name: "UserSingleSelect",
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      selectedId: null,
      selectedRow: null,
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 用户表格数据
      userList: null,
      // 弹出层标题
      title: "",
      // 部门树选项
      deptOptions: undefined,
      highlightCurrent: true,
      // 部门名称
      deptName: "",
      // 日期范围
      dateRange: [],
      // 岗位选项
      postOptions: [],
      // 角色选项
      roleOptions: [],

      defaultProps: {
        children: "children",
        label: "label",
      },
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        userName: undefined,
        mobile: undefined,
        status: 1,
        departmentId: undefined,
      },
    };
  },
  created() {
    // this.getList();
    // this.getTreeselect();
  },
  methods: {
    /** 查询用户列表 */
    getList(isSearch) {
      this.loading = true;
      listUser(this.addDateRange(this.queryParams, this.dateRange)).then(
        (response) => {
          this.userList = response.data.list;
          this.total = response.data.total;
          this.loading = false;
          // 只有搜索状态下进行提示
          if (isSearch == "search") {
            this.$modal.msgSearch(
              "搜索成功，共" + response.data.total + "条数据！"
            );
          }
        }
      );
    },
    /** 查询部门下拉树结构 */
    getTreeselect() {
      treeselect().then((res) => {
        this.deptOptions = [];
        const menu = { id: 0, status: 1, label: "全部", children: [] };
        menu.children = res.data;
        this.deptOptions.push(menu);
      });
    },
    // 筛选节点
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    // 节点单击事件
    handleNodeClick(data) {
      this.highlightCurrent = true;
      if (data.label != "全部") {
        this.queryParams.departmentId = data.id;
      } else {
        this.queryParams.departmentId = undefined;
      }
      this.handleQuery();
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.highlightCurrent = false;
      this.queryParams.departmentId = undefined;
      this.deptName = "";
      this.dateRange = [];
      this.resetForm("queryForm");
      this.handleQuery();
    },
    handleCurrent(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //行双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRow = row;
        this.$emit("onSelected", this.selectedRow);
        this.showFlag = false;
        this.selectedId = undefined;
        this.selectedRow = undefined;
      }
    },
    // 单选选中数据
    handleRowChange(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectedId == null || this.selectedId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectedId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
