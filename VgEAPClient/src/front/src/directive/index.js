import debounce from "./module/debounce";
import wResize from "./module/wResize";
import prevent from "./module/prevent";

const install = function (Vue) {
  Vue.directive("debounce", debounce);
  Vue.directive("wResize", wResize);
  Vue.directive("prevent", prevent);
};

if (window.Vue) {
  Vue.use(install); // eslint-disable-line
}

export default install;
