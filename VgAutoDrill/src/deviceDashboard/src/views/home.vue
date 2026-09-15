<template>
  <v-scale-screen ref="scale-screen" width="1920" height="1080">
    <div class="bg">
      <myHeader />
      <div
        class="main"
        @mouseenter="() => (showBtn = true)"
        @mouseleave="() => (showBtn = false)"
      >
        <el-button
          icon="el-icon-arrow-left"
          circle
          class="left_btn btn"
          @click="prePage"
          :disabled="currentPage == 0"
          v-if="pageNum > 1 && showBtn"
        ></el-button>

        <el-button
          icon="el-icon-arrow-right"
          circle
          class="right_btn btn"
          @click="nextPage"
          v-if="pageNum > 1 && showBtn"
          :disabled="currentPage == pageNum - 1"
        >
        </el-button>
        <div class="box" v-for="item in dataShow" :key="item.deviceCode">
          <div
            class="bg_border"
            :style="{
              border: item.not == 'nothing' ? '4px solid red' : '',
              background: getStatus(item.deviceStatus),
            }"
            :class="item.deviceStatus == 4 ? ' breathe-div' : ''"
          >
            <div class="device_code">{{ item.deviceCode }}</div>
            <div class="device_content">
              <li>状态：{{ getStatusText(item.deviceStatus) }}</li>
              <li>
                稼动率：
                <span v-if="item.duty != undefined">{{ item.duty }}%</span>
              </li>
              <li>
                稼动率达成率：
                <span v-if="item.dutyRate != undefined"
                  >{{ item.dutyRate }}%</span
                >
              </li>
              <li>
                当前料号：<span class="item_code" :title="item.nowItemCode">
                  {{ item.nowItemCode }}</span
                >
              </li>
              <li>
                下一料号：<span class="item_code" :title="item.nextItemCode">
                  {{ item.nextItemCode }}</span
                >
              </li>
              <li>
                <el-progress
                  :percentage="item.percentage"
                  style="width: 100%; padding-bottom: 8px"
                ></el-progress>
              </li>
            </div>
          </div>
        </div>
      </div>
    </div>
  </v-scale-screen>
</template>
    
    <script>
import { getDeviceDatas } from "@/api";
import header from "@/components/header";
import data from "@/mock";
import VScaleScreen from "v-scale-screen";
export default {
  components: { myHeader: header, VScaleScreen },
  data() {
    return {
      list: [],
      // 是否显示button
      showBtn: true,
      totalPage: [], // 所有分页的数据
      pageSize: 30, // 每页显示数量
      pageNum: 1, // 共几页=所有数据/每页现实数量
      dataShow: [], // 当前显示的数据
      currentPage: 0, // 默认当前显示第一页
    };
  },
  computed: {
    getStatus() {
      return (item) => {
        let str = "";
        switch (item) {
          case 0:
            str = "#36a3f7";
            break;
          case 1:
            str = "#666";
            break;
          case 2:
            str = "#FF7301";
            break;
          case 3:
            str = "green";
            break;
          case 4:
            str = "red";
            break;
          case 5:
            str = "rgb(116, 30, 30)";
            break;
        }

        return str;
      };
    },
    getStatusText() {
      return (item) => {
        let str = "";
        switch (item) {
          case 0:
            str = "在线";
            break;
          case 1:
            str = "离线";
            break;
          case 2:
            str = "待机";
            break;
          case 3:
            str = "运行中";
            break;
          case 4:
            str = "报警中";
            break;
          case 5:
            str = "下一趟没有排料";
            break;
        }

        return str;
      };
    },
  },
  created() {
    this.getList();
  },
  methods: {
    getList() {
      getDeviceDatas().then((res) => {
        this.list = res.data;
        this.pageNum = Math.ceil(this.list.length / this.pageSize) || 1; //计算有多少页数据，默认为1
        // 循环页面
        for (let i = 0; i < this.pageNum; i++) {
          // 每一页都是一个数组 形如 [['第一页的数据'],['第二页的数据'],['第三页数据']]
          // 根据每页显示数量 将后台的数据分割到 每一页,假设pageSize为2， 则第一页是1-2条，即slice(0,2)，第二页是3-4条，即slice(3,4)以此类推
          this.totalPage[i] = this.list.slice(
            this.pageSize * i,
            this.pageSize * (i + 1)
          );
        }

        // 获取到数据后默认显示第一页内容
        this.dataShow = this.totalPage[this.currentPage];
        console.log(this.dataShow);
      });
    },
    // 下一页
    nextPage() {
      if (this.currentPage === this.pageNum - 1) return;
      this.dataShow = this.totalPage[++this.currentPage];
    },
    // 上一页
    prePage() {
      if (this.currentPage === 0) return;
      this.dataShow = this.totalPage[--this.currentPage];
    },
  },
};
</script>
    <style lang="scss" scoped>
.color {
  color: #fff;
}
.main {
  width: 100%;
  height: calc(100% - 60px);
  padding: 0 20px 15px 20px;
  padding-top: 0px;
  position: relative;
  .btn {
    font-size: 40px;
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    z-index: 20;
    border-radius: 50%;
    border: none;
    color: #fff;
    background: rgba(0, 0, 0, 0.4);
  }
  .left_btn {
    left: 0;
  }
  .right_btn {
    right: 0;
  }

  .bg_border {
    overflow: hidden;
    width: 100%;
    height: 100%;
    border-radius: 10px;
    position: relative;
    border: solid 3px transparent;

    .device_code {
      height: 40px;
      line-height: 40px;
      //   background: green;
      padding: 0 10px;
      font-size: 24px;
    }

    .device_content {
      padding: 0 10px;
      li {
        line-height: 28px;
        display: flex;
        font-size: 18px;
      }
      .item_code {
        flex: 1;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }
    }
  }

  .box {
    padding: 8px;
    width: calc(100% / 6);
    display: flex;
    display: inline-block;
  }
}

.breathe-div {
  animation: mymove1 1s infinite;
}

@keyframes mymove1 {
  from {
    box-shadow: 0 0 20px 15px yellow;
  }
  to {
    box-shadow: 0 0 10px 7px yellow;
  }
}

::v-deep .el-progress-bar__outer {
  height: 18px !important;
}

::v-deep .el-progress__text {
  color: #fff !important;
}
</style> 