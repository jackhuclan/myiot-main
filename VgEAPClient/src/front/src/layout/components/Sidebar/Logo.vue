<template>
  <div
    class="sidebar-logo-container"
    :class="{ collapse: collapse }"
    :style="{
      backgroundColor:
        sideTheme === 'theme-dark'
          ? variables.menuBackground
          : variables.menuLightBackground,
    }"
  >
    <transition name="sidebarLogoFade">
      <router-link
        v-if="collapse"
        key="collapse"
        class="sidebar-logo-link"
        to="/"
      >
        <img v-if="logo" :src="logo" class="sidebar-logo" />
        <h1
          v-else
          class="sidebar-title"
          :style="{
            color:
              sideTheme === 'theme-dark'
                ? variables.logoTitleColor
                : variables.logoLightTitleColor,
          }"
        >
          {{ title }}
        </h1>
      </router-link>
      <router-link v-else key="expand" class="sidebar-logo-link" to="/">
        <img v-if="logo" :src="logo" class="sidebar-logo" />
        <h1
          class="sidebar-title"
          :style="{
            color:
              sideTheme === 'theme-dark'
                ? variables.logoTitleColor
                : variables.logoLightTitleColor,
          }"
        >
          {{ title }}
        </h1>
      </router-link>
    </transition>
  </div>
</template>

<script>
import logoImg from "@/assets/logo/logo.png";
import variables from "@/assets/styles/variables.scss";

export default {
  name: "SidebarLogo",
  props: {
    collapse: {
      type: Boolean,
      required: true,
    },
  },
  computed: {
    variables() {
      return variables;
    },
    sideTheme() {
      return this.$store.state.settings.sideTheme;
    },
  },
  data() {
    return {
      title: "维嘉数控",
      logo: logoImg,
    };
  },
};
</script>

<style lang="scss" scoped>
.sidebarLogoFade-enter-active {
  transition: opacity 1.5s;
}

.sidebarLogoFade-enter,
.sidebarLogoFade-leave-to {
  opacity: 0;
}

.sidebar-logo-container {
  position: relative;
  width: 100%;
  height: 50px;
  line-height: 50px;
  background: #2b2f3a;
  // text-align: center;
  overflow: hidden;

  & .sidebar-logo-link {
    height: 100%;
    width: 100%;
    // display: flex;
    // align-items: center;
    // justify-content: center;
    // text-align: center;
    text-align: center;

    & .sidebar-logo {
      width: 50px;
      vertical-align: middle;
      margin-right: 5px;
      object-fit: fill;
      object-fit: contain;
      object-fit: scale-down;
      background: transparent;
    }

    & .sidebar-title {
      // overflow: hidden; /*超出部分隐藏*/
      // white-space: nowrap; /*禁止换行*/
      // text-overflow: ellipsis; /*省略号*/
      // width: calc(100% - 44px);
      display: inline-block;
      // margin: 0;
      color: #fff;
      font-weight: 600;
      // line-height: 50px;
      font-size: 16px;
      font-family: Avenir, Helvetica Neue, Arial, Helvetica, sans-serif;
      vertical-align: middle;
    }
    // .sidebar-title:hover {
    //   text-overflow: inherit;
    //   overflow: visible;
    //   white-space: pre-line;
    // }
  }

  &.collapse {
    .sidebar-logo {
      margin-right: 0px;
      width: 40px;
      background: transparent;
    }
  }
}
</style>
