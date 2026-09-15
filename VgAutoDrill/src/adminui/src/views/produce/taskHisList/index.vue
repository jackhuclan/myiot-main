<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="排序方式" prop="queryOrderBy">
        <el-select
          @change="(e) => handleInput(e, 'queryOrderBy')"
          v-model="queryParams.queryOrderBy"
          placeholder="请选择"
          style="width: 150px"
        >
          <el-option
            v-for="item in $status.queryOrderByOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="任务编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入任务编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <!-- 多选任务状态 -->
      <el-form-item label="任务状态" prop="taskStatusList">
        <el-select
          @change="(e) => handleInput(e, 'taskStatusList')"
          v-model="queryParams.taskStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.taskStatusList && queryParams.taskStatusList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.taskOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="是否自动" prop="isAuto">
        <el-select
          @clear="clearQueryParams('isAuto')"
          v-model="queryParams.isAuto"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option label="自动" :value="true" />
          <el-option label="非自动" :value="false" />
        </el-select>
      </el-form-item>
      <!-- 单选任务状态 -->
      <!-- <el-form-item label="任务状态" prop="taskStatus">
        <el-select
          @change="handleSelectStatus"
          v-model="queryParams.taskStatus"
          placeholder="请选择"
          style="width: 150px"
        >
          <el-option
            v-for="item in $status.taskOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item> -->
      <el-form-item label="是否紧急" prop="isUrgent">
        <el-select
          @clear="clearQueryParams('isUrgent')"
          v-model="queryParams.isUrgent"
          @change="(e) => handleInput(e, 'isUrgent')"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option label="是" :value="1" />
          <el-option label="否" :value="0" />
        </el-select>
      </el-form-item>
      <el-form-item label="工单编码" prop="workOrderCode">
        <el-input
          v-trim
          v-model="queryParams.workOrderCode"
          placeholder="请输入工单编码"
          @change="(e) => handleInput(e, 'workOrderCode')"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="产品编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入产品编码"
          @change="(e) => handleInput(e, 'itemCode')"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工作站" prop="workStationCode">
        <el-input
          v-trim
          v-model="queryParams.workStationCode"
          placeholder="请输入工作站"
          @change="(e) => handleInput(e, 'workStationCode')"
          clearable
          @keyup.enter.native="handleQuery"
        /> </el-form-item
      ><el-form-item label="工艺路线" prop="routeCodes">
        <el-select
          v-model="queryParams.routeCodes"
          @change="(e) => handleInput(e, 'routeCodes')"
          placeholder="请输入工艺路线"
          multiple
          collapse-tags
          :class="
            queryParams.routeCodes && queryParams.routeCodes.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in routeQueryList"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          >
          </el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="所属工序" prop="processCode">
        <el-select
          v-model="queryParams.processCode"
          placeholder="请选择"
          @clear="clearProcessCode('processCode')"
          @change="(e) => handleInput(e, 'processCode')"
          clearable
        >
          <el-option
            v-for="item in processOptions"
            :key="item.id"
            :label="item.label"
            :value="item.code"
            v-optionTitle
          ></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="任务开始时间">
        <el-date-picker
          v-model="queryParams.queryStartTime"
          type="datetime"
          placeholder="起始时间"
          :picker-options="pickerCreateStart"
        >
        </el-date-picker>
        ↔
        <el-date-picker
          :picker-options="pickerCreateEnd"
          v-model="queryParams.queryEndTime"
          type="datetime"
          placeholder="结束时间"
        >
        </el-date-picker>
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['produce:taskHisList:export'])"
          >导出</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
        :columns="columns"
        :page="page"
      ></right-toolbar>
    </el-row>
    <el-table border :data="taskList" :ref="page">
      <el-table-column
        label="任务编码"
        fixed="left"
        min-width="160px"
        key="code"
        prop="code"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
      </el-table-column>
      <el-table-column
        label="工单编码"
        min-width="160px"
        key="workOrderCode"
        prop="workOrderCode"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="是否紧急"
        align="center"
        key="isUrgent"
        prop="isUrgent"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isUrgent == 1" type="danger"> 是 </el-tag>
          <el-tag v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="任务状态"
        align="center"
        key="taskStatus"
        prop="taskStatus"
        min-width="100px"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.taskOptions"
            :status="scope.row.taskStatus"
          />
        </template>
      </el-table-column>

      <el-table-column
        label="是否自动"
        align="center"
        key="isAuto"
        min-width="100px"
        v-if="columns[16].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isAuto"> 自动 </el-tag>
          <span v-else-if="scope.row.isAuto == undefined"></span>
          <el-tag v-else type="danger">非自动</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="所属工序"
        min-width="120px"
        key="processCode"
        prop="processCode"
        show-overflow-tooltip
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          {{
            scope.row.processCode && scope.row.processName
              ? scope.row.processCode + "---" + scope.row.processName
              : ""
          }}
        </template></el-table-column
      >

      <el-table-column
        label="工作站"
        min-width="150px"
        key="workStationCode"
        prop="workStationCode"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        label="工艺路线"
        min-width="150px"
        key="routeCode"
        prop="routeCode"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />
      <el-table-column
        label="产品编码"
        min-width="180px"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        v-if="columns[7].visible"
      >
      </el-table-column>
      <el-table-column
        label="规格型号"
        key="specification"
        prop="specification"
        show-overflow-tooltip
        min-width="120px"
        v-if="columns[8].visible"
      />
      <el-table-column
        label="排产数量"
        align="center"
        key="quantity"
        prop="quantity"
        v-if="columns[9].visible"
      />
      <el-table-column
        label="领取数量"
        align="center"
        key="nowWadCount"
        prop="nowWadCount"
        v-if="columns[10].visible"
      />
      <el-table-column
        label="创建时间"
        align="center"
        key="createTime"
        prop="createTime"
        min-width="130px"
        v-if="columns[11].visible"
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
      <el-table-column
        label="实际开始"
        min-width="130px"
        align="center"
        key="realStartTime"
        prop="realStartTime"
        v-if="columns[12].visible"
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
        label="实际完成"
        min-width="130px"
        align="center"
        key="realEndTime"
        prop="realEndTime"
        v-if="columns[13].visible"
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
        label="计划开始"
        min-width="130px"
        align="center"
        key="startTime"
        prop="startTime"
        v-if="columns[14].visible"
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
        label="计划完成"
        min-width="130px"
        align="center"
        key="endTime"
        prop="endTime"
        v-if="columns[15].visible"
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
    </el-table>

    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />
  </div>
</template>

<script>
import { listTaskHisList } from "@/api/produce/task";
import { getDropSelectDatas as getProcessSelectDatas } from "@/api/produce/process";

import { getDropSelectDatas } from "@/api/produce/route";
export default {
  name: "TaskHisList",
  data() {
    return {
      page: "taskHisList",
      // 遮罩层
      loading: true,
      // 选中数组
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 生产报工记录表格数据
      taskList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 表单参数
      form: {},
      // 查询参数
      queryParams: this.$cache.local.get("taskHis_queryParams")
        ? JSON.parse(this.$cache.local.get("taskHis_queryParams"))
        : {
            pageNum: 1,
            pageSize: 10,
            name: undefined,
            code: undefined,
            workOrderCode: undefined,
            itemCode: undefined,
            itemTypeId: undefined,
            batchCode: undefined,
            workStationCode: undefined,
            workOrderId: undefined,
            processId: undefined,
            processCode: undefined,
            status: undefined,
            queryStartTime: undefined,
            queryEndTime: undefined,
            taskStatusList: [],
            isUrgent: undefined,
            queryOrderBy: 4,
            routeCodes: [],
            isAuto: undefined,
          },
      //.限制结束时间必须大于等于开始时间
      pickerCreateEnd: {
        disabledDate: (time) => {
          if (this.queryParams.queryStartTime) {
            return (
              time.getTime() <=
              new Date(this.queryParams.queryStartTime).getTime() - 86400000
            );
          }
        },
      },
      // 限制开始日期必须小于结束时间或当前日期
      pickerCreateStart: {
        disabledDate: (time) => {
          if (this.queryParams.queryEndTime) {
            return (
              time.getTime() > Date.now() ||
              time.getTime() >
                new Date(this.queryParams.queryEndTime).getTime() -
                  8.64e6 /*开始日期要在选择的结束日期之前 若结束日期大于当前日期 则开始日期为小于当前日期*/
            );
          }
        },
      },
      // 工艺路线
      routeQueryList: [],
      // 所属工序
      processOptions: [],
      // 创建时间
      dateRange: [],
      // 列信息
      columns: [
        { key: 0, label: "任务编码", visible: true },
        { key: 1, label: "工单编码", visible: true },
        { key: 2, label: "是否紧急", visible: true },
        { key: 3, label: "任务状态", visible: true },
        { key: 4, label: "工序编码", visible: true },
        { key: 5, label: "工作站", visible: true },
        { key: 6, label: "工艺路线", visible: true },
        { key: 7, label: "产品编码", visible: true },
        { key: 8, label: "规格型号", visible: true },
        { key: 9, label: "排产数量", visible: true },
        { key: 10, label: "领取数量", visible: true },
        { key: 11, label: "创建时间", visible: true },
        { key: 12, label: "实际开始", visible: true },
        { key: 13, label: "实际完成", visible: true },
        { key: 14, label: "计划开始", visible: true },
        { key: 15, label: "计划完成", visible: true },
        { key: 16, label: "是否自动", visible: true },
      ],
    };
  },

  computed: {
    queryParamsChange() {
      return { ...this.queryParams };
    },
  },
  watch: {
    queryParamsChange: {
      handler(val) {
        this.$cache.local.set(
          "taskHis_queryParams",
          JSON.stringify({ ...val })
        );
      },
      deepL: true,
      immediate: true,
    },
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  methods: {
    // 查询工艺路线
    getRouteList() {
      getDropSelectDatas({
        vettingStatus: 1,
        pageNum: 1,
        pageSize: 100,
      }).then((res) => {
        this.routeQueryList = res.data?.map((v) => {
          return { ...v, label: `${v.code}(${v.name})` };
        });
      });
    },

    //查询工序信息
    getProcessList() {
      // 工序状态为正常的
      getProcessSelectDatas({ pageNum: 1, pageSize: 1000, status: 1 }).then(
        (res) => {
          this.processOptions = res.data;
        }
      );
    },
    /** 查询任务列表 */
    getList(isSearch) {
      this.getProcessList();
      this.getRouteList();
      this.loading = true;
      listTaskHisList(this.queryParams).then((res) => {
        this.taskList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    // 表单重置
    reset() {
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.queryParams = {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        workOrderCode: undefined,
        itemCode: undefined,
        itemTypeId: undefined,
        batchCode: undefined,
        workStationCode: undefined,
        workOrderId: undefined,
        processId: undefined,
        processCode: undefined,
        status: undefined,
        queryStartTime: undefined,
        queryEndTime: undefined,
        taskStatusList: [],
        isUrgent: undefined,
        queryOrderBy: 4,
        routeCodes: [],
        isAuto: undefined,
      };
      this.handleQuery();
    },
    // 解决v-model不回显 监听不到变化
    handleInput(val, query) {
      this.queryParams[query] = val;
      this.$forceUpdate();
    },

    // 单选任务状态
    handleSelectStatus(val) {
      this.$cache.local.setJSON("task_Status", val);
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/Task/DownLoadTaskHisList",
        "任务历史.xlsx",
        this.queryParams
      );
    },
  },
};
</script> 