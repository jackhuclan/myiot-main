<template>
  <select-form-dialog
    v-if="showFlag"
    v-model="showFlag"
    title="设备选择"
    :center="true"
    @submitForm="confirmSelect"
  >
    <el-row :gutter="20">
      <!--分类数据-->
      <el-col :span="6" :xs="24" v-if="showLeft">
        <leftTreeSelect
          v-model="deviceTypeName"
          :placeholder="'请输入产品名称'"
          :filterNode="filterNode"
          :options="deviceTypeOptions"
          :defaultProps="defaultProps"
          :handleNodeClick="handleNodeClick"
          :highlightCurrent="highlightCurrent"
        ></leftTreeSelect
      ></el-col>

      <!--设备数据-->
      <el-col :span="showLeft?18:24" :xs="24">
        <el-form
          :model="queryParams"
          ref="queryForm"
          :inline="true"
          v-show="showSearch"
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
          <el-form-item label="状态" prop="deviceStatusList">
            <el-select
              v-model="queryParams.deviceStatusList"
              placeholder="请选择"
              multiple
              collapse-tags
              :class="
                queryParams.deviceStatusList.length >= 2 ? 'select-hastags' : ''
              "
            >
              <el-option
                v-for="item in $status.deviceStatusOptions"
                :key="item.value"
                :label="item.label"
                :value="item.value"
                v-optionTitle
              />
            </el-select>
          </el-form-item>
          <el-form-item>
            <el-button
              type="primary"
              icon="el-icon-search"
              @click="handleQuery"
              v-debounce
              >搜索</el-button
            >
            <el-button icon="el-icon-refresh" @click="resetQuery" v-debounce
              >重置</el-button
            >
          </el-form-item>
        </el-form>

        <el-table
          border
          v-loading="loading"
          :data="deviceList"
          @current-change="handleCurrent"
          @row-dblclick="handleRowDbClick"
          width="100%"
        >
          <el-table-column width="50" align="center">
            <template v-slot="scope">
              <el-radio
                v-removeAriaHidden
                v-model="selectedDeviceCode"
                :label="scope.row.code"
                @change="handleRowChange(scope.row)"
                >{{ "" }}</el-radio
              >
            </template>
          </el-table-column>
          <el-table-column
            label="设备编码"
            show-overflow-tooltip
            align="center"
            min-width="120"
            prop="code"
          />
          <el-table-column
            label="设备名称"
            show-overflow-tooltip
            align="center"
            prop="name"
            min-width="120"
          />
          <el-table-column
            label="设备类别"
            show-overflow-tooltip
            min-width="120"
            align="center"
          >
            <template slot-scope="scope">
              {{
                $status.deviceKinds.find((v) => v.value == scope.row.deviceKind)
                  ? $status.deviceKinds.find(
                      (v) => v.value == scope.row.deviceKind
                    ).label
                  : ""
              }}
            </template>
          </el-table-column>
          <el-table-column label="状态" align="center">
            <template slot-scope="scope">
              <status-tag
                :options="$status.deviceStatusOptions"
                :status="scope.row.deviceStatus"
              ></status-tag>
            </template>
          </el-table-column>
        </el-table>

        <pagination
          v-show="total > 0"
          :total="total"
          :page.sync="queryParams.pageNum"
          :limit.sync="queryParams.pageSize"
          @pagination="getList"
          :autoScroll="false"
        />
      </el-col>
    </el-row>
  </select-form-dialog>
</template>

<script>
import { listDevice, treeselect } from "@/api/device/device";

export default {
  name: "DeviceSelcet", 
  data() {
    return {
      showFlag: false,
      // 遮罩层
      loading: true,
      // 是否显示左侧分类树
      showLeft:true,
      // 选中数组
      selectedDeviceCode: undefined,
      selectedRows: [],
      // 显示搜索条件
      showSearch: true,
      // 总条数
      total: 0,
      // 物料产品表格数据
      deviceList: [],
      // 弹出层标题
      title: "",
      // 树形结构选项
      highlightCurrent: true,
      deviceTypeOptions: undefined,
      defaultProps: {
        children: "children",
        label: "label",
      },
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
        deviceStatusList: [],
        deviceKindList: [],
      },
      deviceStatusOptions: this.$status.deviceStatusOptions,
    };
  },
  watch: {
    // 根据名称筛选产品树
    productName(val) {
      this.$refs.tree.filter(val);
    },
    showFlag(val) {
      if (!val) {
        this.resetForm("queryForm");
        this.queryParams.deviceTypeId = undefined;
      }
    },
  },
  methods: {
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
    /** 查询设备类型下拉树结构 */
    async getTreeselect() {
      const res = await treeselect();
      this.deviceTypeOptions = res.data;
    },
    // 筛选节点
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    // 节点单击事件
    handleNodeClick(data) {
      this.highlightCurrent = true;
      if (data.label != "全部") {
        this.queryParams.deviceTypeId = data.id;
      } else {
        this.queryParams.deviceTypeId = undefined;
      } 
      this.handleQuery();
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
      this.resetForm("queryForm");
      this.handleQuery();
    },
    handleCurrent(row) {
      if (row) {
        this.selectedRows = row;
      }
    },
    //双击选中
    handleRowDbClick(row) {
      if (row) {
        this.selectedRows = row;
        this.$emit("onSelected", this.selectedRows);
        this.showFlag = false;
        this.selectedDeviceCode = undefined;
        this.selectedRow = undefined;
      }
    },
    // 单选选中数据
    handleRowChange(row) {
      if (row) {
        this.selectedRows = row;
      }
    },
    //确定选中
    confirmSelect() {
      if (this.selectedDeviceCode == null || this.selectedDeviceCode == 0) {
        this.$notify({
          title: "提示",
          type: "warning",
          message: "请至少选择一条数据!",
        });
        return;
      }
      this.$emit("onSelected", this.selectedRows);
      this.showFlag = false;
      this.selectedDeviceCode = undefined;
      this.selectedRow = undefined;
    },
  },
};
</script>
