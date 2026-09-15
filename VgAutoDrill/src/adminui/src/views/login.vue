<template>
  <div class="login">
    <el-form
      ref="loginForm"
      :model="loginForm"
      :rules="loginRules"
      class="login-form"
    >
      <h3 class="title">
        <img :src="logo" class="login-logo" />

        {{ htmlTitle }}
      </h3>
      <el-form-item prop="username">
        <el-input
          v-model="loginForm.username"
          type="text"
          auto-complete="off"
          placeholder="账号"
          autofocus
        >
          <svg-icon
            slot="prefix"
            icon-class="user"
            class="el-input__icon input-icon"
          />
        </el-input>
      </el-form-item>
      <el-form-item prop="password">
        <el-input
          class="pwd"
          v-model="loginForm.password"
          type="text"
          auto-complete="off"
          placeholder="密码"
          @keyup.enter.native="handleLogin"
        >
          <svg-icon
            slot="prefix"
            icon-class="password"
            class="el-input__icon input-icon"
          />
        </el-input>
      </el-form-item>
      <el-checkbox
        v-model="loginForm.rememberMe"
        style="margin: 0px 0px 25px 0px"
        >记住密码</el-checkbox
      >
      <el-form-item style="width: 100%">
        <el-button
          v-debounce
          :loading="loading"
          size="medium"
          type="primary"
          style="width: 100%"
          @click.native.prevent="handleLogin"
        >
          <span v-if="!loading">登 录</span>
          <span v-else>登 录 中...</span>
        </el-button>
        <div style="float: right" v-if="register">
          <router-link class="link-type" :to="'/register'"
            >立即注册</router-link
          >
        </div>
      </el-form-item>
    </el-form>
    <!--  底部  -->
    <div class="el-login-footer">
      <span
        >Copyright © 2022-{{ new Date().getFullYear() }} {{ htmlTitle }} All
        Rights Reserved.</span
      >
      <span>Version:{{ version }}</span>
      <!-- <span @click="toIPC">陕ICP备2022002135号-1</span> -->
    </div>
  </div>
</template>

<script>
import md5 from "js-md5";
import Cookies from "js-cookie";
import { encrypt, decrypt } from "@/utils/jsencrypt";
import logoImg from "@/assets/logo/logo.png";
// 版本号
import { version } from "../../public/version.json";
export default {
  name: "Login",
  data() {
    return {
      logo: logoImg,
      loginForm: {
        username: "",
        password: "",
        rememberMe: false,
        uuid: "",
      },
      loginRules: {
        username: [
          { required: true, trigger: "blur", message: "请输入您的账号" },
        ],
        password: [
          { required: true, trigger: "blur", message: "请输入您的密码" },
        ],
      },
      version: version,
      loading: false,
      // 验证码开关
      captchaEnabled: false,
      // 注册开关
      register: false,
      redirect: undefined,
    };
  },
  watch: {
    $route: {
      handler: function (route) {
        this.redirect = route.query && route.query.redirect;
      },
      immediate: true,
    },
  },
  computed: {
    htmlTitle() {
      return this.$store.state.settings.htmlTitle;
    },
  },
  created() {
    this.getCookie();
  },
  methods: {
    getCookie() {
      const username = Cookies.get("username");
      const password = Cookies.get("password");
      const rememberMe = Cookies.get("rememberMe");
      this.loginForm = {
        username: username === undefined ? this.loginForm.username : username,
        password:
          password === undefined ? this.loginForm.password : decrypt(password),
        rememberMe: rememberMe === undefined ? false : Boolean(rememberMe),
      };
    },
    handleLogin() {
      this.$refs.loginForm.validate((valid) => {
        if (valid) {
          this.loading = true;
          if (this.loginForm.rememberMe) {
            Cookies.set("username", this.loginForm.username, { expires: 30 });
            Cookies.set("password", encrypt(this.loginForm.password), {
              expires: 30,
            });
            Cookies.set("rememberMe", this.loginForm.rememberMe, {
              expires: 30,
            });
          } else {
            Cookies.remove("username");
            Cookies.remove("password");
            Cookies.remove("rememberMe");
          }
          this.loading = false;
          this.$store
            .dispatch("Login", {
              ...this.loginForm,
              password: md5(this.loginForm.password),
            })
            .then((res) => {
              // 是否存在记录的退出前的路由
              const curr = this.$cache.session.get("preRoute");
              if (curr == null) {
                this.$cache.session.remove("tabViews");
                this.$router.push({ path: this.redirect || "/" });
              } else {
                // 清除 TagsView
                if (curr == "/") this.$cache.session.remove("tabViews");
                this.$router.push({ path: curr });
              }
              this.$notify({
                type: "success",
                title: "提示",
                message: "登录成功",
                duration: 1000, //1秒后自动关闭
              });
            })
            .catch((err) => {
              if (err.code == 1)
                this.$notify({
                  type: "error",
                  title: "提示",
                  message: err.message,
                  duration: 1000, //1秒后自动关闭
                });
              this.loading = false;
            });
        }
      });
    },
    toIPC() {
      window.open("https://beian.miit.gov.cn/", "_blank");
    },
  },
};
</script>

<style rel="stylesheet/scss" lang="scss">
.login {
  // display: flex;
  // flex-direction: column;
  // justify-content: center;
  // align-items: center;
  position: relative;
  height: 100%;
  background-image: url("../assets/images/login-background.jpg");
  background-size: cover;
}
.title {
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0px auto 20px auto;
  color: #707070;
  .login-logo {
    margin-top: -2px;
    width: 60px;
    // vertical-align: middle;
    margin-right: 8px;
    margin-left: -30px;
    object-fit: fill;
    object-fit: contain;
    object-fit: scale-down;
    background: transparent;
  }
}

.login-form {
  border-radius: 6px;
  background: #ffffff;
  width: 400px;
  padding: 25px 25px 5px 25px;
  // margin: 0 auto;
  position: absolute;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
  .el-input {
    height: 38px;
    input {
      height: 38px;
    }
  }
  .input-icon {
    height: 39px;
    width: 14px;
    margin-left: 2px;
  }
}
.login-tip {
  font-size: 13px;
  text-align: center;
  color: #bfbfbf;
}
.login-code {
  width: 33%;
  height: 38px;
  float: right;
  img {
    cursor: pointer;
    vertical-align: middle;
  }
}
.el-login-footer {
  height: 80px;
  line-height: 80px;
  position: absolute;
  bottom: 0px;
  width: 100%;
  text-align: center;
  color: #fff;
  font-family: Arial;
  font-size: 12px;
  letter-spacing: 1px;
}
.login-code-img {
  height: 38px;
}

.pwd {
  -webkit-text-security: disc;
}
</style>
