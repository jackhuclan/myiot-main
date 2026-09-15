/**
 * 去除两边空格
 * 使用 <el-input v-model="xxx" v-trim></el-input>
 */
function getInput(el) {
  let inputEle;
  if (el.tagName !== "INPUT") {
    inputEle = el.querySelector("input");
  } else {
    inputEle = el;
  }
  return inputEle;
}
function dispatchEvent(el, type) {
  let evt = document.createEvent("HTMLEvents");
  evt.initEvent(type, true, true);
  el.dispatchEvent(evt);
}
const Trim = {
  inserted: (el) => {
    let inputEle = getInput(el);
    const handler = function (event) {
      //兼容浏览器的事件
      event = event || window.event;
      const newVal = event.target.value.trim();
      if (event.target.value != newVal) {
        event.target.value = newVal;
        dispatchEvent(inputEle, "input");
      }
    };
    el.inputEle = inputEle;
    el._blurHandler = handler;
    inputEle.addEventListener("blur", handler);
    inputEle.addEventListener("keyup", (event) => {
      //兼容浏览器的事件
      let theEvent = event || window.event;
      //兼容各浏览器的键盘事件
      let keyCode = theEvent.keyCode || theEvent.which || theEvent.charCode;
      theEvent.preventDefault();
      if (keyCode === 13) {
        handler(event);
      }
    });
  },
  unbind(el) {
    const { inputEle } = el;
    inputEle.removeEventListener("blur", el._blurHandler);
  },
};
Trim.install = function (Vue) {
  Vue.directive("trim", Trim);
};
export default Trim;
