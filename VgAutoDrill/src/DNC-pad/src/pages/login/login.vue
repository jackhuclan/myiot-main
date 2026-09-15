<!-- 账号密码登录页 -->
<template>
  <view class="uni-content">
    <view class="login-logo">
      <image :src="logo"></image>
    </view>
    <!-- 顶部文字 -->
    <text class="title title-box">账号密码登录</text>
    <uni-forms>
      <uni-forms-item name="username">
        <uni-easyinput
          prefixIcon="auth"
          :focus="focusUsername"
          @blur="focusUsername = false"
          class="input-box"
          :inputBorder="false"
          v-model="username"
          placeholder="请输入账号"
        />
      </uni-forms-item>
      <uni-forms-item name="password">
        <uni-easyinput
          prefixIcon="locked"
          :focus="focusPassword"
          @blur="focusPassword = false"
          class="input-box"
          clearable
          type="password"
          :inputBorder="false"
          v-model="password"
          placeholder="请输入密码"
        />
      </uni-forms-item>
      <!--  #ifdef  H5 -->
      <view class="remember-psw-container">
        <label class="remember-psw">
          <checkbox
            value="psw"
            :checked="rememberMe"
            @tap="rememberMe = !rememberMe"
            color="#09CC86"
            class="checkbox"
          />记住账号密码
          <text class="checkbox-label"></text>
        </label>
      </view>
      <!--  #endif -->
    </uni-forms>
    <button class="uni-btn" type="primary" @click="handleLogin">登录</button>
    <!--  #ifdef  H5 -->
    <text class="version">
      <!-- Copyright © {{ new Date().getFullYear() }} VEGA All Rights Reserved.  -->
      Version:{{ version }}</text
    >
    <!--  #endif -->
  </view>
</template>

<script>
import md5 from "js-md5";
import Cookies from "js-cookie";
import { encrypt, decrypt } from "@/utils/jsencrypt";
// 版本号
import { version } from "../../../public/version.json";
export default {
  data() {
    return {
      password: "",
      username: "",
      rememberMe: false,
      focusUsername: false,
      focusPassword: false,
      logo: "/static/logo.png",
      version: version,
    };
  },
  onShow() {
    // #ifdef H5
    window.addEventListener("keydown", this.keyDown); //监听用户回车事件
    // #endif
  },
  mounted() {
    // #ifdef H5
    this.getCookie();
    // #endif
  },
  onUnload() {
    // #ifdef H5
    window.removeEventListener("keydown", this.keyDown, false);
    // #endif
  },
  methods: {
    getCookie() {
      const username = Cookies.get("uni-username");
      const password = Cookies.get("uni-password");
      const rememberMe = Cookies.get("uni-rememberMe");
      this.username = username === undefined ? this.username : username;
      this.password =
        password === undefined ? this.password : decrypt(password);
      this.rememberMe = rememberMe === undefined ? false : Boolean(rememberMe);
    },
    // 回车登录
    keyDown(e) {
      if (e.keyCode == 13) {
        //13是回车键的keycode
        this.handleLogin();
      }
    },
    /**
     * 密码登录
     */
    handleLogin() {
      if (!this.username.length) {
        this.focusUsername = true;
        this.$modal.msgError("请输入账号");
      } else if (!this.password.length) {
        this.focusPassword = true;
        this.$modal.msgError("请输入密码");
      } else {
        // #ifdef H5

        if (this.rememberMe) {
          Cookies.set("uni-username", this.username, { expires: 30 });
          Cookies.set("uni-password", encrypt(this.password), {
            expires: 30,
          });
          Cookies.set("uni-rememberMe", this.rememberMe, {
            expires: 30,
          });
        } else {
          Cookies.remove("uni-username");
          Cookies.remove("uni-password");
          Cookies.remove("uni-rememberMe");
        }
        // #endif
        this.pwdLogin();
      }
    },
    // 密码登录
    pwdLogin() {
      this.$store
        .dispatch("Login", {
          username: this.username,
          password: md5(this.password),
        })
        .then((res) => {
          this.$modal.loading("登录中，请耐心等待...");
          setTimeout(() => {
            this.loginSuccess();
            this.$modal.closeLoading();
          }, 500);
        })
        .catch((res) => {
          this.$modal.showToast(res.message);
        });
    },
    // 登录成功后，处理函数
    loginSuccess(result) {
      // 设置用户信息
      this.$store.dispatch("GetInfo").then((res) => {
        this.$tab.reLaunch("/pages/index/index");
      });
    },
  },
};
</script>

<style lang="scss" scoped>
// 隐藏 edge 浏览器的密码查看按钮
/* #ifdef H5 */
.input-box ::v-deep {
  .uni-input-input[type="password"] {
    &::-ms-reveal {
      display: none;
    }
  }
}
/* #endif */

.uni-content {
  padding: 0 60rpx;
  position: relative;
}

.login-logo {
  display: none;
}

/* #ifndef APP-NVUE */
@media screen and (min-width: 690px) {
  .uni-content {
    /* #ifndef H5 */
    padding: 0;
    max-width: 300px;
    margin-left: calc(50% - 200px);
    /* #endif */
    /* #ifdef H5 */
    margin: 0 auto;
    position: relative;
    top: 100px;
    padding: 30px 40px 80px 40px;
    max-width: 450px;
    max-height: 450px;
    border-radius: 10px;
    box-shadow: 0 0 20px #efefef;
    background-color: #fff;
    // 版本号显示
    .version {
      height: 40px;
      line-height: 40px;
      text-align: center;
      position: absolute;
      bottom: 10px;
      font-family: Arial;
      font-size: 12px;
      letter-spacing: 1px;
    }
    /* #endif */
  }

  /* #ifdef H5 */
  .login-logo {
    display: flex;
    justify-content: center;
  }

  .login-logo image {
    width: 60px;
    height: 60px;
  }

  uni-button {
    padding-bottom: 1px;
  }

  /* #endif */
}

.uni-content view {
  box-sizing: border-box;
}

/* #endif */

.title {
  /* #ifndef APP-NVUE */
  display: flex;
  /* #endif */
  padding: 18px 0;
  font-weight: 800;
  flex-direction: column;
}

/* #ifndef APP-NVUE */
// 解决小程序端开启虚拟节点virtualHost引起的 class = input-box丢失的问题 [详情参考](https://uniapp.dcloud.net.cn/matter.html#%E5%90%84%E5%AE%B6%E5%B0%8F%E7%A8%8B%E5%BA%8F%E5%AE%9E%E7%8E%B0%E6%9C%BA%E5%88%B6%E4%B8%8D%E5%90%8C-%E5%8F%AF%E8%83%BD%E5%AD%98%E5%9C%A8%E7%9A%84%E5%B9%B3%E5%8F%B0%E5%85%BC%E5%AE%B9%E9%97%AE%E9%A2%98)
.uni-content ::v-deep .uni-easyinput__content,
	/* #endif */

 .input-box {
  height: 44px;
  background-color: #f8f8f8 !important;
  border-radius: 0;
  font-size: 14px;
  /* #ifndef APP-NVUE */
  display: flex;
  /* #endif */
  flex: 1;
}

.uni-content ::v-deep .uni-forms-item__inner {
  padding-bottom: 8px;
}

.uni-btn {
  text-align: center;
  height: 40px;
  line-height: 40px;
  margin: 15px 0 10px 0;
  color: #fff !important;
  border-radius: 5px;
  font-size: 16px;
}

.uni-body.uni_modules-uni-id-pages-pages-login-login-withoutpwd {
  height: auto !important;
}

@media screen and (min-width: 690px) {
  .uni-content {
    height: auto;
  }
}

// 以下是记住密码的css
.remember-psw-container {
  display: flex;
  justify-content: flex-start; /* 左对齐 */
  align-items: center; /* 垂直居中 */
  margin-top: 10rpx;
}

.remember-psw {
  display: flex;
  align-items: center; /* 垂直居中 */
}

.checkbox {
  margin-right: 5rpx; /* 文字与复选框之间的间距 */
}

.checkbox-label {
  font-size: 16px; /* 文字大小 */
  color: #333; /* 文字颜色 */
}
::v-deep uni-checkbox .uni-checkbox-input {
  width: 18px !important;
  height: 18px !important;
}
::v-deep uni-checkbox .uni-checkbox-input.uni-checkbox-input-checked:before {
  font-size: 18px !important;
  transform: translate(-45%, -50%) scale(0.73) !important;
  -webkit-transform: translate(-45%, -50%) scale(0.73) !important;
}
</style>
