<template>
  <!-- 从右侧弹出抽屉 -->
  <el-drawer
    v-if="detailOpen"
    :title="''"
    ref="detailDrawer"
    :visible="detailOpen"
    direction="rtl"
    size="400px"
    @close="detailOpen = false"
  >
    <div slot="title" class="drawer_title">
      <p>
        <span> 料仓任务Id--{{ detailInfo.id }} </span>
        <i
          v-if="isRefresh"
          class="el-icon-success"
          style="font-size: 20px; color: #67c23c"
        />
        <i v-else class="el-icon-info" style="font-size: 20px" />
      </p>
    </div>
    <div class="block">
      <div class="info">
        <li>
          分区：
          {{ detailInfo.warehouseCode }}
        </li>
        <li>
          库位:
         {{ detailInfo.forkCode
            }}{{
              detailInfo.positionCodes
                ? "（" + detailInfo.positionCodes + "）"
                : ""
            }} 
        </li>
        <li>
          类型：
          {{
            transportation_kind_options.find(
              (v) => v.value == detailInfo.transportationKind
            ) &&
            transportation_kind_options.find(
              (v) => v.value == detailInfo.transportationKind
            ).label
          }}
        </li>
        <!-- 
          <li>
            产品编码：
            {{ detailTitle.itemCode }}
          </li>
          <li>
            工艺路线：
            {{ detailTitle.routeCode }}
          </li>
  
          <li>
            是否全部派送:
            {{ detailTitle.isAllPanelSent ? "是" : "否" }}
          </li>
          <li>
            是否辅助呼叫:
            {{ detailTitle.isAuxiliary ? "是" : "否" }}
          </li>
          <li>
            申请物料种类:
            {{ detailTitle.requestMaterialKind }}
          </li>
          <li class="isMesage">
            发起设备板料信息:
            <pre class="pre">{{ detailTitle.requestSummaryInfo  }}</pre>
          </li>

        -->
        <li class="isMesage">
          HK反馈信息:
          <pre class="pre">{{ detailInfo.hkResponse }}</pre>
        </li>
        <li class="isMesage">
          备注:
          <pre class="pre">{{ detailInfo.relatedDrillTrace }}</pre>
        </li>
      </div>
      <el-timeline>
        <el-timeline-item
          v-for="(activity, index) in detailTimes"
          :key="index"
          :type="activity.type"
          :timestamp="activity.timestamp"
        >
          {{ activity.content }}
        </el-timeline-item>
      </el-timeline>

      <el-collapse v-model="activeNames">
        <!-- <el-collapse-item title="详情" name="1">
            <span class="collapse-title" slot="title"> 详情</span>
            <json-view :data="detailJSON" v-if="detailJSON" :deep="1" />
            <span v-else>暂无数据</span>
          </el-collapse-item> -->
        <el-collapse-item title="日志" name="2">
          <span class="collapse-title" slot="title"> 日志</span>
          <div
            class="schedulement_detail"
            v-if="transferJobLogDtos && transferJobLogDtos.length > 0"
          >
            <li v-for="(item, index) in transferJobLogDtos" :key="index">
              <p>{{ item.createTime }}</p>
              <p><pre class="pre">{{ item.message }}</pre></p>
            </li>
          </div>
          <span v-else>暂无数据</span>
        </el-collapse-item>
      </el-collapse>
    </div>
  </el-drawer>
</template>
  
  <script>
import jsonView from "vue-json-views";
import { mapState } from "vuex";
import { getTransferJobLog } from "@/api/device/transportationTask";
export default {
  components: { jsonView },
  props: ["detailId" ],
  data() {
    return {
      isRefresh: false,
      detailOpen: false,
      detailTimer: null,
      // 折叠面板默认展开
      activeNames: ["2"],
      detailLoading: false,
      // 详情时间数据
      detailTimes: [],
      detailTitle: {},
      // 详情JSON
      detailJSON: {},
      // 明细
      detailInfo: {},
      transferJobLogDtos: [], // 类型
      transportation_kind_options: [
        {
          label: "空仓",
          value: 1,
        },
        {
          label: "生料仓",
          value: 2,
        },
        {
          label: "熟料仓",
          value: 3,
        },
        {
          label: "首件",
          value: 4,
        },
      ],
    };
  },
  deactivated() {
    clearInterval(this.detailTimer);
    this.detailTimer = null;
    this.isRefresh = false;
  },
  beforeDestroy() {
    clearInterval(this.detailTimer);
    this.detailTimer = null;
    this.isRefresh = false;
  },
  computed: {
    ...mapState({
      schedule_detailInterval: (state) =>
        state.personalized.schedule_detailInterval*1000,
    }),
  },
  watch: {
    schedule_detailInterval: {
      handler(val) {
        if (this.detailOpen) {
          this.getDetatilSetInterval(val);
        }
      },
      deepL: true,
      immediate: true,
    },
    detailOpen(val) {
      if (val) {
        this.getDetatilSetInterval(this.schedule_detailInterval);
      } else {
        clearInterval(this.detailTimer);
        this.detailTimer = null;
        this.isRefresh = false;
        // this.activeNames = ["2"];
        this.$emit("changeSetInterval");
      }
    },
  },
  methods: {
    // 详情数据获取定时器
    getDetatilSetInterval(val) {
      if (this.detailTimer) {
        clearInterval(this.detailTimer);
        this.detailTimer = null;
      }
      this.detailTimer = setInterval(() => {
        setTimeout(() => {
          this.isRefresh = true;
          this.getDetail(this.detailId); //调用接口的方法
        }, 0);
      }, val);
    },
    // 获取详情数据
    getDetail(id) {
      this.detailLoading = true;
      getTransferJobLog(id).then((res) => {
        if(res.code==1) return this.$modal.notifyError(res.message);
        this.detailOpen = true;
        this.detailLoading = false;
        this.detailInfo = res.data;
        this.transferJobLogDtos = res.data.transferJobLogDtos;
        this.detailTimes = [
          {
            content: "创建",
            timestamp: res.data.createTime,
            type: "primary",
          },
          {
            content: "开始调度",
            timestamp: res.data.runningTime,
            type: "warning",
          },
          {
            content: "调度完成",
            timestamp: res.data.completedTime,
            type: "success",
          },
          {
            content: "执行失败",
            timestamp: res.data.failedTime,
            type: "danger",
          },
          {
            content: "取消",
            timestamp: res.data.canceledTime,
            type: "",
          },
        ];
        // if (
        //   res.data.completedTime ||
        //   res.data.failedTime ||
        //   res.data.canceledTime
        // ) {
        //   this.isRefresh = false;
        //   clearInterval(this.detailTimer);
        //   this.detailTimer = null;
        // } else {
        //   this.isRefresh = true;
        // }
      });

      setTimeout(() => {
        this.isRefresh = false;
      }, 1500);
    },
  },
};
</script>
  
  <style scoped lang="scss">
::v-deep .el-drawer__header {
  height: auto;
  padding: 10px;
  margin: 0;
  .drawer_title {
    padding: 0;
    margin: 0;
    font-size: 18px;

    p {
      height: 20px;
      padding: 0;
      margin: 0;
      display: flex;
      align-items: center;
      span {
        color: #414143;
        margin-right: 5px;
      }
    }
  }
}

.block {
  .info {
    padding: 0 10px;
    color: #676769;
    li {
      word-break: break-all;
      margin-top: 5px;
      line-height: 18px;
      font-size: 14px;
    }
  }
  .el-timeline {
    padding: 15px 10px 10px 10px;
  }

  .el-timeline-item {
    padding: 2px 0px !important;
  }

  .el-timeline-item:last-child {
    padding-bottom: 0 !important;
  }
}

.schedulement_detail {
  padding: 0 20px;

  li {
    list-style: none;
    padding: 5px 0;

    p {
      padding: 0;
      margin: 0;
      color: #909399;
      line-height: 1;
      font-size: 13px;
    }

    p:nth-child(2) {
      line-height: 20px;
      margin-top: 5px;
      border-radius: 3px;
      padding: 5px;
      background: #f6f6ff;
      box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
      color: #303133;
      font-size: 14px;
    }
  }
}
.isMesage {
  border-bottom: 1px dashed #ccc;
  padding-bottom: 5px;
  margin-bottom: 5px;
}
.pre {
  padding: 0 !important;
  line-height: 20px !important;
  margin: 0 !important;
  white-space: pre-wrap;
  word-break: break-all;
}

.collapse-title {
  flex: 1 0 90%;
  order: 1;
}

.el-collapse-item__header {
  flex: 1 0 auto;
  order: -1;
}

::v-deep .el-collapse {
  margin: 0 10px;
}
::v-deep .el-collapse {
  border: none;
}
// json插件样式
::v-deep .json-item {
  padding-left: 1rem !important;
  display: block !important;
}
::v-deep .json-key {
  display: inline !important;
  white-space: normal !important;
  word-break: break-all !important;
}

::v-deep .json-value {
  display: inline !important;
}
</style>
  