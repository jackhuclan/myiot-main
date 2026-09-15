import { Message } from "element-ui";
export default {
  inserted(el, binding) {
    let isClick = true;
    let wait = binding.value; // 防抖时间
    if (!wait) {
      // 用户若不设置防抖时间，则默认1s
      wait = 1000;
    }
    let timer;
    el.addEventListener(
      "click",
      (event) => {
        if (!timer) {
          // 第一次执行: 不阻止click⌚️
          timer = setTimeout(() => {
            timer = null;
          }, wait);
        } else {
          clearTimeout(timer);
          timer = setTimeout(() => {
            timer = null;
          }, wait);
          event && event.stopImmediatePropagation();
        }


        if (isClick) {
          isClick = false;
          //事件
          //定时器
          setTimeout(function () {
            isClick = true;
          }, wait);//3秒内不能重复点击
          //执行代码块

        } else {
          Message.error("请勿连续点击！")
        }
      },
      true
    );
  },
};
