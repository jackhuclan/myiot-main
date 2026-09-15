<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="项目选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <!-- 设备维护页面与其他页面使用的方式不一样 -->
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      width="100%"
      v-show="showSearch"
    >
      <el-form-item label="项目编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入项目编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="项目名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入项目名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="项目类型" prop="subjectType">
        <el-select
          v-model="queryParams.subjectType"
          placeholder="请选择项目类型"
          clearable
          @clear="clearQueryParams('subjectType')"
        >
          <el-option :value="'点检'">点检</el-option>
          <el-option :value="'保养'">保养</el-option>
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button
          type="primary"
          icon="el-icon-search"
          @click="handleQuery"
          v-debounce
          >搜索</el-button
        >
        <el-button
          icon="el-icon-refresh"
          @click="resetQuery"
          v-debounce
          >重置</el-button
        >
        <el-button
          icon="el-icon-delete"
          type="danger"
          plain
          @click="removeShowData"
          >清除选中项</el-button
        >
      </el-form-item>
    </el-form>

    <el-table
      border
      v-loading="loading"
      width="100%"
      :data="dvsubjectList"
      ref="multipleTable"
      @select-all="handelSelectAll"
      @select="handelSelect"
      @row-dblclick="rowDblclick"
      :row-style="echoRowStyle"
      :row-key="
        (row) => {
          return row.id;
        }
      "
    >
      <el-table-column
        type="selection"
        width="55"
        align="center"
        :reserve-selection="true"
      />
      <el-table-column
        label="项目编码"
        min-width="150"
        prop="code"
        show-overflow-tooltip
      />
      <el-table-column
        label="项目名称"
        min-width="150"
        prop="name"
        show-overflow-tooltip
      />
      <el-table-column
        label="项目类型"
        align="center"
        prop="subjectType"
        show-overflow-tooltip
      />
      <el-table-column
        label="项目内容"
        min-width="150"
        prop="subjectContent"
        show-overflow-tooltip
      />
      <el-table-column
        label="标准"
        min-width="150"
        prop="standard"
        show-overflow-tooltip
      />
    </el-table>
    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
      :autoScroll="false"
    />
  </select-form-dialog>
</template>

<script>
import { listSubject } from "@/api/device/subject";
export default {
  name: "SubjectSelectCheckbox",
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      showData: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 设备点检保养项目表格数据
      dvsubjectList: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: null,
        code: null,
        subjectType: null,
        subjectContent: null,
        standard: null,
        status: 1,
      },
    };
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.resetForm("queryForm");
      }
    },
  },
  methods: {
    /** 查询设备点检保养项目列表 */
    getList(isSearch) {
      this.loading = true;
      listSubject(this.queryParams).then((res) => {
        this.dvsubjectList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
        // 多选框回显
        this.$nextTick((res) => {
          this.dvsubjectList.forEach((val) => {
            if (this.showData?.length && this.showData.includes(val.id)) {
              this.$refs.multipleTable?.toggleRowSelection(val, true);
            }
          });
        });
      });
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      // 获取表格对象
      let refsElTable = this.$refs.multipleTable;
      let findRow = this.showData.find((c) => c == row.rowId);
      //找到选中的行
      if (findRow) {
        this.showData = this.showData.filter((v) => v != row.id);
        refsElTable.toggleRowSelection(row, false); //如过重复选中，则取消选中
        return;
      }
      this.showData.push(row.id);
      refsElTable.toggleRowSelection(row, true); // 实现选中行中选中事件
    },

    //获取table组件中的全选checkbox的勾选状态
    getIsAllChecked() {
      return this.$refs.multipleTable.store.states.isAllSelected;
    },
    // 全选、反选
    handelSelectAll(selection) {
      if (this.getIsAllChecked()) {
        this.dvsubjectList.forEach((v) => {
          if (!this.showData.includes(v.id)) {
            this.showData.push(v.id);
          }
        });
      } else {
        const dvsubjectId = this.dvsubjectList.map((v) => v.id);
        this.showData = this.showData.filter((v) => !dvsubjectId.includes(v));
      }
    },
    // 单选
    handelSelect(selection, row) {
      if (this.showData.includes(row.id)) {
        this.showData = this.showData.filter((v) => v != row.id);
      } else {
        this.showData.push(row.id);
      }
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.resetForm("queryForm");
      this.handleQuery();
    },
    // 清除选中项
    removeShowData() {
      if (this.showData.length <= 0)
        return this.$modal.msgWarning("未选中任何数据!");
      this.showData = [];
      this.$refs.multipleTable.clearSelection();
    },
    //确定选中
    confirmSelect() {
      if (this.showData == [] || this.showData.length == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.showData);
      this.showFlag = false;
    },
  },
};
</script>
