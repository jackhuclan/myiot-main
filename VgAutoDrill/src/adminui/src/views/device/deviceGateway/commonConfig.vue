<template>
  <edit-form-dialog
    :open="showFlag"
    v-if="showFlag"
    title="修改公共配置"
    :center="true"
    @submitForm="submitForm"
    @cancel="showFlag = false"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
      @submit.native.prevent
    >
      <el-form-item label="网关名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="网关类型" prop="name">
        <el-select
          @clear="clearQueryParams('serviceName')"
          v-model="queryParams.serviceName"
          placeholder="网关类型"
          clearable
        >
          <el-option label="左边" value="左边" />
          <el-option label="右边" value="右边" />
        </el-select>
      </el-form-item>
      <el-form-item label="程序名称" prop="serviceName">
        <el-select
          @clear="clearQueryParams('serviceName')"
          v-model="queryParams.serviceName"
          placeholder="程序名称"
          clearable
        >
          <el-option label="AGV" value="AGV" />
          <el-option label="DRILL" value="DRILL" />
        </el-select>
      </el-form-item>
      <el-form-item label="版本号" prop="aVersion">
        <el-select
          :disabled="!queryParams.serviceName"
          @clear="clearQueryParams('aVersion')"
          v-model="queryParams.aVersion"
          placeholder="请选择版本号"
          clearable
        >
          <!-- <el-option
            v-for="dict in eventLevelOptions"
            :key="dict.value"
            :label="dict.label"
            :value="dict.value"
          /> -->
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button
          v-debounce
          type="primary"
          icon="el-icon-search"
          @click="handleQuery"
          >搜索</el-button
        >
        <el-button
          v-debounce
          icon="el-icon-refresh"
          @click="resetQuery"
          >重置</el-button
        >
      </el-form-item>
    </el-form>
    <el-table
      border
      v-loading="loading"
      :data="colorList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="网关名称"
        prop="name"
        min-width="120"
        show-overflow-tooltip
      />
      <el-table-column
        label="网关类型"
        prop="name"
        min-width="120"
        show-overflow-tooltip
      />
      <el-table-column
        label="程序名称"
        prop="remark"
        min-width="120"
        show-overflow-tooltip
      />
      <el-table-column label="版本号" align="center" prop="status">
      </el-table-column>
      <el-table-column label="配置" align="center" prop="status">
      </el-table-column>
    </el-table>

    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />
    <el-form
      ref="form"
      :model="form"
      label-width="100px"
      label-position="top"
      :rules="rules"
    >
      <el-form-item label="公共配置 ：" prop="parameters">
        <b-code-editor
          v-if="showFlag"
          ref="editor"
          v-model="form.parameters"
          :indent-unit="4"
      /></el-form-item>
    </el-form>
  </edit-form-dialog>
</template>

<script>
export default {
  name: "ColorSelect",
  watch: {
    showFlag(val) {
      if (!val) {
        this.selectedColorId = 0;
        this.resetForm("queryForm");
      }
    },
  },
  data() {
    return {
      showFlag: false,
      selectedColorId: undefined,
      selectedRow: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 客户表格数据
      colorList: [
        { name: "珊瑚色", value: "#f8aba6" },
        { name: "薄柿色", value: "#ca8687" },
        { name: "焦茶色", value: "#6b473c" },
        { name: "柑子色", value: "#faa755" },
        { name: "胭脂色", value: "#b3424a" },
      ],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        status: 1,
      },
      // 表单参数
      form: {
        parameters: "",
      },
      // 表单校验
      rules: {
        workstationId: [
          { required: true, message: "工作站不能为空", trigger: "blur" },
        ],
        quantity: [
          { required: true, message: "排产数量不能为空", trigger: "blur" },
        ],
        startTime: [
          { required: true, message: "请选择开始生产日期", trigger: "blur" },
        ],
        duration: [
          { required: true, message: "清输入估算的生产用时", trigger: "blur" },
        ],
      },
    };
  },
  methods: {
    /** 查询客户列表 */
    async getList(isSearch) {
      //   this.loading = true;
      //   const res = await listClient(this.queryParams);
      //   this.clientList = res.data.list;
      // this.total = res.data.total;

      this.total = 10;
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
    // 点击确定
    submitForm() {
      //   this.showFlag = false;
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
  },
};
</script>
