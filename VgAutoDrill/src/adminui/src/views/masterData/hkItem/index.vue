<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="目标区域" prop="targetPosArea">
        <el-input
          v-trim
          v-model="queryParams.targetPosArea"
          placeholder="请输入目标区域"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="批次" prop="lot">
        <el-input
          v-trim
          v-model="queryParams.lot"
          placeholder="请输入批次"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="物料号" prop="product">
        <el-input
          v-trim
          v-model="queryParams.product"
          placeholder="请输入物料号"
          clearable
          @keyup.enter.native="handleQuery"
        /> </el-form-item
      ><el-form-item label="托盘号" prop="podCode">
        <el-input
          v-trim
          v-model="queryParams.podCode"
          placeholder="请输入托盘号"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="所在位置" prop="address">
        <el-input
          v-trim
          v-model="queryParams.address"
          placeholder="请输入所在位置"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
      <el-form-item label="所在位置区域" prop="addressName">
        <el-input
          v-trim
          v-model="queryParams.addressName"
          placeholder="请输入所在位置区域"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
    </search-form>
    <el-table border :ref="page" v-loading="loading" :data="HK_List">
      <el-table-column
        label="物料号"
        key="product"
        prop="product"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[0].visible"
      >
      </el-table-column>
      <el-table-column
        label="托盘号"
        key="podCode"
        prop="podCode"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[1].visible"
      />

      <el-table-column
        label="批次"
        key="lot"
        prop="lot"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[2].visible"
      />
      <el-table-column
        label="所在位置"
        key="address"
        prop="address"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[3].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.address" /> </template
      ></el-table-column>
      <el-table-column
        label="所在位置区域"
        key="addressName"
        prop="addressName"
        min-width="150"
        show-overflow-tooltip
        v-if="columns[4].visible"
      >
        <template slot-scope="scope">
          <tooltip :value="scope.row.addressName" /> </template
      ></el-table-column>
      <el-table-column
        align="center"
        label="本托盘数量"
        min-width="100"
        key="qty"
        prop="qty"
        show-overflow-tooltip
        v-if="columns[5].visible"
      />
      <el-table-column
        align="center"
        label="系统数量"
        key="sysQty"
        prop="sysQty"
        show-overflow-tooltip
        v-if="columns[6].visible"
      />
      <el-table-column
        align="center"
        label="差数"
        key="difQty"
        prop="difQty"
        show-overflow-tooltip
        v-if="columns[7].visible"
      />
    </el-table>
  </div>
</template>
  
  <script>
import { getStockInfoQuery } from "@/api/produce/externalWorkOrder";

export default {
  name: "HKITEM",
  data() {
    return {
      page: "HK_item",
      // 遮罩层
      loading: true,
      // 显示搜索条件
      showSearch: true,
      // 海康表格数据
      HK_List: [],
      // 查询参数
      queryParams: {
        targetPosArea: undefined,
        lot: undefined,
        product: undefined,
        podCode: undefined,
        address: undefined,
        addressName: undefined,
      },
      // 列信息，
      columns: [
        { key: 0, label: "物料号", visible: true },
        { key: 1, label: "托盘号", visible: true },
        { key: 2, label: "批次", visible: true },
        { key: 3, label: "所在位置", visible: true },
        { key: 4, label: "所在位置区域", visible: true },
        { key: 5, label: "本托盘数量", visible: true },
        { key: 6, label: "系统数量", visible: true },
        { key: 7, label: "差数", visible: true },
      ],
    };
  },
  activated() {
    this.getList();
    // 获取本地存储的表格显隐列信息
    this.columns = this.getColumns(this.columns, this.page);
  },
  methods: {
    /** 查询海康列表 */
    async getList() {
      this.loading = true;
      const res = await getStockInfoQuery(this.queryParams);
      this.HK_List = res.data;
      this.loading = false;
    },

    /** 搜索按钮操作 */
    handleQuery() {
      this.getList();
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.handleQuery();
    },
  },
};
</script>
  