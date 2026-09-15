<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="工作站选择"
    :center="true"
    @submitForm="confirmSelect"
    @cancel="showFlag = false"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="工作站编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入工作站编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工作站名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入工作站名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="所在车间" prop="workshopName">
        <el-select
          clearable
          v-model="queryParams.workshopName"
          placeholder="请选择车间"
          @clear="clearQueryParams('workshopName')"
          @change="selectedWorkShop"
        >
          <el-option
            v-for="item in workshopOptions"
            :key="item.id"
            :value="item.name"
            :label="item.label"
            v-optionTitle
          ></el-option>
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
        <el-button v-debounce icon="el-icon-refresh" @click="resetQuery"
          >重置</el-button
        >
        <el-button
          v-debounce
          v-if="type == 'wareHouse'"
          type="danger"
          icon="el-icon-plus"
          @click="handleAdd"
          >新增</el-button
        >
      </el-form-item>
    </el-form>
    <el-table
      border
      v-loading="loading"
      :data="workStationList"
      @current-change="handleCurrent"
      @row-dblclick="handleRowDbClick"
    >
      <el-table-column width="55" align="center">
        <template v-slot="scope">
          <el-radio
            v-removeAriaHidden
            v-model="selectedWorkStationId"
            :label="scope.row.id"
            @change="handleRowChange(scope.row)"
            >{{ "" }}</el-radio
          >
        </template>
      </el-table-column>
      <el-table-column
        label="工作站编码"
        min-width="150"
        prop="code"
        show-overflow-tooltip
      />
      <el-table-column
        label="工作站名称"
        min-width="150"
        prop="name"
        show-overflow-tooltip
      />
      <el-table-column
        v-if="type != 'wareHouse'"
        label="工序编码"
        min-width="150"
        align="center"
        prop="processCode"
        show-overflow-tooltip
      />
      <el-table-column
        v-if="type != 'wareHouse'"
        align="center"
        label="工序名称"
        min-width="150"
        prop="processName"
        show-overflow-tooltip
      />
      <el-table-column
        label="所在车间"
        min-width="150"
        prop="workshopName"
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
import { addWorkstation, listWorkstation } from "@/api/masterData/workStation";
import { listWorkshop } from "@/api/masterData/workShop";
export default {
  name: "WorkStationSelect", 
  props: ["type", "optType"],
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 选中数组
      selectedWorkStationId: undefined,
      selectedRows: [],

      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工作站表格数据
      workStationList: [],
      //车间选项
      workshopOptions: [],
      //工序选项
      processOptions: [],
      // 弹出层标题
      title: "",

      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        workshopId: undefined,
        workshopCode: undefined,
        workshopName: undefined,
      },
    };
  },
  watch: {
    showFlag(val) {
      if (!val) {
        this.resetForm("queryForm");
        this.selectedWorkStationId = undefined;
      }
    },
  },
  methods: {
    /** 查询工作站列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listWorkstation(this.queryParams);
      this.workStationList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },

    //查询车间信息
    getWorkshops() {
      listWorkshop({
        pageNum: 1,
        pageSize: 100,
      }).then((res) => {
        this.workshopOptions = res.data.list?.map((v) => {
          return { ...v, label: v.code + "---" + v.name };
        });
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
        this.selectedRows = row;
      }
    },
    // 单选选中数据
    handleRowChange(row) {
      if (row) {
        this.selectedRows = row;
      }
    },
    //双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRows = row;
        this.$emit("onSelected", this.selectedRows);
        this.showFlag = false;
        this.selectedWorkStationId = undefined;
        this.selectedRows = undefined;
      }
    },

    //确定选中
    confirmSelect() {
      if (
        this.selectedWorkStationId == null ||
        this.selectedWorkStationId == 0
      ) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRows);
      this.showFlag = false;
      this.selectedWorkStationId = undefined;
      this.selectedRows = undefined;
    },
    handleAdd() {
      const code = this.queryParams.code;
      const name = this.queryParams.name;
      if (code && name) {
        addWorkstation({ code, name, ...this.form }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        this.$modal.msgError("编码与名称不能为空");
      }
    },
    selectedWorkShop(val) {
      if (val != "") {
        this.form = {
          workshopId: this.workshopOptions.find((v) => v.name == val).id,
          workshopCode: this.workshopOptions.find((v) => v.name == val).code,
          workshopName: this.workshopOptions.find((v) => v.name == val).name,
        };
      }
    },
  },
};
</script>
