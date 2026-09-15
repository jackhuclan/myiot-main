/**
 * v-dialogDrag 盒子拖拽
 */
import store from "@/store";
export default {
  bind(el) {
    // 拖拽时间标识
    let firstTime = "";
    let lastTime = "";
    let isDrag = false;

    el.onmousedown = function (e) {
      // 为了区分点击还是拖拽，使用时间差来判断，200毫秒内为点击，200毫秒外为拖拽，初始化为点击
      el.setAttribute("drag-flag", false);
      firstTime = new Date().getTime();
      isDrag = true;
      let posX = e.clientX - el.offsetLeft;
      let posY = e.clientY - el.offsetTop;
      document.onmousemove = (e) => {
        // 防止文字选中
        window.getSelection
          ? window.getSelection().removeAllRanges()
          : document.selection.empty();
        //计算元素位置(需要判断临界值)
        let left = e.clientX - posX;
        let top = e.clientY - posY;
        if (isDrag) {
          // 获取父元素
          // 获取左侧菜单宽度
          const siderBar = document.querySelector(".sidebar-container");
          const screenWidth = document.body.clientWidth; // body当前宽度
          const screenHeight = document.documentElement.clientHeight; // 可见区域高度(应为body高度，可某些环境下无法获取)
          let { offsetHeight: sonNodeHeight, offsetWidth: sonNodeWidth } = el;
          // 左上角(left)
          if (store.state.app.device == "mobile") {
            if (left < 0) {
              left = 0;
            }
          } else {
            if (left < siderBar.offsetWidth) {
              left = siderBar.offsetWidth;
            }
          }

          if (top < 0) {
            top = 0;
          }
          // 左下角
          if (top > screenHeight - sonNodeHeight) {
            top = screenHeight - sonNodeHeight;
          }
          if (left > screenWidth - sonNodeWidth) {
            left = screenWidth - sonNodeWidth;
          }
          //移动当前元素
          el.style.left = left + "px";
          el.style.top = top + "px";
          // 判断下当前时间与初始时间差，大于200毫秒则判断状态为拖拽
          lastTime = new Date().getTime();
          if (lastTime - firstTime > 200) {
            el.setAttribute("drag-flag", true);
          }
        }
      };
      document.onmouseup = (e) => {
        isDrag = false;
        document.onmousemove = null;
        document.onmouseup = null;
      };
    };
  },
};
