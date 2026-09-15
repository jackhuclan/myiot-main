<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="待排产料号" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入待排产料号"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
    </search-form>

    <el-row :gutter="10" class="mb8">
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
      :data="toBeScheduledInfoList"
      @selection-change="handleSelectionChange"
      @row-dblclick="rowDblclick"
      :row-style="rowStyle"
    >
      <el-table-column
        label="待排产料号"
        key="itemCode"
        prop="itemCode"
        show-overflow-tooltip
        fixed="left"
        min-width="150px"
        v-if="columns[0].visible"
      >
      </el-table-column>
      <el-table-column
        label="孔数"
        align="center"
        key="drillCount"
        prop="drillCount"
        v-if="columns[1].visible"
      >
      </el-table-column>

      <el-table-column
        label="叠数"
        align="center"
        key="panelCount"
        prop="panelCount"
        v-if="columns[2].visible"
      >
      </el-table-column>
      <el-table-column
        label="库存数量"
        v-if="columns[3].visible"
        align="center"
        key="lotStockNum"
        prop="lotStockNum"
        min-width="100px"
      />

      <el-table-column
        label="生产数量"
        v-if="columns[4].visible"
        align="center"
        key="quantityProduced"
        prop="quantityProduced"
        min-width="100px"
      />

      <el-table-column
        label="已排产数量"
        v-if="columns[5].visible"
        align="center"
        key="quantityScheduled"
        prop="quantityScheduled"
        min-width="100px"
      />
      <el-table-column
        label="未排产数量"
        v-if="columns[6].visible"
        align="center"
        key="quantityUnScheduled"
        prop="quantityUnScheduled"
        min-width="100px"
      />

      <el-table-column
        label="生产机台"
        v-if="columns[7].visible"
        key="devices"
        prop="devices"
        align="center"
        min-width="100px"
      >
        <template slot-scope="scope">
          <!-- <span
            @click="openDeviceDialog(scope.row)"
            :class="scope.row.devices.length > 0 ? 'click_code' : ''"
            v-if="scope.row.devices"
            >{{ scope.row.devices.length }}</span
          > -->

          <el-dropdown
            trigger="click"
            v-if="scope.row.devices && scope.row.devices.length > 0"
          >
            <span class="el-dropdown-link" @click="openDeviceDialog(scope.row)">
              {{ scope.row.devices.length
              }}<i class="el-icon-arrow-down el-icon--right"></i>
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item v-for="(item, i) in deviceList" :key="i">{{
                item
              }}</el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
          <span v-else>0</span>
        </template>
      </el-table-column>

      <el-table-column
        label="预计完成时长"
        align="center"
        min-width="100px"
        key="estimatedTime"
        v-if="columns[8].visible"
      >
        <template slot-scope="scope">
          {{ scope.row.estimatedTime
          }}{{ scope.row.estimatedTime ? "(分钟)" : "" }}
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
    <!-- <edit-form-dialog
      v-model="openDevices"
      :title="'生产机台'"
      optType="view"
      :isFormModified="isFormModified"
      @submitForm="() => {}" 
    >
      <el-form :inline="true">
        <el-form-item label="待排产料号">
          <el-input v-model="devices_itemCode" disabled></el-input>
        </el-form-item>
      </el-form>

      <el-table :data="deviceList">
        <el-table-column label="机台编码">
          <template slot-scope="scope">
            <span>{{ scope.row }}</span>
          </template>
        </el-table-column>
      </el-table>
    </edit-form-dialog> -->
  </div>
</template>
  
  <script>
import { getMaterialList } from "@/api/produce/toBeScheduled";

export default {
  name: "ToBeScheduledInfo",
  data() {
    return {
      timeout: null,
      page: "toBeScheduledInfo",
      //自动生成编码
      autoGenFlag: false,
      enCode: "",
      optType: undefined,
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 选中数组
      ids: [],
      multipleSelection: [],
      // 生产工单表格数据
      toBeScheduledInfoList: [],
      // 弹出层标题
      title: "",
      // 是否显示弹出层
      open: false,
      openDevices: false,
      devices_itemCode: null,
      deviceList: [],
      total: 0,
      // 查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        itemCode: undefined,
        panelCount: undefined,
      },
      // 表单参数
      form: {},
      initialForm: {},
      routeQueryList: [],
      // 工艺路线
      routeOptions: [],
      // 列信息
      columns: [
        { key: 0, label: "待排产料号", visible: true },
        { key: 1, label: "孔数", visible: true },
        { key: 2, label: "叠数", visible: true },
        { key: 3, label: "库存数量", visible: true },
        { key: 4, label: "生产数量", visible: true },
        { key: 5, label: "已排产数量", visible: true },
        { key: 6, label: "未排产数量", visible: true },
        { key: 7, label: "生产机台", visible: true },
        { key: 8, label: "预计完成时长", visible: true },
      ],
    };
  },

  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },

  methods: {
    /** 查询列表 */
    getList(isSearch) {
      this.loading = true;
      getMaterialList(this.queryParams).then((res) => {
        this.toBeScheduledInfoList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
        // 只有搜索状态下进行提示
        if (isSearch == "search") {
          this.$modal.msgSearch("搜索成功，共" + res.data.total + "条数据！");
        }
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
      this.form = {};
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
    // 点击打开机台弹框
    openDeviceDialog(row) { 
      // this.openDevices = true;
      // this.devices_itemCode = row.itemCode;
      this.deviceList = row.devices;
    },
  },
};
</script>
  