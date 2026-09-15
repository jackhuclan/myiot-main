<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="工作站编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入工作站编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工作站名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入工作站名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="所在车间" prop="workshopCode">
        <el-select
          v-model="queryParams.workshopCode"
          placeholder="请选择"
          @clear="clearQueryParams('workshopCode')"
          clearable
        >
          <el-option
            v-for="item in workShopOptions"
            :key="item.id"
            :value="item.code"
            :label="item.label"
            v-optionTitle
          ></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="所属工序" prop="processCode">
        <el-select
          v-model="queryParams.processCode"
          placeholder="请选择"
          @clear="clearQueryParams('processCode')"
          clearable
        >
          <el-option
            v-for="item in processOptions"
            :key="item.id"
            :value="item.code"
            :label="item.label"
            v-optionTitle
          ></el-option>
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
          :disabled="hasPermi(['masterData:workstation:add'])"
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
          :disabled="hasPermi(['masterData:workstation:remove'])"
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
          :disabled="hasPermi(['masterData:workstation:import'])"
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
      :data="workStationList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <!-- 点即可进行查看 -->
      <el-table-column
        label="工作站编码"
        show-overflow-tooltip
        key="code"
        prop="code"
        min-width="200px"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['masterData:workstation:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="工作站名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        v-if="columns[1].visible"
        min-width="200px"
      />
      <el-table-column
        label="所属工序"
        key="processCode"
        prop="processCode"
        show-overflow-tooltip
        v-if="columns[2].visible"
        min-width="180"
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
        label="所在车间"
        key="workshopCode"
        prop="workshopCode"
        show-overflow-tooltip
        v-if="columns[3].visible"
        min-width="180"
      >
        <template slot-scope="scope">
          {{
            scope.row.workshopCode && scope.row.workshopName
              ? scope.row.workshopCode + "---" + scope.row.workshopName
              : ""
          }}
        </template>
      </el-table-column>
      <el-table-column
        label="状态"
        align="center"
        key="status"
        prop="status"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <dict-tag
            :options="dict.type.sys_normal_disable"
            :value="scope.row.status"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        align="center"
        key="remark"
        prop="remark"
        v-if="columns[5].visible"
        min-width="150"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.remark" />
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
            :disabled="hasPermi(['masterData:workstation:edit'])"
            >修改</el-button
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
                command="handleConfig"
                icon="el-icon-magic-stick"
                :disabled="hasPermi(['masterData:workstation:config'])"
                >配置路线</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['masterData:workstation:remove'])"
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

    <!-- 添加或修改工作站对话框 -->
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
          <el-col :span="10">
            <el-form-item label="工作站编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入工作站编码"
                :disabled="autoGenFlag || optType != 'add'"
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
            <el-form-item label="工作站名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入工作站名称" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="所在车间" prop="workshopId">
              <el-select
                v-model="form.workshopId"
                placeholder="请选择车间"
                @change="handleChangeWorkshop"
              >
                <el-option
                  v-for="item in workShopOptions.filter((v) => v.status != 0)"
                  :key="item.id"
                  :value="item.id"
                  :label="item.label"
                  v-optionTitle
                ></el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="所属工序" prop="processId">
              <el-select
                v-model="form.processId"
                placeholder="请选择所属工序"
                @change="handleChangeProcess"
                clearable
                @clear="clearProcess"
              >
                <el-option
                  v-for="item in processOptions"
                  :key="item.id"
                  :label="item.label"
                  :value="item.id"
                  v-optionTitle
                ></el-option>
              </el-select>
            </el-form-item> </el-col
          ><el-col :span="8" v-if="title == '修改工作站'">
            <el-form-item label="状态" prop="status">
              <el-radio-group v-removeAriaHidden v-model="form.status">
                <el-radio :label="1">正常</el-radio>
                <el-radio :label="0">停用</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="备注" prop="remark">
              <el-input
                v-model="form.remark"
                placeholder="请输入内容"
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <el-tabs
        type="border-card"
        v-if="form.id != null && form.processId && optType == 'view'"
      >
        <el-tab-pane label="关联工艺路线">
          <workStationAndRoute
            :optType="optType"
            :workStationId="form.id"
            ref="workStationAndRoute"
          />
        </el-tab-pane>
      </el-tabs>
    </edit-form-dialog>
    <!-- 配置路线 -->
    <configRoute ref="configRoute" />
    <!-- 工作站导入 -->
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
  listWorkstation,
  getWorkstation,
  delWorkstation,
  addWorkstation,
  updateWorkstation,
  delList,
} from "@/api/masterData/workStation";
import { getDropSelectDatas as getProcessSelectDatas } from "@/api/produce/process";
import workStationAndRoute from "./workStationAndRoute.vue";
import configRoute from "./configRoute.vue";
import { getDropSelectDatas } from "@/api/masterData/workShop";
export default {
  name: "Workstation",
  dicts: ["sys_normal_disable"],
  components: { workStationAndRoute, configRoute },
  data() {
    return {
      page: "workStation",
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
      // 工作站表格数据
      workStationList: [],
      //车间选项
      workShopOptions: [],
      //工序选项
      processOptions: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      addOpen: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        code: undefined,
        name: undefined,
        // 车间Id
        workshopId: undefined,
        // 车间编码
        workshopCode: undefined,
        // 车间名车
        workshopName: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "工作站编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "工作站名称不能为空", trigger: "blur" },
        ],
        status: [{ required: true, message: "状态不能为空", trigger: "blur" }],
      },
      // 工作站导入参数
      // 导入的url
      uploadUrl: "/v1/Workstation/UploadList",
      // 下载的url
      downloadUrl: "/v1/Workstation/DownLoad",
      // 导入的模板下载名
      fileName: "工作站模板.xlsx",
      // 列信息
      columns: [
        { key: 0, label: "工作站编码", visible: true },
        { key: 1, label: "工作站名称", visible: true },
        { key: 2, label: "所属工序", visible: true },
        { key: 3, label: "所在车间", visible: true },
        { key: 4, label: "状态", visible: true },
        { key: 5, label: "备注", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
    this.getWorkShopList();
    this.getProcessList();
  },
  computed: {
    isFormModified() {
      // 对比当前表单值和初始值是否相等，如果相等则未修改，否则已修改
      if (this.optType == "view") return false;
      return JSON.stringify(this.form) !== JSON.stringify(this.initialForm);
    },
  },
  methods: {
    /** 查询工作站列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listWorkstation(this.queryParams);
      this.workStationList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
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
    // 获取车间列表
    getWorkShopList() {
      getDropSelectDatas({ pageNum: 1, pageSize: 1000 }).then((res) => {
        this.workShopOptions = res.data;
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
        workshopId: undefined,
        workshopCode: undefined,
        workshopName: undefined,
        remark: "",
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
      this.resetForm("queryForm");
      this.handleQuery();
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.getWorkShopList();
      this.getProcessList();
      this.open = true;
      this.initialForm = Object.assign({}, this.form);
      this.title = "添加工作站";
      this.optType = "add";
      this.autoGenFlag = false;
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleConfig":
          this.handleConfig(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    // 配置路线
    handleConfig(row) {
      this.reset();
      const workStationId = row.id;
      getWorkstation(workStationId).then((res) => {
        if (res.code == 0) {
          this.$nextTick(() => {
            this.$refs.configRoute.open = true;
            this.$refs.configRoute.form = res.data;
            this.$refs.configRoute.optType = "edit";
            this.$refs.configRoute.form = res.data;
            this.$refs.configRoute.queryParams.workStationId = res.data.id;
            this.$refs.configRoute.processCode = res.data.processCode;
            this.$refs.configRoute.getList();
          });
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getWorkstation(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看工作站信息";
          this.optType = "view";
          this.autoGenFlag = false;
          this.$nextTick(() => {
            if (this.$refs.workStationAndRoute) {
              this.$refs.workStationAndRoute.queryParams.workStationId =
                res.data.id;
              this.$refs.workStationAndRoute.processCode = res.data.processCode;
              this.$refs.workStationAndRoute.getList();
            }
          });
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      this.getWorkShopList();
      this.getProcessList();
      const workStationId = row.id || this.ids;
      getWorkstation(workStationId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.enCode = res.data.code;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改工作站";
          this.optType = "edit";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateWorkstation(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addWorkstation(this.form).then((res) => {
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
    // 删除
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delWorkstation,
          this.getList,
          "工作站编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "工作站导入";
      this.$refs.upload.open = true;
    },

    // 工序下拉框
    handleChangeProcess(val) {
      this.form.processName = this.processOptions.find(
        (v) => v.id == val
      )?.name;
      this.form.processCode = this.processOptions.find(
        (v) => v.id == val
      )?.code;
      this.form.processId = val;
    },
    // 清空工序
    clearProcess() {
      this.form.processName = undefined;
      this.form.processCode = undefined;
      this.form.processId = undefined;
    },
    // 车间下拉框
    handleChangeWorkshop(val) {
      this.form.workshopName = this.workShopOptions.find(
        (v) => v.id == val
      ).name;
      this.form.workshopCode = this.workShopOptions.find(
        (v) => v.id == val
      ).code;
      this.form.workshopId = val;
    },
    //自动生成编码
    handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "WORKSTATION_CODE",
          buildCount: 1,
        }).then((response) => {
          const code = response.data[0];
          this.form.code = code;
        });
      } else {
        if (this.optType == "edit") return (this.form.code = this.enCode);
        this.form.code = "";
      }
    },
  },
};
</script>
