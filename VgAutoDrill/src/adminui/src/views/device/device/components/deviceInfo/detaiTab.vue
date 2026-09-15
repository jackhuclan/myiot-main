<template>
  <div>
    <el-descriptions title="设备信息" direction="vertical" :column="4" border>
      <el-descriptions-item label="设备编码">{{
        deviceInfos.code
      }}</el-descriptions-item>
      <el-descriptions-item label="设备名称">{{
        deviceInfos.name
      }}</el-descriptions-item>
      <el-descriptions-item label="设备状态">{{
        deviceInfos.deviceStatus
      }}</el-descriptions-item>
      <el-descriptions-item label="创建时间">{{
        parseTime(deviceInfos.createTime)
      }}</el-descriptions-item>
    </el-descriptions>
    <el-descriptions
      v-if="properties.length > 0"
      style="margin-top: 10px"
      title="动态属性"
      direction="vertical"
      :column="4"
      border
      :label-style="label_style"
      :contentStyle="content_style"
    >
      <el-descriptions-item
        :key="item.label"
        v-for="item in properties"
        :label="item.label"
        >{{
          typeof item.value == "string" &&
          new Date(item.value) != "Invalid Date"
            ? new Date(item.value).toLocaleString("sv-SE")
            : item.value
        }}</el-descriptions-item
      >
    </el-descriptions>
    <div v-else>
      <p
        style="
          margin-top: 10px;
          font-size: 16px;
          font-weight: bold;
          color: #303133;
        "
      >
        动态属性
      </p>
      <p>暂无数据</p>
    </div>
    <p style="font-size: 16px; font-weight: bold; color: #303133">设备参数</p>
    <json-view
      :data="JSON.parse(deviceInfos.parameters)"
      v-if="deviceInfos.parameters"
      :deep="1"
    />
    <span v-else>暂无数据</span>
  </div>
</template>

<script>
import { getDevice } from "@/api/device/device";
import { getOnlineDeviceInfo } from "@/api/device/onlineDevice";

export default {
  data() {
    return {
      label_style: {
        "word-break": "keep-all",
      },
      content_style: {
        "min-width": "200px",
        "word-break": "break-all", //过长时自动换行
      },
      // 设备信息
      deviceInfos: [],
      // 动态属性
      properties: [],
    };
  },
  methods: {
    getList(val) { 
      getDevice(val.id).then((res) => {
        {
          if (res.code == 0) {
            this.deviceInfos = {
              ...res.data,
              deviceStatus: this.$status.deviceStatusOptions.filter(
                (v) => v.value == res.data.deviceStatus
              )[0]?.label,
            };
          }
        }
      });
      getOnlineDeviceInfo(val.code).then((res) => {
        if (res.code == 0) {
          if (res.data.properties) {
            this.properties = Object.keys(res.data.properties).map(function (
              i
            ) {
              return { label: i, value: res.data.properties[i] };
            }); //对象转化为数组;
          } else {
            this.properties = [];
          }
        } else {
          this.properties = [];
        }
      });
    },
  },
};
</script>
