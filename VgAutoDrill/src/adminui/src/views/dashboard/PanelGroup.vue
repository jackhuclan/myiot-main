<template>
  <div>
    <el-row :gutter="10" class="panel-group">
      <el-col :xs="12" :sm="8" :lg="6" :xl="4" class="card-panel-col">
        <div
          class="card-panel"
          @click="handleSetDeviceData({ name: '所有设备' })"
        >
          <div class="card-panel-icon-wrapper icon-device">
            <svg-icon icon-class="device" class-name="card-panel-icon" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">设备总数</div>
            <count-to
              :start-val="0"
              :end-val="deviceData.allDeviceCount"
              :duration="2600"
              class="card-panel-num"
            />
          </div>
        </div>
      </el-col>
      <el-col
        :xs="12"
        :sm="8"
        :lg="6"
        :xl="4"
        class="card-panel-col"
        v-for="item in $status.deviceStatusOptions"
        :key="item.icon"
      >
        <div
          class="card-panel"
          @click="handleSetDeviceData({ name: item.label, val: item.value })"
        >
          <div :class="`card-panel-icon-wrapper icon-${item.icon}`">
            <svg-icon :icon-class="item.icon" class-name="card-panel-icon" />
          </div>
          <div class="card-panel-description">
            <div class="card-panel-text">{{ item.label }}</div>
            <count-to
              :start-val="0"
              :end-val="deviceData[item.num]"
              :duration="2600"
              class="card-panel-num"
            />
          </div>
        </div>
      </el-col>
    </el-row>
    <!-- 从右侧弹出抽屉 -->
    <el-drawer
      :title="title"
      :visible.sync="detailOpen"
      direction="rtl"
      size="550px"
    >
      <el-table :data="deviceList" v-loading="loading" width="100%">
        <el-table-column
          property="code"
          label="设备编码"
          align="center"
          show-overflow-tooltip
        ></el-table-column>
        <el-table-column
          property="name"
          label="设备名称"
          align="center"
          show-overflow-tooltip
        ></el-table-column>
        <el-table-column label="状态" align="center">
          <template slot-scope="scope">
            <status-tag
              :options="$status.deviceStatusOptions"
              :status="scope.row.deviceStatus"
            />
          </template>
        </el-table-column>
      </el-table>
      <pagination
        v-show="total > 0"
        :total="total"
        small
        :page.sync="queryParams.pageNum"
        :limit.sync="queryParams.pageSize"
        @pagination="getList"
        layout="total, sizes, prev, pager, next"
      />
    </el-drawer>
  </div>
</template>

<script>
import countTo from "@/components/countTo";
import { getDeviceList } from "@/api/dashboard";
export default {
  name: "PanelGroup",
  components: {
    CountTo: countTo,
  },
  props: ["deviceData"],
  data() {
    return {
      detailOpen: false,
      loading: false,
      title: "",
      deviceList: [],
      total: 0,
      // 设备列表查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 10,
        name: undefined,
        code: undefined,
        status: undefined,
        deviceTypeId: undefined,
        deviceStatusList: [],
      },
      // 定时器
      detailTimer: null,
    };
  },
  watch: {
    detailOpen(val) {
      if (val) {
        // this.$emit("closeTimer");
        this.detailTimer = setInterval(() => {
          setTimeout(() => {
            this.getList(); //调用接口的方法
          }, 0);
        }, 5000);
      } else {
        clearInterval(this.detailTimer);
        this.detailTimer = null;
        // this.$emit("openTimer");
      }
    },
  },
  methods: {
    // // 点击切换设备信息
    handleSetDeviceData(type) {
      this.detailOpen = true;
      this.title = type.name;
      this.queryParams.deviceStatusList = type.val != null ? [type.val] : [];
      this.getList();
    },
    // 获取不同数据
    getList() {
      this.loading = true;
      getDeviceList(this.queryParams).then((res) => {
        this.deviceList = res.data.list;
        this.total = res.data.total;
        this.loading = false;
      });
    },
  },
};
</script>

<style lang="scss" scoped>
::v-deep .el-drawer__header {
  height: 20px;
  padding-bottom: 0;
  margin-bottom: 20px;
}

.panel-group {
  .card-panel-col {
    margin-bottom: 12px;
  }

  .card-panel {
    height: 68px;
    cursor: pointer;
    font-size: 12px;
    position: relative;
    overflow: hidden;
    color: #666;
    background: #fff;
    box-shadow: 4px 4px 40px rgba(0, 0, 0, 0.05);
    border-color: rgba(0, 0, 0, 0.05);
    border-radius: 10px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    &:hover {
      .card-panel-icon-wrapper {
        color: #fff;
      }

      .icon-device {
        background: #40c9c6;
      }

      .icon-running {
        background: #36a3f7;
      }

      .icon-weixiuzhong {
        background: #ffc2c2;
      }
      .icon-waitting {
        background: #f4516c;
      }

      .icon-fault {
        background: #34bfa3;
      }
      .icon-onlined {
        background: rgb(142, 48, 223);
      }
      .icon-offline {
        background: rgb(220, 203, 15);
      }
      .icon-didianliang {
        background: #d81e06;
      }
      .icon-chongdian {
        background: #0ebd5d;
      }
    }

    .icon-device {
      color: #40c9c6;
    }

    .icon-running {
      color: #36a3f7;
    }
    .icon-weixiuzhong {
      color: #800000;
    }
    .icon-waitting {
      color: #f4516c;
    }

    .icon-fault {
      color: #34bfa3;
    }
    .icon-build {
      color: seagreen;
    }
    .icon-didianliang {
      color: #53232d;
    }
    .icon-chongdian {
      color: #a895f7;
    }

    .card-panel-icon-wrapper {
      padding: 10px;
      margin-left: 10px;
      transition: all 0.38s ease-out;
      border-radius: 6px;
    }

    .card-panel-icon {
      font-size: 40px;
    }

    .card-panel-description {
      flex: 1;
      padding-left: 10px;
      font-weight: bold;
      .card-panel-text {
        line-height: 16px;
        color: rgba(0, 0, 0, 0.45);
        font-size: 16px;
        margin-bottom: 10px;
      }

      .card-panel-num {
        font-size: 18px;
      }
    }
  }
}
</style>
