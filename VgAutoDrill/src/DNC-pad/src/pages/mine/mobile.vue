<template>
  <view class="mine-container" :style="{ height: `${windowHeight}px` }">
    <!--顶部个人信息栏-->
    <view class="header-section">
      <view class="flex padding justify-between">
        <view v-if="userInfo.userName" @click="handleToInfo" class="user-info">
          <view class="u_title"> 用户昵称：{{ userInfo.userName }}</view>
        </view>
        <view @click="handleLogout" class="font-16">
          <text>退出登录</text>
        </view>
      </view>
    </view>

    <view class="content-section">
      <view class="menu-list">
        <view class="list-cell list-cell-arrow" @click="handleToInfo">
          <view class="menu-item-box">
            <view class="iconfont icon-user menu-icon"></view>
            <view>个人信息</view>
          </view>
        </view>
        <view class="list-cell list-cell-arrow" @click="handleToEditInfo">
          <view class="menu-item-box">
            <view class="iconfont icon-user menu-icon"></view>
            <view>编辑资料</view>
          </view>
        </view>

        <view class="list-cell list-cell-arrow" @click="handleAbout">
          <view class="menu-item-box">
            <view class="iconfont icon-aixin menu-icon"></view>
            <view>关于我们</view>
          </view>
        </view>
        <view class="list-cell list-cell-arrow" @click="handleToSetting">
          <view class="menu-item-box">
            <view class="iconfont icon-setting menu-icon"></view>
            <view>应用设置</view>
          </view>
        </view>
      </view>
    </view>
  </view>
</template>

<script>
export default {
  computed: {
    userInfo() {
      return this.$store.state.user.userInfo;
    },
    windowHeight() {
      return uni.getSystemInfoSync().windowHeight - 50;
    },
  },
  methods: {
    handleToInfo() {
      this.$tab.navigateTo("/pages/mine/info/index");
    },
    handleToEditInfo() {
      this.$tab.navigateTo("/pages/mine/info/edit");
    },
    handleToSetting() {
      this.$tab.navigateTo("/pages/mine/setting/index");
    },
    handleAbout() {
      this.$tab.navigateTo("/pages/mine/about/index");
    },
    handleLogout() {
      this.$modal.confirm("确定注销并退出系统吗？").then(() => {
        this.$store
          .dispatch("LogOut")
          .then(() => {})
          .finally(() => {
            this.$tab.reLaunch("/pages/login/login");
          });
      });
    },
  },
};
</script>

<style lang="scss" scoped>
page {
  background-color: #f5f6f7;
}

.mine-container {
  width: 100%;
  height: 100%;

  .header-section {
    padding: 15px 15px 45px 15px;
    background-color: #3c96f3;
    color: white;

    .user-info {
      margin-left: 15px;

      .u_title {
        font-size: 18px;
        line-height: 30px;
      }
    }
  }

  .content-section {
    position: relative;
    top: -50px;

    .mine-actions {
      margin: 15px 15px;
      padding: 20px 0px;
      border-radius: 8px;
      background-color: white;

      .action-item {
        .icon {
          font-size: 28px;
        }

        .text {
          display: block;
          font-size: 13px;
          margin: 8px 0px;
        }
      }
    }
  }
}
</style>
