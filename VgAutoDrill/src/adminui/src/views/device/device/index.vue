<template>
  <div class="app-container">
    <div class="index">
      <div class="left" :class="{ fold: openType }">
        <leftTreeSelect
          v-model="deviceTypeName"
          :placeholder="'请输入产品名称'"
          :filterNode="filterNode"
          :options="deviceTypeOptions"
          :defaultProps="defaultProps"
          :handleNodeClick="handleNodeClick"
          :highlightCurrent="highlightCurrent"
        ></leftTreeSelect>
      </div>
      <div class="right" :class="{ fold: openType }">
        <search-form
          v-show="showSearch"
          :form="queryParams"
          @search="handleQuery"
          @reset="resetQuery"
          :openType="openType"
        >
          <el-form-item label="设备编码" prop="code">
            <el-input
              v-trim
              v-model="queryParams.code"
              placeholder="请输入设备编码"
              clearable
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item label="设备名称" prop="name">
            <el-input
              v-trim
              v-model="queryParams.name"
              placeholder="请输入设备名称"
              clearable
              @keyup.enter.native="handleQuery"
            />
          </el-form-item>
          <el-form-item label="设备类别" prop="deviceKindList">
            <el-select
              v-model="queryParams.deviceKindList"
              placeholder="请选择"
              multiple
              collapse-tags
              :class="
                queryParams.deviceKindList &&
                queryParams.deviceKindList.length >= 2
                  ? 'select-hastags'
                  : ''
              "
            >
              <el-option
                v-for="item in $status.deviceKinds"
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
          <el-form-item label="是否可用" prop="status">
            <el-select
              @clear="clearQueryParams('status')"
              v-model="queryParams.status"
              placeholder="请选择"
              clearable
              style="width: 150px"
            >
              <el-option label="是" value="1" />
              <el-option label="否" value="0" />
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
              :disabled="hasPermi(['device:device:add'])"
              >新增</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="danger"
              plain
              icon="el-icon-delete"
              @click="handleDelete"
              :disabled="hasPermi(['device:device:remove'])"
              >批量删除</el-button
            >
          </el-col>
          <el-col :span="1.5">
            <el-button
              v-debounce
              type="info"
              plain
              icon="el-icon-upload2"
              @click="handleImport"
              :disabled="hasPermi(['masterData:client:import'])"
              >导入</el-button
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
          :ref="page"
          v-loading="loading"
          :data="deviceList"
          @selection-change="handleSelectionChange"
          @row-dblclick="rowDblclick"
          :row-style="rowStyle"
          border
        >
          <el-table-column type="selection" width="55" align="center" />
          <el-table-column
            label="设备编码"
            show-overflow-tooltip
            key="code"
            prop="code"
            v-if="columns[0].visible"
            min-width="120"
          >
            <template slot-scope="scope">
              <span
                class="click_code"
                :data-id="JSON.stringify(scope.row)"
                v-isGetSelection="['device:device:view']"
                >{{ scope.row.code }}</span
              >
            </template>
          </el-table-column>
          <el-table-column
            label="设备名称"
            show-overflow-tooltip
            min-width="150"
            key="name"
            prop="name"
            v-if="columns[1].visible"
          />
          <el-table-column
            label="设备类别"
            show-overflow-tooltip
            min-width="120"
            key="processCode"
            prop="processCode"
            v-if="columns[2].visible"
          >
            <template slot-scope="scope">
              {{
                $status.deviceKinds.find((v) => v.value == scope.row.deviceKind)
                  ? $status.deviceKinds.find(
                      (v) => v.value == scope.row.deviceKind
                    ).label
                  : ""
              }}
            </template></el-table-column
          >

          <el-table-column
            label="是否自动"
            align="center"
            key="isAuto"
            min-width="100px"
            v-if="columns[6].visible"
          >
            <template slot-scope="scope">
              <el-tag v-if="scope.row.isAuto"> 自动 </el-tag>
              <span v-else-if="scope.row.isAuto == undefined"></span>
              <el-tag v-else type="danger">非自动</el-tag>
            </template>
          </el-table-column>

          <el-table-column
            label="轴数"
            show-overflow-tooltip
            key="spindleNum"
            prop="spindleNum"
            align="center"
            width="80px"
            v-if="columns[3].visible"
          />

          <el-table-column
            label="是否可用"
            align="center"
            key="status"
            v-if="columns[4].visible"
          >
            <template slot-scope="scope">
              <el-tag v-if="scope.row.status"> 是 </el-tag>
              <el-tag v-else type="danger">否</el-tag>
            </template>
          </el-table-column>
          <el-table-column
            label="创建时间"
            align="center"
            key="createTime"
            prop="createTime"
            width="180"
            v-if="columns[5].visible"
          >
            <template slot-scope="scope">
              <span>{{ parseTime(scope.row.createTime) }}</span>
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
                @click="handleEnabledOrDisabled(scope.row, '启用')"
                icon="el-icon-circle-check"
                :disabled="
                  hasPermi(['device:device:enabled']) || scope.row.status != 0
                "
                >启用</el-button
              >
              <el-button
                type="text"
                @click="handleEnabledOrDisabled(scope.row, '禁用')"
                icon="el-icon-remove-outline"
                :disabled="
                  hasPermi(['device:device:disabled']) || scope.row.status != 1
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
                    v-if="scope.row.deviceTypeCode == 'drill'"
                    icon="el-icon-paperclip"
                    command="handleDrillDetail"
                    :disabled="hasPermi(['device:device:edit'])"
                    >操作板料</el-dropdown-item
                  >
                  <el-dropdown-item
                    command="handleSpotCheck"
                    icon="el-icon-magic-stick"
                    :disabled="hasPermi(['device:device:check'])"
                    >点检</el-dropdown-item
                  >
                  <el-dropdown-item
                    icon="el-icon-edit"
                    :disabled="hasPermi(['device:device:edit'])"
                    command="handleUpdate"
                    >修改</el-dropdown-item
                  >
                  <el-dropdown-item
                    command="handleDelete"
                    icon="el-icon-delete"
                    :disabled="hasPermi(['device:device:remove'])"
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
    <!-- 添加或修改设备对话框 -->
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
            <el-form-item label="设备名称" prop="name">
              <el-input
                v-model="form.name"
                placeholder="请输入设备名称"
                :disabled="title == '修改设备'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="设备编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入设备编码"
                :disabled="title == '修改设备'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="归属产品" prop="deviceTypeId">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :normalizer="normalizer"
                v-model="form.deviceTypeId"
                :options="deviceTypeOptions"
                @select="deviceTypeSelect"
                @open="changeTreeselectOpen"
                :show-count="true"
                noOptionsText="暂无数据"
                noChildrenText="暂无数据"
                noResultsText="暂无数据"
                placeholder="请选择归属产品"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="设备类别" prop="deviceKind">
              <el-select
                ref="select1"
                v-model="form.deviceKind"
                placeholder="请选择设备类别"
                :popper-append-to-body="false"
                popper-class="hide-select"
              >
                <el-option
                  v-optionTitle
                  v-for="item in $status.deviceKinds"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="设备状态">
              <el-select
                ref="select2"
                v-model="form.deviceStatus"
                placeholder="请选择设备状态"
              >
                <!-- "未知","在线","离线","准备中","工作中","异常"-->
                <el-option
                  v-for="item in $status.deviceStatusOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                  v-optionTitle
                />
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="工艺路线" prop="routeCode">
              <el-select
                ref="select3"
                :disabled="!form.deviceTypeId"
                v-model="form.routeCode"
                placeholder="请选择工艺路线"
                multiple
                collapse-tags
                :class="
                  form.routeCode && form.routeCode.length >= 2
                    ? 'select-hastags'
                    : ''
                "
              >
                <el-option
                  v-for="item in routeSelectOptions"
                  :key="item.id"
                  :label="item.label"
                  :value="item.code"
                  v-optionTitle
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row v-if="form.deviceKind == 2 || form.deviceKind == 3">
          <el-col :span="8">
            <el-form-item label="是否自动" prop="isAuto">
              <el-radio-group v-removeAriaHidden v-model="form.isAuto">
                <el-radio :label="true"> 自动 </el-radio>
                <el-radio :label="false"> 非自动 </el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="设备机型" prop="machineSize">
              <el-select
                ref="select3"
                v-model="form.machineSize"
                placeholder="请选择设备机型"
                @change="changeEquipmentModel"
              >
                <el-option
                  v-for="item in equipmentModels"
                  :key="item.id"
                  :label="item.value"
                  :value="item.value"
                  v-optionTitle
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="最大板长" prop="maxBoardLength">
              <input-number
                :myNum="form.maxBoardLength"
                @changeNum="changeNum"
                :numName="'maxBoardLength'"
                :min="1"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row v-if="form.deviceKind == 2 || form.deviceKind == 3">
          <el-col :span="8">
            <el-form-item label="生料区库位">
              <el-input
                v-model="form.rawLocationCode"
                placeholder="请输入生料区库位"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="熟料区库位">
              <el-input
                v-model="form.clinkerLocationCode"
                placeholder="请输入熟料区库位"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="轴数" prop="spindleNum">
              <input-number
                :myNum="form.spindleNum"
                @changeNum="changeNum"
                :numName="'spindleNum'"
                :dis="optType == 'view'"
                :min="1"
                :max="1000"
              />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="维护周期" prop="maintainPeriodDays">
              <el-select
                ref="select4"
                v-model="form.maintainPeriodDays"
                placeholder="请选择"
              >
                <el-option
                  v-for="item in maintainPeriodDaysOptions"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="投产日期" prop="productionTime">
              <el-date-picker
                clearable
                v-model="form.productionTime"
                type="date"
                value-format="yyyy-MM-dd"
                placeholder="选择日期时间"
                style="width: 200px"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="设备参数" prop="parameters">
          <!-- <b-code-editor
            v-if="open"
            ref="editor"
            v-model="form.parameters"
            :indent-unit="4"
          /> -->
          <vue-json-editor
            v-model="form.parameters"
            :showBtns="false"
            :mode="'code'"
            style="height: 500px"
            lang="zh"
            @json-save="onJsonSave"
            @json-change="onJsonChange"
            @has-error="onError"
          />
        </el-form-item>
      </el-form>
    </edit-form-dialog>

    <drill-detail :code="drillCode" ref="drill_detail" />
    <!-- 查看弹框 -->
    <device-info :deviceId="form.id" ref="viewDialog" />
    <!-- 点检弹框 -->
    <DeviceAndSubject ref="subject" :optType="optType" :deviceId="form.id" />
    <!-- 设备导入 -->
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
import DeviceAndSubject from "./components/deviceAndSubject.vue"; // 导入模块
import vueJsonEditor from "vue-json-editor-fix-cn";

import deviceInfo from "./components/deviceInfo/index.vue";
import DrillDetail from "./components/drillDetail.vue";

import {
  listDevice,
  getDevice,
  delDevice,
  delList,
  addDevice,
  updateDevice,
  treeselect,
  disableDevice,
  enableDevice,
} from "@/api/device/device";

import { mapActions } from "vuex";
import { getRoutesByProcess } from "@/api/masterData/workStation";
export default {
  name: "Device",
  components: { deviceInfo, DrillDetail, DeviceAndSubject, vueJsonEditor },
  data() {
    // 自定义校验规则
    const deviceTypeIdRule = (rule, value, callback) => {
      if (this.form.deviceTypeId == 1) {
        callback(new Error("根分类不可选，请重新选择"));
      } else if (!this.form.deviceTypeId) {
        callback(new Error("请选择所属分类"));
      } else {
        callback();
      }
    };
    const parametersRule = (rule, value, callback) => {
      if (!this.hasJsonFlag) {
        callback();
      } else {
        callback(new Error("格式不正确"));
      }
    };
    return {
      page: "device",
      optType: "",
      // 分类属性结构显隐
      openType: true,
      // 模拟数据
      tableData: [],
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 设备表格数据
      deviceList: [],
      // 与工序相关的工艺路线
      routeSelectOptions: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 点检弹框
      subjectOpen: false,
      // 树形结构选项
      highlightCurrent: true,
      deviceTypeStatus: undefined,
      deviceTypeOptions: undefined,
      form_deviceTypeCode: undefined,
      form_deviceTypeName: undefined,
      defaultProps: {
        children: "children",
        label: "label",
      },
      drillCode: null,
      // 树形结构双向绑定名称
      deviceTypeName: "",
      // 设备列表查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        status: undefined,
        deviceTypeId: undefined,
        isAuto: undefined,
        deviceStatusList: [],
        deviceKindList: [],
      },
      // 表单参数
      form: {},
      initialForm: {},
      hasJsonFlag: false,
      parametersJson: undefined,
      maintainPeriodDaysOptions: [
        {
          value: 1,
          label: "一天",
        },
        {
          value: 2,
          label: "两天",
        },
        {
          value: 3,
          label: "三天",
        },
        {
          value: 4,
          label: "四天",
        },
        {
          value: 5,
          label: "五天",
        },
      ],
      // 2530(最大尺寸635*762mm)
      // 2537(最大尺寸635*940mm)
      // 2543(最大尺寸635*1095mm)
      // 2732(最大尺寸686*840mm)
      // 2849(最大尺寸745*1245mm)
      // 设备机型
      equipmentModels: [
        {
          label: "2530(最大尺寸635*762mm)",
          value: "2530",
          maxBoardLength: 762,
        },
        {
          label: "2537(最大尺寸635*940mm)",
          value: "2537",
          maxBoardLength: 940,
        },
        {
          label: "2543(最大尺寸635*1095mm)",
          value: "2543",
          maxBoardLength: 1095,
        },
        {
          label: "2732(最大尺寸686*840mm)",
          value: "2732",
          maxBoardLength: 840,
        },
        {
          label: "2849(最大尺寸745*1245mm)",
          value: "2849",
          maxBoardLength: 1245,
        },
      ],
      // 表单校验
      rules: {
        name: [
          { required: true, message: "设备名称不能为空", trigger: "blur" },
        ],
        code: [
          { required: true, message: "设备编码不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "设备状态不能为空", trigger: "blur" },
        ],

        spindleNum: [
          { required: true, message: "轴数不能为空", trigger: "blur" },
        ],

        processCode: [
          { required: true, message: "工序不能为空", trigger: "change" },
        ],
        isAuto: [
          { required: true, message: "请选择是否自动", trigger: "change" },
        ],
        machineSize: [
          { required: true, message: "请选择设备机型", trigger: "change" },
        ],
        deviceTypeId: [
          { required: true, validator: deviceTypeIdRule, trigger: "change" },
        ],
        parameters: [
          { required: false, validator: parametersRule, trigger: "change" },
        ],
      },
      // 设备导入参数
      // 导入的url
      uploadUrl: "/v1/Device/UploadList",
      // 下载url
      downloadUrl: "/v1/Device/DownLoad",
      // 导入的模板下载名
      fileName: "设备模板.xlsx",
      // 列信息
      columns: [
        { key: 0, label: "设备编码", visible: true },
        { key: 1, label: "设备名称", visible: true },
        { key: 2, label: "设备类别", visible: true },
        { key: 3, label: "轴数", visible: true },
        { key: 4, label: "是否可用", visible: true },
        { key: 5, label: "创建时间", visible: true },
        { key: 6, label: "是否自动", visible: true },
      ],
    };
  },

  watch: {
    // 根据名称筛选产品树
    productName(val) {
      this.$refs.tree.filter(val);
    },
    "form.deviceTypeId": {
      handler(val) {
        if (!val) {
          this.form.deviceTypeCode = "";
          this.form.deviceTypeName = "";
        }
      },
      deep: true,
    },
  },
  activated() {
    this.queryParams.deviceTypeId = undefined;
    this.getTreeselect();
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      const parameters = JSON.stringify(this.form.parameters);
      const initialFormParameters = JSON.stringify(this.initialForm.parameters);
      const isParameters = parameters !== initialFormParameters;
      const form = { ...this.form };
      delete form.parameters;
      const initialForm = { ...this.initialForm };
      delete initialForm.parameters;
      const isForm = JSON.stringify(form) !== JSON.stringify(initialForm);
      return isParameters || isForm || this.hasJsonFlag;
    },
  },
  methods: {
    onJsonChange(value) {
      // 实时保存
      this.onJsonSave(value);
    },
    onJsonSave(value) {
      this.hasJsonFlag = false;
      this.form.parameters = value;
    },
    onError(value) {
      this.hasJsonFlag = true;
    },
    // vuex中保存deviceCode方法
    ...mapActions("device", ["setDevice"]),
    /** 查询设备列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listDevice(this.queryParams);
      this.deviceList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    /** 查询设备下拉树结构 */
    async getTreeselect() {
      const res = await treeselect();
      this.deviceTypeOptions = res.data;
      if (res.data[0].id == 1) {
        this.openType = true;
      } else {
        this.openType = false;
      }
    },
    /** 转换设备数据结构 */
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.label,
        children: node.children,
        isDisabled: node.status == 0,
      };
    },
    // 根据工序编码获取关联的工艺路线
    getRouteAndProcessList(processCode) {
      getRoutesByProcess({
        pageNum: 1,
        pageSize: 1000,
        processCode,
      }).then((res) => {
        this.routeSelectOptions = res.data?.list.map((v) => {
          return {
            name: v.name,
            code: v.code,
            id: v.id,
            label: v.name,
          };
        });
      });
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
    // 筛选节点
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    // 节点单击事件
    handleNodeClick(data) {
      this.highlightCurrent = true;
      this.deviceTypeStatus = data.status;
      this.form_deviceTypeCode = data.code;
      this.form_deviceTypeName = data.label;

      if (data.label != "全部") {
        this.queryParams.deviceTypeId = data.id;
      } else {
        this.queryParams.deviceTypeId = undefined;
      }

      this.handleQuery();
    },
    // 显隐分类
    handleOpenType() {
      this.openType = !this.openType;
    },
    // 表单重置
    reset() {
      this.form = {
        name: "",
        code: "",
        deviceTypeId: undefined,
        deviceTypeCode: "",
        deviceTypeName: "",
        maintainPeriodDays: undefined,
        parameters: {},
        deviceStatus: undefined,
        productionTime: null,
        spindleNum: 1,
        clinkerLocationCode: "",
        rawLocationCode: "",
        isAuto: true,
        routeCode: [],
        maxBoardLength: 0,
        machineSize: undefined,
      };
      this.parametersJson = undefined;
      this.resetForm("form");
    },
    /** 搜索按钮操作 */
    handleQuery() {
      this.queryParams.pageNum = 1;
      this.getList("search");
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.deviceTypeName = "";
      this.queryParams.deviceTypeId = undefined;
      this.highlightCurrent = false;
      this.handleQuery();
    },
    // 点击查看
    handleView(data) {
      this.reset();
      // this.getTreeselect();
      // this.form = res.data;
      this.setDevice(JSON.parse(data));
      this.$refs.viewDialog.open = true;
      this.$refs.viewDialog.getList();
      // this.$refs.viewDialog.tableData = {
      //   ...res.data,
      //   deviceStatus: this.$status.deviceStatusOptions.filter(
      //     (v) => v.value == res.data.deviceStatus
      //   )[0]?.label,
      // };

      this.optType = "view";
    },
    // 点击点检
    handleSpotCheck(row) {
      const deviceId = row.id || this.ids;
      getDevice(deviceId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.optType = "view";
          this.$refs.subject.queryParams.deviceId = deviceId;
          this.$refs.subject.getList();
          this.$refs.subject.open = true;
        } else {
          this.$notify({
            type: "error",
            title: "提示",
            message: res.message,
          });
        }
      });
    },
    /** 新增按钮操作 */
    handleAdd() {
      if (this.deviceTypeStatus != null && this.deviceTypeStatus == 0)
        return this.$modal.msgError("当前分类已停用");
      this.reset();
      // this.getTreeselect();
      if (this.queryParams.deviceTypeId) {
        this.form.deviceTypeId = this.queryParams.deviceTypeId;
      } else {
        this.form.deviceTypeId = 1;
      }
      this.open = true;
      this.title = "添加设备";
      this.optType = "add";
      this.maintainPeriodDays = "";
      this.formDeviceStatus = "";
      this.initialForm = Object.assign({}, this.form);
    },

    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      // this.getTreeselect();
      const deviceId = row.id || this.ids;
      getDevice(deviceId).then((res) => {
        if (res.code == 0) {
          this.getRouteAndProcessList(res.data.deviceTypeCode);
          this.form = res.data;
          // this.parametersJson =
          this.form.parameters =
            res.data.parameters != undefined
              ? JSON.parse(res.data.parameters)
              : {};
          this.initialForm = Object.assign({}, res.data);

          this.optType = "edit";
          this.open = true;
          this.title = "修改设备";
        } else {
          this.$notify({
            type: "error",
            title: "提示",
            message: res.message,
          });
        }
      });
    },
    //点击详情
    handleDrillDetail(row) {
      this.drillCode = row.code;
      this.$refs.drill_detail.getList(row.code);
    },
    // 启用or禁用
    async handleEnabledOrDisabled(row, type) {
      const api = type == "启用" ? enableDevice : disableDevice;
      const res = await this.$modal
        .confirm(
          `确定${type} <span style="color:red">设备编码为${row.code} </span>的数据项?`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .catch(() => {});
      if (res) {
        api({ deviceCode: row.code }).then((result) => {
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
        case "handleSpotCheck":
          this.handleSpotCheck(row);
          break;
        case "handleUpdate":
          this.handleUpdate(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        case "handleDrillDetail":
          this.handleDrillDetail(row);
          break;
        default:
          break;
      }
    },
    /** 提交按钮 */
    submitForm: function () {
      const routeCode = this.form.routeCode.join(",");
      if (!this.openType) {
        this.form.deviceTypeCode = this.form_deviceTypeCode;
        this.form.deviceTypeName = this.form_deviceTypeName;
      }
      if (this.form.id != undefined) {
        updateDevice({
          ...this.form,
          routeCode,
          parameters: JSON.stringify(this.form.parameters),
        }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addDevice({
          ...this.form,
          routeCode,
          parameters: JSON.stringify(this.form.parameters),
        }).then((res) => {
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
      if (row.id) {
        this.deleteItem(
          row.id,
          delDevice,
          this.getList,
          "设备编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 选择归属产品时同时保存设备类型code
    deviceTypeSelect(val) {
      this.form.deviceTypeCode = val.code;
      this.form.deviceTypeName = val.label;
      if (val.label == "全部") return;
      this.getRouteAndProcessList(val.code);
    },
    changeTreeselectOpen() {
      const doms = document.querySelectorAll(".el-dialog .el-select");
      doms.forEach((v, i) => {
        this.$refs[`select` + (i + 1)]?.blur();
      });
    },
    // 选择设备机型赋值最大板长
    changeEquipmentModel(val) {
      console.log(val);
      const maxBoardLength = this.equipmentModels.find(
        (v) => v.value == val
      )?.maxBoardLength;
      this.form.maxBoardLength = maxBoardLength;
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "设备导入";
      this.$refs.upload.open = true;
    },
  },
};
</script>
<style>
.jsoneditor-vue {
  height: 100% !important;
}
/* jsoneditor右上角默认有一个链接,加css去掉了 */
.jsoneditor-poweredBy {
  display: none !important;
}
</style>
