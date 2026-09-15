<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="板料编码" prop="panelCode">
        <el-input
          v-trim
          v-model="queryParams.panelCode"
          placeholder="请输入板料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料编码" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入物料编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="批次号" prop="batchCode">
        <el-input
          v-trim
          v-model="queryParams.batchCode"
          placeholder="请输入批次号"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>

      <el-form-item label="产品状态" prop="productStatusList">
        <el-select
          v-model="queryParams.productStatusList"
          placeholder="请选择"
          multiple
          collapse-tags
          :class="
            queryParams.productStatusList &&
            queryParams.productStatusList.length >= 2
              ? 'select-hastags'
              : ''
          "
        >
          <el-option
            v-for="item in productStatusOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
            v-optionTitle
          />
        </el-select>
      </el-form-item>
      <el-form-item label="库位编码" prop="locationCode">
        <el-input
          v-trim
          v-model="queryParams.locationCode"
          placeholder="请输入库位编码"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
    </search-form>
    <el-row :gutter="10" class="mb8">
      <!-- <el-col :span="1.5">
        <el-button
          v-debounce
          type="primary"
          plain
          icon="el-icon-plus"
          @click="handleAdd"
          :disabled="hasPermi(['warehouse:board:add'])"
          >新增</el-button
        >
      </el-col> -->

      <!-- <el-col :span="1.5">
        <el-button
          v-debounce
          type="danger"
          plain
          icon="el-icon-delete"
          @click="handleDelete"
          :disabled="hasPermi(['warehouse:board:remove'])"
          >删除</el-button
        >
      </el-col> -->
      <el-col :span="1.5">
        <el-button
          v-debounce
          type="info"
          plain
          icon="el-icon-download"
          @click="handleExport"
          :disabled="hasPermi(['warehouse:board:export'])"
          >导出</el-button
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
      :default-expand-all="isExpandAll"
      v-if="refreshTable"
      :data="boardTraceList"
      style="width: 100%"
      :row-class-name="getRowClass"
    >
      <el-table-column type="expand">
        <template slot-scope="props">
          <!-- 展开内容表 -->
          <el-table
            border
            v-if="props.row.children.length > 0"
            header-cell-class-name="childrenHeader"
            :data="props.row.children"
            :header-cell-style="{'text-align':'center'}"
            :cell-style="{'text-align':'center'}"
            row-key="id"
          >
            <!-- <el-table-column
              show-overflow-tooltip
              key="deviceCode"
              prop="deviceCode"
              align="center"
              label="设备编码"
            />
            <el-table-column
              key="position"
              prop="position"
              align="center"
              label="位置Id"
              show-overflow-tooltip
            /> -->
            <el-table-column
              type="index"
              :index="reverseIndex"
              label="序号"
              width="60">
            </el-table-column>
            <el-table-column
              show-overflow-tooltip
              label="板料编码"
              key="panelCode"
              prop="panelCode"
              min-width="150"
            >
            </el-table-column>
            <el-table-column
              show-overflow-tooltip
              label="料仓号"
              key="siloCode"
              prop="siloCode"
              min-width="80"
            >
            </el-table-column>
            <el-table-column
              show-overflow-tooltip
              label="库位编码"
              key="locationCode"
              prop="locationCode"
              min-width="100"
            >
              <!-- <template slot-scope="scope">
                <span
                  class="click_code"
                  :data-id="scope.row.id"
                  v-isGetSelection="['warehouse:board:view']"
                  >{{ scope.row.locationCode }}</span
                >
              </template> -->
            </el-table-column>
            <el-table-column
              key="layer"
              prop="layer"
              align="center"
              label="第几层"
              show-overflow-tooltip
            />

            <el-table-column
              key="productStatusDesc"
              prop="productStatusDesc"
              align="center"
              show-overflow-tooltip
              label="产品状态"
            />
            <el-table-column
              label="设备负载板料时间"
              show-overflow-tooltip
              align="center"
              key="devicePanelTime"
              prop="devicePanelTime"
              min-width="180"
            >
              <template slot-scope="scope">
                <span>{{ parseTime(scope.row.devicePanelTime) }}</span>
              </template>
            </el-table-column>
          </el-table>
          <el-table v-else :data="[]"> </el-table>
        </template>
      </el-table-column>
      <!-- <el-table-column
        show-overflow-tooltip
        label="库位编码"
        key="locationCode"
        prop="locationCode"
        v-if="columns[0].visible"
        min-width="150"
      >
        <template slot-scope="scope">
          <span
            class="click_code"
            :data-id="scope.row.id"
            v-isGetSelection="['warehouse:board:view']"
            >{{ scope.row.locationCode }}</span
          >
        </template>
      </el-table-column> -->
      <el-table-column
        show-overflow-tooltip
        label="板料编码"
        key="panelCode"
        prop="panelCode"
        v-if="columns[1].visible"
        min-width="150"
      >
      </el-table-column>
      <el-table-column
        show-overflow-tooltip
        label="物料编码"
        key="itemCode"
        prop="itemCode"
        min-width="150"
        v-if="columns[2].visible"
      />
      <el-table-column label="每叠块数" align="center" prop="pcs" />
      <el-table-column label="板宽" align="center" prop="panelWidth" />
      <el-table-column label="板长" align="center" prop="panelLength" />

      <el-table-column
        label="销钉偏移量"
        min-width="120px"
        align="center"
        prop="pinOffset"
      />
      <!-- <el-table-column
        show-overflow-tooltip
        label="批次号"
        align="center"
        key="batchCode"
        prop="batchCode"
        min-width="150"
        v-if="columns[3].visible"
      />
      <el-table-column
        show-overflow-tooltip
        label="板料位置"
        key="boardLocation"
        prop="boardLocation"
        min-width="150"
        v-if="columns[4].visible"
      />

      <el-table-column
        label="产品状态"
        align="center"
        show-overflow-tooltip
        key="productStatusDesc"
        prop="productStatusDesc"
        min-width="150"
        v-if="columns[5].visible"
      ></el-table-column>
      <el-table-column
        label="创建时间"
        key="createTime"
        align="center"
        min-width="180"
        v-if="columns[6].visible"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.createTime) }}</span>
        </template>
      </el-table-column> -->
    </el-table>
    <pagination
      v-show="total > 0"
      :total="total"
      :page.sync="queryParams.pageNum"
      :limit.sync="queryParams.pageSize"
      @pagination="getList"
    />

    <!-- 添加或修改板料对话框 -->
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
        :rules="rules"
        label-width="100px"
        :disabled="optType == 'view'"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item label="板料编码" prop="panelCode">
              <el-input
                v-model="form.panelCode"
                placeholder="请输入板料编码"
              /> </el-form-item
          ></el-col>
          <el-col :span="12">
            <el-form-item label="板料位置" prop="boardLocation">
              <el-input
                v-model="form.boardLocation"
                placeholder="请输入板料位置"
              /> </el-form-item
          ></el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="物料编码" prop="itemCode">
              <el-input
                v-model="form.itemCode"
                placeholder="请输入物料编码"
              /> </el-form-item
          ></el-col>
          <el-col :span="12">
            <el-form-item label="批次号">
              <el-input v-model="form.batchCode" placeholder="请输入批次号" />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="12">
            <el-form-item label="产品状态" prop="productStatus">
              <el-input
                v-model="form.productStatus"
                placeholder="请输入产品状态"
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
  listBoardTrace,
  getBoardTrace,
  delBoardTrace,
  addBoardTrace,
  updateBoardTrace,
  delList,
} from "@/api/produce/board";

export default {
  name: "BoardTrace",
  data() {
    return {
      page: "board",
      // 全选框样式
      isIndeterminateAll: false,
      globelCheckedAll: false,
      // 子行多选删除
      selectList: [],
      // 父行多选删除
      selectChildList: [],

      // 是否展开，默认全部展开
      isExpandAll: false,
      // 重新渲染表格状态
      refreshTable: true,
      // 自动编码
      autoGenFlag: false,
      // 操作类型
      optType: "",
      // 遮罩层
      loading: true,
      // 选中数组
      ids: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 板料表格数据
      boardTraceList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        itemCode: undefined,
        batchCode: undefined,
        panelCode: undefined,
        status: undefined,
        productStatusList: [],
        locationCode: undefined,
      },
      productStatusOptions: [
        {
          label: "空位",
          value: 0,
        },
        {
          label: "空仓",
          value: 1,
        },
        {
          label: "生料",
          value: 20000,
        },
        {
          label: "熟料",
          value: 40000,
        },
      ],
      // 表单参数
      form: {},
      initialForm: {},
      // 表单校验
      rules: {
        itemCode: [
          { required: true, message: "板料标号不能为空", trigger: "blur" },
        ],
        batchCode: [
          { required: true, message: "批次号不能为空", trigger: "blur" },
        ],
        boardLocation: [
          { required: true, message: "板料位置不能为空", trigger: "blur" },
        ],
        finishStatus: [
          { required: true, message: "产品状态不能为空", trigger: "blur" },
        ],
      },
      // 列信息
      columns: [
        { key: 0, label: "库位编码", visible: true },
        { key: 1, label: "板料编码", visible: true },
        { key: 2, label: "物料编码", visible: true },
        { key: 3, label: "批次号", visible: true },
        { key: 4, label: "板料位置", visible: true },
        { key: 5, label: "产品状态", visible: true },
        { key: 6, label: "创建时间", visible: true },
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
    // 判断表格是否有子项，无子项不显示展开按钮
    getRowClass(row, rowIndex) {
      // children 是你子项的数组 key
      if (row.row.children.length === 0) {
        return "row-expand-cover";
      }
    },
    /** 查询板料列表 */
    async getList(isSearch) {
      this.loading = true;
      const res = await listBoardTrace(this.queryParams);
      this.boardTraceList = res.data.list;
      this.total = res.data.total;
      this.loading = false;
      // 只有搜索状态下进行提示
      if (isSearch == "search") {
        this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
      }
    },
    // 表单重置
    reset() {
      this.form = {
        itemCode: "",
        batchCode: "",
        boardLocation: "",
        panelCode: "",
        productStatus: "",
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
      this.optType = "add";
      this.open = true;
      this.title = "添加板料";
      this.initialForm = Object.assign({}, this.form);
    },
    /** 修改按钮操作 */
    handleUpdate(row) {
      this.reset();
      const boardId = row.id || this.ids;
      getBoardTrace(boardId).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.finishStatus = this.form.finishStatus * 1;
          this.optType = "edit";
          this.open = true;
          this.title = "修改板料";
          this.initialForm = Object.assign({}, res.data);
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    // 点击编码查看
    handleView(id) {
      this.reset();
      getBoardTrace(id).then((res) => {
        if (res.code == 0) {
          this.form = res.data;
          this.form.finishStatus = this.form.finishStatus * 1;
          this.optType = "view";
          this.open = true;
          this.title = "查看板料";
        } else {
          this.$modal.notifyError(res.message);
        }
      });
    },
    /** 提交按钮 */
    submitForm: function () {
      if (this.form.id != undefined) {
        updateBoardTrace(this.form).then((res) => {
          if (res.code == 0) {
            this.$modal.msgSuccess("修改成功");
            this.open = false;
            this.getList();
          } else {
            this.$modal.notifyError(res.message);
          }
        });
      } else {
        addBoardTrace(this.form).then((res) => {
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
        this.deleteItem(row.id, delBoardTrace, this.getList);
      } else {
        this.deleteItem(this.ids, delList, this.getList);
      }
    },
    /** 导出按钮操作 */
    handleExport() {
      this.exportExcel(
        "/v1/BoardTrace/DownLoadList",
        `板料追溯.xlsx`,
        this.queryParams
      );
    },
    // 处理序号=》倒序
    reverseIndex(index) {
          var childLength=this.boardTraceList.reduce((total, parent) => {
                return total + (parent.children?.length || 0)
              }, 0)

          return childLength - index;
    }
  },
};
</script>
<style lang="scss">
.childrenHeader {
  font-size: 14px !important;
  color: rgb(88, 112, 137) !important;
  background: rgba(135, 206, 250, 0.2) !important;
}
::v-deep .el-table__cell.el-table__expanded-cell {
  padding: 0 !important;
}
</style>
