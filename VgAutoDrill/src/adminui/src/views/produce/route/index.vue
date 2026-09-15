<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="工艺路线编码" prop="code">
        <el-input
          v-trim
          v-model="queryParams.code"
          placeholder="请输入工艺路线编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="工艺路线名称" prop="name">
        <el-input
          v-trim
          v-model="queryParams.name"
          placeholder="请输入工艺路线名称"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="审批状态" prop="vettingStatus">
        <el-select
          v-model="queryParams.vettingStatus"
          placeholder="请选择"
          @clear="clearQueryParams('vettingStatus')"
          clearable
          style="width: 120px"
        >
          <el-option :label="'已审批'" :value="1"> </el-option>
          <el-option :label="'未审批'" :value="0"> </el-option>
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
          :disabled="hasPermi(['produce:route:add'])"
          >新增</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="warning"
          plain
          icon="el-icon-check"
          :disabled="hasPermi(['produce:route:commit'])"
          @click="handleApproval"
          >批量审批</el-button
        >
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          plain
          type="warning"
          icon="el-icon-refresh-left"
          @click="handleRevoke"
          :disabled="hasPermi(['produce:route:revoke'])"
        >
          批量撤销
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['produce:route:remove'])"
          >批量删除</el-button
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
      :data="routeList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
      v-if="refreshTable"
      :default-expand-all="isExpandAll"
    >
      <el-table-column type="selection" width="55" align="center" />
      <el-table-column type="expand">
        <template slot-scope="scope">
          <div class="expand">
            <el-tabs
              style="margin-right: 10px"
              type="border-card"
              v-for="(item, i) in scope.row.children"
              :key="item.name + i"
              class="item"
            >
              <el-tab-pane :label="item.name">
                <el-table
                  height="200px"
                  v-if="item.name == '产品大类'"
                  :data="item.list"
                >
                  <el-table-column
                    label="产品大类编码"
                    prop="productCategoryCode"
                    min-width="180px"
                    show-overflow-tooltip
                  />
                  <el-table-column
                    label="产品大类名称"
                    min-width="180px"
                    prop="productCategoryName"
                    show-overflow-tooltip
                  />
                </el-table>
                <el-table
                  v-else
                  height="200px"
                  :data="item.list"
                  style="width: 100%"
                  border
                >
                  <el-table-column
                    label="工作站编码"
                    prop="workStationCode"
                    min-width="150px"
                    show-overflow-tooltip
                  />
                  <el-table-column
                    label="工作站名称"
                    prop="workStationName"
                    min-width="180px"
                    show-overflow-tooltip
                  />
                </el-table>
              </el-tab-pane>
            </el-tabs>
          </div>
        </template>
      </el-table-column>
      <el-table-column
        label="工艺路线编码"
        key="code"
        prop="code"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[0].visible"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['produce:route:view']"
            >{{ scope.row.code }}</span
          >
        </template>
      </el-table-column>
      <el-table-column
        label="工艺路线名称"
        key="name"
        prop="name"
        show-overflow-tooltip
        min-width="150px"
        v-if="columns[1].visible"
      />
      <el-table-column
        label="工艺路线说明"
        min-width="150px"
        key="routeDesc"
        prop="routeDesc"
        v-if="columns[2].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.routeDesc" />
        </template>
      </el-table-column>
      <el-table-column
        label="是否启用"
        align="center"
        key="status"
        prop="status"
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.status == 1">是</el-tag>
          <el-tag type="danger" v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="审批状态"
        align="center"
        key="vettingStatus"
        prop="vettingStatus"
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.vettingStatus == 1">已审批</el-tag>
          <el-tag type="danger" v-else>未审批</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="备注"
        min-width="150px"
        key="remark"
        prop="remark"
        v-if="columns[5].visible"
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
            :disabled="hasPermi(['produce:route:edit'])"
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
                command="handleApproval"
                icon="el-icon-check"
                :disabled="
                  hasPermi(['produce:route:commit']) ||
                  scope.row.vettingStatus != 0
                "
                >审批</el-dropdown-item
              >
              <el-dropdown-item
                command="handleRevoke"
                icon="el-icon-refresh-left"
                :disabled="
                  hasPermi(['produce:route:revoke']) ||
                  scope.row.vettingStatus != 1
                "
                >撤销</el-dropdown-item
              >
              <el-dropdown-item
                command="handleDelete"
                icon="el-icon-delete"
                :disabled="
                  hasPermi(['produce:route:remove']) ||
                  scope.row.vettingStatus == 1
                "
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

    <!-- 添加或修改工艺路线对话框 -->
    <edit-form-dialog
      v-model="open"
      :title="title"
      :optType="optType"
      :isFormModified="isFormModified"
      @submitForm="submitForm"
    >
      <el-form ref="form" :model="form" :rules="rules" label-width="100px">
        <el-row>
          <el-col :span="12">
            <el-form-item label="路线编码" prop="code">
              <el-input
                v-model="form.code"
                placeholder="请输入工艺路线编码"
                :disabled="optType == 'view' || form.vettingStatus == 1"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="路线名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入工艺路线名称" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item
              label="是否启用"
              prop="status"
              v-if="title == '修改工艺路线'"
            >
              <el-radio-group
                v-removeAriaHidden
                v-model="form.status"
                :disabled="optType == 'view' || form.vettingStatus == 1"
              >
                <el-radio :label="1">是</el-radio>
                <el-radio :label="0">否</el-radio>
              </el-radio-group>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="路线说明" prop="routeDesc">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.routeDesc"
                type="textarea"
                placeholder="请输入内容"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="备注" prop="remark">
              <el-input
                :disabled="optType == 'view'"
                v-model="form.remark"
                type="textarea"
                placeholder="请输入内容"
                :autosize="{ minRows: 2, maxRows: 10 }"
                :resize="optType == 'view' ? 'none' : ''"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <el-tabs
        type="border-card"
        v-if="form.id != null && open"
        @tab-click="tabClick"
      >
        <!-- 关联组成工序 -->
        <el-tab-pane label="组成工序">
          <RouteAndProcess
            ref="routeAndProcess"
            v-if="form.id != null"
            :parentOptType="optType"
            :routeId="form.id"
          ></RouteAndProcess>
        </el-tab-pane>
        <!-- 关联产品大类 -->
        <el-tab-pane label="关联产品大类">
          <RouteAndProductCategory
            ref="routeAndProductCategory"
            v-if="form.id != null"
            :routeId="form.id"
            :optType="optType"
          ></RouteAndProductCategory>
        </el-tab-pane>
      </el-tabs>
    </edit-form-dialog>
  </div>
</template>

<script>
import {
  listRoute,
  getRoute,
  delRoute,
  addRoute,
  updateRoute,
  delList,
  vettingRoute,
  cancelVettingRoute,
} from "@/api/produce/route";
// 关联产品大类
import RouteAndProductCategory from "./routeAndProductCategory";
// 关联组成工序
import RouteAndProcess from "./routeAndProcess";
export default {
  name: "Proroute", 
  components: { RouteAndProductCategory, RouteAndProcess },
  data() {
    return {
      tableData: [
        {
          typeId: 1,
          id: "12987122",
          name: "typeId1",
        },
        {
          typeId: 2,
          id: "12987123",
          name: "typeId2",
        },
        {
          typeId: 3,
          id: "12987125",
          name: "typeId3",
        },
        {
          typeId: 4,
          id: "12987126",
          name: "typeId4",
        },
      ],
      page: "route",
      //自动生成编码
      autoGenFlag: false,
      optType: undefined,
      // 遮罩层
      loading: true,
      // 是否展开，默认全部展开
      isExpandAll: false,
      // 重新渲染表格状态
      refreshTable: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 工艺路线表格数据
      routeList: [],
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
        vettingStatus: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        code: [
          { required: true, message: "工艺路线编码不能为空", trigger: "blur" },
        ],
        name: [
          { required: true, message: "工艺路线名称不能为空", trigger: "blur" },
        ],
        status: [
          { required: true, message: "是否启用不能为空", trigger: "blur" },
        ],
      },
      // 工艺路线Id
      routeId: undefined,
      // 列信息
      columns: [
        { key: 0, label: "工艺路线编码", visible: true },
        { key: 1, label: "工艺路线名称", visible: true },
        { key: 2, label: "工艺路线说明", visible: true },
        { key: 3, label: "是否启用", visible: true },
        { key: 4, label: "审批状态", visible: true },
        { key: 5, label: "备注", visible: true },
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
    open: {
      handler(val) {
        if (!val) {
          this.getList();
        }
      },
    },
  },
  methods: {
    /** 查询工艺路线列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listRoute(this.queryParams);
      this.routeList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    //监听row-dblclick事件，实现选中
    rowDblclick(row, column, event) {
      // 点击展开图标禁止双击逻辑
      if (column.type == "expand") return;
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
        routeDesc: "",
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
      this.handleQuery();
    },
    /** 展开/折叠操作 */
    toggleExpandAll() {
      this.refreshTable = false;
      this.isExpandAll = !this.isExpandAll;
      this.$nextTick(() => {
        this.refreshTable = true;
      });
    },
    /** 新增按钮操作 */
    handleAdd() {
      this.reset();
      this.open = true;
      this.title = "添加工艺路线";
      this.optType = "add";
      this.initialForm = Object.assign({}, this.form);
    },
    // 查询明细按钮操作
    handleView(id) {
      this.reset();
      getRoute(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.title = "查看工艺线路信息";
          this.optType = "view";
          this.open = true;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const id = row.id || this.ids;
      this.routeId = row.id;
      getRoute(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.initialForm = Object.assign({}, res.data);
          this.title = "修改工艺路线";
          this.optType = "edit";
          this.open = true;
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击审批
    async handleApproval(row) {
      if (!row.id && this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行审批？")
        .catch(() => {});
      if (results == "confirm") {
        const vettingRoutes = row.id
          ? [
              {
                routeId: row.id,
              },
            ]
          : this.ids.map((v) => {
              return { routeId: v };
            });
        vettingRoute({ vettingRoutes }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("审批成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 点击撤销
    async handleRevoke(row) {
      if (!row.id && this.ids.length <= 0)
        return this.$modal.msgWarning("请选择要操作的数据!");
      const results = await this.$modal
        .confirm("确认执行撤销？")
        .catch(() => {});
      if (results == "confirm") {
        const vettingRoutes = row.id
          ? [
              {
                routeId: row.id,
              },
            ]
          : this.ids.map((v) => {
              return { routeId: v };
            });
        cancelVettingRoute({ vettingRoutes }).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("撤销成功");
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      }
    },
    // 更多操作触发
    handleCommand(command, row) {
      switch (command) {
        case "handleApproval":
          this.handleApproval(row);
          break;
        case "handleRevoke":
          this.handleRevoke(row);
          break;
        case "handleDelete":
          this.handleDelete(row);
          break;
        default:
          break;
      }
    },
    /** 提交按钮 */
    submitForm() {
      if (this.form.id != null) {
        updateRoute(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addRoute(this.form).then((res) => {
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
          delRoute,
          this.getList,
          "路线编码为" + row.code
        );
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    // 点击标签页获取数据
    tabClick(val) {
      if (val._props.label == "关联产品大类") {
        this.$refs.routeAndProductCategory.getList();
      } else {
        this.$refs.routeAndProcess.getList();
      }
    },
    //自动生成编码
    async handleAutoGenChange(autoGenFlag) {
      if (autoGenFlag) {
        this.getEncode({
          rulesCode: "ROUTE_CODE",
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
<style lang="scss" scoped>
::v-deep .el-table__body-wrapper {
  .el-table__expanded-cell {
    z-index: 100;
  }
}
::v-deep .el-table__fixed,
.el-table__fixed-right {
  .el-table__expanded-cell {
    visibility: hidden;
    padding: 0;
  }
}

.expand {
  padding: 0 10px;
  overflow-x: auto;
  display: flex;
  width: calc(100%);
  background: #fff;
}
</style>
