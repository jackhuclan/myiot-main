<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="分区编码" prop="partCode">
        <el-input
          v-trim
          v-model="queryParams.partCode"
          placeholder="请输入分区编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="分区名称" prop="partName">
        <el-input
          v-trim
          v-model="queryParams.partName"
          placeholder="请输入分区名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="休息点" prop="restCode">
        <el-input
          v-trim
          v-model="queryParams.restCode"
          placeholder="请输入休息点"
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
          :disabled="hasPermi(['wareHouse:agvRestAndPart:add'])"
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
          :disabled="hasPermi(['wareHouse:agvRestAndPart:remove'])"
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
          :disabled="hasPermi(['wareHouse:agvRestAndPart:import'])"
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
      border
      :ref="page"
      v-loading="loading"
      :data="agvRestAndPartList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />

      <el-table-column
        label="分区编码"
        min-width="120px"
        key="partCode"
        prop="partCode"
        show-overflow-tooltip
        v-if="columns[0].visible"
      />
      <el-table-column
        label="分区名称"
        min-width="120px"
        key="partName"
        prop="partName"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />
      <el-table-column
        label="休息点"
        key="restCode"
        prop="restCode"
        min-width="120px"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="休息点状态"
        min-width="100"
        key="restStatus"
        prop="restStatus"
        align="center"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.restStatus == 1"> 启用 </el-tag>
          <el-tag v-else type="danger">禁用</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="物理点位"
        key="point"
        prop="point"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />
      <el-table-column
        label="工艺路线"
        key="routeCode"
        prop="routeCode"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        label="预约AGV"
        key="preBookAgv"
        prop="preBookAgv"
        show-overflow-tooltip
        min-width="120px"
        v-if="columns[6].visible"
      />
      <el-table-column
        label="当前AGV"
        key="currentAgv"
        prop="currentAgv"
        show-overflow-tooltip
        v-if="columns[7].visible"
        min-width="120px"
      />
      <el-table-column
        label="AGV类型"
        key="agvDeviceKind"
        prop="agvDeviceKind"
        min-width="100px"
        align="center"
        v-if="columns[8].visible"
      >
        <template slot-scope="scope">
          {{
            $status.deviceKinds.find((v) => v.value == scope.row.agvDeviceKind)
              ? $status.deviceKinds.find(
                  (v) => v.value == scope.row.agvDeviceKind
                ).label
              : ""
          }}
        </template></el-table-column
      >

      <el-table-column
        label="优先级"
        key="priority"
        prop="priority"
        min-width="60px"
        align="center"
        v-if="columns[9].visible"
      />

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
            :disabled="hasPermi(['wareHouse:agvRestAndPart:edit'])"
            >修改</el-button
          >
          <el-button
            type="text"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"
            :disabled="hasPermi(['wareHouse:agvRestAndPart:remove'])"
            >删除</el-button
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

    <!-- 添加或修改休息点对话框 -->
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
          <el-col :span="8">
            <el-form-item label="分区编码" prop="partCode">
              <treeselect
                :class="$store.getters.size + '-treeselect'"
                :disabled="optType == 'view'"
                v-model="form.partCode"
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
            <el-form-item label="分区名称" prop="partName">
              <el-input
                disabled
                v-model="form.partName"
                placeholder="请输入分区名称"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="休息点" prop="restCode">
              <el-autocomplete
                :disabled="optType == 'view'"
                v-model="form.restCode"
                :fetch-suggestions="querySearchAsync"
                placeholder="请输入内容"
                @select="onAgvRestSelected"
                popper-class="el-autocomplete-suggestion"
                :popper-append-to-body="false"
                :debounce="0"
              >
                <el-button
                  :disabled="optType == 'view'"
                  v-debounce
                  slot="append"
                  @click="handleSelectAgvRest"
                  icon="el-icon-search"
                ></el-button
              ></el-autocomplete>
              <AgvRestSelect
                ref="AgvRestSelect"
                @onSelected="onAgvRestSelected"
              >
              </AgvRestSelect>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="工艺路线" prop="routeCode">
              <el-select
                ref="select3"
                v-model="form.routeCode"
                placeholder="请选择工艺路线"
              >
                <el-option
                  v-for="item in routeSelectOptions"
                  :key="item.id"
                  :label="item.code"
                  :value="item.code"
                  v-optionTitle
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="AGV类型" prop="agvDeviceKind">
              <el-select
                ref="select1"
                v-model="form.agvDeviceKind"
                placeholder="请选择AGV类别"
              >
                <el-option label="后上料AGV" :value="6" />
                <el-option label="运料AGV" :value="10" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="优先级" prop="priority">
              <input-number
                :dis="optType == 'view'"
                :myNum="form.priority"
                @changeNum="changeNum"
                :numName="'priority'"
                :min="1"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
    <!-- 休息点配置导入 -->
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
import { listRoute } from "@/api/produce/route";
import { listAgvRest } from "@/api/wareHouse/agvRest";
import {
  listAgvRestAndPart,
  getAgvRestAndPart,
  updateAgvRestAndPart,
  addAgvRestAndPart,
  delAgvRestAndPart,
  delList,
} from "@/api/wareHouse/agvRestAndPart";
import { treeselect } from "@/api/wareHouse/partition";
import AgvRestSelect from "@/components/agvRestSelect";
export default {
  name: "AgvRestAndPart",
  components: { AgvRestSelect },

  data() {
    // 自定义校验规则
    const partCodeRule = (rule, value, callback) => {
      if (this.form.partCode == "000") {
        callback(new Error("根分类不可选，请重新选择"));
      }
      if (!value) {
        callback(new Error("请选择分区"));
      } else {
        callback();
      }
    };
    return {
      page: "agvRestAndPart",
      //自动生成编码
      autoGenFlag: false,
      enCode: "",
      optType: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 休息点表格数据
      agvRestAndPartList: [],
      // 分区下拉选项
      wareHouseTypeOptions: [],
      // 工艺路线
      routeSelectOptions: [],
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
        partCode: undefined,
        partName: undefined,
        restCode: undefined,
        routeCode: undefined,
        agvDeviceKind: undefined,
        point: undefined,
        priority: undefined,
        preBookAgv: undefined,
        currentAgv: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        restCode: [
          { required: true, message: "休息点编码不能为空", trigger: "change" },
        ],

        partCode: [
          { required: true, validator: partCodeRule, trigger: "change" },
        ],
        partName: [
          { required: true, message: "分区编码不能为空", trigger: "blur" },
        ],
        agvDeviceKind: [
          { required: true, message: "请选择AGV类型", trigger: "change" },
        ],
        status: [
          { required: true, message: "是否启用不能为空", trigger: "blur" },
        ],
      },
      // 休息点配置导入参数
      // 导入的url
      uploadUrl: "/v1/AgvRestAndPart/UploadList",
      // 下载url
      downloadUrl: "/v1/AgvRestAndPart/DownLoad",
      // 导入的模板下载名
      fileName: "休息点配置模板.xlsx",
      // 列信息，
      columns: [
        { key: 0, label: "分区编码", visible: true },
        { key: 1, label: "分区名称", visible: true },
        { key: 2, label: "休息点", visible: true },
        { key: 3, label: "休息点状态", visible: true },
        { key: 4, label: "物理点位", visible: true },
        { key: 5, label: "工艺路线", visible: true },
        { key: 6, label: "预约AGV", visible: true },
        { key: 7, label: "当前AGV", visible: true },
        { key: 8, label: "AGV类型", visible: true },
        { key: 9, label: "优先级", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
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
  watch: {
    restCode(val) {
      if (!val) {
        this.form.restCode = "";
        this.form.restName = "";
      }
    },
  },
  methods: {
    /** 查询休息点与分区关系列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listAgvRestAndPart(this.queryParams);
      this.agvRestAndPartList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
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
        restCode: "",
        restName: "",
        partCode: "000",
        partName: "全部",
        routeCode: "",
        agvDeviceKind: undefined,
        priority: 1,
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
    /** 查询 分区下拉树结构 */
    getTreeselect() {
      treeselect().then((res) => {
        this.wareHouseTypeOptions = res.data;
      });
    },
    // 查询工艺路线
    async getRoute() {
      const res = await listRoute({ vettingStatus: 1, pageNum: 1, pageSize: 100 });
      this.routeSelectOptions = res.data.list;
    },
    normalizer(node) {
      if (node.children && !node.children.length) {
        delete node.children;
      }

      return {
        id: node.code,

        label: node.code,

        isDisabled: node.status == 0,

        children: node.children,
      };
    },
    // 选择归属分区时同时保存分区code
    wareHouseTypeSelect(val) {
      this.form.partCode = val.code;
      this.form.partName = val.label;
    },
    changeTreeselectOpen() {
      const doms = document.querySelectorAll(".el-dialog .el-select");
      doms.forEach((v, i) => {
        this.$refs[`select` + (i + 1)]?.blur();
      });
    },

    // 休息点
    querySearchAsync(queryString, cb) {
      if (queryString === "" || !queryString) {
        let arr = [];
        cb(arr);
      } else {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
          listAgvRest({
            pageNum: 1,
            pageSize: 1000,
          }).then((res) => {
            if (res.code === 0) {
              let arr = res.data.list.map((v) => {
                return {
                  value: v.code,
                  ...v,
                };
              });
              const showSuggestion = document.querySelector(
                ".el-autocomplete-suggestion"
              );
              cb(arr);
              if (arr.length > 0) {
                showSuggestion.style.display = "block";
              }
            } else {
              this.$modal.notifyError(res.message);
            }
          });
        }, 500);
      }
    },

    //休息点选择弹出框
    handleSelectAgvRest() {
      this.$refs.AgvRestSelect.showFlag = true;
      this.$refs.AgvRestSelect.title = "产品选择";
      this.$refs.AgvRestSelect.selectedAgvRestId = this.form.restCode
        ? this.form.restCode
        : undefined;
      this.$refs.AgvRestSelect.getList();
    },
    onAgvRestSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "restCode", obj.code);
        this.$set(this.form, "restName", obj.name);
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "休息点配置导入";
      this.$refs.upload.open = true;
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.getTreeselect();
      this.getRoute();
      this.open = true;
      this.title = "添加休息点与分区关系";
      this.initialForm = Object.assign({}, this.form);
      this.optType = "add";
      this.autoGenFlag = false;
    },
    // 查询明细按钮操作
    handleView(row) {
      this.reset();
      this.getTreeselect();
      this.getRoute();
      const agvRestAndPartId = row.id || this.ids;
      getAgvRestAndPart(agvRestAndPartId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看休息点与分区关系";
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
      this.getRoute();
      const agvRestAndPartId = row.id || this.ids;
      getAgvRestAndPart(agvRestAndPartId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.initialForm = Object.assign({}, res.data);

          this.title = "修改休息点与分区关系";
          this.optType = "edit";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateAgvRestAndPart(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addAgvRestAndPart(this.form).then((res) => {
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
        this.deleteItem(
          row.id,
          delAgvRestAndPart,
          this.getList,
          "分区编码为" + row.partCode
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
  },
};
</script>
