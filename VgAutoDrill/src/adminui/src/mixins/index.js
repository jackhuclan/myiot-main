import Cookies from "js-cookie";

export default {
  data() {
    return {
      timeStamp: "",
    };
  },
  methods: {
    getColumnsVisible(key){
      return this.columns.find(v=>v.key==key)?.visible
    },
    // 点击搜索下拉框x清除
    clearQueryParams(val) {
      this.queryParams[val] = undefined;
    },
    // 点击计数器
    changeNum(params) {
      this.$set(this.form, params.str, params.value);
    },
  },
};
