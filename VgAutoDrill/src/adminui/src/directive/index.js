import hasRole from "./permission/hasRole";
import hasPermi from "./permission/hasPermi";
import dialogDrag from "./dialog/drag";
import dialogDragWidth from "./dialog/dragWidth";
import dialogDragHeight from "./dialog/dragHeight";
import dialogClose from "./dialog/dialogClose";
import clipboard from "./module/clipboard";
import debounce from "./debounce";
import draggable from "./draggable";
import Trim from "./trim";
import RemoveAriaHidden from "./removeAriaHidden";
import isGetSelection from "./isGetSelection";
import optionTitle from "./optionTitle";
import affix from "./affix";
import tableHeight from "./tableHeight";
const install = function (Vue) {
  Vue.directive("hasRole", hasRole);
  Vue.directive("hasPermi", hasPermi);
  Vue.directive("clipboard", clipboard);
  Vue.directive("dialogDrag", dialogDrag);
  Vue.directive("debounce", debounce);
  Vue.directive("draggable", draggable);
  Vue.directive("dialogDragWidth", dialogDragWidth);
  Vue.directive("dialogDragHeight", dialogDragHeight);
  // 关闭新增编辑框
  Vue.directive("dialogClose", dialogClose);
  // 去除input两边空格
  Vue.directive("trim", Trim);
  Vue.directive("removeAriaHidden", RemoveAriaHidden);
  Vue.directive("isGetSelection", isGetSelection);
  Vue.directive("optionTitle", optionTitle);
  Vue.directive("affix", affix);
  Vue.directive("tableHeight", tableHeight);
};

if (window.Vue) {
  window["hasRole"] = hasRole;
  window["hasPermi"] = hasPermi;
  Vue.use(install); // eslint-disable-line
}

export default install;
