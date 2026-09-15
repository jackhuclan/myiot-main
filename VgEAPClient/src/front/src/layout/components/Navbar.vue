<template>
  <div
    class="navbar"
    :style="{
      backgroundColor:
        settings.sideTheme === 'theme-dark' && topNav
          ? variables.menuBackground
          : variables.menuLightBackground,
    }"
  >
    <hamburger
      id="hamburger-container"
      :is-active="sidebar.opened"
      class="hamburger-container"
      @toggleClick="toggleSideBar"
      v-if="!topNav"
    />

    <breadcrumb
      id="breadcrumb-container"
      class="breadcrumb-container"
      v-if="!topNav"
    />
    <!--   <top-nav id="topmenu-container" class="topmenu-container" v-if="topNav" />

 -->
    <div
      v-wResize="onMenuContResize"
      id="navbar-left"
      class="navbar__left"
      v-if="topNav"
    >
      <top-nav
        v-if="menuContWidth"
        id="topmenu-container"
        class="topmenu-container"
        :width="menuContWidth"
      />
    </div>
    <div class="right-menu">
      <screenfull id="screenfull" class="right-menu-item hover-effect" />
      <el-tooltip content="布局大小" effect="dark" placement="bottom">
        <size-select id="size-select" class="right-menu-item hover-effect" />
      </el-tooltip>
      <el-tooltip content="布局设置" effect="dark" placement="bottom">
        <span class="right-menu-item hover-effect" @click="setting = true">
          <svg-icon icon-class="theme"
        /></span>
      </el-tooltip>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import Breadcrumb from "@/components/Breadcrumb";
import TopNav from "@/layout/components/TopNav";
import Hamburger from "@/components/Hamburger";
import Screenfull from "@/components/Screenfull";
import SizeSelect from "@/components/SizeSelect"; 
import variables from "@/assets/styles/variables.scss";

export default {
  data() {
    return {
      menuContWidth: 0,
    };
  },
  components: {
    Breadcrumb,
    TopNav,
    Hamburger,
    Screenfull,
    SizeSelect, 
  },
  computed: {
    ...mapGetters(["sidebar", "device"]),
    setting: {
      get() {
        return this.$store.state.settings.showSettings;
      },
      set(val) {
        this.$store.dispatch("settings/changeSetting", {
          key: "showSettings",
          value: val,
        });
      },
    },
    settings() {
      return this.$store.state.settings;
    },
    variables() {
      return variables;
    },
    topNav: {
      get() {
        return this.$store.state.settings.topNav;
      },
    },
  },
  methods: {
    // 菜单容器宽度变化回调
    onMenuContResize({ width }) {
      this.menuContWidth = width;
    },
    toggleSideBar() {
      this.$store.dispatch("app/toggleSideBar");
    },
    async logout() {
      this.$confirm("确定注销并退出系统吗？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          this.$store.dispatch("LogOut").then(() => {
            // 退出登录与过期重新登陆区分（清除记录退出登陆前的路由信息）
            this.$cache.session.remove("preRoute");
            // 清除 TagsView
            this.$cache.session.remove("tabViews");
            location.href = "/";
          });
        })
        .catch(() => {});
    },
  },
};
</script>

<style lang="scss" scoped>
// .navbar {
//   height: 50px;
//   overflow: hidden;
//   position: relative;
//   background: #fff;
//   box-shadow: 0 1px 4px rgba(0, 21, 41, 0.08);

//   .hamburger-container {
//     line-height: 46px;
//     height: 100%;
//     float: left;
//     cursor: pointer;
//     transition: background 0.3s;
//     -webkit-tap-highlight-color: transparent;

//     &:hover {
//       background: rgba(0, 0, 0, 0.025);
//     }
//   }

//   .breadcrumb-container {
//     float: left;
//   }

//   .topmenu-container {
//     position: absolute;
//     left: 50px;
//   }

//   .errLog-container {
//     display: inline-block;
//     vertical-align: top;
//   }

//   .right-menu {
//     float: right;
//     height: 100%;
//     line-height: 50px;
//     margin-right: 10px;

//     &:focus {
//       outline: none;
//     }

//     .right-menu-item {
//       display: inline-block;
//       padding: 0 8px;
//       height: 100%;
//       font-size: 18px;
//       color: #5a5e66;
//       vertical-align: text-bottom;
//       &.msg {
//         font-size: 24px;
//         padding-right: 0;
//       }
//       &.hover-effect {
//         cursor: pointer;
//         transition: background 0.3s;

//         &:hover {
//           background: rgba(0, 0, 0, 0.025);
//         }
//       }
//     }

//     .avatar-container {
//       margin-right: 30px;

//       .avatar-wrapper {
//         > div {
//           font-size: 18px !important;
//         }
//         margin-top: 5px;
//         position: relative;

//         .user-avatar {
//           cursor: pointer;
//           width: 40px;
//           height: 40px;
//           border-radius: 10px;
//         }

//         .el-icon-caret-bottom {
//           cursor: pointer;
//           position: absolute;
//           right: -20px;
//           top: 25px;
//           font-size: 12px;
//         }
//       }
//     }
//   }
// }
.navbar {
  height: 50px;
  overflow: hidden;
  position: relative;
  background: #fff;
  box-shadow: 0 1px 4px rgba(0, 21, 41, 0.08);
  display: flex;

  &__left {
    flex: 1;
    overflow: hidden;
    margin-left: 10px;
    margin-right: 20px;
  }

  .hamburger-container {
    line-height: 46px;
    height: 100%;
    float: left;
    cursor: pointer;
    transition: background 0.3s;
    -webkit-tap-highlight-color: transparent;

    &:hover {
      background: rgba(0, 0, 0, 0.025);
    }
  }

  .breadcrumb-container {
    float: left;
  }

  .topmenu-container {
    display: inline-flex;
    align-items: center;
    height: 50px;
    border: none;
  }

  .errLog-container {
    display: inline-block;
    vertical-align: top;
  }

  .right-menu {
    flex-shrink: 0;
    margin-left: auto;
    height: 100%;
    display: flex;
    align-items: center;

    &:focus {
      outline: none;
    }

    .right-menu-item {
      display: inline-flex;
      align-items: center;
      padding: 0 8px;
      height: 100%;
      font-size: 18px;
      color: #5a5e66;

      &.hover-effect {
        cursor: pointer;
        transition: background 0.3s;

        &:hover {
          background: rgba(0, 0, 0, 0.025);
        }
      }
    }

    .avatar-container {
      margin-right: 30px;

      .avatar-wrapper {
        margin-top: 5px;
        position: relative;

        .user-avatar {
          cursor: pointer;
          width: 40px;
          height: 40px;
          border-radius: 10px;
        }

        .el-icon-caret-bottom {
          cursor: pointer;
          position: absolute;
          right: -20px;
          top: 25px;
          font-size: 12px;
        }
      }
    }
  }
}
</style>
