<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="料仓编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          @keyup.enter.native="handleQuery"
          placeholder="请输入料仓编码"
        ></el-input>
      </el-form-item>
      <el-form-item label="供应商" prop="vendorCode">
        <el-input
          v-trim
          v-model="queryParams.vendorCode"
          @keyup.enter.native="handleQuery"
          placeholder="请输入供应商编码"
        ></el-input>
      </el-form-item>
      <el-form-item label="状态" prop="siloStatus">
        <el-select
          @clear="clearQueryParams('siloStatus')"
          v-model="queryParams.siloStatus"
          placeholder="请选择"
          clearable
          style="width: 120px"
        >
          <el-option label="手动" :value="0" />
          <el-option label="就绪" :value="1" />
          <el-option label="自动" :value="2" />
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
          :disabled="hasPermi(['warehouse:siloManage:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAddInBulk"
          :disabled="hasPermi(['warehouse:siloManage:add'])"
          >批量新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-upload2"
          @click="handleImport"
          :disabled="hasPermi(['warehouse:siloManage:import'])"
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
          :disabled="hasPermi(['warehouse:siloManage:remove'])"
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
      :data="siloManageList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="料仓编码"
        key="code"
        prop="code"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['warehouse:siloManage:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="状态"
        align="center"
        key="siloStatus"
        prop="siloStatus"
        v-if="columns[1].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.siloStatus == -1" type="info">未知</el-tag>
          <el-tag v-else-if="scope.row.siloStatus == 0">手动</el-tag>
          <el-tag v-else-if="scope.row.siloStatus == 1" type="warning"
            >就绪</el-tag
          >
          <el-tag v-else type="success">自动</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="运载尺寸"
        key="size"
        prop="size"
        min-width="120"
        align="center"
        v-if="columns[2].visible"
      />
      <el-table-column
        label="层数"
        key="floorCount"
        prop="floorCount"
        align="center"
        v-if="columns[3].visible"
      />
      <el-table-column
        label="供应商"
        key="vendorCode"
        prop="vendorCode"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />

      <el-table-column
        label="操作"
        align="center"
        fixed="right"
        min-width="240px"
        class-name="small-padding fixed-width"
      >
        <template slot-scope="scope">
          <el-button
            type="text"
            @click="handleSet('手动', scope.row)"
            icon="el-icon-setting"
            :disabled="
              hasPermi(['warehouse:siloManage:setManual']) ||
              scope.row.siloStatus != 1
            "
            >设置手动</el-button
          >
          <el-button
            type="text"
            @click="handleSet('就绪', scope.row)"
            icon="el-icon-setting"
            :disabled="
              hasPermi(['warehouse:siloManage:setReady']) ||
              scope.row.siloStatus != 0
            "
            >设置就绪</el-button
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
                icon="el-icon-edit"
                command="handleUpdate"
                :disabled="hasPermi(['warehouse:siloManage:edit'])"
                >修改</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['warehouse:siloManage:remove'])"
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

    <!-- 添加或修改料仓对话框 -->
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
        <el-row v-if="title == '批量添加料仓信息'">
          <el-col :span="8">
            <el-form-item label="料仓数量" prop="addNum">
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.addNum"
                @changeNum="changeNum"
                :numName="'addNum'"
                :min="1"
              />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item label="运载尺寸" prop="size">
              <el-select
                @clear="clearQueryParams('size')"
                v-model="form.size"
                placeholder="请选择"
                clearable
              >
                <el-option label="500x500" value="500x500" />
                <el-option label="1000x500" value="1000x500" />
                <el-option label="1500x500" value="1500x500" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="层数" prop="floorCount">
              <!-- <el-input v-model="form.floorCount" placeholder="请输入层数" />
                -->
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.floorCount"
                @changeNum="changeNum"
                :numName="'floorCount'"
                :min="1"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row v-else>
          <el-col :span="12" style="display: flex">
            <el-form-item label="料仓编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入料仓编码"
                :disabled="autoGenFlag || optType != 'add'"
              />
            </el-form-item>
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
          <el-col :span="8">
            <el-form-item label="运载尺寸" prop="size">
              <el-select
                @clear="clearQueryParams('size')"
                v-model="form.size"
                placeholder="请选择"
                clearable
              >
                <el-option label="500x500" value="500x500" />
                <el-option label="1000x500" value="1000x500" />
                <el-option label="1500x500" value="1500x500" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="层数" prop="floorCount">
              <!-- <el-input v-model="form.floorCount" placeholder="请输入层数" />
                -->
              <!-- 引入自定义计数器组件 -->
              <input-number
                :myNum="form.floorCount"
                @changeNum="changeNum"
                :numName="'floorCount'"
                :min="1"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="供应商">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.vendorCode"
                placeholder="请输入供应商编码"
              >
                <el-button
                  v-debounce
                  :disabled="optType == 'view'"
                  slot="append"
                  @click="handleSelectVendor"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <VendorSelect ref="vendorSelect" @onSelected="onVendorSelected">
              </VendorSelect>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row v-if="title == '批量添加料仓信息'">
          <el-col :span="8">
            <el-form-item label="供应商">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.vendorCode"
                placeholder="请输入供应商编码"
              >
                <el-button
                  v-debounce
                  :disabled="optType == 'view'"
                  slot="append"
                  @click="handleSelectVendor"
                  icon="el-icon-search"
                ></el-button>
              </el-input>
              <VendorSelect ref="vendorSelect" @onSelected="onVendorSelected">
              </VendorSelect>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
    <setForm ref="setForm" :api="true"> </setForm>
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
import {
  listSilo,
  addSilo,
  updateSilo,
  delSilo,
  delList,
  getSilo,
  unBindAllPanel,
  setManual,
  setReady,
} from "@/api/wareHouse/silo";
import setForm from "../components/changePanel.vue";
// 客户选择
import VendorSelect from "@/components/vendorSelect";
export default {
  name: "siloManage",
  components: { setForm, VendorSelect },
  data() {
    const validateCount = (rule, value, callback) => {
      if (this.form.floorCount == 0) {
        callback(new Error("层数不能为0"));
      }
      callback();
    };
    return {
      page: "siloManage",
      //自动生成编码
      autoGenFlag: false,
      enCode: "",
      optType: "",
      detailOptType: "",
      // 遮罩层
      loading: false,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 料仓表格数据
      siloManageList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        size: undefined,
        floorCount: 0,
        vendorCode: undefined,
        vendorName: undefined,
        location: undefined,
        emptySilo: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "料仓编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "料仓名称不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "是否启用不能为空", trigger: "blur" },
        ],
        floorCount: [
          { required: true, validator: validateCount, trigger: "change" },
        ],
        location: [
          { required: true, message: "请输入位置", trigger: "change" },
        ],
      },
      // 列信息，
      columns: [
        { key: 0, label: "料仓编码", visible: true },
        { key: 1, label: "状态", visible: true },
        { key: 2, label: "运载尺寸", visible: true },
        { key: 3, label: "层数", visible: true },
        { key: 4, label: "供应商", visible: true },
      ],
      // 料仓导入参数
      // 导入的url
      uploadUrl: "/v1/Silo/Upload",
      downloadUrl: "/v1/Silo/DownLoad",
      // 导入的模板下载名
      fileName: "料仓模板.xlsx",
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
    "form.vendorName": {
      handler(val) {
        if (val == "") {
          this.form.vendorCode = "";
          this.form.vendorName = "";
          this.form.vendorId = undefined;
        }
      },
    },
    "form.vendorCode": {
      handler(val) {
        if (val == "") {
          this.form.vendorCode = "";
          this.form.vendorName = "";
          this.form.vendorId = undefined;
        }
      },
    },
  },
  methods: {
    /** 查询料仓列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listSilo(this.queryParams);
      this.siloManageList = res.data.list;
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
        code: "",
        size: "",
        vendorCode: "",
        vendorName: "",
        floorCount: 1,
        location: "",
        addNum: 1,
      };
      this.autoGenFlag = false;
      this.$refs["form"]?.resetFields();
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
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.title = "添加料仓信息";
      this.optType = "add";
      this.autoGenFlag = false;
      this.initialForm = Object.assign({}, this.form);
    },
    // 批量新增
    handleAddInBulk() {
      // this.$modal.msgError("正在开发.....");
      // return;

      this.open = true;
      this.title = "批量添加料仓信息";
      this.reset();
      this.optType = "addAll";
      this.initialForm = Object.assign({}, this.form);
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "料仓导入";
      this.$refs.upload.open = true;
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      this.form.addNum = undefined;
      const slioId = row.id || this.ids;
      getSilo(slioId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "修改料仓信息";
          this.optType = "edit";
          this.autoGenFlag = false;
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateSilo(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addSilo(this.form).then((res) => {
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
        this.deleteItem(row.id, delSilo, this.getList, "料仓编码为" + row.code);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 设置手动/就绪
    async handleSet(label, row) {
      const api = label == "手动" ? setManual : setReady;
      const res = await this.$modal
        .confirm("确定将当前操作的数据项设置" + label + "？")
        .catch(() => {});
      if (res) {
        api({ siloCode: row.code }).then((result) => {
          if (result.code == 0) {
            this.$modal.msgSuccess("设置" + label + "成功");
            row.siloStatus = label == "手动" ? 0 : 1;
            this.getList();
          } else {
            this.$modal.notifyError(result.message);
          }
        });
      }
    },

    // 查看明细操作
    handleView(id) {
      getSilo(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看料仓信息";
          this.optType = "view";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },

    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
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

    //供应商选择弹出框
    handleSelectVendor() {
      this.$refs.vendorSelect.showFlag = true;
      this.$refs.vendorSelect.selectedVendorId = this.form.vendorId
        ? this.form.vendorId
        : undefined;
      this.$refs.vendorSelect.getList();
    },
    onVendorSelected(obj) {
      if (obj != undefined && obj != null) {
        this.$set(this.form, "vendorId", obj.id);
        this.$set(this.form, "vendorCode", obj.code);
        this.$set(this.form, "vendorName", obj.name);
      }
    },
    //自动生成码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "SILO_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
        });
      } else {
        if (this.optType == "edit") {
          this.form.code = this.enCode;
          return;
        }
        this.form.code = "";
      }
    },
  },
};
</script>
