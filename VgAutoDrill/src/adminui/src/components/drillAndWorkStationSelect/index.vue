<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="机台选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-form-item label="当前工单号">
        <el-input
          v-trim
          v-model="queryParams.workOrderCode"
          placeholder="当前工单号"
          disabled
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工艺路线">
        <el-input
          v-trim
          v-model="queryParams.routeCode"
          placeholder="工艺路线"
          disabled
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="机台编码" prop="workStationCode">
        <el-input
          v-trim
          v-model="queryParams.workStationCode"
          placeholder="请输入机台编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="机台名称" prop="workStationName">
        <el-input
          v-trim
          v-model="queryParams.workStationName"
          placeholder="请输入机台名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="设备状态" prop="deviceStatusList">
        <el-select
          v-model="queryParams.deviceStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.deviceStatusList.length >= 2 ? 'select-hastags' : ''
          "
        >
          <el-option
            v-for="item in $status.deviceStatusOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="已选">
        <el-input
          :value="showData.length + ' ' + '个'"
          disabled
          style="width: 100px"
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
      :data="drillAndWorkStationList"
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
        label="机台编码"
        prop="code"
        show-overflow-tooltip
        :min-width="
          flexColumnWidth('机台编码', 'code', drillAndWorkStationList)
        "
      />
      <el-table-column
        label="机台名称"
        prop="name"
        :min-width="
          flexColumnWidth('机台名称', 'name', drillAndWorkStationList)
        "
        show-overflow-tooltip
      />
      <el-table-column label="设备状态" align="center" width="80px">
        <template slot-scope="scope">
          <status-tag
            :options="$status.deviceStatusOptions"
            :status="scope.row.deviceStatus"
          ></status-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="开始空闲时间"
        width="180px"
        align="center"
        prop="beginFreeTime"
        show-overflow-tooltip
      />
      <el-table-column
        label="待做任务"
        width="80px"
        align="center"
        prop="toDoTaskCount"
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
import { getFitWorkStationList } from "@/api/produce/drillWorkOrder";
export default {
  name: "ProductCategorySelect",
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
      // 机台表格数据
      drillAndWorkStationList: [],
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        workOrderCode: undefined,
        workOrderId: undefined,
        routeCode: undefined,
        fitCount: 0,
        processCode: "drill",
        workStationCode: undefined,
        workStationName: undefined,
        deviceStatusList: [],
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
    /** 查询机台列表 */
    getList(isSearch) {
      this.loading = true;
      getFitWorkStationList(this.queryParams).then((res) => {
        this.drillAndWorkStationList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
        // 多选框回显
        this.$nextTick((res) => {
          this.drillAndWorkStationList.forEach((val) => {
            const showDataId = this.showData.map((v) => v.workStationId);
            if (showDataId?.length && showDataId.includes(val.id)) {
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
      let findRow = this.showData.find((c) => c.workStationId == row.rowId);
      //找到选中的行
      if (findRow) {
        this.showData = this.showData.filter((v) => v.workStationId != row.id);
        refsElTable.toggleRowSelection(row, false); //如过重复选中，则取消选中
        return;
      }
      this.showData.push({
        workStationId: row.id,
        workStationCode: row.code,
        workStationName: row.name,
      });
      refsElTable.toggleRowSelection(row, true); // 实现选中行中选中事件
    },
    //获取table组件中的全选checkbox的勾选状态
    getIsAllChecked() {
      return this.$refs.multipleTable.store.states.isAllSelected;
    },
    // 全选、反选
    handelSelectAll(selection) {
      if (this.getIsAllChecked()) {
        const showDataId = this.showData.map((v) => v.workStationId);
        this.drillAndWorkStationList.forEach((v) => {
          if (showDataId.includes(v.id)) return;
          this.showData.push({
            workStationId: v.id,
            workStationCode: v.code,
            workStationName: v.name,
          });
        });
      } else {
        const showDataId = this.drillAndWorkStationList.map((v) => v.id);
        this.showData = this.showData.filter((v) => {
          return !showDataId.includes(v.workStationId);
        });
      }
    },
    // 单选
    handelSelect(selection, row) {
      const showDataId = this.showData.map((v) => v.workStationId);
      if (showDataId.includes(row.id)) {
        this.showData = this.showData.filter((v) => v.workStationId != row.id);
      } else {
        this.showData.push({
          workStationId: row.id,
          workStationCode: row.code,
          workStationName: row.name,
        });
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
