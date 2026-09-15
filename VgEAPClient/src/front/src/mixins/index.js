import Cookies from "js-cookie";

export default {
  methods: {
    // 点击搜索下拉框x清除
    clearQueryParams(val) {
      this.queryParams[val] = undefined;
    },
  },
};
