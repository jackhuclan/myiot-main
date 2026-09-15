<template>
  <div>
    <el-table
      :data="list"
      style="width: 100%"
      :max-height="maxHeight"
      v-loading="loading"
      border
    >
      <el-table-column type="index" label="序号" align="center" />
      <el-table-column
        v-for="item in columns"
        :key="item.key"
        :prop="item.key"
        :label="item.label"
        align="center"
        show-overflow-tooltip
      >
      </el-table-column>

      <el-table-column
        label="创建时间"
        align="center"
        prop="createTime"
        width="180"
      >
      </el-table-column>
    </el-table>
    <div class="pagination">
      <span> 选择显示条数： </span>
      <el-select
        v-model="pageSizeValue"
        placeholder="请选择"
        size="mini"
        @change="handlePageSize"
      >
        <el-option
          v-for="item in options"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        >
        </el-option>
      </el-select>
    </div>
  </div>
</template>

<script>
import { 
  listDeviceEventList,
  listDeviceStatusList,
  listDevicePropertyList,
  listDeviceServiceList,
} from "@/api/device/device";
export default {
  data() {
    return {
      loading: false,
      list: [],
      // 点击查看后的分页字段
      options: [
        {
          value: 10,
          label: "10条",
        },
        {
          value: 20,
          label: "20条",
        },
        {
          value: 50,
          label: "50条",
        },
        {
          value: 100,
          label: "100条",
        },
      ],
      pageSizeValue: "10条",
      queryParams: {
        limit: 10,
        deviceCode: "MockAgv01",
      },
      api: undefined,
    };
  },
  props: ["columns", "maxHeight", "apiValue", "deviceCode"],
  watch: {
    apiValue: {
      handler(val) {
        switch (val) {
          case "eventTab":
            this.api = listDeviceEventList;
            break;
          case "statusTab":
            this.api = listDeviceStatusList;
            break;
          case "serviceTab":
            this.api = listDeviceServiceList;
            break;
          case "propertyTab":
            this.api = listDevicePropertyList;
            break;
        }
      },
      deep: true,
      immediate: true,
    },
  },
  methods: {
    getList() {
      this.queryParams.deviceCode = this.deviceCode;
      this.api(this.queryParams).then((res) => {
        this.list = res.data;
      });
       
    },
    // 改变查看时表格分页条数
    handlePageSize(val) {
      this.queryParams.limit = val;
      this.getList(this.queryParams.deviceCode);
    },
  },
};
</script>
<style scoped lang="scss">
.pagination {
  width: 100%;
  height: 40px;
  display: flex;
  justify-content: flex-end;
  align-items: center;
  .el-select {
    width: 100px !important;
    margin-left: 10px;
  }
}
</style>
