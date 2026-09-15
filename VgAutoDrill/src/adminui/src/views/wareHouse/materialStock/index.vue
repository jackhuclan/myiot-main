<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料名称" prop="itemName">
        <el-input
          v-trim
          v-model="queryParams.itemName"
          placeholder="请输入物料名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="分区" prop="warehouseName">
        <el-input
          v-trim
          v-model="queryParams.warehouseName"
          placeholder="请输入分区"
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
          :disabled="hasPermi(['warehouse:materialStock:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          type="info"
          plain
          icon="el-icon-sort"
          @click="toggleExpandAll"
          >展开/折叠</el-button
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
      :ref="page"
      v-if="refreshTable"
      v-loading="loading"
      :data="wmstockList"
      row-key="id"
      :default-expand-all="isExpandAll"
      :tree-props="{ children: 'children' }"
    >
      <el-table-column
        label="出/入库单号"
        key="code"
        prop="code"
        show-overflow-tooltip
        min-width="200px"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['masterData:item:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="物料编码"
        min-width="180px"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="物料名称"
        min-width="180px"
        key="itemName"
        prop="itemName"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="工单编码"
        min-width="180px"
        key="workOrderCode"
        prop="workOrderCode"
        show-overflow-tooltip
        v-if="columns[3].visible"
      />

      <el-table-column
        label="分区编码"
        min-width="150px"
        key="warehouseCode"
        prop="warehouseCode"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />
      <el-table-column
        label="分区名称"
        min-width="150px"
        key="warehouseName"
        prop="warehouseName"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        label="料仓"
        align="center"
        key="siloCode"
        prop="siloCode"
        show-overflow-tooltip
        v-if="columns[6].visible"
        min-width="150px"
      />
      <el-table-column
        label="料仓最大层"
        align="center"
        key="maxLayers"
        prop="maxLayers"
        width="90px"
        v-if="columns[7].visible"
      />
      <el-table-column
        label="料仓实际层"
        align="center"
        key="currentLayers"
        prop="currentLayers"
        width="90px"
        v-if="columns[8].visible"
      />
      <el-table-column
        label="规格型号"
        align="center"
        key="specification"
        prop="specification"
        show-overflow-tooltip
        v-if="columns[9].visible"
      />
      <el-table-column
        label="数量"
        align="center"
        key="quantityTransaction"
        prop="quantityTransaction"
        v-if="columns[10].visible"
      />
      <el-table-column
        label="在库数量"
        align="center"
        key="quantityOnhand"
        prop="quantityOnhand"
        v-if="columns[11].visible"
      />
      <el-table-column
        label="单位"
        align="center"
        key="unitOfMeasure"
        prop="unitOfMeasure"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[12].visible"
      />
      <el-table-column
        label="批次号"
        align="center"
        key="batchCode"
        prop="batchCode"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[13].visible"
      />
      <el-table-column
        label="出库/入库"
        align="center"
        key="inOrOut"
        prop="inOrOut"
        v-if="columns[14].visible"
      >
        <template slot-scope="scope">{{
          scope.row.inOrOut == "in" ? "入库" : "出库"
        }}</template>
      </el-table-column>
      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="140px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <!-- <el-button
            type="text"
            icon="el-icon-edit"
            @click.native.stop="handleUpdate(scope.row)"
            :disabled="hasPermi(['warehouse:materialStock:edit'])"
            >修改</el-button
          > -->
          <el-button
            v-if="scope.row.inOrOut != 'out'"
            type="text"
            @click="handleAddRow(scope.row)"
            icon="el-icon-plus"
            :disabled="hasPermi(['warehouse:materialStock:outbound'])"
            >出库</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['warehouse:materialStock:remove'])"
            >删除</el-button
          ></template
        >
      </el-table-column>
    </el-table>

    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />

    <!-- 添加或修改库存现有量对话框 -->
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
            <el-form-item label="出/入库" prop="inOrOut">
              <el-radio-group
                v-removeAriaHidden
                @input="selectInOrOut"
                v-model="form.inOrOut"
                :disabled="optType != 'add'"
              >
                <el-radio :label="'in'">入库</el-radio>
                <el-radio :label="'out'">出库</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="10">
            <el-form-item label="出/入库单号" prop="code">
              <el-input
                :disabled="
                  optType == 'edit' || optType == 'view' || autoGenFlag
                "
                v-model="form.code"
                placeholder="出/入库单号"
              />
            </el-form-item>
          </el-col>
          <el-col :span="4">
            <el-form-item label-width="80">
              <el-switch
                :disabled="optType == 'edit' || optType == 'view'"
                v-model="autoGenFlag"
                active-color="#13ce66"
                active-text="自动生成"
                @change="handleAutoGenChange(autoGenFlag)"
              >
              </el-switch>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="生产工单" prop="workOrderCode">
              <el-input
                :disabled="optType == 'addChild' || optType == 'view'"
                v-model="form.workOrderCode"
                placeholder="请选择生产工单"
              >
                <el-button
                  :disabled="optType == 'addChild' || optType == 'view'"
                  slot="append"
                  icon="el-icon-search"
                  @click="handleWorkOrderSelect"
                ></el-button>
              </el-input>
              <WorkOrderSelect
                ref="woSelect"
                @onSelected="onWorkOrderSelected"
              ></WorkOrderSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="物料编码" prop="itemCode">
              <el-input
                :disabled="optType == 'addChild' || optType == 'view'"
                v-model="form.itemCode"
                placeholder="请选择物料"
              >
                <el-button
                  :disabled="optType == 'addChild' || optType == 'view'"
                  slot="append"
                  @click="handleSelectProduct"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <ItemSelect ref="ItemSelect" @onSelected="onItemSelected">
              </ItemSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="物料名称" prop="itemName">
              <el-input
                readonly
                :disabled="optType == 'addChild' || optType == 'view'"
                v-model="form.itemName"
                placeholder="请选择物料"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8" v-if="form.inOrOut == 'in'">
            <el-form-item label="工作站" prop="stationCode">
              <el-input
                v-model="form.stationCode"
                :disabled="optType == 'addChild' || optType == 'view'"
                placeholder="请选择工作站"
              >
                <el-button
                  :disabled="optType == 'addChild' || optType == 'view'"
                  slot="append"
                  @click="handleSelectWorkStation"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <WorkStationSelect
                ref="wsSelect"
                :type="'materialStock'"
                @onSelected="onWorkStationSelected"
              >
              </WorkStationSelect>
            </el-form-item>
          </el-col>
          <el-col
            :span="8"
            v-if="optType != 'addChild' && form.inOrOut == 'out'"
          >
            <el-form-item label="入库记录" prop="parentCode">
              <el-input
                :disabled="optType == 'addChild' || optType == 'view'"
                v-model="form.parentCode"
                placeholder="请选择入库记录"
              >
                <el-button
                  v-debounce
                  slot="append"
                  icon="el-icon-search"
                  @click="handleSelectMaterialStock"
                ></el-button>
              </el-input>
            </el-form-item>
            <MaterialStockSelect
              ref="MaterialStockSelect"
              @onSelected="onMaterialStockSelected"
            />
          </el-col>
          <el-col :span="8" v-else-if="form.inOrOut == 'out'">
            <el-form-item label="入库记录" prop="parentCode">
              <el-input
                v-model="form.parentCode"
                placeholder="请选择入库记录"
                disabled
              >
              </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="单位" prop="unitOfMeasure">
              <el-select
                v-model="form.unitOfMeasure"
                placeholder="请选择单位"
                :disabled="optType == 'addChild' || optType == 'view'"
              >
                <el-option
                  v-for="item in measureOptions"
                  :key="item.id"
                  :label="item.name"
                  :value="item.name"
                  v-optionTitle
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8" v-if="form.inOrOut == 'out'">
            <el-form-item label="在库数量" prop="quantityOnhand">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :dis="form.inOrOut == 'out' || optType == 'view'"
                :myNum="form.quantityOnhand"
                @changeNum="changeNum"
                :numName="'quantityOnhand'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="数量" prop="quantityTransaction">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :dis="optType == 'view'"
                :myNum="form.quantityTransaction"
                :max="10000"
                ref="quantityTransaction"
                @changeNum="changeNumQuantityTransaction"
                :numName="'quantityTransaction'"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item
              label="所属分区"
              prop="warehouseCode"
              label-width="140px"
            >
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                v-model="form.warehouseCode"
                :options="wareHouseTypeOptions"
                @select="wareHouseTypeSelect"
                @open="changeTreeselectOpen"
                :normalizer="normalizer"
                :show-count="true"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                placeholder="请选择归属产品"
              >
              </treeselect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="批次号" prop="batchCode">
              <el-input
                :disabled="optType == 'addChild' || optType == 'view'"
                v-model="form.batchCode"
                placeholder="请输入批次号"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="规格型号" prop="specification">
              <el-input
                :disabled="optType == 'addChild' || optType == 'view'"
                v-model="form.specification"
                placeholder="请输入规格型号"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <!-- <el-form-item label="料仓" prop="siloCode">
              <el-input
                :disabled="optType == 'addChild' || optType == 'view'"
                v-model="form.siloCode"
                placeholder="请输入料仓"
              />
            </el-form-item> -->
            <el-form-item label="料仓" prop="siloCode">
              <el-autocomplete
                :disabled="optType == 'view'"
                v-model="form.siloCode"
                :fetch-suggestions="querySearchAsync"
                placeholder="请输入内容"
                @select="onSiloManageSelected"
                popper-class="el-autocomplete-suggestion"
                :popper-append-to-body="false"
                :debounce="0"
              >
                <el-button
                  :disabled="optType == 'view'"
                  v-debounce
                  slot="append"
                  @click="handleSelectSiloManage"
                  icon="el-icon-search"
                ></el-button
              ></el-autocomplete>
              <siloManageSelect
                ref="siloManageSelect"
                @onSelected="onSiloManageSelected"
              >
              </siloManageSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="料仓最大层" prop="maxLayers">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :dis="optType == 'view' || optType == 'addChild'"
                :myNum="form.maxLayers"
                @changeNum="changeNum"
                :numName="'maxLayers'"
                :min="1"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="料仓实际层" prop="currentLayers">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :dis="optType == 'view' || optType == 'addChild'"
                :myNum="form.currentLayers"
                @changeNum="changeNum"
                :numName="'currentLayers'"
                :min="1"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listMaterialStock,
  getMaterialStock,
  delMaterialStock,
  addMaterialStock,
  updateMaterialStock,
  delList,
} from "@/api/wareHouse/materialStock";
import { listUnitMeasure } from "@/api/masterData/unitMeasure";
import MaterialStockSelect from "@/components/materialStockSelect";
// 物料选择
import ItemSelect from "@/components/itemSelect";
// 工单选择
import WorkOrderSelect from "@/components/workOrderSelect";

// 工作站选择
import WorkStationSelect from "@/components/partitionAndWorkStationSelect";
import { treeselect } from "@/api/wareHouse/partition";
import { listSilo } from "@/api/wareHouse/silo";
// 料仓选择
import siloManageSelect from "@/components/siloManageSelect";
export default {
  name: "MaterialStock",
  components: {
    ItemSelect,
    WorkOrderSelect,
    siloManageSelect,
    MaterialStockSelect,
    WorkStationSelect,
  },
  data() {
    // 自定义校验规则
    const quantityTransactionChange = (rule, value, callback) => {
      // 在库数量
      const quantityOnhand = isNaN(this.form.quantityOnhand)
        ? 0
        : this.form.quantityOnhand;
      // 入库
      if (this.form.inOrOut == "out") {
        // 出库
        if (
          !this.form.quantityTransaction ||
          this.form.quantityTransaction == 0
        ) {
          callback(new Error("出库数量不得等于0"));
        }
      } else {
        if (
          !this.form.quantityTransaction ||
          this.form.quantityTransaction <= 0
        ) {
          callback(new Error("数量不得小于等于0"));
        }
      }
      callback();
    };
    const warehouseCodeRule = (rule, value, callback) => {
      if (this.form.warehouseCode == "000") {
        callback(new Error("根分类不可选，请重新选择"));
      } else if (!this.form.warehouseCode) {
        callback(new Error("请选择所属分类"));
      } else {
        callback();
      }
    };
    return {
      page: "materialStock",
      // 是否展开，默认全部展开
      isExpandAll: true,
      // 重新渲染表格状态
      refreshTable: true,
      //自动生成编码
      autoGenFlag: false,
      // 操作类型
      optType: undefined,
      // 新增类型/出库/入库
      isInOrOut: undefined,
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 库存现有量表格数据
      wmstockList: [],
      // 分区下拉选项
      wareHouseTypeOptions: [],
      // 单位
      measureOptions: undefined,
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        siloCode: undefined,
        inOrOut: undefined,
        itemTypeId: 0,
        itemCode: undefined,
        itemName: undefined,
        warehouseName: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "分区编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "分区名称不能为空", trigger: "blur" },
        ],
        inOrOut: [{ required: true, message: "请选择出入库", trigger: "blur" }],
        workOrderCode: [
          { required: false, message: "请选择工单", trigger: "change" },
        ],
        itemCode: [
          { required: true, message: "请选择产品/物料", trigger: "change" },
        ],
        itemName: [
          { required: true, message: "请选择产品/物料", trigger: "change" },
        ],
        parentCode: [
          { required: true, message: "请选择入库记录", trigger: "change" },
        ],
        stationCode: [
          { required: false, message: "请选择工作站", trigger: "chnage" },
        ],
        quantityTransaction: [
          {
            required: true,
            validator: quantityTransactionChange,
            trigger: "change",
          },
        ],
        warehouseCode: [
          {
            required: true,
            validator: warehouseCodeRule,
            trigger: "change",
          },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "出/入库单号", visible: true },
        { key: 1, label: "物料编码", visible: true },
        { key: 2, label: "物料名称", visible: true },
        { key: 3, label: "工单编码", visible: true },
        { key: 4, label: "分区编码", visible: true },
        { key: 5, label: "分区名称", visible: true },
        { key: 6, label: "料仓", visible: true },
        { key: 7, label: "料仓最大层", visible: true },
        { key: 8, label: "料仓实际层", visible: true },
        { key: 9, label: "规格型号", visible: true },
        { key: 10, label: "数量", visible: true },
        { key: 11, label: "在库数量", visible: true },
        { key: 12, label: "单位", visible: true },
        { key: 13, label: "批次号", visible: true },
        { key: 14, label: "出库/入库", visible: true },
      ],
      quantityOnhand: 0,
    };
  },
  activated() {
    this.getList();
    this.getTreeselect();
    this.getMeasureOptions();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    "form.itemCode": {
      handler(val) {
        if (!val && this.optType != "addChild") {
          this.form.itemCode = undefined;
          this.form.itemId = undefined;
          this.form.itemTypeId = undefined;
          this.form.itemName = undefined;
          this.form.unitOfMeasure = undefined;
          this.form.specification = "";
        }
      },
      deep: true,
    },
    "form.workOrderCode": {
      handler(val) {
        if (!val && this.optType != "addChild") {
          this.form.workOrderId = undefined;
          this.form.workOrderCode = undefined;
          this.form.batchCode = undefined;
          this.form.itemCode = undefined;
          this.form.itemId = undefined;
          this.form.itemTypeId = undefined;
          this.form.itemName = undefined;
          this.form.unitOfMeasure = undefined;
          this.form.specification = "";
        }
      },
      deep: true,
    },
    "form.warehouseCode": {
      handler(val) {
        if (!val) {
          this.form.warehouseCode = undefined;
          this.form.warehouseId = undefined;
          this.form.warehouseName = undefined;
        }
      },
      deep: true,
    },
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询库存现有量列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listMaterialStock(this.queryParams);
      this.wmstockList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 获取单位列表
    async getMeasureOptions() {
      const res = await listUnitMeasure({
        pageNum: 1,
        pageSize: 100,
      });
      this.measureOptions = res.data.list;
    },
    /** 查询 分区下拉树结构 */
    getTreeselect() {
      treeselect().then((res) => {
        this.wareHouseTypeOptions = res.data;
      });
    },
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }

      return {
        id: node.code,

        label: node.label,

        isDisabled: node.status == 0,

        children: node.children,
      };
    },
    // 选择归属分区时同时保存分区code
    wareHouseTypeSelect(val) {
      this.form.warehouseCode = val.code;
      this.form.warehouseName = val.label;
    },
    changeTreeselectOpen() {
      const doms = document.querySelectorAll(".el-dialog .el-select");
      doms.forEach((v, i) => {
        this.$refs[`select` + (i + 1)]?.blur();
      });
    },
    // 表单重置
    reset() {
      this.form = {
        siloCode: "",
        maxLayers: 1,
        currentLayers: 1,
        inOrOut: "",
        itemTypeId: undefined,
        itemId: undefined,
        itemCode: undefined,
        itemName: undefined,
        specification: "",
        unitOfMeasure: undefined,
        quantityTransaction: 0,
        quantityOnhand: 0,
        batchCode: undefined,
        warehouseId: undefined,
        warehouseCode: undefined,
        warehouseName: undefined,
        workOrderId: undefined,
        workOrderCode: undefined,
        code: "",
        parentId: undefined,
        parentCode: "",
      };
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
      this.handleQuery();
    },
    // 多选框选中数据
    handleSelectionChange(selection) {
      this.ids = selection.map((item) => item.id);
    },
    /** 展开/折叠操作 */
    toggleExpandAll() {
      this.refreshTable = false;
      this.isExpandAll = !this.isExpandAll;
      this.$nextTick(() => {
        this.refreshTable = true;
      });
    },
    /** 头部新增按钮操作 */
    handleAdd() {
      this.reset();
      this.getTreeselect();
      this.open = true;
      this.title = "添加库存现有量";
      this.optType = "add";
      this.isInOrOut = "in";
      this.initialForm = Object.assign({}, this.form);
    },
    // 新增时选择出入库变化监听
    selectInOrOut(val) {
      if (this.optType == "add") {
        this.reset();
        this.form.inOrOut = val;
      }
    },
    // 行内新增
    handleAddRow(row) {
      this.autoGenFlag = false;
      this.reset();
      this.getTreeselect();
      const warehouseId = row.id || this.ids;
      getMaterialStock(warehouseId).then((res) => {
        if (res.code == 0) {
          if (res.data.quantityOnhand == 0)
            return this.$modal.notifyError("当前入库已全部出库");
          this.form = {
            ...res.data,
            inOrOut: "out",
            parentId: row.id,
            parentCode: row.code,
            quantityTransaction: 0,
            code: "",
          };
          delete this.form.id;
          this.quantityOnhand = this.form.quantityOnhand;
          this.open = true;
          this.title = "添加出库记录";
          this.optType = "addChild";
        } else {
          this.$modal.notifyError(res.message);
        }
        this.initialForm = Object.assign({}, this.form);
      });
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleAddRow":
          this.handleAddRow(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      this.getTreeselect();
      getMaterialStock(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看分区";
          this.optType = "view";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      this.getTreeselect();
      const warehouseId = row.id || this.ids;
      getMaterialStock(warehouseId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.enCode = res.data.code;
          this.open = true;
          this.initialForm = Object.assign({}, res.data);
          this.title = "修改库存现有量";
          this.optType = "edit";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != undefined) {
        updateMaterialStock(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addMaterialStock(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("新增成功");
            this.open = false;
            this.queryParams.pageNum = 1;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    //删除
    handleDelete(row) {
      if (row.children.length > 0)
        return this.$modal.notifyError("当前入库记录存在出库，不可操作删除");
      if (row.id) {
        this.deleteItem(
          row.id,
          delMaterialStock,
          this.getList,
          "出/入库单号为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    //选择物料弹出框
    handleSelectProduct() {
      this.$refs.ItemSelect.showFlag = true;
      this.$refs.ItemSelect.title = "物料选择";
      this.$refs.ItemSelect.selectedItemCode = this.form.itemCode
        ? this.form.itemCode
        : undefined;
      this.$refs.ItemSelect.getList();
    },
    onItemSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "itemCode", obj.code);
        this.$set(this.form, "itemId", obj.id);
        this.$set(this.form, "itemTypeId", obj.itemTypeId);
        this.$set(this.form, "itemName", obj.name);
        this.$set(this.form, "unitOfMeasure", obj.unitOfMeasure);
        this.$set(this.form, "specification", obj.specification);
      }
    },
    //选择生产工单
    handleWorkOrderSelect() {
      this.$refs.woSelect.showFlag = true;
      this.$refs.woSelect.selectedWorkOrderId = this.form.workOrderId
        ? this.form.workOrderId
        : undefined;
      this.$refs.woSelect.queryParams.pageNum = 1;
      this.$refs.woSelect.getList();
    },
    // 选择生产工单
    onWorkOrderSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "workOrderId", row.id);
        this.$set(this.form, "workOrderCode", row.code);
        this.$set(this.form, "batchCode", row.batchCode);
        this.$set(this.form, "itemId", row.itemId);
        this.$set(this.form, "itemCode", row.itemCode);
        this.$set(this.form, "itemName", row.itemName);
        this.$set(this.form, "itemTypeId", row.itemTypeId);
        this.$set(this.form, "specification", row.specification);
        this.$set(this.form, "unitOfMeasure", row.unitOfMeasure);
      }
    },

    //选择料仓
    handleSelectSiloManage() {
      this.$refs.siloManageSelect.showFlag = true;
      this.$refs.siloManageSelect.selectedSiloId = this.form.siloCode
        ? this.form.siloCode
        : undefined;
      this.$refs.siloManageSelect.getList();
    },
    onSiloManageSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "siloCode", obj.code);
        this.$set(this.form, "currentLayers", obj.floorCount);
        this.$set(this.form, "maxLayers", obj.floorCount);
      }
    },

    querySearchAsync(queryString, cb) {
      if (queryString === "" || !queryString) {
        let arr = [];
        cb(arr);
      } else {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
          listSilo({
            pageNum: 1,
            pageSize: 1000,
            code: queryString,
          }).then((res) => {
            if (res.code === 0) {
              let arr = res.data.list.map((v) => {
                return {
                  value: v.code,
                  ...v,
                };
              });
              cb(arr);
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }, 500);
      }
    },
    // 选择入库记录
    handleSelectMaterialStock() {
      this.$refs.MaterialStockSelect.showFlag = true;
      this.$refs.MaterialStockSelect.selectedMaterialStockId = this.form
        .parentId
        ? this.form.parentId
        : undefined;
      this.$refs.MaterialStockSelect.queryParams.pageNum = 1;
      this.$refs.MaterialStockSelect.getList();
    },
    onMaterialStockSelected(obj) {
      this.$set(this.form, "parentId", obj.id);
      this.$set(this.form, "parentCode", obj.code);
      this.$set(this.form, "quantityOnhand", obj.quantityOnhand);
      this.quantityOnhand = obj.quantityOnhand;
    },
    // 选择工作站
    handleSelectWorkStation() {
      this.$refs.wsSelect.showFlag = true;
      this.$refs.wsSelect.queryParams.pageNum = 1;
      this.$refs.wsSelect.selectedWorkStationId = this.form.stationId
        ? this.form.stationId
        : undefined;
      this.$refs.wsSelect.getList();
      this.$refs.wsSelect.getWorkshops();
    },
    onWorkStationSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "stationId", row.id);
        this.$set(this.form, "stationCode", row.code);
        this.$set(this.form, "processCode", row.processCode);
        this.$set(this.form, "processName", row.processName);
      }
    },
    // //自动生成编码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        if (!this.form.inOrOut) {
          this.$modal.msgError("先选择出入库！");
          this.autoGenFlag = false;
          return;
        }
        if (this.form.inOrOut == "out") {
          this.getEncode({
            rulesCode: "MATERIALSTOCK_OUT_CODE",
            buildCount: 1,
          }).then((response) => {
            const code = response.data[0];
            this.$set(this.form, "code", code);
          });
        } else {
          this.getEncode({
            rulesCode: "MATERIALSTOCK_IN_CODE",
            buildCount: 1,
          }).then((response) => {
            const code = response.data[0];
            this.$set(this.form, "code", code);
          });
        }
      } else {
        if (this.optType == "edit") return (this.form.code = this.enCode);
        this.form.code = "";
      }
    },
    changeNumQuantityTransaction(params) {
      this.$set(this.form, [params.str], params.value);
      if (this.form.inOrOut == "out") {
        this.form.quantityOnhand = this.quantityOnhand - this.form[params.str];
        if (this.form.quantityOnhand <= 0) {
          this.$set(this.form, [params.str], this.quantityOnhand);
          this.$set(this.form, "quantityOnhand", 0);
          this.$refs.quantityTransaction.currentMax = this.quantityOnhand;
        } else {
          this.$refs.quantityTransaction.currentMin = 0;
        }
      } else {
        this.form.quantityOnhand = params.value;
      }
    },
  },
};
</script>
