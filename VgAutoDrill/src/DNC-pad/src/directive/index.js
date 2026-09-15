import dialogColse from "./popup/close";
const install = function (Vue) {
  Vue.directive("dialogColse", dialogColse);
};

if (window.Vue) {
  Vue.use(install); // eslint-disable-line
}

export default install;
