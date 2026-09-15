<template>
  <div class="app-container">
    <search-form
      v-show="showSearch"
      :form="queryParams"
      @search="handleQuery"
      @reset="resetQuery"
    >
      <el-form-item label="批次号" prop="itemCode">
        <el-input
          v-trim
          v-model="queryParams.itemCode"
          placeholder="请输入批次号"
          clearable
          @keyup.enter.native="handleQuery"
        />
      </el-form-item>
    </search-form>
    <el-row
      :style="{
        backgroundColor: '#fff',
        padding: showSearch ? '0 0 10px 10px' : '10px',
      }"
    >
      <el-col :span="5">
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
        @queryTable="handleQuery"
        :page="page"
      ></right-toolbar>
    </el-row>
    <el-collapse v-model="activeNames" class="my-collapse">
      <el-collapse-item name="线边仓">
                <span class="collapse-title" slot="title">线边仓</span>        
        <el-table
          style="width: 100%"
          :ref="page"
          :data="lineSidePositionList"
          border
        >
          <el-table-column
            show-overflow-tooltip
            label="批次号"
            prop="itemCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="托盘号"
            prop="siloCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="所在位置"
            prop="locationCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="本托盘数量"
            prop="rawMaterialCount"
          />
        </el-table>
      </el-collapse-item>
      <el-collapse-item name="转入途中">
                <span class="collapse-title" slot="title">转入途中</span>    
        <el-table style="width: 100%" :ref="page" :data="transferInList" border>
          <el-table-column
            show-overflow-tooltip
            label="批次号"
            prop="itemCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="托盘号"
            prop="siloCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="所在位置"
            prop="locationCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="总组板数"
            prop="rawMaterialCount"
          />
        </el-table>
         
      </el-collapse-item>
      <el-collapse-item name="中转位">
                <span class="collapse-title" slot="title">中转位</span>    
        <el-table
          style="width: 100%"
          :ref="page"
          :data="transpositionList"
          border
        >
          <el-table-column
            show-overflow-tooltip
            label="批次号"
            prop="itemCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="托盘号"
            prop="siloCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="所在位置"
            prop="locationCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="总组板数"
            prop="rawMaterialCount"
          />
        </el-table>
         
      </el-collapse-item>
      <el-collapse-item name="AGV">
                <span class="collapse-title" slot="title">AGV</span>      
        <el-table style="width: 100%" :ref="page" :data="agvList" border>
          <el-table-column
            show-overflow-tooltip
            label="批次号"
            prop="itemCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="托盘号"
            prop="siloCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="所在位置"
            prop="locationCode"
          />
          <el-table-column
            show-overflow-tooltip
            label="总组板数"
            prop="rawMaterialCount"
          />
        </el-table>
      </el-collapse-item>
    </el-collapse>
  </div>
</template>
<script>
import { listVegaRawMaterial } from "@/api/masterData/vegaItem";
export default {
  // 稼动率分析
  name: "VegaItem",
  data() {
    return {
      page: "vegaItem",
      showSearch: true,
      // 线边仓
      lineSidePositionList: [],
      // 转入途中
      transferInList: [],
      // 中转位
      transpositionList: [],
      // AGV
      agvList: [],
      activeNames: ["设备稼动率"],
      queryParams: {
        itemCode: undefined,
      },
      // 测试数据
      list: Array.from({ length: 40 }, (v, k) => {
        return {
          id: k,
          location: [1, 2, 3, 4][Math.floor(Math.random() * 4)],
          // 1、线边仓2、转入途中3、中转位4、AGV
          siloCode: "siloCode" + k,
          itemCode: "itemCode" + k,
          locationCode: "locationCode" + k,
          rawMaterialCount: Math.floor(Math.random() * 100) + 1,
        };
      }),
    };
  },
  activated() {
    // 默认不搜索
    // this.getList();
  },
  methods: {
    // 展开折叠
    toggleExpandAll() {
      let arr = ["线边仓", "转入途中", "AGV", "中转位"];
      if (this.activeNames.length == arr.length) {
        this.activeNames = [];
      } else {
        this.activeNames = [];
        // 注意：由于每点开一个的单独面板 activeName都会发生变化，所以点击全部展开的时候要将activeName置空
        for (const collapseTitleData of arr) {
          this.activeNames.push(collapseTitleData);
        }
      }
    },
    /** 查询列表 */
    getList() {
      this.getLineSidePositionList();
      this.getTransferInList();
      this.getTranspositionList();
      this.getAGVList();
    },
    // 线边仓
    getLineSidePositionList() {
      listVegaRawMaterial({
        ...this.queryParams,
        location: 1,
      }).then((res) => {
        this.lineSidePositionList = res.data;
      });
    },
    // 转入途中
    getTransferInList() {
      listVegaRawMaterial({
        ...this.queryParams,
        location: 2,
      }).then((res) => {
        this.transferInList = res.data;
      });
    },
    // 中转位
    getTranspositionList() {
      listVegaRawMaterial({
        ...this.queryParams,
        location: 3,
      }).then((res) => {
        this.transpositionList = res.data;
      });
    },
    // AGV
    getAGVList() {
      listVegaRawMaterial({
        ...this.queryParams,
        location: 4,
      }).then((res) => {
        this.agvList = res.data;
      });
    },
    /** 搜索按钮操作 */
    handleQuery() {
      if (
        this.queryParams.itemCode != undefined &&
        this.queryParams.itemCode != ""
      ) {
        this.getList();
        // this.toggleExpandAll();
        this.$modal.msgSuccess("搜索成功!");
      } else {
        this.$modal.msgError("请确保搜索条件全部填写!");
      }
    },
    /** 重置按钮操作 */
    resetQuery() {
      this.lineSidePositionList = [];
      this.transferInList = [];
      this.transpositionList = [];
      this.agvList = [];
      this.handleQuery();
    },
  },
};
</script>
<style lang="scss" scoped>
.collapse-title {
  flex: 1 0 90%;
  order: 1;
  font-size: 16px;
  font-weight: bold;
}

.my-collapse {
  .el-collapse-item {
    width: calc(100%);
  }

  ::v-deep .el-collapse-item__header {
    flex: 1 0 auto;
    order: -1;
    padding-left: 4px;
  }

  ::v-deep .el-collapse-item__content {
    background-color: rgb(240, 242, 245);
    padding-bottom: 0;
  }
}
</style>
