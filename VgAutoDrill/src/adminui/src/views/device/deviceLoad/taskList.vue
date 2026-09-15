<template>
  <div>
    <el-form
      :model="queryParams"
      ref="queryForm"
      :inline="true"
      v-show="showSearch"
    >
      <el-row>
        <el-col :span="24">
          <el-form-item label="任务编码" prop="code">
            <el-input
              v-trim
              v-model="queryParams.code"
              placeholder="请输入任务编码"
              clearable
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item label="工单编码" prop="workOrderCode">
            <el-input
              v-trim
              v-model="queryParams.workOrderCode"
              placeholder="请输入工单编码"
              clearable
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item label="产品编码" prop="itemCode">
            <el-input
              v-trim
              v-model="queryParams.itemCode"
              placeholder="请输入产品编码"
              clearable
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>

          <el-form-item label="任务开始时间" prop="queryStartTime">
            <el-date-picker
              v-model="queryParams.queryStartTime"
              type="datetime"
              placeholder="选择日期时间"
            >
            </el-date-picker>
          </el-form-item>
          <el-form-item label="任务结束时间" prop="queryEndTime">
            <el-date-picker
              v-model="queryParams.queryEndTime"
              type="datetime"
              placeholder="选择日期时间"
            >
            </el-date-picker>
          </el-form-item>
        </el-col>
      </el-row>
      <el-form-item label="工艺路线" prop="routeCode">
        <el-select
          v-model="queryParams.routeCode"
          placeholder="请选择 "
          @clear="clearQueryParams('routeCode')"
          clearable
        >
          <el-option
            v-for="item in routeOptions"
            :key="item.id"
            :label="item.code"
            :value="item.code"
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
      </el-form-item>
    </el-form>

    <el-row :gutter="10" class="mb8">
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
      ></right-toolbar>
    </el-row>
    <el-table v-loading="loading" :data="gridData" border>
      <el-table-column
        label="任务编码"
        fixed="left"
        min-width="150px"
        prop="code"
        show-overflow-tooltip
      >
      </el-table-column>
      <el-table-column
        label="工单编码"
        min-width="160px"
        prop="workOrderCode"
        show-overflow-tooltip
      />
      <el-table-column label="是否紧急" align="center" prop="isUrgent">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isUrgent == 1" type="danger"> 是 </el-tag>
          <el-tag v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="任务状态"
        min-width="100px"
        align="center"
        prop="taskStatus"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.taskOptions"
            :status="scope.row.taskStatus"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="工艺路线"
        min-width="150px"
        prop="routeCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="产品编码"
        min-width="180px"
        prop="itemCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="规格型号"
        prop="specification"
        show-overflow-tooltip
        min-width="120px"
      />
      <el-table-column label="排产数量" align="center" prop="quantity" />
      <el-table-column label="领取数量" align="center" prop="nowWadCount" />
      <el-table-column
        label="计划开始"
        align="center"
        min-width="150px"
        prop="startTime"
      >
        <template slot-scope="scope">
          <el-tooltip class="item" effect="dark" placement="top">
            <div slot="content">
              {{ parseTime(scope.row.startTime) }}
            </div>
            <div>
              {{ parseTime(scope.row.startTime, "{m}-{d} {h}:{i}:{s}") }}
            </div>
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column
        label="实际开始"
        align="center"
        min-width="150px"
        prop="realStartTime"
      >
        <template slot-scope="scope">
          <el-tooltip class="item" effect="dark" placement="top">
            <div slot="content">
              {{ parseTime(scope.row.realStartTime) }}
            </div>
            <div>
              {{ parseTime(scope.row.realStartTime, "{m}-{d} {h}:{i}:{s}") }}
            </div>
          </el-tooltip>
        </template>
      </el-table-column>
      <el-table-column
        label="计划完成"
        align="center"
        min-width="150px"
        prop="endTime"
      >
        <template slot-scope="scope">
          <el-tooltip class="item" effect="dark" placement="top">
            <div slot="content">
              {{ parseTime(scope.row.endTime) }}
            </div>
            <div>
              {{ parseTime(scope.row.endTime, "{m}-{d} {h}:{i}:{s}") }}
            </div>
          </el-tooltip>
        </template>
      </el-table-column>

      <el-table-column
        label="实际完成"
        min-width="150px"
        align="center"
        prop="realEndTime"
      >
        <template slot-scope="scope">
          <el-tooltip class="item" effect="dark" placement="top">
            <div slot="content">
              {{ parseTime(scope.row.realEndTime) }}
            </div>
            <div>
              {{ parseTime(scope.row.realEndTime, "{m}-{d} {h}:{i}:{s}") }}
            </div>
          </el-tooltip>
        </template>
      </el-table-column>

      <el-table-column
        label="创建时间"
        align="center"
        prop="createTime"
        min-width="150px"
      >
        <template slot-scope="scope">
          <el-tooltip class="item" effect="dark" placement="top">
            <div slot="content">
              {{ parseTime(scope.row.createTime) }}
            </div>
            <div>
              {{ parseTime(scope.row.createTime, "{m}-{d} {h}:{i}:{s}") }}
            </div>
          </el-tooltip>
        </template>
      </el-table-column>
    </el-table>
    <pagination
      v-show="gridDataTotal > 0"
      :total="gridDataTotal"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
      :autoScroll="false"
    />
  </div>
</template>

<script>
import { listAllProcess } from "@/api/produce/routeAndProcess";
import { listRoute } from "@/api/produce/route";
import { listTask } from "@/api/produce/task";
export default {
  data() {
    return {
      // 显示搜索条件
      showSearch: true,
      loading: true,
      gridData: [],
      gridDataTotal: 0,
      processOptions: [],
      routeOptions: [],
      // 创建时间
      dateRange: [],
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        itemCode: undefined,
        itemTypeId: undefined,
        batchCode: undefined,
        taskStatusList: [10],
        workOrderId: undefined,
        workOrderCode: undefined,
        processId: undefined,
        processCode: undefined,
        requestDate: undefined,
        status: undefined,
        taskNumber: undefined,
        startDate: undefined,
        workStationCode: undefined,
        queryStartTime: undefined,
        queryEndTime: undefined,
        routeId: undefined,
        routeCode: undefined,
        routeName: undefined,
      },
    };
  },
  created() {
    this.getProcess();
    this.getRoute();
  },
  activated() {
    this.getProcess();
    this.getRoute();
  },
  methods: {
    //查询所属工序信息
    async getProcess() {
      const res = await listAllProcess({});
      this.processOptions = res.data.list;
    },
    // 查询工艺路线
    async getRoute() {
      const res = await listRoute({ vettingStatus: 1, pageNum: 1, pageSize: 100 });
      this.routeOptions = res.data.list;
    },
    async getList() {
      this.loading = true;
      const res = await listTask(this.queryParams);
      this.gridData = res.data.list;
      this.gridDataTotal = res.data.total;
      this.loading = false;
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList();
    },
    /** 重置按钮操作 */
    resetQuery() {
      // 重置时时间范围也重置
      this.queryParams.taskStatusList = [];
      this.queryParams.queryEndTime = undefined;
      this.queryParams.queryStartTime = undefined;
      this.dateRange = [];
      this.resetForm("queryForm");
      this.handleQuery();
    },
    // 选择创建时间
    changeDate(val) {
      if (val == null) {
        this.queryParams.queryStartTime = undefined;
        this.queryParams.queryEndTime = undefined;
      } else {
        this.queryParams.queryStartTime = val[0] && val[0];
        this.queryParams.queryEndTime = val[1] && val[1];
      }
    },
    /** 提交按钮 */
    submitForm() {},
  },
};
</script>

<style lang="scss" scoped></style>
