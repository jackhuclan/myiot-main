export default {
  inserted(el, binding, vnode) {
    const that = vnode.context;
    let clickObj = {
      firstTime: "", // mousedown的时间戳
      lastTime: "", // mouseup的时间戳
      selectionTxt: "", // 选中的文本 
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
          that.isClick = true;
        } else { 
          that.isClick = false;
        }
      };
    };
    el.addEventListener("click", function (event) {
      if (! that.isClick) return;
      clickObj.firstTime = "";
      clickObj.lastTime = ""; 
      that.isClick = false;
    });
  },
};
