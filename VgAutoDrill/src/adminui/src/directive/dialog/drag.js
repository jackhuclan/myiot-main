/**
* v-dialogDrag 弹窗拖拽
* Copyright (c) 2019 ruoyi
*/
export default {
  bind(el, binding, vnode, oldVnode) {
    const dialogHeaderEl = el.querySelector('.el-dialog__header');
    const dragDom = el.querySelector('.el-dialog');
    // dialogHeaderEl.style.cursor = 'move';
    dialogHeaderEl.style.cssText += ';cursor:move;';
    dragDom.style.cssText += ';top:0px;';

    // 获取原有属性 ie dom元素.currentStyle 火狐谷歌 window.getComputedStyle(dom元素, null);
    const sty = (function () {
      if (window.document.currentStyle) {
        return (dom, attr) => dom.currentStyle[attr];
      } else {
        return (dom, attr) => getComputedStyle(dom, false)[attr];
      }
    })();
    // dialog点击事件如果是弹出选择框提示 
    el.onclick = function (event) {
      if (el.style.cursor == '' && event.target.className == 'el-dialog__wrapper' && vnode.context.showFlag) {
        vnode.context.showFlag = false;
        // vnode.context.$notify({
        //   title: '提示',
        //   type: 'error',
        //   message: '点击x号或取消进行关闭!',
        // })
      } else {
        event.stopPropagation()
      }
    }
    dialogHeaderEl.onmousedown = e => {
      // 如果鼠标按下时是在弹框关闭按钮上直接终止
      if (e.target.classList.contains("el-dialog__close")) {
        document.onmousemove = null;
        document.onmouseup = null;
        return;
      }
      // 鼠标按下，计算当前元素距离可视区的距离
      const disX = e.clientX - dialogHeaderEl.offsetLeft;
      const disY = e.clientY - dialogHeaderEl.offsetTop;
      const screenWidth = document.body.clientWidth; // body当前宽度
      const screenHeight = document.documentElement.clientHeight; // 可见区域高度(应为body高度，可某些环境下无法获取)
      const dragDomWidth = dragDom.offsetWidth; // 对话框宽度
      const dragDomheight = dragDom.offsetHeight; // 对话框高度
      const minDragDomLeft = dragDom.offsetLeft;
      const maxDragDomLeft = screenWidth - dragDom.offsetLeft - dragDomWidth;
      const minDragDomTop = dragDom.offsetTop;
      const maxDragDomTop = screenHeight - dragDom.offsetTop - dragDomheight;
      // 获取到的值带px 正则匹配替换
      let styL = sty(dragDom, 'left');
      let styT = sty(dragDom, 'top');
      // 注意在ie中 第一次获取到的值为组件自带50% 移动之后赋值为px
      if (styL.includes('%')) {
        // eslint-disable-next-line no-useless-escape
        styL = +document.body.clientWidth * (+styL.replace(/\%/g, '') / 100);
        // eslint-disable-next-line no-useless-escape
        styT = +document.body.clientHeight * (+styT.replace(/\%/g, '') / 100);
      } else {
        styL = +styL.replace(/\px/g, '');
        styT = +styT.replace(/\px/g, '');
      }

      document.onmousemove = function (e) {
        // 防止文字选中
        window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();
        // 通过事件委托，计算移动的距离
        let left = e.clientX - disX;
        let top = e.clientY - disY;

        // 边界处理
        if (-left > minDragDomLeft) {
          left = -minDragDomLeft;
        } else if (left > maxDragDomLeft) {
          left = maxDragDomLeft;
        }
        // 由于表单高度超出屏幕故没有向下拖动最大限制
        if (-top > minDragDomTop) {
          top = -minDragDomTop;
        }
        // } else if (top > maxDragDomTop) {
        //   top = maxDragDomTop;
        // }

        // 移动当前元素
        dragDom.style.cssText += `;left:${left + styL}px;top:${top + styT}px;`;
      };

      document.onmouseup = function (e) {
        document.onmousemove = null;
        document.onmouseup = null;
      };
      return false;
    };
  },

  update(el, binding, vnode, old) {
    // 实现每次打开 Dialog 都复位到中央
    setTimeout(() => {
      if (el.style.display === 'none') {
        el.getElementsByClassName('el-dialog')[0].style.top = 0;
        el.getElementsByClassName('el-dialog')[0].style.left = 0;
      }
    }, 333); // Dialog 的关闭动画用时 300 毫秒，因此这里需要延迟执行，而且要多延迟一点点

  },
}
