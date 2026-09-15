<template>
  <div class="app-container">
    <el-row :gutter="10">
      <el-col :span="8"
        ><div class="grid flex font-bold font-18">
          <div class="flex">
            <svg-icon icon-class="index-work" class-name="device-status-icon" />
            {{ deviceOption.WorkingCount }} 加工台数
          </div>
          <div class="flex">
            <svg-icon
              icon-class="index-alarm"
              class-name="device-status-icon"
            />{{ deviceOption.AlarmCount }} 报警台数
          </div>
        </div></el-col
      >
      <el-col :span="8"
        ><div class="grid flex font-bold font-18">
          <div>
            <svg-icon
              icon-class="index-stop"
              class-name="device-status-icon"
            />{{ deviceOption.StopCount }} 停止台数
          </div>
          <div>
            <svg-icon
              icon-class="index-wait"
              class-name="device-status-icon"
            />{{ deviceOption.WaitingCount }} 等待台数
          </div>
        </div></el-col
      >
      <el-col :span="8"
        ><div class="grid flex font-bold font-18">
          <div>
            <svg-icon
              icon-class="index-idle"
              class-name="device-status-icon"
            />{{ deviceOption.IdleCount }} 闲置台数
          </div>
          <div>
            <svg-icon
              icon-class="index-offline"
              class-name="device-status-icon"
            />{{ deviceOption.OfflineCount }} 离线台数
          </div>
        </div></el-col
      >
    </el-row>
    <el-row :gutter="10">
      <el-col
        :lg="6"
        :md="8"
        :sm="12"
        :xs="24"
        v-for="(item, index) in drillList"
        :key="index"
      >
        <div class="grid flex-cloumn" @click="handleDetail(item)">
          <!-- 图片 -->
          <el-image
            class="img"
            :src="require('@/assets/images/device.png')"
          ></el-image>
          <!-- 稼动率 -->
          <div class="flex">
            <el-rate
              allow-half
              :value="item.sDuty / 20"
              disabled
              show-score
              disabled-void-color="#ccc"
            >
            </el-rate>
            <span class="font-bold">稼动率：{{ item.sDuty }}%</span>
          </div>
          <div>设备状态：{{ item.sWorkMode }}</div>
          <div>设备编号：{{ item.sEquipmentID }}#</div>
          <div>钻孔进度：{{ item.sPersent }}</div>
          <div>已钻孔数：{{ item.sDrilled }}</div>
          <div>总共孔数：{{ item.sNeeded }}</div>
          <div style="color: #818181">钻带文件：{{ item.sActProgram }}</div>
        </div>
      </el-col>
    </el-row>

    <div class="pagination-box" v-show="total > 0">
      <el-pagination
        :current-page.sync="queryParams.pageNum"
        :page-size.sync="queryParams.pageSize"
        layout=" prev, pager, next"
        :total="total"
        @current-change="handleCurrentChange"
      />
    </div>
  </div>
</template>

<script>
import { scrollTo } from "@/utils/scroll-to";
import list from "@/mock";
import { getSysdrillInformation } from "@/api/home";
export default {
  data() {
    return {
      // 总条数
      total: 0,
      //查询参数
      queryParams: {
        pageNum: 1,
        pageSize: 8,
      },
      autoScroll: true,
      drillList: [],
      list,
      screenWidth: false,

      deviceOption: {},
    };
  },
  watch: {
    screenWidth: {
      handler(val) {
        this.getList();
      },
      immediate: true,
      deep: true,
    },
  },
  created() {
    this.handleScreenWidth();
    this.getList();
  },
  mounted() {
    window.onresize = () => {
      this.handleScreenWidth();
    };
  },
  methods: {
    // 检测页面宽度判断一页展示几个数据
    handleScreenWidth() {
      if (document.body.clientWidth > 992 && document.body.clientWidth < 1200) {
        this.screenWidth = true;
      } else {
        this.screenWidth = false;
      }
    },
    // 获取数据
    getList() {
      this.queryParams.pageSize = this.screenWidth ? 9 : 8;

      getSysdrillInformation().then((res) => {
        const list = res.drillList;
        let start = (this.queryParams.pageNum - 1) * this.queryParams.pageSize;
        if (start >= list.length) start = 0;
        let end = this.queryParams.pageNum * this.queryParams.pageSize;
        if (end >= list.length) end = list.length;
        this.drillList = list.slice(start, end);
        this.total = list.length;
        this.deviceOption = {
          WorkingCount: res.WorkingCount,
          AlarmCount: res.AlarmCount,
          OfflineCount: res.OfflineCount,
          StopCount: res.StopCount,
          WaitingCount: res.WaitingCount,
          IdleCount: res.IdleCount,
        };
      });
    },
    // 分页 按钮
    handleCurrentChange(val) {
      this.getList();
      if (this.autoScroll) {
        scrollTo(0, 800);
      }
    },
    // 跳转详情
    handleDetail(item) {
      const code = item.sEquipmentID;
      this.$router.push({
        path: "/detail/" + code,
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.grid {
  font-size: 14px;
  border-radius: 6px;
  min-height: 36px;
  padding: 10px;
  margin-bottom: 10px;
  background: #fff;
}
.flex {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.flex-cloumn {
  display: flex;
  justify-content: center;
  flex-direction: column;
}
.font-bold {
  font-weight: bold;
}
.font-18 {
  font-size: 18px;
}
.img {
  width: 200px;
  height: 100px;
  margin: 0 auto;
}

::v-deep .el-rate__text {
  display: none;
}
.device-status-icon {
  font-size: 20px;
  margin-right: 5px;
}
.pagination-box {
  display: flex;
  align-items: center;
  justify-content: center;
  ::v-deep.el-pagination button {
    min-width: auto !important;
  }
  ::v-deep .el-pager {
    margin: 0 10px;
    display: inline-block;
    align-items: center;
  }
  ::v-deep .el-pager li.active {
    color: #fff;
    background: #1890ff;
    cursor: default;
  }
  ::v-deep .el-pager li {
    margin: 0 2px;
    border-radius: 50% !important;
    width: 30px !important;
    height: 30px !important;
    line-height: 30px !important;
    min-width: auto !important;
    padding: 0 !important;
  }

  ::v-deep .btn-prev {
    border-radius: 50% !important;
    width: 28px !important;
    padding: 0 !important;
  }
  ::v-deep .btn-next {
    border-radius: 50% !important;
    width: 28px !important;
    padding: 0 !important;
  }
}
</style>
