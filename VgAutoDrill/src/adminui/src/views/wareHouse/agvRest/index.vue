<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="休息点编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入休息点编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="休息点名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入休息点名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="是否可用" prop="status">
        <el-select
          @clear="clearQueryParams('status')"
          v-model="queryParams.status"
          placeholder="请选择"
          clearable
          style="width: 150px"
        >
          <el-option :label="'是'" :value="1" />
          <el-option :label="'否'" :value="0" /></el-select
      ></el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['wareHouse:agvRest:add'])"
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
          :disabled="hasPermi(['wareHouse:agvRest:remove'])"
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
          :disabled="hasPermi(['wareHouse:agvRest:import'])"
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
      :data="agvRestList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column
        label="休息点编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        min-width="120"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['wareHouse:agvRest:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="休息点名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        min-width="120"
        v-if="columns[1].visible"
      />

      <el-table-column
        label="物理点位"
        key="point"
        prop="point"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />

      <el-table-column
        label="预定分配(AGV)"
        key="preBookAgv"
        prop="preBookAgv"
        min-width="120"
        show-overflow-tooltip
        v-if="columns[3].visible"
      />

      <el-table-column
        label="正占用(AGV)"
        min-width="120"
        key="currentAgv"
        prop="currentAgv"
        show-overflow-tooltip
        v-if="columns[4].visible"
      />
      <el-table-column
        label="是否可用"
        min-width="80"
        key="status"
        prop="status"
        align="center"
        v-if="columns[5].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1"> 是 </el-tag>
          <el-tag v-else type="danger">否</el-tag>
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
              hasPermi(['warehouse:agvRest:enabled']) || scope.row.status != 0
            "
            >启用</el-button
          >
          <el-button
            type="text"
            icon="el-icon-remove-outline"
            @click="handleEnabledOrDisabled(scope.row, '禁用')"
            :disabled="
              hasPermi(['warehouse:agvRest:disabled']) || scope.row.status != 1
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
                command="handleUpdate"
                icon="el-icon-edit"
                :disabled="hasPermi(['wareHouse:agvRest:edit'])"
                >修改</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="hasPermi(['wareHouse:agvRest:remove'])"
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
            <el-form-item label="休息点编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入休息点编码"
                :disabled="optType == 'view'"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="休息点名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入休息点名称" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="物理点位" prop="point">
              <el-input v-model="form.point" placeholder="请输入物理点位" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">
            <el-form-item
              v-if="form.id != null"
              label-width="130px"
              label="预定分配的AGV"
              prop="preBookAgv"
            >
              <el-input
                v-model="form.preBookAgv"
                placeholder="请输入预定分配的AGV"
              />
            </el-form-item>
          </el-col>

          <el-col :span="8">
            <el-form-item
              v-if="form.id != null"
              label="正占用的AGV"
              label-width="120px"
              prop="currentAgv"
            >
              <el-input
                v-model="form.currentAgv"
                placeholder="请输入正占用的AGV"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item v-if="form.id != null" label="是否启用">
              <el-radio-group
                v-removeAriaHidden
                v-model="form.status"
                :disabled="optType == 'view'"
              >
                <el-radio :label="1">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </edit-form-dialog>
    <!-- 休息点导入 -->
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
  listAgvRest,
  getAgvRest,
  updateAgvRest,
  delAgvRest,
  delList,
  addAgvRest,
  enableAgvRest,
  disableAgvRest,
} from "@/api/wareHouse/agvRest";
export default {
  name: "AgvRest",
  data() {
    return {
      page: "agvRest",
      //自动生成编码
      autoGenFlag: false,

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
      agvRestList: [],

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
        routeCode: undefined,
        agvDeviceKind: undefined,
        point: undefined,
        priority: undefined,
        preBookAgv: undefined,
        currentAgv: undefined,
        status: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "休息点编码不能为空", trigger: "change" },
        ],
        name: [
          { required: true, message: "休息点名称不能为空", trigger: "blur" },
        ],
        point: [
          { required: true, message: "物理点位不能为空", trigger: "blur" },
        ],
      },
      // 休息点导入参数
      // 导入的url
      uploadUrl: "/v1/AgvRest/UploadList",
      // 下载url
      downloadUrl: "/v1/AgvRest/DownLoad",
      // 导入的模板下载名
      fileName: "休息点模板.xlsx",
      // 列信息，
      columns: [
        { key: 0, label: "休息点编码", visible: true },
        { key: 1, label: "休息点名称", visible: true },
        { key: 2, label: "物理点位", visible: true },
        { key: 3, label: "预定分配(AGV)", visible: true },
        { key: 4, label: "正占用(AGV)", visible: true },
        { key: 5, label: "是否可用", visible: true },
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
  methods: {
    /** 查询休息点列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listAgvRest(this.queryParams);
      this.agvRestList = res.data.list;
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
        name: "",
        partCode: "000",
        partName: "全部",
        routeCode: "",
        agvDeviceKind: undefined,
        point: "",
        priority: 1,
        preBookAgv: "",
        currentAgv: "",
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
    /** 导入按钮操作 */
    handleImport() {
      this.$refs.upload.title = "休息点导入";
      this.$refs.upload.open = true;
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.title = "添加休息点";
      this.initialForm = Object.assign({}, this.form);
      this.optType = "add";
      this.autoGenFlag = false;
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getAgvRest(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.open = true;
          this.title = "查看休息点";
          this.optType = "view";
          this.autoGenFlag = false;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const agvRestId = row.id || this.ids;
      getAgvRest(agvRestId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.enCode = res.data.code;
          this.initialForm = Object.assign({}, res.data);
          this.open = true;
          this.title = "修改休息点";
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
        updateAgvRest(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addAgvRest(this.form).then((res) => {
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
    // 启用禁用
    handleEnabledOrDisabled(item, type) {
      let api = type == "启用" ? enableAgvRest : disableAgvRest;
      const that = this;
      this.$modal
        .confirm(
          `确定${type}<span style="color:red"> 休息点编码为${item.code} </span> 的数据项?`,
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
    handleDelete(row) {
      if (row.id) {
        this.deleteItem(
          row.id,
          delAgvRest,
          this.getList,
          "休息点编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
  },
};
</script>
