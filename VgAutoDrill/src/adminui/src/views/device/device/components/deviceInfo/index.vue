<template>
  <el-dialog
    :visible.sync="open"
    width="960px"
    append-to-body
    :close-on-click-modal="false"
    v-dialogClose
    v-dialogDrag
    :title="tableData.code"
  >
    <el-tabs class="my_tabs" v-model="activeName" @tab-click="tabClick">
      <el-tab-pane label="基本信息" name="detailTab">
        <detail-tab ref="detailTab" />
      </el-tab-pane>
      <!-- <el-tab-pane label="板料信息" name="panelTab">
        <panel-tab ref="panelTab" />
      </el-tab-pane> -->
      <!-- <el-tab-pane
        :label="item.label"
        :name="item.value"
        v-for="item in tabList"
        :key="item.value"
      >
        <TabTable
          :ref="item.value"
          :maxHeight="380"
          :columns="item.columns"
          :apiValue="item.value"
          :deviceCode="device.code"
        />
      </el-tab-pane> -->
    </el-tabs>
  </el-dialog>
</template>

<script>
import TabTable from "./tabTable.vue";
import PanelTab from "./panelTab.vue";
import DetailTab from "./detaiTab.vue";

import { mapState } from "vuex";
export default {
  props: ["deviceId"],
  components: { TabTable, PanelTab, DetailTab },
  computed: {
    ...mapState({ device: (state) => state.device.device }),
  },
  data() {
    return {
      tableData: [],
      list: [],
      open: false,
      // 动态属性
      properties: [],
      // 查看tab字段
      activeName: "detailTab",
      serviceName: "主调",

      tabList: [
        {
          label: "事件日志",
          value: "eventTab", // 事件日志表格columns
          columns: [
            {
              label: "事件编码",
              key: "eventCode",
            },
            {
              label: "事件名称",
              key: "eventName",
            },
            {
              label: "设备",
              key: "deviceCode",
            },
            {
              label: "设备类型",
              key: "deviceTypeCode",
            },
          ],
        },

        {
          label: "状态日志",
          value: "statusTab", // 状态日志表格columns
          columns: [
            {
              label: "设备",
              key: "deviceCode",
            },
            {
              label: "设备类型",
              key: "deviceTypeCode",
            },
            {
              label: "新状态",
              key: "newStatus",
            },
            {
              label: "旧状态",
              key: "oldStatus",
            },
          ],
        },
        {
          label: "属性日志",
          value: "propertyTab", // 属性日志表格columns
          columns: [
            {
              label: "设备",
              key: "deviceCode",
            },
            {
              label: "设备类型",
              key: "deviceTypeCode",
            },
            {
              label: "属性数据",
              key: "propertyJson",
            },
          ],
        },
        {
          label: "服务调度日志",
          value: "serviceTab", // 服务调度日志表格columns
          columns: [
            {
              label: "服务编码",
              key: "serviceCode",
            },
            {
              label: "设备",
              key: "deviceCode",
            },
            {
              label: "设备类型",
              key: "deviceTypeCode",
            },
            {
              label: "事件编码",
              key: "eventCode",
            },
            {
              label: "事件名称",
              key: "eventName",
            },
            {
              label: "目标设备",
              key: "targetDeviceCode",
            },
            {
              label: "目标产品",
              key: "targetProductCode",
            },
          ],
        },
      ],
    };
  },
  watch: {
    open(val) {
      if (val) {
        this.activeName = "detailTab";
      }
    },
  },
  methods: {
    getList() {
      this.$nextTick(() => {
        this.$refs[this.activeName].length
          ? this.$refs[this.activeName][0].getList()
          : this.$refs[this.activeName].getList(this.device);
      });
    },

    tabClick(val) {
      this.activeName = val.name;
      this.getList();
    },
  },
};
</script>
<style scoped>
::v-deep .el-dialog__body {
  padding: 0 20px 20px 20px !important;
}
</style>