<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="分区编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入分区编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="分区名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入分区名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="分区类别" prop="partitionKinds">
        <el-select
          v-model="queryParams.partitionKinds"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.partitionKinds && queryParams.partitionKinds.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in $status.partitionKinds"
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
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['warehouse:partition:add'])"
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
      v-loading="loading"
      :data="partitionList"
      v-if="refreshTable"
      row-key="id"
      :default-expand-all="isExpandAll"
      :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
    >
      <el-table-column
        label="分区编码"
        key="code"
        prop="code"
        min-width="250px"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.parentId == 0">{{ scope.row.code }}</span>

          <span
            v-else
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['warehouse:partition:view']"
            >{{ scope.row.code }}</span
          >
        </template></el-table-column
      >
      <el-table-column
        min-width="200px"
        label="分区名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="分区类别"
        show-overflow-tooltip
        min-width="120"
        key="partitionKind"
        prop="partitionKind"
        align="center"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          {{
            $status.partitionKinds.find(
              (v) => v.value == scope.row.partitionKind
            )
              ? $status.partitionKinds.find(
                  (v) => v.value == scope.row.partitionKind
                ).label
              : ""
          }}
        </template></el-table-column
      >
      <el-table-column
        label="料仓类型"
        key="transportationKind"
        prop="transportationKind"
        v-if="columns[3].visible"
        min-width="80"
        align="center"
      >
        <template slot-scope="scope">
          <el-tag
            v-if="
              transportation_kind_options.find(
                (v) => v.value == scope.row.transportationKind
              )
            "
          >
            {{
              transportation_kind_options.find(
                (v) => v.value == scope.row.transportationKind
              ).label
            }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="预约AGV"
        align="center"
        key="preBookAgv"
        prop="preBookAgv"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />
      <el-table-column
        label="预约时间"
        align="center"
        key="preBookTime"
        prop="preBookTime"
        min-width="180px"
        show-overflow-tooltip
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.preBookTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="负责人"
        align="center"
        key="charge"
        prop="charge"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />
      <el-table-column
        label="状态"
        key="status"
        prop="status"
        v-if="columns[7].visible"
        align="center"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status"> 已启用 </el-tag>
          <el-tag v-else type="danger"> 已禁用</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        min-width="150px"
        key="remark"
        prop="remark"
        v-if="columns[8].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
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
            v-if="scope.row.parentId == 0"
            @click="handleAdd(scope.row)"
            icon="el-icon-plus"
            :disabled="hasPermi(['warehouse:partition:add'])"
            >新增</el-button
          >
          <el-button
            v-if="scope.row.parentId != 0"
            type="text"
            icon="el-icon-circle-check"
            @click="handleEnabledOrDisabled(scope.row, '启用')"
            :disabled="
              hasPermi(['warehouse:partition:enabled']) || scope.row.status != 0
            "
            >启用</el-button
          >
          <el-button
            v-if="scope.row.parentId != 0"
            type="text"
            icon="el-icon-remove-outline"
            @click="handleEnabledOrDisabled(scope.row, '禁用')"
            :disabled="
              hasPermi(['warehouse:partition:disabled']) ||
              scope.row.status != 1
            "
            >禁用</el-button
          >
          <el-dropdown
            v-if="scope.row.parentId != 0"
            trigger="click"
            @command="(command) => handleCommand(command, scope.row)"
          >
            <span :class="'el-dropdown-link ' + $store.getters.size">
              <i class="el-icon-d-arrow-right el-icon--right"></i>更多
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                command="handleAdd"
                icon="el-icon-plus"
                :disabled="hasPermi(['warehouse:partition:add'])"
                >新增</el-dropdown-item
              >
              <el-dropdown-item
                command="handleUpdate"
                icon="el-icon-edit"
                :disabled="hasPermi(['warehouse:partition:edit'])"
                >修改</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['warehouse:partition:remove'])"
                >删除</el-dropdown-item
              >
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>

    <!-- <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    /> -->

    <!-- 添加或修改分区设置对话框 -->
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
        <el-tabs v-model="activeName" type="border-card">
          <el-tab-pane label="基本信息" name="first">
            <el-row>
              <el-col :span="10">
                <el-form-item label="分区编码" prop="code">
                  <el-input
                    v-model="form.code"
                    placeholder="请输入分区编码"
                    :disabled="optType != 'add' || autoGenFlag"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="4">
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
              <el-col :span="10">
                <el-form-item label="分区名称" prop="name">
                  <el-input v-model="form.name" placeholder="请输入分区名称" />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="8">
                <el-form-item label="上级分区" prop="parentId">
                  <treeselect
                    :class="$store.getters.size + '-treeselect'"
                    :disabled="optType == 'view'"
                    noOptionsText="暂无数据"
                    noChildrenText="暂无数据"
                    noResultsText="暂无数据"
                    v-model="form.parentId"
                    :options="wareHouseTypeOptions"
                    :normalizer="normalizer"
                    :show-count="true"
                    placeholder="选择上级分区"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item label="分区类别" prop="partitionKind">
                  <el-select v-model="form.partitionKind" placeholder="请选择">
                    <el-option
                      v-for="item in $status.partitionKinds"
                      :key="item.value"
                      :label="item.label"
                      :value="item.value"
                      v-optionTitle
                    />
                  </el-select> </el-form-item
              ></el-col>
              <el-col :span="8">
                <el-form-item label="负责人" prop="charge">
                  <el-input v-model="form.charge" placeholder="请选择负责人">
                    <el-button
                      v-debounce
                      slot="append"
                      @click="handleUserSelect"
                      icon="el-icon-search"
                    ></el-button>
                  </el-input>
                </el-form-item>
                <UserSingleSelect
                  ref="userSelect"
                  @onSelected="onUserSelected"
                ></UserSingleSelect>
              </el-col>
            </el-row>

            <el-row>
              <el-col :span="8">
                <el-form-item label="料仓类型" prop="transportationKind">
                  <el-select
                    v-model="form.transportationKind"
                    placeholder="请选择"
                    clearable
                    @clear="clearTransportationKind"
                  >
                    <el-option
                      v-for="item in transportation_kind_options"
                      :key="item.value"
                      :label="item.label"
                      :value="item.value"
                    />
                  </el-select>
                </el-form-item>
              </el-col>

              <el-col :span="8" v-if="form.id != null">
                <el-form-item label="预约AGV" prop="preBookAgv">
                  <el-input
                    v-model="form.preBookAgv"
                    placeholder="请输入预约AGV"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8" v-if="form.id != null">
                <el-form-item label="预约时间" prop="preBookTime">
                  <el-date-picker
                    clearable
                    v-model="form.preBookTime"
                    type="datetime"
                    placeholder="预约时间"
                    style="width: 205px"
                  >
                  </el-date-picker>
                </el-form-item>
              </el-col>
            </el-row>

            <el-form-item label="备注" prop="remark">
              <el-input
                v-model="form.remark"
                type="textarea"
                placeholder="请输入内容"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-tab-pane>
          <el-tab-pane label="转运配置" name="second">
            <el-row>
              <el-col :span="8">
                <el-form-item
                  label="最多空仓"
                  label-width="130px"
                  prop="maxEmptyBoxNum"
                >
                  <input-number
                    :dis="optType == 'view'"
                    :myNum="form.maxEmptyBoxNum"
                    @changeNum="changeNum"
                    :numName="'maxEmptyBoxNum'"
                    :min="0"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  label="最少空仓"
                  label-width="130px"
                  prop="minEmptyBoxNum"
                >
                  <input-number
                    :dis="optType == 'view'"
                    :myNum="form.minEmptyBoxNum"
                    @changeNum="changeNum"
                    :numName="'minEmptyBoxNum'"
                    :min="0"
                  />
                </el-form-item>
              </el-col>

              <el-col :span="8">
                <el-form-item
                  label="最少空位"
                  label-width="130px"
                  prop="minEmptyLocationNum"
                >
                  <input-number
                    :dis="optType == 'view'"
                    :myNum="form.minEmptyLocationNum"
                    @changeNum="changeNum"
                    :numName="'minEmptyLocationNum'"
                    :min="1"
                  />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="8">
                <el-form-item
                  label="首件最少转出"
                  label-width="130px"
                  prop="minFirstTrackOutNum"
                >
                  <input-number
                    :dis="optType == 'view'"
                    :myNum="form.minFirstTrackOutNum"
                    @changeNum="changeNum"
                    :numName="'minFirstTrackOutNum'"
                    :min="1"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  label="熟料最少转出"
                  label-width="130px"
                  prop="minDrilledTrackOutNum"
                >
                  <input-number
                    :dis="optType == 'view'"
                    :myNum="form.minDrilledTrackOutNum"
                    @changeNum="changeNum"
                    :numName="'minDrilledTrackOutNum'"
                    :min="1"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  label="缓冲区代号"
                  label-width="130px"
                  prop="relatedBufferCode"
                >
                  <el-input
                    v-model="form.relatedBufferCode"
                    placeholder="请输入关联的缓冲区代号"
                    :disabled="optType == 'view'"
                  />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="8">
                <el-form-item
                  label="生料转出超时(秒)"
                  prop="rawTrackOutTimeOutTime"
                  label-width="130px"
                >
                  <input-number
                    :dis="optType == 'view'"
                    :myNum="form.rawTrackOutTimeOutTime"
                    @changeNum="changeNum"
                    :numName="'rawTrackOutTimeOutTime'"
                    :min="300"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  label="熟料转出超时(分)"
                  prop="drilledTrackOutTimeMinutes"
                  label-width="130px"
                >
                  <input-number
                    :dis="optType == 'view'"
                    :myNum="form.drilledTrackOutTimeMinutes"
                    @changeNum="changeNum"
                    :numName="'drilledTrackOutTimeMinutes'"
                    :min="1"
                  />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="12">
                <el-form-item
                  label="料仓中没有钻机中相同的料号时是否立即转出"
                  prop="isAutoTrackOutDrilledSilo"
                  label-width="320px"
                >
                  <el-radio-group
                    v-removeAriaHidden
                    v-model="form.isAutoTrackOutDrilledSilo"
                  >
                    <el-radio :label="true">是</el-radio>
                    <el-radio :label="false">否</el-radio>
                  </el-radio-group>
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item
                  label="转运任务是否由服务端控制"
                  prop="isControlledByServer"
                  label-width="200px"
                >
                  <el-radio-group
                    v-removeAriaHidden
                    v-model="form.isControlledByServer"
                  >
                    <el-radio :label="true">是</el-radio>
                    <el-radio :label="false">否</el-radio>
                  </el-radio-group>
                </el-form-item></el-col
              >
            </el-row></el-tab-pane
          >
        </el-tabs>
      </el-form>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listPartition,
  getPartition,
  delPartition,
  addPartition,
  updatePartition,
  treeselect,
  delList,
  enablePartition,
  disablePartition,
} from "@/api/wareHouse/partition";
// 负责人选择
import UserSingleSelect from "@/components/userSelect";
export default {
  name: "Partition",
  components: { UserSingleSelect },
  data() {
    // 自定义校验规则
    const parentIdRule = (rule, value, callback) => {
      // if (this.form.parentId == 1) {
      //   callback(new Error("根分类不可选，请重新选择"));
      // } else
      if (!this.form.parentId) {
        callback(new Error("请选择所属分类"));
      } else {
        callback();
      }
    };
    const handeleRules = (rule, value, callback) => {
      const formLabel = rule.label;
      const formItem = rule.field;
      const formValue = rule.value;
      if (this.form[formItem] < formValue) {
        //!this.form[formItem] ||
        callback(new Error(formLabel + "数值不能小于" + formValue));
      } else {
        callback();
      }
    };
    return {
      page: "partition",
      //自动生成编码
      autoGenFlag: false,
      optType: undefined,
      // 遮罩层
      loading: true,

      // 是否展开，默认全部折叠
      isExpandAll: true,
      // 重新渲染表格状态
      refreshTable: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 分区设置表格数据
      partitionList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 1000,
        code: undefined,
        name: undefined,
        remark: undefined,
        workStationId: undefined,
        charge: undefined,
        partitionKinds: [],
      },
      // 类型
      transportation_kind_options: [
        {
          label: "空仓",
          value: 1,
        },
        {
          label: "生料仓",
          value: 2,
        },
        {
          label: "熟料仓",
          value: 3,
        },
        {
          label: "首件",
          value: 4,
        },
      ],
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
        parentId: [
          { required: true, validator: parentIdRule, trigger: "change" },
        ],
        partitionKind: [{ required: true }],
        rawTrackOutTimeOutTime: [
          {
            required: true,
            validator: handeleRules,
            value: 300,
            label: "生料转出超时",
            trigger: "change",
          },
        ],
        isAutoTrackOutDrilledSilo: [{ required: true }],
        minFirstTrackOutNum: [
          {
            required: true,
            validator: handeleRules,
            value: 1,
            label: "首件最少转出",
            trigger: "change",
          },
        ],
        minDrilledTrackOutNum: [
          {
            required: true,
            validator: handeleRules,
            value: 1,
            label: "熟料最少转出",
            trigger: "change",
          },
        ],
        maxEmptyBoxNum: [
          {
            required: true,
            validator: handeleRules,
            value: 0,
            label: "最多空仓",
            trigger: "change",
          },
        ],
        minEmptyBoxNum: [
          {
            required: true,
            validator: handeleRules,
            value: 0,
            label: "最少空仓",
            trigger: "change",
          },
        ],
        minEmptyLocationNum: [
          {
            required: true,
            validator: handeleRules,
            value: 1,
            label: "最少空位",
            trigger: "change",
          },
        ],
      },
      wareHouseTypeOptions: [],
      // 列信息
      columns: [
        { key: 0, label: "分区编码", visible: true },
        { key: 1, label: "分区名称", visible: true },
        { key: 2, label: "分区类别", visible: true },
        { key: 3, label: "料仓类型", visible: true },
        { key: 4, label: "预约AGV", visible: true },
        { key: 5, label: "预约时间", visible: true },
        { key: 6, label: "负责人", visible: true },
        { key: 7, label: "状态", visible: true },
        { key: 8, label: "备注", visible: true },
      ],
      activeName: "first",
    };
  },
  activated() {
    this.getList();
    this.getTreeselect();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  watch: {
    "form.workStationName": {
      handler(val) {
        if (!val) {
          this.form.workStationId = undefined;
          this.form.workStationName = undefined;
          this.form.workStationCode = undefined;
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
    /** 查询分区设置列表 */
    getList(isSearch) {
      this.loading = true;
      listPartition(this.queryParams).then((res) => {
        this.partitionList = this.handleTree(res.data.list, "id");
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
      });
    },
    /** 查询分区下拉树结构 */
    getTreeselect() {
      listPartition(this.queryParams).then((res) => {
        this.wareHouseTypeOptions = this.handleTree(res.data.list, "id");
      });
    },
    /** 转换分区数据结构 */
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }
      return {
        id: node.id,
        label: node.name,
        children: node.children,
      };
    },
    /** 展开/折叠操作 */
    toggleExpandAll() {
      this.refreshTable = false;
      this.isExpandAll = !this.isExpandAll;
      this.$nextTick(() => {
        this.refreshTable = true;
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
    // 表单重置
    reset() {
      this.form = {
        code: "",
        name: "",
        remark: "",
        workStationId: undefined,
        workStationCode: undefined,
        workStationName: undefined,
        charge: "",
        partitionKind: 0,
        rawTrackOutTimeOutTime: 1800,
        isAutoTrackOutDrilledSilo: false,
        isControlledByServer: false,
        minFirstTrackOutNum: 1,
        minDrilledTrackOutNum: 1,
        maxEmptyBoxNum: 1,
        minEmptyBoxNum: 1,
        minEmptyLocationNum: 2,
        drilledTrackOutTimeMinutes: 10,
      };
      this.activeName = "first";
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
    // /** 新增按钮操作 */
    // handleAdd() {
    //   this.reset();
    //   this.open = true;
    //   this.title = "添加分区设置";
    //   this.optType = "add";
    //   this.initialForm = Object.assign({}, this.form);
    // },
    // 新增修改时料仓类型可清空
    clearTransportationKind() {
      this.form.transportationKind = undefined;
    },
    /** 新增按钮操作 */
    handleAdd(row) {
      this.showFlag = false;
      this.reset();
      this.getTreeselect();
      if (row.id && row != null) {
        this.form.parentId = row.id;
        getPartition(row.id).then((res) => {
          if (res.code == 0) {
            this.optType = "add";
            this.open = true;
            this.title = "添加分区设置";
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        this.form.parentId = 1;
        this.optType = "add";
        this.open = true;
        this.title = "添加分区设置";
      }
      this.initialForm = Object.assign({}, this.form);
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getPartition(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.transportationKind = res.data.transportationKind
            ? res.data.transportationKind
            : undefined;
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
      const partitionId = row.id || this.ids;
      getPartition(partitionId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.enCode = res.data.code;
          this.form.transportationKind = res.data.transportationKind
            ? res.data.transportationKind
            : undefined;
          this.open = true;
          this.title = "修改分区设置";
          this.optType = "edit";
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updatePartition(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addPartition(this.form).then((res) => {
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
      if (row.children && row.children.length > 0)
        return this.$modal.notifyError("当前分类存在子分类,无法删除!");
      if (row.id) {
        this.deleteItem(
          row.id,
          delPartition,
          this.getList,
          "分区编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 启用禁用
    handleEnabledOrDisabled(item, type) {
      let api = type == "启用" ? enablePartition : disablePartition;
      const that = this;
      this.$modal
        .confirm(
          `确定${type}<span style="color:red"> 分区编码为${item.code} </span> 的数据项?`,
          {
            dangerouslyUseHTMLString: true, // 使用HTML片段
          }
        )
        .then(function () {
          api({ code: item.code }).then((result) => {
            if (result.code == 0) {
              that.$modal.msgSuccess(type + "成功");
              that.getList();
              item.status = item.status === 0 ? 1 : 0;
            } else {
              item.status = item.status === 0 ? 0 : 1;
              this.$modal.notifyError(result.message);
            }
          });
        })
        .catch(function () {
          item.status = item.status === 0 ? 0 : 1;
        });
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleAdd":
          this.handleAdd(row);
          break;
        case "handleUpdate":
          this.handleUpdate(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    //点击负责人选择按钮
    handleUserSelect() {
      this.$refs.userSelect.showFlag = true;
      this.$refs.userSelect.selectedId = this.form.charge
        ? this.form.charge
        : undefined;
      this.$refs.userSelect.getList();
      this.$refs.userSelect.getTreeselect();
    },
    //负责人返回
    onUserSelected(row) {
      if (row != undefined && row != null) {
        this.$set(this.form, "charge", row.realName);
      }
    },
    //自动生成编码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "PARTITION_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
        });
      } else {
        if (this.optType == "edit") return (this.form.code = this.enCode);
        this.form.code = null;
      }
    },
  },
};
</script>
<style scoped lang="scss">
::v-deep .el-dialog__body {
  padding: 0 10px !important;
}
::v-deep .el-tabs__content {
  padding: 15px 15px 0px 0 !important;
}
</style>
