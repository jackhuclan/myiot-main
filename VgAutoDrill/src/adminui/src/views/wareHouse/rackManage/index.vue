<template>
  <div class="app-container">
    <div class="index">
      <div class="left" :class="{ fold: openType }">
        <leftTreeSelect
          ref="treeSelect"
          v-model="wareHouseName"
          :placeholder="'请输入分区名称'"
          :filterNode="filterNode"
          :options="wareHouseTypeOptions"
          :defaultProps="defaultProps"
          :handleNodeClick="handleNodeClick"
          :highlightCurrent="highlightCurrent"
        ></leftTreeSelect>
      </div>
      <div class="right" :class="{ fold: openType }">
        <search-form
          :openType="openType"
          v-show="showSearch"
          :form="queryParams"
          @search="handleQuery"
          @reset="resetQuery"
        >
          <el-form-item label="库位编码" prop="code">
            <el-input
              v-trim
              v-model="queryParams.code"
              @keyup.enter.native="handleQuery"
              placeholder="请输入库位编码"
            ></el-input>
          </el-form-item>
          <el-form-item label="位置码" prop="positionCode">
            <el-input
              v-trim
              v-model="queryParams.positionCode"
              @keyup.enter.native="handleQuery"
              placeholder="请输入位置码"
            ></el-input>
          </el-form-item>

          <el-form-item label="设备类别" prop="deviceKinds">
            <el-select
              v-model="queryParams.deviceKinds"
              placeholder="请选择"
              multiple
              collapse-tags
              :class="
                queryParams.deviceKinds && queryParams.deviceKinds.length >= 2
                  ? 'select-hastags'
                  : ''
              "
            >
              <el-option
                v-for="item in deviceKinds"
                :key="item.value"
                :label="item.label"
                :value="item.value"
                v-optionTitle
              />
            </el-select>
          </el-form-item>
        </search-form>
        <el-row :gutter="10" class="mb8">
          <el-col :span="1.5">
            <el-button
              type="info"
              plain
              icon="el-icon-s-operation"
              @click="handleOpenType"
              >显/隐分类</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="primary"
              plain
              icon="el-icon-plus"
              @click="handleAdd"
              :disabled="hasPermi(['warehouse:rackManage:add'])"
              >新增</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="info"
              plain
              icon="el-icon-upload2"
              @click="handleImport"
              :disabled="hasPermi(['warehouse:rackManage:import'])"
              >导入</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="danger"
              plain
              icon="el-icon-delete"
              @click="handleDelete"
              :disabled="hasPermi(['warehouse:rackManage:remove'])"
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
          :ref="page"
          v-loading="loading"
          :data="rackManageList"
          @selection-change="handleSelectionChange"
          @row-dblclick="rowDblclick"
          :row-style="rowStyle"
        >
          <el-table-column type="selection" width="55" align="center" />

          <el-table-column
            label="库位编码"
            key="code"
            prop="code"
            show-overflow-tooltip
            min-width="100"
            fixed="left"
            v-if="columns[0].visible"
          >
            <template slot-scope="scope">
              <span
                class="click_code"
                :data-id="scope.row.id"
                v-isGetSelection="['warehouse:rackManage:view']"
                >{{ scope.row.code }}</span
              >
            </template>
          </el-table-column>
          <el-table-column
            label="分区"
            key="wareHouseCode"
            prop="wareHouseCode"
            min-width="100"
            fixed="left"
            show-overflow-tooltip
            v-if="columns[1].visible"
          />

          <el-table-column
            label="位置码"
            key="positionCode"
            prop="positionCode"
            show-overflow-tooltip
            fixed="left"
            v-if="columns[2].visible"
          />

          <el-table-column
            label="设备"
            key="relateDeviceCode"
            prop="relateDeviceCode"
            min-width="100"
            fixed="left"
            show-overflow-tooltip
            v-if="columns[3].visible"
          />
          <el-table-column
            label="料仓"
            key="siloCode"
            prop="siloCode"
            min-width="150"
            show-overflow-tooltip
            v-if="columns[4].visible"
          />
          <el-table-column
            label="设备类别"
            key="deviceKind"
            show-overflow-tooltip
            min-width="120"
            v-if="columns[5].visible"
          >
            <template slot-scope="scope">
              {{
                deviceKinds.find((v) => v.value == scope.row.deviceKind)
                  ? deviceKinds.find((v) => v.value == scope.row.deviceKind)
                      .label
                  : ""
              }}
            </template></el-table-column
          >

          <el-table-column
            label="上料AGV内点"
            key="feedAGVInnerPoint"
            prop="feedAGVInnerPoint"
            min-width="150"
            show-overflow-tooltip
            v-if="columns[6].visible"
          />
          <el-table-column
            label="上料AGV外点"
            key="feedAGVOutputPoint"
            prop="feedAGVOutputPoint"
            min-width="150"
            show-overflow-tooltip
            v-if="columns[7].visible"
          />
          <el-table-column
            label="上料AGV休息点"
            key="feedAGVRestPoint"
            prop="feedAGVRestPoint"
            min-width="150"
            show-overflow-tooltip
            v-if="columns[8].visible"
          />
          <el-table-column
            label="转运AGV内点"
            key="transAGVInnerPoint"
            prop="transAGVInnerPoint"
            min-width="150"
            show-overflow-tooltip
            v-if="columns[9].visible"
          />
          <el-table-column
            label="转运AGV外点"
            key="transAGVOutputPoint"
            prop="transAGVOutputPoint"
            min-width="150"
            show-overflow-tooltip
            v-if="columns[10].visible"
          />
          <el-table-column
            label="转运AGV休息点"
            key="transAGVRestPoint"
            prop="transAGVRestPoint"
            min-width="150"
            show-overflow-tooltip
            v-if="columns[11].visible"
          />
          <el-table-column
            label="状态"
            key="status"
            prop="status"
            v-if="columns[12].visible"
            align="center"
          >
            <template slot-scope="scope">
              <el-tag v-if="scope.row.status"> 已启用 </el-tag>
              <el-tag v-else type="danger"> 已禁用</el-tag>
            </template>
          </el-table-column>
          <el-table-column
            label="操作"
            align="center"
            fixed="right"
            min-width="200px"
            class-name="small-padding fixed-width"
          >
            <template slot-scope="scope">
              <el-button
                type="text"
                icon="el-icon-circle-check"
                @click="handleEnabledOrDisabled(scope.row, '启用')"
                :disabled="
                  hasPermi(['warehouse:rackManage:enabled']) ||
                  scope.row.status != 0
                "
                >启用</el-button
              >
              <el-button
                type="text"
                icon="el-icon-remove-outline"
                @click="handleEnabledOrDisabled(scope.row, '禁用')"
                :disabled="
                  hasPermi(['warehouse:rackManage:disabled']) ||
                  scope.row.status != 1
                "
                >禁用</el-button
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
                    icon="el-icon-paperclip"
                    command="handleChangePanel"
                    :disabled="hasPermi([`dashboard:rack:change`])"
                    >操作板料</el-dropdown-item
                  >
                  <el-dropdown-item
                    icon="el-icon-edit"
                    command="handleUpdate"
                    :disabled="hasPermi(['warehouse:rackManage:edit'])"
                    >修改</el-dropdown-item
                  >
                  <el-dropdown-item
                    command="handleDelete"
                    icon="el-icon-delete"
                    :disabled="hasPermi(['warehouse:rackManage:remove'])"
                    >删除</el-dropdown-item
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
      </div>
    </div>

    <!-- 添加或修改库位对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form
        ref="form"
        :model="form"
        label-width="100px"
        :rules="rules"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="7">
            <el-form-item label="库位编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入库位编码"
                :disabled="autoGenFlag || optType != 'add'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="3">
            <el-form-item label-width="80">
              <el-switch
                v-model="autoGenFlag"
                active-color="#13ce66"
                active-text="自动生成"
                @change="handleAutoGenChange(autoGenFlag)"
                :disabled="optType != 'add'"
              >
              </el-switch>
            </el-form-item>
          </el-col>
          <el-col :span="7">
            <el-form-item label="位置码" prop="positionCode">
              <el-input
                v-model="form.positionCode"
                placeholder="请输入位置码"
              />
            </el-form-item>
          </el-col>
          <el-col :span="7">
            <el-form-item label="所属分区" prop="wareHouseId">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                v-model="form.wareHouseId"
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
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="料仓">
              <el-input v-model="form.siloCode" placeholder="请选择料仓">
                <el-button
                  v-debounce
                  slot="append"
                  @click="handleSelectSiloManage"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <siloManageSelect
                ref="siloManageSelect"
                @onSelected="onsiloManageSelected"
              >
              </siloManageSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="设备编码" prop="relateDeviceCode">
              <el-input
                v-model="form.relateDeviceCode"
                placeholder="请选择设备"
              >
                <el-button
                  v-debounce
                  @click="handleDeviceSelectAdd"
                  slot="append"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <DeviceSelect
                ref="deviceSelcet"
                @onSelected="onDeviceSelectAdd"
              ></DeviceSelect>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="设备类别" prop="deviceKind">
              <el-select
                ref="select1"
                v-model="form.deviceKind"
                placeholder="请选择设备类别"
              >
                <el-option
                  v-for="item in deviceKinds"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                  v-optionTitle
                />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item
              label="上料AGV内点"
              prop="feedAGVInnerPoint"
              label-width="120px"
            >
              <el-input
                v-model="form.feedAGVInnerPoint"
                placeholder="请输入上料AGV内点"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              label="上料AGV外点"
              prop="feedAGVOutputPoint"
              label-width="120px"
            >
              <el-input
                v-model="form.feedAGVOutputPoint"
                placeholder="请输入上料AGV外点"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              label="上料AGV休息点"
              prop="feedAGVRestPoint"
              label-width="140px"
            >
              <el-input
                v-model="form.feedAGVRestPoint"
                placeholder="请输入上料AGV休息点"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item
              label="转运AGV内点"
              prop="transAGVInnerPoint"
              label-width="120px"
            >
              <el-input
                v-model="form.transAGVInnerPoint"
                placeholder="请输入转运AGV内点"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              label="转运AGV外点"
              prop="transAGVOutputPoint"
              label-width="120px"
            >
              <el-input
                v-model="form.transAGVOutputPoint"
                placeholder="请输入转运AGV外点"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              label="转运AGV休息点"
              prop="transAGVRestPoint"
              label-width="140px"
            >
              <el-input
                v-model="form.transAGVRestPoint"
                placeholder="请输入转运AGV休息点"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="是否可用" prop="status">
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio :label="1">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
    <setForm ref="setForm"> </setForm>
    <!-- 导入 -->
    <ImportXlsx
      ref="upload"
      @getList="getList"
      :uploadUrl="uploadUrl"
      :downloadUrl="downloadUrl"
      :fileName="fileName"
    ></ImportXlsx>
  </div>
</template>

<script>
import siloManageSelect from "@/components/siloManageSelect";
import { treeselect } from "@/api/wareHouse/partition";
import {
  listRack,
  updateRack,
  addRack,
  delRack,
  getRack,
  enableRack,
  disableRack,
  delList,
} from "@/api/wareHouse/rack";
import setForm from "../components/changePanel.vue";
import DeviceSelect from "@/components/deviceSelect";

export default {
  name: "RackManage",
  components: { siloManageSelect, setForm, DeviceSelect },
  data() {
    // 自定义校验规则
    const wareHouseIdRule = (rule, value, callback) => {
      if (this.form.wareHouseId == 1) {
        callback(new Error("根分类不可选，请重新选择"));
      } else if (!this.form.wareHouseId) {
        callback(new Error("请选择所属分类"));
      } else {
        callback();
      }
    };
    return {
      page: "rackManage",
      //自动生成编码
      autoGenFlag: false,
      enCode: "",
      optType: "",
      // 分类属性结构显隐
      openType: true,
      // 分区下拉选项
      wareHouseTypeOptions: [],
      // 树形结构
      highlightCurrent: true,
      wareHouseStatus: undefined,
      wareHouseCode: undefined,
      defaultProps: {
        children: "children",
        label: "label",
      },
      // 树形结构双向绑定名称
      wareHouseName: "",
      // 遮罩层
      loading: false,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 库位表格数据
      rackManageList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        wareHouseId: undefined,
        deviceKinds: [],
        positionCode: undefined,
      },
      deviceKinds: [
        {
          value: 4,
          label: "叠板机",
        },
        {
          value: 5,
          label: "拆板机",
        },
        {
          value: 13,
          // 板料料架==>公共缓存区
          label: "公共缓存区",
        },
        {
          value: 14,
          label: "刀具料架",
        },
        {
          value: 15,
          // 板料插齿==>中转区
          label: "中转区",
        },
      ],
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "库位编码不能为空", trigger: "change" },
        ],
        feedAGVInnerPoint: [
          { required: true, message: "上料AGV内点不能为空", trigger: "blur" },
        ],
        feedAGVOutputPoint: [
          { required: true, message: "上料AGV外点不能为空", trigger: "blur" },
        ],
        // feedAGVRestPoint: [
        //   { required: true, message: "上料AGV休息点不能为空", trigger: "blur" },
        // ],
        transAGVInnerPoint: [
          { required: true, message: "转运AGV内点不能为空", trigger: "blur" },
        ],
        transAGVOutputPoint: [
          { required: true, message: "转运AGV外点不能为空", trigger: "blur" },
        ],
        // transAGVRestPoint: [
        //   { required: true, message: "转运AGV休息点不能为空", trigger: "blur" },
        // ],

        status: [
          { required: true, message: "是否启用不能为空", trigger: "blur" },
        ],
        wareHouseId: [
          { required: true, validator: wareHouseIdRule, trigger: "change" },
        ],
        relateDeviceCode: [
          { required: true, message: "设备不能为空", trigger: "blur" },
        ],
        deviceKind: [
          { required: true, message: "设备类别不能为空", trigger: "change" },
        ],
      },
      // 列信息，
      columns: [
        { key: 0, label: "库位编码", visible: true },
        { key: 1, label: "分区", visible: true },
        { key: 2, label: "位置码", visible: true },
        { key: 3, label: "设备", visible: true },
        { key: 4, label: "料仓", visible: true },
        { key: 5, label: "设备类别", visible: true },
        { key: 6, label: "上料AGV内点", visible: true },
        { key: 7, label: "上料AGV外点", visible: true },
        { key: 8, label: "上料AGV休息点", visible: true },
        { key: 9, label: "转运AGV内点", visible: true },
        { key: 10, label: "转运AGV外点", visible: true },
        { key: 11, label: "转运AGV休息点", visible: true },
        { key: 12, label: "状态", visible: true },
      ],
      // 库位导入参数
      // 导入的url
      uploadUrl: "/v1/Rack/Upload",
      downloadUrl: "/v1/Rack/DownLoad",
      // 导入的模板下载名
      fileName: "库位模板.xlsx",
    };
  },
  activated() {
    this.getList();
    this.getTreeselect();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询库位列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listRack(this.queryParams);
      this.rackManageList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
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
        id: node.id,

        label: node.label,

        isDisabled: node.status == 0,

        children: node.children,
      };
    },

    // 筛选节点
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    // 节点单击事件
    handleNodeClick(data) {
      this.highlightCurrent = true;
      this.wareHouseStatus = data.status;
      this.wareHouseCode = data.code;
      if (data.label != "全部") {
        this.queryParams.wareHouseId = data.id;
      } else {
        this.queryParams.wareHouseId = undefined;
      }
      this.handleQuery();
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
    // 表单重置
    reset() {
      this.form = {
        code: "",
        status: 1,
        wareHouseCode: "",
        siloCode: "",
        innerPoint: "",
        outPoint: "",
        positionCode: "",
      };
      this.autoGenFlag = false;
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.code = this.queryParams.code?.trim();
      this.queryParams.name = this.queryParams.name?.trim();
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.wareHouseName = "";
      this.queryParams.wareHouseId = undefined;
      this.highlightCurrent = false;
      this.handleQuery();
    },
    // 显隐分类
    handleOpenType() {
      this.openType = !this.openType;
    },
    // 选择归属分区时同时保存分区code
    wareHouseTypeSelect(val) {
      this.form.wareHouseCode = val.code;
    },
    changeTreeselectOpen() {
      const doms = document.querySelectorAll(".el-dialog .el-select");
      doms.forEach((v, i) => {
        this.$refs[`select` + (i + 1)]?.blur();
      });
    },
    /** 新增按钮操作 */
    handleAdd() {
      if (this.wareHouseStatus != null && this.wareHouseStatus == 0)
        return this.$modal.msgError("当前分类已停用");

      this.reset();
      // this.getTreeselect();
      if (this.queryParams.wareHouseId) {
        this.form.wareHouseId = this.queryParams.wareHouseId;
      } else {
        this.form.wareHouseId = 1;
      }
      this.open = true;
      this.title = "添加库位信息";
      this.optType = "add";
      this.autoGenFlag = false;
      this.initialForm = Object.assign({}, this.form);
    },
    //设备资源选择弹出
    handleDeviceSelectAdd() {
      this.$refs.deviceSelcet.showFlag = true;
      this.$refs.deviceSelcet.selectedDeviceCode = this.form.relateDeviceCode
        ? this.form.relateDeviceCode
        : undefined;
      this.$refs.deviceSelcet.queryParams.deviceKindList = this.deviceKinds.map(
        (v) => v.value
      );
      this.$refs.deviceSelcet.getList();
      this.$refs.deviceSelcet.getTreeselect();
    },
    //设备资源选择回调
    onDeviceSelectAdd(row) {
      if (row != null && row != undefined) {
        this.$set(this.form, "relateDeviceCode", row.code);
        this.$set(this.form, "deviceKind", row.deviceKind);
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "库位导入";
      this.$refs.upload.open = true;
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      // this.getTreeselect();
      const rackId = row.id || this.ids;
      getRack(rackId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改库位信息";
          this.optType = "edit";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (!this.openType) {
        this.form.wareHouseCode = this.wareHouseCode;
      }
      if (this.form.id != null) {
        updateRack(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addRack(this.form).then((res) => {
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
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(row.id, delRack, this.getList, "库位编码为" + row.code);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 启用or禁用
    async handleEnabledOrDisabled(row, type) {
      const api = type == "启用" ? enableRack : disableRack;
      const res = await this.$modal
        .confirm(
          `确定${type} <span style="color:red">库位编码为${row.code} </span>的数据项?`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .catch(() => {});
      if (res) {
        api({ code: row.code }).then((result) => {
          if (result.code == 0) {
            this.$modal.msgSuccess(type + "成功");
            this.getList();
          } else {
            this.$modal.notifyError(result.message);
          }
        });
      }
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleUpdate":
          this.handleUpdate(row);
          break;
        case "handleChangePanel":
          this.handleChangePanel(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    //料仓选择弹出框
    handleSelectSiloManage() {
      this.$refs.siloManageSelect.showFlag = true;
      this.$refs.siloManageSelect.selectedSiloId = this.form.siloCode
        ? this.form.siloCode
        : undefined;
      this.$refs.siloManageSelect.getList();
    },
    onsiloManageSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "siloCode", obj.code);
      }
    },
    //自动生成码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "RACK_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
          // this.form.name = code;
        });
      } else {
        if (this.optType == "edit") {
          this.form.code = this.enCode;
          // this.form.name = this.enName;
          return;
        }
        this.form.code = "";
        // this.form.name = null;
      }
    },
    // 操作板料
    handleChangePanel(row) {
      this.$refs.setForm.showFlag = true;
      this.$refs.setForm.topForm = { ...row };
      this.$refs.setForm.oldSiloCode = row.siloCode;

      this.$refs.setForm.dialogTitle = `操作板料`;
      this.$refs.setForm.queryParams.siloCode = row.siloCode;
      this.$refs.setForm.getList();
    },
    // 查看料仓板料
    handleView(id) {
      this.reset();
      const rackId = id;
      getRack(rackId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "查看库位信息";
          this.optType = "view";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
  },
};
</script>
