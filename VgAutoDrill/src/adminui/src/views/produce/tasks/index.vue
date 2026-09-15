<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="是否刷新">
        <!-- <el-checkbox v-model="queryParams.checked"></el-checkbox> -->
        <el-switch v-model="queryParams.checked" />
      </el-form-item>
      <el-form-item label="频率" prop="refresh">
        <el-select
          v-model="queryParams.refresh"
          placeholder="请选择"
          @change="handleRefreshSelect"
          style="width: 150px"
        >
          <el-option :label="'3秒'" :value="3" />
          <el-option :label="'5秒'" :value="5" />
          <el-option :label="'10秒'" :value="10" />
        </el-select>
      </el-form-item>
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
          plain
          type="primary"
          icon="el-icon-check"
          @click="handleCommitAll"
          :disabled="hasPermi(['produce:tasks:commit'])"
          >批量提交</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          plain
          type="warning"
          icon="el-icon-refresh-left"
          @click="handleRevokeAll"
          :disabled="hasPermi(['produce:tasks:revoke'])"
        >
          批量撤销
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          plain
          type="warning"
          icon="el-icon-video-play"
          @click="handleStartAll"
          :disabled="hasPermi(['produce:tasks:start'])"
        >
          批量开始
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          plain
          type="warning"
          icon="el-icon-circle-check"
          @click="handleFinishAll"
          :disabled="hasPermi(['produce:tasks:finish'])"
        >
          批量完成
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-share"
          @click="handleTransferTask"
          :disabled="hasPermi(['produce:tasks:transferTask'])"
          >批量转发</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDeleteAll"
          :disabled="hasPermi(['produce:tasks:remove'])"
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
      :data="taskList"
      :ref="page"
      @select="handleSelect"
      @selection-change="handleSelectionChange"
      @select-all="handleSelectAll"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />

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
        ><template slot-scope="scope">
          <el-tooltip effect="dark" placement="bottom" content="点击查看库存">
            <span
              class="one-line-red"
              @click="openItemSearchDialog(scope.row.itemCode)"
              >{{ scope.row.itemCode }}</span
            >
          </el-tooltip>
        </template></el-table-column
      >
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

      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            icon="el-icon-video-play"
            @click="handleStart(scope.row)"
            :disabled="
              hasPermi(
                ['produce:tasks:start'] ||
                  (scope.row.taskStatus != 10 &&
                    scope.row.taskStatus != 20 &&
                    scope.row.taskStatus != 30)
              )
            "
            >开始</el-button
          >
          <el-button
            type="text"
            @click="handleFinish(scope.row)"
            icon="el-icon-circle-check"
            :disabled="
              hasPermi(['produce:tasks:finish']) || scope.row.taskStatus != 40
            "
            >完成</el-button
          >
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
    <TransferTask
      ref="TransferTask"
      :tasks="multipleSelection"
      @onSelected="onTransferTaskSelected"
    />
    <ItemSearchDialog ref="ItemSearchDialog" />
  </div>
</template>

<script>
import {
  updateByOutSide,
  updateListByOutSide,
  listTask,
  delTask,
  delList,
  commitTask,
  revokeCommit,
  verifyTaskBelongOneRouteAndProcess,
  transferTask,
} from "@/api/produce/task";
import { getDropSelectDatas as getProcessSelectDatas } from "@/api/produce/process";

import TransferTask from "./transferTask.vue";
import { getDropSelectDatas } from "@/api/produce/route";
export default {
  name: "Tasks",
  components: {
    TransferTask,
  },
  data() {
    return {
      page: "tasks",
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
      queryParams: this.$cache.local.get("tasks_queryParams")
        ? JSON.parse(this.$cache.local.get("tasks_queryParams"))
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
            refresh: 10,
            checked: false,
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
      // 定时刷新数据
      timer: null,
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
    // 拿出要监听的属性
    listenChange() {
      const { refresh, checked } = this.queryParams;
      return { refresh, checked };
    },
    queryParamsChange() {
      return { ...this.queryParams };
    },
  },
  watch: {
    queryParamsChange: {
      handler(val) {
        this.$cache.local.set("tasks_queryParams", JSON.stringify({ ...val }));
      },
      deepL: true,
      immediate: true,
    },
    listenChange: {
      // 开启深度监听
      handler(val, old) {
        const { refresh, checked } = val;
        if (checked) {
          if (this.timer != null) {
            clearInterval(this.timer);
            this.timer = null;
          } else {
            this.changeSetInterval(refresh);
          }
        } else {
          clearInterval(this.timer);
          this.timer = null;
        }
      },
      immediate: true,
      deep: true,
    },
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  // keep-alive 特有钩子函数 关闭定时器
  deactivated() {
    clearInterval(this.timer);
    this.timer = null;
    this.queryParams.checked = false;
  },
  methods: {
    // 定时器
    changeSetInterval(refresh) {
      this.timer = setInterval(() => {
        setTimeout(() => {
          this.input_trim();
          this.handleQuery(); //调用接口的方法
        }, 0);
      }, refresh * 1000);
    },
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
    // 去空格
    input_trim() {
      this.queryParams.code = this.queryParams.code?.trim();
      this.queryParams.name = this.queryParams.name?.trim();
      this.queryParams.workOrderCode = this.queryParams.workOrderCode?.trim();
      this.queryParams.itemCode = this.queryParams.itemCode?.trim();
      this.queryParams.itemTypeId = this.queryParams.itemTypeId?.trim();
      this.queryParams.batchCode = this.queryParams.batchCode?.trim();
      this.queryParams.workOrderId = this.queryParams.workOrderId?.trim();
      this.queryParams.processId = this.queryParams.processId?.trim();
      this.queryParams.processCode = this.queryParams.processCode?.trim();
      this.queryParams.workStationCode =
        this.queryParams.workStationCode?.trim();
    },
    /** 查询任务列表 */
    getList(isSearch) {
      this.getProcessList();
      this.getRouteList();
      this.loading = true;
      listTask(this.queryParams).then((res) => {
        this.taskList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      if (this.timer) {
        this.$modal.msgWarning("警告,定时刷新已关闭,稍后请手动开启!");
      }
      // 点击选中时先取消定时刷新
      this.queryParams.checked = false;
      clearInterval(this.timer);
      this.timer = null;
      this.handleDoubleClick(row, this, column);
    },
    // 勾选表格checkbox清除定时器
    handleSelect(selection, row) {
      if (this.timer) {
        this.$modal.msgWarning("警告,定时刷新已关闭,稍后请手动开启!");
      }
      // 点击选中时先取消定时刷新
      this.queryParams.checked = false;
      clearInterval(this.timer);
      this.timer = null;
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.multipleSelection = selection;
      this.ids = selection.map((item) => item.id);
    },
    // 监听全选表格事件
    handleSelectAll() {
      if (this.timer) {
        this.$modal.msgWarning("警告,定时刷新已关闭,稍后请手动开启!");
      }
      // 点击选中时先取消定时刷新
      this.queryParams.checked = false;
      clearInterval(this.timer);
      this.timer = null;
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
        refresh: 10,
        checked: false,
        taskStatusList: [],
        isUrgent: undefined,
        queryOrderBy: 4,
        routeCodes: [],
        isAuto: undefined,
      };
      this.handleQuery();
    },
    // 切换刷新频率
    handleRefreshSelect(val) {
      clearInterval(this.timer);
      this.timer = null;
    },
    // 解决v-model不回显 监听不到变化
    handleInput(val, query) {
      this.queryParams[query] = val;
      this.$forceUpdate();
    },

    // 多选任务状态
    handleSelectStatusList(val) {
      this.$cache.local.setJSON(
        "task_statusList",
        JSON.stringify(Array.isArray(val) ? val : [val])
      );
    },
    // 单选任务状态
    handleSelectStatus(val) {
      this.$cache.local.setJSON("task_Status", val);
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
    // 点击产品编码打开弹框
    openItemSearchDialog(code) {
      this.$refs.ItemSearchDialog.open = true;
      this.$refs.ItemSearchDialog.title = `查看库存--(${code})`;
      this.$refs.ItemSearchDialog.queryParams.itemCode = code;
      this.$refs.ItemSearchDialog.getList();
    },
    // confirm确认框
    async handleConfirm(label, taskStatus, api, apiParams) {
      if (taskStatus != undefined) {
        if (this.ids.length <= 0)
          return this.$modal.msgWarning("请选择要操作的数据!");
        let falg;
        if (Array.isArray(taskStatus)) {
          falg = this.multipleSelection.some((item) => {
            return (
              item.taskStatus != taskStatus[0] &&
              item.taskStatus != taskStatus[1] &&
              item.taskStatus != taskStatus[2]
            );
          });
        } else {
          falg = this.multipleSelection.some(
            (item) => item.taskStatus != taskStatus
          );
        }
        if (falg)
          return this.$modal.notifyError(
            "当前的数据不能执行" + label + "操作!"
          );
        const results = await this.$modal
          .confirm("确认执行" + label + "？")
          .catch(() => {});
        if (results == "confirm") {
          api(apiParams).then((res) => {
            if (res.code == 0) {
              this.getList();
              this.$modal.msgSuccess("操作成功");
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }
      } else {
        const results = await this.$modal
          .confirm("确认执行" + label + "？")
          .catch(() => {});
        if (results == "confirm") {
          api(apiParams).then((res) => {
            if (res.code == 0) {
              this.getList();
              this.$modal.msgSuccess("操作成功");
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }
      }
    },
    // 点击完成
    handleFinish(row) {
      const params = {
        code: row.code,
        taskStatus: 50,
      };
      this.handleConfirm("完成", undefined, updateByOutSide, params);
    },
    // 批量完成
    handleFinishAll() {
      const updateTasks = this.multipleSelection.map((v) => {
        return {
          code: v.code,
          nowWadCount: 0,
        };
      });
      const params = {
        updateTasks,
        taskStatus: 50,
      };
      this.handleConfirm("完成", 40, updateListByOutSide, params);
    },

    // 点击开始
    handleStart(row) {
      const params = {
        code: row.code,
        taskStatus: 40,
      };
      this.handleConfirm("开始", undefined, updateByOutSide, params);
    },
    // 批量开始
    handleStartAll() {
      const updateTasks = this.multipleSelection.map((v) => {
        return {
          code: v.code,
          nowWadCount: 0,
        };
      });
      const params = {
        updateTasks,
        taskStatus: 40,
      };
      this.handleConfirm("开始", [10, 20, 30], updateListByOutSide, params);
    },
    // 点击提交按钮
    handleCommitAll() {
      const params = {
        ids: this.ids,
        taskStatus: 10,
      };
      this.handleConfirm("提交", 0, commitTask, params);
    },
    // 点击撤销提交
    handleRevokeAll() {
      const params = {
        ids: this.ids,
        taskStatus: 0,
      };
      this.handleConfirm("撤销", 10, revokeCommit, params);
    },
    // 点击批量转发
    handleTransferTask() {
      if (this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      // 是否是草稿数据
      const falg = this.multipleSelection.some((item) => item.taskStatus != 0);
      if (falg) return this.$modal.notifyError("当前操作仅草稿数据可执行!");
      verifyTaskBelongOneRouteAndProcess(this.multipleSelection).then((res) => {
        if (res.code == 0) {
          this.$refs.TransferTask.showFlag = true;
          this.$refs.TransferTask.queryParams.routeCode = res.data.routeCode;
          this.$refs.TransferTask.queryParams.processCode =
            res.data.processCode;
          this.$refs.TransferTask.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 执行转发
    onTransferTaskSelected(row) {
      const form = {
        taskIds: this.ids,
        workStationId: row.id,
        workStationName: row.name,
        workStationCode: row.code,
      };
      transferTask(form).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("操作成功");
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 删除
    handleDeleteAll() {
      const falg = this.multipleSelection.some((item) => item.taskStatus != 0);
      if (falg) return this.$modal.notifyError("当前的数据不能执行删除操作!");
      this.deleteItem(this.ids, delList, this.getList);
    },
  },
};
</script> 