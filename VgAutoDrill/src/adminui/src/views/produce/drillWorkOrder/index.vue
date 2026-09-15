<template>
  <div class="app-container">
    <div :class="!isCollapse ? 'left' : 'left active'">
      <el-button
        type="text"
        icon="el-icon-close"
        class="closebtn"
        @click="closeLeft"
      ></el-button>
      <el-card class="box-card">
        <div class="left_info">
          <ul class="info">
            <li>
              <p>工单编码:</p>
              <span>{{ infoForm.workOrderCode }}</span>
            </li>
            <li>
              <p>产品编码:</p>
              <span>{{ infoForm.itemCode }}</span>
            </li>
            <li>
              <p>库存统计时间:</p>
              <span>{{ parseTime(infoForm.createTime) }}</span>
            </li>
            <li>
              <p>物料当前可用库存:</p>
              <span>{{ infoForm.sumOnhand }}</span>
            </li>
            <li>
              <p>待制任务占用:</p>
              <span>{{ infoForm.sumPlan }}</span>
            </li>
            <li>
              <p>预计可用量:</p>
              <span>{{ infoForm.usableCount }}</span>
            </li>
          </ul>
          <ul class="wad_count">
            <li>
              <p>已钻孔</p>
              <span>xx叠</span>
            </li>
            <li>
              <p>正在钻孔</p>
              <span>xx叠</span>
            </li>
            <li>
              <p>待制</p>
              <span>xx叠</span>
            </li>
            <li>
              <p>未安排</p>
              <span>xx叠</span>
            </li>
          </ul>

          <el-tabs type="card" @tab-click="tabClick">
            <el-tab-pane label="分配机台">
              <TabMachineTable
                ref="TabMachineTable"
                :workOrderCode="form.workOrderCode"
                :workOrderId="form.workOrderId"
                :routeCode="form.route"
              />
            </el-tab-pane>
            <el-tab-pane label="物料库存信息">
              <StoragesTable ref="storages"
            /></el-tab-pane>
            <el-tab-pane label="待制任务信息"
              ><ProduceTasksTable ref="produceTasks"
            /></el-tab-pane>
          </el-tabs>
        </div>
      </el-card>
    </div>
    <div :class="!isCollapse ? 'right' : 'right active'">
      <search-form
        v-show="showSearch"
        :form="queryParams"
        @search="handleQuery"
        @reset="resetQuery"
      >
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
      </search-form>
      <el-row :gutter="10" class="mb8">
        <el-col :span="1.5">
          <el-button
            v-debounce
            type="primary"
            plain
            icon="el-icon-plus"
            @click="handleAdd"
            :disabled="hasPermi(['produce:drillWorkOrder:add'])"
            >新增</el-button
          >
        </el-col>
        <el-col :span="1.5">
          <el-button
            v-debounce
            type="success"
            plain
            icon="el-icon-edit"
            @click="handleOpenDrillTask"
            :disabled="hasPermi(['produce:drillWorkOrder:editDrillTask'])"
            >使用图表编辑</el-button
          >
        </el-col>
        <el-col :span="1.5">
          <el-button
            v-debounce
            type="warning"
            plain
            icon="el-icon-edit-outline"
            @click="handleRecalculate"
            :disabled="hasPermi(['produce:drillWorkOrder:recalculate'])"
            >重新计算库存</el-button
          >
          <el-button
            v-debounce
            type="danger"
            plain
            @click="handleReset"
            icon="el-icon-refresh"
            :disabled="
              hasPermi(['produce:drillWorkOrder:resetTask']) || ids.length <= 0
            "
          >
            批量重置
          </el-button>
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
        :ref="page"
        v-loading="loading"
        :data="drillWorkOrderList"
        :max-height="computeHeight"
        @selection-change="handleSelectionChange"
        @row-dblclick="rowDblclick"
        :row-style="rowStyle"
      >
        <el-table-column type="selection" width="55" align="center" />

        <el-table-column
          fixed="left"
          label="工单编码"
          min-width="200"
          key="workOrderCode"
          prop="workOrderCode"
          show-overflow-tooltip
          v-if="columns[0].visible"
        />

        <el-table-column
          label="产品编码"
          min-width="200"
          prop="itemCode"
          show-overflow-tooltip
          v-if="columns[1].visible"
        />

        <el-table-column
          label="数量"
          align="center"
          key="quantity"
          prop="quantity"
          v-if="columns[2].visible"
        />

        <el-table-column
          label="叠板层数"
          align="center"
          key="panelCount"
          prop="panelCount"
          v-if="columns[3].visible"
        />

        <el-table-column
          label="计划叠数"
          align="center"
          key="wadCount"
          prop="wadCount"
          v-if="columns[4].visible"
        />

        <el-table-column
          label="轴数"
          align="center"
          key="shaftCount"
          prop="shaftCount"
          v-if="columns[5].visible"
        />

        <el-table-column
          label="孔数"
          align="center"
          key="drillCount"
          prop="drillCount"
          v-if="columns[6].visible"
        />
        <el-table-column
          label="总趟数"
          align="center"
          key="allPassesCount"
          prop="allPassesCount"
          v-if="columns[7].visible"
        />

        <el-table-column
          label="已排趟数"
          align="center"
          key="remainderPassesCount"
          prop="remainderPassesCount"
          show-overflow-tooltip
          v-if="columns[8].visible"
        />

        <el-table-column
          label="单趟耗时(分钟)"
          align="center"
          key="singleTripTime"
          prop="singleTripTime"
          min-width="150"
          v-if="columns[9].visible"
        />

        <el-table-column
          label="总耗时(分钟)"
          align="center"
          key="drillAllTime"
          prop="drillAllTime"
          min-width="150"
          show-overflow-tooltip
          v-if="columns[10].visible"
        />

        <el-table-column
          label="建议机台数"
          align="center"
          key="dispenseMachines"
          prop="dispenseMachines"
          v-if="columns[11].visible"
          min-width="150"
        >
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
              v-if="!scope.row.inpShow"
              type="text"
              icon="el-icon-document-checked"
              @click="handleUpdate(scope.row)"
              :disabled="hasPermi(['produce:drillWorkOrder:openLeft'])"
              >选中</el-button
            >

            <el-dropdown
              trigger="click"
              @command="(command) => handleCommand(command, scope.row)"
            >
              <span :class="'el-dropdown-link ' + $store.getters.size">
                <i class="el-icon-d-arrow-right el-icon--right"></i>更多
              </span>
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item
                  command="handleCommit"
                  icon="el-icon-finished"
                  :disabled="
                    hasPermi(['produce:drillWorkOrder:commit']) ||
                    scope.row.isSubmited == 1
                  "
                  >{{
                    scope.row.isSubmited == 1 ? "已提交" : "提交"
                  }}</el-dropdown-item
                >
                <el-dropdown-item
                  command="handleReset"
                  icon="el-icon-refresh"
                  :disabled="hasPermi(['produce:drillWorkOrder:resetTask'])"
                  >重置</el-dropdown-item
                >
                <el-dropdown-item
                  command="handleAgain"
                  icon="el-icon-film"
                  :disabled="hasPermi(['produce:drillWorkOrder:again'])"
                  >再次生成</el-dropdown-item
                >
              </el-dropdown-menu>
            </el-dropdown>
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
      <!-- 添加或修改钻孔工单对话框 -->
      <edit-form-dialog
        v-model="open"
        :title="title"
        :optType="optType"
        :isFormModified="isFormModified"
        @submitForm="submitForm"
      >
        <el-form ref="form" :model="form" :rules="rules" label-width="100px">
          <el-row>
            <el-col :span="8">
              <el-form-item label="生产工单" prop="workOrderCode">
                <el-input
                  v-model="form.workOrderCode"
                  placeholder="请选择生产工单"
                >
                  <el-button
                    v-debounce
                    slot="append"
                    icon="el-icon-search"
                    @click="handleWorkOrderSelect"
                  ></el-button>
                </el-input>
              </el-form-item>

              <WorkOrderSelect
                ref="woSelect"
                :isAdd="true"
                @onSelected="onWorkOrderSelected"
              ></WorkOrderSelect>
            </el-col>

            <el-col :span="8">
              <el-form-item label="数量" prop="quantity">
                <el-input
                  placeholder="请输入数量"
                  disabled
                  v-model="form.quantity"
                />
              </el-form-item>
            </el-col>

            <el-col :span="8">
              <el-form-item label="叠板层数" prop="panelCount" class="">
                <el-input
                  placeholder="请输入叠板层数"
                  disabled
                  v-model="form.panelCount"
                />
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="8">
              <el-form-item
                label="计划叠数"
                prop="wadCount"
                class="prompt_form_item"
              >
                <el-input
                  placeholder="请输入计划叠数"
                  disabled
                  v-model="form.wadCount"
                />

                <span class="prompt">计划叠数 = 数量 / 叠板层数</span>
              </el-form-item>
            </el-col>

            <el-col :span="8">
              <el-form-item label="轴数" prop="shaftCount">
                <!-- 引入自定义计数器组件 -->

                <input-number
                  :myNum="form.shaftCount"
                  @changeNum="changeNum"
                  :numName="'shaftCount'"
                  :min="1"
                />
              </el-form-item>
            </el-col>

            <!-- <el-col :span="8">
                <el-form-item label="可用叠数" prop="">
                  <input-number
                    :myNum="form.usableCount"
                    @changeNum="changeNum"
                    :numName="'usableCount'"
                    :min="0"
                  />
                </el-form-item>
              </el-col> -->
          </el-row>

          <el-row>
            <el-col :span="8">
              <el-form-item label="孔数" prop="drillCount">
                <!-- 引入自定义计数器组件 -->

                <input-number
                  :myNum="form.drillCount"
                  @changeNum="changeNum"
                  :numName="'drillCount'"
                  :min="10000"
                /> </el-form-item
            ></el-col>

            <el-col :span="8">
              <el-form-item label="总趟数" class="prompt_form_item">
                <el-input
                  placeholder="请输入总趟数"
                  disabled
                  v-model="form.allPassesCount"
                />

                <span class="prompt">总趟数 = 计划叠数 / 轴数</span>
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="8">
              <el-form-item
                label="总耗时"
                prop="drillAllTime"
                class="prompt_form_item"
              >
                <el-input
                  disabled
                  v-model="form.drillAllTime"
                  placeholder="请输入总耗时(分钟)"
                />

                <span class="prompt">总耗时 = 总趟数 * 单趟耗时</span>
              </el-form-item>
            </el-col>

            <el-col :span="8">
              <el-form-item label="单趟耗时" prop="singleTripTime">
                <el-input
                  v-model="form.singleTripTime"
                  disabled
                  placeholder="请输入单趟耗时(分钟)"
                /> </el-form-item
            ></el-col>

            <el-col :span="8">
              <el-form-item label="建议机台数" prop="dispenseMachines">
                <!-- 引入自定义计数器组件 -->

                <el-input v-model="form.dispenseMachines" disabled />
              </el-form-item>
            </el-col>
          </el-row>
        </el-form>

        <el-tabs type="border-card">
          <el-tab-pane label="分配机台">
            <FormMachineTable
              :workOrderId="form.workOrderId"
              :workOrderCode="form.workOrderCode"
              :routeCode="form.route"
              ref="FormMachineTable"
            />
          </el-tab-pane>
        </el-tabs>
      </edit-form-dialog>
    </div>
  </div>
</template>

<script>
import {
  listDrillWorkOrder,
  addDrillWorkOrder,
  drillTaskReset,
  drillWorkOrderCommit,
  rfreshStockData,
  getMaterialData,
  getDrillWorkOrder,
  bulkAddDrillTask,
} from "@/api/produce/drillWorkOrder";
// 工单选择
import WorkOrderSelect from "@/components/workOrderSelect";
// 表单中分配机台表格
import FormMachineTable from "./formMachineTable.vue";
// 左侧分配机台表格
import TabMachineTable from "./tabMachineTable.vue";
// 物料库存信息表格
import StoragesTable from "./storagesTable.vue";
// 待制作任务信息表格
import ProduceTasksTable from "./produceTasksTable.vue";
// 引入存储、获取方法
import {
  getIsCollapse,
  getItemCode,
  getWorkOrderCode,
  getWorkOrderId,
  getRouteCode,
  setIsCollapse,
  setItemCode,
  setWorkOrderCode,
  setWorkOrderId,
  setRouteCode,
} from "./getParams";
export default {
  name: "DrillWorkOrder", 
  components: {
    WorkOrderSelect,
    FormMachineTable,
    StoragesTable,
    ProduceTasksTable,
    TabMachineTable,
  },
  data() {
    return {
      page: "drillWorkOrder",
      optType: "",
      // 显示搜索条件
      showSearch: true,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 总条数
      total: 0,
      // 钻孔工单表格数据
      drillWorkOrderList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      isCollapse: getIsCollapse() == "true" ? true : false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        workOrderId: undefined,
        workOrderCode: undefined,
        itemId: undefined,
        itemCode: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        workOrderCode: [
          { required: true, message: "工单号不能为空", trigger: "change" },
        ],
        panelCount: [
          { required: true, message: "叠板层数不能为空", trigger: "change" },
        ],
        quantity: [
          { required: true, message: "数量不能为空", trigger: "change" },
        ],
        wadCount: [
          { required: true, message: "计划叠数不能为空", trigger: "change" },
        ],
        shaftCount: [
          { required: true, message: "轴数不能为空", trigger: "change" },
        ],
        allPassesCount: [
          { required: true, message: "总趟数不能为空", trigger: "change" },
        ],
        drillCount: [
          { required: true, message: "孔数不能为空", trigger: "change" },
        ],
        singleTrips: [
          { required: true, message: "单机趟数不能为空", trigger: "change" },
        ],
        dispenseMachines: [
          { required: true, message: "建议机台数不能为空", trigger: "change" },
        ],
      },
      infoForm: {},
      // 列信息
      columns: [
        { key: 0, label: "工单编码", visible: true },
        { key: 1, label: "产品编码", visible: true },
        { key: 2, label: "数量", visible: true },
        { key: 3, label: "叠板层数", visible: true },
        { key: 4, label: "计划叠数", visible: true },
        { key: 5, label: "轴数", visible: true },
        { key: 6, label: "孔数", visible: true },
        { key: 7, label: "总趟数", visible: true },
        { key: 8, label: "已排趟数", visible: false },
        { key: 9, label: "单趟耗时(分钟)", visible: true },
        { key: 10, label: "总耗时(分钟)", visible: true },
        { key: 11, label: "建议及台数", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    this.$nextTick(() => {
      if (this.isCollapse) {
        this.refreshList();
      }
    });
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    "form.workOrderCode": {
      handler(val) {
        if (!val) {
          this.form.workOrderId = undefined;
          this.form.workOrderCode = undefined;
          this.form.workOrderName = undefined;
          this.form.itemId = undefined;
          this.form.itemCode = undefined;
          this.form.itemName = undefined;
          this.form.itemTypeId = undefined;
          this.form.route = undefined;
          this.form.panelCount = 1;
          this.form.quantity = 1;
          this.form.wadCount = 1;
          this.form.shaftCount = 5;
          this.form.allPassesCount = 0;
          this.form.remainderPassesCount = 0;
          this.form.drillCount = 10000;
          this.form.singleTripTime = 40;
          this.form.drillAllTime = 0;
          this.form.singleTrips = 0;
          this.form.dispenseMachines = 2;
          this.form.usableCount = 0;
          this.form.specification = undefined;
          this.form.unitOfMeasure = undefined;
          this.addNumber();
        }
      },
      deep: true,
    },
  },
  computed: {
    // 高度自适应
    computeHeight() {
      let h = window.innerHeight - (this.isCollapse ? 320 : 270);
      return h;
    },
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },

  methods: {
    /** 查询钻孔工单列表 */
    getList(isSearch) {
      this.loading = true;
      listDrillWorkOrder(this.queryParams).then((res) => {
        this.drillWorkOrderList = res.data.list;
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
      this.handleDoubleClick(row, this, column);
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.multipleSelection = selection;
      this.ids = selection.map((item) => {
        return { workOrderCode: item.workOrderCode, itemCode: item.itemCode };
      });
    },
    // 表单重置
    reset() {
      this.form = {
        workOrderId: undefined,
        workOrderCode: undefined,
        itemId: undefined,
        itemCode: undefined,
        panelCount: 1,
        quantity: 1,
        wadCount: 1,
        shaftCount: 5,
        allPassesCount: 0,
        remainderPassesCount: 0,
        drillCount: 10000,
        singleTripTime: 40,
        drillAllTime: 0,
        singleTrips: 0,
        dispenseMachines: 2,
        usableCount: 0,
      };
      this.resetForm("form");
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

    // 跳转到任务拖拽页面
    handleOpenDrillTask() {
      this.$router.push({ path: "/produce/drillTask/taskedit" });
    },
    // 点击重新计算
    handleRecalculate() {
      const recalculateLoading = this.$loading({
        lock: true,
        text: "正在计算库存,请稍后",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.7)",
      });
      rfreshStockData().then((res) => {
        this.refreshList();
        recalculateLoading.close();
      });
    },
    // 计算数值
    addNumber() {
      if (this.form.quantity == 0 || this.form.panelCount == 0) {
        // 计划叠数计算
        this.form.wadCount = 0;
      } else {
        // 计划叠数计算
        this.form.wadCount = Math.ceil(
          this.form.quantity / this.form.panelCount
        );
      }
      if (this.form.wadCount == 0 || this.form.shaftCount == 0) {
        this.form.allPassesCount = 0;
      } else {
        // 总趟数计算
        this.form.allPassesCount = Math.ceil(
          this.form.wadCount / this.form.shaftCount
        );
      }

      // 单趟耗时计算
      this.form.singleTripTime = Math.ceil((this.form.drillCount / 10000) * 40);
      // 总耗时计算
      this.form.drillAllTime =
        this.form.allPassesCount * this.form.singleTripTime;
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.$nextTick(() => {
        this.$refs.FormMachineTable.workOrderAndWorkStation = [];
      });
      this.addNumber();
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加钻孔工单";
      this.optType = "add";
    },
    // 关闭左侧
    closeLeft() {
      this.isCollapse = false;
      setIsCollapse(false);
    },
    // 点击提交
    handleCommit(row) {
      drillWorkOrderCommit({
        workOrderCode: row.workOrderCode,
        processCode: "drill",
      }).then((res) => {
        if (res.code == 0) {
          row.isSubmited = 1;
          this.$modal.msgSuccess("提交成功");
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击tab标签页
    tabClick() {
      this.refreshList();
    },
    // 刷新左侧的表格数据
    refreshList(item) {
      const workOrderCode = item ? item.workOrderCode : getWorkOrderCode();
      const workOrderId = item ? item.workOrderId : getWorkOrderId();
      const routeCode = item ? item.routeCode : getRouteCode();
      const itemCode = item ? item.itemCode : getItemCode();
      this.$refs.storages.loading = true;
      this.$refs.produceTasks.loading = true;
      // 重新获取库存及待制任务信息
      getMaterialData({
        workOrderCode,
        itemCode,
      }).then((res) => {
        this.infoForm = {
          ...res.data.materialStockOverview,
          workOrderCode,
          itemCode,
        };
        this.$refs.TabMachineTable.queryParams.workOrderId = workOrderId * 1;
        this.$refs.TabMachineTable.queryParams.workOrderCode = workOrderCode;
        this.$refs.TabMachineTable.queryParams.routeCode = routeCode;

        this.$refs.storages.queryParams.workOrderCode = workOrderCode;
        this.$refs.storages.queryParams.itemCode = itemCode;
        this.$refs.produceTasks.queryParams.workOrderCode = workOrderCode;
        this.$refs.produceTasks.queryParams.itemCode = itemCode;
        // 获取物料库存信息
        this.$refs.storages.list = res.data.storages.list;
        this.$refs.storages.total = res.data.storages.total;
        // 获取待制任务信息
        this.$refs.produceTasks.list = res.data.produceTasks.list;
        this.$refs.produceTasks.total = res.data.produceTasks.total;
        this.$refs.storages.loading = false;
        this.$refs.produceTasks.loading = false;
        // 获取机台信息
        this.$refs.TabMachineTable.getList();
      });
    },
    // 点击选中
    handleUpdate(row) {
      getDrillWorkOrder(row.id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.isCollapse = true;
          setIsCollapse(true);
          setItemCode(res.data.itemCode);
          setWorkOrderCode(res.data.workOrderCode);
          setWorkOrderId(res.data.workOrderId);
          setRouteCode(res.data.route);

          this.refreshList({
            ...res.data,
            workOrderCode: this.form.workOrderCode,
            workOrderId: this.form.workOrderId,
            itemCode: this.form.itemCode,
          });
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击重置
    handleReset(row) {
      const resetReq =
        row.id != undefined
          ? [
              {
                workOrderCode: row.workOrderCode,
                itemCode: row.itemCode,
              },
            ]
          : this.ids;
      drillTaskReset({
        resetReq,
        processCode: "drill",
      }).then((res) => {
        if (res.code == 0) {
          this.getList();
          this.$modal.msgSuccess("重置成功");
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    //点击再次生成
    handleAgain(row) {
      getDrillWorkOrder(row.id).then(async (res) => {
        if (res.code == 0) {
          this.form = res.data;
          // 获取库存及待制任务信息
          const {
            data: { materialStockOverview },
          } = await getMaterialData({
            workOrderCode: res.data.workOrderCode,
            itemCode: res.data.itemCode,
          });
          if (materialStockOverview) {
            this.form.usableCount = materialStockOverview.usableCount;
          } else {
            this.form.usableCount = 0;
          }
          bulkAddDrillTask(this.form).then((res1) => {
            if (res1.code == 0) {
              // 调用重新计算库存接口
              this.handleRecalculate();
              this.$modal.msgSuccess("操作成功");
            } else {
              this.$modal.notifyError(res1.message);
            }
          });
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 更多触发操作
    handleCommand(command, row) {
      switch (command) {
        case "handleReset":
          this.handleReset(row);
          break;
        case "handleCommit":
          this.handleCommit(row);
          break;
        case "handleAgain":
          this.handleAgain(row);
          break;
        default:
          break;
      }
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.$refs.FormMachineTable.workOrderAndWorkStation) {
        if (
          this.$refs.FormMachineTable.workOrderAndWorkStation.length !=
          this.form.dispenseMachines
        ) {
          this.$confirm(
            "当前所选建议机台数量与建议数量不一致，是否继续？",
            "提示",
            {
              confirmButtonText: "确定",
              cancelButtonText: "取消",
              type: "warning",
            }
          )
            .then((res) => {
              this.handleAddDrillWorkOrder();
            })
            .catch(() => {});
        } else {
          this.handleAddDrillWorkOrder();
        }
      } else {
        this.handleAddDrillWorkOrder();
      }
    },
    // 操作数据库添加机台
    handleAddDrillWorkOrder() {
      addDrillWorkOrder(this.form).then((res) => {
        if (res.code == 0) {
          this.$modal.msgSuccess("新增成功");
          this.open = false;
          this.queryParams.pageNum = 1;
          this.getList();
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击计数器
    changeNum(params) {
      this.form[params.str] = params.value;
      this.addNumber();
    },
    //选择生产工单
    handleWorkOrderSelect() {
      this.$refs.woSelect.showFlag = true;
      this.$refs.woSelect.selectedWorkOrderId = this.form.workOrderId
        ? this.form.workOrderId
        : undefined;
      // 只可选择审批后的工单
      this.$refs.woSelect.getList();
    },
    // 选择生产工单
    onWorkOrderSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "workOrderId", row.id);
        this.$set(this.form, "workOrderCode", row.code);
        this.$set(this.form, "workOrderName", row.name);
        this.$set(this.form, "itemId", row.itemId);
        this.$set(this.form, "itemCode", row.itemCode);
        this.$set(this.form, "itemName", row.itemName);
        this.$set(this.form, "itemTypeId", row.itemTypeId);
        this.$set(this.form, "route", row.route);
        this.$set(this.form, "specification", row.specification);
        this.$set(this.form, "unitOfMeasure", row.unitOfMeasure);
        this.$set(this.form, "quantity", row.quantityChanged); //数量
        this.$set(this.form, "panelCount", row.panelCount); //叠板层数
        this.$set(this.form, "drillCount", row.drillCount); //孔数
        this.$set(this.form, "usableCount", row.wadCount); // 可用叠数
        this.$set(
          this.form,
          "dispenseMachines",
          row.dispenseMachines != null ? row.dispenseMachines : 0
        );
        this.addNumber();
        this.$nextTick(() => {
          this.$refs.FormMachineTable.getList(row.id);
        });
      }
    },
  },
};
</script>
<style lang="scss" scoped>
* {
  list-style: none;
}
.left {
  float: left;
  width: 500px;
  overflow-y: auto;
  height: calc(100vh - 120px);
  transition: 1s;
  transform: translateX(calc(-100% - 40px));

  .closebtn {
    position: sticky;
    top: 0px;
    z-index: 999;
    font-size: 20px;
    left: 100%;
  }
  .box-card {
    margin-top: -40px;
    width: 100%;
  }
}
.left.active {
  float: left;
  width: 500px;
  overflow-y: auto;
  height: calc(100vh - 120px);
  transition: 1s;
  transform: translateX(0);
  .closebtn {
    position: sticky;
    left: 100%;
    top: 0px;
    z-index: 999;
    font-size: 20px;
  }
  .box-card {
    margin-top: -40px;
    width: 100%;
    position: relative;
  }
}
.right {
  overflow-y: auto;
  height: calc(100vh - 120px);
  padding: 0 20px;
  position: absolute;
  left: 0;
  width: 100%;
  transition: 1s;
}
.right.active {
  overflow-y: auto;
  height: calc(100vh - 120px);
  padding: 0 20px;
  left: 520px;
  transition: 1s;
  width: calc(100% - 500px);
}
.left_info {
  padding: 0;
  margin: 0;
  width: 100%;
  display: flex;
  flex-direction: column;
  .info {
    font-size: 14px;
    width: 100%;
    display: flex;
    padding: 0;
    margin: 0;
    flex-direction: column;
    li {
      height: 30px;
      display: flex;
      align-items: center;
      p {
        margin-right: 3px;
      }
    }
  }
  .wad_count {
    font-size: 14px;
    width: 100%;
    display: flex;
    flex-wrap: wrap;
    padding: 0;
    margin: 0;
    margin-bottom: 10px;
    li {
      height: 30px;
      display: flex;
      align-items: center;
      margin-right: 10px;
      p {
        margin-right: 3px;
      }
    }
  }
  .el-table {
    margin: 5px 0;
  }
  .table {
    width: 100%;
  }
}
.mb8 {
  .el-button {
    margin-bottom: 5px;
  }
}
</style>
