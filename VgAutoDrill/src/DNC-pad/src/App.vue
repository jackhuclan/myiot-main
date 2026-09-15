<script>
import config from "@/admin.config.js";
import routingIntercept from "@/utils/intercept.js";
	import { uniAdminCacheKey } from './store/constants.js'
import {    mapMutations } from "vuex";
export default {
  methods: {
    ...mapMutations("app", ["SET_THEME"]),
  },
  onLaunch: function () {
    // theme
    this.SET_THEME(uni.getStorageSync(uniAdminCacheKey.theme) || "default");
    // #ifndef APP
    uni.hideTabBar();
    // #endif
  },
  onShow() {
    routingIntercept();
  },
  onPageNotFound(msg) {
    uni.redirectTo({
      url: config.error.url,
    });
  },
};
</script>
<style lang="scss">
@import "@/common/uni.css";
@import "@/common/uni-icons.css";
@import "@/common/admin-icons.css";
@import "@/common/theme.scss";
</style>
