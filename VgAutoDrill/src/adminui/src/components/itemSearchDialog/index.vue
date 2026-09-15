<template>
  <edit-form-dialog
    v-model="open"
    :title="title"
    optType="view"
    @submitForm="() => {}"
  >
    <el-row class="itemSearchRow">
      <el-col :span="5">
        <el-button
          type="info"
          plain
          icon="el-icon-sort"
          @click="toggleExpandAll"
          >展开/折叠</el-button
        >
      </el-col>
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
  </edit-form-dialog>
</template>
<script>
import { listVegaRawMaterial } from "@/api/masterData/vegaItem";
export default {
  name: "ItemSearchDialog",
  data() {
    return {
      page: "itemSearchDialog",
      open: false,
      title: "",
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
    };
  },
  watch: {
    open(val) {
      if (!val) {
        // 线边仓
        this.lineSidePositionList = [];
        // 转入途中
        this.transferInList = [];
        // 中转位
        this.transpositionList = [];
        // AGV
        this.agvList = [];
      }
    },
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
.itemSearchRow {
  margin-top: -30px;
  margin-bottom: 10px;
}
</style>
