<template>
  <div id="index" ref="appRef">
    <div class="bg">
      <dv-loading v-show="loading">Loading...</dv-loading>
      <ScreenHeader :route="'device'" />
      <dv-border-box-10 class="h40 device_header">
        <div class="header_box">
          <div
            class="item"
            v-for="(item, i) in $store.state.headerList"
            :key="i"
          >
            <el-badge :value="item.value">
              <p @click="handleStatus(item.title)">{{ item.title }}</p>
            </el-badge>
          </div>
        </div>
      </dv-border-box-10>
      <div class="device_box">
        <el-carousel
          height="100%"
          trigger="click"
          arrow="never"
          :interval="5000"
        >
          <el-carousel-item
            class="device_content"
            v-for="item in pageNum"
            :key="item.id"
          >
            <div
              class="device_item"
              v-for="v in $store.state.list.slice((item - 1) * 10, item * 10)"
              :key="v.id"
            >
              <img
                v-lazy="require('@/assets/img/Multi.png')"
                alt="图片"
                @click="goDetail(v.id)"
              />
              <p class="device_desc">{{ v.title }}----{{ v.status }}</p>
              <p class="item">
                <el-progress
                  :text-inside="true"
                  :stroke-width="26"
                  :percentage="v.progress"
                ></el-progress>
              </p>
              <p class="item">
                <el-progress
                  status="success"
                  :text-inside="true"
                  :stroke-width="26"
                  :percentage="v.progress1"
                ></el-progress>
              </p>
              <dl class="status">
                <template>
                  <span
                    v-for="(v1, i) in v.state"
                    :key="i"
                    :style="{ background: v1.bool == true ? '' : 'red' }"
                    >{{ i + 1 }}</span
                  >
                </template>
              </dl>
            </div>
          </el-carousel-item>
        </el-carousel>
        <!-- <div class="device_content">
          <div
            class="device_item"
            v-for="item in $store.state.list"
            :key="item.id"
          >
            <img
              v-lazy="require('@/assets/img/Multi.png')"
              alt="图片"
              @click="goDetail(item.id)"
            />
            <p class="device_desc">{{ item.title }}</p>
            <p class="item">
              <el-progress
                :text-inside="true"
                :stroke-width="26"
                :percentage="item.progress"
              ></el-progress>
            </p>
            <p class="item">
              <el-progress
                status="success"
                :text-inside="true"
                :stroke-width="26"
                :percentage="item.progress1"
              ></el-progress>
            </p>
            <dl class="status">
              <template v-for="(v, i) in item.state">
                <span :style="{ background: v.bool == true ? '' : 'red' }">{{
                  i + 1
                }}</span>
              </template>
            </dl>
          </div>
        </div> -->
      </div>
    </div>
  </div>
</template>
<script>
import ScreenHeader from "../components/header";

export default {
  name: "DeviceView",
  components: { ScreenHeader },
  data() {
    return {
      loading: true,
      list: [],
      timer: null,
      pageNum: 0,
    };
  },
  watch: {
    "$store.state.list": {
      handler(val) {
        this.pageNum = Math.ceil(val?.length / 10); //默认一页10条数据
      },
    },
  },
  created() {
    this.$store.dispatch("getDeviceList");
  },

  // 定时刷新1分钟
  mounted() {
    this.timer = setInterval(() => {
      this.$store.dispatch("getDeviceList");
    }, 60000);
  },
  destroyed() {
    clearInterval(this.timer);
    this.timer = null;
  },
  methods: {
    goDetail(id) {
      // 保存id
      // this.$store.dispatch('changeDeviceId',id)
      this.$router.push("/detail");
    },
    handleStatus(val) {
      this.$store.dispatch("getDeviceList", val);
    },
  },
};
</script>
<style scoped lang="scss">
.device_header {
  margin-bottom: 10px;
  .header_box {
    padding-top: 5px;
    height: 40px;
    display: flex;
    align-items: center;
    .item {
      margin: 0 10px;
      p {
        font-size: 16px;
        padding: 0 10px;
      }
    }
    .item p:hover {
      cursor: pointer;
    }
  }
}

.device_box {
  width: 100%;
  height: 90%;
  overflow-y: auto;
  // 详情页图片
  .device_content {
    height: 95%;
    padding-bottom: 20px;
    /* 声明一个容器 */
    display: grid;
    /*  声明列的宽度  */
    grid-template-columns: repeat(5, 19.5%);
    /*  声明行高  */
    grid-template-rows: 50%;
    /*  声明行间距和列间距  */
    grid-gap: 10px;
    .device_item {
      border: #008cff 1px solid;
      padding: 15px;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      img {
        width: 100%;
        height: 240px;
        background: #fff;
      }
      .device_desc {
        margin-top: 20px;
        width: 100%;
        display: flex;
        align-items: center;
        font-size: 20px;
        height: 20px;
      }
      .item {
        width: 100%;
        height: 20px;
        margin-top: 20px;
      }
      .status {
        width: 100%;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-top: 20px;
        span {
          background: orange;
          height: 100%;
          width: 70px;
          text-align: center;
          line-height: 40px;
        }
      }
    }
  }
}
::-webkit-scrollbar {
  width: 10px;
}

::-webkit-scrollbar-thumb {
  background: rgba(65, 105, 225, 0.3);
  border-radius: 5px;
}
</style>
