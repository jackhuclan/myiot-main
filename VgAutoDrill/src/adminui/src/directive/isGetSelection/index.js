import { hasPermi } from "@/utils/public";
export default {
  inserted(el, binding, vnode) {
    // 查看按钮权限
    if (binding.value && hasPermi(binding.value)) {
      el.style.color = "#606266";
      // 禁止点击事件
      el.style.pointerEvents = "none";
    }
    const that = vnode.context;
    let clickObj = {
      firstTime: "", // mousedown的时间戳
      lastTime: "", // mouseup的时间戳
      selectionTxt: "", // 选中的文本
      isClick: false, // false--禁止点击，true--可点击
    };
    el.onmousedown = function (e) {
      clickObj.firstTime = new Date().getTime();
      document.onmouseup = function (e) {
        clickObj.lastTime = new Date().getTime();
        // 获取选中文本
        clickObj.selectionTxt = window.getSelection().toString();

        // 鼠标点击和抬起的时间差<200，或者选中文本为空时，可以触发点击
        if (
          clickObj.lastTime - clickObj.firstTime < 200 ||
          clickObj.selectionTxt.trim() == ""
        ) {
          clickObj.isClick = true;
        } else {
          clickObj.isClick = false;
        }
      };
    };
    el.addEventListener("click", function (event) {
      if (!clickObj.isClick) return;
      clickObj.firstTime = "";
      clickObj.lastTime = "";
      clickObj.isClick = false;
      // // 调用组件内写好的handleView方法
      const id = el.getAttribute("data-id");
      that.handleView(id);
    });
  },
};
