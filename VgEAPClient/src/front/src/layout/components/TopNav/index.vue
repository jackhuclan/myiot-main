<template>
  <el-menu
    class="topNav" 
    :default-active="activeMenu"
    :unique-opened="true"
    :collapse-transition="false"
    mode="horizontal"
    menu-trigger="hover"
    :background-color="
      settings.sideTheme === 'theme-dark' && topNav
        ? variables.menuBackground
        : variables.menuLightBackground
    "
    :text-color="
      settings.sideTheme === 'theme-dark' && topNav
        ? variables.menuColor
        : variables.menuLightColor
    "
  >
    <sidebar-item
      v-for="(route, index) in visibleRouters"
      :key="route.path + index"
      :item="route"
      :base-path="route.path"
    />
    <!-- 顶部菜单超出数量折叠 -->
    <el-submenu v-if="moreRouters.length" index="more">
      <template slot="title">
        <svg-icon icon-class="table" />
        <span>更多菜单</span>
      </template>
      <sidebar-item
        class="nest-menu"
        v-for="(route, index) in moreRouters"
        :key="route.path + index"
        :item="route"
        :base-path="route.path"
      />
    </el-submenu>
  </el-menu>
</template>

<script>
import { mapGetters } from "vuex";
import { deepClone } from "@/utils/index";
import SidebarItem from "./SidebarItem.vue";
import variables from "@/assets/styles/variables.scss";

export default {
  props: {
    width: {
      type: Number,
      default: 0,
    },
  },
  components: { SidebarItem },
  data() {
    return {
      // 顶部栏初始数
      visibleNumber: 5,
      menuKey: new Date().getTime(),
    };
  },
  computed: {
    ...mapGetters(["sidebarRouters", "moduleList", "moduleCode"]),
    theme() {
      return this.$store.state.settings.theme;
    },
    topNav() {
      return this.$store.state.settings.topNav;
    },
    settings() {
      return this.$store.state.settings;
    },
    variables() {
      return variables;
    },
    // 所有的路由信息
    routers() {
      // 过滤掉隐藏的路由
      return this.sidebarRouters.filter((item) => !item.hidden);
    },
    // 可视菜单路由
    visibleRouters() {
      const length = this.routers.length;
      if (length <= this.visibleNumber) {
        return deepClone(this.routers);
      }
      return this.routers.slice(0, this.visibleNumber - 1);
    },
    // 更多菜单路由
    moreRouters() {
      const length = this.routers.length;
      if (length <= this.visibleNumber) {
        return [];
      }
      return this.routers.slice(this.visibleNumber - 1, length);
    },
    // 默认激活的菜单
    activeMenu() {
      const route = this.$route;
      const { meta, path } = route;
      // if set path, the sidebar will highlight the path you set
      if (meta.activeMenu) {
        return meta.activeMenu;
      }
      return path;
    },
  },
  watch: {
    width: {
      handler(newVal) {
        this.setVisibleNumber(newVal);
        // 更新菜单组件key，避免当前菜单被折叠后，再次出现时未高亮显示
        this.menuKey = new Date().getTime();
      },
      immediate: true,
    },
  },
  mounted() {},
  methods: {
    // 根据宽度计算设置显示栏数
    setVisibleNumber(width) {
      // 112是取的菜单项最大宽度，此处还可进行优化
      this.visibleNumber = parseInt(width / 112);
    },
  },
};
</script>

<style lang="scss">
#topNav,
.el-menu--horizontal {
  .el-menu-item {
    // margin: 0 5px;
    &.is-active {
      color: #1890ff !important;
    }

    .svg-icon {
      margin-left: 0 !important;
      margin-right: 5px;
    }
  }

  .el-submenu {
    // padding: 0 10px !important;
    // margin: 0 5px;
    &.is-active {
      .el-submenu__title {
        color: #1890ff !important;
      }
    }

    .el-submenu__title {
      display: flex;
      align-items: center;
      padding: 0 20px !important;

      .svg-icon {
        margin-left: 0 !important;
        margin-right: 5px;
      }

      .el-submenu__icon-arrow {
        position: initial;
        margin-left: 5px;
        margin-top: 1px;
      }
    }
  }
  .el-menu--popup-bottom-start {
    margin-top: 0px;
  }
  .el-menu--popup {
    min-width: auto !important;
    .el-submenu__icon-arrow {
      margin-left: auto !important;
    }
  }
}
</style>
