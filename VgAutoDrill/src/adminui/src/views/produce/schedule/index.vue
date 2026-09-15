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
          @change="handleQueryOrderBy"
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
      <el-form-item label="工单编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入工单编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="来源类型" prop="orderSource">
        <el-select
          v-model="queryParams.orderSource"
          placeholder="请选择"
          clearable
          @clear="clearQueryParams('orderSource')"
          style="width: 150px"
        >
          <el-option :value="'客户订单'">客户订单</el-option>
          <el-option :value="'库存需求'">库存需求</el-option>
        </el-select>
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

      <el-form-item label="客户名称" prop="clientName">
        <el-input
          v-trim
          v-model="queryParams.clientName"
          placeholder="请输入客户名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <!-- 单选工单状态 -->
      <el-form-item label="工单状态" prop="manuOrderStatusList">
        <el-select
          v-model="queryParams.manuOrderStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          @change="handleselectManuOrderStatusList"
          :class="
            queryParams.manuOrderStatusList &&
            queryParams.manuOrderStatusList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.workOrderOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <!-- 单选工单状态 -->
      <!-- <el-form-item label="工单状态" prop="manuOrderStatus">
        <el-select
          v-model="queryParams.manuOrderStatus"
          placeholder="请选择"
          @change="handleselectManuOrderStatus"
          style="width: 150px"
        >
          <el-option
            v-for="item in $status.workOrderOptions"
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
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option label="是" :value="1" />
          <el-option label="否" :value="0" />
        </el-select>
      </el-form-item>
      <el-form-item label="是否外部工单" prop="isExternal">
        <el-select
          v-model="queryParams.isExternal"
          placeholder="请选择"
          clearable
          @clear="clearQueryParams('isExternal')"
          style="width: 150px"
        >
          <el-option label="是" :value="1" />
          <el-option label="否" :value="0" />
        </el-select>
      </el-form-item>
      <el-form-item label="层数" prop="layerNumList">
        <el-select
          v-model="queryParams.layerNumList"
          placeholder="请选择"
          multiple
          collapse-tags
          style="width: 150px"
        >
          <el-option
            v-for="item in layerNumOptions"
            :key="item.value"
            :label="item.label + '层'"
            :value="item.value"
          />
        </el-select>
      </el-form-item>

      <el-form-item label="需求日期" prop="requestDate">
        <el-date-picker
          clearable
          v-model="queryParams.requestDate"
          type="date"
          value-format="yyyy-MM-dd"
          placeholder="请选择需求日期"
          style="width: 180px"
        >
        </el-date-picker>
      </el-form-item>
    </search-form>
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="success"
          plain
          icon="el-icon-edit"
          @click="handleOpenGantt"
          :disabled="hasPermi(['produce:schedule:editGantt'])"
          v-if="scheduleList.length > 0"
          >使用甘特图编辑</el-button
        >
      </el-col>
      <right-toolbar
        :showSearch.sync="showSearch"
        @queryTable="getList"
        :columns="columns"
        :page="page"
      ></right-toolbar>
    </el-row>
    <!-- <div class="wrapper" v-if="scheduleList.length > 0">
        <div class="container">
          <GanttChar
            class="left-container"
            ref="ganttChar"
            :tasks="tasks"
            :optType="optType"
            @getList="getGanttTasks"
          ></GanttChar>
        </div>
      </div> -->

    <el-table
      border
      :ref="page"
      v-loading="loading"
      :data="scheduleList"
      @selection-change="handleSelectionChange"
      :style="{ margin: '20px 0 0 0' }"
    >
      <el-table-column
        label="工单编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        fixed="left"
        min-width="200px"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['produce:workorder:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>

      <el-table-column
        label="产品编码"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        min-width="200px"
        v-if="columns[1].visible"
      />
      <el-table-column
        label="工艺路线"
        key="routeCode"
        prop="routeCode"
        show-overflow-tooltip
        min-width="100px"
        v-if="columns[2].visible"
      />
      <el-table-column
        label="是否紧急"
        align="center"
        key="isUrgent"
        prop="isUrgent"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isUrgent == 1" type="danger"> 是 </el-tag>
          <el-tag v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="状态"
        align="center"
        key="manuOrderStatus"
        prop="manuOrderStatus"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <status-tag
            :options="$status.workOrderOptions"
            :status="scope.row.manuOrderStatus"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="层数"
        align="center"
        key="layerNum"
        prop="layerNum"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />

      <el-table-column
        label="是否外部工单"
        align="center"
        key="isExternal"
        prop="isExternal"
        v-if="columns[6].visible"
        min-width="120"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.isExternal == 1" type="danger"> 是 </el-tag>
          <el-tag v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="标记颜色"
        align="center"
        key="color"
        prop="color"
        v-if="columns[7].visible"
      >
        <template slot-scope="scope">
          <div
            :style="{
              margin: '0 auto',
              width: '20px',
              height: '20px',
              background: scope.row.remarkColor
                ? scope.row.remarkColor
                : workOrderColor,
            }"
          ></div>
        </template>
      </el-table-column>

      <!-- <el-table-column
        label="批次号"
        align="center"
        prop="batchCode"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[6].visible"
      />
      <el-table-column
        label="规格型号"
        align="center"
        prop="specification"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[7].visible"
      /> -->
      <el-table-column
        label="单位"
        align="center"
        key="unitOfMeasure"
        prop="unitOfMeasure"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[8].visible"
      />
      <el-table-column
        label="调整数量"
        align="center"
        key="quantityChanged"
        prop="quantityChanged"
        v-if="columns[9].visible"
      />
      <el-table-column
        label="已排产"
        align="center"
        key="quantityScheduled"
        prop="quantityScheduled"
        v-if="columns[10].visible"
      />
      <el-table-column
        label="已生产"
        align="center"
        key="quantityProduced"
        prop="quantityProduced"
        v-if="columns[11].visible"
      />

      <el-table-column
        label="客户名称"
        key="clientName"
        prop="clientName"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[12].visible"
      />
      <el-table-column
        key="requestDate"
        label="需求日期"
        align="center"
        width="100"
        v-if="columns[13].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.requestDate, "{y}-{m}-{d}") }}</span>
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
            icon="el-icon-edit"
            @click="handleUpdate(scope.row)"
            :disabled="
              hasPermi(['produce:schedule:edit']) ||
              scope.row.manuOrderStatus == 0 ||
              scope.row.manuOrderStatus == 4
            "
            >排产</el-button
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

    <!-- 添加或修改生产工单对话框 -->
    <edit-form-dialog
      v-model="open"
      title="排产"
      optType="view"
      @submitForm="() => {}"
    >
      <el-form ref="form" :model="form" disabled label-width="100px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="工单编码" prop="code">
              <el-input v-model="form.code" placeholder="请输入工单编码" />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="工单名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入工单名称" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="需求日期" prop="requestDate">
              <el-input
                v-model="form.requestDate"
                placeholder="请输入需求日期"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="来源类型" prop="orderSource">
              <el-input
                v-model="form.orderSource"
                placeholder="请输入来源类型"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="来源单据" prop="sourceCode">
              <el-input
                v-model="form.sourceCode"
                placeholder="请输入来源单据"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="是否紧急" prop="a">
              <el-radio-group v-removeAriaHidden v-model="form.isUrgent">
                <el-radio :label="1">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="产品编码" prop="itemCode">
              <el-input v-model="form.itemCode" placeholder="请输入产品编码" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="工艺路线" prop="route">
              <el-input
                v-model="form.route"
                :placeholder="form.routeId != null ? form.route : '暂未配置'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="层数" prop="layerNum">
              <!-- 引入自定义计数器组件 -->
              <el-input v-model="form.layerNum" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="产品名称" prop="itemName">
              <el-input
                v-model="form.itemName"
                placeholder="请选择产品"
                disabled
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="单位" prop="unitOfMeasure">
              <el-input
                v-model="form.unitOfMeasure"
                placeholder="请选择产品"
                disabled
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="规格型号" prop="specification">
              <el-input
                disabled
                v-model="form.specification"
                placeholder="请选择产品"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="批次号" prop="batchCode">
              <el-input v-model="form.batchCode" placeholder="请输入批次号" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="SpecGroup" prop="specGroup">
              <el-input
                v-model="form.specGroup"
                placeholder="请输入SpecGroup"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="待排产叠数" prop="wadCount">
              <el-input v-model="form.wadCount" disabled />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="孔数" prop="drillCount">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.drillCount"
                @changeNum="changeNum"
                :dis="optType == 'view'"
                :numName="'drillCount'"
                :min="0"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="叠数" prop="panelCount">
              <el-input
                disabled
                v-model="form.panelCount"
                placeholder="请选择产品"
              />
            </el-form-item> </el-col
          ><el-col :span="8">
            <el-form-item label="Move In" prop="moveInTime">
              <el-input v-model="form.moveInTime" disabled />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="Move Out" prop="moveOutTime">
              <el-input v-model="form.moveOutTime" disabled />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="Track In" prop="trackInTime">
              <el-input v-model="form.trackInTime" disabled />
            </el-form-item> </el-col
          ><el-col :span="8">
            <el-form-item label="Track Out" prop="trackOutTime">
              <el-input v-model="form.trackOutTime" disabled />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="生产数量" prop="quantity">
              <!-- 引入自定义计数器组件 -->
              <el-input v-model="form.quantity" placeholder="请输入生产数量" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="调整数量" prop="quantityChanged">
              <el-input
                v-model="form.quantityChanged"
                placeholder="请输入调整数量"
              />
            </el-form-item> </el-col
          ><el-col :span="8">
            <el-form-item label="标记颜色" prop="remarkColor">
              <div :style="{ display: 'flex' }">
                <div
                  :style="{
                    background: form.remarkColor,
                    marginRight: '3px',
                  }"
                  :class="$store.getters.size + '-remarkColor'"
                ></div>
                <el-button disabled icon="el-icon-search"></el-button>
              </div>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item label="客户编码" prop="clientCode">
              <el-input v-model="form.clientCode" placeholder="请输入客户编码">
              </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="客户名称" prop="clientName">
              <el-input
                v-model="form.clientName"
                placeholder="请输入客户名称"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="转换前路径" prop="beforeDrillFilePath">
              <el-input
                v-model="form.beforeDrillFilePath"
                type="textarea"
                placeholder="请输入文件路径"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <el-steps
        :active="activeProcess"
        v-if="form.id != null && processOptions.length > 0"
        align-center
        simple
      >
        <el-step
          v-for="(item, index) in processOptions"
          :title="item.processName"
          :key="item.id"
          @click.native="handleStepClick(index, item)"
        ></el-step>
      </el-steps>
      <div v-for="(item, index) in processOptions" :key="index">
        <ProTask
          v-if="activeProcess == index && form.id != null && open"
          @father_getGanttTasks="getGanttTasks"
          @father_getList="getList"
          @handleUpdate="handleUpdate(form, item.processId)"
          :workOrderForm="form"
          :processForm="item"
          :taskOptType="optType"
        >
        </ProTask>
      </div>
    </edit-form-dialog>
  </div>
</template>

<script>
import { listRouteAndProcess } from "@/api/produce/routeAndProcess";
import { getGanttTaskList, listAllProcess } from "@/api/produce/task";
import { listWorkOrder, getWorkOrder } from "@/api/produce/workOrder";
import GanttChar from "@/components/ganttChar";
import ProTask from "./proTask";

export default {
  name: "Schedule",
  components: {
    GanttChar,
    ProTask,
  },
  data() {
    return {
      page: "schedule",
      //自动生成编码
      autoGenFlag: false,
      optType: undefined,
      // 步骤条所需
      itemId: undefined,
      // 步骤条
      activeProcess: 0,
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 生产工单表格数据
      scheduleList: [],
      //当前生产工单中产品对应的工序列表
      processOptions: [],
      // 工艺路线数据
      routeList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,

      total: 0,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        sourceCode: undefined,
        itemId: undefined,
        itemCode: undefined,
        itemName: undefined,
        itemTypeId: undefined,
        batchCode: undefined,
        status: undefined,
        code: undefined,
        name: undefined,
        orderSource: undefined,
        clientName: undefined,
        clientCode: undefined,
        requestDate: undefined,
        manuOrderStatusList: [],
        queryOrderBy: 4,
      },
      // 甘特图
      GanttParams: {
        pageNum: 1,
        pageSize: 10,
      },
      tasks: {
        data: [],
        links: [],
      },
      // 表单参数
      form: {},
      processForm: undefined,
      workOrderForm: undefined,
      // 工艺路线名称
      routeName: "",
      routeId: undefined,
      // 列信息
      columns: [
        { key: 0, label: "工单编码", visible: true },
        { key: 1, label: "产品编码", visible: true },
        { key: 2, label: "工艺路线", visible: true },
        { key: 3, label: "是否紧急", visible: true },
        { key: 4, label: "状态", visible: true },
        { key: 5, label: "层数", visible: true },
        { key: 6, label: "是否外部工单", visible: true },
        { key: 7, label: "标记颜色", visible: true },
        // { key: 6, label: "批次号", visible: true },
        // { key: 7, label: "规格型号", visible: true },
        { key: 8, label: "单位", visible: true },
        { key: 9, label: "调整数量", visible: true },
        { key: 10, label: "已排产", visible: true },
        { key: 11, label: "已生产", visible: true },
        { key: 12, label: "客户名称", visible: true },
        { key: 13, label: "需求日期", visible: true },
      ],
      //标记颜色默认色
      workOrderColor: "#ff5722",
      layerNumOptions: Array.from({ length: 12 }, (v, i) => {
        return { label: i + 1, value: i + 1 };
      }),
    };
  },
  activated() {
    this.getList();
    this.getGanttTasks();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  methods: {
    /** 查询生产工单列表 */
    async getList(isSearch) {
      const local_arr = this.$cache.local.getJSON(
        "schedule_workOrder_statusList"
      )
        ? JSON.parse(this.$cache.local.getJSON("schedule_workOrder_statusList"))
        : [];
      this.queryParams.manuOrderStatusList = Array.isArray(local_arr)
        ? local_arr
        : [local_arr];

      this.queryParams.queryOrderBy =
        this.$cache.local.get("schedule_QueryOrderBy") != undefined &&
        this.$cache.local.get("schedule_QueryOrderBy") != "0"
          ? this.$cache.local.get("schedule_QueryOrderBy") * 1
          : 4;
      this.loading = true;
      const res = await listWorkOrder(this.queryParams);
      this.scheduleList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 获取对应工序
    getAllProcess(val) {
      listRouteAndProcess({
        routeId: val,
      }).then((res) => {
        this.processOptions = res.data.list.filter(
          (v) => v.processName != "钻孔"
        );
        this.processForm = {
          ...this.processOptions[0],
        };
      });
    },
    // 跳转到甘特图编辑
    handleOpenGantt() {
      this.$router.push({ path: "/produce/gantt/ganttedit" });
    },
    // 查询任务列表
    async getGanttTasks() {
      const res = await getGanttTaskList(this.GanttParams);
      this.tasks.data = res;
      this.$refs.ganttChar && this.$refs.ganttChar.reload();
    },
    // 表单重置
    reset() {
      this.form = {
        workOrderId: undefined,
        workOrderCode: undefined,
        workOrderName: undefined,
        orderSource: undefined,
        sourceCode: undefined,
        productId: undefined,
        itemCode: undefined,
        itemName: undefined,
        productSpc: undefined,
        unitOfMeasure: undefined,
        quantity: undefined,
        quantityProduced: undefined,
        quantityChanged: undefined,
        quantityScheduled: undefined,
        wadCount: undefined,
        clientId: undefined,
        clientCode: undefined,
        clientName: undefined,
        requestDate: undefined,
        parentId: undefined,
        status: undefined,
        remark: undefined,
        createBy: undefined,
        createTime: undefined,
        updateBy: undefined,
        updateTime: undefined,
        routeId: undefined,
      };
      this.activeProcess = 0;
      this.autoGenFlag = false;
      this.resetForm("form");
    },

    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.$cache.local.remove("schedule_workOrder_statusList");
      this.$cache.local.remove("schedule_QueryOrderBy");
      this.resetForm("queryForm");
      this.handleQuery();
    },

    // 多选工单状态
    handleselectManuOrderStatusList(val) {
      this.$cache.local.setJSON(
        "schedule_workOrder_statusList",
        JSON.stringify(Array.isArray(val) ? val : [val])
      );
    },
    // 单选工单状态
    handleselectManuOrderStatus(val) {
      this.$cache.local.setJSON("schedule_workOrder_status", val);
    },

    // 监听排序方式选择
    handleQueryOrderBy(val) {
      this.$cache.local.set("schedule_QueryOrderBy", val);
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
    },
    //Step点击
    handleStepClick(index, item) {
      this.activeProcess = index;
    },
    /** 点击排产按钮操作 */
    handleUpdate(row, processId) {
      this.optType = "edit";
      const id = row.id || this.ids;
      this.reset();
      getWorkOrder(id).then((res) => {
        if (res.code == 0) {
          if (res.data.routeId == null)
            return this.$modal.msgError("请前往工单页面进行配置工艺路线!");
          this.form = { ...res.data };
          this.getAllProcess(res.data.routeId);
          this.open = true;
          this.saveOneProcess(row, processId);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },

    // 默认保存第一个工序
    saveOneProcess(row, processId) {
      const index = this.processOptions.findIndex(
        (v) => v.processId == processId
      );
      index != -1 ? (this.activeProcess = index) : 0;
      // 默认保存第一个工序
      this.processForm = {
        ...this.processOptions[0],
        processName: row.name,
        processId: row.id,
        processCode: row.code,
      };
    },
    // 点击编码查看
    handleView(id) {
      this.optType = "view";
      this.reset();
      getWorkOrder(id).then((res) => {
        if (res.code == 0) {
          if (res.data.routeId == null) {
            this.processOptions = [];
          } else {
            this.getAllProcess(res.data.routeId);
          }
          this.form = { ...res.data };

          this.open = true;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
  },

  beforeDestroy() {
    this.$refs.ganttChar && this.$refs.ganttChar.closeGantt();
  },
};
</script>
<style scoped lang="scss">
.wrapper {
  height: 200px;
}
.container {
  height: 100%;
  width: 100%;
}
.left-container {
  overflow: hidden;
  position: relative;
  height: 100%;
}
</style>
