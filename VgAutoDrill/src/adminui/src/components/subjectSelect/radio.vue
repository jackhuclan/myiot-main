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
        <el-button type="primary" icon="el-icon-search" @click="handleQuery"
          >搜索</el-button
        >
        <el-button icon="el-icon-refresh" @click="resetQuery">重置</el-button>
      </el-form-item>
    </el-form>

    <el-table
      border
      v-loading="loading"
      :data="dvsubjectList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectSubjectId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="项目编码"
        min-width="150"
        show-overflow-tooltip
        prop="code"
      />
      <el-table-column
        label="项目名称"
        min-width="150"
        show-overflow-tooltip
        prop="name"
      />
      <el-table-column label="项目类型" align="center" prop="subjectType" />
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
  name: "SubjectSelect",
  props: ["type"],
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,

      // 总条数
      total: 0,
      // 设备点检保养项目表格数据
      dvsubjectList: [],
      selectSubjectId: undefined,
      selectedRow: undefined,
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
        this.selectSubjectId = undefined;
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
      });
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

    handleCurrent(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    // 单选选中数据
    handleRowChange(row) {
      if (row) {
        this.selectedRow = row;
      }
    },
    //双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRow = row;
        this.$emit("onSelected", this.selectedRow);
        this.showFlag = false;
        this.selectSubjectId = undefined;
        this.selectedRow = undefined;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectSubjectId == null || this.selectSubjectId == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRow);
      this.showFlag = false;
      this.selectSubjectId = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
