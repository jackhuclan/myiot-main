<template>
  <el-card>
    <div slot="header" class="clearfix">
      <span>生产进度</span>
    </div>
    <el-table
      :data="workorderList"
      row-key="id"
      default-expand-all
      height="300px"
      :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
    >
      <el-table-column
        label="工单编码"
        prop="code"
        fixed="left"
        show-overflow-tooltip
      />
      <!-- <el-table-column
        label="订单编码"
        width="140"
        align="center"
        prop="sourceCode"
             show-overflow-tooltip
      /> -->
      <el-table-column
        label="客户名称"
        align="center"
        prop="clientName"
        show-overflow-tooltip
      />
      <el-table-column
        label="产品编码"
        align="center"
        prop="itemCode"
        show-overflow-tooltip
      />
      <el-table-column
        label="产品名称"
        align="center"
        prop="itemName"
        show-overflow-tooltip
      />
      <el-table-column
        label="规格型号"
        align="center"
        prop="specification"
        show-overflow-tooltip
      />
      <el-table-column
        label="单位"
        align="center"
        prop="unitOfMeasure"
        show-overflow-tooltip
      />
      <el-table-column
        label="生产进度"
        align="center"
        width="200px"
        show-overflow-tooltip
      >
        <template slot-scope="scope">
          <el-progress
            v-if="
              scope.row.quantityChanged == undefined ||
              scope.row.quantityProduced == undefined
            "
            :text-inside="true"
            :stroke-width="20"
            :percentage="0"
            color="#67c23a"
          ></el-progress>

          <el-progress
            v-else
            :text-inside="true"
            :stroke-width="20"
            :percentage="getPercentage(scope.row)"
            color="#67c23a"
          ></el-progress>
        </template>
      </el-table-column>
      <el-table-column
        label="需求日期"
        align="center"
        prop="requestDate"
        width="180"
      >
        <template slot-scope="scope">
          <span>{{ parseTime(scope.row.requestDate, "{y}-{m}-{d}") }}</span>
        </template>
      </el-table-column>
    </el-table>
  </el-card>
</template>

<script>
export default {
  name: "WorkOrderTable",
  props: ["workorderList", "loading"],
  methods: {
    getPercentage(row) {
      if (
        ((row.quantityProduced / row.quantityChanged) * 100).toFixed(0) >= 100
      ) {
        return 100;
      }
      const num = row.quantityProduced / row.quantityChanged;
      if (isNaN(num)) return 0;
      return (num * 100).toFixed(0) * 1;
    },
  },
};
</script>

<style lang="scss" scoped></style>
